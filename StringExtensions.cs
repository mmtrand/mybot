using System;

namespace RequestResponse
{
    public static class StringExtensions
    {
        public static bool IsAccessRequest(this string message)
        {
            return message.IndexOf("Application", StringComparison.InvariantCultureIgnoreCase) > -1 &&
                   message.IndexOf("access ", StringComparison.InvariantCultureIgnoreCase) >-1;
        }
    }
}