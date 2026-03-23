using System.Text.Json;
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

namespace Salad_Monitoring_System___Server
{
    public class WebsocketMonitoring : WebSocketBehavior
    {
        public SmsApiFunctions smsApiFunctions;

        private string ip_address;

        protected override void OnOpen()
        {
            string ip = Context.UserEndPoint.Address.MapToIPv4().ToString();
            Logging.LogInfo("A WebSocket Client has been connected ! Creating client to database " + ip);

            var cookies = Context.CookieCollection;

            this.ip_address = Context.UserEndPoint.Address.MapToIPv4().ToString();

            SaladClient saladClient = new SaladClient(Guid.Parse(cookies[0].Value), DateTime.Now, this.ip_address, name: cookies[1].Value, status: cookies[2].Value);

            smsApiFunctions.CreateClient(saladClient);
        }

        protected override void OnClose(CloseEventArgs e)
        {
            Logging.LogInfo("A WebSocket Client has been disconnected ! Deleting client from database ");

            smsApiFunctions.DeleteClientByIp(this.ip_address);
            
        }

        protected override void OnMessage(MessageEventArgs e)
        {            
            string ip = Context.UserEndPoint.Address.MapToIPv4().ToString();
            var messageDict = JsonSerializer.Deserialize<Dictionary<string, string>>(e.Data);

            switch (messageDict?["type"])
            {
                //case "Connected":
                //    if (messageDict.Count < 5) break;

                //    smsApiFunctions.CreateClient(
                //        Guid.Parse(messageDict["uuid"]),
                //        Guid.Parse(messageDict["internuuid"]),
                //        Context.UserEndPoint.Address.ToString(),
                //        messageDict["name"],
                //        messageDict["status"]
                //    );

                //    break;

                //case "Disconnected":
                //    if (messageDict.Count < 2 && !messageDict.ContainsKey("uuid")) break;

                //    smsApiFunctions.DeleteClient(Guid.Parse(messageDict["uuid"]));

                //    break;

                case "Statistics":
                    Logging.LogInfo("Le client " + Context.UserEndPoint.ToString() + " envoie des stats");

                    break;



                default:
                    Logging.LogWarn("Received unimplemented message type :" + messageDict?["type"]);
                    break;

            }

        }

    }
}
