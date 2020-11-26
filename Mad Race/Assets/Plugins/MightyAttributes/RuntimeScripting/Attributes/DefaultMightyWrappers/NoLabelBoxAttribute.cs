namespace MightyAttributes
{
    [Box("GroupName", false, true, drawLine: false)]
    public class NoLabelBoxAttribute : BaseWrapperAttribute
    {
        public string GroupName { get; }

        /// <summary>
        /// Group together several fields inside a simple box area with no label.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        public NoLabelBoxAttribute(string groupName) => GroupName = groupName;
    }
}