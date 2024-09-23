using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool {


    public class ScreenHelper
    {
        /// <summary>
        /// 相机旋转都为0时使用，获得鼠标点击的位置上z轴距离相机正面为z单位的第一个物体
        /// </summary>
        /// <param name="z">2d透视检测的z轴坐标</param>
        /// <returns></returns>
        public static RaycastHit2D Get2DRayFormMouse(float z)
        {
            var ray = GetMouseWorldPosition(z);
            var hit = Physics2D.Raycast(new Vector3(ray.x, ray.y, 0), Vector2.zero);
            Debug.DrawRay(new Vector3(ray.x, ray.y, 0), Vector2.up);
            if (Camera.main.transform.rotation == new Quaternion(0, 0, 0, 0))
            { Debug.Log("camaer:rotation error"); }
            return hit;
        }

        public static Vector3 GetMouseWorldPosition2D()
        {
            var onScreenPosition = Input.mousePosition;
            var positonOut = Camera.main.ScreenToWorldPoint(new Vector3(onScreenPosition.x, onScreenPosition.y, 0));
            return new Vector3(positonOut.x, positonOut.y, 0);
        }
        public static Vector3 GetMouseWorldPosition(float z)
        {
            var onScreenPosition = Input.mousePosition;
            var positonOut = Camera.main.ScreenToWorldPoint(new Vector3(onScreenPosition.x, onScreenPosition.y, z));
            return positonOut;
        }

    }
}

