using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Deste.Domain.Entities;
using Deste.Domain.Models;
using Deste.Domain.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Deste.Infrastructure.Ef.Services;

public class AuthService(UserManager<IdentityUser> userManager, DataContext dataContext) : IAuthService
{
    public async Task<LoginModel> Login(AuthModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Login);
        if (user == null) throw new Exception("User or Password is not valid");
        var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);
        if (!isPasswordValid) throw new Exception("User or Password is not valid");
        var roles = await userManager.GetRolesAsync(user);
        var token = await GetToken(user, roles);
        var profile = await dataContext.Set<Profile>().FirstOrDefaultAsync(p => p.UserId == user.Id);
        return new LoginModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token), Profile = profile,
            Roles = roles
        };
    }

    private async Task<JwtSecurityToken> GetToken(IdentityUser user, IEnumerable<string> roles)
    {
        var authSigningKey = new SymmetricSecurityKey("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"u8.ToArray());
        var securityStamp = await userManager.GetSecurityStampAsync(user);
        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), new(ClaimTypes.UserData, securityStamp)
        };
        authClaims.AddRange(roles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
        var token = new JwtSecurityToken(
            issuer: "issuer", audience: "audience",
            expires: DateTime.Now.AddHours(3), claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
        return token;
    }

    public async Task<LoginModel> Register(RegisterModel model)
    {
        var user = await userManager.FindByEmailAsync(model.Login);
        if (user != null) throw new Exception("User already exist");
        user = new IdentityUser
        {
            UserName = model.Login,
            Email = model.Login
        };
        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Errors.Any()) throw new Exception("Error");
        model.Profile.UserId = user.Id;
        dataContext.Set<Profile>().Add(model.Profile);
        await dataContext.SaveChangesAsync();
        var token = await GetToken(user, []);
        return new LoginModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token), Profile = model.Profile,
            Roles = new List<string>()
        };
    }
}