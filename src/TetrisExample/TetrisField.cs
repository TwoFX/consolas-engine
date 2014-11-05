using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireworkEngine;

namespace TetrisExample
{
    enum UserInput
    {
        Left = 0, Right, Down, Lock, RotateClockwise, RotateCClockwise, Hold
    }

    class TetrominoLockedEventArgs : EventArgs
    {
        public Tetromino Next
        {
            get;
            set;
        }
    }

    class TetrominoHeldEventArgs : EventArgs
    {
        public Tetromino Hold
        {
            get;
            set;
        }
    }

    class TetrisField : IRenderable
    {
        private bool[][] permaBlocked;
        private volatile ConsoleColor?[][] permaColors;
        private Tetromino active, next, ghost, hold;
        private bool dueLock;
        private bool isLocking;

        public delegate void lockedDelegate(object sender, TetrominoLockedEventArgs e);
        public event lockedDelegate TetrominoLocked;

        public delegate void holdDelegate(object sender, TetrominoHeldEventArgs e);
        public event holdDelegate TetrominoHeld;

        public Tetromino Next
        {
            get
            {
                return next;
            }
        }

        public Tetromino Hold
        {
            get
            {
                return hold;
            }
        }

        public TetrisField()
        {
            permaBlocked = new bool[22][];
            for (int i = 0; i < 22; i++)
            {
                permaBlocked[i] = new bool[10];
            }
            permaColors = new ConsoleColor?[20][];
            for (int i = 0; i < 20; i++)
            {
                permaColors[i] = new ConsoleColor?[10];
            }

            active = Tetromino.getRandom();
            next = Tetromino.getRandom();
            ghost = new Tetromino(active);

            HasChanged = true;
            active.X = 0;
            active.Y = 3;
        }

        public int Width
        {
            get
            {
                return 20;
            }
        }

        public int Height
        {
            get
            {
                return 40;
            }
        }

        public bool HasChanged
        {
            get;
            private set;
        }

        public void Invalidate()
        {
            HasChanged = true;
        }

        public Canvas Render()
        {
            Canvas field = new Canvas(40, 20);
            ghost.Reset();

            while (tryMove(1, 0, ghost))
            {
                ghost.X++;
            }

            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    field.Symbols[x][y] = isBlocked((int)Math.Floor(x / 2.0) + 2, (int)Math.Floor(y / 2.0)) ? '#' : ' ';
                    field.Colors[x][y] = effectiveColor((int) Math.Floor(x / 2.0) + 2, (int)Math.Floor(y / 2.0));
                }
            }
            return field;
        }

        public bool isBlocked(int x, int y)
        {
            return permaBlocked[x][y] || active.effectiveBlock(x, y) || ghost.effectiveBlock(x, y);
        }

        public ConsoleColor effectiveColor(int x, int y)
        {
            return x >= 2 ? permaColors[x - 2][y] ?? (active.effectiveBlock(x, y) ? active.Color : ghost.Color) : active.Color;
        }

        private void lockActive()
        {
            isLocking = true;
            for (int i = 0; i < 4; i++)
            {
                
                for (int j = 0; j < 4; j++)
                {
                    if (i + active.X < 22 && j + active.Y < 10 && i + active.X >= 0 && j + active.Y >= 0)
                    {
                        permaBlocked[i + active.X][j + active.Y] |= active.Blocking[i][j];
                        if (active.Blocking[i][j] && i + active.X >= 2)
                        {
                            permaColors[i + active.X - 2][j + active.Y] = active.Color;
                        }
                    }
                }
            }
            active = next;
            next = Tetromino.getRandom();
            ghost = new Tetromino(active);
            active.X = 0;
            active.Y = 3;
            dueLock = false;
            isLocking = false;

            for (int i = 2; i < 22; i++)
            {
                if (permaBlocked[i].All(x => x))
                {
                    for (int j = i; j > 0; j--)
                    {
                        permaBlocked[j] = permaBlocked[j - 1];
                        if (j >= 3)
                        {
                            permaColors[j - 2] = permaColors[j - 3];
                        }
                    }
                    permaBlocked[0] = new bool[10];
                    permaColors[0] =  new ConsoleColor?[10];
                }
            }

            var args = new TetrominoLockedEventArgs();
            args.Next = next;
            TetrominoLocked(this, args);
            if (permaBlocked[1].Any(x => x))
            {
                Environment.Exit(0);
            }
        }

        private void evaluateLock()
        {
            if (!tryMove(1, 0, active))
            {
                dueLock = true;
            }
        }

        private bool tryMove(int x, int y, Tetromino tet)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (tet.Blocking[i][j])
                    {
                        int translatedX = tet.X + i + x;
                        int translatedY = tet.Y + j + y;

                        if (occupied(translatedX, translatedY))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool occupied(int x, int y)
        {
            return x < 0 || y < 0 || x >= 22 || y >= 10 || permaBlocked[x][y];
        }

        private void doRotate(bool counter)
        {
            int index = active.RotationState % 4;
            if (counter)
            {
                index += 4;
            }

            if (counter)
            {
                active.RotateCClockwise();
            }
            else
            {
                active.RotateClockwise();
            }

            if (!tryMove(0, 0, active))
            {
                // Assumption: this code path will never be taken if active.Type == TetrominoType.O

                int[][][] kicktable = active.Type == TetrominoType.I ? iKickTable : kickTable;

                foreach (int[] translation in kicktable[index])
                {
                    int xMove = -translation[1];
                    int yMove = translation[0];

                    if (tryMove(xMove, yMove, active))
                    {
                        active.X += xMove;
                        active.Y += yMove;
                        dueLock = false;
                        return;
                    }
                }

                // Rotation failed, revert
                if (counter)
                {
                    active.RotateClockwise();
                }
                else
                {
                    active.RotateCClockwise();
                }
            }
        }

        public void Gravity()
        {
            if (dueLock && !isLocking)
            {
                lockActive();
            }
            else
            {
                if (tryMove(1, 0, active))
                {
                    active.X++;
                }
                evaluateLock();
            }
            HasChanged = true;
        }

        public void ProcessInput(UserInput input)
        {
            switch (input)
            {
                case UserInput.Down:
                    if (tryMove(1, 0, active))
                    {
                        active.X++;
                    }
                    break;

                case UserInput.Left:
                    if (tryMove(0, -1, active))
                    {
                        active.Y--;
                    }
                    break;

                case UserInput.Right:
                    if (tryMove(0, 1, active))
                    {
                        active.Y++;
                    }
                    break;

                case UserInput.Lock:
                    while (tryMove(1, 0, active))
                    {
                        active.X++;
                    }
                    lockActive();
                    break;

                case UserInput.RotateClockwise:
                    doRotate(false);
                    break;

                case UserInput.RotateCClockwise:
                    doRotate(true);
                    break;

                case UserInput.Hold:
                    if (hold == null)
                    {
                        hold = active;
                        active = next;
                        next = Tetromino.getRandom();
                        TetrominoLockedEventArgs largs = new TetrominoLockedEventArgs();
                        largs.Next = next;
                        TetrominoLocked(this, largs);
                    }
                    else
                    {
                        Tetromino temp = hold;
                        hold = active;
                        active = temp;
                    }
                    ghost = new Tetromino(active);
                    active.X = 0;
                    active.Y = 3;
                    TetrominoHeldEventArgs args = new TetrominoHeldEventArgs();
                    args.Hold = hold;
                    TetrominoHeld(this, args);
                    break;
            }
            dueLock = false;
            evaluateLock();
            HasChanged = true;
        }

        #region Kicktables
        private static readonly int[][][] kickTable = new[]
        {
            new[] // 0 -> R
            {
                new[] { -1, 0 },
                new[] { -1, 1 },
                new[] { 0, -2 },
                new[] { -1, -2 }
            },
            new[] // R -> 2
            {
                new[] { 1, 0 },
                new[] { 1, -1 },
                new[] { 0, 2 },
                new[] { 1, 2 }
            },
            new[] // 2 -> L
            {
                new[] { 1, 0 },
                new[] { 1, 1 },
                new[] { 0, -2 },
                new[] { 1, -2 }
            },
            new[] // L -> 0
            {
                new[] { -1, 0 },
                new[] { -1, -1 },
                new[] { 0, 2 },
                new[] { -1, 2 }
            },
            new[] // 0 -> L
            {
                new[] { 1, 0 },
                new[] { 1, 1 },
                new[] { 0, -2 },
                new[] { 1, -2 }
            },
            new[] // R -> 0
            {
                new[] { 1, 0 },
                new[] { 1, -1 },
                new[] { 0, 2 },
                new[] { 1, 2 }
            },
            new[] // 2 -> R
            {
                new[] { -1, 0 },
                new[] { -1, 1 },
                new[] { 0, -2 },
                new[] { -1, -2 }
            },
            new[] // L -> 2
            {
                new[] { -1, 0 },
                new[] { -1, -1 },
                new[] { 0, 2 },
                new[] { -1, 2 }
            }
        };

        private static readonly int[][][] iKickTable = new[]
        {
            new[] // 0 -> R
            {
                new[] { -2, 0 },
                new[] { 1, 0 },
                new[] { -2, -1 },
                new[] { 1, 2 }
            },
            new[] // R -> 2
            {
                new[] { -1, 0 },
                new[] { 2, 0 },
                new[] { -1, 2 },
                new[] { 2, -1 }
            },
            new[] // 2 -> L
            {
                new[] { 2, 0 },
                new[] { -1, 0 },
                new[] { 2, 1 },
                new[] { -1, -2 }
            },
            new[] // L -> 0
            {
                new[] { 1, 0 },
                new[] { -2, 0 },
                new[] { 1, -2 },
                new[] { -2, 1}
            },
            new[] // 0 -> L
            {
                new[] { -1, 0 },
                new[] { 2, 0 },
                new[] { -1, 2 },
                new[] { 2, -1 }
            },
            new[] // R -> 0
            {
                new[] { 2, 0 },
                new[] { -1, 0 },
                new[] { 2, 1 },
                new[] { -1, -2 }
            },
            new[] // 2 -> R
            {
                new[] { 1, 0 },
                new[] { -2, 0 },
                new[] { 1, -2 },
                new[] { -2, 1 }
            },
            new[] // L -> 2
            {
                new[] { -2, 0 },
                new[] { 1, 0 },
                new[] { -2, -1 },
                new[] { 1, 2 }
            }
        };
        #endregion
    }
}
