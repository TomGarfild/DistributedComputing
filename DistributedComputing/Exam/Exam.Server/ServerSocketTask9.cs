using System.Net.Sockets;
using System.Net;
using System.Text;
using Exam.Domain;

namespace Exam.Server;

public class ServerSocketTask9 : IDisposable
{
    private readonly Socket _socket;
    private const string Address = "127.0.0.1";
    private const int Port = 8005;
    private readonly Supermarket _supermarket;
    public ServerSocketTask9()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _supermarket = new Supermarket();
    }

    public void Start()
    {
        try
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            _socket.Bind(ipPoint);
            _socket.Listen(10);


            while (true)
            {
                var clientSocket = _socket.Accept();
                var thread = new Thread(() => StartClient(clientSocket));
                thread.Start();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Dispose();
        }
    }

    private void StartClient(Socket clientSocket)
    {
        while (true)
        {
            var data = new byte[256];

            var bytes = clientSocket.Receive(data, data.Length, 0);
            var msg = Encoding.Unicode.GetString(data, 0, bytes);

            var @params = msg.Split('|');
            var command = @params[0].ToUpper();
            var message = "";

            switch (command)
            {
                case "GET_BY_NAME":
                    var ids = _supermarket.GetByName(@params[1]).Select(p => p.Id);
                    message = string.Join(',', ids);
                    break;
                case "GET_BY_NAME_PRICE":
                    var ids2 = _supermarket.GetByNameBelowOrEqPrice(@params[1], int.Parse(@params[2])).Select(p => p.Id);
                    message = string.Join(',', ids2);
                    break;
                case "GET_OVERDUE":
                    var ids3 = _supermarket.GetOverdue(DateTime.Parse(@params[1])).Select(p => p.Id);
                    message = string.Join(',', ids3);
                    break;
            }

            clientSocket.Send(Encoding.Unicode.GetBytes(message));
        }
    }

    public void Dispose()
    {
        _socket?.Close();
    }
}