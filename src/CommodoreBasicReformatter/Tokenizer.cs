﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CommodoreBasicReformatter
{
    public enum TokenKind
    {
        Digit,
        Colon,
        Keyword,
        Symbol,
        String,
        NewLine,
        Name,
        EOF,
    }

    public class Token
    {
        public TokenKind Type;
        public string Value;

        public Token(TokenKind type, string value)
        {
            Type = type;
            Value = value;
        }

        public bool IsRemark()
        {
            return Type == TokenKind.Keyword && Value.StartsWith("rem");
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class Tokenizer
    {
        public string Text;
        public int pos { get; private set; }

        public Tokenizer(string text)
        {
            Text = text.Replace("\r\n", "\n").Replace("\r", "\n");
            pos = 0;
        }

        public List<Token> ReadAll()
        {
            var result = new List<Token>();
            do
            {
                result.Add(Next());
            } while (result.Last().Type != TokenKind.EOF);

            return result;
        }

        Token Next()
        {
            SkipSpaces();

            Token t;



            if ((t = ParseEOf()) != null)
                return t;
            if ((t = ParseNewline()) != null)
                return t;
            if ((t = ParseString()) != null)
                return t;
            if ((t = ParseDigit()) != null)
                return t;
            if ((t = ParseColon()) != null)
                return t;
            if ((t = ParseSymbol()) != null)
                return t;
            if ((t = ParseKeyword()) != null)
                return t;
            if ((t = ParseName()) != null)
                return t;

            //throw new Exception("Stopped at " + pos + " ..." + Text.Substring(pos, Math.Min(45, Text.Length - pos)) + "...");
            return new Token(TokenKind.Symbol, Text[pos++].ToString());
        }

        Token ParseEOf()
        {
            if (pos != Text.Length)
                return null;
            return new Token(TokenKind.EOF, null);
        }

        void SkipSpaces()
        {
            while (pos < Text.Length && Text[pos] == ' ')
                pos++;
        }

        Token ParseDigit()
        {
            if (!char.IsDigit(Text[pos]))
                return null;

            int i = 1;
            bool hasDot = false;

            while (pos + i < Text.Length)
            {
                char c = Text[pos + i];

                if (char.IsDigit(c))
                {
                    i++;
                }
                else if (c == '.' && !hasDot)
                {
                    hasDot = true;
                    i++;
                }
                else
                {
                    break;
                }
            }

            var numberStr = Text.Substring(pos, i);

            // İstersen isFloat şeklinde ayrı bir TokenKind da ekleyebilirsin
            pos += i;
            return new Token(TokenKind.Digit, numberStr);
        }


        Token ParseName()
        {
            if (!char.IsLetter(Text[pos]))
                return null;

            int i = 1;
            while (pos + i < Text.Length 
                   && (char.IsLetterOrDigit(Text[pos + i]) || Text[pos + i] == '$' || Text[pos + i] == '%') 
                   && GetKeywordAt(pos+i) == null)
                i++;

            var t = new Token(TokenKind.Name, Text.Substring(pos, i));
            pos += i;
            return t;
        }

        Token ParseString()
        {
            if (Text[pos] != '"')
                return null;
            if ((pos>4159) &&(pos<4200))
            {
                int debug = 1;

            }
            int start = pos;
            int end;

            int quotePos = Text.IndexOf('"', pos + 1);
            int newlinePos = Text.IndexOf('\n', pos);

            if (quotePos == -1 && newlinePos == -1)
            {
                end = Text.Length;
            }
            else if (quotePos == -1)
            {
                end = newlinePos;
            }
            else if (newlinePos == -1)
            {
                end = quotePos;
            }
            else
            {
                end = Math.Min(quotePos, newlinePos);
            }

            string v;
            if (end == -1)
            {
                // Kapanış tırnağı yok → satır sonuna kadar al
                int newline = Text.IndexOf('\n', pos);
                if (newline == -1) newline = Text.Length;

                v = Text.Substring(pos, newline - pos);
                pos = newline; // tırnak kapanmadığı için newline'e kadar geç
            }
            else
            {
                // Normal durumda: tırnaklar arasında
                v = Text.Substring(pos, end + 1 - pos);
                pos = end + 1;
               // System.Diagnostics.Debug.WriteLine(v);

            }

            return new Token(TokenKind.String, v);
        }


        Token ParseNewline()
        {
            if (Text[pos] != '\n')
                return null;

            pos++;
            return new Token(TokenKind.NewLine, "\n");
        }

        Token ParseColon()
        {
            if (Text[pos] != ':')
                return null;
            pos++;
            return new Token(TokenKind.Colon, ":");
        }

        Token ParseSymbol()
        {
            string[] symbols = {",", "+", "-", "*", "/", "(", ")", "=", "<", ">", "<>", ";", "#"};

            var text = Text.Substring(pos);
            var match = symbols
                .Where(x => text.StartsWith(x, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Length)
                .FirstOrDefault();

            if (match == null)
                return null;

            pos += match.Length;

            return new Token(TokenKind.Symbol, match);
        }

        static readonly string[] C64BasicV2Keywords =
        {
            "abs", "and", "asc", "atn", "chr$", "close",
            "clr", "cmd", "cont", "cos", "data", "def",
            "dim", "end", "exp", "fn", "for", "fre",
            "get", "get#", "gosub", "goto","if","input",
            "input#","int","left$","len","let","list",
            "load", "log", "mid$","new", "next", "not",
            "on", "open", "or", "peek", "poke", "pos",
            "print", "print#","read","rem","restore","return",
            "right$	rnd", "run", "save", "sgn", "sin",
            "spc", "sqr", "status", "step", "stop", "str$",
            "sys", "tab", "tan", "then", "time", "time$", 
            "to", "usr", "val", "verify", "wait",
        };

        string GetKeywordAt(int position)
        {
            var text = Text.Substring(position);
            var match = C64BasicV2Keywords
                .Where(x => text.StartsWith(x, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.Length)
                .FirstOrDefault();

            return match;
        }

        Token ParseKeyword()
        {
            var match = GetKeywordAt(pos);

            if (match == null)
                return null;

            Token result;
            if (match == "rem")
            {
                var posNewline = Text.IndexOf('\n', pos);
                var content = Text.Substring(pos + 3, posNewline - pos - 3).Trim();
                var commentTextExcludingNewline = $"rem {content}";
                result = new Token(TokenKind.Keyword, commentTextExcludingNewline);
                pos = posNewline;
            }
            else
            {
                result = new Token(TokenKind.Keyword, match);
                pos += match.Length;
            }

            return result;
        }
    }
}
