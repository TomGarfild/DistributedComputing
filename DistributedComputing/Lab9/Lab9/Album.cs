namespace Lab9;

public class Album
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Genre { get; set; }
    public int Year { get; set; }
    public string ArtistName { get; set; }

    public Album(Guid id, string name, string genre, int year, string artistName)
    {
        Id = id;
        Name = name;
        Genre = genre;
        Year = year;
        ArtistName = artistName;
    }
}