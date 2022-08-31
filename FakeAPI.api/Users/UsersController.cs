using System;
using System.Threading.Tasks;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI.Api;

[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserDataService _dataContainer;
    private readonly IAppDataService _appDataService;
    private readonly SecretClient _secretClient;

    public UsersController(
        IUserDataService dataContainer,
        IAppDataService appDataService,
        SecretClient secretClient)
    {
        _dataContainer = dataContainer;
        _appDataService = appDataService;
        _secretClient = secretClient;
    }

    [HttpGet]
    [Route("users")]
    [SystemAuthorise]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _dataContainer.GetUsers());
    }

    [HttpGet]
    [Route("users/{userName}")]
    [SecretHeader("userName")]
    public async Task<IActionResult> GetUser([FromRoute] UserName userName)
    {
        return Ok(await _appDataService.GetUserApps(userName));
    }

    [HttpPut]
    [Route("users/{userName}")]
    [SystemAuthorise]
    public async Task<IActionResult> AddUser([FromRoute] UserName userName)
    {
        await _dataContainer.AddUser(userName);
        var secret = Guid.NewGuid();
        await _secretClient.SetSecretAsync(new KeyVaultSecret($"apimock-{userName}", $"{secret}"));

        return Ok(secret);
    }

    [HttpDelete]
    [Route("users/{userName}")]
    [SystemAuthorise]
    public async Task<IActionResult> DeleteUser([FromRoute] UserName userName)
    {
        await _dataContainer.DeleteUser(userName);
        await _secretClient.StartDeleteSecretAsync($"apimock-{userName}");

        return NoContent();
    }
}