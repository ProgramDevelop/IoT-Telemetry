using Telemetry.Base;

namespace Telemetry.Web.ViewModels.Api.Sensors
{
    public class SetValueViewModel
    {
        public string Name { get; set; }

        public ApiPayload Payload { get; set; }
    }
}