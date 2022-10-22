using System.Text;

const int count = 10;
const string characters = "ABCD";

Dictionary<char, char> replacable = new Dictionary<char, char>()
{
    {'A', 'C'},
    {'C', 'A'},
    {'B', 'D'},
    {'D', 'B'}
};

var values = new StringBuilder[4];
for (var i = 0; i < values.Length; i++)
{
    values[i] = new StringBuilder(Faker.StringFaker.SelectFrom(count, characters));
}

var success = false;

var barrier = new Barrier(4, (b) =>
{
    Console.WriteLine($"Step={b.CurrentPhaseNumber}");
    var groups = values.Select(v => v.ToString().Count(c => c is 'A' or 'B')).GroupBy(v => v);
    foreach (var group in groups)
    {
        if (group.Count() >= 3)
        {
            foreach (var value in values)
            {
                Console.WriteLine(value);
            }
            success = true;
        }
    }
});

void Work(StringBuilder sb)
{
    while (!success)
    {

        if (success)
        {
            break;
        }
        
        var kv = replacable.ElementAt(Random.Shared.Next(replacable.Count));

        for (var i = 0; i < sb.Length; i++)
        {
            if (sb[i] == kv.Key)
            {
                sb[i] = kv.Value;
            }
        }

        // sb[i] = (char)('A' + (sb[i] - 'A' + 2) % 4);
        barrier.SignalAndWait();
    }
    //for (var i = 0; i < sb.Length; i++)
    //{
    //    if (success)
    //    {
    //        break;
    //    }

    //    if (sb[i] == 'C')
    //    {
    //        sb[i] = 'A';
    //    }
    //    else if (sb[i] == 'D')
    //    {
    //        sb[i] = 'B';
    //    }
    //    barrier.SignalAndWait();
    //}
}

Parallel.ForEach(values, Work);