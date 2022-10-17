var count = 0;

var barrier = new Barrier(4, (b) =>
{
    Console.WriteLine("Post-Phase action: count={0}, phase={1}", count, b.CurrentPhaseNumber);
});

void Work()
{
    Interlocked.Increment(ref count);
    barrier.SignalAndWait();
    Interlocked.Increment(ref count);
    barrier.SignalAndWait();
    Interlocked.Increment(ref count);
    barrier.SignalAndWait();
    Interlocked.Increment(ref count);
    barrier.SignalAndWait();
}

Parallel.Invoke(Work, Work, Work, Work);