using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Telemetry.Web.Models.ViewModels
{
    //TODO Перенести ViewModels уточнить
    public class SensorModel
    {

        [Required(ErrorMessage = "Название датчика не может быть пустым!")]
        [DataType(DataType.Custom)]
        public string Name { get; set; }


        [Required(ErrorMessage = "Описание не может быть пустым!")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

    }
}
