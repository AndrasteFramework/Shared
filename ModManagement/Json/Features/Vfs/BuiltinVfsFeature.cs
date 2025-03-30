using System.Collections.Generic;

namespace Andraste.Shared.ModManagement.Json.Features.Vfs
{
    public class BuiltinVfsFeature
    {
        public string[] Directories { get; set; }
        public Dictionary<string, string> Files { get; set; }
    }
}
