namespace AutomatedGreenhouseSimulator.Systems;

public class IrrigationSystem
{
    public bool IsOn { get; set; }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine("IrrigationSystem turned on.");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine("IrrigationSystem turned off.");
    }
}
