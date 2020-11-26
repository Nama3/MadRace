#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace MightyAttributes.Editor
{
    public class ShowAssetPreviewDrawer : BaseArrayDecoratorDrawer<ShowAssetPreviewAttribute>, IArrayDecoratorDrawer
    {
        #region Overrides

        #region Not Implemented

        void IArrayDecoratorDrawer.BeginDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        void IArrayDecoratorDrawer.EndDrawArray(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        void IArrayDecoratorDrawer.BeginDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        void IArrayDecoratorDrawer.EndDrawHeader(MightySerializedField serializedField, BaseArrayDecoratorAttribute baseAttribute)
        {
        }

        #endregion /Not Implemented

        void IArrayDecoratorDrawer.BeginDrawElement(MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute)
        {
            var decoratorPosition = PositionByMember(serializedField, baseAttribute);
            if (decoratorPosition.Contains(ArrayDecoratorPosition.Before | ArrayDecoratorPosition.BeforeElements))
                DrawDecoratorElement(serializedField, index, (ShowAssetPreviewAttribute) baseAttribute);
        }

        void IArrayDecoratorDrawer.EndDrawElement(MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute)
        {
            var decoratorPosition = PositionByMember(serializedField, baseAttribute);
            if (decoratorPosition.Contains(ArrayDecoratorPosition.After | ArrayDecoratorPosition.AfterElements))
                DrawDecoratorElement(serializedField, index, (ShowAssetPreviewAttribute) baseAttribute);
        }

        Rect IArrayDecoratorDrawer.BeginDrawElement(Rect position, MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute)
        {
            var decoratorPosition = PositionByMember(serializedField, baseAttribute);
            if (decoratorPosition.Contains(ArrayDecoratorPosition.Before | ArrayDecoratorPosition.BeforeElements))
                position = DrawDecoratorElement(position, serializedField, index, (ShowAssetPreviewAttribute) baseAttribute);

            return position;
        }

        Rect IArrayDecoratorDrawer.EndDrawElement(Rect position, MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute)
        {
            var decoratorPosition = PositionByMember(serializedField, baseAttribute);
            if (decoratorPosition.Contains(ArrayDecoratorPosition.After | ArrayDecoratorPosition.AfterElements))
                position = DrawDecoratorElement(position, serializedField, index, (ShowAssetPreviewAttribute) baseAttribute);

            return position;
        }

        float IArrayDecoratorDrawer.GetElementHeight(MightySerializedField serializedField, int index,
            BaseArrayDecoratorAttribute baseAttribute)
        {
            var position = PositionByMember(serializedField, baseAttribute);

            var decoratorHeight = GetDecoratorHeight(serializedField, index, (ShowAssetPreviewAttribute) baseAttribute);
            var elementHeight = 0f;

            if (position.Contains(ArrayDecoratorPosition.Before | ArrayDecoratorPosition.BeforeElements))
                elementHeight += decoratorHeight;
            if (position.Contains(ArrayDecoratorPosition.After | ArrayDecoratorPosition.AfterElements))
                elementHeight += decoratorHeight;

            return elementHeight;
        }

        protected override void DrawDecorator(BaseMightyMember mightyMember, ShowAssetPreviewAttribute attribute) =>
            DrawPreview(((MightySerializedField) mightyMember).Property, attribute);

        protected override Rect DrawDecoratorElement(Rect position, MightySerializedField serializedField, int index,
            ShowAssetPreviewAttribute attribute) =>
            DrawPreview(position, serializedField.GetElement(index), attribute);

        protected override void DrawDecoratorElement(MightySerializedField serializedField, int index, ShowAssetPreviewAttribute attribute)
            => DrawPreview(serializedField.GetElement(index), attribute);

        protected override float GetDecoratorHeight(MightySerializedField serializedField, int index, ShowAssetPreviewAttribute attribute)
        {
            var element = serializedField.GetElement(index);

            Texture2D preview;
            if (element.propertyType != SerializedPropertyType.ObjectReference || element.objectReferenceValue == null ||
                (preview = AssetPreview.GetAssetPreview(element.objectReferenceValue)) == null)
                return 40;
            return GetClampedWidthAndHeight(preview.width, preview.height, attribute.Size).Item2 +
                   MightyGUIUtilities.FIELD_SPACING;
        }

        #endregion /Overrides

        private static void DrawPreview(SerializedProperty property, ShowAssetPreviewAttribute attribute)
        {
            Texture2D preview;
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null &&
                (preview = AssetPreview.GetAssetPreview(property.objectReferenceValue)) != null)
            {
                var (width, height) = GetClampedWidthAndHeight(preview.width, preview.height, attribute.Size);

                var align = attribute.Align;

                GUILayout.BeginVertical(GUILayout.Height(height));
                MightyGUIUtilities.BeginDrawAlign(align);

                GUILayout.Label(preview, GUILayout.Width(width), GUILayout.Height(height));

                MightyGUIUtilities.EndDrawAlign(align);
                GUILayout.EndVertical();
            }
            else
                MightyGUIUtilities.DrawHelpBox($"{property.name} doesn't have an asset preview");
        }

        private static Rect DrawPreview(Rect position, SerializedProperty property, ShowAssetPreviewAttribute attribute)
        {
            Texture2D preview;
            if (property.propertyType == SerializedPropertyType.ObjectReference && property.objectReferenceValue != null &&
                (preview = AssetPreview.GetAssetPreview(property.objectReferenceValue)) != null)
            {
                var (width, height) = GetClampedWidthAndHeight(preview.width, preview.height, attribute.Size);

                GUI.Label(MightyGUIUtilities.GetAlignPosition(position, width, height, attribute.Align), preview);
                position = MightyGUIUtilities.JumpHeight(position, height + MightyGUIUtilities.FIELD_SPACING);
            }
            else
            {
                MightyGUIUtilities.DrawHelpBox(new Rect(position.x, position.y, position.width, 40),
                    $"{property.name} doesn't have an asset preview");
                position = MightyGUIUtilities.JumpHeight(position, 40);
            }

            return position;
        }

        private static (int, int) GetClampedWidthAndHeight(int width, int height, int preferredSize) =>
            (Mathf.Clamp(preferredSize, 0, width), Mathf.Clamp((int) (preferredSize / ((float) width / height)), 0, height));
    }
}
#endif