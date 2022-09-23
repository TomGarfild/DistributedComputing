namespace Lab3_b;

public class Customer
{
    private readonly BarberShop _barberShop;

    private readonly int _waitPeriod;
    public int Id { get; }

    public Customer(BarberShop barberShop, int waitPeriod, int id)
    {
        _barberShop = barberShop;
        _waitPeriod = waitPeriod;
        Id = id;
    }

    public void Start()
    {
        var thread = new Thread(GetHairCut);
        thread.Start();
    }

    public void GetHairCut()
    {
        Thread.Sleep(_waitPeriod);

        if (_barberShop.IsEmpty)
        {
            _barberShop.WakeUpBarber();
            Console.WriteLine($"Customer {Id} will get his hair cut next");
        }
        else
        {
            Console.WriteLine($"Customer {Id} is waiting in queue");
        }

        _barberShop.AddCustomer(this);
    }

    public void HairCutIsDone()
    {
        Console.WriteLine($"Customer {Id} has gotten his hair cut");
        Start();
    }
}