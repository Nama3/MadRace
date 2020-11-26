namespace MightyAttributes
{
    public class FindAssetInFoldersAttribute : BaseSearchAssetAttribute
    {
        public string[] Folders { get; }

        /// <summary>
        /// Finds the first object in the asset database that have the same type than the field.
        /// </summary>
        /// <param name="folders">The folders to look into.</param>
        public FindAssetInFoldersAttribute(params string[] folders) : base(false) => Folders = folders;

        /// <summary>
        /// Finds the first object in the asset database that have the same type than the field.
        /// </summary>
        /// <param name="folders">The folders to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetInFoldersAttribute(string[] folders, bool executeInPlayMode = false) : base(executeInPlayMode) => Folders = folders;

        /// <summary>
        /// Finds the first object in the asset database that have the same type than the field.
        /// </summary>
        /// <param name="name">The name of the asset to look for.</param>
        /// <param name="folders">The folders to look into.</param>
        /// <param name="executeInPlayMode">Choose whether or not this attribute should run during Play Mode (default: false).</param>
        public FindAssetInFoldersAttribute(string name, string[] folders, bool executeInPlayMode = false) : base(name, executeInPlayMode) =>
            Folders = folders;
    }
}