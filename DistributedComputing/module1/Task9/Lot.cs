namespace Task9;

public class Lot
{
    public int Id { get; }
    public Bidder Bidder { get; private set; }

    public Lot(int i)
    {
        Id = i;
        Bidder = new Bidder(-1);
    }

    public void Start()
    {
        Auction.StartedLot = true;
        Thread.Sleep(5000);
        Auction.StartedLot = false;

        Console.WriteLine($"Stopped Lot {Id}");

        Console.WriteLine($"Bidder {Bidder.Id} proposed the biggest bid {Bidder.GetBid()}");
        Bidder.MakePayment();
    }

    public bool RaiseBid(Bidder bidder)
    {
        lock (Bidder)
        {
            if (bidder.GetBid() > Bidder.GetBid() && Auction.StartedLot)
            {
                Bidder = bidder;
                return true;
            }
        }

        return false;
    }
}