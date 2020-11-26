using System;

namespace MightyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class OrderAttribute : BaseSpecialAttribute
    {
        public short Order { get; }
        public string OrderCallback { get; }
        
        /// <summary>
        /// Allows you to overwrite the draw order of any member.
        /// All the attributes of the member will be executed at the specified order.
        /// </summary>
        /// <param name="order">The draw order of the member.</param>
        public OrderAttribute(short order) => Order = order;

        /// <summary>
        /// Allows you to overwrite the draw order of any member.
        /// All the attributes of the member will be executed at the specified order.
        /// </summary>
        /// <param name="orderCallback">Callback for the order of the member.
        /// The callback type can be short or int.</param>
        public OrderAttribute(string orderCallback) => OrderCallback = orderCallback;
    }
}