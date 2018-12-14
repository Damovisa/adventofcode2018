using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new List<int>() { 3, 7 };
            
            //var s = new int[] {0,5,9,4,1,4};
            //var s = new int[] {1,0,1,2,4,5};
            //var s = new int[] {0,9,2,5,1,0};
            //var s = new int[] {7,6,8,0,7,1};
            //var s = new int[] {1,6,7,7,9,2};

            var s = new int[] {7,9,3,0,6,1};

            var e1 = 0;
            var e2 = 1;
            // loop until we find it
            var found = false;
            while (!found)
            {
                // new recipes added
                var sumdigits = (start[e1] + start[e2]).ToString().Select(c => int.Parse(c.ToString()));
                start.AddRange(sumdigits);

                // pick new recipe
                e1 = (e1 + start[e1] + 1) % start.Count;
                e2 = (e2 + start[e2] + 1) % start.Count;

                if (start.Count>5) {
                    var len = start.Count;
                    found = (
                        (start[len-6] == s[0]) &&
                        (start[len-5] == s[1]) &&
                        (start[len-4] == s[2]) &&
                        (start[len-3] == s[3]) &&
                        (start[len-2] == s[4]) &&
                        (start[len-1] == s[5]));
                }

            }

            Console.WriteLine($"There are {start.Count} elements");
            Console.WriteLine(start.Count-6);
            foreach (var num in start.Skip(start.Count-10))
            {
                Console.Write(num);
            }
            Console.WriteLine();
        }
    }
}
