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
                    // �ڵ�һ�η���ʱ�����ҳ������Ƿ��Ѵ��ڸ����͵�ʵ��
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        // ��������в����ڣ�����һ���µ�ʵ��
                        GameObject singletonObject = new GameObject("singlemono");
                        instance = singletonObject.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake()
        {
            // ȷ��ʵ����Awakeʱ��������
            DontDestroyOnLoad(gameObject);
        }
    }
}