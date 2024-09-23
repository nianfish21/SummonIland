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
            // 加载预制件
            TextAsset text = Resources.Load<TextAsset>(FinalPath);
            return text;

        }
        public GameObject GetPerfab(string fileName)
        {
            string projectPath = Application.dataPath;
            string FinalPath = perfabPath + fileName;
            Debug.Log("Project Path: " + FinalPath);
            // 加载预制件
            GameObject prefab = Resources.Load<GameObject>(FinalPath);
            if (prefab == null)
            {
                Debug.Log("没有成功加载perfab：" + fileName );
            }else
            {

                Debug.Log("成功加载perfab：" + fileName);
            }
            return GameObject .Instantiate( prefab);
        }

    }

}