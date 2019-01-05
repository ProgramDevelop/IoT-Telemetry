using System;
using System.Linq;
using Telemetry.Base;
using Telemetry.Database.Models;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class SensorValuesList
    {

        public SensorValuesList(Database.Models.ValueType valueType, Value[] values)
        {
            SensorId = valueType.SensorId;
            ValueName = valueType.Name;
            ValueType = valueType.Type;

            Values = values.Select(v => new ValueViewModel(v)).ToArray();
        }

        public Guid SensorId { get; set; }
        public string ValueName { get; set; }
        public PayloadType ValueType { get; set; }
        public ValueViewModel[] Values { get; set; }
    }
}
