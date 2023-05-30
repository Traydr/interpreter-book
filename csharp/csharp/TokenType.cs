﻿namespace csharp;

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
}