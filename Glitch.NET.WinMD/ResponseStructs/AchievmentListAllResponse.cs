using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Glitch.NET.WinMD.ResponseStructs
{
    [DataContract]
    public class GlitchAchievement
    {
        [DataMember]
        public string name = "";
        [DataMember]
        public string desc = "";
        [DataMember]
        public string url = "";
        [DataMember]
        public string image_60 = "";
        [DataMember]
        public string image_180 = "";
    }

    [DataContract]
    public class AchievmentListAllResponse {
        [DataMember]
        public int okay = 0;
        [DataMember]
        public int pages = 0;
        [DataMember]
        public int per_page = 0;
        [DataMember]
        public int page = 0;
        [DataMember]
        public int total = 0;
        [DataMember]
        public Dictionary<string, GlitchAchievement> items = new Dictionary<string, GlitchAchievement>();
    }
}
