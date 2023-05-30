namespace csharp.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        string input = "=+(){},;";

        List<Token> tokens = new List<Token>()
        {
            new Token(TokenType.Assign, '='),
            new Token(TokenType.Plus, '+'),
            new Token(TokenType.Lparen, '('),
            new Token(TokenType.Rparen, ')'),
            new Token(TokenType.Lsquirly, '{'),
            new Token(TokenType.Rsquirly, '}'),
            new Token(TokenType.Comma, ','),
            new Token(TokenType.Semicolon, ';'),
            new Token(TokenType.Eof, ""),
        };

        Lexer lexer = new Lexer(input);

        int index = 0;
        foreach (var expected in tokens)
        {
            var token = lexer.NextToken();

            if (token.Type != expected.Type)
            {
                Assert.Fail($"Tests[{index}] - TestTokenType Wrong. Expected:{expected.Type}, Got:{token.Type}");
            }

            if (token.Literal != expected.Literal)
            {
                Assert.Fail($"Tests[{index}] - Literal Wrong. Expected:{expected.Literal}, Got:{token.Literal}");
            }

            index++;
        }

        Assert.Pass();
    }
}