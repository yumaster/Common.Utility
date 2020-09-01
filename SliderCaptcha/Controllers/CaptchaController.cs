using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SliderCaptcha.Controllers
{
    /// <summary>
    /// 滑块服务器端验证方法
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CaptchaController : ControllerBase
    {
        /// <summary>
        /// 服务器端滑块验证方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public bool Post([FromBody]List<int> datas)
        {
            var sum = datas.Sum();
            var avg = sum * 1.0 / datas.Count;
            var stddev = datas.Select(v => Math.Pow(v - avg, 2)).Sum() / datas.Count;
            return stddev != 0;
        }
    }
}