namespace Salad_Monitoring_System___Server
{
    public class SaladClient
    {
        public Guid ClientUUID { get; set; }
        public string ClientName { get; set; }
        public string InternUUID { get; private set; }

        public DateTime LastSeen { get; set; }

        public string Status { get; set; }

        public SaladClient(Guid iUUID, DateTime time, string name = "Salad Client", string status = "Unknown")
        {
            ClientName = name;
            ClientUUID = Guid.CreateVersion7();
            InternUUID = iUUID.ToString();
            LastSeen = time;
            Status = status;
        }

    }
}
