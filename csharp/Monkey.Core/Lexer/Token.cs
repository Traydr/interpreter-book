namespace Monkey.Core.Lexer;

public record Token(TokenType Type, string Literal)
{
    private static readonly Dictionary<string, TokenType> Keywords = new()
    {
        { "fn", TokenType.Function },
        { "let", TokenType.Let },
        { "true", TokenType.True },
        { "false", TokenType.False },
        { "if", TokenType.If },
        { "else", TokenType.Else },
        { "return", TokenType.Return },
    };

    public Token(TokenType type, char literal) : this(type, literal.ToString())
    {
    }

    public static TokenType LookupIdent(string ident)
    {
        return Keywords.TryGetValue(ident, out TokenType tokenType) ? tokenType : TokenType.Ident;
    }

    public override string ToString()
    {
        return $"Type:{Type} Literal:{Literal}";
    }
}