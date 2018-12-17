using System;
using System.Collections.Generic;
using System.Linq;

namespace day16
{
    class Program
    {
        public static int[] reg;
        public static OpCode[] ops;
        static void Main(string[] args)
        {
            reg = new int[3];
            ops = (OpCode[])Enum.GetValues(typeof(OpCode));

            // (in, instruction, out)
            var samples = new List<(int[] In, int[] Instr, int[] Out)>();

            // read all the samples
            var lines = System.IO.File.ReadAllLines("input-part1.txt");
            for (int i=0;i<lines.Count();i+=4) {
                var l = lines[i].Substring(9, lines[i].Length-10).Replace(" ","");   // strip front and back and remove spaces
                var inp = l.Split(',').Select(v => int.Parse(v)).ToArray(); // in

                var ins = lines[i+1].Split(' ').Select(v => int.Parse(v)).ToArray();    // instruction

                l = lines[i+2].Substring(9, lines[i+2].Length-10).Replace(" ","");  // strip front and back and remove spaces
                var outp = l.Split(',').Select(v => int.Parse(v)).ToArray();    // out

                samples.Add((inp, ins, outp));
            }

            // test all the samples

            var numWithThree = 0;
            foreach (var sample in samples) {
                // run a check
                var inp = (int[])sample.In.Clone(); // for debugging
                var target = sample.Out;

                var instr = sample.Instr;
                var possibles = new HashSet<OpCode>();
                foreach (var op in ops)
                {
                    reg = (int[])sample.In.Clone();
                    DoOperation(op, instr[1], instr[2], instr[3]);
                    if (CheckExpected(target))
                        possibles.Add(op);
                }
                foreach (var op in possibles) {
                    Console.WriteLine($"{op} is possible");
                }
                if (possibles.Count >= 3)
                    numWithThree++;
            }
            Console.WriteLine("*********");
            Console.WriteLine($"There are {numWithThree} samples that behave like 3 or more opcodes:");
        }

        public static void DoOperation(OpCode code, int A, int B, int C)
        {
            int newVal = reg[C];
            switch (code)
            {
                case (OpCode.addr):
                    newVal = reg[A] + reg[B];
                    break;
                case (OpCode.addi):
                    newVal = reg[A] + B;
                    break;
                case (OpCode.mulr):
                    newVal = reg[A] * reg[B];
                    break;
                case (OpCode.muli):
                    newVal = reg[A] * B;
                    break;
                case (OpCode.banr):
                    newVal = reg[A] & reg[B];
                    break;
                case (OpCode.bani):
                    newVal = reg[A] & B;
                    break;
                case (OpCode.borr):
                    newVal = reg[A] | reg[B];
                    break;
                case (OpCode.bori):
                    newVal = reg[A] | B;
                    break;
                case (OpCode.setr):
                    newVal = reg[A];
                    break;
                case (OpCode.seti):
                    newVal = A;
                    break;
                case (OpCode.gtir):
                    newVal = A > reg[B] ? 1 : 0;
                    break;
                case (OpCode.gtri):
                    newVal = reg[A] > B ? 1 : 0;
                    break;
                case (OpCode.gtrr):
                    newVal = reg[A] > reg[B] ? 1 : 0;
                    break;
                case (OpCode.eqir):
                    newVal = A.Equals(reg[B]) ? 1 : 0;
                    break;
                case (OpCode.eqri):
                    newVal = reg[A].Equals(B) ? 1 : 0;
                    break;
                case (OpCode.eqrr):
                    newVal = reg[A].Equals(reg[B]) ? 1 : 0;
                    break;

                default:
                    throw new ArgumentException("How did you choose an OpCode that doesn't exist?!");
            }
            reg[C] = newVal;
        }
        public static bool CheckExpected(int[] expected)
        {
            return
                expected[0] == reg[0] &&
                expected[1] == reg[1] &&
                expected[2] == reg[2] &&
                expected[3] == reg[3];
        }
    }

    public enum OpCode
    {
        addr,
        addi,
        mulr,
        muli,
        banr,
        bani,
        borr,
        bori,
        setr,
        seti,
        gtir,
        gtri,
        gtrr,
        eqir,
        eqri,
        eqrr
    }
}

public static class EnumUtil {
    public static IEnumerable<T> GetValues<T>() {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}