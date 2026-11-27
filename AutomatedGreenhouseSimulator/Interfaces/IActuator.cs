namespace AutomatedGreenhouseSimulator.Interfaces;

public interface IActuator
{
    bool IsOn { get; }
    void TurnOn();
    void TurnOff();
}