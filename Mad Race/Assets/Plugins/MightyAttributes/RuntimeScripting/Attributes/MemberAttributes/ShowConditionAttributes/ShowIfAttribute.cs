namespace MightyAttributes
{
    public class ShowIfAttribute : BaseShowConditionAttribute
    {
        public string[] ConditionCallbacks { get; }
        
        /// <summary>
        /// Let you define the conditions on which the member should be shown.
        /// The member will be shown if all the conditions are met.
        /// </summary>
        /// <param name="conditionCallbacks">Array of callbacks for all the conditions on which the member should be shown.
        /// The callbacks type should be bool.</param>
        public ShowIfAttribute(params string[] conditionCallbacks) => ConditionCallbacks = conditionCallbacks;
    }
}
