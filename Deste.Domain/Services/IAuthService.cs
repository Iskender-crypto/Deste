using Deste.Domain.Models;

namespace Deste.Domain.Services;

public interface IAuthService
{
    public Task<LoginModel> Login(AuthModel model);
    public Task<LoginModel> Register(RegisterModel model);
}