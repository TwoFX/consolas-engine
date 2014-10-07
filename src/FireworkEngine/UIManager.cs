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
            public string Text
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
        static private Canvas rendered;
        static private List<List<Switch>> switches;
        static private bool isInitialized = false;

        public static ConsoleColor DefaultColor
        {
            get;
            set;
        }

        public static void Initialize(ConsoleColor defaultColor = ConsoleColor.Gray)
        {
            switches = new List<List<Switch>>();
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

            rendered = currentElement.Render();
            processSwitches();
        }

        // Creates a collection that keeps track of when to switch colors
        private static void processSwitches()
        {
            switches.Clear();
            for (int i = 0; i < rendered.Height; i++)
            {
                switches.Add(processLine(rendered.Colors[i], rendered.Symbols[i]));
            }
        }

        private static List<Switch> processLine(ConsoleColor[] line, char[] text)
        {
            List<Switch> res = new List<Switch>();

            // current values
            int at = 0;
            ConsoleColor current;

            while (at < line.Length)
            {
                current = line[at];

                // Check how long the same color is used
                int length = line.Skip(at).TakeWhile((c, i) => c == current || text[i + at] == '\0' || text[i + at] == ' ').Count();

                char[] rText = new char[length];
                Array.Copy(text, at, rText, 0, length);

                res.Add(new Switch { Text = new string(rText), Color = current });

                

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
                foreach (Switch sw in switches[i])
                {
                    // Set color
                    Console.ForegroundColor = sw.Color;

                    // Get the string to draw
                    Console.Write(sw.Text);
                }
                Console.WriteLine();
            }
        }
    }
}
