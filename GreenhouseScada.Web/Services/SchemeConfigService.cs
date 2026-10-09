using Models.Entities;

namespace GreenhouseScada.Web.Services
{
    public class SchemeConfigService
    {
        private readonly List<SensorView> _sensors = new List<SensorView>
        {
            new SensorView
            {
                Name = "Температура зовні", Topic = "greenhouse/1/sensors/outside/temperature", X = 20, Y = 40
            },
            new SensorView
            {
                Name = "Вологість зовні", Topic = "greenhouse/1/sensors/outside/humidity", X = 20, Y = 130

            },
            new SensorView
            {
                Name = "Температура всередині", Topic = "greenhouse/1/sensors/inside/temperature", X = 250, Y = 190
            },
            new SensorView
            {
                Name = "Вологість всередині", Topic = "greenhouse/1/sensors/inside/humidity", X = 250, Y = 280
            }
        };

        public IReadOnlyList<SensorView> GetSensors()
        {
            return _sensors;
        }
    }
}
