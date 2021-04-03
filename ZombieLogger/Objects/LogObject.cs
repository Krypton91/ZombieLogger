using Newtonsoft.Json;
using ZombieLogger.Helper;

namespace ZombieLogger.Objects
{

    public class LogObject
    {
        [JsonProperty]
        public string Reason { get; set; }
        public string PlayerName { get; set; }

        public string Steam64 { get; set; }

        public string Weapon { get; set; }

        public string PlayersPosition { get; set; }

        public string TimeStamp { get; set; }

        public string TargetPosition { get; set; }

        public string GetCorrectFormattedPlayerPos()
        {
            var CordArray = statics.VectorStringToCordArray(PlayersPosition);

            return CordArray[0] + ", " + CordArray[2] + ", " + CordArray[1];
        }

        public string GetCorrectFormattedTargetPos() 
        {
            var CordArray = statics.VectorStringToCordArray(TargetPosition);
            
            return CordArray[0] + ", " + CordArray[2] + ", " + CordArray[1];
        }
    }
}
