using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class Polygon : Shape
    {
        public int sides = 3;
        public float radius = 0.5f;

        protected override void SetPath()
        {
            _path.Clear();
            for (var index = 0; index <= sides; index++)
            {
                var theta = index / sides * Utils.TAU - Utils.TAU / 4;
                var x = Math.Cos(theta) * radius;
                var y = Math.Sin(theta) * radius;
                _path.Add(new Line(new Vector((float)x, (float)y, 0)));
            }
        }

        public override Polygon Copy()
        {
            return Copy(new Polygon());
        }

        protected override Polygon Copy(Anchor shape)
        {
            return (base.Copy(shape) as Polygon).Also((it) =>
            {
                it.sides = sides;
                it.radius = radius;
            });
        }
    }
}
