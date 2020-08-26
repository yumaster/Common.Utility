using System;

namespace Common.Utility
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.WriteLine("=======================");

            bool oneRet = IsInTimeRange(DateTime.Now, "1:00", "8:00");
            Console.WriteLine("后夜- - - -当前时间：{0},范围：1:00-8:00,结果：{1}", DateTime.Now, oneRet);

            bool twoRet = IsInTimeRange(DateTime.Now, "8:00", "17:00");
            Console.WriteLine("白班- - - -当前时间：{0},范围：8:00-17:00,结果：{1}", DateTime.Now, twoRet);

            bool threeRet = IsInTimeRange(DateTime.Now, "17:00", "1:00");
            Console.WriteLine("前夜- - - -当前时间：{0},范围：17:00-1:00,结果：{1}", DateTime.Now, threeRet);

            Console.ReadKey();
        }
        /// <summary>
        /// 判断时间是否在某个时间段内
        /// </summary>
        /// <param name="nowTime"></param>
        /// <param name="beginHm"></param>
        /// <param name="endHm"></param>
        /// <returns></returns>
        public static bool IsInTimeRange(DateTime nowTime, string beginHm, string endHm)
        {
            DateTime start = DateTime.Parse(beginHm);
            DateTime end = DateTime.Parse(endHm);
            if (start.CompareTo(end) == 1 || start.Equals(end))
            {
                if (GetTimeSpan(nowTime.ToString()) >= GetTimeSpan(start.ToString()) || GetTimeSpan(nowTime.ToString()) <= GetTimeSpan(end.ToString()))
                {
                    return true;
                }
            }
            else
            {
                if (GetTimeSpan(nowTime.ToString()) >= GetTimeSpan(start.ToString()) && GetTimeSpan(nowTime.ToString()) <= GetTimeSpan(end.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取类似java gettime() 格式的时间
        /// </summary>
        /// <param name="time">要转换的时间</param>
        /// <returns>long类型的时间秒数</returns>
        public static long GetTimeSpan(string time)
        {
            if (string.IsNullOrEmpty(time))
            {
                TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);//ToUniversalTime()转换为标准时区的时间,去掉的话直接就用北京时间
                return (long)ts.TotalMilliseconds; //精确到毫秒
            }
            else
            {
                DateTime dt = Convert.ToDateTime(time);
                TimeSpan ts = dt.ToUniversalTime() - new DateTime(1970, 1, 1);//ToUniversalTime()转换为标准时区的时间,去掉的话直接就用北京时间
                return (long)ts.TotalMilliseconds; //精确到毫秒
            }
        }
    }
}