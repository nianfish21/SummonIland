using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{

    /// <summary>
    /// 算法工具
    /// </summary>
    public class AlgorithmTool
    {
        //sealed是密封避免被继承的意思  在方法上使用可以避免被子类重写

        /// <summary>
        /// 抛物线y轴函数
        /// </summary>
        /// <param name="_time">可以从负数到正数来得到抛物线</param>
        /// <param name="_gravity">重力</param>
        /// <returns></returns>
        public static float ParabolaHeightFunc(float _time,float _maxHeightTime, float _gravity)
        {

            //公式 s=v*t  h=(1/2)gt^2   t^2=2h/g   
            //s代表水平距离，v代表水平速度，t代表时间
            //h代表高度，g代表重力系数
            //抛物线 h=vup*t - (1/2)gt^2
            //抛物线 (h+ (1/2)gt^2)/t =vup
            //初始升速度 float vup=(h + (1 / 2)*g * Mathf.Pow(t , 2)) / t;
            //一半时间的自由落体高度 float h=0.5f * g * Mathf.Pow(t/2,2);


            float t = _time- _maxHeightTime;//花费时间
            float g = -_gravity;
            float h = 0.5f * g * Mathf.Pow(t, 2);
            return h;
        }


        //知道最高高度和一次跳跃时间返回当前时间高度
        public static float AbsSinHeightFunc(float _time, float _cycleTime, float _maxHeight)
        {
            float cycleNum = _time / _cycleTime ;
            float normalizeHeight = Mathf.Abs( Mathf.Sin(cycleNum * Mathf.PI));
            float h = normalizeHeight * _maxHeight;
            return h;
        }

    }

   
}