using System;
using System.Collections.Generic;
using System.IO;
using apiClientDotNet.Models;
using apiClientDotNet;
using apiClientDotNet.Listeners;
using apiClientDotNet.Services;
using apiClientDotNet.Models.Events;
using System.Diagnostics;
using System.Net.Mime;
using Stream = apiClientDotNet.Models.Stream;

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

    public class BotLogic : RoomListener
    {
        public IList<BnppApplication> ApplicationList { get; set; }
        public BotLogic()
        {
            this.ApplicationList = InMemoryData.ApplicationList;
        }
        public void onRoomMessage(Message inboundMessage)
        {
            if (!inboundMessage.IsFromTheBot())
            {
                var messageContent = inboundMessage.message;
                if (messageContent.IsAccessRequest())
                {
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage("Which application do you want to access?"));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateChoicesMessage(ApplicationList));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage("Please chose the application you want to access from the list :)"));
                }
                if (messageContent.IsApplictionChoice())
                {
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage($"You have chosen { messageContent.GetUserChoice() }"));

                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage($"Please be patient... we are sending a request to Line Manager"));
                }
                else
                {
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage("I didn't understand your choice !"));
                }
            }
            
        }

       

        private void SendMessageTo(string name, Stream stream, Message message)
        {
            string filePath = Path.GetFullPath("config.json");
            SymBotClient symBotClient = new SymBotClient();
            SymConfig symConfig = symBotClient.initBot(filePath);
            MessageClient messageClient = new apiClientDotNet.MessageClient();
            messageClient.sendMessage(symConfig, message, stream);

        }

        public void onRoomCreated(RoomCreated roomCreated)
        {
            Console.WriteLine("Room " + roomCreated.roomProperties.name);

        }
        public void onRoomDeactivated(RoomDeactivated roomDeactivated) { }
        public void onRoomMemberDemotedFromOwner(RoomMemberDemotedFromOwner roomMemberDemotedFromOwner) { }
        public void onRoomMemberPromotedToOwner(RoomMemberPromotedToOwner roomMemberPromotedToOwner) { }
        public void onRoomReactivated(apiClientDotNet.Models.Stream stream) { }

        public void onRoomUpdated(RoomUpdated roomUpdated)
        {
            Message message2 = new Message();
            message2.message = "<messageML> Hi " + roomUpdated.stream.roomName + "!</messageML>";
            MessageClient messageClient = new apiClientDotNet.MessageClient();
            messageClient.sendMessage(Program.symConfig, message2, roomUpdated.stream);
        }

        public void onUserJoinedRoom(UserJoinedRoom userJoinedRoom)
        {
            Message message2 = new Message();
            message2.message = "<messageML> Hi " + userJoinedRoom.affectedUser.firstName + "!</messageML>";
            MessageClient messageClient = new apiClientDotNet.MessageClient();
            messageClient.sendMessage(Program.symConfig, message2, userJoinedRoom.stream);
        }
        public void onUserLeftRoom(UserLeftRoom userLeftRoom) { }
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
    }


    internal enum MessageTypes
    {
         SelectList,
         Text
    }
}
