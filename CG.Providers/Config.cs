using System;
using System.Collections.Generic;
using System.Text;

namespace CG.Providers
{
    public class Config
    {

        public static readonly string ApiUrl = "https://loveproj.fun/";
        public static readonly string AuthUserInfoUrl = "Account/getauthuserinfo";
        public static readonly string UpdateRefreshTokenUrl = "Account/longsessionupdate";
        public static readonly string CreateFirstSessionUrl = "MessangerSession/createfirstsession";
        public static readonly string CreateSessionUrl = "MessangerSession/createsession";
        public Config()
        {
            // var assembly = typeof(Config).GetTypeInfo().Assembly;
            //Stream stream = assembly.GetManifestResourceStream("config.json");
            //using(var reader =new System.IO.StreamReader(stream))
            //{
            // var json = reader.ReadToEnd();
            //ApiUrl = JsonConvert.DeserializeObject<String>(json);
            //}
        }

    }
}
