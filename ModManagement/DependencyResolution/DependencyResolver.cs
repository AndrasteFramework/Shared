using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using Semver;

namespace Andraste.Shared.ModManagement.DependencyResolution
{
    public class DependencyResolver
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        public static IDependencyVersionRequirement ParseDependency(string sourceSlug, KeyValuePair<string, string> entry)
        {
            if (entry.Value.StartsWith("file:"))
            {
                throw new NotImplementedException("File dependencies are not implemented yet");
            }

            return new SemVerRangeRequirement(sourceSlug, entry.Key, SemVersionRange.ParseNpm(entry.Value));
        }

        internal class SemVerRangeRequirement : IDependencyVersionRequirement
        {
            public string TargetSlug { get; }
            public string SourceSlug { get; }
            public SemVersionRange Range { get; }

            public SemVerRangeRequirement(string sourceSlug, string targetSlug, SemVersionRange range)
            {
                SourceSlug = sourceSlug;
                TargetSlug = targetSlug;
                Range = range;
            }

            public bool IsSatisfiedBy(SemVersion version)
            {
                return Range.Contains(version);
            }
        }

        // public static void ValidateRequirementFulfillment(Dictionary<string, List<SemVersion>> availableModVersions,
        //     List<IDependencyVersionRequirement> requirements)
        // {
        // TODO: Implement
        //     
        // }

        /// <summary>
        /// Validates if the requirements can be fulfilled with the given mods. The use case would be a given
        /// installation that is about to be booted up. <br />
        /// It can neither directly find contradictions between requirements (it's just that one of the requirements
        /// can't be satisfied for the given mod versions) and it also can't derive which mod version to pick,
        /// so that all requirements are satisfied. 
        /// </summary>
        /// <param name="availableMods">The mods and their version that are available</param>
        /// <param name="requirements">The requirements that need to be fulfilled</param>
        /// <returns>Whether we fulfill all requirements or not.</returns>
        public static List<RequirementViolation> ValidateRequirementsSimple(Dictionary<string, SemVersion> availableMods,
            List<IDependencyVersionRequirement> requirements)
        {
            // A "non simple" approach could build a dependency tree and then working them step by step. That way one
            // can check violations more efficiently while also deriving a load order, finding contradictions and
            // thus potentially interesting versions. 

            var errors = new List<RequirementViolation>();
            var disabledMods = new HashSet<string>();
            int previousIterationErrors;

            do
            {
                previousIterationErrors = errors.Count;
                ValidateIteration(availableMods, requirements, disabledMods, errors);
            } while (errors.Count > previousIterationErrors);

            return errors;
        }

        private static void ValidateIteration(Dictionary<string, SemVersion> availableMods,
            List<IDependencyVersionRequirement> requirements, HashSet<string> disabledMods,
            List<RequirementViolation> errors)
        {
            foreach (var requirement in requirements)
            {
                if (disabledMods.Contains(requirement.TargetSlug))
                {
                    disabledMods.Add(requirement.SourceSlug);
                    if (!errors.Any(err => err.Concerns(requirement)))
                    {
                        errors.Add(new RequirementViolation(RequirementViolation.Type.AlreadyDisabled, requirement));
                    }

                    continue;
                }

                if (!availableMods.TryGetValue(requirement.TargetSlug, out var version))
                {
                    disabledMods.Add(requirement.SourceSlug);
                    if (!errors.Any(err => err.Concerns(requirement)))
                    {
                        errors.Add(new RequirementViolation(RequirementViolation.Type.Missing, requirement));
                    }

                    continue;
                }
                
                if (version == null)
                {
                    Logger.Warn($"Specified SemVersion is null, most likely {requirement.TargetSlug} is missing a " +
                                "SemanticVersion field in it's mods.json, making it unresolvable by dependency");
                    continue;
                }

                // Depending on an unavailable version
                if (!requirement.IsSatisfiedBy(version))
                {
                    disabledMods.Add(requirement.SourceSlug);
                    if (!errors.Any(err => err.Concerns(requirement)))
                    {
                        errors.Add(new RequirementViolation(RequirementViolation.Type.WrongVersion, requirement));
                    }
                }
            }
        }
    }

    public interface IDependencyVersionRequirement
    {
        public bool IsSatisfiedBy(SemVersion version);
        public string SourceSlug { get; }
        public string TargetSlug { get; }
    }
}