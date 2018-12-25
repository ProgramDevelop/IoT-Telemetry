using System;
using System.Collections.Generic;
using System.Text;
using Telemetry.Base;
using Telemetry.Base.Interfaces;

namespace Telemetry.Database.Models
{
    public class Value : ISensorData
    {
        public Guid Id { get; set; }

        public ValueType ValueType { get; set; }
        public Guid ValueTypeId { get; set; }

        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}
