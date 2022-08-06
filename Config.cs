using MelonLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVRANTICRASH
{
    
  
    internal class ConfigMan
    {
        public static Config GetConfig()
        {
            return config;
        }
  
        internal static Config config;
        public static void SaveConfig()
        {
            config = new Config();

            if (!Directory.Exists(MelonUtils.GameDirectory + "\\CVRMG"))
            {
                Directory.CreateDirectory(MelonUtils.GameDirectory + "\\CVRMG");
            }
            if (!File.Exists(MelonUtils.GameDirectory + "\\CVRMG\\Config.Json"))
            {
                File.Create(MelonUtils.GameDirectory + "\\CVRMG\\Config.Json");
                File.WriteAllText(MelonUtils.GameDirectory + "\\CVRMG\\Config.Json", JsonConvert.SerializeObject(config));

            }
            try
            {
            }
            catch
            {
                MelonLogger.Msg("a");
            }
            try
            {
                LoadConfig();

            }
            catch
            {
                MelonLogger.Msg("b");
            }
        }
        public static void LoadConfig()
        {
            config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(MelonUtils.GameDirectory + "\\CVRMG\\Config.Json"));
        }

    }
    internal class Config
    {
        [JsonIgnore]
        public static string JOE = null;

        public float CVRACVersion = 1.0f;

        public bool joe = false;

        public bool PropAnti = true;

        public bool AntiSus = true;

        public bool Particle = true;

        public bool DynamicBone = true;

        public bool Light = true;

        public bool Mesh = true;

        public bool SkinnedMeshRender = true;

        public bool MeshRenderer = true;

        public bool Audio = true;

        public bool Material = true;

        public bool Shader = true;

        public bool WhitelistFriend = true;

        public List<string> UserBlackList = new List<string>();

        public List<string> AvatarWhitelist = new List<string>();

        public List<string> AvatarBlackList = new List<string>();

        public List<string> UserWhiteList = new List<string>();

    
    }

}
