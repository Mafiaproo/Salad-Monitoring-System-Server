using MySqlConnector;
using System.Data;
using System.Reflection.Metadata;
using System.Runtime.Serialization;


namespace Salad_Monitoring_System___Server
{
    public class SmsApiFunctions
    {
        private string _host;
        private string _port;
        private string _username;
        private string _password;

        private const string DBNAME = "SMSDb";

        //private MySqlConnection _mysqlconnection;

        //public MySqlConnection mySqlConnection
        //{
        //    get { return _mysqlconnection; }
        //    private set { _mysqlconnection = value; }
        //}

        public SmsApiFunctions(string host, string port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            

            MySqlConnection mySqlConnection = new MySqlConnection($"Server={this._host};Port={this._port};User={this._username};Password={this._password}");

            // Connection au Serveur Base de Donnée
            int cpt = 5;
            while (cpt > 0 && mySqlConnection.State != ConnectionState.Open)
            {
                try
                {
                    Logging.LogInfo("Connecting to Database...");
                    mySqlConnection.Open();
                    Logging.LogInfo("Connected to Database !");
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"[WARN] An Error has been occured during the connection to the database. Retrying {cpt-5}/4 !\nException : " + e.ToString());
                    if (cpt <= 0)
                    {
                        Logging.LogError("SMS can't connect to the database. Ending Server!"); 
                        Environment.Exit(1);
                    }
                    else cpt--;

                }
            }

            MySqlCommand listDbCmd = new MySqlCommand("SHOW DATABASES;", mySqlConnection);
            var reader = listDbCmd.ExecuteReader();

            bool isCreated = false;

            while(reader.Read())
            {
                if (reader.GetString(0) == DBNAME)
                {
                    isCreated = true;
                    Logging.LogInfo("SMSDb exist.");
                }
            }



            reader.Close();
            
            if(!isCreated)
            {
                
                try
                {
                    Logging.LogWarn("SMSDb is not created. Creating Database...");

                    MySqlCommand createDbCmd = new MySqlCommand($"CREATE DATABASE {DBNAME};", mySqlConnection);
                    reader = createDbCmd.ExecuteReader();

                    reader.Close();

                    Logging.LogInfo("Setup Tables ...");
                    MySqlCommand setupDbCmd = new MySqlCommand(File.ReadAllText("./DB_CONFIG.sql"), mySqlConnection);
                    reader = setupDbCmd.ExecuteReader();
                    Logging.LogInfo("Tables are Setup ...");

                    reader.Close();

                    Logging.LogWarn("SMSDb has been created succesfuly !");
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"[ERROR] An Error has been occured during the creation of the database. Ending Server ! \nException : " + e.ToString());
                    Environment.Exit(1);
                }
            }



        }

        public SaladClient GetClientByUuid(Guid uuid)
        {
            SaladClient client = new SaladClient(uuid, DateTime.Now, "192.168.2.100");

            return client;
        }

        public SaladClient[] GetConnectedClients()
        {
            using MySqlConnection connection = new MySqlConnection(
                $"Server={_host};Port={_port};User={_username};Password={_password};Database=SMSDb"
            );

            connection.Open();

            List<SaladClient> clients = new List<SaladClient>();

            using MySqlCommand command = new MySqlCommand("SELECT * FROM clients;", connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                // UUID DATETIME NOM STATUS
                SaladClient client = new SaladClient(reader.GetGuid(0), reader.GetDateTime(3), reader.GetString(5), reader.GetString(1), reader.GetString(4));


                clients.Add(client);
            }

            return clients.ToArray();
        }

        public SaladClient CreateClient(Guid iUUID, Guid internalUUID, string name, string status, string ipaddress)
        {
            SaladClient newClient = new SaladClient(iUUID, DateTime.Now, ipaddress, name, status);

            using MySqlConnection connection = new MySqlConnection(
                $"Server={_host};Port={_port};User={_username};Password={_password};Database=SMSDb"
            );

            connection.Open();

            using MySqlCommand command = new MySqlCommand(
                $"INSERT IGNORE INTO clients VALUES(\"{iUUID.ToString()}\"," +
                $" \"{name}\"," +
                $" \"{internalUUID.ToString()}\"," +
                $" \"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\"," +
                $" \"{status}\"," +
                $" \"{ipaddress}\");", connection);

            using var reader = command.ExecuteReader();

            reader.Close();

            return newClient;

        }

        /*
         Add Client on database and return created client as a SaladClient instance
         */
        public SaladClient CreateClient(SaladClient client)
        {
            using MySqlConnection connection = new MySqlConnection(
                $"Server={_host};Port={_port};User={_username};Password={_password};Database=SMSDb"
            );

            connection.Open();

            using MySqlCommand command = new MySqlCommand(
                $"INSERT IGNORE INTO clients VALUES(\"{client.ClientUUID.ToString()}\"," +
                $" \"{client.ClientName}\"," +
                $" \"{client.InternUUID.ToString()}\"," +
                $" \"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}\"," +
                $" \"{client.Status}\"," +
                $" \"{client.IpAddress}\");", connection);

            using var reader = command.ExecuteReader();

            reader.Close();

            return client;

        }

        public void DeleteClient(Guid uuid)
        {
            using MySqlConnection connection = new MySqlConnection(
                $"Server={_host};Port={_port};User={_username};Password={_password};Database=SMSDb"
            );

            connection.Open();

            try
            {
                using MySqlCommand commandDelete = new MySqlCommand($"USE SMSDb; DELETE FROM clients WHERE uuid=\"{uuid.ToString()}\";", connection);
                using var reader = commandDelete.ExecuteReader();

                reader.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"[WARN] Can't delete client with uuid {uuid.ToString()} ! \nException : " + e.ToString());
            }


        }

        public void DeleteClientByIp(string ip)
        {
            using MySqlConnection connection = new MySqlConnection(
                $"Server={_host};Port={_port};User={_username};Password={_password};Database=SMSDb"
            );

            connection.Open();

            try
            {
                using MySqlCommand commandDelete = new MySqlCommand($"USE SMSDb; DELETE FROM clients WHERE ip_address=\"{ip}\";", connection);
                using var reader = commandDelete.ExecuteReader();

                reader.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine($"[WARN] Can't delete client with IP {ip} ! \nException : " + e.ToString());
            }


        }

    }
}
