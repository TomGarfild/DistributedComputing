using Lab7.Domain.Models;

namespace Lab7_b
{
    internal class Program
    {
        static void Main(string[] args)
        {
        //    var studio = new Studio();
        //    studio.Start(true);
        //    var eminemId = studio.AddArtist("Eminem");
        //    var nfId = studio.AddArtist("NF");
        //    Print(studio);

        //    studio.AddAlbum("The Eminem Show", "Rap", 2002, eminemId);
        //    var albumId = studio.AddAlbum("The Marshall Mathers LP", "Rap", 2000, eminemId);
        //    Print(studio);

        //    studio.UpdateArtist(eminemId, "Slim Shady");
        //    Print(studio);
            
        //    studio.DeleteAlbum(albumId);
        //    Print(studio);

        //    albumId = studio.AddAlbum("The Search", "Rap", 2019, nfId);
        //    Print(studio);
        //    studio.UpdateAlbum(albumId, "The Search(2)", "Rap", 2019);
        //    var album = studio.GetAlbum(albumId);
        //    Console.WriteLine($"{album.Name}; {album.Genre}; {album.Year}");
        //    studio.DeleteArtist(nfId);
        //    Print(studio);


        //    var server = new Server();
        //    server.Start();

            var server = new RabbitMqServer();
            server.Start();
        }

        static void Print(Studio studio)
        {
            var artists = studio.GetAllArtists();
            for (int i = 0; i < artists.Count; i++)
            {
                Console.WriteLine($"{i}. {artists[i].Name}");
                var albums = artists[i].Albums.Length == 0 ? artists[i].Albums.ToList() : studio.GetAlbumsForArtist(artists[i].Id);
                foreach (var album in albums)
                {
                    Console.WriteLine($"\t{album.Name}; {album.Genre}; {album.Year}");
                }
            }

            Console.WriteLine();
        }
    }
}