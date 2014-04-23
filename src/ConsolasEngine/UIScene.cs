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
        }

        public string[] RenderAndReturn()
        {
            if (HasChanged)
            {
                string[] ret = new string[height];
                char?[][] builder = new char?[height][];
                List<int[]> elementPoints = new List<int[]>();

                for (int i = 0; i < builder.Length; i++)
                {
                    builder[i] = new char?[width];
                }

                for (int element = 0; element < unrenderedContents.Length; element++)
                {
                    char?[][] renderedElement = unrenderedContents[element].RenderAndReturn().Select(str => str.ToCharArray().Select(c => (char?)c).ToArray()).ToArray();
                    insertInto<char?>(builder, renderedElement, positions[element][0], positions[element][1]);
                }

                char[][] filled = fillBorders(builder);

                for (int i = 0; i < filled.Length; i++)
                {
                    ret[i] = new string(filled[i]);
                }
                lastRendered = ret;
                return ret;
            }
            return lastRendered;
        }

        private void insertInto<T>(T[][] dest, T[][] source, int fromX, int fromY)
        {
            for (int line = 0; line < source.Length; line++)
            {
                Array.Copy(source[line], 0, dest[line + fromX], fromY, source[line].Length);
            }
        }

        private char[][] fillBorders(char?[][] scene)
        {
            char[][] result = new char[scene.Length][];
            for (int x = 0; x < scene.Length; x++)
            {
                result[x] = new char[scene[x].Length];
                for (int y = 0; y < scene.Length; y++)
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

            int count = 4 - sidesFree.Count(s => s);

            if (count > 2) return 'O';
            if (count == 2) return !opposite ? 'O' : (topBot ? '|' : '=');
            if (count == 1) return sidesFree.TakeWhile(s => !s).Count() < 2 ? '=' : '|';
            return '#';
        }
    }
}
