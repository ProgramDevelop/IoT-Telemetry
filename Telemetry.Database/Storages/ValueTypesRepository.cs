using System;
using System.Linq;
using Telemetry.Database.Base;

namespace Telemetry.Database.Storages
{
    public class ValueTypesRepository : Repository<Models.ValueType>, IValueTypesRepository
    {
        public ValueTypesRepository(TelemetryContext context) : base(context)
        {
        }

        public Models.ValueType[] GetValueTypesForSensor(Guid sensoorId) => 
            GetAll().Where(v => v.SensorId == sensoorId).ToArray();
    }
}
