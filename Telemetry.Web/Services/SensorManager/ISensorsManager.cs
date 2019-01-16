using System;
using Telemetry.Base;
using Telemetry.Base.Interfaces;
using Telemetry.Database.Models;

namespace Telemetry.Web.Services.SensorManager
{
    public interface ISensorsManager
    {
        #region Sensor methods

        Sensor CreateSensor(Guid userId, string sensorName, string description);
        bool DeleteSensor(Guid sensorId);
        Sensor[] GetSensorsForUser(Guid id);
        Sensor GetSensorById(Guid id);

        #endregion

        #region ValueType methods

        Database.Models.ValueType[] GetValueTypes(Guid sensorId);
        Database.Models.ValueType GetValueType(Guid sensorId, string name);
        Database.Models.ValueType CreateValueType(Guid sensorId, string name, PayloadType type);

        #endregion

        #region Values methods

        Value[] GetValues(Guid valueTypeId);

        bool StoreValue(Guid id, ISensorData payload);

        #endregion
    }
}
