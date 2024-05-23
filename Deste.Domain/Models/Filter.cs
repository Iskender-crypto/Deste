using System.Text.Json;

namespace Deste.Domain.Models;

public class Filter
{
    public string Name { get; set; }
    public JsonElement Value { get; set; }
}