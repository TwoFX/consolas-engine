using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using FireworkEngine;

namespace TetrisExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //System.Threading.Thread msgLoop = new System.Threading.Thread(new System.Threading.ThreadStart())

            TetrisField field = new TetrisField();
            Timer gameTimer = new Timer(1000);
            gameTimer.Elapsed += (sender, e) =>
                {
                    field.Gravity();
                    UIManager.Render();
                    UIManager.DrawFrame();
                };
            gameTimer.Start();

            Display disp = new Display(field.Next);
            field.TetrominoLocked += (sender, e) => disp.Update(e.Next);

            Display held = new Display(field.Hold);
            field.TetrominoHeld += (sender, e) => held.Update(e.Hold);

            //UIThread.Start();

            UIScene scene = new UIScene(new IRenderable[] { field, disp, held }, new[] { new[] { 1, 1 }, new[] { 1, 22 }, new[] {10, 22}}, new[] { "Field", "Next", "Hold"}, 42, 31, ConsoleColor.White, ConsoleColor.White);
            UIManager.Initialize();
            UIManager.setScene(scene);
            while (true)
            {
                UIManager.Render();
                UIManager.DrawFrame();
                ConsoleKeyInfo k = Console.ReadKey(false);
                if (k.Key == ConsoleKey.LeftArrow)
                {
                    field.ProcessInput(UserInput.Left);
                }
                else if (k.Key == ConsoleKey.RightArrow)
                {
                    field.ProcessInput(UserInput.Right);
                }
                else if (k.Key == ConsoleKey.UpArrow)
                {
                    field.ProcessInput(UserInput.RotateClockwise);
                }
                else if (k.Key == ConsoleKey.DownArrow)
                {
                    field.ProcessInput(UserInput.Down);
                }
                else if (k.Key == ConsoleKey.Spacebar)
                {
                    field.ProcessInput(UserInput.Lock);
                }
                else if (k.Key == ConsoleKey.A)
                {
                    field.ProcessInput(UserInput.Hold);
                }
            }
            Console.ReadKey(true);
        }
    }
}
