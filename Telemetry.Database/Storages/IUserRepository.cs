using Telemetry.Database.Base;
using Telemetry.Database.Models;

namespace Telemetry.Database.Storages
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByEmail(string email);
    }
}
