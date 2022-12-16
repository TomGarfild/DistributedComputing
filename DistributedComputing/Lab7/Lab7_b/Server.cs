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
        _studio.Start(true);
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
                        var id = _studio.AddArtist(@params[1]);
                        message = id.ToString();
                        break;
                    case "DELETE_ARTIST":
                        _studio.DeleteArtist(Guid.Parse(@params[1]));
                        message = $"deleted {@params[1]}";
                        break;
                    case "UPDATE_ARTIST":
                        _studio.UpdateArtist(Guid.Parse(@params[1]), @params[2]);
                        message = $"updated {@params[1]}";
                        break;
                    case "ADD_ALBUM":
                        var albumId = _studio.AddAlbum(@params[1], @params[2], int.Parse(@params[3]), Guid.Parse(@params[4]));
                        message = albumId.ToString();
                        break;
                    case "COUNT_ALBUMS":
                        var count = _studio.CountAlbumsForArtist(Guid.Parse(@params[1]));
                        message = count.ToString();
                        break;
                    case "GET_ALBUMS_BY_ARTIST":
                        var albums = _studio.GetAlbumsForArtist(Guid.Parse(@params[1]));
                        message = string.Join(',', albums.Select(a => a.Id));
                        break;
                    case "GET_ARTISTS":
                        var artists = _studio.GetAllArtists();
                        message = string.Join(',', artists.Select(a => a.Id));
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