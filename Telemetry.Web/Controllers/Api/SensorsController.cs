using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Database.Storages;
using Telemetry.Web.Services.SensorManager;
using Telemetry.Web.ViewModels.Api.Sensors;

namespace Telemetry.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SensorsController : ControllerBase
    {
        private readonly ISensorsManager _sensorManager;
        private readonly IUserRepository _userRepository;

        public SensorsController(ISensorsManager sensorManager, IUserRepository userRepository)
        {
            _sensorManager = sensorManager;
            _userRepository = userRepository;
        }

        [HttpPost("{sensorId}/value/set")]
        public IActionResult StoreValue(Guid sensorId, SetValueViewModel model)
        {
            var sensor = _sensorManager.GetSensorById(sensorId);
            if (sensor == null)
                return NotFound($"Sensor {sensorId} not found");

            var userEmail = User.Identity.Name;
            var user = _userRepository.GetUserByEmail(userEmail);

            if (sensor.UserId != user.Id)
                return Forbid();

            var valueType = _sensorManager.GetValueType(sensorId, model.Name);

            if (valueType == null)
                return NotFound($"ValueType {model.Name} for sensor {sensorId} not found");

            var result = _sensorManager.StoreValue(valueType.Id, model.Payload);

            if (result)
                return Ok();
            return BadRequest();
        }

    }
}