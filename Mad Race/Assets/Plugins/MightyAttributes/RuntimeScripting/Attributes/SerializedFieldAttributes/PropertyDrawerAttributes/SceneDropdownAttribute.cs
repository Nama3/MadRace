namespace MightyAttributes
{
    public class SceneDropdownAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Displays a dropdown that lets you select a scene.
        /// Stores the build index of the scene in the field.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public SceneDropdownAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}