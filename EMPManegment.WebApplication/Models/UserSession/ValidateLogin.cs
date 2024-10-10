using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EMPManegment.Web.Models.UserSession
{
    public class ValidateLogin : ActionFilterAttribute 
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var currentId = (filterContext.HttpContext.User.Identity as ClaimsIdentity);
            if (currentId.Claims.Count() <= 0)
            {
                if (IsAjaxRequest(filterContext.HttpContext.Request))
                {
                    filterContext.Result = new UnauthorizedResult();
                }
                else
                {
                    filterContext.HttpContext.Session.Clear();
                    filterContext.Result = new RedirectResult("~/Home/Index?q=st");
                    
                }
              
            }
            base.OnActionExecuting(filterContext);
        }
        public bool IsAjaxRequest(HttpRequest request)
        {
            if (request.Headers != null)
                return request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
