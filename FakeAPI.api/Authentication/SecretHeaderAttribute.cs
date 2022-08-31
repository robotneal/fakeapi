using System;
using System.Linq;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FakeAPI.Api;

[AttributeUsage(AttributeTargets.Method)]
public class SecretHeaderAttribute : Attribute, IAsyncAuthorizationFilter
{
    private readonly string _userNameToken;
    public SecretHeaderAttribute(string userNameToken)
    {
        _userNameToken = userNameToken;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userName = (UserName)context.RouteData.Values[_userNameToken]!;
        var isAuthorised = await SecretHeader.Check(context.HttpContext, userName);
        if(isAuthorised == false)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}