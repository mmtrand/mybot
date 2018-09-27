using System;
using System.Collections.Generic;
using System.IO;
using apiClientDotNet;
using apiClientDotNet.Listeners;
using apiClientDotNet.Models;
using apiClientDotNet.Models.Events;
using RequestResponse;
using RequestResponse.Properties;
using Stream = apiClientDotNet.Models.Stream;

namespace RequestResponse
{
    public class BotLogic : RoomListener
    {

        private BotState currentState = BotState.Initial;
        public IList<BnppApplication> ApplicationList { get; set; }
        public BotLogic()
        {
            this.ApplicationList = InMemoryData.ApplicationList;
        }

        public void onRoomMessage(Message inboundMessage)
        {
            if (!inboundMessage.IsFromTheBot())
            {
                if (currentState == BotState.Initial)
                {
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage(Resources.LanguageChoiceResponse));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateChoicesMessage(InMemoryData.Languages));
                    currentState = BotState.ChosingLanguage;
                }


                var messageContent = inboundMessage.message.StripHTML();

                if (currentState == BotState.ChosingLanguage)
                {
                    if (messageContent.IsLanguageChoice())
                    {
                        SetCurrentLanguage(messageContent);
                    }
                    else
                    {
                        currentState = BotState.Initial;
                    }
                }

                SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                    MessageFactory.CreateTextMessage(Resources.ApplicationChoicesResponse));

                if (messageContent.IsAccessRequest())
                {
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage(Resources.ApplicationChoicesResponse));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateChoicesMessage((IList<BnppApplication>)ApplicationList));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage(Resources.ChoseApplicationResponse));
                }
                if (messageContent.IsApplictionChoice())
                {
                    var app = messageContent.GetUserChoice();

                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage($"You have chosen { app.Name }"));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage(app.Description));
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage(Resources.SendingRequestToLineManagerResponse));
                }
                else
                {
                    SendMessageTo(inboundMessage.user.firstName, inboundMessage.stream,
                        MessageFactory.CreateTextMessage(Resources.WrongChoiceResponse));
                }
            }

        }

        private void SetCurrentLanguage(string messageContent)
        {
            string lang = "en-EN";
            if (messageContent == "Fr")
            {
                lang = "fr-FR";
            }

            System.Threading.Thread.CurrentThread.CurrentCulture =
                System.Globalization.CultureInfo.CreateSpecificCulture(lang);
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
}