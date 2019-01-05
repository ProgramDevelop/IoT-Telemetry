using System;
using System.Collections.Generic;
using Telemetry.Base;
using Telemetry.Base.Interfaces;
using Telemetry.Database.Base;

namespace Telemetry.Database.Models
{  
    public class ValueType : IValueType, IEntity
    {
        public Guid Id { get; set; }

        public Guid SensorId { get; set; }
        public Sensor Sensor { get; set; }

        public string Name { get; set; }
        public PayloadType Type { get; set; }

        public List<Value> Values { get; set; }
    }
}