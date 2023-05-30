namespace csharp;

public record Token(TokenType Type, string Literal)
{
    public Token(TokenType type, char literal) : this(type, literal.ToString())
    {

    }
}