using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    public abstract class DataBasedElement<T> : IRenderable
    {
        protected bool hasChanged;
        protected T unrendered;
        protected Canvas lastRendered;

        public bool HasChanged
        {
            get { return hasChanged; }
        }

        public int Height
        {
            get;
            protected set;
        }

        public int Width
        {
            get;
            protected set;
        }

        public void Update(T contents)
        {
            unrendered = contents;
            hasChanged = true;
        }

        public void Invalidate()
        {
            hasChanged = true;
        }

        public abstract Canvas Render();
    }
}