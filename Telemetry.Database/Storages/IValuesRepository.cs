using System;
using Telemetry.Database.Base;
using Telemetry.Database.Models;

namespace Telemetry.Database.Storages
{
    public interface IValuesRepository : IRepository<Value>
    {
        Value[] GetValues(Guid valueTypeId);
    }
}
