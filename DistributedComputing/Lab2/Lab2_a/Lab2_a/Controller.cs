namespace Lab2_a;

public class Controller
{
    public bool[,] Field { get; }
    private volatile bool _bearFound;
    private int _currentArea = -1;

    public Controller()
    {
        Field = new bool[Constants.FieldSize, Constants.FieldSize];
    }

    public void SetBear(int x, int y)
    {
        Field[y, x] = true;
    }

    public int GetArea()
    {
        return _bearFound ? -1 : Interlocked.Increment(ref _currentArea) % Constants.FieldSize;
    }

    public void BearIsFound()
    {
        _bearFound = true;
    }
}