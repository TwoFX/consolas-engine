using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    public class ProgressBar : DataBasedElement<double>
    {
        public ProgressBar(double initial, int width)
        {
            Height = 1;
            Width = width;
            lastRendered = new Canvas(1, Width);
            Update(initial);
        }

        public override Canvas Render()
        {
            if (hasChanged)
            {
                int tip = (int)Math.Floor(data * (Width - 2) + 1);
                lastRendered.Colors[0][0] = UIManager.DefaultColor;
                lastRendered.Colors[0][Width - 1] = UIManager.DefaultColor;
                lastRendered.Symbols[0][0] = '[';
                lastRendered.Symbols[0][Width - 1] = ']';
                for (int i = 1; i < Width - 1; i++)
                {
                    lastRendered.Colors[0][i] = UIManager.DefaultColor;
                    if (i < tip)
                    {
                        lastRendered.Symbols[0][i] = '=';
                    }
                    else if (data > 0 && i == tip)
                    {
                        lastRendered.Symbols[0][i] = '>';
                    }
                    else
                    {
                        lastRendered.Symbols[0][i] = ' ';
                    }
                }
                hasChanged = false;
            }
            return lastRendered;
        }
    }
}
