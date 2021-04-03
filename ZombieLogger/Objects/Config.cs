using Newtonsoft.Json;
namespace ZombieLogger.Objects
{
    public class Config
    {
        [JsonProperty]
        public string DataBaseIP { get; set; }
        public string DataBaseName { get; set; }
        public string DataBaseUsername { get; set; }

        public string DataBasePassword { get; set; }

        public string ServerProfilePath { get; set; }

    }
}
