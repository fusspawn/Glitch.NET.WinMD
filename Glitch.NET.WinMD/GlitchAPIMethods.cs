using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web;
using Windows.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Glitch.NET.WinMD.ResponseStructs;


namespace Glitch.NET.WinMD
{
    public sealed class GlitchAPI
    {
        public string PlayerTSID = "";
        public bool IsSignedIn = false;
        
        private static readonly string API_BASE_URL = "https://api.glitch.com";
        private string AccessToken = null;

        private string APIKEY = "100-07f054de4cb20859f1985b85b0ad714036b85ef6";
        private string APISECRET = "5b931d4fd923158976c2f93b0e35816813ce8b96";
        private bool AuthInProgress = false;
        private int TokenLength = 60;



        public GlitchAPI(string Key, string Secret)
        {
            APIKEY = Key;
            APISECRET = Secret;
        }

        public GlitchAPI() { }

        #region API Calls
        public async Task<bool> authorise(string Scope) {
            AuthInProgress = true;
            var Result = false;
            var param = "/oauth2/authorize?response_type=token&client_id="
                + APIKEY + "&scope=" + Scope;
                
            param = API_BASE_URL + param;
            var result = await WebAuthenticationBroker.AuthenticateAsync(
                WebAuthenticationOptions.Default, 
                new Uri(param + "&redirect_uri=http://example.com/nullauth"),
                new Uri("http://example.com/nullauth")
            );

            if (!result.ResponseData.ToString().Contains("#access_token="))
                throw new Exception("API Error: " + result.ResponseData);

            else {
                AuthInProgress = false;
                return ExtractToken(result.ResponseData);
            }
        }

        public async Task<AuthCheckResponse> authCheck(string Scope) {
            var Okay = false;
            if (!IsSignedIn)
                Okay = await authorise(Scope);

            if (!Okay)
                return null;

            string AuthResponse = await RequestURLTaskAsync(BuildAuthedRequestURL("auth.check", null));
            AuthCheckResponse Check = ParseData<AuthCheckResponse>(AuthResponse);
            if (Check.ok == 1)
            {
                IsSignedIn = true;
                PlayerTSID = Check.player_tsid;
            }
            return Check;
        }
        public async Task<bool> skillsListLearning()
        {
            return ParseData<SkillsLearningResponse>(await RequestURLTaskAsync(BuildAuthedRequestURL("skills.listLearning", null))).ok
                == 1;
        }
        public async Task<Dictionary<string, GlitchAchievement>> achievementslistAll(int page = 1, int per_page = 10)
        {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            urlParams.Add("page", page.ToString());
            urlParams.Add("per_page", per_page.ToString());

            return ParseData<AchievmentListAllResponse>(await RequestURLTaskAsync(BuildAuthedRequestURL("achievements.listAll", urlParams))).items;
        }
        public async Task<PlayerFullInfoResponse> playersFullInfo(string tsid) {
            Dictionary<string, string> urlParams = new Dictionary<string, string>();
            urlParams.Add("player_tsid", tsid.ToString());
            return ParseData<PlayerFullInfoResponse>(await RequestURLTaskAsync(BuildAuthedRequestURL("players.fullInfo", urlParams)));
        }
        #endregion

        #region Utils
        private bool ExtractToken(string s)
        {
            int sindex = s.IndexOf("#access_token=");
            sindex += 14;
            AccessToken = s.Substring(sindex, TokenLength);
            IsSignedIn = true;
            return true;
        }
        private string BuildAuthedRequestURL(string Method, Dictionary<string, string> ToEncode) 
        {
            string result = API_BASE_URL + "/simple/" + Method + "?oauth_token=" + AccessToken;
            if (ToEncode == null)
                return result;
            foreach (KeyValuePair<string, string> kvp in ToEncode) 
                    result += "&" + kvp.Key + "=" + kvp.Value.ToString();
            return result;
        }       
        private async Task<string> RequestURLTaskAsync(string URL)
        {
            var http = new HttpClient();
            var resp = await http.GetAsync(URL);
            var retval = resp.Content.ReadAsString();
            return retval;
        }
        private T ParseData<T>(string Data)
        {
           var o =  JsonConvert.DeserializeObject<T>(Data);
           return o;
        }
        #endregion
    }
}
