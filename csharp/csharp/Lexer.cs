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
    }

    public Token NextToken()
    {
        return new Token(TokenType.Illegal, "");
    }

    public void ReadChar()
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
}