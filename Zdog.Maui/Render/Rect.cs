using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdog.Maui.Render
{
    public class Rect : Shape
    {
        public float width { get; set; } = 1f;
        public float height { get; set; } = 1f;

        protected override void SetPath()
        {
            float x = width / 2;
            float y = height / 2;

            Path(
                Utils.move(x: -x, y: -y),
                Utils.line(x: x, y: -y),
                Utils.line(x: x, y: y),
                Utils.line(x: -x, y: y)
            );
        }

        public override Rect Copy()
        {
            return Copy(new Rect());
        }

        protected override Rect Copy(Anchor shape)
        {
            var r = base.Copy(shape) as Rect;
            Func<Rect, Rect> also = (it) =>
            {
                it.width = width;
                it.height = height;

                return it;
            };
            return also.Invoke(r);
        }
    }
}
