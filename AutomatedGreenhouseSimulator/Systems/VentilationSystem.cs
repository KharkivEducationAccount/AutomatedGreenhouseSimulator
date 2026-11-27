using AutomatedGreenhouseSimulator.Interfaces;

namespace AutomatedGreenhouseSimulator.Systems;

public class VentilationSystem : IActuator
{
    public bool IsOn { get; private set; }

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