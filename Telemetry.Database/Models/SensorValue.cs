using System;

namespace Telemetry.Database.Models
{
    public enum ValueType
    {
        String = 0,
        Number = 1,
    }
    
    public class SensorValue
    {
        public Guid Id { get; set; }
        public Guid SensorId { get; set; }
        public Sensor Sensor { get; set; }
        public string Name { get; set; }
        public ValueType Type { get; set; }
        public string Value { get; set; }
    }
}