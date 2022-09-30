namespace Lab4_b.Monitors;

public class ConsoleMonitor : Monitor
{
    public ConsoleMonitor(Garden garden) : base(garden)
    {
    }

    protected override void Watch()
    {
        Garden.Print();
    }
}