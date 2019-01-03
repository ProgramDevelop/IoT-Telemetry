using System;

namespace Telemetry.Database
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
