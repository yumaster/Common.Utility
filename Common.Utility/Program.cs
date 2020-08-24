using System;

namespace Common.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            string ret = QRCodeHelper.GetQRCode("这里面保存内容", "这是标题测试标题");
            Console.WriteLine(ret);

            Console.ReadLine();
        }
    }
}
