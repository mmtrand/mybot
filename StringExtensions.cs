using apiClientDotNet.Models;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RequestResponse
{
    public static class StringExtensions
    {
        public static bool IsAccessRequest(this string message)
        {
            return message.IndexOf("ApplicationAccess", StringComparison.InvariantCultureIgnoreCase) > -1;
        }

        public static bool IsApplictionChoice(this string message)
        {
            var appId = 0;
            if (int.TryParse(message, out appId))
            {
                return InMemoryData.ApplicationList.Any(p => p.Id == appId);
            }
            else
            {
                return InMemoryData.ApplicationList.Any(p => p.Name == message);
            }
        }

        public static BnppApplication GetUserChoice(this string message)
        {

            var appId = 0;
            if (int.TryParse(message, out appId))
            {
                return InMemoryData.ApplicationList.FirstOrDefault(p => p.Id == appId);
            }
            return InMemoryData.ApplicationList.FirstOrDefault(p => p.Name == message);
        }

        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

    }

    public static class BotExtensions
    {
        public static bool IsFromTheBot(this Message message)
        {
            return message.user.email.Contains("bot");
        }
    }
}