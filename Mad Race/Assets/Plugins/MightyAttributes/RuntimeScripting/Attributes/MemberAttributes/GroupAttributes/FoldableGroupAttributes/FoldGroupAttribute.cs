namespace MightyAttributes
{
    public class FoldGroupAttribute : BaseFoldGroupAttribute
    {
        public override ColorValue GetDefaultBackgroundColor() => ColorValue.White;

        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group (default: true).</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string (default: false).</param>
        /// <param name="backgroundColorName">The color name for the background color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the group.
        /// See the doc for more info on color names.</param>
        public FoldGroupAttribute(string groupName, bool drawName = true, bool nameAsCallback = false, string backgroundColorName = null,
            string contentColorName = null) : base(groupName, drawName, nameAsCallback, backgroundColorName, contentColorName)
        {
        }

        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group.</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: ColorValue.Default).
        /// See the doc fore more info on color values.</param>
        public FoldGroupAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor = ColorValue.Default) : base(groupName, drawName, nameAsCallback, backgroundColor, contentColor)
        {
        }

        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: ColorValue.Default).
        /// See the doc fore more info on color values.</param>
        public FoldGroupAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default) :
            base(groupName, backgroundColor, contentColor)
        {
        }

        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="backgroundColor">The background color value of the group (default: ColorValue.White).
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: ColorValue.Default).
        /// See the doc fore more info on color values.</param>
        public FoldGroupAttribute(ColorValue backgroundColor = ColorValue.White, ColorValue contentColor = ColorValue.Default) :
            base(backgroundColor, contentColor)
        {
        }
    }

    public class FoldAttribute : FoldGroupAttribute, IInheritDrawer
    {
        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group (default: true).</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string (default: false).</param>
        /// <param name="backgroundColorName">The color name for the background color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the group.
        /// See the doc for more info on color names.</param>
        public FoldAttribute(string groupName, bool drawName = true, bool nameAsCallback = false, string backgroundColorName = null,
            string contentColorName = null) : base(groupName, drawName, nameAsCallback, backgroundColorName, contentColorName)
        {
        }

        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group.</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: ColorValue.Default).
        /// See the doc fore more info on color values.</param>
        public FoldAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor = ColorValue.Default) : base(groupName, drawName, nameAsCallback, backgroundColor, contentColor)
        {
        }

        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: ColorValue.Default).
        /// See the doc fore more info on color values.</param>
        public FoldAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default)
            : base(groupName, backgroundColor, contentColor)
        {
        }
        
        /// <summary>
        /// Group together several members into a simple foldout.
        /// </summary>
        /// <param name="backgroundColor">The background color value of the group (default: ColorValue.White).
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: ColorValue.Default).
        /// See the doc fore more info on color values.</param>
        public FoldAttribute(ColorValue backgroundColor = ColorValue.White, ColorValue contentColor = ColorValue.Default) 
            : base(backgroundColor, contentColor)
        {
        }
    }
}