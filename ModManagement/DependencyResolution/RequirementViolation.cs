namespace Andraste.Shared.ModManagement.DependencyResolution
{
    public class RequirementViolation
    {
        public enum Type
        {
            Missing,
            AlreadyDisabled,
            WrongVersion
        }
        
        public readonly Type ViolationType;
        public readonly string SourceSlug;
        public readonly string TargetSlug;
        public readonly string Message;

        public RequirementViolation(Type type, string sourceSlug, string targetSlug, string? message = null)
        {
            ViolationType = type;
            SourceSlug = sourceSlug;
            TargetSlug = targetSlug;
            Message = message ?? type switch
            {
                Type.AlreadyDisabled => $"{sourceSlug} has been disabled because it depends on {targetSlug} that has been disabled due to violations.",
                Type.Missing => $"{sourceSlug} has been disabled because it depends on the missing mod {targetSlug}",
                Type.WrongVersion => $"{sourceSlug} has been disabled because {targetSlug} is not in the required version range.",
                _ => message
            };
        }

        public RequirementViolation(Type type, IDependencyVersionRequirement requirement, string? message = null)
            : this(type, requirement.SourceSlug, requirement.TargetSlug, message)
        {
            if (message == null && type == Type.WrongVersion && requirement is DependencyResolver.SemVerRangeRequirement semver)
            {
                Message = $"{SourceSlug} has been disabled because {TargetSlug} is not in the required version range {semver.Range}.";
            }
        }

        /// <summary>
        /// Whether this violation concerns the resolution from source -> target.
        /// This is used to prevent reporting multiple violations for essentially the same mapping.
        /// </summary>
        /// <param name="sourceSlug">The source slug to compare against</param>
        /// <param name="targetSlug">The target slug to compare against</param>
        /// <returns>true, if this violation concerns the same source -> target mapping, i.e. if the slugs are identical</returns>
        public bool Concerns(string sourceSlug, string targetSlug)
        {
            return SourceSlug == sourceSlug && TargetSlug == targetSlug;
        }

        public bool Concerns(IDependencyVersionRequirement requirement)
        {
            return Concerns(requirement.SourceSlug, requirement.TargetSlug);
        }
    }
}