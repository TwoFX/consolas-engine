using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class RenderedField
    {
        public char[][] Symbols
        {
            get;
            private set;
        }

        public ConsoleColor[][] Colors
        {
            get;
            private set;
        }

        public int Height
        {
            get;
            private set;
        }

        public int Width
        {
            get;
            private set;
        }

        public RenderedField(char[][] symbols, ConsoleColor[][] colors)
        {
            this.Height = symbols.Length;
            this.Width = symbols[0].Length;
            this.Symbols = symbols;
            this.Colors = colors;

            if (colors.Length != this.Height ||
                symbols.Any(row => row.Length != this.Width) ||
                colors.Any(row => row.Length != this.Width))
            {
                throw new ArgumentOutOfRangeException("Input dimensions do not match.");
            }
        }
    }
}
