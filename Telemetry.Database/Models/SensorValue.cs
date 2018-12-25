using System;
using Telemetry.Base;
using Telemetry.Base.Interfaces;

namespace Telemetry.Database.Models
{  
    public class SensorValue : ISensorValue
    {
        public Guid Id { get; set; }
        public Guid SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public DateTime DateTime { get; set; }
        //TODO Должно ли Name совпадать с именем Name Sensor
        public string Name { get; set; }
        public PayloadType Type { get; set; }
        public string Value { get; set; }
    }
}