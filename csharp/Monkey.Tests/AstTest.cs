using Monkey.Core.Lexer;
using Monkey.Core.Parser;

namespace Monkey.Test;

public class AstTest
{
    [Test]
    public void TestString()
    {
        Ast program = new Ast(new List<IStatement>()
        {
            new LetStatement
            {
                Token = new Token(TokenType.Let, "let"),
                Name = new Identifier
                {
                    Token = new Token(TokenType.Ident, "myVar"),
                    Value = "myVar"
                },
                Value = new Identifier
                {
                    Token = new Token(TokenType.Ident, "anotherVar"),
                    Value = "anotherVar"
                }
            },
        });

        if (program.ToString() != "let myVar = anotherVar;")
        {
            Assert.Fail($"program.String wrong. got {program}");
        }
    }
}