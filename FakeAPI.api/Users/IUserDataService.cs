using System.Collections.Generic;
using System.Threading.Tasks;

namespace FakeAPI.Api
{
    public interface IUserDataService
    {
        Task<IEnumerable<User>> GetUsers();
        Task AddUser(UserName userName);
        Task DeleteUser(UserName userName);
    }
}