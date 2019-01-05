using System;
using System.ComponentModel.DataAnnotations;
using Telemetry.Base;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class ValueTypeViewModel
    {
        public ValueTypeViewModel()
        {
        }

        public ValueTypeViewModel(Database.Models.ValueType valueType)
        {
            Id = valueType.Id;
            Name = valueType.Name;
            Type = valueType.Type;
            SensorId = valueType.SensorId;
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Id датчика
        /// </summary>
        [Required(ErrorMessage = "Id датчика не может быть пустым!")]
        public Guid SensorId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        [Required(ErrorMessage = "Название не может быть пустым")]
        public string Name { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        [Required(ErrorMessage = "Тип не может быть пустым!")]
        public PayloadType Type { get; set; }
    }
}
