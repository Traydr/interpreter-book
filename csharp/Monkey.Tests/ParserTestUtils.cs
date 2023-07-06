using System.Text;
using Monkey.Core.Parser;

namespace Monkey.Test;

public static class ParserTestUtils
{
    /// <summary>
    /// Checks through the errors given ( if any ) and fails the test
    /// </summary>
    /// <param name="errors">List of errors from the parser</param>
    public static void CheckParserErrors(List<string> errors)
    {
        if (errors.Count == 0)
        {
            return;
        }

        StringBuilder errorBuilder = new StringBuilder();
        errorBuilder.AppendLine($"parser has {errors.Count} errors:");
        foreach (var error in errors)
        {
            errorBuilder.Append("parser error: ");
            errorBuilder.AppendLine(error);
        }

        Assert.Fail(errorBuilder.ToString());
    }

    /// <summary>
    /// Tests that the IExpression passed to it is an IntegerLiteral and is equal to the 2nd argument
    /// </summary>
    /// <param name="expression">Any IExpression</param>
    /// <param name="value">Expected long value</param>
    public static void TestIntegerLiteral(IExpression? expression, long value)
    {
        if (expression != null && expression.GetType() != typeof(IntegerLiteral))
        {
            Assert.Fail($"Expected IntegerLiteral, got {expression.GetType()}");
        }


        IntegerLiteral literal = (IntegerLiteral)expression!;
        if (literal.Value != value)
        {
            Assert.Fail($"Expected {value}, got {literal.Value}");
        }

        if (literal.Token.Literal != value.ToString())
        {
            Assert.Fail($"Expected {value}, got {literal.Token.Literal}");
        }
    }

    public static void TestIdentifier(IExpression expression, string value)
    {
        if (expression.GetType() != typeof(Identifier))
        {
            Assert.Fail($"Expected Identifier, got {expression.GetType()}");
        }

        Identifier identifier = (Identifier)expression;
        if (identifier.Value != value)
        {
            Assert.Fail($"Expected identifier.Value {value}, got {identifier.Value}");
        }

        if (identifier.Token.Literal != value)
        {
            Assert.Fail($"Expected identifier.Token.Literal {value}, got {identifier.Token.Literal}");
        }
    }

    public static void TestLiteralExpression(IExpression? expression, Object expected)
    {
        switch (expected)
        {
            case int castedInt:
                TestIntegerLiteral(expression!, castedInt);
                break;
            case long castedLong:
                TestIntegerLiteral(expression!, castedLong);
                break;
            case string castedString:
                TestIdentifier(expression!, castedString);
                break;
            default:
                Assert.Fail($"Type of expression not handled: {expression?.GetType()}");
                break;
        }
    }

    public static void TestInfixExpression(IExpression expression, Object left, string op, Object right)
    {
        if (expression.GetType() != typeof(InfixExpression))
        {
            Assert.Fail($"Expected InfixExpression, got {expression.GetType()}");
        }

        InfixExpression infixExpression = (InfixExpression)expression;
        TestLiteralExpression(infixExpression.Left, left);

        if (infixExpression.Operator != op)
        {
            Assert.Fail($"Expected {op}, got {infixExpression.Operator}");
        }

        TestLiteralExpression(infixExpression.Right, right);
    }
}