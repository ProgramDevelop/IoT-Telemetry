using System;
using System.Collections.Generic;

namespace Telemetry.Database.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<Sensor> Sensors { get; set; }
    }
}