using System;
using System.Collections.Generic;
using System.Text;
using Telemetry.Base;
using Telemetry.Base.Interfaces;
using Telemetry.Database.Base;

namespace Telemetry.Database.Models
{
    public class Value : ISensorData, IEntity
    {
        public Guid Id { get; set; }

        public ValueType ValueType { get; set; }
        public Guid ValueTypeId { get; set; }

        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}
