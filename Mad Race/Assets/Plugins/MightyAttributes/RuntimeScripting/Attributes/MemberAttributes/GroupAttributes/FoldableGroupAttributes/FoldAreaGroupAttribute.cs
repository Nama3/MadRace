namespace MightyAttributes
{
    public class FoldAreaGroupAttribute : BaseFoldGroupAttribute
    {
        public bool DrawLine { get; }
        public ColorValue LineColor { get; } = ColorValue.Contrast;

        public override ColorValue GetDefaultBackgroundColor() => ColorValue.White;

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group (default: true).</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string (default: false).</param>
        /// <param name="backgroundColorName">The color name for the background color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: Contrast).
        /// See the doc fore more info on color values.</param>
        public FoldAreaGroupAttribute(string groupName, bool drawName = true, bool nameAsCallback = false,
            string backgroundColorName = null, string contentColorName = null, bool drawLine = true,
            ColorValue lineColor = ColorValue.Contrast) : base(groupName, drawName, nameAsCallback, backgroundColorName, contentColorName)
        {
            DrawLine = drawLine;
            LineColor = lineColor;
        }

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group.</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: Contrast).
        /// See the doc fore more info on color values.</param>
        public FoldAreaGroupAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor = ColorValue.Default, bool drawLine = true, ColorValue lineColor = ColorValue.Contrast)
            : base(groupName, drawName, nameAsCallback, backgroundColor, contentColor)
        {
            DrawLine = drawLine;
            LineColor = lineColor;
        }

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public FoldAreaGroupAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default)
            : base(groupName, backgroundColor, contentColor) => DrawLine = true;

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="backgroundColor">The background color value of the group (default: White).
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public FoldAreaGroupAttribute(ColorValue backgroundColor = ColorValue.White, ColorValue contentColor = ColorValue.Default)
            : base(backgroundColor, contentColor) => DrawLine = false;
    }

    public class FoldAreaAttribute : FoldAreaGroupAttribute, IInheritDrawer
    {
        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group (default: true).</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string (default: false).</param>
        /// <param name="backgroundColorName">The color name for the background color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: Contrast).
        /// See the doc fore more info on color values.</param>
        public FoldAreaAttribute(string groupName, bool drawName = true, bool nameAsCallback = false,
            string backgroundColorName = null, string contentColorName = null, bool drawLine = true,
            ColorValue lineColor = ColorValue.Contrast)
            : base(groupName, drawName, nameAsCallback, backgroundColorName, contentColorName, drawLine, lineColor)
        {
        }

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group.</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: Contrast).
        /// See the doc fore more info on color values.</param>
        public FoldAreaAttribute(string groupName, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor = ColorValue.Default, bool drawLine = true, ColorValue lineColor = ColorValue.Contrast)
            : base(groupName, drawName, nameAsCallback, backgroundColor, contentColor, drawLine, lineColor)
        {
        }

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public FoldAreaAttribute(string groupName, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default)
            : base(groupName, backgroundColor, contentColor)
        {
        }

        /// <summary>
        /// Group together several members into a foldable area.
        /// </summary>
        /// <param name="backgroundColor">The background color value of the group (default: White).
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public FoldAreaAttribute(ColorValue backgroundColor = ColorValue.White, ColorValue contentColor = ColorValue.Default)
            : base(backgroundColor, contentColor)
        {
        }
    }
}