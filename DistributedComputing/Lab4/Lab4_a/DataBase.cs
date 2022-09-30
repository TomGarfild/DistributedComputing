using System;
using System.IO;

namespace Lab4_a;

public class DataBase
{
    private const string Path = "data.txt";
    private readonly object _lock = new();
    private readonly List<User> _users = new();

    public DataBase()
    {
        Init();
    }

    public User Get(Func<User, bool> predicate)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(predicate);
        }
    }

    public void Write(User user)
    {
        lock (_lock)
        {
            _users.Add(user);
            using var writer = File.AppendText(Path);
            writer.WriteLine($"{_users.Count}.{user}");
            writer.Flush();
        }
    }

    public int Delete(Predicate<User> filter)
    {
        lock (_lock)
        {
            var deleted = _users.RemoveAll(filter);
            File.WriteAllText(Path, string.Empty);
            using var writer = File.AppendText(Path);
            var count = 1;
            foreach (var user in _users)
            {
                writer.WriteLine($"{count++}.{user}");
            }
            writer.Flush();

            return deleted;
        }
    }

    private void Init()
    {
        if (!File.Exists(Path))
        {
            return;
        }
        foreach (var line in File.ReadLines(Path))
        {
            var parts = line.Split('.')[1].Split('-');
            var name = parts[0].Split(' ');

            _users.Add(new User(name[0], name[1], name[2], parts[1]));
        }
    }
}