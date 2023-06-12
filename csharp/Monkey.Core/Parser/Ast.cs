using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

public class Ast
{
    public record Node(Token Token);

    public record Statement(Token Token) : Node(Token)
    {
        public string TokenLiteral => Token.Literal;
        public virtual string String => Token.Literal;
    }

    public record Expression(Token Token) : Node(Token)
    {
        public string TokenLiteral => Token.Literal;
        public virtual string String => Token.Literal;
    }

    public record Program(List<Statement> Statements) : Node(new Token(TokenType.Eof, ""))
    {
        public string TokenLiteral => Statements.Any() ? Statements[0].TokenLiteral : "";
        public string String => String.Join("", Statements.Select(x => x.String));
    }

    public record Identifier(Token Token, string Value) : Expression(Token)
    {
        public new string TokenLiteral => Token.Literal;
        public override string String => $"{Value}";
    }

    public record LetStatement
        (Token Token, Identifier Name, Expression Value) : Statement(Token)
    {
        public new string TokenLiteral => Token.Literal;
        public override string String => $"{Token.Literal} {Name.String} = {Value.String};";
    }

    public record ReturnStatement(Token Token, Expression ReturnValue) : Statement(Token)
    {
        public new string TokenLiteral => Token.Literal;
        public override string String => $"{Token.Literal} {ReturnValue.String};";
    }

    public record ExpressionStatement(Token Token, Expression Expression) : Statement(Token)
    {
        public new string TokenLiteral => Token.Literal;
        public override string String => $"{Expression.String};";
    }
}