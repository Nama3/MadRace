#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace MightyAttributes.Utilities
{
    public class GizmosUtilities : MonoBehaviour
    {
        public static void DrawCross(Vector2 position, Color color = default, float width = 2f)
        {
            Gizmos.color = color;
            var width2 = width / 2;
            Gizmos.DrawLine(new Vector2(position.x - width2, position.y + width2), new Vector2(position.x + width2, position.y - width2));
            Gizmos.DrawLine(new Vector2(position.x - width2, position.y - width2), new Vector2(position.x + width2, position.y + width2));
        }

        public static void DrawWireCapsule(Vector3 position, Quaternion rotation, float radius, float height, Color color = default)
        {
            if (color != default) Handles.color = color;

            var angleMatrix = Matrix4x4.TRS(position, rotation, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (height - radius * 2) / 2;

                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
                Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);

                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);

                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
            }
        }

        public static void DrawTopDownRectangle(Vector3 position, Quaternion rotation, Vector2 size, Color faceColor, Color outlineColor)
        {
            var verts = new[]
            {
                rotation * new Vector3(position.x - size.x, position.y, position.z - size.y),
                rotation * new Vector3(position.x - size.x, position.y, position.z + size.y),
                rotation * new Vector3(position.x + size.x, position.y, position.z + size.y),
                rotation * new Vector3(position.x + size.x, position.y, position.z - size.y)
            };

            Handles.DrawSolidRectangleWithOutline(verts, faceColor, outlineColor);
        }
    }
}
#endif