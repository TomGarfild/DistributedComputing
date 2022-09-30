namespace Lab4_b;

public class Nature
{

    private readonly Garden _garden;

    public Nature(Garden garden)
    {
        _garden = garden;
    }

    public void Start()
    {
        while (true)
        {
            _garden.NaturalChanges();
            Console.WriteLine("Nature finished processes");
        }
    }
}