using Monkey.Core.Lexer;

namespace Monkey.Core.Ast;

public class Ast
{
    public record Node(Token Token);

    public record Statement(Token Token) : Node(Token)
    {
        public string TokenLiteral() => Token.Literal;
    }

    public record Expression(Token Token) : Node(Token)
    {

    }
}