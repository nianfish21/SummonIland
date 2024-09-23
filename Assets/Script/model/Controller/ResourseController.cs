using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace model
{

    public class ResourseController : single <ResourseController>
{
        public const string perfabPath = "perfab/";
        public const string textPath = "ConfigMap/";

        public TextAsset GetText(string fileName)
        {
            string FinalPath = textPath + fileName;
            Debug.Log("Project Path: " + FinalPath);
            // ����Ԥ�Ƽ�
            TextAsset text = Resources.Load<TextAsset>(FinalPath);
            return text;

        }
        public GameObject GetPerfab(string fileName)
        {
            string projectPath = Application.dataPath;
            string FinalPath = perfabPath + fileName;
            Debug.Log("Project Path: " + FinalPath);
            // ����Ԥ�Ƽ�
            GameObject prefab = Resources.Load<GameObject>(FinalPath);
            if (prefab == null)
            {
                Debug.Log("û�гɹ�����perfab��" + fileName );
            }else
            {

                Debug.Log("�ɹ�����perfab��" + fileName);
            }
            return GameObject .Instantiate( prefab);
        }

    }

}