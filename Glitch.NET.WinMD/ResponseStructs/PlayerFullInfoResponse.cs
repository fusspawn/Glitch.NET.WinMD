using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glitch.NET.WinMD.ResponseStructs
{
    public class PlayerFullInfoResponse
    {
        public int ok = 0;
        public string player_name = "";
        public string player_tsid = "";
        public bool is_online = false;
        public long last_online = 0;
        public Dictionary<string, string> avatar = new Dictionary<string, string>();
        public Dictionary<string, int> stats = new Dictionary<string, int>();
        //public Dictionary<string, string> latest_skill = new Dictionary<string, string>();
        //public Dictionary<string, string> latest_acheivment = new Dictionary<string, string>();
        public Dictionary<string, string> location = new Dictionary<string, string>();
        public Dictionary<string, string> pol = new Dictionary<string, string>();
        public Dictionary<string, bool> relationship = new Dictionary<string, bool>();
    }
}
