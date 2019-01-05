using System;
using Telemetry.Database.Models;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class ValueViewModel
    {
        public ValueViewModel(Value value)
        {
            Id = value.Id;
            DateTime = value.DateTime;
            Data = value.Data;
        }

        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}