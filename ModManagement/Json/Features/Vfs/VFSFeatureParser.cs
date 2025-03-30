using System.Text.Json;
using System.Text.Json.Nodes;
using Andraste.Shared.ModManagement.Json.Features;
using Andraste.Shared.ModManagement.Json.Features.Vfs;

namespace Andraste.Shared.ModManagement.Features
{
    public class VFSFeatureParser : IFeatureParser
    {
        private static readonly JsonSerializerOptions ReadingOptions = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            // Writing Option: PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            PropertyNameCaseInsensitive = true
        };
        
        public dynamic Parse(JsonObject obj)
        {
            return obj.Deserialize<BuiltinVfsFeature>(ReadingOptions);
        }
    }
}