using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
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

                for (int i = 0; i < Height; i++)
                {

                    for (int sp = 0; sp < Width; sp++)
                    {
                        ConsoleColor chosenColor = TextColor;

                        // Decide whether a slot is part of a heading
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

                    // Iterate over each word and fill it in
                    for (int j = 0; j < 2; j++)
                    {
                        if (unrenderedContents[i][0].Length + unrenderedContents[i][1].Length >= Width)
                        {
                            throw new UIException("Column length sum exceeded table width");
                        }

                        char[] word = unrenderedContents[i][j].ToCharArray();

                        // Start the copy left-bound or fight-bound
                        int start = 0;
                        if (j == 1)
                        {
                            start = Width - unrenderedContents[i][j].Length;
                        }

                        Array.Copy(word, 0, lastRendered.Symbols[i], start, word.Length);
                    }

                }
                hasChanged = false;
            }
            return lastRendered;
        }
    }
}
