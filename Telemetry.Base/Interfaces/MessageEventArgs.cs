using System;

namespace Telemetry.Base.Interfaces
{
    public class MessageEventArgs
    {
        public Guid SensorId { get; set; }
        public string ValueName { get; set; }
        public ISensorData Payload { get; set; }
    }
}
