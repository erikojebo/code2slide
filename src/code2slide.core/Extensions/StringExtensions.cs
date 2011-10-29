using System;
using System.Collections.Generic;
using System.Text;

namespace code2slide.core.Extensions
{
    public static class StringExtensions
    {
        public static string[] SplitAtLine(this string original, Predicate<string> filter)
        {
            var parts = new List<string>();

            var lines = original.GetLinesKeepingLineBreaks();

            var builder = new StringBuilder();

            foreach (var line in lines)
            {
                if (filter(line))
                {
                    parts.Add(builder.ToString());
                    builder.Clear();
                }
                else
                {
                    builder.Append(line);
                }
            }

            if (builder.Length > 0)
            {
                parts.Add(builder.ToString());
            }

            return parts.ToArray();
        }

        public static string[] GetLinesKeepingLineBreaks(this string s)
        {
            var lines = new List<string>();

            int lastLineStart = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\n')
                {
                    lines.Add(s.Substring(lastLineStart, i + 1 - lastLineStart)); 
                    lastLineStart = i + 1;
                }
            }

            lines.Add(s.Substring(lastLineStart));

            return lines.ToArray();
        }

        public static string StripWhitespace(this string s)
        {
            return s.Replace("\n", "")
                .Replace("\r", "")
                .Replace("\t", "")
                .Replace(" ", "");
        }
    }
}