namespace Zdog.Maui.Extensions
{
    internal static class EnumerableExtensions
    {
        /// <summary>
        /// https://stackoverflow.com/questions/521687/foreach-with-index
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        public static void Each<T>(this IEnumerable<T> ie, Action<int, T> action)
        {
            var i = 0;
            foreach (var e in ie) action(i++, e);
        }
        
        /// <summary>
        /// 返回一个新的数组。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ie"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T1[] map<T,T1>(this T[] ie, Func<T, T1> action)
        {
            var result = new T1[ie.Length];
            for (int i = 0; i < ie.Length; i++)
            {
                result[i] = action.Invoke(ie[i]);
            }
            return result;
        }
        
        public static List<T1> map<T, T1>(this List<T> ie, Func<T, T1> action)
        {
            var result = new List<T1>();
            for (int i = 0; i < ie.Count; i++)
            {
                result.Add(action.Invoke(ie[i]));
            }
            return result;
        }


        public static List<T> listOf<T>(params T[] any)
        {
            var result = new List<T>();
            foreach(var item in any) result.Add(item);
            return result;
        }
    }
}
