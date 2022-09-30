namespace Lab4_b;

public class Gardener
{
    private readonly Garden _garden;

    public Gardener(Garden garden)
    {
        _garden = garden;
    }

    public void Start()
    {
        while (true)
        {
            _garden.Water();
            Console.WriteLine("Gardener finished watering plants");
        }
    }
}