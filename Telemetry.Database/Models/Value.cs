using System;
using Telemetry.Base.Interfaces;
using Telemetry.Database.Base;

namespace Telemetry.Database.Models
{
    public class Value : ISensorData, IEntity
    {
        public Value() { }

        public Value(Guid valueTypeId, ISensorData payload)
        {
            ValueTypeId = valueTypeId;
            DateTime = payload.DateTime;
            Data = payload.Data;
        }


        public Guid Id { get; set; }

        public ValueType ValueType { get; set; }
        public Guid ValueTypeId { get; set; }

        public DateTime DateTime { get; set; }
        public string Data { get; set; }
    }
}