using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Threading;
using System.IO;

namespace Tool
{

    /// <summary>
    /// �˵���չ
    /// </summary>
    public class ChackUsingEditor : Editor
    {

        static string[] assetGUIDs;
        static string[] assetPaths;
        static string[] allAssetPaths;
        static Thread thread;

        [MenuItem("zpyTools/������Դ����", false)]
        static void FindAssetRefMenu()
        {
            if (Selection.assetGUIDs.Length == 0)
            {
                Debug.Log("����ѡ������һ��������ٻ��˲˵�");
                return;
            }

            assetGUIDs = Selection.assetGUIDs;

            assetPaths = new string[assetGUIDs.Length];

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
            }

            allAssetPaths = AssetDatabase.GetAllAssetPaths();

            thread = new Thread(new ThreadStart(FindAssetRef));
            thread.Start();
        }

        static void FindAssetRef()
        {
            Debug.Log(string.Format("��ʼ��������{0}����Դ��", string.Join(",", assetPaths)));
            List<string> logInfo = new List<string>();
            string path;
            string log;
            for (int i = 0; i < allAssetPaths.Length; i++)
            {
                path = allAssetPaths[i];
                if (path.EndsWith(".prefab") || path.EndsWith(".unity"))
                {
                    string content = File.ReadAllText(path);
                    if (content == null)
                    {
                        continue;
                    }

                    for (int j = 0; j < assetGUIDs.Length; j++)
                    {
                        if (content.IndexOf(assetGUIDs[j]) > 0)
                        {
                            log = string.Format("{0} ������ {1}", path, assetPaths[j]);
                            logInfo.Add(log);
                        }
                    }
                }
            }

            for (int i = 0; i < logInfo.Count; i++)
            {
                Debug.Log(logInfo[i]);
            }

            Debug.Log("ѡ���������������" + logInfo.Count);

            Debug.Log("�������");
        }
    }
}