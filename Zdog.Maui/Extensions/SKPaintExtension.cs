using Microsoft.Maui.Graphics;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Zdog.Maui.Extensions
{
    internal static class SKPaintExtension
    {
        public static void SetShadowLayer(this SKPaint paint, float radius, float dx, float dy, SKColor color)
        {
            paint.ImageFilter = SKImageFilter.CreateDropShadow(
                                   dx,
                                   dy,
                                   radius,
                                   radius,
                                   color);
        }

        public static void ClearShadowLayer(this SKPaint paint)
        {
            paint.ImageFilter = null;
        }
    }
}
