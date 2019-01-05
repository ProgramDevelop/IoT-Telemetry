using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Telemetry.Database.Models;
using Telemetry.Database.Storages;

namespace Telemetry.Web.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool CheckCredentials(string login, string password)
        {
            login = login.Trim().ToUpper();
            password = password.Trim();
            var hash = GetHash(password);
            var user = _userRepository.GetAll().FirstOrDefault(u => u.Email == login && u.Password == hash);
            return user != null;
        }

        public bool IsUserExist(string login)
        {
            var user = _userRepository.GetUserByEmail(login);
            return user != null;
        }

        public bool Register(string login, string password)
        {
            if (IsUserExist(login))
                return false;
            login = login.Trim().ToUpper();
            password = password.Trim();
            var hash = GetHash(password);
            var user = new User { Email = login, Password = hash };
            return _userRepository.CreateAsync(user).GetAwaiter().GetResult();
        }

        private string GetHash(string text)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToUpper();
            }
        }
    }
}
