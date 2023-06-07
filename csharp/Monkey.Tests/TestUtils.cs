using Monkey.Core;

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
}