#nullable enable
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Semver;

namespace Andraste.Shared.ModManagement.Json
{
    public class ModInformation
    {
        public string Slug { get; set; }
        public string Name { get; set; }
        public SemVersion SemanticVersion { get; set; }
        public string DisplayVersion { get; set; }
        public string Description { get; set; }
        public string[] Authors { get; set; }
        /// <summary>
        /// Map of dependent-slug to a semver range
        /// </summary>
        public Dictionary<string, string>? Dependencies { get; set; }
        public Dictionary<string, ModConfiguration> Configurations { get; set; }
    }

    public class ModConfiguration
    {
        public Dictionary<string, JsonObject> Features { get; set; }
        public Dictionary<string, dynamic> _parsedFeatures = new Dictionary<string, dynamic>();
    }
}
#nullable restore