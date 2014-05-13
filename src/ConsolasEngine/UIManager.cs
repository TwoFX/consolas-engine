using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsolasEngine
{
    public static class UIManager
    {
        static private UIScene currentElement;
        static private char[][] symbols;
        static private ArrayList[] switches;

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
                currentElement = new UIScene(new IRenderable[] { newScene }, new int[][] { new int[] { 1, 1 } }, new string[] { "" }, newScene.Height + 2, newScene.Width + 2);
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
            Canvas rendered = currentElement.Render();
            symbols = rendered.Symbols;
            switches = processSwitches(rendered.Colors);
        }

        private static ArrayList[] processSwitches(ConsoleColor[][] map)
        {
            var sw = new ArrayList[map.Length];
            for (int line = 0; line < map.Length; line++)
            {
                sw[line] = new ArrayList();
                IEnumerable<ConsoleColor> working = map[line];
                int sCount = map[line].Length;
                while (working.Any())
                {
                    int atIndex = sCount - working.Count();
                    sw[line].Add(atIndex);
                    sw[line].Add(map[line][atIndex]);
                    working = working.SkipWhile(clr => clr == map[line][atIndex]);
                }
                sw[line].Add(map[line].Length);
            }
            return sw;
        }

        public static void DrawFrame()
        {
            Console.Clear();
            for (int line = 0; line < currentElement.Height; line++)
            {
                for (int ind = 0; ind < switches[line].Count - 1; ind += 2)
                {
                    Console.ForegroundColor = (ConsoleColor)switches[line][ind + 1];
                    int start = (int)switches[line][ind];
                    int end = (int)switches[line][ind + 2];
                    char[] p = new char[end - start];
                    Array.Copy(symbols[line], start, p, 0, end - start);
                    Console.Write(p);
                }
                Console.WriteLine();
            }
        }
    }
}
