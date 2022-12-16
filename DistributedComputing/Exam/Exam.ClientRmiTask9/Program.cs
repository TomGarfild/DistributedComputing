using Grpc.Net.Client;

namespace Exam.ClientRmiTask9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7286");
            var client = new Supermarket.SupermarketClient(channel);

            var reply = client.GetByName(new GetByNameRequest { Name = "Milk" });
            Console.WriteLine("Milk: " + reply.Ids);
            var reply2 = client.GetByName(new GetByNameRequest { Name = "Bread" });
            Console.WriteLine("Bread: " + reply2.Ids);
            var reply3 = client.GetByName(new GetByNameRequest { Name = "Potato" });
            Console.WriteLine("Potato: " + reply3.Ids);

            var reply4 = client.GetByNameBelowOrEqPrice(new GetByNameBelowOrEqPriceRequest { Name = "Milk", Price = 100 });
            Console.WriteLine("Milk, <=100: " + reply4.Ids);
            var reply5 = client.GetOverdue(new GetOverdueRequest
            {
                Day = 16, Month = 12, Year = 2022
            });
            Console.WriteLine("Today: " + reply5.Ids);
        }
    }
}