using Lab4_b;
using Lab4_b.Monitors;

var garden = new Garden(10);

var gardener = new Gardener(garden);
var nature = new Nature(garden);
var consoleMonitor = new ConsoleMonitor(garden);
var fileMonitor = new FileMonitor(garden);

new Thread(gardener.Start).Start();
new Thread(nature.Start).Start();
new Thread(consoleMonitor.Start).Start();
new Thread(fileMonitor.Start).Start();