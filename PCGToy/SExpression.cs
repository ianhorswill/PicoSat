﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCGToy
{
    public static class SExpression
    {
        public static object Read(TextReader r)
        {
            var result = ReadInternal(r);
            Skip(r);
            return result;
        }

        private static object ReadInternal(TextReader r)
        {
            int c;
            Skip(r);
            switch (c = r.Read())
            {
                case -1:
                    throw new FileFormatException("Premature end of file");

                case '(':
                    return ReadList(r);

                case '"':
                    return ReadString(r);

                case '+':
                case '-':
                case '.':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    return ReadNumber(r, (char)c);

                default:
                    if (char.IsLetter((char) c))
                        return ReadSymbol(c, r);

                    throw new FileFormatException($"Unexpected character '{(char)c}'");
            }
        }

        private static object ReadNumber(TextReader r, char initial)
        {
            StringBuffer.Clear();
            var sign = initial == '-' ? -1 : 1;
            var decimalPointCount = initial == '.' ? 1 : 0;
            if (char.IsDigit(initial))
                StringBuffer.Append(initial);

            while (IsDigitOrDecimal(r.Peek()))
            {
                char c = (char) r.Read();
                StringBuffer.Append(c);
                if (c == '.')
                    decimalPointCount++;
            }

            switch (decimalPointCount)
            {
                case 0:
                    return sign * Int32.Parse(StringBuffer.ToString());

                case 1:
                    return sign * float.Parse(StringBuffer.ToString());

                default:
                    throw new FileFormatException($"Unknown number formatl {StringBuffer.ToString()}");
            }
        }

        private static bool IsDigitOrDecimal(int c)
        {
            return c == '.' || char.IsDigit((char) c);
        }

        private static object ReadList(TextReader r)
        {
            var listBuffer = new List<object>();
            Skip(r);
            while (r.Peek() != ')')
            {
                listBuffer.Add(ReadInternal(r));
                Skip(r);
            }

            r.Read();

            return listBuffer;
        }

        private static object ReadSymbol(int initialChar, TextReader r)
        {
            StringBuffer.Clear();
            StringBuffer.Append((char) initialChar);
            while (!SymbolTerminator((char) r.Peek()))
                StringBuffer.Append((char) r.Read());

            return StringBuffer.ToString();
        }

        private static bool SymbolTerminator(char c)
        {
            return char.IsWhiteSpace(c) || c == '(' || c == ')';
        }

        private static readonly StringBuilder StringBuffer = new StringBuilder();
        private static string ReadString(TextReader r)
        {
            StringBuffer.Clear();
            int c=0;
            while (c != '"')
            {
                switch (c = r.Read())
                {
                    case -1:
                        throw new FileFormatException($"Premature end of file inside string \"{StringBuffer.ToString()}\"");

                    case '"':
                        // Do nothing; the while loop will terminate
                        break;

                    case '\\':
                        StringBuffer.Append((char) r.Read());
                        break;

                    default:
                        StringBuffer.Append((char) c);
                        break;
                }
            }

            return StringBuffer.ToString();
        }

        private static void Skip(TextReader r)
        {
            while (SkipComment(r) || SkipWhitespace(r))
            { }
        }

        private static bool SkipComment(TextReader r)
        {
            if (r.Peek() != ';')
                return false;
            while (!CommentTerminator(r.Read()))
            { }

            return true;
        }

        private static bool CommentTerminator(int c)
        {
            switch (c)
            {
                case -1:
                case '\r':
                case '\n':
                    return true;

                default:
                    return false;
            }
        }

        private static bool SkipWhitespace(TextReader r)
        {
            if (!Whitespace(r.Peek()))
                return false;

            while (Whitespace(r.Peek()))
                r.Read();

            return true;
        }

        private static bool Whitespace(int c)
        {
            return c > 0 && char.IsWhiteSpace((char) c);
        }
    }

    public class FileFormatException : Exception
    {
        public FileFormatException(string message) : base(message)
        {
            
        }
    }
}