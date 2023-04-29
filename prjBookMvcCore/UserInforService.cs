using Microsoft.CodeAnalysis.CSharp.Syntax;
using prjBookMvcCore.Models;

public class UserInforService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserInforService (IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public int UserId
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            var varId = varCliams.Where(x => x.Type == "Id").First().Value;
            return Convert.ToInt32(varId);
        }
    }
    public int UserMessageCount
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            var varUserMessageCount = varCliams.Where(x => x.Type == "MessageCount").First().Value;
            return Convert.ToInt32(varUserMessageCount);
        }
    }
    public int UserPoints
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            var varUserPoints  = varCliams.Where(x => x.Type == "Points").First().Value;
            return Convert.ToInt32(varUserPoints);
        }
    }
    public string UserLevel
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            return varCliams.Where(x => x.Type == "Level").First().Value;
        }
    }
}