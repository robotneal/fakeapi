using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FakeAPI.Api;

[ApiController]
public class AppsController : ControllerBase
{
    private readonly IAppDataService _appDataService;
    public AppsController(IAppDataService appDataService)
    {
        _appDataService = appDataService;
    }

    [HttpPut]
    [Route("apps/{userName}/{appName}")]
    [SecretHeader("userName")]
    public async Task<IActionResult> UploadApp(
        [FromRoute] UserName userName,
        [FromRoute] AppName appName,
        [FromBody] MappedAppModel? appData)
    {
        if(appData is null)
        {
            return BadRequest();
        }

        var app = await _appDataService.InsertApp(userName, appName, appData!);
        return Created("apps/{userName}/{appName}", app);
    }

    [HttpGet]
    [Route("apps/{userName}/{appName}")]
    [SecretHeader("userName")]
    public async Task<IActionResult> GetApp([FromRoute] UserName userName, [FromRoute] AppName appName)
    {
        var app = await _appDataService.GetApp(userName, appName);
        return Ok(app);
    }
}