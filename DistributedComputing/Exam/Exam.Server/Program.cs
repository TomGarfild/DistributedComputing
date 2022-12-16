namespace Exam.Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var server = new ServerSocketTask9();
            server.Start();
        }
    }
}