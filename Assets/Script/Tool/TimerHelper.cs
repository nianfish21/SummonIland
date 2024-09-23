using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tool
{

    /// <summary>
    /// 时间工具
    /// </summary>
    public class TimeHelper
    {
        private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        /// <summary>
		/// 客户端时间
		/// </summary>
		/// <returns></returns>
		public static long ClientNow()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000;
        }

        public static long ClientNowSeconds()
        {
            return (DateTime.UtcNow.Ticks - epoch) / 10000000;
        }

        public static long Now()
        {
            return ClientNow();
        }


        /// <summary>
        /// 从1970-01-01 08:00:00 的时间
        /// </summary>
        /// <param name="MillisecondsClock"> 时间刻需要毫秒 </param>
        /// <returns></returns>
        public static DateTime clock(long MillisecondsClock)
        {

            bool outTime;
            //返回为时间格式 AddMilliseconds为毫秒 
            DateTime tEnd = Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString("1970-01-01 08:00:00")).AddMilliseconds(MillisecondsClock).ToString());
            return tEnd;
            //tEnd = tEnd.AddDays(7);
            //DateTime.Parse("2020-07-14 14:23:40");
            //var a = tEnd - DateTime.Now;
        }
    }
}