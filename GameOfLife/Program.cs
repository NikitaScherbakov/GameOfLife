using System;
using System.Linq;
using System.Threading;

namespace GameOfLife
{
    public class Program
    {
        static void Main(string[] args)
        {
            var life = new Life { { 1, 1 }, { 2, 2 }, { 3, 3 }, { 1, 2 }, { 1, 3 } };

            while (life.Any())
            {
                Console.SetCursorPosition(0, 0);
                life.Next();
                Console.WriteLine(life);
                Thread.Sleep(500);
            }

            Console.WriteLine(life.Any() ? "* Stagnation!" : "* Extinction!");
        }
    }
}
