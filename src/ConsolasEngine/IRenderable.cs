using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public interface IRenderable
    {
        int Width
        {
            get;
        }

        int Height
        {
            get;
        }

        bool HasChanged
        {
            get;
        }

        Canvas Render();
        void Invalidate();
    }
}
