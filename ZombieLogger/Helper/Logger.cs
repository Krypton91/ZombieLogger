using System;

namespace ZombieLogger.Helper
{
    public static class Logger
    {

        public enum LogType 
        {
            ERROR = 0,
            SUCESS,
            WARNING
        }

        public static void Log(LogType type, string message, bool TimeStamp = true) 
        {
            if (type == LogType.ERROR)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (TimeStamp)
                    Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff") + " | " + message);
                else
                    Console.WriteLine(message);
            }
            else if (type == LogType.SUCESS)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                if (TimeStamp)
                    Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff") + " | " + message);
                else
                    Console.WriteLine(message);
            }


            else if (type == LogType.WARNING)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (TimeStamp)
                    Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.ff") + " | " + message);
                else
                    Console.WriteLine(message);
            }
        }

        public static void Init() 
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@" _____               _     _      _                                ");
            Console.WriteLine(@"|__  /___  _ __ ___ | |__ (_) ___| |    ___   __ _  __ _  ___ _ __ ");
            Console.WriteLine(@"  / // _ \| '_ ` _ \| '_ \| |/ _ \ |   / _ \ / _` |/ _` |/ _ \ '__|");
            Console.WriteLine(@" / /| (_) | | | | | | |_) | |  __/ |__| (_) | (_| | (_| |  __/ |   ");
            Console.WriteLine(@"/____\___/|_| |_| |_|_.__/|_|\___|_____\___/ \__, |\__, |\___|_|   ");
            Console.WriteLine(@"                                             |___/ |___/          ");
            Console.WriteLine(@"Coded by Krypton91 © 2021");
        }
    }
}
