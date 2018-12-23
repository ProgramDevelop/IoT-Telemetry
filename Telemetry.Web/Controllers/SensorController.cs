using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telemetry.Database;
using Telemetry.Database.Models;
using Telemetry.Web.ViewModels.Sensor;

namespace Telemetry.Web.Controllers
{
    [Authorize]
    public class SensorController : Controller
    {
        private readonly TelemetryContext _db;

        public SensorController(TelemetryContext context)
        {
            _db = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(SensorModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.Name;
                var listSensors = _db.Sensors.Where(r => r.UserId == Guid.Parse(userId));
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SensorModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.Name;
                var user = _db.Users.FirstOrDefault(u => u.Id == Guid.Parse(userId));

                var sensor = new Sensor
                {
                    UserId = user.Id,
                    Name = model.Name,
                    Description = model.Description
                };

                _db.Sensors.Add(sensor);
                await _db.SaveChangesAsync();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }

        public async Task<IActionResult> Delete(Sensor model)
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

            return RedirectToAction("Index", "Sensor");
        }
    }
}
