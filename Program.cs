using System.Collections.Generic;
using System.IO;
using apiClientDotNet.Models;
using apiClientDotNet;
using apiClientDotNet.Listeners;
using apiClientDotNet.Services;
using System.Diagnostics;
using System.Net.Mime;

namespace RequestResponse
{
    class Program
    {
        public static SymConfig symConfig { get; set; }
        static void Main(string[] args)
        {
            string filePath = Path.GetFullPath("config.json");
            
            SymBotClient symBotClient = new SymBotClient(); 
            DatafeedEventsService datafeedEventsService = new DatafeedEventsService();
            symConfig = symBotClient.initBot(filePath);

            
            RoomListener botLogic = new BotLogic();
            DatafeedClient datafeedClient = datafeedEventsService.init(symConfig);
            Datafeed datafeed = datafeedEventsService.createDatafeed(symConfig, datafeedClient);
            datafeedEventsService.addRoomListener(botLogic);
            datafeedEventsService.getEventsFromDatafeed(symConfig, datafeed, datafeedClient);
        }
    }

    internal class MessageFactory
    {
        public static Message CreateTextMessage(string content)
        {
            Message message = new Message();
            message.message = "<messageML>" + content + "!</messageML>";
            return message;
        }

        public static Message CreateChoicesMessage(IList<BnppApplication> list)
        {
            Message message = new Message();
            string content = "<messageML><ul>";
            foreach (var element in list)
            {
                content += $"<li>{element.Id})- {element.Name}</li>"; 
            }
            message.message = content + "</ul></messageML>";
            return message;
        }

        public static Message CreateChoicesMessage(IList<string> list)
        {
            Message message = new Message();
            string content = "<messageML><ul>";
            var counter = 0;
            foreach (var element in list)
            {
                content += $"<li>{element}</li>";
            }
            message.message = content + "</ul></messageML>";
            return message;
        }
    }


    internal enum MessageTypes
    {
         SelectList,
         Text
    }
}
