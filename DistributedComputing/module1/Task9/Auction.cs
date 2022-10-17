namespace Task9;

public class Auction
{
    private const int BiddersCount = 5;
    private const int LotsCount = 4;
    
    public static volatile bool StartedLot = false;

    public static Lot CurrentLot;
    private static List<Bidder> _bidders = new ();


    public static void Start()
    {
        for (var i = 0; i < BiddersCount; i++)
        {
            var bidder = new Bidder(i);
            _bidders.Add(bidder);
            new Thread(bidder.Start).Start();
        }

        Thread.Sleep(1000);
        for (var i = 0; i < LotsCount; i++)
        {
            Console.WriteLine("STARTED LOT: " + i);
            CurrentLot = new Lot(i);
            CurrentLot.Start();
            foreach (var bidder in _bidders)
            {
                bidder.ResetBid();
            }
        }
    }
}