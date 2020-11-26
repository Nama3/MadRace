#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindAssetsInFoldersDrawer : BaseSearchObjectsDrawer<Object, FindAssetsInFoldersAttribute>
    {
        protected override Object[] GetFoundArray(MightySerializedField mightyMember, FindAssetsInFoldersAttribute attribute) => 
            mightyMember.Property.FindAssetsInFolders(attribute.Name, attribute.Folders);
    }
}
#endif