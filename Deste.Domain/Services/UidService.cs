using Microsoft.AspNetCore.Http;

namespace Deste.Domain.Services;

public class UidService(IHttpContextAccessor httpContextAccessor)   
{
    public Guid GetUid()
    {
        var context = httpContextAccessor.HttpContext;
        if (context == null) throw new NullReferenceException("HttpContext не доступен");
        
        
        var uidCookie = context.Request.Cookies["_uid"];
        
        
        if (uidCookie != null)
        {
            if (Guid.TryParse(uidCookie, out var tryResult))
            {
                return tryResult;
            }
        }
        
        var uid = Guid.NewGuid();
        context.Response.Cookies.Append("_uid", uid.ToString(), new CookieOptions
        {
            Expires = DateTime.Now.AddYears(1)
        });
        return uid;
    }
}