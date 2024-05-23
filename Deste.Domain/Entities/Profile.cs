
using System.Text.Json.Serialization;

namespace Deste.Domain.Entities;

public class Profile : Entity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? UserId { get; set; }
    [JsonIgnore] public IdentityUser? User { get; set; }
}