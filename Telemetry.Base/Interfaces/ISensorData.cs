using System;

namespace Telemetry.Base.Interfaces
{
    /// <summary>
    /// Describes the sensor data interface
    /// </summary>
    public interface ISensorData
    {
        /// <summary>
        /// Value from the sensor
        /// </summary>
        string Data { get; set; }

        /// <summary>
        /// Time at which values were obtained
        /// </summary>
        DateTime DateTime { get; set; }
    }
}
