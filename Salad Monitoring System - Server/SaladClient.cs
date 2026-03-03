namespace Salad_Monitoring_System___Server
{
    public class SaladClient
    {
        public Guid ClientUUID { get; set; }
        public required string ClientName { get; set; }
        public Guid InternUUID { get; private set; }

        public SaladClient(Guid iUUID)
        {
            ClientUUID = Guid.CreateVersion7();
            InternUUID = iUUID;
        }

    }
}
