using System;
using System.Numerics;

namespace ZombieLogger.Helper
{
    public static class statics
    {
        public static string[] VectorStringToCordArray(string sVector)
        {
            if (sVector.StartsWith("<") && sVector.EndsWith(">"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }

            // split the items
            string[] sArray = sVector.Split(',');
            return sArray;
        }

        public static string Distance(string position1, string position2) 
        {
            Vector3 pos1 = StringToVector3(position1);
            Vector3 pos2 = StringToVector3(position2);

            return Math.Round(Vector3.Distance(pos1, pos2), 0, MidpointRounding.AwayFromZero).ToString();
        }

        public static Vector3 StringToVector3(string sVector)
        {
            // Remove the parentheses
            if (sVector.StartsWith("<") && sVector.EndsWith(">"))
            {
                sVector = sVector.Substring(1, sVector.Length - 2);
            }
            
            if(sVector.Contains(" ") || sVector.Contains("  ")) 
            {
                sVector.Replace(" ", "");
                sVector.Replace("   ", "");
            }
            // split the items
            string[] sArray = sVector.Split(',');

            // store as a Vector3
            Vector3 result = new Vector3(
                float.Parse(sArray[0]),
                float.Parse(sArray[2]),
                float.Parse(sArray[1]));

            return result;
        }
    }
}
