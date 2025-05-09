using System;
using System.Collections.Generic;
using System.Text;

namespace PdfGen
{
    internal class CommandParser
    {
        internal const string CommandPrefix = "\\";
        internal const char QuoteChar = '\'';
        internal const char Eol = (char)0;

        private int index;
        private string line;
        private List<string> tokens;
        private StringBuilder currentToken;
        private delegate StateFunc StateFunc(char c);

        internal static string[] Parse(string line)
        {
            var parser = new CommandParser(line);
            return parser.tokens.ToArray();
        }

        private CommandParser(string line)
        {
            if (!line.StartsWith(CommandPrefix))
            {
                throw new ArgumentException("Command expected");
            }

            Parse();
        }

        private void Parse()
        {
            StateFunc state = BetweenTokens;
            foreach (char c in line)
            {
                state = state(c);
            }

            state(Eol);
            return;
        }

        private StateFunc BetweenTokens(char c)
        {
            if (Char.IsWhiteSpace(c) || c == Eol)
            {
                return BetweenTokens;
            }

            currentToken = new StringBuilder();
            if (c == QuoteChar)
            {
                return InQuotedToken;
            }
            else
            {
                return InPlainToken(c);
            }
        }

        private StateFunc InPlainToken(char c)
        {
            if (Char.IsWhiteSpace(c) || c == Eol)
            {
                tokens.Add(currentToken.ToString());
                return BetweenTokens;
            }

            currentToken.Append(c);
            return InPlainToken;
        }

        private StateFunc InQuotedToken(char c)
        {
            if (c == Eol)
            {
                throw new ArgumentException("Unterminated quoted string");
            }

            if (c == QuoteChar)
            {
                return AfterQuoteInQuotedToken;
            }

            currentToken.Append(c);
            return InQuotedToken;
        }

        private StateFunc AfterQuoteInQuotedToken(char c)
        {
            if (c == QuoteChar)
            {
                currentToken.Append(QuoteChar);
                return InQuotedToken;
            }
            
            tokens.Add(currentToken.ToString());
            return BetweenTokens(c);
        }
    }
}
