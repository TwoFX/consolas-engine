using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public static class UIManager
    {
        static private System.Timers.Timer frames = new System.Timers.Timer(500);
        static private UIScene currentElement;
        static private string[] renderedScreen;

        public static void Initialize()
        {
        }

        public static void setScene(IRenderable newScene)
        {
            if (newScene.GetType() == typeof(UIScene))
            {
                currentElement = (UIScene)newScene;
            }
            else
            {
                currentElement = new UIScene(new IRenderable[] { newScene }, new int[][] { new int[] { 0, 0 } }, new string[] { "" }, newScene.Height, newScene.Width);
            }
            Console.WindowWidth = currentElement.Width;
            Console.WindowHeight = currentElement.Height + 1;
            Console.BufferHeight = currentElement.Height + 1;
            Console.BufferWidth = currentElement.Width + 1;
        }

        public static UIScene GetScene()
        {
            return currentElement;
        }

        public static void Render()
        {
            renderedScreen = currentElement.RenderAndReturn();
        }

        public static void DrawFrame()
        {
            Console.Clear();
            foreach (string line in renderedScreen)
                Console.WriteLine(line);
        }
    }
}
