namespace MightyAttributes
{
    [DarkBox("GroupName", false, true, drawLine: false)]
    public class NoLabelDarkBoxAttribute : BaseWrapperAttribute
    {
        public string GroupName { get; }

        /// <summary>
        /// Group together several fields inside a dark box area with no label.
        /// </summary>
        /// <param name="groupName">The name of the group.</param>
        public NoLabelDarkBoxAttribute(string groupName) => GroupName = groupName;
    }
}