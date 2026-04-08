using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public enum TokenType { ID, NUMBER, IF, ELSE, OP, EOF }

public class Token
{
    public TokenType Type;
    public string Value;
    public int Line;
    public int Column;

    public Token(TokenType t, string v, int line, int col)
    {
        Type = t;
        Value = v;
        Line = line;
        Column = col;
    }
}

public static class Lexer
{
    public static List<Token> Tokenize(string input)
    {
        List<Token> tokens = new List<Token>();
        int line = 1, col = 1;

        var regex = new Regex(@"\s+|[A-Za-z_]\w*|\d+|==|=|>|<|\+|-|\*|/");

        foreach (Match m in regex.Matches(input))
        {
            string v = m.Value;

            if (Regex.IsMatch(v, @"^\s+$"))
            {
                col += v.Length;
                continue;
            }

            TokenType type;

            if (v == "if") type = TokenType.IF;
            else if (v == "else") type = TokenType.ELSE;
            else if (Regex.IsMatch(v, @"^\d+$")) type = TokenType.NUMBER;
            else if (Regex.IsMatch(v, @"^[A-Za-z_]\w*$")) type = TokenType.ID;
            else type = TokenType.OP;

            tokens.Add(new Token(type, v, line, col));
            col += v.Length;
        }

        tokens.Add(new Token(TokenType.EOF, "", line, col));
        return tokens;
    }
}
