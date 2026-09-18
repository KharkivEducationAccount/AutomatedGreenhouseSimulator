using AutomatedGreenhouseSimulator.Systems;

namespace AutomatedGreenhouseSimulator;

public class Greenhouse
{
    public List<Sensor> Sensors { get; set; }
    public VentilationSystem Ventilation { get; set; }
    public IrrigationSystem Irrigation { get; set; }

    public Greenhouse()
    {
        Sensors = new List<Sensor>();
        Ventilation = new VentilationSystem();
        Irrigation = new IrrigationSystem();
    }

    public void Monitor()
    {
        foreach(Sensor sensor in Sensors)
        {
            sensor.ReadValue();
        }
    }
}