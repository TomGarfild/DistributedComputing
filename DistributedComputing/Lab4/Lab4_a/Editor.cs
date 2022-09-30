namespace Lab4_a;

public class Editor
{
    private readonly DataBase _dataBase;

    public Editor(DataBase dataBase)
    {
        _dataBase = dataBase;
    }
    public void Add(Func<User> userFunc)
    {
        while (true)
        {
            Thread.Sleep(2000);
            var user = userFunc();
            _dataBase.Write(user);
            Console.WriteLine($"Editor have added {user}");
        }
    }

    public void Delete(Predicate<User> predicate)
    {
        while (true)
        {
            Thread.Sleep(3000);
            var count = _dataBase.Delete(predicate);
            Console.WriteLine($"Editor have deleted {count}");
        }
    }
}