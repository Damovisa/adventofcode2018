using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace day5
{
    class Program
    {
        public static string[] letters = {"a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p","q","r","s","t","u","v","w","x","y","z"};

        static void Main(string[] args)
        {
            var filename = "input.txt";
            var input = System.IO.File.ReadAllText(filename);

            var minResult = input.Length;
            foreach (var letter in letters)
            {
                var newinput = new StringBuilder(input);
                newinput.Replace(letter, "");
                newinput.Replace(letter.ToUpper(), "");

                var bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(newinput.ToString());
                var bytelist = new List<Byte>(bytes);
                //Console.WriteLine(ToStr(bytelist.ToArray()));
                var i = 0;
                var lastNon = 0;

                for (i = 1; i < bytelist.Count; i++)
                {
                    var b1 = bytelist[i - 1];
                    var b2 = bytelist[i];
                    if (b1 - b2 == 32 || b1 - b2 == -32)
                    {
                        bytelist.RemoveAt(i - 1);
                        bytelist.RemoveAt(i - 1);
                        i = lastNon;    // backtrack - they're dead
                        lastNon = Math.Max(lastNon - 2, 0);
                        //Console.Write($"{ToStr(b1)}{ToStr(b2)}|");
                    }
                    else
                    {
                        lastNon = i - 1;
                    }
                }

                Console.WriteLine($"Removing {letter}: {bytelist.Count}");
                if (bytelist.Count < minResult) {
                    Console.WriteLine("  -- new minimum");
                    minResult = bytelist.Count;
                }
            }
        }

        private static string ToStr(byte byteVal)
        {
            return (System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { byteVal }));
        }
        private static string ToStr(byte[] byteVal)
        {
            return (System.Text.ASCIIEncoding.ASCII.GetString(byteVal));
        }
    }
}
