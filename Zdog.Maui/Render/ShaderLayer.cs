using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdog.Maui.Render
{
    public class ShaderLayer
    {
        public float Radius { get; set; }
        public float Dx { get; set; }
        public float Dy { get; set; }
        public int Color { get; set; }

        public ShaderLayer(float radius = 0f, float dx = 0f, float dy = 0f, int color = 0)
        {
            Radius = radius;
            Dx = dx;
            Dy = dy;
            Color = color;
        }

        public static ShaderLayer Empty()
        {
            return new ShaderLayer();
        }
    }
}
