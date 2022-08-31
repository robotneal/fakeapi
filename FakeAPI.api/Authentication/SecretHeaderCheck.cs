using System.Linq;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Http;

namespace FakeAPI.Api;

public static class SecretHeader
{
    public static async Task<bool> Check(HttpContext context, UserName userName)
    {
        if (context.RequestServices.GetService(typeof(SecretClient)) is not SecretClient client || 
            context.Request.Headers["x-fakeapi-secret"].FirstOrDefault() is not string secretHeader)
        {
            return false;
        }

        var secret = await client.GetSecretAsync($"fakeapi-{userName}");
        return secret.Value.Value == secretHeader;
    }
}