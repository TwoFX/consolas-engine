using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    public class UIElement : IRenderable
    {
        private Canvas contents;
        private int height;
        private int width;
        private bool hasChanged;

        public int Height
        {
            get { return height; }
        }

        public int Width
        {
            get { return width; }
        }

        public bool HasChanged
        {
            get { return hasChanged; }
        }

        public UIElement(Canvas contents)
        {
            height = contents.Height;
            width = contents.Width;
            Update(contents);
        }

        public void Invalidate()
        {
            hasChanged = true;
        }

        public Canvas Render()
        {
            hasChanged = false;
            return contents;
        }

        public void Update(Canvas contents)
        {
            this.contents = contents;
            hasChanged = true;
        }
    }
}
