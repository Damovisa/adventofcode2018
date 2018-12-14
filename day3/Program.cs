using System;

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
            ushort[,] fabric = new ushort[fabricSize,fabricSize];

            foreach (var line in lines) {
                // find square
                var segments = line.Split(' ');
                var coords = segments[2].Split(',');
                var x = int.Parse(coords[0]);
                var y = int.Parse(coords[1].Substring(0,coords[1].Length-1));
                var size = segments[3].Split('x');
                var w = int.Parse(size[0]);
                var h = int.Parse(size[1]);

                for (var px=x;px<x+w;px++) {
                    for (var py=y;py<y+h;py++) {
                        fabric[px,py]++;    // could just mark it off, but may as well add
                    }
                }
            }
            // ok, all squares have been marked out
            //  find elements > 1
            var counter = 0;
            for (int i=0;i<fabricSize;i++) {
                for (int j=0;j<fabricSize;j++) {
                    counter = (fabric[i,j]>1) ? counter+1 : counter;
                }
            }
            Console.WriteLine($"Total of {counter} squares overlapped");
        }
    }
}
