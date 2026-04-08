using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class Parser
{
    private List<Token> tokens;
    private int pos = 0;
    private DataGridView dgv;

    public Parser(string input, DataGridView dgvResults)
    {
        this.dgv = dgvResults;
        tokens = Lexer.Tokenize(input);
    }

    private Token Current => pos < tokens.Count ? tokens[pos] : new Token(TokenType.EOF, "", -1, -1);

    private void Scan() => pos++;

    private void Error(string message)
    {
        dgv.Rows.Add(Current.Value, $"строка {Current.Line}, позиция {Current.Column}", message);

        // Нейтрализация по Айронсу — пропуск до безопасного токена
        while (Current.Type != TokenType.EOF &&
               Current.Type != TokenType.ID &&
               Current.Type != TokenType.NUMBER)
        {
            Scan();
        }
    }

    public void Parse()
    {
        dgv.Rows.Clear();
        pos = 0;

        ParseStmt();

        if (dgv.Rows.Count == 0)
            MessageBox.Show("Ошибок не обнаружено");
    }

    private void ParseStmt()
    {
        ParseAssign();
    }

    private void ParseAssign()
    {
        if (Current.Type != TokenType.ID)
        {
            Error("Ожидался идентификатор");
            return;
        }

        Scan();

        if (Current.Value != "=")
        {
            Error("Ожидался символ '='");
            return;
        }

        Scan();
        ParseCond();
    }

    private void ParseCond()
    {
        ParseExpr();

        if (Current.Value != "if")
        {
            Error("Ожидалось ключевое слово 'if'");
            return;
        }

        Scan();
        ParseExpr();

        if (Current.Value != "else")
        {
            Error("Ожидалось ключевое слово 'else'");
            return;
        }

        Scan();
        ParseExpr();
    }

    private void ParseExpr()
    {
        if (Current.Type == TokenType.ID || Current.Type == TokenType.NUMBER)
        {
            Scan();
        }
        else
        {
            Error("Ожидалось выражение (идентификатор или число)");
        }
    }
}
