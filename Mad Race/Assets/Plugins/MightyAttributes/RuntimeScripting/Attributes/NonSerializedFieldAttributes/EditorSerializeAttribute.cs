using System;

namespace MightyAttributes
{
    public class EditorSerializeAttribute : BaseNonSerializedFieldAttribute
    {
        public string PreviousName { get; }

        public EditorFieldOption Options { get; }

        public bool ExecuteInPlayMode { get; }

        /// <summary>
        /// Serializes the value of the field within the editor only.
        /// The value isn’t serialized in builds.
        /// </summary>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: false).</param>
        [Obsolete("Not really obsolete but not good at the moment, see the doc for more info.", false)]
        public EditorSerializeAttribute(bool executeInPlayMode = false)
        {
            Options = EditorFieldOption.Default;
            ExecuteInPlayMode = executeInPlayMode;
        }

        /// <summary>
        /// Serializes the value of the field within the editor only.
        /// The value isn’t serialized in builds.
        /// </summary>
        /// <param name="previousName">If you happen to change the name of your field, you can specify what was the previous one (before Unity reloads the script), so you don’t loose the serialized value.</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: false).</param>
        [Obsolete("Not really obsolete but not good at the moment, see the doc for more info.", false)]
        public EditorSerializeAttribute(string previousName, bool executeInPlayMode = false)
        {
            Options = EditorFieldOption.Default;
            PreviousName = previousName;
            ExecuteInPlayMode = executeInPlayMode;
        }

        /// <summary>
        /// Serializes the value of the field within the editor only.
        /// The value isn’t serialized in builds.
        /// </summary>
        /// <param name="options">Special options for the field. See the doc for more info.</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: false).</param>
        [Obsolete("Not really obsolete but not good at the moment, see the doc for more info.", false)]
        public EditorSerializeAttribute(EditorFieldOption options, bool executeInPlayMode = false)
        {
            Options = options;
            ExecuteInPlayMode = executeInPlayMode;
        }

        /// <summary>
        /// Serializes the value of the field within the editor only.
        /// The value isn’t serialized in builds.
        /// </summary>
        /// <param name="options">Special options for the field. See the doc for more info.</param>
        /// <param name="previousName">If you happen to change the name of your field, you can specify what was the previous one (before Unity reloads the script), so you don’t loose the serialized value.</param>
        /// <param name="executeInPlayMode">Choose whether or not the attribute should run during Play Mode (default: false).</param>
        [Obsolete("Not really obsolete but not good at the moment, see the doc for more info.", false)]
        public EditorSerializeAttribute(EditorFieldOption options, string previousName, bool executeInPlayMode = false)
        {
            Options = options;
            PreviousName = previousName;
            ExecuteInPlayMode = executeInPlayMode;
        }
    }
}