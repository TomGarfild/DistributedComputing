using System.Xml.Serialization;

namespace Lab7.Domain.Models;

public class Artist
{
    [XmlAttribute]
    public Guid Id { get; set; }
    [XmlAttribute]
    public string Name { get; set; }
    [XmlArray]
    public Album[] Albums { get; set; }

    public Artist()
    {
    }
    public Artist(Guid id, string name, Album[] albums)
    {
        Id = id;
        Name = name;
        Albums = albums;
    }
}