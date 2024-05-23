using Deste.Domain.Entities;

namespace Deste.Domain.Models;

public class LoginModel
{
    public string Token { get; set; }
    public Profile Profile { get; set; }
    public IList<string> Roles { get; set; }
}