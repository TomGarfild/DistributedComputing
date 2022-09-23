namespace Lab3_b;

public class Barber
{
    private readonly BarberShop _barberShop;
    private const int WorkDuration = 3000;
    private bool _started;

    public Barber(BarberShop barberShop)
    {
        _barberShop = barberShop;
    }

    public void Work()
    {
        _started = true;
        while (true)
        {
            if (_barberShop.IsEmpty)
            {
                if (_started)
                {
                    Thread.Sleep(100);
                    continue;
                }

                Console.WriteLine("Queue of customers is empty. Barber is sleeping...\n");
                break;
            }

            _started = false;
            var customer = _barberShop.GetCustomer();
            Console.WriteLine($"Barber started cutting off hair for {customer?.Id}");

            Thread.Sleep(WorkDuration);

            Console.WriteLine($"Barber finished cutting off hair for {customer?.Id}");

            customer?.HairCutIsDone();
        }
    }
}