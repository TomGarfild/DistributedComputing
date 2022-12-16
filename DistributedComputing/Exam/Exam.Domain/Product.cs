namespace Exam.Domain
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Upc { get; set; }
        public string Manufacturer { get; set; }
        public int Price { get; set; }
        public DateTime BestBefore { get; set; }
        public int Count { get; set; }

        public Product(int id, string name, int upc, string manufacturer, int price, DateTime bestBefore, int count)
        {
            Id = id;
            Name = name;
            Upc = upc;
            Manufacturer = manufacturer;
            Price = price;
            BestBefore = bestBefore;
            Count = count;
        }
    }
}