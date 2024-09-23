using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace model
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    // 在第一次访问时，查找场景中是否已存在该类型的实例
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        // 如果场景中不存在，创建一个新的实例
                        GameObject singletonObject = new GameObject("singlemono");
                        instance = singletonObject.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            // 确保实例在Awake时不被销毁
            DontDestroyOnLoad(gameObject);
        }
    }
}