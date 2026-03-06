using Salad_Monitoring_System___Server;
using System.Text.Json.Nodes;
using WebSocketSharp;
using WebSocketSharp.Server;

var mysqlserver = new SmsApiFunctions("localhost", "3306", "root", "azTy23pm");
WebSocketServer server = new WebSocketServer("ws://localhost:5588");

// INSERT INTO clients VALUES("019cbfc3-f0fa-7e9a-8571-7c5834a32afc", "Test Client", "019cbfc3-f0fa-7e9a-8571-7c5834a32afc", "2026-03-05 21:52:30", "Online");
Console.WriteLine("[INFO] Ajout du client test");
mysqlserver.CreateClient(Guid.Parse("019cbfc3-f0fa-7e9a-8571-7c5834a32afc"), Guid.Parse("019cbfc3-f0fa-7e9a-8571-7c5834a32afc"), "Salad Client-1", "Online");

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

app.MapGet("/api/v1/getClient/{uuid}", (Guid uuid) => mysqlserver.GetClientByUuid(uuid));

SaladClient client = new SaladClient(Guid.CreateVersion7(), DateTime.Now)
{
    ClientName = "Salad-1"
};

app.MapGet("/api/v1/getConnectedClients", () => mysqlserver.GetConnectedClients());

server.AddWebSocketService<WebsocketMonitoring>("/monitoring", behavior => behavior.smsApiFunctions = mysqlserver);

server.Start();
app.Run();






