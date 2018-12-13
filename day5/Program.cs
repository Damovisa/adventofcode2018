using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace day5
{
    class Program
    {
        public static string[] pairs = {"aA","bB","cC","dD","eE","fF","gG","hH","iI","jJ","kK","lL","mM","nN","oO","pP","qQ","rR","sS","tT","uU","vV","wW","xX","yY","zZ",
                "Aa","Bb","Cc","Dd","Ee","Ff","Gg","Hh","Ii","Jj","Kk","Ll","Mm","Nn","Oo","Pp","Qq","Rr","Ss","Tt","Uu","Vv","Ww","Xx","Yy","Zz"};

        static void Main(string[] args)
        {
            var filename = "input.txt";
            var bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(System.IO.File.ReadAllText(filename));
            var bytelist = new List<Byte>(bytes);
            Console.WriteLine(ToStr(bytelist.ToArray()));
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
                    Console.Write($"{ToStr(b1)}{ToStr(b2)}|");
                }
                else
                {
                    lastNon = i - 1;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Final result: " + ToStr(bytelist.ToArray()));

            Console.WriteLine();
            Console.WriteLine($"{bytelist.Count} units remain");
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
