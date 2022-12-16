namespace Lab9;

public class Studio
{
    private readonly List<Album> _albums;

    public Studio()
    {
        _albums = new List<Album>()
        {
            new(Guid.NewGuid(), "The Eminem Show", "Rap", 2002, "Eminem"),
            new(Guid.NewGuid(), "The Marshall Mathers LP", "Rap", 2000, "Eminem"),
            new(Guid.NewGuid(), "Kamikaze", "Rap", 2018, "Eminem"),
            new(Guid.NewGuid(), "Music to Be Murdered By", "Rap", 2020, "Eminem"),
            new(Guid.NewGuid(), "The Search", "Rap", 2019, "NF"),
            new(Guid.NewGuid(), "Clouds (The Mixtape)", "Rap", 2021, "NF"),
            new(Guid.NewGuid(), "Legends Never Die", "Rap", 2020, "Juice WRLD"),
        };
    }

    public List<Album> GetAlbums()
    {
        return _albums;
    }
}