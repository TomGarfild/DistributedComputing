using Grpc.Net.Client;

namespace Lab8_b.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:7158");
            var client = new Studio.StudioClient(channel);


            var eminemId = client.AddArtist(new AddArtistRequest() { Name = "Eminem" });
            Console.WriteLine($"Eminem: {eminemId.Id}");
            var nfId = client.AddArtist(new AddArtistRequest() { Name = "NF" });
            Console.WriteLine($"NF: {nfId.Id}");

            Console.WriteLine(client.GetArtists(new GetArtistsRequest()).Ids);

            client.DeleteArtist(new DeleteArtistRequest() { Id = nfId.Id });
            client.UpdateArtist(new UpdateArtistRequest() { Id = eminemId.Id, Name = "Slim Shady" });

            var album1Id = client.AddAlbum(new AddAlbumRequest()
                { ArtistId = eminemId.Id, Genre = "Rap", Name = "Kamikaze", Year = 2018 });
            Console.WriteLine($"Album1: {album1Id.Id}"); 

            var album2Id = client.AddAlbum(new AddAlbumRequest()
                { ArtistId = eminemId.Id, Genre = "Rap", Name = "Music to Be Murdered By", Year = 2020 });
            Console.WriteLine($"Album2: {album2Id.Id}");

            var count = client.CountAlbums(new CountAlbumsByArtistRequest() { ArtistId = eminemId.Id });
            Console.WriteLine(count.Count);

            var albums = client.GetAlbumsByArtist(new GetAlbumsByArtistRequest() { ArtistId = eminemId.Id });
            Console.WriteLine(albums.Ids);
        }
    }
}