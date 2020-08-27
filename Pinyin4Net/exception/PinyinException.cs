using System;

namespace Pinyin4Net.exception
{
    /// <summary>
    /// 拼音异常类
    /// </summary>
    public class PinyinException : Exception
    {
        public PinyinException(string message) : base(message)
        {

        }
    }
}

