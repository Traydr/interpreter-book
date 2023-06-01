using csharp.Core;

namespace csharp.Repl;

public class Repl
{
    private const string Prompt = ">> ";

    public void Start()
    {
        while (true)
        {
            Console.Write(Prompt);
            string input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            Lexer lexer = new Lexer(input);
            for (Token token = lexer.NextToken();
                 token.Type != TokenType.Eof;
                 token = lexer.NextToken())
            {
                Console.WriteLine(token);
            }
        }
    }
}