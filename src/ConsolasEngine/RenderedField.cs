using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class Canvas
    {
        public char[][] Symbols
        {
            get;
            set;
        }

        public ConsoleColor[][] Colors
        {
            get;
            set;
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

        public Canvas(char[][] symbols, ConsoleColor[][] colors)
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

        public Canvas(int height, int width)
        {
            this.Height = height;
            this.Width = width;

            this.Symbols = new char[this.Height][];
            this.Colors = new ConsoleColor[this.Height][];


            for (int line = 0; line < height; line++)
            {
                this.Symbols[line] = new char[this.Width];
                this.Colors[line] = new ConsoleColor[this.Width];
            }
        }
    }
}
