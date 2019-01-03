using System;
using System.Collections.Generic;
using Telemetry.Base;
using Telemetry.Base.Interfaces;

namespace Telemetry.Database.Models
{  
    public class ValueType : IValueType
    {
        public Guid Id { get; set; }

        public Guid SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public string Name { get; set; }
        public PayloadType Type { get; set; }

        public List<Value> Values { get; set; }
    }
}