namespace Lab8_a
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new Client();
            client.Start();
            Console.ReadKey();
        }
    }
}