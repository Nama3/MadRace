namespace MightyAttributes
{
    public class ArraySizeAttribute : BaseAutoValueAttribute
    {
        public int Size { get; }
        public string SizeCallback { get; }

        /// <summary>
        /// Allows you to force the size of an array.
        /// </summary>
        /// <param name="size">The forced size of the array</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public ArraySizeAttribute(int size, bool executeInPlayMode = false) : base(executeInPlayMode) => Size = size;

        /// <summary>
        /// Allows you to force the size of an array.
        /// </summary>
        /// <param name="sizeCallback">Callback for the forced size of the array.
        /// The callback type should be int.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public ArraySizeAttribute(string sizeCallback, bool executeInPlayMode = false) : base(executeInPlayMode) =>
            SizeCallback = sizeCallback;
    }
}