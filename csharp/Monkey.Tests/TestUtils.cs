using Monkey.Core;
using Monkey.Core.Lexer;
using Monkey.Core.Parser;

namespace Monkey.Test;

public static class TestUtils
{
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

    public static void CompareLetStatement(Ast.Statement statement, string name)
    {

        if (statement.TokenLiteral != "let")
        {
            Assert.Fail($"Expected let Literal, got {statement.TokenLiteral}");
        }

        if (statement.GetType() != typeof(Ast.LetStatement))
        {
            Assert.Fail($"Expected let Type, got {statement.GetType()}");
        }

        Ast.LetStatement letStatement = (statement as Ast.LetStatement)!;

        if (letStatement.Name.Value != name)
        {
            Assert.Fail($"Expected Name.Value = {name}, got {letStatement.Name.Value}");
        }

        if (letStatement.Name.TokenLiteral != name)
        {
            Assert.Fail(
                $"Expected Name.TokenLiteral = {name}, got {letStatement.Name.TokenLiteral}");
        }
    }
}