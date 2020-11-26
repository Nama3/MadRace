namespace MightyAttributes
{
    public class DisableIfAttribute : BaseEnableConditionAttribute
    {
        public string[] ConditionCallbacks { get; }

        /// <summary>
        /// Let you define the conditions on which the member should be disabled.
        /// The member will be disabled if one of the conditions is met.
        /// </summary>
        /// <param name="conditionCallbacks">Array of callbacks for all the conditions on which the member should be disabled.
        /// The callbacks type should be bool.</param>
        public DisableIfAttribute(params string[] conditionCallbacks) => ConditionCallbacks = conditionCallbacks;
    }
}
