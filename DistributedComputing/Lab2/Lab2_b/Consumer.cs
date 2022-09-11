using System.Threading.Channels;

namespace Lab2_b;

public class Consumer<TData>
{
    public string Name { get; init; } = "Consumer";
    public int Success = 0;

    private readonly ChannelReader<TData> _reader;
    private readonly int _count;

    public Consumer(ChannelReader<TData> reader) : this(reader, 5) {}

    public Consumer(ChannelReader<TData> reader, int count)
    {
        _reader = reader;
        _count = count;
    }

    public Task Consume(Action<TData> action)
    {
        var consumer = new Task[_count];
        for (var i = 0; i < consumer.Length; i++)
        {
            consumer[i] = Task.Factory.StartNew(async () => {
                while (await _reader.WaitToReadAsync())
                {
                    if (_reader.TryRead(out var data))
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"{Name} gets {data}");
                        Interlocked.Increment(ref Success);

                        action(data);
                    }
                }
            });
        }

        return Task.WhenAll(consumer);
    }
}