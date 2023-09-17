using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zdog.Maui.Extensions
{
    internal static class KotlinExtension
    {
        public static T Also<T>(this T ob, Action<T> action)
        {
            action?.Invoke(ob);
            return ob;
        }
        
        public static T apply<T>(this T ob, Action<T> action)
        {
            return Also(ob, action);
        }
        
        /// <summary>
        /// kotlin中let返回块中最后一行的值,此处设计为主动返回值。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T1"></typeparam>
        /// <param name="ob"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T1 let<T, T1>(this T ob, Func<T, T1> action)
        {
            return action.Invoke(ob);
        }
    }
}
