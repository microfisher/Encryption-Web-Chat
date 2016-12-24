using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Wesley.Component.WebChat.Filters
{
    public class ActionRequestFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["Ticks"] = DateTime.Now.Ticks;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            var ticks = context.HttpContext.Items["Ticks"];
            if (ticks != null)
            {
                var timeSpan = new TimeSpan(DateTime.Now.Ticks - Convert.ToInt64(ticks));
                context.HttpContext.Response.Redirect("/Home/Index/"+ timeSpan);
            }
        }

    }
}
