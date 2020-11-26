#if UNITY_EDITOR
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class FindAssetInFoldersDrawer : BaseSearchObjectDrawer<Object, FindAssetInFoldersAttribute>
    {
        protected override Object GetObject(MightySerializedField mightyMember, FindAssetInFoldersAttribute attribute) => 
            mightyMember.Property.FindAssetInFolders(attribute.Name, attribute.Folders);
    }
}
#endif