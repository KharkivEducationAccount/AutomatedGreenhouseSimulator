namespace Models;

public class TemperatureSensor : Sensor
{
    public TemperatureSensor(string name) : base(name)
    {
    }

    public override void ReadValue()
    {
        Value = new Random().NextDouble() * 50;
        Console.WriteLine($"{Name} measured temperature {Value}.");
    }
}