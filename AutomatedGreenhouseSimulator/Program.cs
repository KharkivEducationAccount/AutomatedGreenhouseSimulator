using AutomatedGreenhouseSimulator;

public class Program
{
    public static void Main()
    {
        TemperatureSensor outsideTemperatureSensor = new TemperatureSensor("Outside temperature");
        HumiditySensor outsideHumiditySensor = new HumiditySensor("Outside humidity");
        TemperatureSensor innerTemperatureSensor = new TemperatureSensor("Inner temperature");
        HumiditySensor innerHumiditySensor = new HumiditySensor("Inner humidity");

        Greenhouse greenhouse = new Greenhouse();
        greenhouse.Sensors.Add(outsideTemperatureSensor);
        greenhouse.Sensors.Add(outsideHumiditySensor);
        greenhouse.Sensors.Add(innerTemperatureSensor);
        greenhouse.Sensors.Add(innerHumiditySensor);

        for (int i = 0; i < 10; i++)
        {
            Console.WriteLine($"Iteration {i+1}");
            greenhouse.Monitor();

            if (innerTemperatureSensor.Value > 25)
            {
                greenhouse.Ventilation.TurnOn();
            }
            else
            {
                greenhouse.Ventilation.TurnOff();
            }


            if (innerHumiditySensor.Value < 25)
            {
                greenhouse.Irrigation.TurnOn();
            }
            else
            {
                greenhouse.Irrigation.TurnOff();
            }

            Console.WriteLine();
        }
    }
}