using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Salad_Monitoring_System___Server
{
    public class WebsocketMonitoring : WebSocketBehavior
    {
        public SmsApiFunctions smsApiFunctions;

        protected override void OnOpen()
        {
            string ip = Context.UserEndPoint.Address.ToString();
            Console.WriteLine("[INFO] A WebSocket Client has been connected ! Creating client to database " + ip);
        }

        protected override void OnMessage(MessageEventArgs e)
        {            
            string ip = Context.UserEndPoint.Address.ToString();
            var messageDict = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);

            switch (messageDict?["type"])
            {
                case "Connected":
                    if (messageDict.Count < 5) break;

                    smsApiFunctions.CreateClient(
                        Guid.Parse(messageDict["uuid"]),
                        Guid.Parse(messageDict["internuuid"]),
                        messageDict["name"],
                        messageDict["status"]
                    );

                    break;

                default:
                    Console.WriteLine("[WARN] Received unimplemented message type :" + messageDict["type"]);
                    break;

            }

        }

    }
}
