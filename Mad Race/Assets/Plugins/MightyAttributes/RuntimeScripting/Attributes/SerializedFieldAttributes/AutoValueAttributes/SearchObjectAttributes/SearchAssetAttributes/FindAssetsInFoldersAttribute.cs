namespace MightyAttributes
{
    public class FindAssetsInFoldersAttribute : BaseSearchAssetAttribute
    {
        public string[] Folders { get; }

        /// <summary>
        /// Finds all the objects in the asset database that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="folders">The folders to look into.</param>
        public FindAssetsInFoldersAttribute(params string[] folders) : base(false) => Folders = folders;

        /// <summary>
        /// Finds all the objects in the asset database that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="folders">The folders to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetsInFoldersAttribute(string[] folders, bool executeInPlayMode = false) : base(executeInPlayMode) =>
            Folders = folders;

        /// <summary>
        /// Finds all the objects in the asset database that have the same element type than the field.
        /// The field needs to be an array.
        /// </summary>
        /// <param name="name">The name of the assets to look for.</param>
        /// <param name="folders">The folders to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetsInFoldersAttribute(string name, string[] folders, bool executeInPlayMode = false) : base(name, executeInPlayMode)
            => Folders = folders;
    }
}