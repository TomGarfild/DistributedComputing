using System.Xml.Serialization;

namespace Lab7_a.Models;

public class Album
{
    [XmlAttribute]
    public Guid Id { get; set; }
    [XmlAttribute]
    public Guid ArtistId { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlAttribute]
    public string Genre { get; set; }
    [XmlAttribute]
    public int Year { get; set; }

    public Album()
    {
    }
    public Album(Guid id, Guid artistId, string name, string genre, int year)
    {
        Id = id;
        ArtistId = artistId;
        Name = name;
        Genre = genre;
        Year = year;
    }
}