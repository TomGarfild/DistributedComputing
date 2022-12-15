using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab7_b;

public class Server : IDisposable
{
    private readonly Socket _socket;
    private const string Address = "127.0.0.1";
    private const int Port = 8005;
    private readonly Studio _studio;
    public Server()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _studio = new Studio();
    }

    public void Start()
    {
        try
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            _socket.Bind(ipPoint);
            _socket.Listen(1);
            var clientSocket = _socket.Accept();

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
                    case "ADD_ARTIST":
                        
                        break;
                    case "DELETE_ARTIST":
                        break;
                    case "UPDATE_ARTIST":
                        break;
                    case "ADD_ALBUM":
                        break;
                    case "COUNT_ALBUMS":
                        break;
                    case "GET_ALBUMS_BY_ARTIST":
                        break;
                    case "GET_ARTISTS":
                        break;

                }

                clientSocket.Send(Encoding.Unicode.GetBytes(message));
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

    public void Dispose()
    {
        _socket?.Close();
    }
}