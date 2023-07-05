using Monkey.Core.Lexer;

namespace Monkey.Core.Parser;

using InfixFn = Func<IExpression, IExpression>;
using PrefixFn = Func<IExpression>;

enum Precedence
{
    Lowest,
    Equals, // ==
    LessGreater, // > or <
    Sum, // +
    Product, // *
    Prefix, // -X or !X
    Call, // myFunction(X)
}

public class Parser
{
    private readonly Lexer.Lexer _lexer;
    private Token _currentToken;
    private Token _peekToken;

    private readonly List<string> _errors;
    private readonly Dictionary<TokenType, PrefixFn> _prefixParseFns;
    private readonly Dictionary<TokenType, InfixFn> _infixParseFns;
    private readonly Dictionary<TokenType, Precedence> _precedences;

    public Parser(Lexer.Lexer lexer)
    {
        _lexer = lexer;
        _errors = new List<string>();

        _currentToken = new Token(TokenType.Illegal, "");
        _peekToken = new Token(TokenType.Illegal, "");

        _prefixParseFns = new Dictionary<TokenType, PrefixFn>()
        {
            { TokenType.Ident, ParseIdentifier },
            { TokenType.Int, ParseIntegerLiteral },
            { TokenType.Bang, ParsePrefixExpression },
            { TokenType.Minus, ParsePrefixExpression },
        };
        _infixParseFns = new Dictionary<TokenType, InfixFn>()
        {
            { TokenType.Plus, ParseInfixExpression },
            { TokenType.Minus, ParseInfixExpression },
            { TokenType.ForwardSlash, ParseInfixExpression },
            { TokenType.Asterisk, ParseInfixExpression },
            { TokenType.Equal, ParseInfixExpression },
            { TokenType.NotEqual, ParseInfixExpression },
            { TokenType.LessThan, ParseInfixExpression },
            { TokenType.GreaterThan, ParseInfixExpression },
        };
        _precedences = new Dictionary<TokenType, Precedence>()
        {
            { TokenType.Equal, Precedence.Equals },
            { TokenType.NotEqual, Precedence.Equals },
            { TokenType.LessThan, Precedence.LessGreater },
            { TokenType.GreaterThan, Precedence.LessGreater },
            { TokenType.Plus, Precedence.Sum },
            { TokenType.Minus, Precedence.Sum },
            { TokenType.ForwardSlash, Precedence.Sum },
            { TokenType.Asterisk, Precedence.Sum },
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

    private void NoPrefixParseFnError(TokenType type)
    {
        string message = $"No prefix parse function for {type} found";
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
            _ => ParseExpressionStatement(),
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

        return new LetStatement { Token = token, Name = name };
    }


    private ReturnStatement ParseReturnStatement()
    {
        ReturnStatement statement = new ReturnStatement { Token = _currentToken };
        NextToken();

        while (!IsCurrentTokenOfType(TokenType.Semicolon))
        {
            NextToken();
        }

        return statement;
    }

    private ExpressionStatement ParseExpressionStatement()
    {
        ExpressionStatement statement = new ExpressionStatement
        {
            Token = _currentToken,
            Expression = ParseExpression(Precedence.Lowest)
        };

        while (IsPeekTokenOfType(TokenType.Semicolon))
        {
            NextToken();
        }

        return statement;
    }

    private IExpression? ParseExpression(Precedence precedence)
    {
        if (!_prefixParseFns.TryGetValue(_currentToken.Type, out PrefixFn prefix))
        {
            NoPrefixParseFnError(_currentToken.Type);
            return null;
        }

        var leftExp = prefix();

        while (!IsPeekTokenOfType(TokenType.Semicolon) && precedence < PeekPrecedence())
        {
            if (!_infixParseFns.TryGetValue(_currentToken.Type, out InfixFn infix))
            {
                return leftExp;
            }

            NextToken();
            leftExp = infix(leftExp);
        }

        return leftExp;
    }

    private IExpression ParseIdentifier()
    {
        return new Identifier() { Token = _currentToken, Value = _currentToken.Literal };
    }

    private IExpression ParseIntegerLiteral()
    {
        if (!long.TryParse(_currentToken.Literal, out long parsed))
        {
            string msg = $"Couldn't parse {_currentToken.Literal} as integer";
            _errors.Add(msg);
        }

        return new IntegerLiteral()
        {
            Token = _currentToken,
            Value = parsed
        };
    }

    private IExpression ParsePrefixExpression()
    {
        PrefixExpression expression = new PrefixExpression
        {
            Token = _currentToken,
            Operator = _currentToken.Literal,
            Right = null
        };

        NextToken();

        expression.Right = ParseExpression(Precedence.Prefix);
        return expression;
    }

    private IExpression ParseInfixExpression(IExpression left)
    {
        InfixExpression expression = new InfixExpression
        {
            Token = _currentToken,
            Left = left,
            Operator = _currentToken.Literal,
            Right = null
        };

        Precedence precedence = CurrentPrecedence();
        NextToken();
        expression.Right = ParseExpression(precedence);
        return expression;
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

    private Precedence PeekPrecedence()
    {
        bool isPrecedence = _precedences.TryGetValue(_peekToken.Type, out Precedence precedence);
        return isPrecedence ? precedence : Precedence.Lowest;
    }

    private Precedence CurrentPrecedence()
    {
        bool isPrecedence = _precedences.TryGetValue(_currentToken.Type, out Precedence precedence);
        return isPrecedence ? precedence : Precedence.Lowest;
    }
}