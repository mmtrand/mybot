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
            message = message.Replace("<div data-format=\"PresentationML\" data-version=\"2.0\" class=\"wysiwyg\">","");
            Regex regex = new Regex(@"[\d]");
            if (regex.IsMatch(message))
            {
                var appIdString = regex.Match(message);
                var appId = Convert.ToInt16(appIdString.Value);
                return InMemoryData.ApplicationList.Any(p => p.Id == appId);
            }
            else
            {
                return InMemoryData.ApplicationList.Any(p => p.Name == message);
            }
        }

        public static string GetUserChoice(this string message)
        {
            message = message.Replace("<div data-format=\"PresentationML\" data-version=\"2.0\" class=\"wysiwyg\">", "");
            Regex regex = new Regex(@"[\d]");
            if (regex.IsMatch(message))
            {
                var appIdString = regex.Match(message);
                var appId = Convert.ToInt16(appIdString.Value);
                return InMemoryData.ApplicationList.FirstOrDefault(p => p.Id == appId).Name;
            }
            
            return InMemoryData.ApplicationList.FirstOrDefault(p => message.Contains(p.Name) ).Name;
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