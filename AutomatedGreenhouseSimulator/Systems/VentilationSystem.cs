namespace AutomatedGreenhouseSimulator.Systems;

public class VentilationSystem
{
    public bool IsOn { get; set; }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine("VentilationSystem turned on.");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine("VentilationSystem turned off.");
    }
}
