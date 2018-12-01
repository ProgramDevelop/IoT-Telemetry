namespace Telemetry.Base.Interfaces
{
    /// <summary>
    /// Describes the sensor data interface
    /// </summary>
    public interface ISensorValue
    {
        /// <summary>
        /// The value name from the sensor
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Type of data from the sensor
        /// </summary>
        PayloadType Type { get; set; }

        /// <summary>
        /// Value from the sensor
        /// </summary>
        string Value { get; set; }
    }
}
