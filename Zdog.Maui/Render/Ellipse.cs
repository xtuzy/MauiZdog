using Zdog.Maui.Extensions;

namespace Zdog.Maui.Render
{
    public class Ellipse : Shape
    {
        public float diameter = 1;
        public float width = 0;
        public float height = 0;
        public int quarters = 4;

        public Ellipse()
        {
            closed = true;
            fill = true;
        }

        protected override void SetPath()
        {
            var x = (width != 0f) ? width / 2 : diameter / 2;
            var y = (height != 0f) ? height / 2 : diameter / 2;
            Path(
                new Move(new Vector(x: 0f, y: -y)),
                new Arc(new[] {
                    new Vector(x : x, y : -y),
                    new Vector(x : x, y : 0f)
                    }
                )
            ).apply((it) =>
            {
                if (quarters > 1)
                {
                    it.Add(new Arc(
                        new[] {

                    new Vector(x: x, y: y),
                    new Vector(x: 0f, y: y)
                            }
                        ));
                }
                if (quarters > 2)
                {
                    it.Add(new Arc(new[] {
                        new Vector(x : -x, y : y),
                        new Vector(x : -x, y : 0f)
                        }
                    ));
                }
                if (quarters > 3)
                {
                    it.Add(new Arc(
                        new[] {
                        new Vector(x : -x, y : -y),
                        new Vector(x : 0f, y : -y)
                       }
                    ));
                }
            });
        }

        public override Ellipse Copy()
        {
            return Copy(new Ellipse());
        }

        protected override Ellipse Copy(Anchor shape)
        {
            return (base.Copy(shape) as Ellipse).Also((it) =>
            {
                it.diameter = diameter;
                it.width = width;
                it.height = height;
                it.quarters = quarters;
            });
        }
    }
}
