using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Glitch.NET.WinMD.ResponseStructs
{
    [DataContract]
    public class AuthCheckResponse
    {
        [DataMember]
        public int ok = 0;
        
        [DataMember]
        public string player_tsid = "";

        [DataMember]
        public string player_name = "";
         
        [DataMember]
        public Dictionary<string, int> scope = new Dictionary<string, int>();
    }
}
