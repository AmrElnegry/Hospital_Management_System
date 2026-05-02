using System;

namespace Project_Hospital
{
    static class ValidationHelper
    {
        public static bool IsValidFullName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            bool inWord = false;
            int wordCount = 0;

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                if (char.IsLetter(ch) || ch == ' ' || ch == '.')
                {
                    if (ch != ' ' && !inWord)
                    {
                        wordCount++;
                        inWord = true;
                    }
                    else if (ch == ' ')
                    {
                        inWord = false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return wordCount >= 2;
        }

        public static bool IsValidTextOnly(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];
                if (!(char.IsLetter(ch) || ch == ' ' || ch == '.'))
                    return false;
            }

            return true;
        }

        public static bool IsValidEgyptianPhone(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 11)
                return false;

            for (int i = 0; i < input.Length; i++)
            {
                if (!char.IsDigit(input[i]))
                    return false;
            }

            return input.StartsWith("010") ||
                   input.StartsWith("011") ||
                   input.StartsWith("012") ||
                   input.StartsWith("015");
        }
    }
}
