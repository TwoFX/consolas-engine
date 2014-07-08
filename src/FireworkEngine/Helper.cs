using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    static class Helper
    {
        /// <summary>
        /// Inserts an entire two-dimensional array into another twoidimensional array
        /// </summary>
        /// <param name="dest">The destination array</param>
        /// <param name="source">The source array</param>
        /// <param name="fromX">The first index of the position of the first element of the source array within the destination array</param>
        /// <param name="fromY">The second index of the position of the first element of the source array within the destination array</param>
        public static void Copy2D<T>(T[][] dest, T[][] source, int fromX, int fromY)
        {
            for (int line = 0; line < source.Length; line++)
            {
                Array.Copy(source[line], 0, dest[line + fromX], fromY, source[line].Length);
            }
        }

        /// <summary>
        /// Inserts a one-dimensional array into a two-dimensional array
        /// </summary>
        /// <param name="dest">The destination array</param>
        /// <param name="source">The source array</param>
        /// <param name="fromX">The first index of the position of the first element of the source array within the destination array</param>
        /// <param name="fromY">The second index of the position of the first element of the source array within the destination array</param>
        public static void Copy2D<T>(T[][] dest, T[] source, int fromX, int fromY)
        {
            Copy2D(dest, Wrap(source), fromX, fromY);
        }

        /// <summary>
        /// Inserts a singe value into a two-dimensional array
        /// </summary>
        /// <param name="dest">The destination array</param>
        /// <param name="source">The source array</param>
        /// <param name="fromX">The first index of the position of the first element of the source array within the destination array</param>
        /// <param name="fromY">The second index of the position of the first element of the source array within the destination array</param>
        public static void Copy2D<T>(T[][] dest, T source, int fromX, int fromY)
        {
            Copy2D(dest, Wrap(source), fromX, fromY);
        }

        /// <summary>
        /// Returns an array with a single value repeated multiple times
        /// </summary>
        public static T[] Repeat<T>(T element, int count)
        {
            T[] r = new T[count];
            for (int i = 0; i < count; i++)
            {
                r[i] = element;
            }
            return r;
        }

        /// <summary>
        /// Elevates a value or array into an array with a dimension that is one higher than the input
        /// </summary>
        public static T[] Wrap<T>(T element)
        {
            return new T[] { element };
        }
    }
}
