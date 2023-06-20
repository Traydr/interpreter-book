using Monkey.Core.Lexer;
using Monkey.Core.Parser;

namespace Monkey.Test;

public class ParserTests
{
    [Test]
    public void TestLetStatement()
    {
        string input = """
                          let x = 5;
                          let y = 10;
                          let foobar = 838383;
                       """;
        Lexer lexer = new Lexer(input);
        Parser parser = new Parser(lexer);
        Ast program = parser.ParseProgram();
        TestUtils.CheckParserErrors(parser.Errors());

        if (program.Statements.Count != 3)
        {
            Assert.Fail(
                $"Expected 3 elements, got {program.Statements.Count}");
        }

        List<string> expectedIdents = new List<string>() { "x", "y", "foobar" };

        for (int i = 0; i < expectedIdents.Count; i++)
        {
            TestUtils.CompareLetStatement(program.Statements[i], expectedIdents[i]);
        }

        Assert.Pass();
    }

    [Test]
    public void TestReturnStatement()
    {
        string input = """
                          return 5;
                          return 10;
                          return 993322;
                       """;
        Lexer lexer = new Lexer(input);
        Parser parser = new Parser(lexer);
        Ast program = parser.ParseProgram();
        TestUtils.CheckParserErrors(parser.Errors());

        if (program.Statements.Count != 3)
        {
            Assert.Fail(
                $"Expected 3 elements, got {program.Statements.Count}");
        }

        foreach (var statement in program.Statements)
        {
            if (statement.GetType() != typeof(ReturnStatement))
            {
                Assert.Fail($"Expected return Type, got {statement.GetType()}");
            }

            if (statement.Token.Literal != "return")
            {
                Assert.Fail($"Expected return Literal, got {statement.Token.Literal}");
            }
        }

        Assert.Pass();
    }
}