using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public enum TableMode
    {
        NoHeader, LeftHeader, TopHeader
    }

    public class Table : IRenderable
    {
        private string[][] unrenderedContents;
        private Canvas lastRendered;
        private bool hasChanged;
        private ConsoleColor? textColor;
        private ConsoleColor? headingColor;

        public bool HasChanged
        {
            get { return hasChanged; }
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

        public ConsoleColor TextColor
        {
            get
            {
                return textColor ?? UIManager.DefaultColor;
            }

            set
            {
                textColor = value;
            }
        }

        public ConsoleColor HeadingColor
        {
            get
            {
                return headingColor ?? UIManager.DefaultColor;
            }

            set
            {
                headingColor = value;
            }
        }

        public TableMode TableMode
        {
            get;
            set;
        }

        public void nullableSetTextColor(ConsoleColor? value)
        {
            textColor = value;
        }

        public void nullableSetHeadingColor(ConsoleColor? value)
        {
            headingColor = value;
        }

        public void Invalidate()
        {
            hasChanged = true;
        }

        public Table(string[][] contents, int lengthSum, ConsoleColor? textColor = null, ConsoleColor? headingColor = null, TableMode tableMode = TableMode.NoHeader)
        {
            this.TableMode = tableMode;
            this.headingColor = headingColor;
            this.textColor = textColor;
            Height = contents.Length;
            Width = lengthSum;
            lastRendered = new Canvas(Height, Width);
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
                char[][] renderedContents = new char[Height][];
                for (int i = 0; i < Height; i++)
                {
                    for (int sp = 0; sp < Width; sp++)
                    {
                        renderedContents[i] = new char[Width];
                        renderedContents[i][sp] = ' ';

                        ConsoleColor chosenColor = TextColor;

                        switch (this.TableMode)
                        {
                            case TableMode.NoHeader:
                                chosenColor = TextColor;
                                break;

                            case TableMode.LeftHeader:
                                chosenColor = sp < unrenderedContents[i][0].Length ?
                                    HeadingColor :
                                    TextColor;
                                break;

                            case TableMode.TopHeader:
                                chosenColor = i == 0 ?
                                    HeadingColor :
                                    TextColor;
                                break;
                        }

                        lastRendered.Colors[i][sp] = chosenColor;
                    }
                    for (int j = 0; j < 2; j++)
                    {
                        for (int k = 0; k < unrenderedContents[i][j].Length; k++)
                        {
                            if (unrenderedContents[i][0].Length + unrenderedContents[i][1].Length >= Width)
                                throw new UIException("Column length sum exceeded table width");
                            renderedContents[i][j == 0 ? k : Width - unrenderedContents[i][j].Length + k] = unrenderedContents[i][j].ToCharArray()[k];
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
