namespace MightyAttributes
 {
     [EndHorizontal, Width("LabelWidth", "ContentWidth")]
     public class EndHorizontalWidthAttribute : BaseWrapperAttribute
     {
         public float LabelWidth { get; }
         public float? ContentWidth { get; }
 
         /// <summary>
         /// Ends a horizontal area and manually set the width of the member's label.
         /// </summary>
         /// <param name="labelWidth">The width of the label.</param>
         public EndHorizontalWidthAttribute(float labelWidth) => LabelWidth = labelWidth;

         /// <summary>
         /// Ends a horizontal area and manually set the width of the member's label.
         /// Forces the width of the member's content.
         /// </summary>
         /// <param name="labelWidth">The width of the label.</param>
         /// <param name="contentWidth">The width of the content.</param>
         public EndHorizontalWidthAttribute(float labelWidth, float contentWidth)
         {
             LabelWidth = labelWidth;
             ContentWidth = contentWidth;
         }
     }
 }