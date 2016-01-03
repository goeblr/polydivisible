using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace polydivisible
{
    class Program
    {
        static void Main(string[] args)
        {
            byte bStart = 3;
            if (args.Length >= 1)
            {
                byte.TryParse(args[0], out bStart);
            }

            for (byte b = bStart; b <= 255; b++)
            {
                tryBase(b);
            }
            
            if (System.Diagnostics.Debugger.IsAttached)
            {
                Console.WriteLine("Press enter to close...");
                Console.ReadLine();
            }
        }

        static void tryBase(byte b)
        {
            Console.Out.WriteLine("Base " + b + " (" + DateTime.Now.ToString() + ")");
            Parallel.For(1, b,
                   index =>
                   {
                       byte dInner = (byte)index;
                       PPDN p = new PPDN(b);
                       tryDigit(p, dInner, (byte)(b - 1));
                       Console.Out.WriteLine("    " + dInner);
                   });
            //for (byte d1 = 1; d1 < b; d1++)
            //{
            //    PPDN p = new PPDN(b);
            //    tryDigit(p, d1, (byte)(b - 1));
            //}
        }

        static void tryDigit(PPDN p, byte digit, byte levelsLeft)
        {
            bool worked = p.addDigit(digit);
            if (worked)
            {
                if (levelsLeft - 1 > 0)
                {
                    List<byte> digitsToTry = new List<byte>(p.digitsLeft());
                    foreach (byte nextDigit in digitsToTry)
                    {
                        tryDigit(p, nextDigit, (byte)(levelsLeft - 1));
                    }
                }
                else
                {
                    Console.Out.WriteLine("  FOUND! " + p);
                }
                p.removeLastDigit();
            }
        }
    }
}
