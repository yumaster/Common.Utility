﻿using Pinyin4Net.data;
using Pinyin4Net.exception;
using System.Linq;

namespace Pinyin4Net
{
    /// <summary>
    /// 处理姓名专用
    /// </summary>
    public static class Pinyin4Name
    {
        #region // 获取单字拼音
        /// <summary>
        /// 获取姓的拼音，如果是复姓则由空格分隔
        /// </summary>
        /// <param name="firstName">要查询拼音的姓</param>
        /// <param name="format">输出拼音格式化参数</param>
        /// <returns>返回姓的拼音，若未找到姓，则返回null</returns>
        /// <exception cref="UnsupportedUnicodeException">当要获取拼音的字符不是汉字时抛出此异常</exception>
        public static string GetPinyin(string firstName, PinyinFormat format = PinyinFormat.None)
        {
            if (!firstName.All(PinyinUtil.IsHanzi))
            {
                // 不是汉字
                throw new UnsupportedUnicodeException("不支持的字符: 请输入汉字字符");
            }
            var pinyin = NameDB.Instance.GetPinyin(firstName);
            if (format == PinyinFormat.None)
            {
                return pinyin;
            }

            return string.Join(" ", pinyin.Split(' ').Select(item => PinyinUtil.Format(item, format)));
        }

        /// <summary>
        /// 获取姓的首字母，如果是复姓则由空格分隔首字母
        /// </summary>
        /// <param name="firstName">要查询拼音的姓</param>
        /// <returns>返回姓的拼音首字母，若未找到姓，则返回null</returns>
        /// <exception cref="UnsupportedUnicodeException">当要获取拼音的字符不是汉字时抛出此异常</exception>
        public static string GetFirstLetter(string firstName)
        {
            var pinyin = GetPinyin(firstName);
            if (pinyin == null)
            {
                return null;
            }
            return string.Join(" ", pinyin.Split(' ').Select(py => py[0]));
        }
        #endregion

        /// <summary>
        /// 根据拼音查询匹配的姓
        /// </summary>
        /// <param name="pinyin"></param>
        /// <param name="matchAll">是否全部匹配，为true时，匹配整个拼音，否则匹配开头字符，此参数用于告知传入的拼音是完整拼音还是仅仅是声母</param>
        /// <returns>匹配的姓数组</returns>
        public static string[] GetHanzi(string pinyin, bool matchAll)
        {
            return NameDB.Instance.GetHanzi(pinyin.ToLower(), matchAll);
        }
    }
}
