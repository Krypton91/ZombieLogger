using System;
using System.IO;
using ZombieLogger.Objects;
using Newtonsoft.Json;

namespace ZombieLogger.Helper
{
   
    internal class ConfigManager
    {
        internal Config Config;
        private string CONFIG_DIR = AppDomain.CurrentDomain.BaseDirectory + @"Config\";
        internal ConfigManager() 
        {
            Config = new Config();
            if(!File.Exists(CONFIG_DIR + "config.json")) 
            {
                Config = WriteDefaultConfig();
            }
            else 
            {
                Config = LoadCondig();
            }
        }
        private Config WriteDefaultConfig() 
        {
            if (!Directory.Exists(CONFIG_DIR))
                Directory.CreateDirectory(CONFIG_DIR);

            try 
            {
                var config = DefaultSettings();
                var json = JsonConvert.SerializeObject(config, Formatting.Indented);
                using (var wr = new StreamWriter(CONFIG_DIR + "config.json"))
                {
                    wr.Write(json);
                }
                return config;
            }
            catch(Exception ex) 
            {
                Logger.Log(Logger.LogType.ERROR, "FATAL ERROR: " + ex.StackTrace);
            }
            return null;
        }

        private Config LoadCondig() 
        {
            try 
            {
                string FileContent;
                using(var reader = new StreamReader(CONFIG_DIR + "config.json")) 
                {
                    FileContent = reader.ReadToEnd();
                }

                return JsonConvert.DeserializeObject<Config>(FileContent);

            }
            catch(Exception ex) 
            {
                Logger.Log(Logger.LogType.ERROR, ex.StackTrace);
            }
            return null;
        }

        private Config DefaultSettings() 
        {
            var entry = new Config
            {
                DataBaseIP = "127.0.0.1",
                DataBaseName = "ZombieLogger",
                DataBaseUsername = "root",
                DataBasePassword = "",
                ServerProfilePath = ""
            };

            return entry;
        }
    }
}
