using Microsoft.CodeAnalysis.CSharp.Syntax;
using prjBookMvcCore.Models;
using System.Security.Claims;

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
            var varCliams = _contextAccessor.HttpContext!.User.Claims.ToList();
            return Convert.ToInt32(varCliams.Where(x => x.Type == "Id").FirstOrDefault()?.Value);
        }
    }
    public string UserName
    {
        get
        {
            var varCliams = _contextAccessor.HttpContext!.User.Claims.ToList();
            return varCliams.Where(x => x.Type == ClaimTypes.Name).FirstOrDefault()!.Value;
        }
    }

}