namespace Lab2_b;

public class Weapon
{
    public Weapon()
    {
    }

    public Weapon(string name, Color color, int price)
    {
        Name = name;
        Color = color;
        Price = price;
    }

    public string Name { get; init; } = Faker.NameFaker.Name();
    public Color Color { get; init; } = Faker.EnumFaker.SelectFrom<Color>();
    public int Price { get; init; } = 2;


    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Color)}: {Color}, {nameof(Price)}: {Price}";
    }
}