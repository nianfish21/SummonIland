using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Threading;



namespace Tool
{
    
    /// <summary>
    /// �༭������
    /// </summary>
    public class EditorTool
    {

        public void Main()
        {

        }


        /// <summary>
        /// ��ʾ��Ϣ
        /// </summary>
        public void ShowMessage()
        {
            //��ʾ�ո�
            GUILayout.Space(10);
            //��ʾstring
            GUILayout.Label("1.����");
        }


        bool ToggleValue;
        /// <summary>
        /// ��ѡ��
        /// </summary>
        public void Toggle()
        {

            ToggleValue = GUILayout.Toggle(ToggleValue, "��ѡ��");
            if (ToggleValue)
                Test();
        }

        /// <summary>
        /// ��������
        /// </summary>
        string TextFieldValue = "����";
        public void InputString()
        {
            //��ʾ����
            TextFieldValue = EditorGUILayout.TextField(TextFieldValue);
        }


        /// <summary>
        /// ��������
        /// </summary>
        int IntValue = 1;
        public void InputNumber()
        {
            //��ʾ����
            IntValue = EditorGUILayout.IntField(IntValue);
        }

        /// <summary>
        /// ��ť
        /// </summary>
        public void Button()
        {
            if (GUILayout.Button("��ť"))
            {
                Test();
            }
        }




        private AnimationClip ani;
        /// <summary>
        /// ʹ�ö������޸�����
        /// </summary>
        /// <param name="_obj"></param>
        public void AnimationClipSetPose(GameObject _obj)
        {
            //ʹ�õ�һ֡�޸�
            ani.SampleAnimation(_obj, 0.1f);
            Debug.Log(_obj.name + "���");

        }


        #region -------------ѡ��---------------


        /// <summary>
        /// ������Դ
        /// </summary>
        public T SelectAsset<T>() where T : UnityEngine.Object
        {
            T ShowObj = null;
            T InputObj = (T)EditorGUILayout.ObjectField(typeof(T).ToString(), ShowObj, typeof(T));
            return InputObj;
        }

        /// <summary>
        /// ��ȡ����asset�ļ�·��
        /// </summary>
        /// <returns></returns>
        public string[] GetAllAssetPath()
        {

            string[] allAssetPaths;
            allAssetPaths = AssetDatabase.GetAllAssetPaths();
            return allAssetPaths;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">gameobject֮���</typeparam>
        public void GetAllResources<T>() where T : UnityEngine.Object
        {
            //Selection.objectsӴi������ȡ��ǰѡ������
            UnityEngine.Object[] allGos = Resources.FindObjectsOfTypeAll(typeof(T));
            UnityEngine.Object[] previousSelection = Selection.objects;
            Selection.objects = allGos;
            //�����ڵĿɱ༭transform
            Transform[] selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            Selection.objects = previousSelection;
        }

        /// <summary>
        /// ���·��
        /// </summary>
        /// <returns></returns>
        public static string[] GetSelectPath()
        {

            string[] assetGUIDs;
            string[] assetPaths;

            assetGUIDs = Selection.assetGUIDs;

            assetPaths = new string[assetGUIDs.Length];
            if (Selection.assetGUIDs.Length == 0 || Selection.assetGUIDs.Length > 1)
            {
                Debug.Log("����ѡ������һ���ļ��У��ٻ��˲˵�");
                return assetPaths;
            }

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
            }


            return assetPaths;
        }

        #endregion



        #region -------------�޸�importer---------------

        /// <summary>
        /// �޸�ͼƬ����
        /// </summary>
        /// <param name="path"></param>
        public void SetImageFile(string path)
        {
            string type = path.PathGetFileType();
            if (type == ".jpg" || type == ".JPG" || type == ".png" || type == ".PNG" || type == ".tga" || type == ".TGA")
            {
                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                if (importer == null)
                    return;
                Debug.Log("�޸�ͼƬ:" + path);
                //�޸ĵ�ģʽ
                importer.filterMode = FilterMode.Point;
                importer.SaveAndReimport();
            }

        }

        /// <summary>
        /// �޸Ĳ����ļ�
        /// </summary>
        /// <param name="path"></param>
        public void SetMaterial(string path)
        {

            string type = path.PathGetFileType();
            if (type == ".mat")
            {
                var importer = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));//����
                if (importer == null)
                    return;
                if (importer.shader.name != "Universal Render Pipeline/Lit")
                    return;
                importer.SetFloat("_SpecularHighlights", 0f);
                Debug.Log("�޸Ĳ���:" + path);
            }
        }


        /// <summary>
        /// �޸�ѡ���ļ���Ԥ����
        /// </summary>
        /// <returns></returns>
        public void SetSelectFile()
        {

            var assetPaths = GetSelectPath();

            for (int i = 0; i < assetPaths.Length; i++)
            {
                //creat
                GameObject perfab = PrefabUtility.LoadPrefabContents(assetPaths[i]);
                {
                    //modify
                }
                //�����������Ŀ����Ҫж��
                PrefabUtility.UnloadPrefabContents(perfab);//ж��

            }
        }


        /// <summary>
        /// ����ģ�� ��fbxģ�͵Ĵ���
        /// </summary>
        /// <param name="path"></param>
        public void SetModelImport(string path)
        {

            ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(path);

            if (importer == null)
            {
                return;
            }
            //���ļ������Զ���������
            importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnTextureName, ModelImporterMaterialSearch.RecursiveUp);
            Debug.Log(importer.GetExternalObjectMap().Count);

            //��ȡģ�Ͳ�������
            var keys = new Dictionary<string, bool>();
            foreach (var render in importer.GetExternalObjectMap())
            {
                if (!keys.ContainsKey(render.Key.name.PathGetFileName_NoType()))
                {
                    keys.Add(render.Key.name, true);
                    Debug.Log(render.Key.name);
                }
            }

            //�Ѵ���Ĳ��ʻ����µĲ���
            importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
            if (keys.Count > 0 || importer.materialLocation != ModelImporterMaterialLocation.InPrefab)
            {
                var newMaterial = new Material("");//��дshader
                var kind = typeof(UnityEngine.Material);
                foreach (var it in keys)
                {
                    var id = new AssetImporter.SourceAssetIdentifier();
                    id.name = it.Key;
                    id.type = kind;
                    //�޸Ĳ���
                    importer.RemoveRemap(id);
                    importer.AddRemap(id, newMaterial);
                }
                importer.materialLocation = ModelImporterMaterialLocation.InPrefab;
                importer.SaveAndReimport();
            }



        }


        /// <summary>
        /// ����ͼƬ������������
        /// </summary>
        /// <param name="baseMaterialPath"></param>
        /// <param name="baseTextureGuid"></param>
        /// <param name="JpgPath"></param>
        private void CreateJpgMaterial(string baseMaterialPath, string baseTextureGuid, string JpgPath)
        {

            //ֱ���õ������ļ�
            string[] files = Directory.GetFiles(JpgPath, "*.*", SearchOption.AllDirectories);
            //string[] files = Directory.GetFiles(path2); ȡ��һ���ļ�ʱ��
            if (files.Length > 0)
            {
                Debug.Log("���ļ�:" + files.Length);
                int i = 0;
                foreach (string s in files)
                {
                    try
                    {
                        if (s.PathGetFileType() == ".jpg" || s.PathGetFileType() == ".JPG" || s.PathGetFileType() == ".tga") //��ͼƬ
                        {
                            string metaPath = s + ".meta";

                            string metacontent = File.ReadAllText(metaPath); //��ȡ����ͼƬ��guid
                            string guid = metacontent.Substring(metacontent.IndexOf("guid: ") + 6, 32);

                            string matcontent = File.ReadAllText(baseMaterialPath);
                            //Debug.Log(i + "λ�ã�1");
                            if (matcontent == null)
                            {
                                continue;
                            }
                            if (baseTextureGuid != guid) //��guid
                                while (matcontent.IndexOf(baseTextureGuid) > 0)
                                {

                                    matcontent = matcontent.Replace(baseTextureGuid, guid);
                                }
                            int MatNameNum = 0; //������
                            if (baseMaterialPath.PathGetFileName_NoType() != s.PathGetFileName_NoType())
                                while (matcontent.IndexOf(baseMaterialPath.PathGetFileName_NoType()) > 0)
                                {
                                    MatNameNum++;
                                    matcontent = matcontent.Replace(baseMaterialPath.PathGetFileName_NoType(), s.PathGetFileName_NoType());
                                }
                            if (MatNameNum > 1)
                            {
                                Debug.LogError("����ҵ�ԭʼ�������֣���������޸ĵĲ��֣�����ԭʼ�����������޸�" + s.PathBackLevel(1) + "\\" + s.PathGetFileName_NoType() + ".mat");
                            }
                            File.WriteAllText(s.PathBackLevel(1) + "\\" + s.PathGetFileName_NoType() + ".mat", matcontent);

                            Debug.Log(i + "λ�ã�" + s.PathBackLevel(1) + "\\" + s.PathGetFileName_NoType() + ".mat" + "\n" + guid);
                        }

                    }
                    catch
                    {
                        Debug.LogError("����" + s);
                    }
                    i++;
                }
            }
        }


        #endregion

        #region -------------�ļ�---------------


        /// <summary>
        /// ����Ԥ����
        /// </summary>
        /// <param name="path"></param>
        public void SavePerfab(GameObject orginalPerfab, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(orginalPerfab, path);
            Debug.Log("�����ַ" + path);

        }

        /// <summary>
        /// ����asset��Դ
        /// </summary>
        /// <param name="path"></param>
        public T loadGameobjectByAssetDatabase<T>(string path) where T : UnityEngine.Object
        {

            T perfab = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));//����
            return perfab;

        }




        #endregion



        #region ------------------�߳�-----------------

        private static Dictionary<string, Thread> e_threadTable = new Dictionary<string, Thread>();
        /// <summary>
        /// �����߳�
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="_hander"></param>
        public static void CreatThread(string eventType, Action _hander)
        {
            if (!e_threadTable.ContainsKey(eventType))
            {
                Thread thread = new Thread(new ThreadStart(_hander));
                thread.Start();
                e_threadTable.Add(eventType, thread);
                Debug.Log($"�̴߳���:{eventType}");
            }
            else
            {

            }

        }
        /// <summary>
        /// �ر��߳�
        /// </summary>
        /// <param name="eventType"></param>
        public static void CloseThread(string eventType)
        {
            if (e_threadTable.ContainsKey(eventType))
            {
                if (e_threadTable[eventType] != null)
                {

                    if (e_threadTable[eventType].IsAlive)
                        e_threadTable[eventType].Abort();
                    e_threadTable.Remove(eventType);
                    Debug.Log($"�̹߳ر�{eventType}");
                }
            }
        }


        #endregion

        //ʾ������
        public void Test()
        {

        }
    }


}