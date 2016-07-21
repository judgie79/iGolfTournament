using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Golf.Tournament.Models
{
    public class ErrorHandler : HandleErrorAttribute
    {
        public string Message { get; set; }

        public string StackTrace { get; set; }

        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            filterContext.ExceptionHandled = true;
            var model = new HandleErrorInfo(filterContext.Exception, filterContext.Controller.GetType().ToString(), filterContext.RequestContext.RouteData.Route.ToString());

            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                ViewData = new ViewDataDictionary(model)
            };
        }
    }
}