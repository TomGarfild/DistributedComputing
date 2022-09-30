using System.Text;

namespace Lab4_b;

public class Garden
{
    private readonly int _size;
    private readonly bool[,] _field;
    private readonly Random _rand = new();
    private readonly object _lock = new ();
    private const int Duration = 5000;

    private const string Path = "MonitorResult.txt";

    public Garden(int size)
    {
        _size = size;
        _field = new bool[size, size];
        ChangeField(() => true);
    }
    public void Water()
    {
        ChangeField(() => true);
    }

    public void NaturalChanges()
    {
        ChangeField(() => _rand.Next()%2 == 0);
    }

    private void ChangeField(Func<bool> func)
    {
        lock (_lock)
        {
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    _field[i,j] = func();
                }
            }
            Thread.Sleep(Duration);
        }
    }

    public void Print()
    {
        lock (_lock)
        {
            Console.WriteLine(FieldAsString());
        }
    }

    public void OutputToFile()
    {
        lock (_lock)
        {
            using var writer = File.AppendText(Path);
            writer.WriteLine(FieldAsString());
            writer.Flush();
        }
    }

    private string FieldAsString()
    {
        var strBuilder = new StringBuilder();
        for (var i = 0; i < _size; i++)
        {
            for (var j = 0; j < _size; j++)
            {
                strBuilder.Append(_field[i, j] ? "W" : "D"); // W - watered, D - dry
            }

            strBuilder.AppendLine();
        }

        return strBuilder.ToString();
    }
}