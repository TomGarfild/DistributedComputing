using Exam.Domain;

namespace Exam.Server;

public class Supermarket
{
    private readonly List<Product> _products;


    public Supermarket()
    {
        _products = new List<Product>()
        {
            new(1, "Milk", 228, "Ukraine", 100, DateTime.UtcNow.AddDays(2), 2),
            new(2, "Milk", 115, "Germany", 120, DateTime.UtcNow.AddDays(-2), 3),
            new(3, "Milk", 12, "France", 150, DateTime.UtcNow.AddDays(1), 10),
            new(4, "Milk", 144, "UK", 80, DateTime.UtcNow.AddDays(-1), 22),
            new(5, "Milk", 444, "USA", 100, DateTime.UtcNow, 5),
            new(6, "Bread", 2128, "Ukraine", 20, DateTime.UtcNow.AddDays(10), 6),
            new(7, "Bread", 555, "Poland", 30, DateTime.UtcNow.AddDays(20), 7),
            new(8, "Bread", 66, "Romania", 35, DateTime.UtcNow.AddDays(-2), 4),
            new(9, "Potato", 664, "Ukraine", 5, DateTime.UtcNow.AddDays(20), 5),
            new(10, "Potato", 45444455, "Poland", 8, DateTime.UtcNow, 10)
        };
    }
    public void Add(Product product)
    {
        _products.Add(product);
    }

    public void Add(List<Product> products)
    {
        _products.AddRange(products);
    }

    public List<Product> GetByName(string name)
    {
        return _products.Where(p => p.Name == name).ToList();
    }

    public List<Product> GetByNameBelowOrEqPrice(string name, int price)
    {
        return _products.Where(p => p.Name == name && p.Price <= price).ToList();
    }

    public List<Product> GetOverdue(DateTime overdue)
    {
        return _products.Where(p => p.BestBefore < overdue).ToList();
    }
}