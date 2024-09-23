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
    /// 编辑器工具
    /// </summary>
    public class EditorTool
    {

        public void Main()
        {

        }


        /// <summary>
        /// 显示信息
        /// </summary>
        public void ShowMessage()
        {
            //显示空格
            GUILayout.Space(10);
            //显示string
            GUILayout.Label("1.文字");
        }


        bool ToggleValue;
        /// <summary>
        /// 复选框
        /// </summary>
        public void Toggle()
        {

            ToggleValue = GUILayout.Toggle(ToggleValue, "复选框");
            if (ToggleValue)
                Test();
        }

        /// <summary>
        /// 输入文字
        /// </summary>
        string TextFieldValue = "文字";
        public void InputString()
        {
            //显示文字
            TextFieldValue = EditorGUILayout.TextField(TextFieldValue);
        }


        /// <summary>
        /// 输入数字
        /// </summary>
        int IntValue = 1;
        public void InputNumber()
        {
            //显示文字
            IntValue = EditorGUILayout.IntField(IntValue);
        }

        /// <summary>
        /// 按钮
        /// </summary>
        public void Button()
        {
            if (GUILayout.Button("按钮"))
            {
                Test();
            }
        }




        private AnimationClip ani;
        /// <summary>
        /// 使用动画来修改物体
        /// </summary>
        /// <param name="_obj"></param>
        public void AnimationClipSetPose(GameObject _obj)
        {
            //使用第一帧修改
            ani.SampleAnimation(_obj, 0.1f);
            Debug.Log(_obj.name + "完成");

        }


        #region -------------选中---------------


        /// <summary>
        /// 拖入资源
        /// </summary>
        public T SelectAsset<T>() where T : UnityEngine.Object
        {
            T ShowObj = null;
            T InputObj = (T)EditorGUILayout.ObjectField(typeof(T).ToString(), ShowObj, typeof(T));
            return InputObj;
        }

        /// <summary>
        /// 获取所有asset文件路径
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
        /// <typeparam name="T">gameobject之类的</typeparam>
        public void GetAllResources<T>() where T : UnityEngine.Object
        {
            //Selection.objects哟i你来获取当前选择物体
            UnityEngine.Object[] allGos = Resources.FindObjectsOfTypeAll(typeof(T));
            UnityEngine.Object[] previousSelection = Selection.objects;
            Selection.objects = allGos;
            //场景内的可编辑transform
            Transform[] selectedTransforms = Selection.GetTransforms(SelectionMode.Editable | SelectionMode.ExcludePrefab);
            Selection.objects = previousSelection;
        }

        /// <summary>
        /// 获得路径
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
                Debug.Log("请先选择任意一个文件夹，再击此菜单");
                return assetPaths;
            }

            for (int i = 0; i < assetGUIDs.Length; i++)
            {
                assetPaths[i] = AssetDatabase.GUIDToAssetPath(assetGUIDs[i]);
            }


            return assetPaths;
        }

        #endregion



        #region -------------修改importer---------------

        /// <summary>
        /// 修改图片内容
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
                Debug.Log("修改图片:" + path);
                //修改点模式
                importer.filterMode = FilterMode.Point;
                importer.SaveAndReimport();
            }

        }

        /// <summary>
        /// 修改材质文件
        /// </summary>
        /// <param name="path"></param>
        public void SetMaterial(string path)
        {

            string type = path.PathGetFileType();
            if (type == ".mat")
            {
                var importer = (Material)AssetDatabase.LoadAssetAtPath(path, typeof(Material));//加载
                if (importer == null)
                    return;
                if (importer.shader.name != "Universal Render Pipeline/Lit")
                    return;
                importer.SetFloat("_SpecularHighlights", 0f);
                Debug.Log("修改材质:" + path);
            }
        }


        /// <summary>
        /// 修改选中文件的预制体
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
                //如果不是在项目中需要卸载
                PrefabUtility.UnloadPrefabContents(perfab);//卸载

            }
        }


        /// <summary>
        /// 导入模型 和fbx模型的处理
        /// </summary>
        /// <param name="path"></param>
        public void SetModelImport(string path)
        {

            ModelImporter importer = (ModelImporter)AssetImporter.GetAtPath(path);

            if (importer == null)
            {
                return;
            }
            //从文件夹下自动搜索材质
            importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnTextureName, ModelImporterMaterialSearch.RecursiveUp);
            Debug.Log(importer.GetExternalObjectMap().Count);

            //获取模型材质名字
            var keys = new Dictionary<string, bool>();
            foreach (var render in importer.GetExternalObjectMap())
            {
                if (!keys.ContainsKey(render.Key.name.PathGetFileName_NoType()))
                {
                    keys.Add(render.Key.name, true);
                    Debug.Log(render.Key.name);
                }
            }

            //把储存的材质换成新的材质
            importer.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
            if (keys.Count > 0 || importer.materialLocation != ModelImporterMaterialLocation.InPrefab)
            {
                var newMaterial = new Material("");//填写shader
                var kind = typeof(UnityEngine.Material);
                foreach (var it in keys)
                {
                    var id = new AssetImporter.SourceAssetIdentifier();
                    id.name = it.Key;
                    id.type = kind;
                    //修改材质
                    importer.RemoveRemap(id);
                    importer.AddRemap(id, newMaterial);
                }
                importer.materialLocation = ModelImporterMaterialLocation.InPrefab;
                importer.SaveAndReimport();
            }



        }


        /// <summary>
        /// 根据图片批量创建材质
        /// </summary>
        /// <param name="baseMaterialPath"></param>
        /// <param name="baseTextureGuid"></param>
        /// <param name="JpgPath"></param>
        private void CreateJpgMaterial(string baseMaterialPath, string baseTextureGuid, string JpgPath)
        {

            //直接拿到左右文件
            string[] files = Directory.GetFiles(JpgPath, "*.*", SearchOption.AllDirectories);
            //string[] files = Directory.GetFiles(path2); 取到一个文件时候
            if (files.Length > 0)
            {
                Debug.Log("总文件:" + files.Length);
                int i = 0;
                foreach (string s in files)
                {
                    try
                    {
                        if (s.PathGetFileType() == ".jpg" || s.PathGetFileType() == ".JPG" || s.PathGetFileType() == ".tga") //是图片
                        {
                            string metaPath = s + ".meta";

                            string metacontent = File.ReadAllText(metaPath); //获取这张图片的guid
                            string guid = metacontent.Substring(metacontent.IndexOf("guid: ") + 6, 32);

                            string matcontent = File.ReadAllText(baseMaterialPath);
                            //Debug.Log(i + "位置：1");
                            if (matcontent == null)
                            {
                                continue;
                            }
                            if (baseTextureGuid != guid) //改guid
                                while (matcontent.IndexOf(baseTextureGuid) > 0)
                                {

                                    matcontent = matcontent.Replace(baseTextureGuid, guid);
                                }
                            int MatNameNum = 0; //改名字
                            if (baseMaterialPath.PathGetFileName_NoType() != s.PathGetFileName_NoType())
                                while (matcontent.IndexOf(baseMaterialPath.PathGetFileName_NoType()) > 0)
                                {
                                    MatNameNum++;
                                    matcontent = matcontent.Replace(baseMaterialPath.PathGetFileName_NoType(), s.PathGetFileName_NoType());
                                }
                            if (MatNameNum > 1)
                            {
                                Debug.LogError("多次找到原始材质名字，请检查错误修改的部分，或者原始材质名字请修改" + s.PathBackLevel(1) + "\\" + s.PathGetFileName_NoType() + ".mat");
                            }
                            File.WriteAllText(s.PathBackLevel(1) + "\\" + s.PathGetFileName_NoType() + ".mat", matcontent);

                            Debug.Log(i + "位置：" + s.PathBackLevel(1) + "\\" + s.PathGetFileName_NoType() + ".mat" + "\n" + guid);
                        }

                    }
                    catch
                    {
                        Debug.LogError("报错：" + s);
                    }
                    i++;
                }
            }
        }


        #endregion

        #region -------------文件---------------


        /// <summary>
        /// 保存预制体
        /// </summary>
        /// <param name="path"></param>
        public void SavePerfab(GameObject orginalPerfab, string path)
        {
            PrefabUtility.SaveAsPrefabAsset(orginalPerfab, path);
            Debug.Log("保存地址" + path);

        }

        /// <summary>
        /// 导入asset资源
        /// </summary>
        /// <param name="path"></param>
        public T loadGameobjectByAssetDatabase<T>(string path) where T : UnityEngine.Object
        {

            T perfab = (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));//加载
            return perfab;

        }




        #endregion



        #region ------------------线程-----------------

        private static Dictionary<string, Thread> e_threadTable = new Dictionary<string, Thread>();
        /// <summary>
        /// 创建线程
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
                Debug.Log($"线程创建:{eventType}");
            }
            else
            {

            }

        }
        /// <summary>
        /// 关闭线程
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
                    Debug.Log($"线程关闭{eventType}");
                }
            }
        }


        #endregion

        //示例方法
        public void Test()
        {

        }
    }


}