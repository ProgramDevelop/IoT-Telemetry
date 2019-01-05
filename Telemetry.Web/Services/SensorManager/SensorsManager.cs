using System;
using System.Linq;
using Telemetry.Base;
using Telemetry.Database.Models;
using Telemetry.Database.Storages;

namespace Telemetry.Web.Services.SensorManager
{
    public class SensorsManager : ISensorsManager
    {
        private readonly ISensorsRepository _sensorsRepository;
        private readonly IValueTypesRepository _valueTypesRepository;
        private readonly IValuesRepository _valuesRepository;

        public SensorsManager(
            ISensorsRepository sensorsRepository,
            IValueTypesRepository valueTypesRepository,
            IValuesRepository valuesRepository
            )
        {
            _sensorsRepository = sensorsRepository;
            _valueTypesRepository = valueTypesRepository;
            _valuesRepository = valuesRepository;
        }

        #region Sensors Implementation

        public Sensor CreareSensor(Guid userId, string sensorName, string description) {

            var sensor = new Sensor
            {
                UserId = userId,
                Name = sensorName.Trim().ToUpper(),
                Description = description.Trim()
            };

            var result = _sensorsRepository.CreateAsync(sensor).GetAwaiter().GetResult();

            return result ? sensor : null;
        }

        public Sensor GetSensorById(Guid sensorId) => _sensorsRepository.GetByIdAsync(sensorId).GetAwaiter().GetResult();

        public bool DeleteSensor(Guid sensorId) => _sensorsRepository.DeleteAsync(sensorId).GetAwaiter().GetResult();

        public Sensor[] GetSensorsForUser(Guid id) => _sensorsRepository.GetAll().Where(s => s.UserId == id).ToArray();

        #endregion

        #region ValueTypes Implementation

        public Database.Models.ValueType CreateValueType(Guid sensorId, string name, PayloadType type)
        {
            var sensor = GetSensorById(sensorId);
            if (sensor == null)
                return null;

            var value = new Database.Models.ValueType
            {
                SensorId = sensor.Id,
                Type = type,
                Name = name.Trim().ToUpper()
            };

            var result = _valueTypesRepository.CreateAsync(value).GetAwaiter().GetResult();
            return result ? value : null;
        }

        public Database.Models.ValueType[] GetValueTypes(Guid sensorId) => _valueTypesRepository.GetValueTypesForSensor(sensorId);

        public Database.Models.ValueType GetValueType(Guid sensorId, string name) => 
            _valueTypesRepository.GetAll()
                .FirstOrDefault(s => s.SensorId == sensorId && s.Name == name.Trim().ToUpper());

        #endregion

        #region Values Implementation

        public Value[] GetValues(Guid valueTypeId) =>
            _valuesRepository.GetValues(valueTypeId);

        #endregion
    }
}
