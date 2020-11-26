namespace MightyAttributes
{
    public class AnimatorParameterAttribute : BaseAutoValueAttribute
    {
        public string ParameterName { get; }
        public bool NameAsCallback { get; }

        /// <summary>
        /// Initialize an int field with the value of the hash code of the specified Animator parameter name.
        /// </summary>
        /// <param name="parameterName">The name of the animator parameter you want to convert as a hash code.</param>
        /// <param name="nameAsCallback">Choose whether or not parameterName should be considered as a Callback of type string (default: false).</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public AnimatorParameterAttribute(string parameterName, bool nameAsCallback = false, bool executeInPlayMode = false) 
            : base(executeInPlayMode)
        {
            ParameterName = parameterName;
            NameAsCallback = nameAsCallback;
        }
    }
}