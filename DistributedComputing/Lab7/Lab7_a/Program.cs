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
            studio.Print();

            studio.Save();
            studio.Load();

            studio.Print();
        }
    }
}