namespace Lab4_b.Monitors;

public abstract class Monitor
{
    protected readonly Garden Garden;

    protected Monitor(Garden garden)
    {
        Garden = garden;
    }

    protected abstract void Watch();

    public void Start()
    {
        while (true)
        {
            Watch();
            Console.WriteLine($"{GetType().Name} finished monitoring garden");
        }
    }
}