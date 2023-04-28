using prjBookMvcCore.Models;

public class UserInforService
{
    private readonly IHttpContextAccessor _contextAccessor;

    public UserInforService (IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public int getUserInfor()
    {
        var varCliams = _contextAccessor.HttpContext.User.Claims.ToList();
        var varId = varCliams.Where(x=>x.Type == "Id").First().Value;
        int userId = Convert.ToInt32(varId);
        return userId;
    }
}