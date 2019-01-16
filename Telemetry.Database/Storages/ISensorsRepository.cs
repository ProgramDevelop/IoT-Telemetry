using System;
using Telemetry.Database.Base;
using Telemetry.Database.Models;

namespace Telemetry.Database.Storages
{
    public interface ISensorsRepository : IRepository<Sensor>
    {
        Sensor[] GetSensorsForUser(Guid userId);
    }
}