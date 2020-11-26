namespace MightyAttributes
{
    public class EnableIfAttribute : BaseEnableConditionAttribute
    {
        public string[] ConditionCallbacks { get; }

        /// <summary>
        /// Let you define the conditions on which the member should be enabled.
        /// The member will be enabled if all the conditions are met.
        /// </summary>
        /// <param name="conditionCallbacks">Array of callbacks for all the conditions on which the member should be enabled.
        /// The callbacks type should be bool.</param>
        public EnableIfAttribute(params string[] conditionCallbacks) => ConditionCallbacks = conditionCallbacks;
    }
}