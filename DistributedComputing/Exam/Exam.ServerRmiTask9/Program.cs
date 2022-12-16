using Exam.ServerRmiTask9.Services;
using SupermarketServ = Exam.Server.Supermarket;

namespace Exam.ServerRmiTask9
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Additional configuration is required to successfully run gRPC on macOS.
            // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddSingleton<SupermarketServ>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<SupermarketService>();

            app.Run();
        }
    }
}