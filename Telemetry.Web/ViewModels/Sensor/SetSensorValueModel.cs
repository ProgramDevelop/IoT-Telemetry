using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Telemetry.Base;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class SetSensorValueModel
    {
        /// <summary>
        /// Id датчика
        /// </summary>
        [Required(ErrorMessage = "Id датчика не может быть пустым!")]
        public string SensorId { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Заначение
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Тип
        /// </summary>
        [Required(ErrorMessage = "Тип значения датчика не может быть пустым!")]
        public PayloadType Type { get; set; }
    }
}
