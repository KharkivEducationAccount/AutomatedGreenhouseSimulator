using Models.Entities;

namespace GreenhouseScada.Web.Services
{
    public class SchemeConfigService
    {
        private readonly List<SensorView> _sensors = new List<SensorView>
        {
            new SensorView
            {
                Name = "Температура зовні", Topic = "greenhouse/1/sensors/outside/temperature", X = 20, Y = 40, Max = 30
            },
            new SensorView
            {
                Name = "Вологість зовні", Topic = "greenhouse/1/sensors/outside/humidity", X = 20, Y = 130, Max = 50

            },
            new SensorView
            {
                Name = "Температура всередині", Topic = "greenhouse/1/sensors/inside/temperature", X = 250, Y = 190, Max = 20
            },
            new SensorView
            {
                Name = "Вологість всередині", Topic = "greenhouse/1/sensors/inside/humidity", X = 250, Y = 280, Max = 40
            }
        };

        private readonly List<ActuatorView> _actuators = new List<ActuatorView>
        {
            new ActuatorView
            {
                Name = "Вентиляція", Topic = "greenhouse/1/actuators/ventilation/state", Kind = "fan", X = 470, Y = 190
            },
            new ActuatorView
            {
                Name = "Полив", Topic = "greenhouse/1/actuators/irrigation/state", Kind = "water", X = 470, Y = 280
            }
        };

        public bool UpdateMax(string topic, int max)
        {
            foreach (SensorView sensor in _sensors)
            {
                if (sensor.Topic == topic)
                {
                    sensor.Max = max;

                    return true;
                }
            }

            return false;
        }

        public IReadOnlyList<SensorView> GetSensors()
        {
            return _sensors;
        }

        public IReadOnlyList<ActuatorView> GetActuators()
        {
            return _actuators;
        }
    }
}
