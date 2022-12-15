using System.Xml.Serialization;

namespace Lab7_a
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var studio = new Studio();
            studio.Load();

            studio.Print();
            var artist = studio.AddArtist("Adele");
            studio.AddAlbum(artist.Id, "21", "Pop", 2011);
            var album = studio.AddAlbum(artist.Id, "30", "Pop", 2020);
            studio.Print();

            studio.UpdateArtist(artist.Id, "Adele new");
            studio.Print();

            studio.DeleteAlbum(album.Id);
            studio.Print();

            var artist2 = studio.AddArtist("Adele2");
            studio.AddAlbum(artist2.Id, "21", "Pop", 2011);
            studio.AddAlbum(artist2.Id, "30", "Pop", 2020);
            studio.Print();
            studio.DeleteArtist(artist2.Id);
            studio.Print();

            studio.Save();
            studio.Load();

            studio.Print();
        }
    }
}