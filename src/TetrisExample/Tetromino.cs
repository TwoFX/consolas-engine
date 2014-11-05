using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TetrisExample
{
    enum TetrominoType
    {
        I = 0, O, T, S, Z, J, L
    }

    class Tetromino
    {
        #region Fields and Properties
        public ConsoleColor Color
        {
            get;
            private set;
        }

        public int X
        {
            get;
            set;
        }

        public int Y
        {
            get;
            set;
        }

        private bool[][] _blocking;

        public bool[][] Blocking
        {
            get
            {
                if (mother == null)
                {
                    return _blocking;
                }
                else return mother.Blocking;
            }
            private set
            {
                _blocking = value;
            }
        }

        public TetrominoType Type
        {
            get;
            private set;
        }

        public int RotationState
        {
            get;
            private set;
        }

        private Tetromino mother;
        #endregion

        #region Public Methods and Constructors
        public Tetromino(TetrominoType type)
        {
            this.Type = type;
            RotationState = 0;
            switch (type)
            {
                case TetrominoType.I:;
                    this.Color = ConsoleColor.Cyan;
                    break;

                case TetrominoType.O:
                    this.Color = ConsoleColor.DarkYellow;
                    break;

                case TetrominoType.T:
                    this.Color = ConsoleColor.Magenta;
                    break;

                case TetrominoType.S:
                    this.Color = ConsoleColor.Green;
                    break;

                case TetrominoType.Z:
                    this.Color = ConsoleColor.DarkRed;
                    break;

                case TetrominoType.J:
                    this.Color = ConsoleColor.White;
                    break;

                case TetrominoType.L:
                    this.Color = ConsoleColor.Red;
                    break;
            }
            this.Blocking = fromShort(rotationData[(int)Type][RotationState]);
        }

        public void Reset()
        {
            if (mother != null)
            {
                this.X = mother.X;
                this.Y = mother.Y;
            }
        }

        public Tetromino(Tetromino mother) // Ghost piece
        {
            this.Color = ConsoleColor.Gray;
            this.mother = mother;
        }
       
        public void RotateClockwise()
        {
            RotationState++;
            this.Blocking = fromShort(rotationData[(int)Type][RotationState % 4]);
        }

        public void RotateCClockwise()
        {
            RotationState--;
            if (RotationState == -1)
            {
                RotationState = 3;
            }
            this.Blocking = fromShort(rotationData[(int)Type][RotationState % 4]);
        }

        public bool effectiveBlock(int x, int y)
        {
            int offsetX = x - X;
            int offsetY = y - Y;

            if (offsetX >= 0 && offsetY >= 0 && offsetX < 4 && offsetY < 4)
            {
                return Blocking[offsetX][offsetY];
            }
            return false;
        }
        #endregion

        #region Private Methods and Constructors
        private bool[][] fromShort(short n)
        {
            bool[][] result = new bool[4][];

            for (int i = 0; i < 4; i++)
            {
                result[3 - i] = new bool[4];
                for (int j = 0; j < 4; j++)
                {
                    result[3 - i][3 - j] = (n >> (4 * i + j)) % 2 != 0;
                }
            }

            return result;
        }
        #endregion

        #region Static
        private static Random rng;

        public static Tetromino getRandom()
        {
            if (rng == null)
            {
                rng = new Random();
            }
            return new Tetromino((TetrominoType)rng.Next(7));
        }

        private static readonly short[][] rotationData = new[]
        {
            new short[] { 3840, 8738, 240, 17476 },
            new short[] { 26112, 26112, 26112, 26112 },
            new short[] { 19968, 17984, 3648, 19520 },
            new short[] { 27648, 17952, 1728, -29632 },
            new short[] { -14848, 9792, 3168, 19584 },
            new short[] { -29184, 25664, 3616, 17600 },
            new short[] { 11776, 17504, 3712, -15296 }
        };
        #endregion
    }
}