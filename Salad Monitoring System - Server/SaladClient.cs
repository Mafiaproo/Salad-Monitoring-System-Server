namespace Salad_Monitoring_System___Server
{
    public class SaladClient
    {
        public Guid ClientUUID { get; set; }
        public string ClientName { get; set; }
        public string InternUUID { get; private set; }

        public string IpAddress { get; private set; }

        public DateTime LastSeen { get; set; }

        public string Status { get; set; }

        public SaladClient(Guid iUUID, DateTime time, string ipaddress, string name = "Salad Client", string status = "Unknown")
        {
            this.ClientName = name;
            this.IpAddress = ipaddress;
            this.ClientUUID = Guid.CreateVersion7();
            this.InternUUID = iUUID.ToString();
            this.LastSeen = time;
            this.Status = status;
        }

    }
}
