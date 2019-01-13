using System;
using Telemetry.Base.Interfaces;

namespace Telemetry.Base
{
    public class ApiPayload : ISensorData
    {
        public string Data { get; set; }
        public DateTime DateTime { get; set; }
    }
}