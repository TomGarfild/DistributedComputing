using System.Threading.Channels;

namespace Lab2_b;

public class Producer<TData>
{
    public string Name { get; init; } = "Producer";
    public int _success = 0;

    private readonly ChannelWriter<TData> _writer;

    public Producer(ChannelWriter<TData> writer)
    {
        _writer = writer;
    }

    public Task Produce(List<TData> data)
    {
        return Task.Factory.StartNew(async () =>
        {
            foreach (var v in data)
            {
                await Produce(v);
            }

            _writer.Complete();
        });
    }

    public async Task Produce(TData data)
    {
        while (!await _writer.WaitToWriteAsync())
        {
            await Task.Delay(100);
        }

        if (_writer.TryWrite(data))
        {
            Console.WriteLine($"{Name} puts {data}");
            Interlocked.Increment(ref _success);
        }
        else
        {
            Console.WriteLine($"Error {Name}");
        }
    }
}