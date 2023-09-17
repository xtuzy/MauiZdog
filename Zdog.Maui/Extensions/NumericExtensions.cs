using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Zdog.Maui.Extensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class NumericExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float self, float min, float max)
        {
            if (max < min)
            {
                return max;
            }
            else if (self < min)
            {
                return min;
            }
            else if (self > max)
            {
                return max;
            }

            return self;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Clamp(this double self, double min, double max)
        {
            if (max < min)
            {
                return max;
            }
            else if (self < min)
            {
                return min;
            }
            else if (self > max)
            {
                return max;
            }

            return self;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Bound(this int self, int min, int max)
        {
            if (max < min)
            {
                return max;
            }
            else if (self < min)
            {
                return min;
            }
            else if (self > max)
            {
                return max;
            }

            return self;
        }

        public static float Angle(this float value)
        {
            return (float)((value / Math.PI) * 180);
        }

        public static float Sin(this float value)
        {
           return (float)Math.Sin(value); 
        }

        public static float Cos(this float value)
        {
             return (float)Math.Cos(value); 
        }

        public static float Tan(this float value)
        {
             return (float)Math.Tan(value); 
        }
    }
}
