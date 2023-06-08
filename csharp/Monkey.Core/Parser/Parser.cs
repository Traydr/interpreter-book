using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

public class Parser
{
    private Lexer.Lexer _lexer;
    private Token _currentToken;
    private Token _peekToken;

    public Parser(string input)
    {
        _lexer = new Lexer.Lexer(input);

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
        return new Ast.Program(Array.Empty<Ast.Statement>());
    }
}