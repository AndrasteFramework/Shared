using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Andraste.Shared.ModManagement
{
    /// <summary>
    /// This class will search a given folder recursively for potential mods (that list should be filtered by trying
    /// to parse the mod information of each mod)
    /// </summary>
    public static class PotentialModEnumerator
    {
        public static IEnumerable<string> FindAllPotentialMods(string basePath, bool recursive = false)
        {
            if (Directory.Exists(basePath))
            {
                var dirs = Directory.EnumerateDirectories(basePath, "*",
                    recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                return dirs.Where(x => File.Exists(Path.Combine(x, "mod.json")));
            }
            else
            {
                return Array.Empty<string>();
            }
        } 
    }
}