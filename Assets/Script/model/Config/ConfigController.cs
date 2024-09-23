using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Text.Json;
using System;
using System.IO;

namespace model
{
    //只能序列化过的一维数组，只能包含string int 
    public class ConfigController : SingletonMonoBehaviour<ConfigController>
    {
        Dictionary<string, Dictionary<long, Config>> ConfigurationTable;
        public const string ConfigMapPath = "/Resources/ConfigMap/";
        public string configPathMain;
        public void Awake()
        {

            configPathMain = Application.dataPath ;
        }



        public void SaveConfigGameData(ConfigGameData data)
        {
            
            SaveJsonFile<ConfigGameData>(data, configPathMain + ConfigMapPath + "ConfigGameData" + ".json");
            Debug.Log("保存成功" + configPathMain + ConfigMapPath + "ConfigGameData" + ".json");
        }

        public ConfigGameData ReadConfigGameData()
        {
            ConfigGameData configMap;
            //configMap = ReadJsonFile<ConfigGameData>(configPathMain + ConfigMapPath + name + ".json");
            configMap = ReadJsonFile<ConfigGameData>( name );
            Debug.Log("读取成功" + configPathMain + ConfigMapPath + name + ".json");
            return configMap;
        }


        public void SaveConfigMape(ConfigMap ConfigMap,string name)
        {
            
            SaveJsonFile<ConfigMap>(ConfigMap, configPathMain + ConfigMapPath+ name+".json");
            Debug.Log("保存成功" + configPathMain + ConfigMapPath + name + ".json");
        }


        public ConfigMap ReadConfigMape( string name)
        {
            ConfigMap configMap;
            configMap = ReadJsonFile<ConfigMap>( name );
            Debug.Log("读取成功" + configPathMain + ConfigMapPath + name + ".json");
            return configMap;
        }



        /// <summary>
        /// 这里有个加密公式，来保存加密
        /// </summary>
        public void save()
        {

        }

        T ReadJsonFile<T>(string filePath)
        {
            //try
            //{
                //if (File.Exists(filePath))
                //{
                    var a = ResourseController.ins.GetText(filePath);
            Debug.Log(filePath);
            if (a == null) return default(T);
                    string jsonText = a.text;// File.ReadAllText(filePath);
                    return JsonUtility.FromJson<T>(jsonText);
            //    }
            //    else
            //    {
            //        Debug.LogWarning($"File not found: {filePath}");
            //        return default(T);
            //    }
            //}
            //catch (System.Exception ex)
            //{
            //    Debug.LogError($"Error reading JSON file: {ex.Message}");
            //    return default(T);
            //}
        }

        void SaveJsonFile<T>(T jsonData, string filePath)
        {
            try
            {
                string jsonText = JsonUtility.ToJson(jsonData, true);
                File.WriteAllText(filePath, jsonText);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error saving JSON file: {ex.Message}");
            }
        }
    }
}

public class Config
{
}
