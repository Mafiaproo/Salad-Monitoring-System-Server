using Salad_Monitoring_System___Server;
using System.Diagnostics;
using System.Text.Json.Nodes;
using WebSocketSharp;
using WebSocketSharp.Server;

Console.ForegroundColor = ConsoleColor.Green;

Console.WriteLine(@"
  ░██████╗  ░█████╗  ██╗       ░█████╗  ██████╗
  ██╔════╝ ██╔══██╗ ██║      ██╔══██╗ ██╔══██╗
  ░██████╗ ███████║ ██║      ███████║ ██║  ██║
  ╚════██║ ██╔══██║ ██║      ██╔══██║ ██║  ██║
  ██████╔╝ ██║  ██║ ███████╗ ██║  ██║ ██████╔╝
  ╚═════╝  ╚═╝  ╚═╝ ╚══════╝ ╚═╝  ╚═╝ ╚═════╝
         MONITORING SYSTEM - SERVER
");

Console.ResetColor();


var smsApi = new SmsApiFunctions("localhost", "3306", "root", "azTy23pm");
WebSocketServer server = new WebSocketServer("ws://localhost:5588");

Logging.LogInfo("Le Serveur se Lance !");
Logging.LogWarn("Attention Test !");
Logging.LogError("Une Erreur test est survenue :");

// INSERT INTO clients VALUES("019cbfc3-f0fa-7e9a-8571-7c5834a32afc", "Test Client", "019cbfc3-f0fa-7e9a-8571-7c5834a32afc", "2026-03-05 21:52:30", "Online");
Logging.LogInfo("Ajout du client test");
smsApi.CreateClient(Guid.Parse("019cbfc3-f0fa-7e9a-8571-7c5834a32afc"), Guid.Parse("019cbfc3-f0fa-7e9a-8571-7c5834a32afc"), "127.0.0.1", "Salad Client-1", "Online");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Oh, I see that you found the SMS API :)");

app.MapGet("/api/v1/getClient/{uuid}", (Guid uuid) => smsApi.GetClientByUuid(uuid));

SaladClient client = new SaladClient(Guid.CreateVersion7(), DateTime.Now, "127.0.0.1")
{
    ClientName = "Salad-1"
};

app.MapGet("/api/v1/getConnectedClients", () => smsApi.GetConnectedClients());

server.AddWebSocketService<WebsocketMonitoring>("/monitoring", behavior => behavior.smsApiFunctions = smsApi);

server.Start();
app.Run();

int cpu = 21;
int bw = 48;
int nodes = smsApi.GetConnectedClients().Count();
string uptime = "02h 14m";

string bar(int value, int max = 10)
{
    int filled = value * max / 100;
    return new string('█', filled) + new string('░', max - filled);
}



//Console.WriteLine("  ┌──────────────────────────────────────┐");
//Console.WriteLine($"  │ STATUS     : ● ONLINE                │");
//Console.WriteLine($"  │ CPU        : {bar(cpu)}  {cpu}%         │");
//Console.WriteLine($"  │ BANDWIDTH  : {bar(bw)}  {bw} MB/s     │");
//Console.WriteLine($"  │ NODES      : {nodes} ACTIVE              │");
//Console.WriteLine($"  │ UPTIME     : {uptime}               │");
//Console.WriteLine("  └──────────────────────────────────────┘");

