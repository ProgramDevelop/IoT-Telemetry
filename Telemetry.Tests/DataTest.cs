using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;
using Telemetry.Base;
using Telemetry.Database.Models;
using ValueType = System.ValueType;

namespace Telemetry.Tests
{
    class DataTest
    {
        public const string USER_ONE_ID  = "00000000-1111-0000-0000-000000000000";
        public const string USER_TWO_ID = "00000000-0000-2222-0000-000000000000";
        public const string SENSOR_ONE_ID = "B031F5CE-0C29-4952-A2C9-FF788188AB24";
        public const string SENSOR_TWO_ID = "5FFBE49C-8089-4015-A8C7-5A94CD113B68";
        public const string SENSOR_THREE_ID = "28D7652C-5A90-4EDF-857C-A859BDDCCB62";
        public const string SENSOR_FOUR_ID = "010975A1-9CFA-4CE8-B6FA-42996B59EE56";
        public const string SENSOR_FIVE_ID = "DF7E1861-4874-4987-A5A5-7F3616FE500F";

        public IQueryable<Sensor> GetSensors() => new Sensor[]
        {
            new Sensor
            {
                Id = Guid.Parse(SENSOR_ONE_ID),
                Name = "Name1",
                Description = "Desc 1",
                UserId = Guid.Parse(USER_ONE_ID),
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_TWO_ID),
                Name = "Name2",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID),
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_THREE_ID),
                Name = "Name3",
                Description = null,
                UserId = Guid.Parse(USER_ONE_ID),
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_FOUR_ID),
                Name = "Name4",
                Description = "Some desc",
                UserId = Guid.Parse(USER_TWO_ID),
            },
            new Sensor
            {
                Id = Guid.Parse(SENSOR_FIVE_ID),
                Name = "Name5",
                Description = null,
                UserId = Guid.Parse(USER_TWO_ID),
            },
        }.AsQueryable();

        public IQueryable<Database.Models.ValueType> GetSensorsValueTypes() => new Database.Models.ValueType[]
        {
            new Database.Models.ValueType
            {
                Id = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
                Name = "NAMEVALUETYPE1",
                SensorId = Guid.Parse(SENSOR_ONE_ID),
                Type = PayloadType.Number,
            },
            new Database.Models.ValueType
            {
                Id = Guid.Parse("89740433-0A86-463A-9430-9A570D145B51"),
                Name = "NAMEVALUETYPE2",
                SensorId = Guid.Parse(SENSOR_ONE_ID),
                Type = PayloadType.Number,
            },
            new Database.Models.ValueType
            {
                Id = Guid.Parse("155C4D30-E739-4324-9D61-EFF015C5A125"),
                Name = "NAMEVALUETYPE3",
                SensorId = Guid.Parse(SENSOR_TWO_ID),
                Type = PayloadType.Number,
            }
        }.AsQueryable();

        public IQueryable<Value> GetValues() => new Value[]
        {
            new Value
            {
                Id = Guid.Parse("E618A42B-0DBB-4099-ADF8-3926C62A5EB8"),
                Data = "Data1",
                DateTime = DateTime.Now,
                ValueTypeId = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
            },
            new Value
            {
                Id = Guid.Parse("78D6D7AA-A9B7-4D30-96DC-25638E1DC8F5"),
                Data = "Data2",
                DateTime = DateTime.Now,
                ValueTypeId = Guid.Parse("89740433-0A86-463A-9430-9A570D145B51"),
            },
            new Value
            {
                Id = Guid.Parse("5B2A3DC5-5C9D-4327-960F-5BD204AFFDB0"),
                Data = "Data3",
                DateTime = DateTime.Now,
                ValueTypeId = Guid.Parse("0DAC21AC-67A2-4639-9C6E-30E993C288CC"),
            }
        }.AsQueryable();
    }
}
