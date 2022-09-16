using Lab2_b;
using System.Threading.Channels;

const int messageLimit = 5;
var wayPoint = Channel.CreateBounded<Weapon>(messageLimit);
var van = Channel.CreateBounded<Weapon>(messageLimit);

var ivanov = new Producer<Weapon>(wayPoint) { Name = "Ivanov" };
var petrovConsumer = new Consumer<Weapon>(wayPoint) { Name = "Petrov" };
var petrovProducer = new Producer<Weapon>(van) { Name = "Petrov" };
var nechiporchuk = new Consumer<Weapon>(van) { Name = "Nechiporchuk" };

var data = new List<Weapon>(100);
for (var i = 0; i < data.Capacity; i++)
{
    data.Add(new Weapon());
}

var ivanovTask = ivanov.Produce(data);
var petrovTask = petrovConsumer.Consume(async weapon => await petrovProducer.Produce(weapon));
var sum = 0;
var nechiporchukTask = nechiporchuk.Consume(weapon => Task.Run(() => Interlocked.Add(ref sum, weapon.Price)));

await ivanovTask;
await petrovTask;
await nechiporchukTask;

Console.ReadKey();

Console.WriteLine($"Price of all weapons: {sum}");