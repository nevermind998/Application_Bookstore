using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoboPraksa_Zadatak1.Helper
{
    public class ProtectionHelper
    {
        private IConfiguration _configuration;
        private IDataProtector _protector;

        private static readonly ProtectionHelper singleton;

        public ProtectionHelper()
        {

        }
        public static ProtectionHelper Singleton
        {
            get { return singleton; }
        }

        static ProtectionHelper()
        {
            singleton = new ProtectionHelper();
        }

        public void SetConfig(IConfiguration configuration)
        {
            singleton._configuration = configuration;
        }
        public void SetProvider(IDataProtectionProvider provider, String protectionKey)
        {
            _protector = provider.CreateProtector(protectionKey);
        }

        public String GetSectionValue(String key)
        {
            String[] keys = key.Split(":");
            String value = _configuration[key + ":Value"]; //putanja

            bool isProtected = bool.Parse(_configuration[key + ":Protected"]);

            if (!isProtected) //prvi put pristupamo bazi
            {
                String appSettingsPath = System.IO.Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                String json = System.IO.File.ReadAllText(appSettingsPath);
                dynamic jsonObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
                jsonObject[keys[0]][keys[1]]["Value"] = _protector.Protect(value);
                jsonObject[keys[0]][keys[1]]["Protected"] = true;

                String output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObject,Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(appSettingsPath, output);
                return value;
            }

            return _protector.Unprotect(value);
        }



    }
}
