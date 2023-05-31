namespace csharp;

public record Token(TokenType Type, string Literal)
{
    private static readonly Dictionary<string, TokenType> _keywords = new()
    {
        { "fn", TokenType.Function },
        { "let", TokenType.Let },
    };

    public Token(TokenType type, char literal) : this(type, literal.ToString())
    {
    }

    public static TokenType LookupIdent(string ident)
    {
        return _keywords.TryGetValue(ident, out TokenType tokenType) ? tokenType : TokenType.Ident;
    }
}