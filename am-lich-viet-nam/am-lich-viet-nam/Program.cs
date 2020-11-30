using System;

namespace am_lich_viet_nam
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] r1 = AmLich.convertSolar2Lunar(14, 1, 1998, 7.0);
            int[] r2 = AmLich.convertSolar2Lunar(10, 9, 1993, 7.0);
            Console.WriteLine("Am Lich Viet Nam!");
        }
    }
}
