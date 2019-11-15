using Horizon.Reflection;

namespace Horizon.OData.Factories
{
    internal static class NameFactory
    {
        internal static string GetControllerName(ControllerData controller)
        {
            const string suffix = "Controller";

            var name = GetMemberName(controller.ControllerType, suffix);

            if (name == null)
            {
                throw new ControllerException($"The name '{controller.ControllerType.Name}' does have the suffix '{suffix}'.", controller.ControllerType);
            }

            return name;
        }

        internal static string GetProfileName(ProfileData profile)
        {
            const string suffix = "Profile";

            var name = GetMemberName(profile.ProfileType, suffix);

            if (name == null)
            {
                throw new ProfileException($"The name '{profile.ProfileType.Name}' does have the suffix '{suffix}'.", profile.ProfileType);
            }

            return name;
        }

        private static string GetMemberName<TMemberData>(TMemberData member, string suffix) where TMemberData : MemberData
        {
            return member.Name.EndsWith(suffix) ? member.Name.Substring(0, member.Name.Length - suffix.Length) : null;
        }
    }
}