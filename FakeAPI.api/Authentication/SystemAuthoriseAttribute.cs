using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FakeAPI.Api;

[AttributeUsage(AttributeTargets.Method)]
public class SystemAuthoriseAttribute : Attribute, IAsyncAuthorizationFilter
{
    public SystemAuthoriseAttribute()
    {
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var isAuthorised = await SecretHeader.Check(context.HttpContext, new("system"));
        if(isAuthorised == false)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}