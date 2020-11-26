namespace MightyAttributes
{
    public class HierarchyIconAttribute : BaseHierarchyAttribute
    {
        public string IconPath { get; }
        public bool PathAsCallback { get; }

        /// <summary>
        /// Draws an icon to the left of this script Game Object in the hierarchy.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// It begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="priority">The priority of the icon (default: 0).
        /// See the doc for more info on priority.</param>
        public HierarchyIconAttribute(string iconPath, int priority = 0) : base(priority)
        {
            IconPath = iconPath;
            PathAsCallback = false;
        }

        /// <summary>
        /// Draws an icon to the left of this script Game Object in the hierarchy.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// It begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="pathAsCallback">Choose whether or not the path should be consider as a callback of type string.</param>
        /// <param name="priority">The priority of the icon (default: 0).
        /// See the doc for more info on priority.</param>
        public HierarchyIconAttribute(string iconPath, bool pathAsCallback, int priority = 0) : base(priority)
        {
            IconPath = iconPath;
            PathAsCallback = pathAsCallback;
        }
        
        /// <summary>
        /// Draws an icon to the left of this script Game Object in the hierarchy.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// It begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="priorityCallback">Callback for the priority.
        /// The callback type should be int.</param>
        public HierarchyIconAttribute(string iconPath, string priorityCallback) : base(priorityCallback)
        {
            IconPath = iconPath;
            PathAsCallback = false;
        }

        /// <summary>
        /// Draws an icon to the left of this script Game Object in the hierarchy.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// It begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="pathAsCallback">Choose whether or not the path should be consider as a callback of type string.</param>
        /// <param name="priorityCallback">Callback for the priority.
        /// The callback type should be int.</param>
        public HierarchyIconAttribute(string iconPath, bool pathAsCallback, string priorityCallback) : base(priorityCallback)
        {
            IconPath = iconPath;
            PathAsCallback = pathAsCallback;
        }
    }
}