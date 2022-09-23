namespace Lab3_a;

public class Bear
{
    private readonly HoneyPot _honeyPot;

    public Bear(HoneyPot honeyPot)
    {
        _honeyPot = honeyPot;
        _honeyPot.NotifiedWhenFull += Start;
    }

    public void Start(object sender, EventArgs args)
    {
        var thread = new Thread(_honeyPot.Eat);
        thread.Start();
    }
}