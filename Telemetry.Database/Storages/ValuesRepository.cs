using System;
using System.Linq;
using Telemetry.Database.Base;
using Telemetry.Database.Models;

namespace Telemetry.Database.Storages
{
    public class ValuesRepository : Repository<Value>, IValuesRepository
    {
        public ValuesRepository(TelemetryContext context) : base(context)
        {
        }

        public Value[] GetValues(Guid valueTypeId) => GetAll().Where(v => v.ValueTypeId == valueTypeId).ToArray();
    }
}
