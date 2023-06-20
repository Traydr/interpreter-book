using System.Text;
using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

public interface INode
{
    Token Token { get; set; }
}

public interface IStatement : INode
{
}

public interface IExpression : INode
{
}

public class Identifier : IExpression
{
    public required Token Token { get; set; }
    public required string Value { get; set; }

    public Identifier(Token token, string value)
    {
        Token = token;
        Value = value;
    }

    public override string ToString() => Value;
}

public class LetStatement : IStatement
{
    public required Token Token { get; set; }
    public required Identifier Name { get; set; }
    public IExpression? Value { get; set; }

    public LetStatement(Token token, Identifier name, IExpression? value)
    {
        Token = token;
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        var builder = new StringBuilder($"{Token.Literal} {Name} = ");
        if (Value is not null)
        {
            builder.Append(Value);
        }

        builder.Append(';');
        return builder.ToString();
    }
}

public class ReturnStatement : IStatement
{
    public Token Token { get; set; }
    public IExpression? Expression { get; set; }

    public override string ToString()
    {
        var builder = new StringBuilder($"{Token.Literal} ");
        if (Expression is not null)
        {
            builder.Append(Expression);
        }

        builder.Append(';');
        return builder.ToString();
    }
}

public class ExpressionStatement : IStatement
{
    public Token Token { get; set; }
    public IExpression? Expression { get; set; }

    public override string ToString()
    {
        return Expression?.ToString() ?? "";
    }
}

public class Ast
{
    public List<IStatement> Statements { get; private set; } = new();

    public Ast(IEnumerable<IStatement>? statements)
    {
        if (statements is not null)
        {
            Statements.AddRange(statements);
        }
    }

    public override string ToString()
    {
        var builder = new StringBuilder();

        foreach (var statement in Statements)
        {
            builder.Append(statement);
        }

        return builder.ToString();
    }
}