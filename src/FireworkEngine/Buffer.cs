using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    public class Buffer : IRenderable
    {
        public int Width
        {
            get
            {
                return Margin.Left + Margin.Right + Inner.Width;
            }
        }

        public int Height
        {
            get
            {
                return Margin.Top + Margin.Bottom + Inner.Height;
            }
        }

        public Margin Margin
        {
            get;
            private set;
        }

        public bool HasChanged
        {
            get
            {
                return !firstDrawn || Inner.HasChanged;
            }
        }

        public IRenderable Inner
        {
            get;
            private set;
        }

        public void Invalidate()
        {
            Inner.Invalidate();
        }

        public char Filler
        {
            get;
            private set;
        }

        private Canvas lastRendered;
        private bool firstDrawn;

        public Buffer(IRenderable inner, Margin margin, char filler = ' ')
        {
            this.Filler = filler;
            this.Margin = margin;
            this.Inner = inner;

            // Init canvas
            lastRendered = new Canvas(this.Height, this.Width);

            lastRendered.Symbols = Helper.Repeat(Helper.Repeat(filler, this.Width), this.Height);
            lastRendered.Colors = Helper.Repeat(Helper.Repeat(UIManager.DefaultColor, this.Width), this.Height);

            firstDrawn = false;
        }

        public Canvas Render()
        {
            if (HasChanged)
            {
                Canvas inner = Inner.Render();
                firstDrawn = true;
                Helper.Copy2D(lastRendered.Symbols, inner.Symbols, this.Margin.Top, this.Margin.Left);
                Helper.Copy2D(lastRendered.Colors, inner.Colors, this.Margin.Top, this.Margin.Left);
            }
            return lastRendered;
        }
    }
}
