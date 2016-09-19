using System;

namespace Shared
{
    public static class Helper
    {
        public static string GeneratePassword(int len = 8)
        {
            var password = "";
            var randomChars = new[] { "w", "e", "r", "t", "y", "u", "o", "p", "a", "s", 
                                                "d", "f", "h", "k", "z", "x", "c", "v", "b",
                                                "n", "m", "2", "3", "4", "5", "6", "7", "8", "9", "0"};
            var r = new Random();
            for (var i = 0; i < len; i++)
            {
                var j = r.Next(0, randomChars.Length - 1);
                password += randomChars[j];
            }
            return password;
        }
    }
}
