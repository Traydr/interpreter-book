namespace csharp;

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
                tok = new Token(TokenType.Assign, _ch);
                break;
            case ';':
                tok = new Token(TokenType.Semicolon, _ch);
                break;
            case '(':
                tok = new Token(TokenType.Lparen, _ch);
                break;
            case ')':
                tok = new Token(TokenType.Rparen, _ch);
                break;
            case ',':
                tok = new Token(TokenType.Comma, _ch);
                break;
            case '+':
                tok = new Token(TokenType.Plus, _ch);
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

    private string ReadIdentifier()
    {
        int position = _position;
        while (IsLetter(_ch))
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
        return char.IsLetter(c) || c == '_';
    }
}