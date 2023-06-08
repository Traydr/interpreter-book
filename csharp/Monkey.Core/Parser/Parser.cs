using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

public class Parser
{
    private Lexer.Lexer _lexer;
    private Token? _currentToken;
    private Token? _peekToken;

    public Parser(Lexer.Lexer lexer)
    {
        _lexer = lexer;

        NextToken();
        NextToken();
    }

    private void NextToken()
    {
        _currentToken = _peekToken;
        _peekToken = _lexer.NextToken();
    }

    public Ast.Program ParseProgram()
    {
        Ast.Program program = new Ast.Program(new List<Ast.Statement>());

        return program;
    }
}