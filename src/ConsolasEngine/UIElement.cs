using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public class UIElement : IRenderable
    {
        private string[] renderedContents;

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

        public UIElement(string[] contents, int maxLength)
        {
            height = contents.Length;
            width = maxLength;
            Update(contents);
        }

        private void validateLength(string[] contents)
        {
            bool lengthFail = false;
            foreach (string item in contents)
            {
                if (item.Length > width) lengthFail = true;
            }
            if (lengthFail) throw new UIException("The length of the UI data does not match to the UIElement's parameters");
        }

        public string[] RenderAndReturn()
        {
            hasChanged = false;
            return renderedContents;
        }

        public void Update(string[] contents)
        {
            validateLength(contents);
            renderedContents = contents;
            hasChanged = true;
        }
    }
}
