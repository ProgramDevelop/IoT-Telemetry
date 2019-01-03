using System;
using System.Collections.Generic;
using System.Text;
using Telemetry.Base.Interfaces;

namespace Telemetry.Receivers
{
    public class SensorData : ISensorData
    {
        public string Data { get; set; }
        public DateTime DateTime { get ; set; }
    }
}
