using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdog.Maui.Render
{
    public class CylinderEllipse:Ellipse
    {
        public override T copyGraph<T>(Action<T> block)
        {
            throw new InvalidOperationException("Couldn't copy graph for CylinderEllipse");
        }
    }
}
