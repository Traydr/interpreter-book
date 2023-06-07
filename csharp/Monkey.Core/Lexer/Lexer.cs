namespace Monkey.Core.Lexer;

public class Lexer
{
    private readonly string _input;
    private int _position;
    private int _readPosition;
    private char _ch;

    public Lexer(string input)
    {
        _input = input;
        ReadChar();
    }

    public Token NextToken()
    {
        Token tok = new Token(TokenType.Illegal, "");
        SkipWhitespace();

        switch (_ch)
        {
            case '=':
                if (PeakChar() == '=')
                {
                    char ch = _ch;
                    ReadChar();
                    string literal = $"{ch}{_ch}";
                    tok = new Token(TokenType.Equal, literal);
                }
                else
                {
                    tok = new Token(TokenType.Assign, _ch);
                }

                break;
            case '+':
                tok = new Token(TokenType.Plus, _ch);
                break;
            case '-':
                tok = new Token(TokenType.Minus, _ch);
                break;
            case '!':
                if (PeakChar() == '=')
                {
                    char ch = _ch;
                    ReadChar();
                    string literal = $"{ch}{_ch}";
                    tok = new Token(TokenType.NotEqual, literal);
                }
                else
                {
                    tok = new Token(TokenType.Bang, _ch);
                }

                break;
            case '/':
                tok = new Token(TokenType.ForwardSlash, _ch);
                break;
            case '*':
                tok = new Token(TokenType.Asterisk, _ch);
                break;
            case '<':
                tok = new Token(TokenType.LessThan, _ch);
                break;
            case '>':
                tok = new Token(TokenType.GreaterThan, _ch);
                break;
            case ';':
                tok = new Token(TokenType.Semicolon, _ch);
                break;
            case ',':
                tok = new Token(TokenType.Comma, _ch);
                break;
            case '(':
                tok = new Token(TokenType.Lparen, _ch);
                break;
            case ')':
                tok = new Token(TokenType.Rparen, _ch);
                break;
            case '{':
                tok = new Token(TokenType.Lsquirly, _ch);
                break;
            case '}':
                tok = new Token(TokenType.Rsquirly, _ch);
                break;
            case '\0':
                tok = new Token(TokenType.Eof, "");
                break;
            default:
                if (IsLetter(_ch))
                {
                    string literal = ReadIdentifier();
                    tok = new Token(Token.LookupIdent(literal), literal);
                    return tok;
                }
                else if (IsDigit(_ch))
                {
                    tok = new Token(TokenType.Int, ReadNumber());
                    return tok;
                }
                else
                {
                    tok = new Token(TokenType.Illegal, _ch);
                }

                break;
        }

        ReadChar();
        return tok;
    }

    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(_ch))
        {
            ReadChar();
        }
    }

    private char PeakChar()
    {
        if (_readPosition >= _input.Length)
        {
            return '\0';
        }
        else
        {
            return _input[_readPosition];
        }
    }

    private string ReadIdentifier()
    {
        int position = _position;
        while (IsLetter(_ch))
        {
            ReadChar();
        }

        return _input.Substring(position, _position - position);
    }

    private string ReadNumber()
    {
        int position = _position;
        while (IsDigit(_ch))
        {
            ReadChar();
        }

        return _input.Substring(position, _position - position);
    }

    private void ReadChar()
    {
        if (_readPosition >= _input.Length)
        {
            _ch = '\0';
        }
        else
        {
            _ch = _input[_readPosition];
        }

        _position = _readPosition;
        _readPosition++;
    }

    private bool IsLetter(char c)
    {
        return char.IsAsciiLetter(c) || c == '_';
    }

    private bool IsDigit(char c)
    {
        return char.IsAsciiDigit(c);
    }
}