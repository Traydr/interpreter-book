using Monkey.Core.Lexer;
using Monkey.Core.Parser;

namespace Monkey.Test;

public class AstTest
{
    [Test]
    public void TestString()
    {
        Ast.Program program = new Ast.Program(new List<Ast.Statement>()
        {
            new Ast.LetStatement(new Token(TokenType.Let, "let"),
                new Ast.Identifier(new Token(TokenType.Ident, "myVar"), "myVar"),
                new Ast.Identifier(new Token(TokenType.Ident, "anotherVar"), "anotherVar")),
        });

        if (program.String != "let myVar = anotherVar;")
        {
            Assert.Fail($"program.String wrong. got {program.String}");
        }
    }
}