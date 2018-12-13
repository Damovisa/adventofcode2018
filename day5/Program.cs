using System;
using System.IO;
using System.Collections.Generic;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            var filename = "inputtest.txt";
            var bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(System.IO.File.ReadAllText(filename));
            var newbytes = new List<Byte>();
            var killedSome = true;
            while (killedSome)
            {
                newbytes.Clear();
                killedSome = false;
                var i=0;
                for (i=0; i < bytes.Length-2; i++)
                {
                    var b1 = bytes[i];
                    var b2 = bytes[i + 1];
                    if (b1 - b2 == 32 || b1 - b2 == -32)
                    {
                        i++;    // skip - they're dead
                        killedSome = true;
                        Console.Write($"{ToStr(b1)}{ToStr(b2)}|");
                    }
                    else
                    {
                        newbytes.Add(b1);
                    }
                }
                bytes = newbytes.ToArray();
                Console.WriteLine($"Pass: {ToStr(newbytes.ToArray())}");
            }
            Console.WriteLine();
            Console.WriteLine("Final result: " + ToStr(newbytes.ToArray()));
        }

        private static string ToStr(byte byteVal) {
            return (System.Text.ASCIIEncoding.ASCII.GetString(new byte[] {byteVal}));
        }
        private static string ToStr(byte[] byteVal) {
            return (System.Text.ASCIIEncoding.ASCII.GetString(byteVal));
        }
    }
}
