using FindMyValentine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;


namespace FindMyValentine.Attributes
{
    public class StudentAuthorizeOnly:ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionContext = context.HttpContext;

            try
            {
                bool isAuthorized = false;

                if (actionContext.Request.Cookies.Keys.Contains("FMVSession"))
                {
                    string currentUserCookie = actionContext.Request.Cookies["FMVSession"] ?? "";
                    var translatedKeys = JsonConvert.DeserializeObject<Account>(currentUserCookie);
                    Controller controller = context.Controller as Controller;
                    controller.ViewData["User"] = translatedKeys;
                    isAuthorized = true;
                }

                if (!isAuthorized)
                {
                    context.Result = new RedirectResult("~/Users/Login");
                }

                //base.OnAuthorization(actionContext);
            }
            catch (Exception ex)
            {
                context.Result = new RedirectResult("~/Home/Error");
            }
        }

    }

    public class AdminAuthorizeOnly : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionContext = context.HttpContext;

            try
            {
                bool isAuthorized = false;

                if (actionContext.Request.Cookies.Keys.Contains("FMVSessionAdmin"))
                {
                    string currentUserCookie = actionContext.Request.Cookies["FMVSessionAdmin"] ?? "";
                    var translatedKeys = JsonConvert.DeserializeObject<Account>(currentUserCookie);
                    Controller controller = context.Controller as Controller;
                    controller.ViewData["Admin"] = translatedKeys;
                    isAuthorized = true;
                }

                if (!isAuthorized)
                {
                    context.Result = new RedirectResult("~/Users/AdminLogin");
                }

                //base.OnAuthorization(actionContext);
            }
            catch (Exception ex)
            {
                context.Result = new RedirectResult("~/Home/Error");
            }
        }

    }
}
