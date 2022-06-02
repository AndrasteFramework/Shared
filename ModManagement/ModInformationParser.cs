using System.Text.Json;
using System.Text.Json.Nodes;
using Andraste.Shared.ModManagement.Json;
using Andraste.Shared.ModManagement.Json.Features;

namespace Andraste.Shared.ModManagement
{
    public static class ModInformationParser
    {
        private static readonly JsonSerializerOptions ReadingOptions = new JsonSerializerOptions()
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            // Writing Option: PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            PropertyNameCaseInsensitive = true
        };
        
        public static ModInformation ParseString(string fileContent)
        {
            return JsonSerializer.Deserialize<ModInformation>(fileContent, ReadingOptions);
        }

        public static BuiltinVfsFeature ParseBuiltinFeature(JsonObject obj)
        {
            return obj.Deserialize<BuiltinVfsFeature>(ReadingOptions);
        }

        public static bool Validate(ModInformation modInfo)
        {
            return modInfo.Authors != null && modInfo.Authors.Length > 0 && modInfo.Slug != null &&
                   modInfo.Name != null;
        }
    }
}