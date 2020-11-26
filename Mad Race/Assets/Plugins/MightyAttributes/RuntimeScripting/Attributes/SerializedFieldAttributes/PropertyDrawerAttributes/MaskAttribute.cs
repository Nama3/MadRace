using System;

namespace MightyAttributes
{
    public class MaskAttribute : BasePropertyDrawerAttribute
    {
        public string[] MaskNames { get; }

        public string MaskNamesCallback { get; }
        
        [Obsolete("Mask constructor should have one or more parameters", true)]
        public MaskAttribute() : base(FieldOption.Nothing){}
        
        /// <summary>
        /// Displays a mask dropdown that allows you to select multiple flags for an int.
        /// </summary>
        /// <param name="maskNames">The names of all the flags available for this mask.</param>
        public MaskAttribute(params string[] maskNames) : base(FieldOption.Nothing) => MaskNames = maskNames;
        
        /// <summary>
        /// Displays a mask dropdown that allows you to select multiple flags for an int.
        /// </summary>
        /// <param name="maskNames">The names of all the flags available for this mask.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public MaskAttribute(string[] maskNames, FieldOption options = FieldOption.Nothing) : base(options) => MaskNames = maskNames;

        /// <summary>
        /// Displays a mask dropdown that allows you to select multiple flags for an int.
        /// </summary>
        /// <param name="maskNamesCallback">The callback for all the names of all the flags available for this mask.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public MaskAttribute(string maskNamesCallback, FieldOption options = FieldOption.Nothing) : base(options) =>
            MaskNamesCallback = maskNamesCallback;
    }
}