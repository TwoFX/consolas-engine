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
        private string[] lastRendered;
        private int height;
        private int width;
        private bool hasChanged;

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
            Update(contents);
        }

        public void Update(string[][] contents)
        {
            unrenderedContents = contents;
            hasChanged = true;
        }

        public string[] RenderAndReturn()
        {
            if (hasChanged)
            {
                string[] renderedContents = new string[height];
                for (int i = 0; i < height; i++)
                {
                    char[] builder = new char[width];
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < unrenderedContents[i][j].Length; k++)
                        {
                            if (unrenderedContents[i][0].Length + unrenderedContents[i][1].Length >= width)
                                throw new UIException("Column length sum exceeded table width");
                            builder[j == 0 ? k : width - unrenderedContents[i][j].Length + k] = unrenderedContents[i][j].ToCharArray()[k];
                        }
                    }
                    renderedContents[i] = new string(builder);
                }
                hasChanged = false;
                lastRendered = renderedContents;
                return renderedContents;
            }
            return lastRendered;
        }
    }
}
