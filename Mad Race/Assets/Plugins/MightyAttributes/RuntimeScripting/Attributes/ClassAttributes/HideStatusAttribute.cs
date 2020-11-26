namespace MightyAttributes
{
    public class HideStatusAttribute : BaseClassAttribute
    {
        public HideStatus HideStatus { get; }

        /// <summary>
        /// Provides some options to hide things of your script in the inspector.
        /// </summary>
        /// <param name="hideStatus">The status indicating what to hide.
        /// See the doc for more info on hide status.</param>
        public HideStatusAttribute(HideStatus hideStatus) => HideStatus = hideStatus;
    }
    
    public class HideScriptFieldAttribute : HideStatusAttribute, IInheritDrawer
    {
        /// <summary>
        /// Hides the serialized fields of the script.
        /// </summary>
        public HideScriptFieldAttribute() : base(HideStatus.ScriptField)
        {
        }
    }

    public class HideSerializedFieldsAttribute : HideStatusAttribute, IInheritDrawer
    {
        /// <summary>
        /// Hides the field that links to the script.
        /// </summary>
        public HideSerializedFieldsAttribute() : base(HideStatus.SerializedFields)
        {
        }
    }
}