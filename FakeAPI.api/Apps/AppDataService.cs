using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeAPI.Api;

public class AppDataService : IAppDataService
{
    private readonly IDataContainer _dataContainer;
    public AppDataService(IDataContainer dataContainer)
    {
        _dataContainer = dataContainer;
    }

    public async Task<MappedApp> GetApp(UserName userName, AppName appName) =>
        await _dataContainer.Get<MappedApp>(appName, userName);

    public async Task<MappedApp> InsertApp(UserName userName, AppName appName, MappedAppModel appData) =>
        await _dataContainer.Upsert(new MappedApp
        {
            Id = appName,
            UserName = userName,
            Mappings = appData.Mappings!,
            RequireUserPassword = appData.RequireUserPassword!.Value
        });
    

    public async Task<IEnumerable<MappedApp>> GetUserApps(UserName userName) => 
        await _dataContainer.GetAll<MappedApp>(userName);

    public async Task Delete(UserName userName, AppName appName) => 
        await _dataContainer.Delete<MappedApp>(appName, userName);
}