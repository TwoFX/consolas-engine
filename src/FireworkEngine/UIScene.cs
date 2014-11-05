using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    public class UIScene : IRenderable
    {
        // TODO: Infer size
        private IRenderable[] unrenderedContents;
        private Canvas lastRendered;
        private int[][] positions;
        private string[] captions;
        private int height;
        private int width;
        private ConsoleColor? captionColor;
        private ConsoleColor? borderColor;

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

        public ConsoleColor CaptionColor
        {
            get
            {
                return captionColor ?? UIManager.DefaultColor;
            }

            set
            {
                captionColor = value;
            }
        }
        public ConsoleColor BorderColor
        {
            get
            {
                return borderColor ?? UIManager.DefaultColor;
            }

            set
            {
                borderColor = value;
            }
        }

        public void nullableSetCaptionColor(ConsoleColor? value)
        {
            captionColor = value;
        }

        public void nullableSetBorderColor(ConsoleColor? value)
        {
            borderColor = value;
        }

        public UIScene(IRenderable[] contents, int[][] positions, string[] captions, int height, int width, ConsoleColor? captionColor = null, ConsoleColor? borderColor = null)
        {
            unrenderedContents = contents;
            this.positions = positions;
            this.captions = captions;
            this.height = height;
            this.width = width;
            this.captionColor = captionColor;
            this.borderColor = borderColor;
            lastRendered = new Canvas(height, width);
            this.preRender();
            this.Invalidate();
        }

        public Canvas Render()
        {
            if (this.HasChanged)
            {
                Canvas rendered = lastRendered;

                // Render every element that has changed since last time and insert it into the canvas
                for (int element = 0; element < unrenderedContents.Length; element++)
                {
                    if (unrenderedContents[element].HasChanged)
                    {
                        Canvas renderedElement = unrenderedContents[element].Render();
                        Helper.Copy2D(rendered.Symbols, renderedElement.Symbols, positions[element][0], positions[element][1]);
                        Helper.Copy2D(rendered.Colors, renderedElement.Colors, positions[element][0], positions[element][1]);
                    }
                }
                lastRendered = rendered;
            }
            return lastRendered;
        }

        public void Invalidate()
        {
            foreach (IRenderable element in unrenderedContents)
            {
                element.Invalidate();
            }
        }

        private void preRender()
        {
            // Initialize
            char?[][] template = new char?[height][];
            for (int i = 0; i < height; i++)
            {
                template[i] = new char?[width];
            }

            // Fill all places where there will be something later with somethigs that is not null
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

            for (int x = 0; x < height; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    if (template[x][y] == null)
                    {
                        lastRendered.Colors[x][y] = this.BorderColor;
                    }
                }
            }

            lastRendered.Symbols = fillNullWithBorders(template);

            // Captions
            for (int element = 0; element < unrenderedContents.Length; element++)
            {
                if (captions[element].Length <= unrenderedContents[element].Width - 2)
                {
                    Helper.Copy2D(lastRendered.Symbols, captions[element].ToCharArray(), positions[element][0] - 1, positions[element][1] + 1);
                    Helper.Copy2D(lastRendered.Colors, Helper.Repeat(this.CaptionColor, captions[element].Length), positions[element][0] - 1, positions[element][1] + 1);
                }
            }
        }

        // Fill all pieces of the canvas where there are no elements (value == null) with borders
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

        // Analyze the surroundings of a position within the canvas and find out whether to put '0', '-' or '|'
        private char determineBorderType(char?[][] reference, int x, int y)
        {
            bool[] sidesFree = { x > 0 ? reference[x - 1][y] == null : false, /* Top */
                             x < reference.Length - 1 ? reference[x + 1][y] == null : false, /* Bottom */
                             y > 0 ? reference[x][y - 1] == null : false, /* Left */
                             y < reference[x].Length - 1 ? reference[x][y + 1] == null : false, /* Right */ };

            bool topBot = sidesFree[0] && sidesFree[1];
            bool leftRight = sidesFree[2] && sidesFree[3];

            bool opposite = topBot || leftRight;

            int count = sidesFree.Count(s => s);

            switch (count)
            {
                case 0:
                    // UI coordinates have to take borders into consideration.
                    throw new UIException("UI layout does not enable bordering.");

                case 1:
                    // Small hack: sides.Free.TakeWhile(s => !s).Count() returns the first index that is not true,
                    // so the first (and only) occupied side. Due to the ordering of the elements of sidesFree,
                    // if index < 2 we are below or above, and if index >= 2 we are left or right.
                    return sidesFree.TakeWhile(s => !s).Count() < 2 ? '-' : '|';

                case 2:
                    // Either two oppsoite walls or a corner
                    return !opposite ? '0' : (topBot ? '|' : '-');

                default:
                    // Large intersection
                    return Enumerable.Range(x - 1, 3).All(_x => Enumerable.Range(y - 1, 3).All(_y => 
                        _x < 0 || _y < 0 || _x >= reference.Length || _y >= reference[_x].Length || reference[_x][_y] == null)) ? ' ' : '0';
                    //return '0';
            }
        }
    }
}