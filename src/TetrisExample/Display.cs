using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireworkEngine;

namespace TetrisExample
{
    class Display : DataBasedElement<Tetromino>
    {
        public Display(Tetromino tet)
        {
            this.Width = 8;
            this.Height = 8;
            Update(tet);
        }

        public override Canvas Render()
        {
            Canvas cvs = new Canvas(8, 8);
            if (data != null)
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 8; y++)
                    {
                        cvs.Colors[x][y] = data.Color;
                        cvs.Symbols[x][y] = data.Blocking[x / 2][y / 2] ? '#' : ' ';
                    }
                }
            }
            return cvs;
        }
    }
}
