using System.Collections.Generic;
using System.Text.Json.Nodes;

namespace Andraste.Shared.ModManagement.Json
{
    public class ModSettings
    {
        public Dictionary<string, JsonObject> FeatureSettings { get; set; }
        public ModSetting[] EnabledMods { get; set; }
    }

    public class ModSetting
    {
        public string ModPath { get; set; }
        public string ActiveConfiguration { get; set; }
    }
}