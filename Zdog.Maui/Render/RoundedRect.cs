using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class RoundedRect : Shape
    {
        float width = 1, height = 1;
        float cornerRadius = 0.25f;

        protected override void SetPath()
        {
            var xA = width / 2;
            var yA = height / 2;
            var shortSide = Math.Min(xA, yA);
            var cornerRadius1 = Math.Min(cornerRadius, shortSide);
            var xB = xA - cornerRadius;
            var yB = yA - cornerRadius;

            new List<Render.PathCommand> {
                    new Move(new Vector(xB ,-yA ,0)),
                    new Arc(new []{
                        new Vector(xA, -yA ,0), new Vector(xA, -yB ,0)
                    }),
                    }.Also((list) =>
                    {
                        if (yB != 0f)
                        {
                            list.Add(new Line(new Vector(x: xA, y: yB)));
                        }
                        list.Add(new Arc(
                            new[] {
                            new Vector(x: xA, y: yA),
                            new Vector(x: xB, y: yA)
                            }));
                        if (xB != 0f)
                        {
                            list.Add(new Line(new Vector(x: -xB, y: yA)));
                        }
                        list.Add(new Arc(
                        new[] {
                            new Vector(x : -xA, y : yA),
                            new Vector(x : -xA, y : yB)
                            }));
                        if (yB != 0f)
                        {
                            list.Add(new Line(new Vector(x: -xA, y: -yB)));
                        }
                        list.Add(new Arc(
                        new[] {
                            new Vector(x : -xA, y : -yA),
                            new Vector(x : -xB, y : -yA)
                        }));
                        if (xB != 0f)
                        {
                            list.Add(new Line(new Vector(x: xB, y: -yA)));
                        }
                    });
        }

        public override RoundedRect Copy()
        {
            return Copy(new RoundedRect());
        }

        protected override RoundedRect Copy(Anchor shape)
        {
            return (base.Copy(shape) as RoundedRect).Also((it) => {
                it.width = width;
                it.height = height;
                it.cornerRadius = cornerRadius;
            });
        }
    }
}