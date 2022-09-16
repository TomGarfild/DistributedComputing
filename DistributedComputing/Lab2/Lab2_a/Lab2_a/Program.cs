using Lab2_a;


var rand = new Random();
var controller = new Controller();
controller.SetBear(rand.Next(Constants.FieldSize), rand.Next(Constants.FieldSize));
var bees = new List<Bee>();

for (var i = 0; i < Constants.BeesCount; i++)
{
    bees.Add(new Bee(controller));
}

var tasks = bees.Select(bee => bee.StartSearch()).ToList();

Task.WhenAll(tasks);

Console.ReadKey();