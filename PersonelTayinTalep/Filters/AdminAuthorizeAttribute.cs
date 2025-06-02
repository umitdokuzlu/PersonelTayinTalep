using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PersonelTayinTalep.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var personelId = session.GetInt32("PersonelId");
            var isAdmin = session.GetString("IsAdmin");

            if (!personelId.HasValue)
            {
                context.Result = new RedirectToActionResult("Login", "Auth", null);
                return;
            }

            if (isAdmin != "true")
            {
                context.Result = new RedirectToActionResult("Index", "Personel", null);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
