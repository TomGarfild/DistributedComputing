using Lab7.Domain.Models;
using Microsoft.Data.Sqlite;

namespace Lab7_b;

public class Studio : IDisposable
{
    private readonly SqliteConnection _connection;
    private const string ArtistsTable = "Artists";
    private const string AlbumsTable = "Albums";

    public Studio(string dbName = "studio.db")
    {
        _connection = new SqliteConnection($"Data Source={dbName}");
    }

    public void Start(bool @new = false)
    {
        _connection.Open();
        if (@new)
        {
            DropTable(AlbumsTable);
            DropTable(ArtistsTable);
        }
        CreateTable(ArtistsTable, "Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, Name TEXT NOT NULL");
        CreateTable(AlbumsTable, "Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, Name TEXT NOT NULL, Genre TEXT, Year INT, ArtistId UNIQUEIDENTIFIER NOT NULL");
    }

    private void CreateTable(string name, string columns)
    {
        ExecuteCommand($"CREATE TABLE IF NOT EXISTS {name}({columns})");
    }

    private void DropTable(string name)
    {
        ExecuteCommand($"DROP TABLE IF EXISTS {name}");
    }

    public Guid AddArtist(string name)
    {
        var id = Guid.NewGuid();
        ExecuteCommand($"INSERT INTO {ArtistsTable} (Id, Name) VALUES ('{id}', '{name}')");
        return id;
    }

    public void UpdateArtist(Guid id, string name)
    {
        ExecuteCommand($"UPDATE {ArtistsTable} SET Name='{name}' WHERE Id='{id}'");
    }

    public void DeleteArtist(Guid id)
    {
        ExecuteCommand($"DELETE FROM {ArtistsTable} WHERE Id='{id}'");
        var albums = GetAlbumsForArtist(id);
        foreach (var album in albums)
        {
            DeleteAlbum(album.Id);
        }
    }

    public Artist GetArtist(Guid id)
    {
        var rdr = ExecuteQuery($"SELECT * FROM {ArtistsTable} WHERE Id='{id}' LIMIT 1");
        rdr.Read();
        return new Artist(rdr.GetGuid(0), rdr.GetString(1), GetAlbumsForArtist(id).ToArray());
    }

    public List<Artist> GetAllArtists()
    {
        var rdr = ExecuteQuery($"SELECT * FROM {ArtistsTable}");
        var res = new List<Artist>();
        while (rdr.Read())
        {
            var id = rdr.GetGuid(0);
            res.Add(new Artist(id, rdr.GetString(1), GetAlbumsForArtist(id).ToArray()));
        }

        return res;
    }

    public Guid AddAlbum(string name, string genre, int year, Guid artistId)
    {
        var id = Guid.NewGuid();
        ExecuteCommand($"INSERT INTO {AlbumsTable} (Id, Name, Genre, Year, ArtistId) VALUES ('{id}', '{name}', '{genre}', {year}, '{artistId}')");
        return id;
    }

    public void UpdateAlbum(Guid id, string name, string genre, int year)
    {
        ExecuteCommand($"UPDATE {AlbumsTable} SET Name='{name}', Genre='{genre}', Year='{year}' WHERE Id='{id}'");
    }

    public void DeleteAlbum(Guid id)
    {
        ExecuteCommand($"DELETE FROM {AlbumsTable} WHERE Id='{id}'");
    }

    public Album GetAlbum(Guid id)
    {
        var rdr = ExecuteQuery($"SELECT * FROM {AlbumsTable} WHERE Id='{id}' LIMIT 1");
        rdr.Read();
        return new Album(rdr.GetGuid(0), rdr.GetGuid(4), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3));
    }

    public List<Album> GetAlbumsForArtist(Guid artistId)
    {
        var rdr = ExecuteQuery($"SELECT * FROM {AlbumsTable} WHERE ArtistId='{artistId}'");
        var res = new List<Album>();
        while (rdr.Read())
        {
            res.Add(new Album(rdr.GetGuid(0), rdr.GetGuid(4), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3)));
        }

        return res;
    }

    public List<Album> GetAllAlbums()
    {
        var rdr = ExecuteQuery($"SELECT * FROM {AlbumsTable}");
        var res = new List<Album>();
        while (rdr.Read())
        {
            res.Add(new Album(rdr.GetGuid(0), rdr.GetGuid(4), rdr.GetString(1), rdr.GetString(2), rdr.GetInt32(3)));
        }

        return res;
    }

    public void ExecuteCommand(string text)
    {
        var cmd = new SqliteCommand(text, _connection);
        cmd.ExecuteNonQuery();
    }

    public SqliteDataReader ExecuteQuery(string text)
    {
        var cmd = new SqliteCommand(text, _connection);
        return cmd.ExecuteReader();
    }

    public void Dispose()
    {
        _connection.Close();
        _connection.Dispose();
    }
}