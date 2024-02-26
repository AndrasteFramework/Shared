using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Andraste.Shared.ModManagement;
using Andraste.Shared.ModManagement.Json;

namespace Andraste.Shared.Util
{
    #nullable enable
    [Obsolete("This is only a utility for a transition period/simple launchers. " +
              "Always prefer building the mods.json with your dedicated launcher")]
    public static class ModJsonBuilder
    {
        /// <summary>
        /// In case we didn't have a mods.json passed in, we're generating one on the fly.
        /// </summary>
        public static byte[]? WriteModsJson(string modsFolder)
        {
            var settings = new ModSettings();
            var enabledMods = new List<ModSetting>();
            
            foreach (var mod in PotentialModEnumerator.FindAllPotentialMods(modsFolder))
            {
                var modInfo = ModInformationParser.ParseString(File.ReadAllText(Path.Combine(mod, "mod.json")));
                Console.WriteLine($"Found a potential mod in {mod}: {modInfo.Slug ?? "Could not load mod.json"}");
                if (modInfo.Slug != null)
                {
                    Console.WriteLine($"Enabling {modInfo.Name} by {string.Join(", ", modInfo.Authors)}");
                    // TODO: if mod has more than one configuration, check if one is called default etc.

                    string active;
                    if (modInfo.Configurations.Count == 1)
                    {
                        active = modInfo.Configurations.First().Key;
                    }
                    else
                    {
                        if (modInfo.Configurations.ContainsKey("default"))
                        {
                            active = "default";
                        }
                        else
                        {
                            Console.Error.WriteLine($"{modInfo.Slug} has {modInfo.Configurations.Count} configurations, " +
                                                    "but none of it is called `default`. Manually picking one is TODO");
                            return null;
                        }
                    }

                    enabledMods.Add(new ModSetting
                    {
                        ModPath = mod,
                        ActiveConfiguration = active
                    });
                }
            }

            // TODO: Do things with the features to resolve conflicts
            settings.EnabledMods = enabledMods.ToArray();
            return JsonSerializer.SerializeToUtf8Bytes(settings, new JsonSerializerOptions { WriteIndented = true });
        }
    }
    #nullable restore
}