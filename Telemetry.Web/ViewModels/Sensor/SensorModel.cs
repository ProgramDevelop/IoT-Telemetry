using System;
using System.ComponentModel.DataAnnotations;

namespace Telemetry.Web.ViewModels.Sensor
{
    public class SensorViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Название датчика не может быть пустым!")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
