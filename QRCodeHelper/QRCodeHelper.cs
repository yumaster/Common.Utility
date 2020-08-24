using QRCoder;
using System;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;

namespace Common.Utility
{
    public class QRCodeHelper
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">二维码里面存的内容</param>
        /// <param name="title">二维码下面显示的文字</param>
        /// <returns></returns>
        public static string GetQRCode(string content, string title)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);//容错级别为Q 25%
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(7, Color.Black, Color.White, null, 15, 6, false);//获取生成的图片 7 为像素，白底黑码 
            using (var ms = new System.IO.MemoryStream())
            {
                string[] titRet = SpliceText(title, 9);//每9个字为一行
                Bitmap bmp = new Bitmap(qrCodeImage.Width, qrCodeImage.Height + 15 * titRet.Length);
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                g.DrawImage(qrCodeImage, 0, 0);
                if (!string.IsNullOrEmpty(title))
                {
                    Font font = new Font("GB2312", 14f, FontStyle.Bold, GraphicsUnit.Pixel);//设置字体，大小，粗细
                    for (int i = 0; i < titRet.Length; i++)
                    {
                        int strWidth = (int)g.MeasureString(titRet[i], font).Width;//文字长度
                        int wordStartX = (qrCodeImage.Width - strWidth) / 2;//总长度减去文字长度的一半，居中显示
                        int wordStartY = qrCodeImage.Height + 15 * i;//开始输入文本的Y坐标
                        g.DrawString(titRet[i], font, Brushes.Black, wordStartX, wordStartY);
                    }
                }
                // 合并位图
                qrCodeImage.Dispose();
                bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                g.Dispose();
                bmp.Dispose();

                string base64 = Convert.ToBase64String(ms.ToArray());
                return base64;
            }
        }
        /// <summary>
        /// 每N个字符进行分割 - ZhangYu 2020年8月24日11:30:38
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lineLength"></param>
        /// <returns></returns>
        public static string[] SpliceText(string text, int lineLength)
        {
            return Regex.Matches(text, ".{1," + lineLength + "}").Cast<Match>().Select(m => m.Value).ToArray();
        }
    }
}
