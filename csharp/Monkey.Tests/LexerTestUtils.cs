using System.Text;
using Monkey.Core;
using Monkey.Core.Lexer;
using Monkey.Core.Parser;

namespace Monkey.Test;

public static class LexerTestUtils
{
    // Lexer Test Utils

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
                Assert.Fail(
                    $"Tests[{index}] - TestTokenType Wrong. Expected:{expect.Type}, Got:{token.Type}");
            }

            if (token.Literal != expect.Literal)
            {
                Assert.Fail(
                    $"Tests[{index}] - Literal Wrong. Expected:{expect.Literal}, Got:{token.Literal}");
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


}