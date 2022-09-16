namespace Lab2_a;

public class Bee
{
    private readonly Controller _controller;

    public Bee(Controller controller)
    {
        _controller = controller;
    }

    public Task StartSearch(CancellationToken cancellationToken = default)
    {
        return Task.Factory.StartNew(() =>
        {
            while (true)
            {
                var area = _controller.GetArea();
                if (area == -1)
                {
                    Console.WriteLine($"Bee {Task.CurrentId} finished search");
                    break;
                }

                Console.WriteLine($"Bee {Task.CurrentId} started search in {area} area");

                for (var i = 0; i < Constants.FieldSize; i++)
                {
                    if (_controller.Field[area, i])
                    {
                        _controller.BearIsFound();
                        Console.WriteLine($"Bee {Task.CurrentId} found bear at ({area}, {i})");
                        break;
                    }
                }
            }
        }, cancellationToken);
    }
}