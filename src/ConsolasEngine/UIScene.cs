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
        private string[] lastRendered;
        private int[][] positions;
        private string[] captions;
        private int height;
        private int width;
        private char[][] preRendered;

        public bool HasChanged
        {
            get { return unrenderedContents.Any(item => item.HasChanged); }
        }

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public IRenderable this[int index]
        {
            get { return unrenderedContents[index]; }
        }

        public UIScene(IRenderable[] contents, int[][] positions, string[] captions, int height, int width)
        {
            unrenderedContents = contents;
            this.positions = positions;
            this.captions = captions;
            this.height = height;
            this.width = width;

            this.preRender();
        }

        public string[] RenderAndReturn()
        {
            if (this.HasChanged)
            {
                string[] ret = new string[height];
                char[][] rendered = preRendered;

                for (int element = 0; element < unrenderedContents.Length; element++)
                {
                    if (unrenderedContents[element].HasChanged)
                    {
                        char[][] renderedElement = unrenderedContents[element].RenderAndReturn().Select(str => str.ToCharArray()).ToArray();
                        insertInto<char>(rendered, renderedElement, positions[element][0], positions[element][1]);
                    }
                }

                for (int i = 0; i < rendered.Length; i++)
                {
                    ret[i] = new string(rendered[i]);
                }
                lastRendered = ret;
                return ret;
            }
            return lastRendered;
        }

        private void preRender()
        {
            char?[][] template = new char?[height][];

            for (int i = 0; i < height; i++)
            {
                template[i] = new char?[width];
            }

            for (int element = 0; element < unrenderedContents.Length; element++)
            {
                for (int elX = positions[element][0]; elX < positions[element][0] + unrenderedContents[element].Height; elX++)
                {
                    for (int elY = positions[element][1]; elY < positions[element][1] + unrenderedContents[element].Width; elY++)
                    {
                        template[elX][elY] = default(char);
                    }
                }
            }

            preRendered = fillNullWithBorders(template);
        }

        private void insertInto<T>(T[][] dest, T[][] source, int fromX, int fromY)
        {
            for (int line = 0; line < source.Length; line++)
            {
                Array.Copy(source[line], 0, dest[line + fromX], fromY, source[line].Length);
            }
        }

        private char[][] fillNullWithBorders(char?[][] scene)
        {
            char[][] result = new char[scene.Length][];
            for (int x = 0; x < scene.Length; x++)
            {
                result[x] = new char[scene[x].Length];
                for (int y = 0; y < scene[x].Length; y++)
                {
                    result[x][y] = scene[x][y] ?? determineBorderType(scene, x, y);
                }
            }
            return result;
        }

        private char determineBorderType(char?[][] reference, int x, int y)
        {
            bool[] sidesFree = { x > 0 ? reference[x - 1][y] == null : false, /* Top */
                             x < reference.Length - 1 ? reference[x + 1][y] == null : false, /* Bottom */
                             y > 0 ? reference[x][y - 1] == null : false, /* Left */
                             y < reference[x].Length - 1 ? reference[x][y + 1] == null : false, /* Right */ };

            bool topBot = (sidesFree[0] && sidesFree[1]);
            bool leftRight = (sidesFree[2] && sidesFree[3]);

            bool opposite = topBot || leftRight;

            int count = sidesFree.Count(s => s);

            // Free space
            if (count == 4) return '#';

            // Large intersection
            if (count > 2) return '0';

            // Either two oppsoite walls or a corner
            if (count == 2) return !opposite ? '0' : (topBot ? '|' : '-');

            // Small hack: sides.Free.TakeWhile(s => !s).Count() returns the first index that is not true,
            // so the first (and only) occupied side. Due to the ordering of the elements of sidesFree,
            // if index < 2 we are below or above, and if index >= 2 we are left or right.
            if (count == 1) return sidesFree.TakeWhile(s => !s).Count() < 2 ? '-' : '|';

            // UI coordinates have to take borders into consideration.
            throw new UIException("UI layout does not enable bordering.");
        }
    }
}