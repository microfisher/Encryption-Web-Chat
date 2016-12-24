using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Wesley.Component.WebChat.Filters
{
    public class ActionAuthorizeFilter:AuthorizeFilter
    {
        public ActionAuthorizeFilter(AuthorizationPolicy policy): base(policy){}

        public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymous && item != this))
            {
                return Task.FromResult(0);
            }

            return base.OnAuthorizationAsync(context);
        }
    }
}
