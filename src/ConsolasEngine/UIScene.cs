using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class UIScene : IRenderable
    {
        private IRenderable[] unrenderedContents;
        private int[][] positions;
        private string[] captions;
        private int height;
        private int width;

        public bool HasChanged
        {
            get
            {
                bool ret = false;
                foreach (IRenderable item in unrenderedContents)
                    if (item.HasChanged == true)
                        ret = true;
                return ret;
            }
        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public IRenderable GetContents(int index)
        {
            return unrenderedContents[index];
        }

        public UIScene(IRenderable[] contents, int[][] positions, string[] captions, int height, int width)
        {
            unrenderedContents = contents;
            this.positions = positions;
            this.captions = captions;
            this.height = height;
            this.width = width;
        }

        public string[] RenderAndReturn()
        {
            string[] ret = new string[height];
            char[][] builder = new char[height][];
            List<int[]> elementPoints = new List<int[]>();

            for (int i = 0; i < builder.Length; i++)
            {
                builder[i] = new char[width];
            }

            for (int e = 0; e < unrenderedContents.Length; e++)
            {
                string[] renderedElement = unrenderedContents[e].RenderAndReturn();
                for (int i = 0; i < unrenderedContents[e].Height; i++)
                {
                    for (int j = 0; j < unrenderedContents[e].Width; j++)
                    {
                        int newX = positions[e][0] + i;
                        int newY = positions[e][1] + j;
                        builder[newX][newY] = renderedElement[i].ToCharArray()[j];
                        elementPoints.Add(new int[] { newX, newY });
                    }
                }
                int capX = positions[e][0] - 1;
                for (int c = 0; c < captions[e].Length; c++)
                {
                    int capY = positions[e][1] + c + 1;
                    builder[capX][capY] = captions[e].ToCharArray()[c];
                    elementPoints.Add(new int[] { capX, capY });
                }
            }

            SequenceEqualityComparer<int> secint = new SequenceEqualityComparer<int>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (!(elementPoints.Contains<int[]>(new int[] { i, j }, secint)))
                    {
                        /*
                         * 0: Top
                         * 1: Bottom
                         * 2: Left
                         * 3: Right
                         */
                        bool[] dirs = new bool[4];
                        int cCount = 0;
                        bool c;

                        dirs[0] = elementPoints.Contains<int[]>(new int[] { i - 1, j }, secint);
                        dirs[1] = elementPoints.Contains<int[]>(new int[] { i + 1, j }, secint);
                        dirs[2] = elementPoints.Contains<int[]>(new int[] { i, j - 1 }, secint);
                        dirs[3] = elementPoints.Contains<int[]>(new int[] { i, j + 1 }, secint);

                        if (i == 0)
                        {
                            dirs[0] = true;
                            cCount++;
                        }

                        if (j == 0)
                        {
                            dirs[2] = true;
                            cCount++;
                        }

                        if (i == height - 1)
                        {
                            dirs[1] = true;
                            cCount++;
                        }

                        if (j == width - 1)
                        {
                            dirs[3] = true;
                            cCount++;
                        }

                        c = cCount == 2;

                        if (dirs.Count(x => x) < 2 || c)
                            builder[i][j] = 'O';
                        else if (dirs[0] && dirs[1])
                            builder[i][j] = '=';
                        else if (dirs[2] && dirs[3])
                            builder[i][j] = '|';
                        else if (dirs[0] || dirs[1])
                            builder[i][j] = '=';
                        else if (dirs[2] || dirs[3])
                            builder[i][j] = '|';
                    }
                }
            }

            for (int i = 0; i < builder.Length; i++)
            {
                foreach (char chr in builder[i])
                {
                    ret[i] += chr;
                }
            }
            return ret;
        }
    }
}
