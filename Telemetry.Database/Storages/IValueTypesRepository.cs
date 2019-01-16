using System;
using Telemetry.Database.Base;

namespace Telemetry.Database.Storages
{
    public interface IValueTypesRepository : IRepository<Models.ValueType>
    {
        Models.ValueType[] GetValueTypesForSensor(Guid sensoorId);
    }
}
