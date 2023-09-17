using Microsoft.Maui.Controls.Shapes;

namespace Zdog.Maui.Render
{
    public static class Utils
    {
        public static PathCommand move(float x = 0f, float y = 0f, float z = 0f)
        {
            return new Move(vector(x, y, z));
        }

        public static PathCommand move(Vector vector)
        {
            return new Move(vector);
        }

        public static PathCommand line(float x = 0f, float y = 0f, float z = 0f)
        {
            return new Line(vector(x, y, z));
        }

        public static PathCommand line(Vector vector)
        {
            return new Line(vector);
        }

        public static PathCommand bezier(Vector cp0, Vector cp1, Vector cp2)
        {
            return new Bezier(new[] { cp0, cp1, cp2 });
        }

        public static PathCommand arc(Vector cp0, Vector cp1)
        {
            return new Arc(new[] { cp0, cp1 });
        }

        // Shape
        public static Vector vector(float f)
        {
            return new Vector(f, f, f);
        }

        public static Vector vector(float x = 0f, float y = 0f, float z = 0f)
        {
            return new Vector(x, y, z);
        }

        public static Vector vector(Vector vector)
        {
            return new Vector(vector);
        }

        public static Anchor anchor(Action<Anchor> block)
        {
            return new Anchor().set(block);
        }

        public static Group group(Action<Group> block)
        {
            return new Group().set(block);
        }

        public static Illustration illustration(Action<Illustration> block)
        {
            return new Illustration().set(block);
        }

        public static Shape shape(Action<Shape> block)
        {
            return new Shape().set(block);
        }

        public static Rect rect(Action<Rect> block)
        {
            return new Rect().set(block);
        }

        public static RoundedRect roundedRect(Action<RoundedRect> block)
        {
            return new RoundedRect().set(block);
        }

        public static Ellipse ellipse(Action<Ellipse> block)
        {
            return new Ellipse().set(block);
        }

        public static Polygon polygon(Action<Polygon> block)
        {
            return new Polygon().set(block);
        }

        public static Hemisphere hemisphere(Action<Hemisphere> block)
        {
            return new Hemisphere().set(block);
        }

        public static Cone cone(Action<Cone> block)
        {
            return new Cone().set(block);
        }

        public static Cylinder cylinder(Action<Cylinder> block)
        {
            return new Cylinder().set(block);
        }

        public static Box box(Action<Box> block)
        {
            return new Box().set(block);
        }

        public static Combine combine(Action<Combine> block)
        {
            return new Combine().set(block);
        }

        public static Text text(Action<Text> block)
        {
            return new Text().set(block);
        }

        public static T copy<T>(this T anchor, Action<T> block) where T : Anchor
        {
            return (anchor.Copy() as T).set(block);
        }

        public static T copyEx<T>(this T anchor, Action<T> block) where T : Anchor
        {
            anchor.copyGraph(block);
            return anchor;
        }

        public static T set<T>(this T propety, Action<T> block = null) where T : IProperty
        {
            block?.Invoke(propety);
            propety.onCreate();
            return propety;
        }

        public static T setup<T>(this T propety, Action<T> block = null) where T : IProperty
        {
            block?.Invoke(propety);
            propety.onCreate();
            return propety;
        }

        public static T attachTo<T>(this T any, List<T> collection)
        {
            collection.Add(any);
            return any;
        }


        public static float TAU = (float)(Math.PI * 2);

        /*Zdog.extend = function(a, b )
        {
  for (var prop in b)
            {
                a[prop] = b[prop];
            }
            return a;
        };*/

        public static float lerp(float a, float b, float alpha)
        {
            return (b - a) * alpha + a;
        }

        public static float modulo(float num, float div)
        {
            return ((num % div) + div) % div;
        }

        static Dictionary<int, Func<float, float>> powerMultipliers = new Dictionary<int, Func<float, float>>()
        {
            { 2, (a) =>  a * a },
            { 3, (a)=>a * a * a},
            { 4, (a)=> a * a * a * a},
            { 5, (a)=>a * a * a * a * a},
        };

        public static float easeInOut(float alpha, int power = 2)
        {
            if (power == 1) return alpha;
            alpha = Math.Max(0f, Math.Min(1f, alpha));
            bool isFirstHalf = alpha < 0.5f;
            float slope = isFirstHalf ? alpha : 1 - alpha;
            slope /= 0.5f;
            Func<float, float> powerMultiplier = power < 2 || power > 5 ?
                powerMultipliers[2] :
                powerMultipliers[power];
            float curve = powerMultiplier(slope);
            curve /= 2f;
            return isFirstHalf ? curve : 1 - curve;
        }

        internal const float arcHandleLength = 9 / 16f;

        internal static float magnitudeSqrt(float sum)
        {
            if (Math.Abs(sum - 1) < 0.00000001)
            {
                return 1f;
            }
            return (float)Math.Sqrt(sum);
        }
    }
}
