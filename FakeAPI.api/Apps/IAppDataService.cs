using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeAPI.Api;

public interface IAppDataService
{
    Task<MappedApp> GetApp(UserName userName, AppName appName);
    Task<MappedApp> InsertApp(UserName userName, AppName appName, MappedAppModel appData);
    Task<IEnumerable<MappedApp>> GetUserApps(UserName userName);
    Task Delete(UserName userName, AppName appName);
}