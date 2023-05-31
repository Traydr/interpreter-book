namespace csharp.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestSymbols()
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

        TestUtils.CompareExpectedTokens(tokens, input);
        Assert.Pass();
    }

    [Test]
    public void TestNextToken()
    {
        string input = """
                            let five = 5;
                            let ten = 10;

                            let add = fn(x, y) {
                              x + y;
                            };

                            let result = add(five, ten);
                       """;

        List<Token> tokens = new List<Token>()
        {
            new Token(TokenType.Let, "let"),
            new Token(TokenType.Ident, "five"),
            new Token(TokenType.Assign, '='),
            new Token(TokenType.Int, "5"),
            new Token(TokenType.Semicolon, ';'),
            new Token(TokenType.Let, "let"),
            new Token(TokenType.Ident, "ten"),
            new Token(TokenType.Assign, '='),
            new Token(TokenType.Int, "10"),
            new Token(TokenType.Semicolon, ';'),
            new Token(TokenType.Let, "let"),
            new Token(TokenType.Ident, "add"),
            new Token(TokenType.Assign, '='),
            new Token(TokenType.Function, "fn"),
            new Token(TokenType.Lparen, '('),
            new Token(TokenType.Ident, "x"),
            new Token(TokenType.Comma, ','),
            new Token(TokenType.Ident, "y"),
            new Token(TokenType.Rparen, ')'),
            new Token(TokenType.Lsquirly, '{'),
            new Token(TokenType.Ident, "x"),
            new Token(TokenType.Plus, '+'),
            new Token(TokenType.Ident, "y"),
            new Token(TokenType.Semicolon, ';'),
            new Token(TokenType.Rsquirly, '}'),
            new Token(TokenType.Semicolon, ';'),
            new Token(TokenType.Let, "let"),
            new Token(TokenType.Ident, "result"),
            new Token(TokenType.Assign, '='),
            new Token(TokenType.Ident, "add"),
            new Token(TokenType.Lparen, '('),
            new Token(TokenType.Ident, "five"),
            new Token(TokenType.Comma, ','),
            new Token(TokenType.Ident, "ten"),
            new Token(TokenType.Rparen, ')'),
            new Token(TokenType.Semicolon, ';'),
            new Token(TokenType.Eof, ""),
        };

        TestUtils.CompareExpectedTokens(tokens, input);
        Assert.Pass();
    }
}