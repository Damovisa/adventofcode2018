using System;
using System.Collections;
using System.Collections.Generic;

namespace day1
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = "input.txt";
            var file = System.IO.File.ReadAllLines(filename);
            var ht = new System.Collections.Generic.HashSet<int>();
            int runningFrequency = 0;
            ht.Add(runningFrequency);
            var found = false;
            while (!found)
            {
                foreach (var l in file)
                {
                    var value = int.Parse(l.Substring(1));
                    switch (l.Substring(0, 1))
                    {
                        case "+":
                            runningFrequency += value;
                            break;
                        case "-":
                            runningFrequency -= value;
                            break;
                        default:
                            Console.WriteLine("WHAT.");
                            break;
                    }
                    if (ht.Contains(runningFrequency))
                    {
                        Console.WriteLine($"First frequency to be repeated is {runningFrequency}");
                        found = true;
                        break;
                    }
                    ht.Add(runningFrequency);
                }
            }
            Console.WriteLine($"Final frequency = {runningFrequency}");
        }
    }
}
