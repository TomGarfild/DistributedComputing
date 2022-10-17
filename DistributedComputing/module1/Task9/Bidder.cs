namespace Task9;

public class Bidder
{
    private int _currentBid = 100;
    private bool _skippedTheCurrentLot;
    private int _lotsToSkip;
    public int Id { get; }

    public Bidder(int i)
    {
        Id = i;
        if (i == -1)
        {
            _currentBid = 0;
        }
    }

    public void MakePayment()
    {
        if (Random.Shared.Next() % 2 == 0)
        {
            Console.WriteLine($"Customer {Id} could not make payment");
            _lotsToSkip = 1;
        }
    }
    public void RaiseBid()
    {
        _currentBid = (Auction.CurrentLot.Bidder?.GetBid() ?? 0) + 100;
        if (Auction.CurrentLot.RaiseBid(this))
        {
            Console.WriteLine($"Customer {Id} raised payment to {_currentBid}");
        }
    }

    public int GetBid()
    {
        return _currentBid;
    }

    public void ResetBid()
    {
        _currentBid = 100;
    }

    public void Start()
    {
        Console.WriteLine($"Started new bidder {Id}");
        while (true)
        {
            if (Auction.StartedLot)
            {
                if (!_skippedTheCurrentLot)
                {
                    if (_lotsToSkip > 0)
                    {
                        _lotsToSkip--;
                        _skippedTheCurrentLot = true;
                        Console.WriteLine($"Bidder {Id} skipped the current lot");
                        continue;
                    }

                    double chanceToRaise = Random.Shared.Next();
                    if (chanceToRaise % 10 <= 3)
                    {
                        RaiseBid();
                    }

                    Thread.Sleep(1000);
                }
            }
            else
            {
                if (_lotsToSkip > 0)
                {
                    _skippedTheCurrentLot = false;
                }
            }
        }
    }
}