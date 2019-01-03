using Telemetry.Database.Models;

namespace Telemetry.Database
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUserByEmail(string emeil);
    }
}
