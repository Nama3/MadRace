using System;
using UnityEngine;

namespace MightyAttributes.Utilities
{
    [Serializable]
    public struct Line
    {
        public float slope;
        public float intercept;

        public Line(float slope, float intercept)
        {
            this.slope = slope;
            this.intercept = intercept;
        }

        public static float GetSlope(Vector2 pointA, Vector2 pointB) => GetSlope(pointA.x, pointA.y, pointB.x, pointB.y);
        public static float GetSlope(float xA, float yA, float xB, float yB) => (yB - yA) / (xB - xA);

        public void SetSlope(Vector2 pointA, Vector2 pointB) => SetSlope(pointA.x, pointA.y, pointB.x, pointB.y);
        public void SetSlope(float xA, float yA, float xB, float yB) => slope = (yB - yA) / (xB - xA);

        public static float GetIntercept(Vector2 point, float slope) => GetIntercept(point.x, point.y, slope);
        public static float GetIntercept(float x, float y, float slope) => y - x * slope;

        public void SetIntercept(Vector2 point) => SetIntercept(point.x, point.y);
        public void SetIntercept(float x, float y) => intercept = y - x * slope;

        public static Line GetLine(Vector2 pointA, Vector2 pointB) => GetLine(pointA, GetSlope(pointA, pointB));
        public static Line GetLine(Vector2 point, float slope) => GetLine(point.x, point.y, slope);

        public static Line GetLine(float x, float y, float slope) => new Line(slope, GetIntercept(x, y, slope));

        public void SetLine(Vector2 pointA, Vector2 pointB)
        {
            SetSlope(pointA, pointB);
            SetIntercept(pointA);
        }

        public float ComputeY(float x) => ComputeY(slope, intercept, x);
        public static float ComputeY(float slope, float intercept, float x) => x * slope + intercept;
    }

    [Serializable]
    public struct Circle
    {
        public struct CircleLineIntersections
        {
            public int count;
            public Vector2 intersection1;
            public Vector2 intersection2;
        }

        public float radius;
        public Vector2 center;

        public CircleLineIntersections IntersectLine(Vector2 pointA, Vector2 pointB) => IntersectLine(Line.GetLine(pointA, pointB));

        public CircleLineIntersections IntersectLine(Line line) => IntersectLine(line.slope, line.intercept);

        public CircleLineIntersections IntersectLine(float lineSlope, float lineIntercept)
        {
            var intersections = new CircleLineIntersections();
            var h = center.x;
            var k = center.y;
            var slope2 = 2 * lineSlope;

            var a = lineSlope * lineSlope + 1;
            var b = -2 * h + slope2 * lineIntercept + slope2 * k;
            var c = h * h + lineIntercept * lineIntercept + 2 * lineIntercept * k + k * k - radius * radius;
            var delta = b * b - 4 * a * c;
            intersections.count = MathUtilities.GetXValues(delta, a, b, out var x1, out var x2);

            switch (intersections.count)
            {
                case 1:
                    intersections.intersection1.x = x1;
                    intersections.intersection1.y = Line.ComputeY(lineSlope, lineIntercept, x1);
                    break;
                case 2:
                    intersections.intersection2.x = x2;
                    intersections.intersection2.y = Line.ComputeY(lineSlope, lineIntercept, x2);
                    goto case 1;
            }

            return intersections;
        }
    }

    public static class MathUtilities
    {
        public const float DEG_2_RAD_90 = 1.5707961f;

        private const float DEFAULT_PRECISION = 0.0001f;

        public static Vector2Int GetAspectRatio(Vector2 size, float precision = DEFAULT_PRECISION, int loopCount = 10000) =>
            GetAspectRatio(size.x, size.y, precision, loopCount);

        public static Vector2Int GetAspectRatio(float width, float height, float precision = DEFAULT_PRECISION, int loopCount = 10000)
        {
            var ratio = width / height;
            var i = 0;
            while (i < loopCount)
            {
                var value = ++i * ratio;
                var roundedValue = (float) Math.Round(value, 3);

                if (Math.Abs(roundedValue - Mathf.RoundToInt(value)) < precision)
                    return new Vector2Int((int) roundedValue, i);
            }

            return new Vector2Int((int) width, (int) height);
        }

        public static int GetXValues(float delta, float a, float b, out float x1, out float x2, float precision = DEFAULT_PRECISION)
        {
            if (Math.Abs(delta) < precision)
            {
                x1 = -b / (2 * a);
                x2 = 0;
                return 1;
            }

            if (delta > 0)
            {
                var sqrtDelta = (float) Math.Sqrt(delta);
                var a2 = 2 * a;
                x1 = (-b - sqrtDelta) / a2;
                x2 = (-b + sqrtDelta) / a2;
                return 2;
            }

            x1 = x2 = 0;
            return 0;
        }

        public static float Angle(Vector2 pointA1, Vector2 pointA2, Vector2 pointB1, Vector2 pointB2) =>
            Angle(Line.GetSlope(pointA1, pointB1), Line.GetSlope(pointA2, pointB2));

        public static float Angle(Line lineA, Line lineB) => Angle(lineA.slope, lineB.slope);

        public static float Angle(float mA, float mB) => Mathf.Rad2Deg * Mathf.Atan((mB - mA) / (1 - mB * mA));

        public static float Angle(Vector2 positionA, Vector2 positionB)
        {
            var xDiff = positionB.x - positionA.x;
            var yDiff = positionB.y - positionA.y;
            return Mathf.Rad2Deg * Mathf.Asin(Abs(xDiff) / Mathf.Sqrt(xDiff * xDiff + yDiff * yDiff));
        }

        public static Vector2 PositionFromAngleAndLength(Vector2 position, float angle, float length)
        {
            var radAngle = angle * Mathf.Deg2Rad;
            return new Vector2(position.x + length * Mathf.Sin(DEG_2_RAD_90 - radAngle), position.y + length * Mathf.Sin(radAngle));
        }

        public static float AngleFromLengths(float lengthA, float lengthC) => Mathf.Rad2Deg * Mathf.Asin(lengthA / lengthC);

        public static float Abs(float value) => value >= 0 ? value : -value;

        public static float Round(float value, int decimals)
        {
            var factor = GetDecimalsFactor(decimals);
            return (int) (value * factor) / factor;
        }

        public static float GetDecimalsFactor(int decimals)
        {
            var factor = 1;
            for (var i = 0; i < decimals; i++) factor *= 10;
            return factor;
        }

        public static float Pow(float value, int power, float precision = DEFAULT_PRECISION)
        {
            if (Abs(value) < precision) return 0;
            if (power == 0) return 1;
            return Pow(value, power - 1) * value;
        }

        public static uint Pow(int value, int power)
        {
            if (value == 0) return 0;
            if (power == 0) return 1;
            return (uint) (Pow(value, power - 1) * value);
        }

        public static float Log(this float value, float newBase) => (float) Math.Log(value, newBase);

        public static Vector3 Round(Vector3 vector, int decimals)
        {
            vector.x = Round(vector.x, decimals);
            vector.y = Round(vector.y, decimals);
            vector.z = Round(vector.z, decimals);
            return vector;
        }

        public static Vector2 Round(Vector2 vector, int decimals)
        {
            vector.x = Round(vector.x, decimals);
            vector.y = Round(vector.y, decimals);
            return vector;
        }
    }
}