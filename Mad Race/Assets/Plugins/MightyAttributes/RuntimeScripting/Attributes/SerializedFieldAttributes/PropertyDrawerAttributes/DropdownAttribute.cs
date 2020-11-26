using System.Collections;
using System.Collections.Generic;

namespace MightyAttributes
{
    public class DropdownAttribute : BasePropertyDrawerAttribute
    {
        public string ValuesFieldName { get; }

        /// <summary>
        /// Displays a dropdown to select a specific value for the field.
        /// </summary>
        /// <param name="valuesCallback">The callback for the list of values.</param>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public DropdownAttribute(string valuesCallback, FieldOption options = FieldOption.Nothing) : base(options) =>
            ValuesFieldName = valuesCallback;
    }

    public interface IDropdownValues : IEnumerable<KeyValuePair<string, object>>
    {
    }

    public class DropdownValues<T> : IDropdownValues
    {
        private readonly List<KeyValuePair<string, object>> m_values = new List<KeyValuePair<string, object>>();

        public void Add(string name, T value) => m_values.Add(new KeyValuePair<string, object>(name, value));

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => m_values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}