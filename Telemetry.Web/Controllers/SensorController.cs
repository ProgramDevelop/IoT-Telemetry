using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Database.Models;
using Telemetry.Database.Storages;
using Telemetry.Web.Services.SensorManager;
using Telemetry.Web.ViewModels.Sensor;

namespace Telemetry.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class SensorController : Controller
    {
        private readonly ISensorsManager _sensorsManager;
        private readonly IUserRepository _userRepository;

        public SensorController(ISensorsManager manager, IUserRepository userRepository)
        {
            _sensorsManager = manager;
            _userRepository = userRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var email = User.Identity.Name;
            var user = _userRepository.GetUserByEmail(email);
            Sensor[] sensors = _sensorsManager.GetSensorsForUser(user.Id);
            var sensorsViewModels = sensors.Select(s => new SensorViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToArray();

            return View(sensorsViewModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var sensor = _sensorsManager.GetSensorById(id);
            var values = _sensorsManager.GetValueTypes(sensor.Id);
            return View(new SensorInfoViewModel(sensor, values));
        }

        [HttpGet("{id}/Value/{name}")]
        public async Task<IActionResult> GetValue(Guid id, string name)
        {
            var value = _sensorsManager.GetValueType(id, name);
            var values = _sensorsManager.GetValues(value.Id);


            var model = new SensorValuesList(value, values);

            return View(model);
        }

        [HttpGet("{id}/Add")]
        public async Task<IActionResult> AddValueType(Guid id)
        {
            var sensor = _sensorsManager.GetSensorById(id);
            return View(new ValueTypeViewModel { SensorId = sensor.Id });
        }

        [HttpPost("{id}/Add")]
        public async Task<IActionResult> AddValueType(ValueTypeViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(_sensorsManager.CreateValueType(model.SensorId, model.Name, model.Type) != null)
                {
                    return RedirectToAction(nameof(Info), new { id = model.SensorId });
                }
            }
            return View(model);

        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(SensorViewModel model)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity.Name;
                var user = _userRepository.GetUserByEmail(email);

                var sensor = _sensorsManager.CreateSensor(user.Id, model.Name, model.Description);

                return RedirectToAction(nameof(Info), new { id = sensor.Id });
            }

            return View(model);
        }

        [HttpGet("{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                if (!_sensorsManager.DeleteSensor(id))
                {
                    ModelState.AddModelError("", "Ошибка при удалении датчика");
                    return View();
                }
            }
            return RedirectToAction("Index", "Sensor");
        }
    }
}
