using Salad_Monitoring_System___Server;
using System.Text.Json.Nodes;
using WebSocketSharp;
using WebSocketSharp.Server;

var mysqlserver = new SmsApiFunctions("localhost", "3306", "root", "azTy23pm");

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/", () => "Oh, I see that you found the SMS API :) ");

SaladClient client = new SaladClient(Guid.CreateVersion7())
{
    ClientName = "Salad-1"
};

app.MapGet("/api/v1/getConnectedClients", () => client);

app.Run();
