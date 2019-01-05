using System;
using System.Collections.Generic;
using System.Text;
using Telemetry.Web.Services.SensorManager;
using Xunit;
using Moq;
using Telemetry.Database.Storages;
using Telemetry.Database.Models;
using System.Linq;
using Telemetry.Base;
using ValueType = Telemetry.Database.Models.ValueType;

namespace Telemetry.Tests
{
    public class SensorsManagerTests
    {
        private const string USER_ONE_ID = "00000000-1111-0000-0000-000000000000";
        private const string USER_TWO_ID = "00000000-0000-2222-0000-000000000000";

        private const string SENSOR_ONE_ID = "B031F5CE-0C29-4952-A2C9-FF788188AB24";
        private const string SENSOR_TWO_ID = "5FFBE49C-8089-4015-A8C7-5A94CD113B68";
        private const string SENSOR_THREE_ID = "28D7652C-5A90-4EDF-857C-A859BDDCCB62";
        private const string SENSOR_FOUR_ID = "010975A1-9CFA-4CE8-B6FA-42996B59EE56";
        private const string SENSOR_FIVE_ID = "DF7E1861-4874-4987-A5A5-7F3616FE500F";

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
                Id = Guid.Parse(SENSOR_ONE_ID),
                Name = "Name1",
                Description = "Desc 1",
                UserId = Guid.Parse(USER_ONE_ID),
                Values = GetSensorsValueTypes()
                    .Where(v => v.SensorId == Guid.Parse(SENSOR_ONE_ID)).ToList()
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_TWO_ID),
                Name = "Name2",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID),
                Values = GetSensorsValueTypes()
                    .Where(v => v.SensorId == Guid.Parse(SENSOR_TWO_ID)).ToList()
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_THREE_ID),
                Name = "Name3",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID),
                Values = GetSensorsValueTypes()
                    .Where(v => v.SensorId == Guid.Parse(SENSOR_THREE_ID)).ToList()
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_FOUR_ID),
                Name = "Name4",
                Description = "Some desc",
                UserId = Guid.Parse(USER_TWO_ID),
                Values = GetSensorsValueTypes()
                    .Where(v => v.SensorId == Guid.Parse(SENSOR_FOUR_ID)).ToList()
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_FIVE_ID),
                Name = "Name5",
                Description = null,
                UserId = Guid.Parse(USER_TWO_ID),
                Values = GetSensorsValueTypes()
                    .Where(v => v.SensorId == Guid.Parse(SENSOR_FIVE_ID)).ToList()
            },
        }.AsQueryable();

        private IQueryable<ValueType> GetSensorsValueTypes() => new ValueType[]
        {
            new ValueType
            {
                Id = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
                Name = "NameValueType1",
                SensorId = Guid.Parse(SENSOR_ONE_ID),
                Type = PayloadType.Number,
                Values = GetValues().Where(v => v.ValueTypeId == Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC")).ToList()
            },
            new ValueType
            {
                Id = Guid.Parse("89740433-0A86-463A-9430-9A570D145B51"),
                Name = "NameValueType2",
                SensorId = Guid.Parse(SENSOR_ONE_ID),
                Type = PayloadType.Number,
                Values = GetValues().Where(v => v.ValueTypeId == Guid.Parse("89740433-0A86-463A-9430-9A570D145B51")).ToList()
            },
            new ValueType
            {
                Id = Guid.Parse("155C4D30-E739-4324-9D61-EFF015C5A125"),
                Name = "NameValueType3",
                SensorId = Guid.Parse(SENSOR_TWO_ID),
                Type = PayloadType.Number,
                Values = GetValues().Where(v => v.ValueTypeId == Guid.Parse("155C4D30-E739-4324-9D61-EFF015C5A125")).ToList()
            }
        }.AsQueryable();

        private IQueryable<Value> GetValues() => new Value[]
        {
            new Value
            {
                Id = Guid.Parse("E618A42B-0DBB-4099-ADF8-3926C62A5EB8"),
                Data = "Data1",
                DateTime = DateTime.Now,
                ValueTypeId = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
            },
            new Value
            {
                Id = Guid.Parse("78D6D7AA-A9B7-4D30-96DC-25638E1DC8F5"),
                Data = "Data2",
                DateTime = DateTime.Now,
                ValueTypeId = Guid.Parse("89740433-0A86-463A-9430-9A570D145B51"),
            },
            new Value
            {
                Id = Guid.Parse("5B2A3DC5-5C9D-4327-960F-5BD204AFFDB0"),
                Data = "Data3",
                DateTime = DateTime.Now,
                ValueTypeId = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
            }
        }.AsQueryable();


        [Fact]
        public void ReturnSensorValueType()
        {
            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.GetAll()).Returns(GetSensors);

            var sensorValueTypeRepo = new Mock<IValueTypesRepository>();
            sensorValueTypeRepo.Setup(s => s.GetAll()).Returns(GetSensorsValueTypes);

            var sensorValues = new Mock<IValuesRepository>();
            sensorValues.Setup(s => s.GetAll()).Returns(GetValues);

            var _sensorManager = new SensorsManager(sensorRepo.Object, sensorValueTypeRepo.Object, sensorValues.Object);

            var sensorId = Guid.Parse(SENSOR_ONE_ID);

            var sensorValueTypes  = _sensorManager.GetValueTypes(sensorId);

            var expectedSensorsValueType = GetSensorsValueTypes().Where(s => s.SensorId == sensorId);

           Assert.Equal(expectedSensorsValueType.Count(), sensorValueTypes.Count());
           Assert.True(sensorValueTypes.All(s => s.SensorId == sensorId));
        }
    }
}
