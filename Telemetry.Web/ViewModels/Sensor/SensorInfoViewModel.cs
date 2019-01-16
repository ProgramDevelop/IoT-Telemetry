using System;
using System.Linq;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class SensorInfoViewModel
    {

        public SensorInfoViewModel(Database.Models.Sensor sensor)
        {
            Id = sensor.Id;
            Name = sensor.Name;
            Description = sensor.Description;
            Values = new ValueTypeViewModel[] {};
        }

        public SensorInfoViewModel(Database.Models.Sensor sensor, Database.Models.ValueType[] values) : this(sensor)
        {
            Values = values.Select(v => new ValueTypeViewModel(v)).ToArray();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ValueTypeViewModel[] Values { get; set; }
    }
}
