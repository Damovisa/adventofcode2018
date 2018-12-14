using System;
using System.Collections.Generic;
using System.Linq;

namespace day14
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = new List<int>() {7,6,8,0,7,1};
            var input = new List<int>() { 3, 7 };

            var e1 = 0;
            var e2 = 1;
            // loop until we hit the required number
            while (input.Count<=768071 + 10)
            {
                // new recipes added
                var sumdigits = (input[e1] + input[e2]).ToString().Select(c => int.Parse(c.ToString()));
                input.AddRange(sumdigits);

                // pick new recipe
                e1 = (e1 + input[e1] + 1) % input.Count;
                e2 = (e2 + input[e2] + 1) % input.Count;
            }
            Console.WriteLine(input.Count);
            foreach (var num in input)
            {
                Console.Write(num);
            }
            Console.WriteLine();
        }
    }
}
