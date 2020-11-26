using System;

namespace MightyAttributes
{
    [ItemNames("EnumNames", "OptionsCallback", true)]
    public class EnumArrayAttribute : BaseWrapperAttribute
    {
        public Type EnumType { get; }
        [CallbackName] public string OptionsCallback { get; }

        /// <summary>
        /// Creates an array with as many elements as the specified enum type has values, and displays the values of the enum for the elements labels.
        /// </summary>
        /// <param name="enumType">The type of the enum to work with.</param>
        /// <param name="options">Some drawing options for the field (default: DisableSizeField).</param>
        public EnumArrayAttribute(Type enumType, ArrayOption options = ArrayOption.DisableSizeField)
        {
            EnumType = enumType;
            OptionsCallback = options.ToString();
        }

        /// <summary>
        /// Creates an array with as many elements as the specified enum type has values, and displays the values of the enum for the elements labels.
        /// </summary>
        /// <param name="enumType">The type of the enum to work with.</param>
        /// <param name="optionsCallback">Callback for the drawing options of the field.
        /// The callback type should be ArrayOption.</param>
        public EnumArrayAttribute(Type enumType, string optionsCallback)
        {
            EnumType = enumType;
            OptionsCallback = optionsCallback;
        }

        private string[] EnumNames => Enum.GetNames(EnumType);
    }
}