using System;
using Classes;

namespace day13
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputLines = System.IO.File.ReadAllLines("input2.txt");
            var problem = new Problem(inputLines);

            Console.WriteLine($"1,1 is {problem.GetLocation(1,1)}");
            Console.WriteLine($"2,4 is {problem.GetLocation(2,4)}");
            Console.WriteLine($"3,9 is {problem.GetLocation(3,9)}");

            var collision = false;
            
            Cart[,] array = new [problem.MapCells.Count,] {}
        }
    }
}
