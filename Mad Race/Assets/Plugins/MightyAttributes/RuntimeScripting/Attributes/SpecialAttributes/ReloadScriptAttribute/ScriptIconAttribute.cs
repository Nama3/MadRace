namespace MightyAttributes
{
    public class ScriptIconAttribute : BaseReloadScriptsAttribute
    {
        public string IconPath { get; }
        public bool PathAsCallback { get; }

        public int Priority { get; }
        public string PriorityCallback { get; }

        /// <summary>
        /// Changes the icon of the script and its children types.
        /// If you don’t want the icon to be applied for a specific child type of the script, you can use the [IgnoreScriptIcon] Attribute on this child.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// The path begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="priority">The priority of the icon (default: 0).
        /// See the doc for more info on priority.</param>
        public ScriptIconAttribute(string iconPath, int priority = 0)
        {
            IconPath = iconPath;
            Priority = priority;
        }

        /// <summary>
        /// Changes the icon of the script and its children types.
        /// If you don’t want the icon to be applied for a specific child type of the script, you can use the [IgnoreScriptIcon] Attribute on this child.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// The path begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="pathAsCallback">Choose whether or not the path should be consider as a callback of type string.</param>
        /// <param name="priority">The priority of the icon (default: 0).
        /// See the doc for more info on priority.</param>
        public ScriptIconAttribute(string iconPath, bool pathAsCallback, int priority = 0)
        {
            IconPath = iconPath;
            PathAsCallback = pathAsCallback;
            Priority = priority;
        }
        
        /// <summary>
        /// Changes the icon of the script and its children types.
        /// If you don’t want the icon to be applied for a specific child type of the script, you can use the [IgnoreScriptIcon] Attribute on this child.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// It begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="priorityCallback">Callback for the priority.
        /// The callback type should be int.</param>
        public ScriptIconAttribute(string iconPath, string priorityCallback)
        {
            IconPath = iconPath;
            PriorityCallback = priorityCallback;
        }
        
        /// <summary>
        /// Changes the icon of the script and its children types.
        /// If you don’t want the icon to be applied for a specific child type of the script, you can use the [IgnoreScriptIcon] Attribute on this child.
        /// </summary>
        /// <param name="iconPath">The path of the icon.
        /// It begins in the Assets folder, you can make it starts by “Assets/”, but it’s not required to make it work.</param>
        /// <param name="pathAsCallback">Choose whether or not the path should be consider as a callback of type string.</param>
        /// <param name="priorityCallback">Callback for the priority.
        /// The callback type should be int.</param>
        public ScriptIconAttribute(string iconPath, bool pathAsCallback, string priorityCallback)
        {
            IconPath = iconPath;
            PathAsCallback = pathAsCallback;
            PriorityCallback = priorityCallback;
        }
    }
}