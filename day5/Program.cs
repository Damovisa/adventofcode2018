using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace day5
{
    class Program
    {
        static void Main2() {
            string[] replacements =
                {"aA","bB","cC","dD","eE","fF","gG","hH","iI","jJ","kK","lL","mM","nN","oO","pP","qQ","rR","sS","tT","uU","vV","wW","xX","yY","zZ",
                "Aa","Bb","Cc","Dd","Ee","Ff","Gg","Hh","Ii","Jj","Kk","Ll","Mm","Nn","Oo","Pp","Qq","Rr","Ss","Tt","Uu","Vv","Ww","Xx","Yy","Zz"};
            var input = new StringBuilder(System.IO.File.ReadAllText("input.txt"));

            bool cut = true;
            while (cut) {
                int length = input.Length;
                foreach (var r in replacements)
                    input.Replace(r,"");
                cut = input.Length < length;
                Console.WriteLine($"Cut from {length} to {input.Length}");
            }
        }

        static void Main(string[] args)
        {
            //Main2();
            //Environment.Exit(0);


            var filename = "input.txt";
            var bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(System.IO.File.ReadAllText(filename));
            var bytelist = new List<Byte>(bytes);
            var killedSome = true;
            while (killedSome)
            {
                Console.WriteLine(ToStr(bytelist.ToArray()));
                killedSome = false;
                var i=0;
                var lastNon = 0;
                for (i=1; i < bytelist.Count; i++)
                {
                    var b1 = bytelist[i-1];
                    var b2 = bytelist[i];
                    if (b1 - b2 == 32 || b1 - b2 == -32)
                    {
                        bytelist.RemoveAt(i-1);
                        bytelist.RemoveAt(i-1);
                        i = lastNon;    // backtrack - they're dead
                        lastNon = Math.Max(lastNon-2,0);
                        killedSome = true;
                        Console.Write($"{ToStr(b1)}{ToStr(b2)}|");
                    } else {
                        lastNon = i-1;
                    }
                }
                //if (i == bytes.Length) newbytes.Add(bytes[bytes.Length-1]);
                Console.WriteLine();
            }
            Console.WriteLine("Final result: " + ToStr(bytelist.ToArray()));

            Console.WriteLine();
            Console.WriteLine($"{bytelist.Count} units remain");
        }

        private static string ToStr(byte byteVal) {
            return (System.Text.ASCIIEncoding.ASCII.GetString(new byte[] {byteVal}));
        }
        private static string ToStr(byte[] byteVal) {
            return (System.Text.ASCIIEncoding.ASCII.GetString(byteVal));
        }
    }
}
