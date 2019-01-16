using System;
using System.Collections.Generic;
using Telemetry.Database.Base;

namespace Telemetry.Database.Models
{
    public class Sensor : IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ValueType> Values { get; set; }
    }
}