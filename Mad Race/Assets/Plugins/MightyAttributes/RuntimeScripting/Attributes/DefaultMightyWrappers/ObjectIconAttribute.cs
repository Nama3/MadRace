// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedType.Global


// ReSharper disable MemberCanBeProtected.Global
namespace MightyAttributes
{
    [HierarchyIcon("IconPath", true, "Priority"), ScriptIcon("IconPath", true, "Priority")]
    public class ObjectIconAttribute : BaseWrapperAttribute
    {
        public string IconPath { get; }
        
        public int Priority { get; }
        
        /// <summary>
        /// Draws an icon in the hierarchy and changes the script icon.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// The path begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="priority">The priority of the icon (default: 0).
        /// See the doc for more info on priority.</param>
        public ObjectIconAttribute(string iconPath, int priority = 0)
        {
            IconPath = iconPath;
            Priority = priority;
        }
    }
}