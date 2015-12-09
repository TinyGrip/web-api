using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using OutdoorSolution.Controllers;
using Microsoft.AspNet.Identity;

namespace OutdoorSolution.Filters
{
    /// <summary>
    /// Calls a method to initialize user id before action is executed.
    /// This filter is created in order to automize initialization of UserId on every action
    /// </summary>
    public class InitUserIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var userResourceController = actionContext.ControllerContext.Controller as UserResourceController;
            if (userResourceController != null)
            {
                userResourceController.InitUser(userResourceController.User.Identity.GetUserId());
            }
        }
    }
}