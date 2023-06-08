using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

public class Parser
{
    private readonly Lexer.Lexer _lexer;
    private Token _currentToken;
    private Token _peekToken;
    private List<string> _errors;

    public Parser(Lexer.Lexer lexer)
    {
        _lexer = lexer;
        _errors = new List<string>();

        _currentToken = new Token(TokenType.Illegal, "");
        _peekToken = new Token(TokenType.Illegal, "");

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

        while (_currentToken.Type != TokenType.Eof)
        {
            Ast.Statement? statement = ParseStatement();

            if (statement is not null)
            {
                program.Statements.Add(statement);
            }

            NextToken();
        }

        return program;
    }

    public List<string> Errors()
    {
        return _errors;
    }

    private void PeekError(TokenType type)
    {
        string message = $"Expected next token to be {type}, got {_peekToken.Type}";
        _errors.Add(message);
    }

    private Ast.Statement? ParseStatement()
    {
        switch (_currentToken.Type)
        {
            case TokenType.Let:
                return ParseLetStatement();
                break;
            default:
                return null;
                break;
        }
    }

    private Ast.LetStatement? ParseLetStatement()
    {
        var token = _currentToken;
        if (!ExpectPeekTokenOfType(TokenType.Ident))
        {
            return null;
        }

        var name = new Ast.Identifier(_currentToken, _currentToken.Literal);
        if (!ExpectPeekTokenOfType(TokenType.Assign))
        {
            return null;
        }

        while (IsCurrentTokenOfType(TokenType.Semicolon))
        {
            NextToken();
        }

        // TODO This return should not include a null
        return new Ast.LetStatement(token, name, null);
    }

    private bool IsCurrentTokenOfType(TokenType type)
    {
        return _currentToken.Type == type;
    }

    private bool IsPeekTokenOfType(TokenType type)
    {
        return _peekToken.Type == type;
    }

    private bool ExpectPeekTokenOfType(TokenType type)
    {
        if (IsPeekTokenOfType(type))
        {
            NextToken();
            return true;
        }

        PeekError(type);
        return false;
    }
}