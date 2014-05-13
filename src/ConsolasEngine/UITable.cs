using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class UITable : IRenderable
    {
        private string[][] unrenderedContents;
        private Canvas lastRendered;
        private int height;
        private int width;
        private bool hasChanged;
        private ConsoleColor? textColor;
        private ConsoleColor? headingColor;

        public bool HasChanged
        {
            get { return hasChanged; }
        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public UITable(string[][] contents, int lengthSum)
        {
            height = contents.Length;
            width = lengthSum;
            lastRendered = new Canvas(height, width);
            Update(contents);
        }

        public void Update(string[][] contents)
        {
            unrenderedContents = contents;
            hasChanged = true;
        }

        public Canvas Render()
        {
            if (hasChanged)
            {
                char[][] renderedContents = new char[height][];
                for (int i = 0; i < height; i++)
                {
                    for (int sp = 0; sp < width; sp++)
                    {
                        renderedContents[i] = new char[width];
                        renderedContents[i][sp] = ' ';
                        lastRendered.Colors[i][sp] = i == 0 ?
                            headingColor ?? ConsoleColor.Blue :
                            textColor ?? ConsoleColor.Gray;
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < unrenderedContents[i][j].Length; k++)
                        {
                            if (unrenderedContents[i][0].Length + unrenderedContents[i][1].Length >= width)
                                throw new UIException("Column length sum exceeded table width");
                            renderedContents[i][j == 0 ? k : width - unrenderedContents[i][j].Length + k] = unrenderedContents[i][j].ToCharArray()[k];
                        }
                    }
                }
                hasChanged = false;
                lastRendered.Symbols = renderedContents;
            }
            return lastRendered;
        }
    }
}
