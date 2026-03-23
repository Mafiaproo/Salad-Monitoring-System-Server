using System.Runtime.CompilerServices;
using System.Text;

namespace Salad_Monitoring_System___Server
{
    public static class Logging
    {
        private static ConsoleColor foregroundColor = Console.ForegroundColor;
        private static ConsoleColor backgroundColor = Console.BackgroundColor;

        public static string errorHeader = " [ERROR] ";
        public static string warnHeader = " [WARN] ";
        public static string infoHeader = " [INFO] ";

        public static ConsoleColor errorColor = ConsoleColor.Red;
        public static ConsoleColor warnColor = ConsoleColor.Yellow;
        public static ConsoleColor infoColor = ConsoleColor.Cyan;

        public static void LogInfo(string message)
        {
            string toSend = DateTime.Now.ToShortTimeString() + infoHeader + message;
            Console.ForegroundColor = infoColor;

            Console.WriteLine(toSend);
            Console.ResetColor();

            WriteToFile(toSend);
        }

        public static void LogWarn(string message)
        {
            string toSend = DateTime.Now.ToShortTimeString() + warnHeader + message;

            Console.ForegroundColor = warnColor;

            Console.WriteLine(toSend);
            Console.ResetColor();

            WriteToFile(toSend);
        }

        public static void LogError(string message)
        {
            string toSend = DateTime.Now.ToShortTimeString() + errorHeader + message;

            Console.ForegroundColor = errorColor;

            Console.WriteLine(toSend);
            Console.ResetColor();

            WriteToFile(toSend);
        }

        private static void WriteToFile(string str)
        {
            string filename = "./Logs/" + DateTime.Today.ToString().Replace("/", "-").Replace(" 00:00:00", "") + ".log";

            try
            {
                StreamWriter stream = File.AppendText(filename);

                stream.WriteLine(str);

                stream.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Erreur du fichier logs : \n" + e);
            }


        }
    }
}
