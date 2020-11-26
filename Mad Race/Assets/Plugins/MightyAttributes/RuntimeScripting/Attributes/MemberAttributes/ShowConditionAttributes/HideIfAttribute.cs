namespace MightyAttributes
{
    public class HideIfAttribute : BaseShowConditionAttribute
    {
        public string[] ConditionCallbacks { get; }

        /// <summary>
        /// Let you define the conditions on which the member should be hidden.
        /// The member will be hidden if one of the conditions is met.
        /// </summary>
        /// <param name="conditionCallbacks">Array of callbacks for all the conditions on which the member should be hidden.
        /// The callbacks type should be bool.</param>
        public HideIfAttribute(params string[] conditionCallbacks) => ConditionCallbacks = conditionCallbacks;
    }
}
