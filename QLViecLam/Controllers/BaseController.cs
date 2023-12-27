using Microsoft.AspNetCore.Mvc;

namespace QLViecLam.Controllers
{
    public class BaseController : Controller
    {
        public string CurrentUser
        {
            get
            {
                return HttpContext.Session.GetString("USER_NAME");
            }
            set
            {
                HttpContext.Session.SetString("USER_NAME", value);
            }
        }
        public bool IsLogin
        {
            get
            {
                return !string.IsNullOrEmpty(CurrentUser);
            }
        }
    }
}
