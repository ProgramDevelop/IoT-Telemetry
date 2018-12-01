using System;
using System.Collections.Generic;

namespace Telemetry.Base.Interfaces
{
    /// <summary>
    /// Describes the interface for plug-in connection, for receiving data from sensors
    /// </summary>
    public interface IReceiver
    {
        #region Plugin info

        /// <summary>
        /// Plugin name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plug-in description, such as version, connection type, etc.
        /// </summary>
        string Description { get; }

        #endregion

        #region Actions

        /// <summary>
        /// Sends data to the collection server
        /// </summary>
        /// <param name="sensorId">Id of the sensor</param>
        /// <param name="payload">The collection of data from the sensor</param>
        /// <returns></returns>
        bool Send(Guid sensorId, IEnumerable<ISensorValue> payload);

        #endregion
    }
}
