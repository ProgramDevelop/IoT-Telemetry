using System;

namespace Telemetry.Base.Interfaces
{
    /// <summary>
    /// Describes the sensor data interface
    /// </summary>
    public interface IValueType
    {
        /// <summary>
        /// The value name from the sensor
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of data from the sensor
        /// </summary>
        PayloadType Type { get; set; }
    }
}
