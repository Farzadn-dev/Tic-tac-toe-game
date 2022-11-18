using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TicToc
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Select Mode --> 1[PvP], 2[PvCPU] ");
                short num = Convert.ToInt16(Console.ReadKey().KeyChar.ToString());
                if (num == 1)
                    TicTocGame.Play();
                else if (num == 2)
                    TicTocGame.AI_Play();
                Console.Clear();
            }

        }
    }
}
