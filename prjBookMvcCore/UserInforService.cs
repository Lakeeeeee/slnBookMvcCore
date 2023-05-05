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
            return Convert.ToInt32(varCliams.Where(x => x.Type == "Id").FirstOrDefault()?.Value);
        }
    }
    public int UserMessageCount
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            return Convert.ToInt32(varCliams.Where(x => x.Type == "MessageCount").First().Value);
        }
    }
    public int UserOrders
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            return Convert.ToInt32(varCliams.Where(x => x.Type == "MessageCount").First().Value);
        }
    }

    public int UserPoints
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
            return Convert.ToInt32(varCliams.Where(x => x.Type == "Points").First().Value);
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