using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Telemetry.Database.Models;
using Telemetry.Database.Storages;
using Telemetry.Web.Services.Auth;
using Xunit;

namespace Telemetry.Tests
{
    public class AuthServiceTests
    {
        private const string USER_ONE_ID = "00000000-1111-0000-0000-000000000000";
        private const string USER_TWO_ID = "00000000-0000-2222-0000-000000000000";

        [Fact]
        public void FindUser_UserExist()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            foreach (var user in users)
            {
                userRepository.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            }
            IAuthService authService = new AuthService(userRepository.Object);
            var result = authService.IsUserExist(users.First()?.Email);
            Assert.True(result);
        }

        [Fact]
        public void FindUser_UserNotExist()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            foreach (var user in users)
            {
                userRepository.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            }
            IAuthService authService = new AuthService(userRepository.Object);
            var result = authService.IsUserExist("USER3@DOMAIN.COM");
            Assert.False(result);
        }

        [Fact]
        public void Auth_CorrectCredantials()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            userRepository.Setup(r => r.GetAll()).Returns(GetUsers);

            IAuthService authService = new AuthService(userRepository.Object);

            var result = authService.CheckCredentials("user1@domain.COM", "12345678");
            Assert.True(result);
        }

        [Fact]
        public void Auth_InCorrectEmail()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            userRepository.Setup(r => r.GetAll()).Returns(GetUsers);

            IAuthService authService = new AuthService(userRepository.Object);

            var result = authService.CheckCredentials("user3@domain.COM", "12345678");
            Assert.False(result);
        }

        [Fact]
        public void Auth_InCorrectPassword()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            userRepository.Setup(r => r.GetAll()).Returns(GetUsers);

            IAuthService authService = new AuthService(userRepository.Object);

            var result = authService.CheckCredentials("user1@domain.COM", "12345678_fake");
            Assert.False(result);
        }

        [Fact]
        public void Registration_CreateDuplicateEmailUser()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            foreach (var user in users)
            {
                userRepository.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            }
            userRepository.Setup(r => r.Create(It.IsAny<User>())).Returns(true);

            IAuthService authService = new AuthService(userRepository.Object);

            var result = authService.Register("user1@domain.com", "UserVeryStrongP@ssword");
            Assert.False(result);
        }

        [Fact]
        public void Registration_CreateUser()
        {
            var userRepository = new Mock<IUserRepository>();
            var users = GetUsers();
            foreach (var user in users)
            {
                userRepository.Setup(r => r.GetUserByEmail(user.Email)).Returns(user);
            }
            userRepository.Setup(r => r.Create(It.IsAny<User>())).Returns(true);

            IAuthService authService = new AuthService(userRepository.Object);

            var result = authService.Register("user1@domain.com", "VeryStrongP@ssword");
            Assert.False(result);
        }

        private IQueryable<User> GetUsers() => new User[]
       {
            new User
            {
                Id = Guid.Parse(USER_ONE_ID),
                Email = "USER1@DOMAIN.COM",
                Password = GetHash("12345678")
            },
            new User
            {
                Id = Guid.Parse(USER_TWO_ID),
                Email = "USER2@DOMAIN.COM",
                Password = GetHash("abcdef")
            }
        }.AsQueryable();

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
