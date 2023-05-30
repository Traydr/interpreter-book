namespace csharp;

public class Lexer
{
    public readonly string _input;

    public Lexer(string input)
    {
        _input = input;
    }

    public Token NextToken()
    {
        return new Token(TokenType.Illegal, "");
    }
}