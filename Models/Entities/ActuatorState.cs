namespace Models.Entities;

public class ActuatorState
{
    public string Topic { get; set; } = "";
    public bool IsOn { get; set; }
    public string Reason { get; set; } = "";
    public DateTimeOffset ChangedAtUtc { get; set; }
}