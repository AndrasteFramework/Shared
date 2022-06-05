using System.Text.Json;
using System.Text.Json.Nodes;

namespace Andraste.Shared.ModManagement.Json.Features.Plugin
{
    public class PluginFeatureParser : IFeatureParser
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
            return obj.Deserialize<BuiltinPluginFeature>(ReadingOptions);
        }
    }
}