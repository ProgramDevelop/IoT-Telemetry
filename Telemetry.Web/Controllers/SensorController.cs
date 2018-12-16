using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Database;
using Telemetry.Database.Models;
using Telemetry.Web.Models.ViewModels;

namespace Telemetry.Web.Controllers
{
    public class SensorController : Controller
    {
        private readonly TelemetryContext _db;

        public SensorController(TelemetryContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Sensor()
        {
            return View();
        }

        [HttpPost]
        public Task<IActionResult> CreateSensor(SensorModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.Name;
                var user = _db.Users.FirstOrDefault(u => u.Id == Guid.Parse(userId));

                var sensor = new Sensor
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.Parse(userId),
                    User = user,
                    Name = model.Name,
                    Description = model.Description
                };

                _db.Sensors.Add(sensor);
                _db.SaveChanges();
            }

            return View(model);
        }

        public Task<IActionResult> DeleteSensor(Sensor model)
        {
            if (ModelState.IsValid)
            {
                var sensor = _db.Sensors.FirstOrDefault(s => s.Id == model.Id);

                if (sensor != null)
                {
                    _db.Sensors.Remove(sensor);
                }
                else
                {
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                }
            }

            return View(model);
        }

    }
}
