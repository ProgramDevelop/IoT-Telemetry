using System.Linq;
using Telemetry.Database.Base;
using Telemetry.Database.Models;

namespace Telemetry.Database.Storages
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TelemetryContext context) : base(context)
        {
        }

        public User GetUserByEmail(string email)
        {
            email = email.Trim().ToUpper();
            var user = GetAll().FirstOrDefault(u => u.Email == email);
            return user;
        }
    }
}
