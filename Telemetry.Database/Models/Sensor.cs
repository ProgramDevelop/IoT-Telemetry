using System;
using System.Collections.Generic;

namespace Telemetry.Database.Models
{
    public class Sensor
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Sensor> Values { get; set; }
    }
}