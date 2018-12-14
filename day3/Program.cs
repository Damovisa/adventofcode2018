using System;
using System.Collections.Generic;

namespace day3
{
    class Program
    {
        /*
        #1 @ 1,3: 4x4
        #2 @ 3,1: 4x4
        #3 @ 5,5: 2x2
         */
        static void Main(string[] args)
        {
            var lines = System.IO.File.ReadAllLines("input.txt");
            const int fabricSize = 1000;
            string[,] fabric = new string[fabricSize, fabricSize];
            var items = new HashSet<string>();

            foreach (var line in lines)
            {
                // find square
                var segments = line.Split(' ');
                var id = segments[0];
                items.Add(id);
                var coords = segments[2].Split(',');
                var x = int.Parse(coords[0]);
                var y = int.Parse(coords[1].Substring(0, coords[1].Length - 1));
                var size = segments[3].Split('x');
                var w = int.Parse(size[0]);
                var h = int.Parse(size[1]);

                for (var px = x; px < x + w; px++)
                {
                    for (var py = y; py < y + h; py++)
                    {
                        var current = fabric[px, py];
                        if (string.IsNullOrEmpty(current))
                        {
                            fabric[px, py] = id;
                        }
                        else
                        {
                            items.Remove(id);
                            items.Remove(current);
                        }
                    }
                }
            }
            // ok, all squares have been marked out
            //  find remaining items
            Console.WriteLine($"Claims that don't overlap: {items.Count}");
            foreach (var claim in items) {
                Console.WriteLine(claim);
            }
        }
    }
}
