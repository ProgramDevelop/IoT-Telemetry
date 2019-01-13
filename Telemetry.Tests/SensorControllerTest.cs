using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Moq;
using Telemetry.Base;
using Telemetry.Database.Models;
using Telemetry.Database.Storages;
using Telemetry.Web.Controllers;
using Telemetry.Web.Services.SensorManager;
using Telemetry.Web.ViewModels.Sensor;
using Xunit;
using ValueType = Telemetry.Database.Models.ValueType;

namespace Telemetry.Tests
{
    public class SensorControllerTest
    {
        private const string USER_ONE_ID = "00000000-1111-0000-0000-000000000000";

        private const string SENSOR_ONE_ID = "B031F5CE-0C29-4952-A2C9-FF788188AB24";
        private const string SENSOR_TWO_ID = "5FFBE49C-8089-4015-A8C7-5A94CD113B68";
 
        private IQueryable<Sensor> GetSensors() => new Sensor[]
        {
            new Sensor
            {
                Id = Guid.Parse(SENSOR_ONE_ID),
                Name = "Name1",
                Description = "Desc 1",
                UserId = Guid.Parse(USER_ONE_ID),
                Values = GetSensorsValueTypes().Where(v => v.SensorId == Guid.Parse(SENSOR_ONE_ID)).ToList()
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_TWO_ID),
                Name = "Name2",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID),
                Values = GetSensorsValueTypes().Where(v => v.SensorId == Guid.Parse(SENSOR_TWO_ID)).ToList()
            }
        }.AsQueryable();

        private IQueryable<ValueType> GetSensorsValueTypes() => new ValueType[]
        {
            new ValueType
            {
                Id = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
                Name = "NAMEVALUETYPE1",
                SensorId = Guid.Parse(SENSOR_ONE_ID),
                Type = PayloadType.Number,
                Values = GetValues()
                    .Where(v => v.ValueTypeId == Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC")).ToList()
            },
            new ValueType
            {
                Id = Guid.Parse("89740433-0A86-463A-9430-9A570D145B51"),
                Name = "NAMEVALUETYPE2",
                SensorId = Guid.Parse(SENSOR_ONE_ID),
                Type = PayloadType.Number,
                Values = GetValues()
                    .Where(v => v.ValueTypeId == Guid.Parse("89740433-0A86-463A-9430-9A570D145B51")).ToList()
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
            }
        }.AsQueryable();

        [Theory]
        [InlineData(SENSOR_ONE_ID, "NAMEVALUETYPE1")]
        public void ReturnGetValueModel(string id, string nameValueType)
        {
            var sensorId = Guid.Parse(id);
            var sensorValueTypeRepo = new Mock<IValueTypesRepository>();
            sensorValueTypeRepo.Setup(s => s.GetAll()).Returns(GetSensorsValueTypes);

            var valueType = GetSensorsValueTypes()
                .FirstOrDefault(r => r.SensorId == sensorId && r.Name == nameValueType.ToUpper());

            Assert.NotNull(valueType);

            var sensorValuesRepo = new Mock<IValuesRepository>();
            sensorValuesRepo.Setup(s => s.GetValues(valueType.Id))
                .Returns(GetValues().Where(v => v.ValueTypeId == valueType.Id).ToArray);

            var _sensorsManager = new SensorsManager(null, sensorValueTypeRepo.Object, sensorValuesRepo.Object);
            
            var sensorController = new SensorController(_sensorsManager,null);

           var result =  sensorController.GetValue(sensorId, nameValueType).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SensorValuesList>(viewResult.ViewData.Model);

            Assert.Equal(valueType.SensorId, model.SensorId);
            Assert.Equal(valueType.Name, model.ValueName);
            Assert.Equal(valueType.Type, model.ValueType);
            Assert.Equal(valueType.Values.Count(), model.Values.Count());
        }

        [Theory]
        [InlineData(SENSOR_ONE_ID)]
        public void ReturnInfoModel(string id)
        {
            var sensorId = Guid.Parse(id);

            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.GetById(sensorId)).Returns(GetSensors().FirstOrDefault(s => s.Id == sensorId));

            var sensorValueTypeRepo = new Mock<IValueTypesRepository>();
            sensorValueTypeRepo.Setup(s => s.GetValueTypesForSensor(sensorId))
                .Returns(GetSensorsValueTypes().Where(v => v.SensorId == sensorId).ToArray);

            var _sensorsManager = new SensorsManager(sensorRepo.Object, sensorValueTypeRepo.Object, null);

            var sensorController = new SensorController(_sensorsManager, null);

            var result = sensorController.Info(sensorId).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SensorInfoViewModel>(viewResult.ViewData.Model);

            var expectedSensorInfo = GetSensors().FirstOrDefault(s => s.Id == sensorId);

            Assert.NotNull(expectedSensorInfo);
            Assert.Equal(expectedSensorInfo.Id, model.Id);
            Assert.Equal(expectedSensorInfo.Description, model.Description);
            Assert.Equal(expectedSensorInfo.Name, model.Name);
            Assert.Equal(expectedSensorInfo.Values.Count(), model.Values.Count());
        }

        [Theory]
        [InlineData(SENSOR_ONE_ID)]
        public void ReturnAddValueType(string id)
        {
            var sensorId = Guid.Parse(id);

            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.GetById(sensorId)).Returns(GetSensors().FirstOrDefault(s => s.Id == sensorId));

            var _sensorsManager = new SensorsManager(sensorRepo.Object, null, null);

            var sensorController = new SensorController(_sensorsManager, null);

            var result = sensorController.AddValueType(sensorId).Result;

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ValueTypeViewModel>(viewResult.ViewData.Model);

            Assert.Null(model.Name);
            Assert.Equal(Guid.Empty, model.Id);
            Assert.Equal(sensorId,model.SensorId);
            Assert.Equal(PayloadType.String,model.Type);
        }

        public static IEnumerable<object[]> ValueTypeViewModelData() => new List<object[]>
        {
            new object[]
            {
              new ValueTypeViewModel
              {
                  Id = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
                  SensorId = Guid.Parse(SENSOR_ONE_ID),
                  Name = "NameValueType",
                  Type = PayloadType.String
              } 
            }
        };

        [Theory]
        [MemberData(nameof(ValueTypeViewModelData))]
        public void ReturnAddValueTypeModel(ValueTypeViewModel model)
        {
            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.GetById(model.SensorId)).Returns(GetSensors().FirstOrDefault(s => s.Id == model.SensorId));

            var sensorValueTypeRepo = new Mock<IValueTypesRepository>();
            sensorValueTypeRepo.Setup(s => s.Create(It.IsAny<ValueType>())).Returns(true);

            var _sensorsManager = new SensorsManager(sensorRepo.Object, sensorValueTypeRepo.Object, null);

            var sensorController = new SensorController(_sensorsManager, null);

            var result = sensorController.AddValueType(model).Result;

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Info", redirectToActionResult.ActionName);

        }


        [Theory]
        [InlineData(SENSOR_ONE_ID)]
        [InlineData(SENSOR_TWO_ID)]
        public void ReturnDeleteModel(string id)
        {
            var sensorId = Guid.Parse(id);

            var sensorRepo = new Mock<ISensorsRepository>();
            sensorRepo.Setup(s => s.Delete(sensorId)).Returns(true);

            var _sensorManager = new SensorsManager(sensorRepo.Object, null, null);

            var sensorController = new SensorController(_sensorManager, null);

            var result = sensorController.Delete(sensorId).Result;

            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("Sensor", redirectToActionResult.ControllerName);
        }
    }
}
