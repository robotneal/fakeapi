using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FakeAPI.Api;

public static class EndpointsMapper
{
    public static string RoutePath => "apps/{user}/{name}/{*tests}";

    public static RequestDelegate Request => async (context) =>
    {
        IAppDataService dataContainer = context.RequestServices.GetService<IAppDataService>()!;

        var userName = (UserName)context.Request.RouteValues["user"]!;
        var appName = (AppName)context.Request.RouteValues["name"]!;

        var app = await dataContainer.GetApp(userName, appName);

        if( app.RequireUserPassword == true && 
            await SecretHeader.Check(context, userName) == false)
        {
            context.Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorised");
            return;
        }
        
        var tests = (string)context.Request.RouteValues["tests"]!;
        var mapping = app.Mappings
            .Where(x => x.Route.ToLower() == tests.ToLower() &&
                    x.Method.ToLower() == context.Request.Method.ToLower())
            .Single();

        context.Response.StatusCode = mapping.ResponseCode;
        await context.Response.WriteAsync(mapping.ResponseBody);
    };
}