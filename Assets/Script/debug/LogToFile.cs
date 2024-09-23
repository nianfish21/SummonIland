using System;
using System.IO;
using UnityEngine;
 
public class LogToFile : MonoBehaviour
{
    private static string logFileName = "unity_log.txt";
    private static string logFilePath;

    void Start()
    {
        Application.logMessageReceived += HandleLog;
        logFilePath = Path.Combine(Application.persistentDataPath, logFileName);
        GameObject.DontDestroyOnLoad(gameObject);
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        // 将日志信息写入文件
        File.AppendAllText(logFilePath, string.Format("[{0}] {1}\n{2}\n", DateTime.Now, logString, stackTrace));
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }
}