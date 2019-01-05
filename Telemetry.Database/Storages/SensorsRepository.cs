using System;
using System.Linq;
using Telemetry.Database.Base;
using Telemetry.Database.Models;

namespace Telemetry.Database.Storages
{
    public class SensorsRepository : Repository<Sensor>, ISensorsRepository
    {
        public SensorsRepository(TelemetryContext context) : base(context)
        {
        }

        public Sensor[] GetSensorsForUser(Guid userId)
        {
            return GetAll().Where(s => s.UserId == userId).ToArray();
        }
    }
}
