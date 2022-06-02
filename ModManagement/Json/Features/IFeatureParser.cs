using System.Text.Json.Nodes;

namespace Andraste.Shared.ModManagement.Json.Features
{
    public interface IFeatureParser
    {
        public dynamic Parse(JsonObject obj);
    }
}