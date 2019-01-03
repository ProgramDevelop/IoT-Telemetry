using System;
using System.ComponentModel.DataAnnotations;
using Telemetry.Base;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class ValueTypeViewModel
    {
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
