using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{

    /// <summary>
    /// �㷨����
    /// </summary>
    public class AlgorithmTool
    {
        //sealed���ܷ���ⱻ�̳е���˼  �ڷ�����ʹ�ÿ��Ա��ⱻ������д

        /// <summary>
        /// ������y�ắ��
        /// </summary>
        /// <param name="_time">���ԴӸ������������õ�������</param>
        /// <param name="_gravity">����</param>
        /// <returns></returns>
        public static float ParabolaHeightFunc(float _time,float _maxHeightTime, float _gravity)
        {

            //��ʽ s=v*t  h=(1/2)gt^2   t^2=2h/g   
            //s����ˮƽ���룬v����ˮƽ�ٶȣ�t����ʱ��
            //h����߶ȣ�g��������ϵ��
            //������ h=vup*t - (1/2)gt^2
            //������ (h+ (1/2)gt^2)/t =vup
            //��ʼ���ٶ� float vup=(h + (1 / 2)*g * Mathf.Pow(t , 2)) / t;
            //һ��ʱ�����������߶� float h=0.5f * g * Mathf.Pow(t/2,2);


            float t = _time- _maxHeightTime;//����ʱ��
            float g = -_gravity;
            float h = 0.5f * g * Mathf.Pow(t, 2);
            return h;
        }


        //֪����߸߶Ⱥ�һ����Ծʱ�䷵�ص�ǰʱ��߶�
        public static float AbsSinHeightFunc(float _time, float _cycleTime, float _maxHeight)
        {
            float cycleNum = _time / _cycleTime ;
            float normalizeHeight = Mathf.Abs( Mathf.Sin(cycleNum * Mathf.PI));
            float h = normalizeHeight * _maxHeight;
            return h;
        }

    }

   
}