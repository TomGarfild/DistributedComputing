using Lab3_a;

var honeyPot = new HoneyPot(4);
var bear = new Bear(honeyPot);

for (var i = 0; i < 5; i++)
{
    var bee = new Bee(honeyPot, i);
    var thread = new Thread(bee.FillHoney);
    thread.Start();
}