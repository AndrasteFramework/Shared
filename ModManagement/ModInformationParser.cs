using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Andraste.Shared.ModManagement.Json;
using Semver;

namespace Andraste.Shared.ModManagement
{
    public static class ModInformationParser
    {
        private static readonly JsonSerializerOptions ReadingOptions = new JsonSerializerOptions()
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            // Writing Option: PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            PropertyNameCaseInsensitive = true,
            Converters = { new SemverJsonConverter() }
        };
        
        public static ModInformation ParseString(string fileContent)
        {
            return JsonSerializer.Deserialize<ModInformation>(fileContent, ReadingOptions);
        }

        public static bool Validate(ModInformation modInfo)
        {
            return modInfo.Authors != null && modInfo.Authors.Length > 0 && modInfo.Slug != null &&
                   modInfo.Name != null;
        }
    }

    internal class SemverJsonConverter : JsonConverter<SemVersion>
    {
        public override SemVersion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return SemVersion.Parse(reader.GetString(), SemVersionStyles.Any);
        }

        public override void Write(Utf8JsonWriter writer, SemVersion value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}