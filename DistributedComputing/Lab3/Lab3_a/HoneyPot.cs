namespace Lab3_a;

public sealed class HoneyPot
{
    private readonly int _maxCapacity;
    private int _currentCapacity;

    private bool IsFull => _maxCapacity == _currentCapacity;

    public event EventHandler NotifiedWhenFull;
    public static readonly object Obj = new();

    public HoneyPot(int maxCapacity)
    {
        _maxCapacity = maxCapacity;
    }
    public void Eat()
    {
        try
        {
            Monitor.Enter(Obj);
            Console.WriteLine("Bear starts eating honey");
            Thread.Sleep(_maxCapacity * 1000);
            Interlocked.Exchange(ref _currentCapacity, 0);
            Console.WriteLine("Bear finished eating");
        }
        finally
        {
            Monitor.Exit(Obj);
        }
    }

    public void Fill(int id)
    {
        while (Monitor.IsEntered(Obj))
        {
            Thread.Sleep(100);
        }

        Thread.Sleep(1000);
        if (!IsFull)
        {
            Interlocked.Increment(ref _currentCapacity);
            Console.WriteLine($"Bee {id} filled honey pot");

            if (IsFull)
            {
                NotifiedWhenFull?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}