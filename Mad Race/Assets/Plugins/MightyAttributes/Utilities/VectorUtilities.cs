using System;
using UnityEngine;

namespace MightyAttributes.Utilities
{
    public static class VectorUtilities
    {
        public static int[] Vector2IntToArray(Vector2Int source) => new[] {source.x, source.y};
        public static float[] Vector2IntToFloatArray(Vector2Int source) => new[] {(float) source.x, source.y};

        public static Vector2Int ArrayToVector2Int(int[] source) => new Vector2Int(source[0], source[1]);
        public static Vector2Int FloatArrayToVector2Int(float[] source) => new Vector2Int((int) source[0], (int) source[1]);

        public static float[] Vector2ToArray(Vector2 source) => new[] {source.x, source.y};

        public static Vector2 ArrayToVector2(float[] source) => new Vector2(source[0], source[1]);

        public static float[] Vector3ToArray(Vector3 source) => new[] {source.x, source.y, source.z};

        public static Vector3 ArrayToVector3(float[] source) => new Vector3(source[0], source[1], source[2]);

        public static float[] Vector4ToArray(Vector4 source) => new[] {source.x, source.y, source.z, source.w};

        public static Vector4 ArrayToVector4(float[] source) => new Vector4(source[0], source[1], source[2], source[3]);

        public static Vector4 Divide(Vector4 a, Vector4 b) => new Vector4(a.x / b.x, a.y / b.y, a.z / b.z, a.w / b.w);

        public static float Magnitude(Vector3 vector) => Magnitude(vector.x, vector.y, vector.z);

        public static float Magnitude(float x, float y, float z) => (float) Math.Sqrt(SqrMagnitude(x, y, z));

        public static float SqrMagnitude(Vector3 vector) => SqrMagnitude(vector.x, vector.y, vector.z);

        public static float SqrMagnitude(float x, float y, float z) => x * x + y * y + z * z;

        public static float Magnitude(this Vector2 vector) => Magnitude(vector.x, vector.y);

        public static float Magnitude(float x, float y) => (float) Math.Sqrt(SqrMagnitude(x, y));

        public static float SqrMagnitude(Vector2 vector) => SqrMagnitude(vector.x, vector.y);

        public static float SqrMagnitude(float x, float y) => x * x + y * y;

        public static float Distance(Vector3 pointA, Vector3 pointB) =>
            Magnitude(pointA.x - pointB.x, pointA.y - pointB.y, pointA.z - pointB.z);

        public static float Distance(Vector2 pointA, Vector2 pointB) => Magnitude(pointA.x - pointB.x, pointA.y - pointB.y);

        public static float SqrDistance(Vector3 pointA, Vector3 pointB) =>
            SqrMagnitude(pointA.x - pointB.x, pointA.y - pointB.y, pointA.z - pointB.z);

        public static float SqrDistance(Vector2 pointA, Vector2 pointB) => SqrMagnitude(pointA.x - pointB.x, pointA.y - pointB.y);

        public static Vector2 Round(Vector2 value, int decimals)
        {
            var factor = MathUtilities.GetDecimalsFactor(decimals);
            return new Vector2((int) (value.x * factor) / factor, (int) (value.y * factor) / factor);
        }

        public static Vector3 Round(Vector3 value, int decimals)
        {
            var factor = MathUtilities.GetDecimalsFactor(decimals);
            return new Vector3((int) (value.x * factor) / factor, (int) (value.y * factor) / factor, (int) (value.z * factor) / factor);
        }
    }
}