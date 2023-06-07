namespace Monkey.Core.Lexer;

public enum TokenType
{
    Illegal,
    Eof,

    // Identifiers and Literals
    Ident, // add, foobar, x, y, ...
    Int, // 1343456

    // Operators
    Assign, // =
    Plus, // +
    Minus, // -
    Bang, // !
    Asterisk, // *
    ForwardSlash, // /

    LessThan, // <
    GreaterThan, // >
    Equal, // ==
    NotEqual, // !=

    // Delimiters
    Comma, // ,
    Semicolon, // ;

    Lparen, // (
    Rparen, // )
    Lsquirly, // {
    Rsquirly, // }

    // Keywords
    Function,
    Let,
    True,
    False,
    If,
    Else,
    Return,
}