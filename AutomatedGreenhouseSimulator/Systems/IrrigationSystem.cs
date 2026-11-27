using AutomatedGreenhouseSimulator.Interfaces;

namespace AutomatedGreenhouseSimulator.Systems;

public class IrrigationSystem : IActuator
{
    public bool IsOn { get; private set; }

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