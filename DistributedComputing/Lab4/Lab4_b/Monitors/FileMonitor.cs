namespace Lab4_b.Monitors;

public class FileMonitor : Monitor
{
    public FileMonitor(Garden garden) : base(garden)
    {
    }

    protected override void Watch()
    {
        Garden.OutputToFile();
    }
}