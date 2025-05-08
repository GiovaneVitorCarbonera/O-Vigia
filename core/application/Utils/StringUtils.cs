using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Vigia.core.application.utils
{
    internal class StringUtils
    {
        public static string maxStringSize(string value, int sizeMax)
        {
            if (value.Length > sizeMax - 3)
                value = value.Substring(0, sizeMax) + "...";

            return value;
        }

        public static string convertToSafeString(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var output = new StringBuilder();
            foreach (Rune rune in value.EnumerateRunes())
            {
                int val = rune.Value;
                bool isLatin =
                    (val >= 0x0041 && val <= 0x005A) || // A-Z
                    (val >= 0x0061 && val <= 0x007A) || // a-z
                    (val >= 0x00C0 && val <= 0x00FF) || // À-ÿ
                    (val == 0x0020);

                if ((isLatin || Rune.IsNumber(rune) || Rune.IsWhiteSpace(rune)) || (rune.Value >= 0x1D400 && rune.Value <= 0x1D7FF))
                    output.Append(rune);
            }
            return output.ToString().Trim();
        }

        public static string transformeStringMinSize(string value, int size)
        {
            while (value.Length < size)
            {
                value = value + " ";
            }
            return value;
        }

        public static bool StartsWithWord(string content, string word)
        {
            return Regex.IsMatch(content, $@"^\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase);
        }

        public static string[] splitStringForSize(string value, int size, char charSplit = char.MinValue)
        {
            List<string> rsSplits = new List<string>();
            if (charSplit == char.MinValue)
            {
                while (value.Length > 0)
                {
                    rsSplits.Add(value.Substring(0, Math.Min(size, value.Length)));
                }
            }
            else
            {
                var splits = value.Split(charSplit);
                string rsValue = "";
                foreach(var split in splits)
                {
                    if (rsValue.Length + split.Length > size)
                    {
                        rsSplits.Add(rsValue);
                        rsValue = "";
                    }
                    rsValue += split + charSplit;
                }
                if (!string.IsNullOrEmpty(rsValue))
                    rsSplits.Add(rsValue);

            }
            return rsSplits.ToArray();
        }
    }
}
