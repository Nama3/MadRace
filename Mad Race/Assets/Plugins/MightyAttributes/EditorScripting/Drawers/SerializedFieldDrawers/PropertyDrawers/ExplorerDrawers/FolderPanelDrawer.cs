#if UNITY_EDITOR
using UnityEditor;

namespace MightyAttributes.Editor
{
    public class FolderPanelDrawer : BaseExplorerDrawer<FolderPanelAttribute>
    {
        protected override string DisplayPanel(string panelTitle, string path, BaseMightyMember mightyMember,
            FolderPanelAttribute attribute) => EditorUtility.OpenFolderPanel(panelTitle, path, "");
    }
}
#endif