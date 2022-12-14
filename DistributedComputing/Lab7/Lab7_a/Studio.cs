using System.Xml.Serialization;
using Lab7_a.Models;

namespace Lab7_a;

[XmlRoot(nameof(Studio))]
public class Studio
{
    private List<Artist> _artists = new();
    private List<Album> _albums = new();

    public Studio()
    {
    }

    public Studio(List<Artist> artists, List<Album> albums)
    {
        _artists = artists;
        _albums = albums;
    }

    public void Load(string fileName = "studio.xml")
    {
        var xmlSerializer = new XmlSerializer(typeof(Artist[]));
        using var fs = new FileStream(fileName, FileMode.OpenOrCreate);
        _artists = (xmlSerializer.Deserialize(fs) as Artist[])?.ToList() ?? new List<Artist>();
        _albums = _artists.SelectMany(a => a.Albums).ToList();
    }

    public void Save(string fileName = "studio.xml")
    {
        var xmlSerializer = new XmlSerializer(typeof(Artist[]));
        using var fs = new FileStream(fileName, FileMode.OpenOrCreate);
        xmlSerializer.Serialize(fs, _artists.ToArray());
    }

    public Artist AddArtist(string name)
    {
        var artist = new Artist(Guid.NewGuid(), name, Array.Empty<Album>());
        _artists.Add(artist);
        return artist;
    }

    public Artist GetArtist(Guid id)
    {
        return _artists.FirstOrDefault(a => a.Id == id);
    }

    public void UpdateArtist(Guid id, string name)
    {
        var artist = GetArtist(id);

        if (artist == null)
        {
            Console.WriteLine($"Artist with {id} does not exist");
            return;
        }

        artist.Name = name;
    }

    public void DeleteArtist(Guid id)
    {
        var artist = GetArtist(id);

        if (artist == null)
        {
            Console.WriteLine($"Artist with {id} does not exist");
            return;
        }

        _artists.Remove(artist);
        DeleteAlbums(id);
    }

    public void AddAlbum(Guid artistId, string name, string genre, int year)
    {
        var artist = GetArtist(artistId);

        if (artist == null)
        {
            Console.WriteLine($"Artist with {artistId} does not exist");
            return;
        }
        
        _albums.Add(new Album(Guid.NewGuid(), artistId, name, genre, year));
        artist.Albums = _albums.Where(a => a.ArtistId == artistId).ToArray();
    }

    public Album GetAlbum(Guid id)
    {
        return _albums.FirstOrDefault(a => a.Id == id);
    }

    public void DeleteAlbum(Guid id)
    {
        var album = GetAlbum(id);

        if (album == null)
        {
            Console.WriteLine($"Album with {id} does not exist");
            return;
        }

        _albums.Remove(album);
        var artist = _artists.FirstOrDefault(a => a.Albums.Any(al => al.Id == id));
        if (artist == null) return;
        artist.Albums = _albums.Where(a => a.ArtistId == artist.Id).ToArray();
    }

    public int DeleteAlbums(Guid artistId)
    {
        return _albums.RemoveAll(a => a.ArtistId == artistId);
    }

    public void Print()
    {
        foreach (var artist in _artists)
        {
            Console.WriteLine($"{artist.Name}:");
            foreach (var album in artist.Albums)
            {
                Console.WriteLine($"\t{album.Name}; {album.Genre}; {album.Year}");
            }
        }

        Console.WriteLine();
    }
}