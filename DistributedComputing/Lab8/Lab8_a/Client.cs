using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Lab8_a;

public class Client : IDisposable
{

    private readonly Socket _socket;
    private const string Address = "127.0.0.1";
    private const int Port = 8005;
    public Client()
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }


    public void Start()
    {
        try
        {
            var ipPoint = new IPEndPoint(IPAddress.Parse(Address), Port);
            _socket.Connect(ipPoint);


            // Add artist
            _socket.Send(Encoding.Unicode.GetBytes("ADD_ARTIST|EMINEM"));
            var eminemId = GetMessage();
            Console.WriteLine(eminemId);

            _socket.Send(Encoding.Unicode.GetBytes("ADD_ARTIST|NF"));
            var nfId = GetMessage();
            Console.WriteLine(nfId);

            _socket.Send(Encoding.Unicode.GetBytes("GET_ARTISTS"));
            var artists = GetMessage();
            Console.WriteLine(artists);


            _socket.Send(Encoding.Unicode.GetBytes($"DELETE_ARTIST|{nfId}"));
            var status = GetMessage();
            Console.WriteLine(status);

            _socket.Send(Encoding.Unicode.GetBytes($"UPDATE_ARTIST|{eminemId}|SLIM shady"));
            status = GetMessage();
            Console.WriteLine(status);

            _socket.Send(Encoding.Unicode.GetBytes($"ADD_ALBUM|Kamikaze|Rap|2018|{eminemId}"));
            var  albumId = GetMessage();
            Console.WriteLine(albumId);

            _socket.Send(Encoding.Unicode.GetBytes($"ADD_ALBUM|Music to Be Murdered By|Rap|2020|{eminemId}"));
            albumId = GetMessage();
            Console.WriteLine(albumId);

            _socket.Send(Encoding.Unicode.GetBytes($"COUNT_ALBUMS|{eminemId}"));
            var count = GetMessage();
            Console.WriteLine(count);

            _socket.Send(Encoding.Unicode.GetBytes($"GET_ALBUMS_BY_ARTIST|{eminemId}"));
            var albums = GetMessage();
            Console.WriteLine(albums);
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