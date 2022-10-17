using System.Text;

const int count = 3;
const string characters = "ABCD";

var values = new StringBuilder[4];
for (var i = 0; i < values.Length; i++)
{
    values[i] = new StringBuilder(Faker.StringFaker.SelectFrom(count, characters));
}

var success = false;

var barrier = new Barrier(4, (b) =>
{
    if (b.CurrentPhaseNumber == 500000)
    {
        Console.WriteLine("Oh fuck");
    }
    Console.WriteLine($"Phase action, phase={b.CurrentPhaseNumber}");
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
//    while (!success)
//    {
//        for (var i = 0; i < sb.Length; i++)
//        {
//            if (success)
//            {
//                break;
//            }

//            sb[i] = (char)('A' + (sb[i] - 'A' + 2) % 4);
//            barrier.SignalAndWait();
//        }
//    }

        for (var i = 0; i < sb.Length; i++)
        {
            if (success)
            {
                break;
            }

            if (sb[i] == 'C')
            {
                sb[i] = 'A';
            }
            else if (sb[i] == 'D')
            {
                sb[i] = 'B';
            }
            barrier.SignalAndWait();
        }
}

Parallel.ForEach(values, Work);