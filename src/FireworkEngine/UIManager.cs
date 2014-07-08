using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireworkEngine
{
    public static class UIManager
    {
        private class Switch
        {
            public int Length
            {
                get;
                set;
            }

            public ConsoleColor Color
            {
                get;
                set;
            }
        }

        static private UIScene currentElement;
        static private char[][] symbols;
        static List<List<Switch>> switches;
        static private bool isInitialized = false;

        public static ConsoleColor DefaultColor
        {
            get;
            set;
        }

        public static void Initialize(ConsoleColor defaultColor = ConsoleColor.Gray)
        {
            DefaultColor = defaultColor;
            isInitialized = true;
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
            if (!isInitialized)
            {
                throw new InvalidOperationException("UIManager has to be initialized");
            }

            Canvas rendered = currentElement.Render();
            symbols = rendered.Symbols;
            switches = processSwitches(rendered.Colors);
        }

        // Creates a collection that keeps track of when to switch colors
        private static List<List<Switch>> processSwitches(ConsoleColor[][] map)
        {
            var sw = new List<List<Switch>>();
            foreach (ConsoleColor[] line in map)
            {
                sw.Add(processLine(line));
            }
            return sw;
        }

        private static List<Switch> processLine(ConsoleColor[] line)
        {
            List<Switch> res = new List<Switch>();

            // current values
            int at = 0;
            ConsoleColor current;

            while (at < line.Length)
            {
                current = line[at];

                // Check how long the same color is used
                int length = line.Skip(at).TakeWhile(c => c == current).Count();

                res.Add(new Switch { Length = length, Color = current });

                // Skip forward
                at += length;
            }
            return res;
        }

        public static void DrawFrame()
        {
            Console.Clear();
            for (int i = 0; i < switches.Count; i++)
            {
                int at = 0;
                foreach (Switch sw in switches[i])
                {
                    // Set color
                    Console.ForegroundColor = sw.Color;

                    // Get the string to draw
                    char[] text = new char[sw.Length];
                    Array.Copy(symbols[i], at, text, 0, sw.Length);
                    Console.Write(text);

                    // Skip forward
                    at += sw.Length;
                }
                Console.WriteLine();
            }
        }
    }
}
