using Pinyin4Net;
using System;
using System.Text;

namespace Common.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            Console.WriteLine(Hanzi2Pinyin("张宇"));
        }

        private static string Hanzi2Pinyin(string hanzi)
        {
            string py = Pinyin4Name.GetPinyin(hanzi);
            return py;
        }
    }
}
