using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

public class Ast
{
    public record Node(Token Token);

    public record Statement(Token Token) : Node(Token)
    {
        public string TokenLiteral => Token.Literal;
    }

    public record Expression(Token Token) : Node(Token)
    {
        public string TokenLiteral => Token.Literal;
    }

    public record Program(List<Statement> Statements) : Node(new Token(TokenType.Eof, ""))
    {
        public string TokenLiteral => Statements.Any() ? Statements[0].TokenLiteral : "";
    }

    public record LetStatement
        (Token Token, Identifier Name, Expression Value) : Statement(Token)
    {
        public new string TokenLiteral => Token.Literal;
    }

    public record Identifier(Token Token, string Value) : Expression(Token)
    {
        public new string TokenLiteral => Token.Literal;
    }
}