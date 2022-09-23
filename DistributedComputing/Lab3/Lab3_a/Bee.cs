namespace Lab3_a;

public class Bee
{
    private readonly HoneyPot _honeyPot;
    private readonly int _id;
    public Bee(HoneyPot honeyPot, int id)
    {
        _honeyPot = honeyPot;
        _id = id;
    }

    public void FillHoney()
    {
        while (true)
        {
            _honeyPot.Fill(_id);
            Thread.Sleep(1000);
        }
    }
}