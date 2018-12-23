using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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
        /// Start up receiver
        /// </summary>
        /// <param name="token">Receiver Stop Token</param>
        /// <returns></returns>
        Task StartAsync(CancellationToken token);

        /// <summary>
        /// Stops the receiver
        /// </summary>
        /// <returns></returns>
        Task StopAsync();

        #endregion
        
        #region Events

        /// <summary>
        /// Reports that a message has arrived from the sensors.
        /// </summary>
        event Action<Guid, IEnumerable<ISensorData>> OnMessageReceive;

        #endregion
    }
}
