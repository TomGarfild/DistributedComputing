using Lab4_a;

var dataBase = new DataBase();

dataBase.Write(new User("Oleksii", "S", "S", "1235"));
dataBase.Write(new User("Someone", "P", "I", "1234"));
dataBase.Write(new User("Someone2", "F", "A", "1233"));

var selector = new Selector(dataBase);
var editor = new Editor(dataBase);

new Thread(() => selector.Select(user => user.PhoneNumber == "1234", "Phone number eq 1234")).Start();
new Thread(() => selector.Select(user => user.FullName == "Oleksii S S", "Full name is Oleksii S S")).Start();

var rand = new Random();

new Thread(() => editor.Add(() => new User(Faker.NameFaker.FirstName(), Faker.StringFaker.Alpha(1).ToUpper(), Faker.StringFaker.Alpha(1).ToUpper(), Faker.StringFaker.SelectFrom(4, "1234")))).Start();
new Thread(() => editor.Delete(user => user.PhoneNumber == $"123{rand.Next(0, 10)}")).Start();