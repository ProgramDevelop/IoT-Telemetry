using System;
using System.Collections.Generic;
using System.Text;
using Telemetry.Web.Services.SensorManager;
using Xunit;
using Moq;
using Telemetry.Database.Storages;
using Telemetry.Database.Models;
using System.Linq;

namespace Telemetry.Tests
{
    public class SensorsManagerTests
    {
        private const string USER_ONE_ID = "00000000-1111-0000-0000-000000000000";
        private const string USER_TWO_ID = "00000000-0000-2222-0000-000000000000";

        [Fact]
        public void ReturnSensorOnCreate()
        {
            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.Create(It.IsAny<Sensor>())).Returns(true);
            var _sensorManager = new SensorsManager(sensorRepo.Object, null, null);

            var userId = Guid.NewGuid();
            var name = "    TemperatureSensor    ";
            var description = "The sensor of storing temperature and humidity";

            var actualSensor = _sensorManager.CreateSensor(userId, name, description);

            Assert.NotNull(actualSensor);
            Assert.Equal(userId, actualSensor.UserId);
            Assert.Equal("TemperatureSensor", actualSensor.Name);
            Assert.NotNull(actualSensor.Description);
            Assert.Equal(description, actualSensor.Description);
        }

        [Fact]
        public void ReturnUserSensors()
        {
            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.GetAll()).Returns(GetSensors);
            var _sensorManager = new SensorsManager(sensorRepo.Object, null, null);

            var userId = Guid.Parse(USER_ONE_ID);

            var expectedSensors = GetSensors().Where(s => s.UserId == userId);

            var userSensors = _sensorManager.GetSensorsForUser(userId);

            Assert.Equal(expectedSensors.Count(), userSensors.Count());
            Assert.True(userSensors.All(s => s.UserId == userId));
        }

        private IQueryable<Sensor> GetSensors() => new Sensor[]
        {
            new Sensor
{
                Id = Guid.Parse("B031F5CE-0C29-4952-A2C9-FF788188AB24"),
                Name = "Name1",
                Description = "Desc 1",
                UserId = Guid.Parse(USER_ONE_ID)
            },
            new Sensor
            { 
                Id = Guid.Parse("5FFBE49C-8089-4015-A8C7-5A94CD113B68"),
                Name = "Name2",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID)
            },
            new Sensor
            { 
                Id = Guid.Parse("28D7652C-5A90-4EDF-857C-A859BDDCCB62"),
                Name = "Name3",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID)
            },
            new Sensor
            { 
                Id = Guid.Parse("010975A1-9CFA-4CE8-B6FA-42996B59EE56"),
                Name = "Name4",
                Description = "Some desc",
                UserId = Guid.Parse(USER_TWO_ID)
            },
            new Sensor
            {
                Id = Guid.Parse("DF7E1861-4874-4987-A5A5-7F3616FE500F"),
                Name = "Name5",
                Description = null,
                UserId = Guid.Parse(USER_TWO_ID)
            },
        }.AsQueryable();
    }
}
