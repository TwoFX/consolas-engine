using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FW = FireworkEngine;
using FireworkEngine;

namespace FireworkEngineTests
{
    class Program
    {
        static void Main(string[] args)
        {
            MainTest();
        }

        static void MainTest()
        {
            UIScene currentElement;
            Table myElement, myOtherElement, ThirdElement;
            FW.Buffer buffer;
            ProgressBar pb;

            UIManager.Initialize(ConsoleColor.White);

            myElement = new Table(new string[][] {new string[] {
                "Left 1", "Right 1"},
                new string[] {"Left 2", "Right 2"},
                new string[] {"Left 3", "Right 3 Looong"},
                new string[] {"Left 444444554545454444", "t"},
                new string[] {"i", "asdasdasdasdasdasdasdasd"},
                new string[] {"Level", "9001"}}, 30, null, ConsoleColor.Red, TableMode.LeftHeader);

            myOtherElement = new Table(new string[][] { // So sieht der Quellcode aus
                new string[] {"This", "Here"},
                new string[] {"is", "be"},
                new string[] {"the", "data"},
                new string[] {"title", "00"},
                new string[] {"line", "000"},
                new string[] {"!", "0000"}}, 30, null, ConsoleColor.Red, TableMode.LeftHeader);

            ThirdElement = new Table(new string[][] {new string[] {
                "Name", "OP"},
                new string[] {"OP?", "OP"},
                new string[] {",,,,,,,,", "......"},
                new string[] {"       Needs trimming", "9002"},
                new string[] {"Intelligenz", "Really does                   "},
                new string[] {"Level", "Colorful console game thingy"}}, 61, null, ConsoleColor.Red, TableMode.TopHeader);

            pb = new ProgressBar(0, 61);

            currentElement = new UIScene(new IRenderable[] { myElement, myOtherElement, ThirdElement, pb },
                new int[][] { new int[] { 1, 1 }, new int[] { 1, 32 }, new int[] { 8, 1 }, new int[] { 15, 1 } },
                new string[] { "Spieler 1", "Spieler 2", "OMG Gegner", "Progress" }, 17, 63, ConsoleColor.Green, null);



            UIManager.setScene(currentElement);

            UIManager.Render();
            UIManager.DrawFrame();

            Console.ReadKey();

            pb.Update(0.5);
            UIManager.Render();
            UIManager.DrawFrame();
            Console.ReadKey();

            pb.Update(0.3);
            UIManager.Render();
            UIManager.DrawFrame();
            Console.ReadKey();

            pb.Update(1);
            UIManager.Render();
            UIManager.DrawFrame();
            Console.ReadKey();

            currentElement = new UIScene(new IRenderable[] { myOtherElement, myElement, ThirdElement },
                new int[][] { new int[] { 1, 1 }, new int[] { 1, 32 }, new int[] { 8, 1 } },
                new string[] { "Spieler 1", "Spieler 2", "OMG Gegner" }, 15, 63, ConsoleColor.Green, null);



            UIManager.setScene(currentElement);

            UIManager.Render();
            UIManager.DrawFrame();

            Console.ReadKey();

            buffer = new FW.Buffer(myElement, new FireworkEngine.Margin { Top = 3, Left = 6, Bottom = 3, Right = 7 }, ' ');
            currentElement = new UIScene(new[] { buffer }, new[] { new[] { 1, 1 } }, new[] { "Buffer Test" }, 14, 45);

            UIManager.setScene(currentElement);

            UIManager.Render();
            UIManager.DrawFrame();

            Console.ReadKey();
        }
    }
}
