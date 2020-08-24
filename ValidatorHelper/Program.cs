using System;

namespace Common.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            bool ret = ValidatorHelper.HasEmail("yumaster@yeah.net");
            Console.WriteLine(ret);
            Console.ReadLine();
        }
    }
}
