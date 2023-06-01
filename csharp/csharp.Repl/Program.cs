using csharp.Repl;

Repl repl = new Repl();
string user = Environment.UserName;

Console.WriteLine($"Hello {user}! This is the Monkey programming language!");
Console.WriteLine("Feel free to type in commands");

repl.Start();