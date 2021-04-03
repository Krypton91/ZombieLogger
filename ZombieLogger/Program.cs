using System;
using System.IO;
using System.Numerics;
using System.Text;
using Newtonsoft.Json;
using ZombieLogger.Helper;
using ZombieLogger.MySqlDatabase;
using ZombieLogger.Objects;

namespace ZombieLogger
{
    class Program
    {
        static FileSystemWatcher watcher = new FileSystemWatcher();
        static ConfigManager cfg = new ConfigManager();
        static MySqlDataBase db;
        public static int lastReadLine = 0;
        static void Main(string[] args)
        {
            Logger.Init();
           
            if (cfg.Config != null)
            {
                Logger.Log(Logger.LogType.SUCESS, "Loaded Config! START READING CONFIG VALUES!");
                Logger.Log(Logger.LogType.SUCESS, "DB Name: " + cfg.Config.DataBaseName);
                Logger.Log(Logger.LogType.SUCESS, "DB User: " + cfg.Config.DataBaseUsername);
                Logger.Log(Logger.LogType.SUCESS, "DayZ Profilepath: " + cfg.Config.ServerProfilePath);
            }
            try
            {
                watcher.Path = cfg.Config.ServerProfilePath + @"\ZombieLogger";
                watcher.Filter = "Logs.json";
                watcher.Changed += OnChanged;
                watcher.EnableRaisingEvents = true;
                db = new MySqlDataBase(cfg.Config.DataBaseIP, cfg.Config.DataBaseName, cfg.Config.DataBaseUsername, cfg.Config.DataBasePassword);
            }
            catch(Exception ex) 
            {
                Logger.Log(Logger.LogType.ERROR, "Config Error cant find path to watch for changes code stack trace: \n" + ex.StackTrace);
            }

            Console.ReadKey();
        }


        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            try
            {

                using (var fs = new FileStream(e.FullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.UTF8))
                {
                    int counter = 0;
                    string line;

                    // Read the file and display it line by line.  
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        if (counter > lastReadLine)
                        {
                            Logger.Log(Logger.LogType.SUCESS, line);
                            LogObject data = JsonConvert.DeserializeObject<LogObject>(line);
                            if (data != null)
                            {
                                db.CheckPlayer(data);
                                Logger.Log(Logger.LogType.WARNING, "Distance: " + Vector3.Distance(statics.StringToVector3(data.PlayersPosition), statics.StringToVector3(data.TargetPosition)) + "m");
                            }
                        }


                    }
                    if (counter > lastReadLine)
                        lastReadLine = counter;

                    fs.Close();
                }
            }
            catch(Exception ex) 
            {
                Logger.Log(Logger.LogType.ERROR, ex.StackTrace); 
            }
        }
    }
}
