using System.Threading.Channels;

namespace Lab2_b;

[Obsolete]
public class ProducerConsumer<TData>
{
    public string Name { get; init; } = "Producer";
    public int _success = 0;


    private readonly ChannelWriter<TData> _writer;
    private readonly ChannelReader<TData> _reader;
    private readonly int _count;

    public ProducerConsumer(ChannelWriter<TData> writer, ChannelReader<TData> reader) : this(writer, reader, 5) { }

    public ProducerConsumer(ChannelWriter<TData> writer, ChannelReader<TData> reader, int count)
    {
        _writer = writer;
        _reader = reader;
        _count = count;
    }

    public Task Process()
    {
        var processors = new Task[_count];
        for (var i = 0; i < processors.Length; i++)
        {
            processors[i] = Task.Factory.StartNew(async () => {
                while (await _reader.WaitToReadAsync())
                {
                    if (_reader.TryRead(out var data))
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"{Name} gets {data}");


                        while (!await _writer.WaitToWriteAsync())
                        {
                            await Task.Delay(100);
                        }

                        if (_writer.TryWrite(data))
                        {
                            Console.WriteLine($"{Name} puts {data}");
                            Interlocked.Increment(ref _success);
                        }
                    }
                }
            });
        }

        return Task.WhenAll(processors);
    }
}