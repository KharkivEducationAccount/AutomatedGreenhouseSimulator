namespace AutomatedGreenhouseSimulator;

public class HumiditySensor : Sensor
{
    public HumiditySensor(string name) : base(name)
    {
    }

    public override void ReadValue()
    {
        Value = new Random().NextDouble() * 100;
        Console.WriteLine($"{Name} measured humidity {Value}.");
    }
}