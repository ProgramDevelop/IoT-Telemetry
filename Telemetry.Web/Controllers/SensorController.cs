using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Telemetry.Database;
using Telemetry.Database.Models;
using Telemetry.Web.ViewModels.Sensor;

namespace Telemetry.Web.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class SensorController : Controller
    {
        private readonly TelemetryContext _db;

        public SensorController(TelemetryContext context)
        {
            _db = context;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var login = User.Identity.Name;
            var user = _db.Users.FirstOrDefault(u => u.Email == login);

            var sensors = _db.Sensors.Where(s => s.UserId == user.Id).Select(s => new SensorViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToArray();

            return View(sensors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var sensor = await _db.Sensors.FirstOrDefaultAsync(s => s.Id == id);

            var values = await _db.ValueTypes.Where(vt => vt.SensorId == sensor.Id).ToArrayAsync();

            return View(new SensorInfoViewModel(sensor, values));
        }

        [HttpGet("{id}/Value/{name}")]
        public async Task<IActionResult> GetValue(Guid id, string name)
        {
            var value = await _db.ValueTypes.FirstOrDefaultAsync(s => s.SensorId == id && s.Name == name);

            var values = await _db.Values.Where(v => v.ValueTypeId == value.Id).ToArrayAsync();

            var model = new SensorValuesList(value, values);

            return View(model);
        }

        [HttpGet("{id}/Add")]
        public async Task<IActionResult> AddValueType(Guid id)
        {
            var sensor = await _db.Sensors.FirstOrDefaultAsync(s => s.Id == id);

            return View(new ValueTypeViewModel { SensorId = sensor.Id });
        }

        [HttpPost("{id}/Add")]
        public async Task<IActionResult> AddValueType(ValueTypeViewModel model)
        {
            if (ModelState.IsValid)
            {

                var sensor = await _db.Sensors.FirstOrDefaultAsync(s => s.Id == model.SensorId);

                var value = new Database.Models.ValueType
                {
                    SensorId = sensor.Id,
                    Type = model.Type,
                    Name = model.Name
                };

                await _db.ValueTypes.AddAsync(value);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Info), new { id = sensor.Id });

            }
            return View(model);

        }


        //[HttpPost("SetValue")]
        //public async Task<IActionResult> SetSensorValue(SetSensorValueModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var sensor = _db.Sensors.FirstOrDefault(r => r.Id == Guid.Parse(model.SensorId));
        //        var sensorValue = new SensorValue
        //        {
        //            SensorId = Guid.Parse(model.SensorId),
        //            Name = model.Name,
        //            Type = model.Type,
        //            Value = model.Value,
        //            DateTime = DateTime.Now,
        //            Sensor = sensor
        //        };

        //        _db.SensorValues.Add(sensorValue);
        //        await _db.SaveChangesAsync();
        //        return Ok();
        //    }

        //    return NotFound();
        //}


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
                var login = User.Identity.Name;
                var user = _db.Users.FirstOrDefault(u => u.Email == login);

                var sensor = new Sensor
                {
                    UserId = user.Id,
                    Name = model.Name,
                    Description = model.Description
                };

                _db.Sensors.Add(sensor);
                await _db.SaveChangesAsync();

                return RedirectToAction(nameof(Info), new { id = sensor.Id });
            }

            return View(model);
        }

        [HttpGet("Delete")]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpGet("{id}/Delete")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var sensor = await _db.Sensors.FirstOrDefaultAsync(s => s.Id == id);

                if (sensor != null)
                {
                    _db.Sensors.Remove(sensor);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }

            return RedirectToAction("Index", "Sensor");
        }
    }
}
