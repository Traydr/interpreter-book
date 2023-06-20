using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

using InfixFn = Func<IExpression, IExpression>;
using PrefixFn = Func<IExpression>;

public class Parser
{
    private readonly Lexer.Lexer _lexer;
    private Token _currentToken;
    private Token _peekToken;
    private List<string> _errors;

    private Dictionary<TokenType, PrefixFn> _prefixParseFns;
    private Dictionary<TokenType, InfixFn> _infixParseFns;

    public Parser(Lexer.Lexer lexer)
    {
        _lexer = lexer;
        _errors = new List<string>();

        _currentToken = new Token(TokenType.Illegal, "");
        _peekToken = new Token(TokenType.Illegal, "");

        _prefixParseFns = new Dictionary<TokenType, PrefixFn>()
        {

        };
        _infixParseFns = new Dictionary<TokenType, InfixFn>()
        {

        };

        NextToken();
        NextToken();
    }

    /// <summary>
    /// Advances both the current and peek token
    /// </summary>
    private void NextToken()
    {
        _currentToken = _peekToken;
        _peekToken = _lexer.NextToken();
    }

    /// <summary>
    /// Iterates through all the tokens and turns all valid ones into statements or expressions
    /// </summary>
    /// <returns>A program with all valid statements</returns>
    public Ast ParseProgram()
    {
        List<IStatement> statements = new List<IStatement>();

        while (_currentToken.Type != TokenType.Eof)
        {
            IStatement? statement = ParseStatement();

            if (statement is not null)
            {
                statements.Add(statement);
            }

            NextToken();
        }

        return new Ast(statements);
    }

    /// <summary>
    /// Returns private list of errors
    /// </summary>
    /// <returns>List of errors</returns>
    public List<string> Errors()
    {
        return _errors;
    }

    /// <summary>
    /// Adds an error if the type of peek token is not the same as input
    /// </summary>
    /// <param name="type">Expected type</param>
    private void PeekError(TokenType type)
    {
        string message = $"Expected next token to be {type}, got {_peekToken.Type}";
        _errors.Add(message);
    }

    /// <summary>
    /// Parses statements bases on the token
    /// </summary>
    /// <returns>A statement or null if it was an invalid statement</returns>
    private IStatement? ParseStatement()
    {
        return _currentToken.Type switch
        {
            TokenType.Let => ParseLetStatement(),
            TokenType.Return => ParseReturnStatement(),
            _ => null
        };
    }

    private LetStatement? ParseLetStatement()
    {
        var token = _currentToken;
        if (!ExpectPeekTokenOfType(TokenType.Ident))
        {
            return null;
        }

        var name = new Identifier { Token = _currentToken, Value = _currentToken.Literal };
        if (!ExpectPeekTokenOfType(TokenType.Assign))
        {
            return null;
        }

        while (IsCurrentTokenOfType(TokenType.Semicolon))
        {
            NextToken();
        }

        // TODO This return should not include a null
        return new LetStatement { Token = token, Name = name };
    }


    private ReturnStatement? ParseReturnStatement()
    {
        // TODO This return should not include a null
        ReturnStatement statement = new ReturnStatement { Token = _currentToken };
        NextToken();

        while (!IsCurrentTokenOfType(TokenType.Semicolon))
        {
            NextToken();
        }

        return statement;
    }

    /// <summary>
    /// Checks if the current token is input type
    /// </summary>
    /// <param name="type">Type to be checked against</param>
    /// <returns>True if the types match, false otherwise</returns>
    private bool IsCurrentTokenOfType(TokenType type)
    {
        return _currentToken.Type == type;
    }

    /// <summary>
    /// Checks if the peek token is input type
    /// </summary>
    /// <param name="type">Type to be checked against</param>
    /// <returns>True if the types match, false otherwise</returns>
    private bool IsPeekTokenOfType(TokenType type)
    {
        return _peekToken.Type == type;
    }

    /// <summary>
    /// If the peek token is not of this type, then an error is added to the parsers list
    /// </summary>
    /// <param name="type">Expected type of peek token</param>
    /// <returns>True if types match, false otherwise</returns>
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