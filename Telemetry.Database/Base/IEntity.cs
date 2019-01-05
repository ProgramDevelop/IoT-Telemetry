using System;

namespace Telemetry.Database.Base
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
