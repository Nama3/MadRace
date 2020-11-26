namespace MightyAttributes
{
    public class SceneSliderAttribute : BasePropertyDrawerAttribute
    {
        /// <summary>
        /// Displays a slider that lets you select a scene.
        /// Stores the build index of the scene in the field.
        /// </summary>
        /// <param name="options">Some drawing options for the field (default: Nothing).</param>
        public SceneSliderAttribute(FieldOption options = FieldOption.Nothing) : base(options)
        {
        }
    }
}