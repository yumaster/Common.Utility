using System;

namespace Common.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            string ret = QRCodeHelper.GetQRCode("这里面保存内容", "这是标题测试标题这是第二行标题测试标题");
            string base64Data = "data:image/jpg;base64," + ret;
            Console.WriteLine(base64Data);

            Console.ReadLine();
        }
    }
}
