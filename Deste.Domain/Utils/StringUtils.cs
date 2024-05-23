namespace Deste.Domain.Utils;

public static class StringUtils
{
    public static bool IsBase64String(this string base64)
    {
        var buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out var bytesParsed);
    }
}