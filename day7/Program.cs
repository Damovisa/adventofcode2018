using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace day7
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = System.IO.File.ReadAllText("input.txt");
            var sb = new StringBuilder(input);
            sb.Replace("Step ", "").Replace(" must be finished before step ", "").Replace(" can begin.", "");   // clean

            var lines = sb.ToString().Split(Environment.NewLine);

            var steps = new Dictionary<string, List<string>>();

            var totalseconds = 0;

            // build
            foreach (var line in lines)
            {
                var name = line.Substring(1);
                var prereq = line.Substring(0, 1);
                if (steps.ContainsKey(name))
                {
                    steps[name].Add(prereq);
                }
                else
                {
                    steps.Add(name, new List<string>() { prereq });
                }
                if (!steps.ContainsKey(prereq))
                {
                    steps.Add(prereq, new List<string>());
                }
            }

            (string, int)[] workers = new[] { ("", 1000000), ("", 1000000), ("", 1000000), ("", 1000000), ("", 1000000) };

            var order = new StringBuilder();
            var available = new List<string>();
            while (steps.Count > 0)
            {
                // assignment step
                var keys = steps.Select(s => s.Key).ToArray();
                foreach (var key in keys)
                {
                    if (steps[key].Count == 0)
                    {
                        available.Add(key);
                        steps.Remove(key);
                    }
                }
                available.Sort();
                int taken = 0;
                foreach (var a in available)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (workers[i].Item1 == "")
                        {
                            workers[i].Item1 = a;
                            workers[i].Item2 = 60 + (a[0] - 64);
                            taken++;
                            break;
                        }
                    }
                }
                for (int i = 0; i < taken; i++)
                {
                    available.RemoveAt(0);
                }

                // working step
                var finishedIndex = -1;
                while (workers.All(w => w.Item2 > 0))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        workers[i].Item2 = workers[i].Item2 - 1;
                        if (workers[i].Item2 == 0)
                            finishedIndex = i;
                    }
                    totalseconds++;
                }
                var finished = workers.First(w => w.Item2 == 0).Item1;
                workers[finishedIndex].Item1 = "";
                workers[finishedIndex].Item2 = 1000000;
                order.Append(finished);
                foreach (var key in steps.Keys)
                {
                    steps[key].Remove(finished);
                }
                Console.WriteLine(order.ToString());
            }
            Console.WriteLine($"Total seconds: {totalseconds}");
        }
    }
}