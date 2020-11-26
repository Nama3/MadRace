#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public static class BuildTargetUtilities
    {
        public static BuildTargetGroup ToBuildTargetGroup(this BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneLinux64:
                    return BuildTargetGroup.Standalone;
                case BuildTarget.iOS:
                    return BuildTargetGroup.iOS;
                case BuildTarget.Android:
                    return BuildTargetGroup.Android;
                case BuildTarget.WebGL:
                    return BuildTargetGroup.WebGL;
                case BuildTarget.WSAPlayer:
                    return BuildTargetGroup.WSA;
                case BuildTarget.PS4:
                    return BuildTargetGroup.PS4;
                case BuildTarget.XboxOne:
                    return BuildTargetGroup.XboxOne;
                case BuildTarget.tvOS:
                    return BuildTargetGroup.tvOS;
                case BuildTarget.Switch:
                    return BuildTargetGroup.Switch;
                case BuildTarget.Lumin:
                    return BuildTargetGroup.Lumin;
                default:
                    return BuildTargetGroup.Unknown;
            }
        }

        public static BuildTarget[] ToBuildTargets(this BuildTargetGroup targetGroup)
        {
            switch (targetGroup)
            {
                case BuildTargetGroup.Standalone:
                    return new[]
                    {
                        BuildTarget.StandaloneOSX, BuildTarget.StandaloneWindows, BuildTarget.StandaloneWindows64,
                        BuildTarget.StandaloneLinux64
                    };
                case BuildTargetGroup.Android:
                    return new[] {BuildTarget.Android};
                case BuildTargetGroup.WebGL:
                    return new[] {BuildTarget.WebGL};
                case BuildTargetGroup.WSA:
                    return new[] {BuildTarget.WSAPlayer};
                case BuildTargetGroup.PS4:
                    return new[] {BuildTarget.PS4};
                case BuildTargetGroup.XboxOne:
                    return new[] {BuildTarget.XboxOne};
                case BuildTargetGroup.tvOS:
                    return new[] {BuildTarget.tvOS};
                case BuildTargetGroup.Switch:
                    return new[] {BuildTarget.Switch};
                case BuildTargetGroup.Lumin:
                    return new[] {BuildTarget.Lumin};
                default:
                    return new[] {BuildTarget.NoTarget};
            }
        }
    }
}
#endif