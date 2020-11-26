namespace MightyAttributes
{
    public class DarkSimpleScrollGroupAttribute : BaseSimpleScrollGroupAttribute
    {
        public override ColorValue GetDefaultBackgroundColor() => ColorValue.SoftContrast;

        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="height">The fixed height of the scrollable area (default: 100).</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group (default: true).</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string (default: false).</param>
        /// <param name="backgroundColorName">The color name for the background color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: HardContrast).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollGroupAttribute(string groupName, float height = 100, bool drawName = true, bool nameAsCallback = false,
            string backgroundColorName = null, string contentColorName = null, bool drawLine = true,
            ColorValue lineColor = ColorValue.HardContrast)
            : base(groupName, height, drawName, nameAsCallback, backgroundColorName, contentColorName, drawLine, lineColor)
        {
        }

        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="height">The fixed height of the scrollable area.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group.</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: HardContrast).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollGroupAttribute(string groupName, float height, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor = ColorValue.Default, bool drawLine = true, ColorValue lineColor = ColorValue.HardContrast)
            : base(groupName, height, drawName, nameAsCallback, backgroundColor, contentColor, drawLine, lineColor)
        {
        }

        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="height">The fixed height of the scrollable area.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollGroupAttribute(string groupName, float height, ColorValue backgroundColor, 
            ColorValue contentColor = ColorValue.Default) : base(groupName, height, backgroundColor, contentColor, ColorValue.HardContrast)
        {
        }

        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="height">The fixed height of the scrollable area (default: 100).</param>
        /// <param name="backgroundColor">The background color value of the group (default: SoftContrast).
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollGroupAttribute(float height = 100, ColorValue backgroundColor = ColorValue.SoftContrast,
            ColorValue contentColor = ColorValue.Default) : base(height, backgroundColor, contentColor)
        {
        }
    }

    public class DarkSimpleScrollAttribute : DarkSimpleScrollGroupAttribute, IInheritDrawer
    {
        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="height">The fixed height of the scrollable area (default: 100).</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group (default: true).</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string (default: false).</param>
        /// <param name="backgroundColorName">The color name for the background color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="contentColorName">The color name for the content color of the group.
        /// See the doc for more info on color names.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: HardContrast).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollAttribute(string groupName, float height = 100, bool drawName = true, bool nameAsCallback = false,
            string backgroundColorName = null, string contentColorName = null, bool drawLine = true,
            ColorValue lineColor = ColorValue.HardContrast)
            : base(groupName, height, drawName, nameAsCallback, backgroundColorName, contentColorName, drawLine, lineColor)
        {
        }

        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="height">The fixed height of the scrollable area.</param>
        /// <param name="drawName">Choose whether or not the name of the group should be displayed at the top of the group.</param>
        /// <param name="nameAsCallback">Choose whether or not the name of the group should be considered as a callback of type string.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        /// <param name="drawLine">Choose whether or not to draw line between the group name and the content of the group (default: true).</param>
        /// <param name="lineColor">The color value of the line (default: HardContrast).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollAttribute(string groupName, float height, bool drawName, bool nameAsCallback, ColorValue backgroundColor,
            ColorValue contentColor = ColorValue.Default, bool drawLine = true, ColorValue lineColor = ColorValue.HardContrast)
            : base(groupName, height, drawName, nameAsCallback, backgroundColor, contentColor, drawLine, lineColor)
        {
        }
        
        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="groupName">The name of the group that is used to know what members should be grouped together.</param>
        /// <param name="height">The fixed height of the scrollable area.</param>
        /// <param name="backgroundColor">The background color value of the group.
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollAttribute(string groupName, float height, ColorValue backgroundColor, ColorValue contentColor = ColorValue.Default)
            : base(groupName, height, backgroundColor, contentColor)
        {
        }

        /// <summary>
        /// Group together several members inside a dark scrollable area of a specified height.
        /// All Scroll Groups with the same group name will use the height specify in the first attribute of that group name.
        /// </summary>
        /// <param name="height">The fixed height of the scrollable area (default: 100).</param>
        /// <param name="backgroundColor">The background color value of the group (default: SoftContrast).
        /// See the doc fore more info on color values.</param>
        /// <param name="contentColor">The content color value of the group (default: Default).
        /// See the doc fore more info on color values.</param>
        public DarkSimpleScrollAttribute(float height = 100, ColorValue backgroundColor = ColorValue.SoftContrast,
            ColorValue contentColor = ColorValue.Default) : base(height, backgroundColor, contentColor)
        {
        }
    }
}