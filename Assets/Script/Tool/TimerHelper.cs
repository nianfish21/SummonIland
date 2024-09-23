using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Tool
{

    /// <summary>
    /// ʱ�乤��
    /// </summary>
    public class TimeHelper
    {
        private static readonly long epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        /// <summary>
		/// �ͻ���ʱ��
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
        /// ��1970-01-01 08:00:00 ��ʱ��
        /// </summary>
        /// <param name="MillisecondsClock"> ʱ�����Ҫ���� </param>
        /// <returns></returns>
        public static DateTime clock(long MillisecondsClock)
        {

            bool outTime;
            //����Ϊʱ���ʽ AddMillisecondsΪ���� 
            DateTime tEnd = Convert.ToDateTime(DateTime.Parse(DateTime.Now.ToString("1970-01-01 08:00:00")).AddMilliseconds(MillisecondsClock).ToString());
            return tEnd;
            //tEnd = tEnd.AddDays(7);
            //DateTime.Parse("2020-07-14 14:23:40");
            //var a = tEnd - DateTime.Now;
        }
    }
}