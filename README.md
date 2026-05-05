
<img width="1536" height="1024" alt="SMS_Icon Fait Par IA" src="https://github.com/user-attachments/assets/672cdaf5-682c-49d4-b38d-84c72afc18b4" />
# Salad Monitoring System - Server

## About
SMS-Server is the backend of the SMS System. Sms is a local service for managing **salad.com** machines.
It opens an api for the Dashboard (https://github.com/Mafiaproo/SaladMonitoringSystem-Dashboard) and a Websocket server for the client
(https://github.com/Mafiaproo/Salad-Monitoring-System-Client)

The System is actually in developpement for now. 
The server and client is programmed in C# and the Dashboard with React.

## Functionality
- Stats about hourly earning
- Clients/Machines basic controls
  - Shutdown/Reboot
  - Start/Restart/Stop Salad Client
  - Logs View
  - Machine Stats(cpu, mem, bandwidth, storage)
- Using Salad Dashboard API for informations that can't be obtained on the client.

**If you have any new ideas. Feel free to create an Issue and describe it :)**

