using System.Text;
using Monkey.Core;
using Monkey.Core.Lexer;
using Monkey.Core.Parser;

namespace Monkey.Test;

public static class TestUtils
{
    /// <summary>
    /// Compares a list of tokens, and checks they are in the same order
    /// </summary>
    /// <param name="expected">List of tokens that are expected</param>
    /// <param name="input">Input to be passed to a lexer</param>
    public static void CompareExpectedTokens(List<Token> expected, string input)
    {
        Lexer lexer = new Lexer(input);

        int index = 0;
        foreach (var expect in expected)
        {
            var token = lexer.NextToken();

            if (token.Type != expect.Type)
            {
                Assert.Fail($"Tests[{index}] - TestTokenType Wrong. Expected:{expect.Type}, Got:{token.Type}");
            }

            if (token.Literal != expect.Literal)
            {
                Assert.Fail($"Tests[{index}] - Literal Wrong. Expected:{expect.Literal}, Got:{token.Literal}");
            }

            index++;
        }
    }

    /// <summary>
    /// Checks any given statement to be a letStatement with the correct name,
    /// if anything is incorrect, then the test fails
    /// </summary>
    /// <param name="statement">Statement to check</param>
    /// <param name="name"></param>
    public static void CompareLetStatement(IStatement statement, string name)
    {
        if (statement.Token.Literal != "let")
        {
            Assert.Fail($"Expected let Literal, got {statement.Token.Literal}");
        }

        if (statement.GetType() != typeof(LetStatement))
        {
            Assert.Fail($"Expected let Type, got {statement.GetType()}");
        }

        LetStatement letStatement = (statement as LetStatement)!;

        if (letStatement.Name.Value != name)
        {
            Assert.Fail($"Expected Name.Value = {name}, got {letStatement.Name.Value}");
        }

        if (letStatement.Name.Token.Literal != name)
        {
            Assert.Fail(
                $"Expected Name.TokenLiteral = {name}, got {letStatement.Name.Token.Literal}");
        }
    }

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

    public static void TestIntegerLiteral(IExpression expression, long value)
    {
        if (expression.GetType() != typeof(IntegerLiteral))
        {
            Assert.Fail($"Expected IntegerLiteral, got {expression.GetType()}");
        }


        IntegerLiteral literal = (IntegerLiteral) expression;
        if (literal.Value != value)
        {
            Assert.Fail($"Expected {value}, got {literal.Value}");
        }

        if (literal.Token.Literal != value.ToString())
        {
            Assert.Fail($"Expected {value}, got {literal.Token.Literal}");
        }
    }
}