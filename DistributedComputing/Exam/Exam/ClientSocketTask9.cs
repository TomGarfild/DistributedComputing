using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Exam;

public class ClientSocketTask9 : IDisposable
{

    private readonly Socket _socket;
    private const string Address = "127.0.0.1";
    private const int Port = 8005;
    public ClientSocketTask9()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }


    public void Start()
    {
        try
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            _socket.Connect(ipPoint);
            
            _socket.Send(Encoding.Unicode.GetBytes("GET_BY_NAME|Milk"));
            var ids = GetMessage();
            Console.WriteLine(ids);
            
            _socket.Send(Encoding.Unicode.GetBytes("GET_BY_NAME|Bread"));
            ids = GetMessage();
            Console.WriteLine(ids);
            
            _socket.Send(Encoding.Unicode.GetBytes("GET_BY_NAME|Potato"));
            ids = GetMessage();
            Console.WriteLine(ids);
            
            _socket.Send(Encoding.Unicode.GetBytes("GET_BY_NAME_PRICE|Milk|100"));
            ids = GetMessage();
            Console.WriteLine(ids);
            
            _socket.Send(Encoding.Unicode.GetBytes($"GET_OVERDUE|{DateTime.Today}"));
            ids = GetMessage();
            Console.WriteLine($"{ids} overdue {DateTime.Today}");
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

    private string GetMessage()
    {
        var data = new byte[256];

        var bytes = _socket.Receive(data, data.Length, 0);
        return Encoding.Unicode.GetString(data, 0, bytes);
    }

    public void Dispose()
    {
        _socket?.Close();
    }
}