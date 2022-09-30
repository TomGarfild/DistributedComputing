namespace Lab4_a;

public class Selector
{
    private readonly DataBase _dataBase;

    public Selector(DataBase dataBase)
    {
        _dataBase = dataBase;
    }

    public void Select(Func<User, bool> predicate, string queryName)
    {
        while (true)
        {
            Thread.Sleep(5000);
            var user = _dataBase.Get(predicate);

            Console.WriteLine(user == null
                ? $"Have not found user by {queryName}"
                : $"Found user by {queryName}: {user}");
        }
    }
}