using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeAPI.Api;

public class UserDataService : IUserDataService
{
    readonly UserName SystemUser = new("system");
    readonly IdString UserTableName = new("user_table");

    private readonly IDataContainer _dataContainer;
    private readonly IAppDataService _appDataService;
    public UserDataService(IAppDataService appDataService, IDataContainer dataContainer)
    {
        _appDataService = appDataService;
        _dataContainer = dataContainer;
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        var userTable = await _dataContainer.Get<UserTable>(UserTableName, SystemUser);
        return userTable.Users;
    }

    public async Task AddUser(UserName userName)
    {
        var userTable = await _dataContainer.Get<UserTable>(UserTableName, SystemUser);
        userTable.Users.Add(new User
        {
            Name = userName
        });

        await _dataContainer.Upsert(userTable);
    }

    public async Task DeleteUser(UserName userName)
    {
        var userTable = await _dataContainer.Get<UserTable>(UserTableName, SystemUser);
        var user = userTable.Users.SingleOrDefault(x => x.Name == userName);
        if(user is null)
        {
            return;
        }

        userTable.Users.Remove(user);
        await _dataContainer.Upsert(userTable);

        var userApps = await _appDataService.GetUserApps(userName);
        foreach(var app in userApps)
        {
            await _appDataService.Delete(userName, app.Id);
        }
    }
}