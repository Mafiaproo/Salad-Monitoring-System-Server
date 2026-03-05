using MySqlConnector;
using System.Data;
using System.Reflection.Metadata;


namespace Salad_Monitoring_System___Server
{
    public class SmsApiFunctions
    {
        private string _host;
        private string _port;
        private string _username;
        private string _password;

        private const string DBNAME = "SMSDb";

        private MySqlConnection _mysqlconnection;

        public MySqlConnection mySqlConnection
        {
            get { return _mysqlconnection; }
            private set { _mysqlconnection = value; }
        }

        public SmsApiFunctions(string host, string port, string username, string password)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            

            mySqlConnection = new MySqlConnection($"Server={this._host};Port={this._port};User={this._username};Password={this._password}");

            // Connection au Serveur Base de Donnée
            int cpt = 5;
            while (cpt > 0 && mySqlConnection.State != ConnectionState.Open)
            {
                try
                {
                    Console.WriteLine("[INFO] Connecting to Database...");
                    mySqlConnection.Open();
                    Console.WriteLine("[INFO] Connected to Database !");
                }
                catch (MySqlException e)
                {
                    Console.WriteLine($"[WARN] An Error has been occured during the connection to the database. Retrying {cpt-5}/4 !\nException : " + e.ToString());
                    if (cpt <= 0)
                    {
                        Console.WriteLine("[ERROR] SMS can't connect to the database. Ending Server!"); 
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
                    Console.WriteLine("[INFO] SMSDb exist.");
                }
            }



            reader.Close();
            
            if(!isCreated)
            {
                
                try
                {
                    Console.WriteLine("[WARN] SMSDb is not created. Creating Database...");

                    MySqlCommand createDbCmd = new MySqlCommand($"CREATE DATABASE {DBNAME};", mySqlConnection);
                    reader = createDbCmd.ExecuteReader();

                    reader.Close();

                    Console.WriteLine("[INFO] Setup Tables ...");
                    MySqlCommand setupDbCmd = new MySqlCommand(File.ReadAllText("./DB_CONFIG.sql"), mySqlConnection);
                    reader = setupDbCmd.ExecuteReader();
                    Console.WriteLine("[INFO] Tables are Setup ...");

                    reader.Close();

                    Console.WriteLine("[WARN] SMSDb has been created succesfuly !");
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
            SaladClient client = new SaladClient(uuid) { ClientName = " Test Client" };

            return client;
        }

        public SaladClient[] GetConnectedClients()
        {
            SaladClient[] saladClients = { };
            MySqlCommand allClientsCommand = new MySqlCommand("USE SMSDb; SELECT * FROM clients;", mySqlConnection);

            var reader = allClientsCommand.ExecuteReader();

            if(!reader.HasRows)
            {
                return saladClients;
            }

            while(reader.Read())
            {
                reader.GetValues(saladClients);
            }

            reader.Close();

            return saladClients;
        }

    }
}
