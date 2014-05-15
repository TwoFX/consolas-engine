using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsolasEngine;

namespace ConsolasEngineTests
{
    class Program
    {
        static void Main(string[] args)
        {
            UIScene currentElement;
            UITable myElement, myOtherElement, ThirdElement;

            UIManager.Initialize(ConsoleColor.White);

            myElement = new UITable(new string[][] {new string[] {
                "Name", "Markus"},
                new string[] {"Rasse", "Epische Epischheit"},
                new string[] {"Staerke", "9001"},
                new string[] {"Agilitaet", "9001"},
                new string[] {"Intelligenz", "asdasdasdasdasd"},
                new string[] {"Level", "9001"}}, 30, null, ConsoleColor.Red);

            myOtherElement = new UITable(new string[][] { // So sieht der Quellcode aus
                new string[] {"Name", "Lukas"},
                new string[] {"Rasse", "Menschendings"},
                new string[] {"Staerke", "0"},
                new string[] {"Agilitaet", "00"},
                new string[] {"Intelligenz", "000"},
                new string[] {"Level", "0000"}}, 30);

            ThirdElement = new UITable(new string[][] {new string[] {
                "Name", "Gegner sind OP"},
                new string[] {"Rasse", "Winfestor"},
                new string[] {"Staerke", "9002"},
                new string[] {"Agilitaet", "9002"},
                new string[] {"Intelligenz", "9002"},
                new string[] {"Level", "9002"}}, 61);

            currentElement = new UIScene(new IRenderable[] { myElement, myOtherElement, ThirdElement },
                new int[][] { new int[] { 1, 1 }, new int[] { 1, 32 }, new int[] { 8, 1 } },
                new string[] { "Spieler 1", "Spieler 2", "OMG Gegner" }, 15, 63, ConsoleColor.Green, null);

            UIManager.setScene(currentElement);

            UIManager.Render();
            UIManager.DrawFrame();

            Console.ReadKey();

        }
    }
}
