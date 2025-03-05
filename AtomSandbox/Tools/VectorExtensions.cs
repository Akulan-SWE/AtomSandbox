using System.Numerics;

namespace AtomSandbox.Tools
{
    static class VectorExtensions
    {
        private const float DegToRad = (float)(Math.PI / 180);
        private const float RadToDeg = (float)(180 / Math.PI);

        public static void Set(this Vector2 v, float x, float y)
        {
            v.X = x;
            v.Y = y;
        }

        public static Vector2 ToUnit(this Vector2 v)
        {
            return Vector2.Normalize(v);
        }

        public static Vector2 ToSqUnit(this Vector2 v)
        {
            return v / v.LengthSquared();
        }

        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            return v.RotateRadians(degrees * DegToRad);
        }

        public static Vector2 RotateRadians(this Vector2 v, float radians)
        {
            var ca = (float)Math.Cos(radians);
            var sa = (float)Math.Sin(radians);
            return new Vector2(ca * v.X - sa * v.Y, sa * v.X + ca * v.Y);
        }

        public static Vector2 Reflection(this Vector2 v, Vector2 p1, Vector2 p2, float k)
        {
            var unit = (p1 - p2).ToUnit();
            return v - (1 + k) * Vector2.Dot(v, unit) * unit;
        }
    }
}
