using System.Collections.Concurrent;

namespace Lab3_b;

public class BarberShop
{
    private readonly Barber _barber;
    private Thread _barberThread;
    private readonly ConcurrentQueue<Customer> _customers;
    private const int CustomersCount = 10;
    public bool IsEmpty => _customers.IsEmpty;

    public BarberShop()
    {
        _customers = new ConcurrentQueue<Customer>();
        _barber = new Barber(this);
    }

    public void Open()
    {
        for (var i = 0; i < CustomersCount; i++)
        {
            var customer = new Customer(this, 1000 * (i + 1), i);
            customer.Start();
        }

        Console.ReadKey();
    }

    public void WakeUpBarber()
    {
        if (_barberThread == null || !_barberThread.IsAlive)
        {
            _barberThread = new Thread(_barber.Work);
            _barberThread.Start();
        }
    }

    public void AddCustomer(Customer customer)
    {
        _customers.Enqueue(customer);
    }

    public Customer GetCustomer()
    {
        _customers.TryDequeue(out var customer);
        return customer;
    }
}