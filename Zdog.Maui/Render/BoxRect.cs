using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdog.Maui.Render
{
    internal class BoxRect: Rect
    {
        public override T copyGraph<T>(Action<T> block)
        {
            throw new InvalidOperationException("Couldn't copy graph for BoxRect");
        }
    }
}
