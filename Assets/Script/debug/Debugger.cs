using UnityEngine;

using System.Collections.Generic;

using System;

using UnityEngine.Profiling;

public class DebuggerTest : MonoBehaviour

{

    /// <summary>

    /// �Ƿ��������

    /// </summary>

    public bool AllowDebugging = true;

    private DebugType _debugType = DebugType.Console;

    private List<LogData> _logInformations = new List<LogData>();

    private int _currentLogIndex = -1;

    private int _infoLogCount = 0;

    private int _warningLogCount = 0;

    private int _errorLogCount = 0;

    private int _fatalLogCount = 0;

    private bool _showInfoLog = true;

    private bool _showWarningLog = true;

    private bool _showErrorLog = true;

    private bool _showFatalLog = true;

    private Vector2 _scrollLogView = Vector2.zero;

    private Vector2 _scrollCurrentLogView = Vector2.zero;

    private Vector2 _scrollSystemView = Vector2.zero;

    private bool _expansion = false;

    private Rect _windowRect = new Rect(0, 0, 100, 60);

    private int _fps = 0;

    private Color _fpsColor = Color.white;

    private int _frameNumber = 0;

    private float _lastShowFPSTime = 0f;

    private void Awake()
    {

        if (AllowDebugging)

        {

            Application.logMessageReceived += LogHandler;

        }
    }

    private void Start()

    {


    }

    private void Update()

    {

        if (AllowDebugging)

        {

            _frameNumber += 1;

            float time = Time.realtimeSinceStartup - _lastShowFPSTime;

            if (time >= 1)

            {

                _fps = (int)(_frameNumber / time);

                _frameNumber = 0;

                _lastShowFPSTime = Time.realtimeSinceStartup;

            }

        }

    }

    private void OnDestory()

    {

        if (AllowDebugging)

        {

            Application.logMessageReceived -= LogHandler;

        }

    }

    private void LogHandler(string condition, string stackTrace, LogType type)

    {

        LogData log = new LogData();

        log.time = DateTime.Now.ToString("HH:mm:ss");

        log.message = condition;

        log.stackTrace = stackTrace;

        if (type == LogType.Assert)

        {

            log.type = "Fatal";

            _fatalLogCount += 1;

        }

        else if (type == LogType.Exception || type == LogType.Error)

        {

            log.type = "Error";

            _errorLogCount += 1;

        }

        else if (type == LogType.Warning)

        {

            log.type = "Warning";

            _warningLogCount += 1;

        }

        else if (type == LogType.Log)

        {

            log.type = "Info";

            _infoLogCount += 1;

        }

        _logInformations.Add(log);

        if (_warningLogCount > 0)

        {

            _fpsColor = Color.yellow;

        }

        if (_errorLogCount > 0)

        {

            _fpsColor = Color.red;

        }

    }

    private void OnGUI()

    {

        if (AllowDebugging)

        {

            if (_expansion)

            {

                _windowRect = GUI.Window(0, _windowRect, ExpansionGUIWindow, "DEBUGGER");

            }

            else

            {

                _windowRect = GUI.Window(0, _windowRect, ShrinkGUIWindow, "DEBUGGER");

            }

        }

    }

    private void ExpansionGUIWindow(int windowId)

    {

        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        #region title

        GUILayout.BeginHorizontal();

        GUI.contentColor = _fpsColor;

        if (GUILayout.Button("FPS:" + _fps, GUILayout.Height(30)))

        {

            _expansion = false;

            _windowRect.width = 100;

            _windowRect.height = 60;

        }

        GUI.contentColor = (_debugType == DebugType.Console ? Color.white : Color.gray);

        if (GUILayout.Button("Console", GUILayout.Height(30)))

        {

            _debugType = DebugType.Console;

        }

        GUI.contentColor = (_debugType == DebugType.Memory ? Color.white : Color.gray);

        if (GUILayout.Button("Memory", GUILayout.Height(30)))

        {

            _debugType = DebugType.Memory;

        }

        GUI.contentColor = (_debugType == DebugType.System ? Color.white : Color.gray);

        if (GUILayout.Button("System", GUILayout.Height(30)))

        {

            _debugType = DebugType.System;

        }

        GUI.contentColor = (_debugType == DebugType.Screen ? Color.white : Color.gray);

        if (GUILayout.Button("Screen", GUILayout.Height(30)))

        {

            _debugType = DebugType.Screen;

        }

        GUI.contentColor = (_debugType == DebugType.Quality ? Color.white : Color.gray);

        if (GUILayout.Button("Quality", GUILayout.Height(30)))

        {

            _debugType = DebugType.Quality;

        }

        GUI.contentColor = (_debugType == DebugType.Environment ? Color.white : Color.gray);

        if (GUILayout.Button("Environment", GUILayout.Height(30)))

        {

            _debugType = DebugType.Environment;

        }

        GUI.contentColor = Color.white;

        GUILayout.EndHorizontal();

        #endregion

        #region console

        if (_debugType == DebugType.Console)

        {

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Clear"))

            {

                _logInformations.Clear();

                _fatalLogCount = 0;

                _warningLogCount = 0;

                _errorLogCount = 0;

                _infoLogCount = 0;

                _currentLogIndex = -1;

                _fpsColor = Color.white;

            }

            GUI.contentColor = (_showInfoLog ? Color.white : Color.gray);

            _showInfoLog = GUILayout.Toggle(_showInfoLog, "Info [" + _infoLogCount + "]");

            GUI.contentColor = (_showWarningLog ? Color.white : Color.gray);

            _showWarningLog = GUILayout.Toggle(_showWarningLog, "Warning [" + _warningLogCount + "]");

            GUI.contentColor = (_showErrorLog ? Color.white : Color.gray);

            _showErrorLog = GUILayout.Toggle(_showErrorLog, "Error [" + _errorLogCount + "]");

            GUI.contentColor = (_showFatalLog ? Color.white : Color.gray);

            _showFatalLog = GUILayout.Toggle(_showFatalLog, "Fatal [" + _fatalLogCount + "]");

            GUI.contentColor = Color.white;

            GUILayout.EndHorizontal();

            _scrollLogView = GUILayout.BeginScrollView(_scrollLogView, "Box", GUILayout.Height(165));

            for (int i = 0; i < _logInformations.Count; i++)

            {

                bool show = false;

                Color color = Color.white;

                switch (_logInformations[i].type)

                {

                    case "Fatal":

                        show = _showFatalLog;

                        color = Color.red;

                        break;

                    case "Error":

                        show = _showErrorLog;

                        color = Color.red;

                        break;

                    case "Info":

                        show = _showInfoLog;

                        color = Color.white;

                        break;

                    case "Warning":

                        show = _showWarningLog;

                        color = Color.yellow;

                        break;

                    default:

                        break;

                }

                if (show)

                {

                    GUILayout.BeginHorizontal();

                    if (GUILayout.Toggle(_currentLogIndex == i, ""))

                    {

                        _currentLogIndex = i;

                    }

                    GUI.contentColor = color;

                    GUILayout.Label("[" + _logInformations[i].type + "] ");

                    GUILayout.Label("[" + _logInformations[i].time + "] ");

                    GUILayout.Label(_logInformations[i].message);

                    GUILayout.FlexibleSpace();

                    GUI.contentColor = Color.white;

                    GUILayout.EndHorizontal();

                }

            }

            GUILayout.EndScrollView();

            _scrollCurrentLogView = GUILayout.BeginScrollView(_scrollCurrentLogView, "Box", GUILayout.Height(100));

            if (_currentLogIndex != -1)

            {

                GUILayout.Label(_logInformations[_currentLogIndex].message + "\r\n\r\n" + _logInformations[_currentLogIndex].stackTrace);

            }

            GUILayout.EndScrollView();

        }

        #endregion

        #region memory

        else if (_debugType == DebugType.Memory)

        {

            GUILayout.BeginHorizontal();

            GUILayout.Label("Memory Information");

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");

#if UNITY_5

            GUILayout.Label("���ڴ棺" + Profiler.GetTotalReservedMemory() / 1000000 + "MB");

            GUILayout.Label("��ռ���ڴ棺" + Profiler.GetTotalAllocatedMemory() / 1000000 + "MB");

            GUILayout.Label("�������ڴ棺" + Profiler.GetTotalUnusedReservedMemory() / 1000000 + "MB");

            GUILayout.Label("��Mono���ڴ棺" + Profiler.GetMonoHeapSize() / 1000000 + "MB");

            GUILayout.Label("��ռ��Mono���ڴ棺" + Profiler.GetMonoUsedSize() / 1000000 + "MB");

#endif

#if UNITY_7

            GUILayout.Label("���ڴ棺" + Profiler.GetTotalReservedMemoryLong() / 1000000 + "MB");

            GUILayout.Label("��ռ���ڴ棺" + Profiler.GetTotalAllocatedMemoryLong() / 1000000 + "MB");

            GUILayout.Label("�������ڴ棺" + Profiler.GetTotalUnusedReservedMemoryLong() / 1000000 + "MB");

            GUILayout.Label("��Mono���ڴ棺" + Profiler.GetMonoHeapSizeLong() / 1000000 + "MB");

            GUILayout.Label("��ռ��Mono���ڴ棺" + Profiler.GetMonoUsedSizeLong() / 1000000 + "MB");

#endif

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("ж��δʹ�õ���Դ"))

            {

                Resources.UnloadUnusedAssets();

            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("ʹ��GC��������"))

            {

                GC.Collect();

            }

            GUILayout.EndHorizontal();

        }

        #endregion

        #region system

        else if (_debugType == DebugType.System)

        {

            GUILayout.BeginHorizontal();

            GUILayout.Label("System Information");

            GUILayout.EndHorizontal();

            _scrollSystemView = GUILayout.BeginScrollView(_scrollSystemView, "Box");

            GUILayout.Label("����ϵͳ��" + SystemInfo.operatingSystem);

            GUILayout.Label("ϵͳ�ڴ棺" + SystemInfo.systemMemorySize + "MB");

            GUILayout.Label("��������" + SystemInfo.processorType);

            GUILayout.Label("������������" + SystemInfo.processorCount);

            GUILayout.Label("�Կ���" + SystemInfo.graphicsDeviceName);

            GUILayout.Label("�Կ����ͣ�" + SystemInfo.graphicsDeviceType);

            GUILayout.Label("�Դ棺" + SystemInfo.graphicsMemorySize + "MB");

            GUILayout.Label("�Կ���ʶ��" + SystemInfo.graphicsDeviceID);

            GUILayout.Label("�Կ���Ӧ�̣�" + SystemInfo.graphicsDeviceVendor);

            GUILayout.Label("�Կ���Ӧ�̱�ʶ�룺" + SystemInfo.graphicsDeviceVendorID);

            GUILayout.Label("�豸ģʽ��" + SystemInfo.deviceModel);

            GUILayout.Label("�豸���ƣ�" + SystemInfo.deviceName);

            GUILayout.Label("�豸���ͣ�" + SystemInfo.deviceType);

            GUILayout.Label("�豸��ʶ��" + SystemInfo.deviceUniqueIdentifier);

            GUILayout.EndScrollView();

        }

        #endregion

        #region screen

        else if (_debugType == DebugType.Screen)

        {

            GUILayout.BeginHorizontal();

            GUILayout.Label("Screen Information");

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");

            GUILayout.Label("DPI��" + Screen.dpi);

            GUILayout.Label("�ֱ��ʣ�" + Screen.currentResolution.ToString());

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("ȫ��"))

            {

                Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, !Screen.fullScreen);

            }

            GUILayout.EndHorizontal();

        }

        #endregion

        #region Quality

        else if (_debugType == DebugType.Quality)

        {

            GUILayout.BeginHorizontal();

            GUILayout.Label("Quality Information");

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");

            string value = "";

            if (QualitySettings.GetQualityLevel() == 0)

            {

                value = " [���]";

            }

            else if (QualitySettings.GetQualityLevel() == QualitySettings.names.Length - 1)

            {

                value = " [���]";

            }

            GUILayout.Label("ͼ��������" + QualitySettings.names[QualitySettings.GetQualityLevel()] + value);

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("����һ��ͼ������"))

            {

                QualitySettings.DecreaseLevel();

            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("����һ��ͼ������"))

            {

                QualitySettings.IncreaseLevel();

            }

            GUILayout.EndHorizontal();

        }

        #endregion

        #region Environment

        else if (_debugType == DebugType.Environment)

        {

            GUILayout.BeginHorizontal();

            GUILayout.Label("Environment Information");

            GUILayout.EndHorizontal();

            GUILayout.BeginVertical("Box");

            GUILayout.Label("��Ŀ���ƣ�" + Application.productName);

#if UNITY_5

            GUILayout.Label("��ĿID��" + Application.bundleIdentifier);

#endif

#if UNITY_7

            GUILayout.Label("��ĿID��" + Application.identifier);

#endif

            GUILayout.Label("��Ŀ�汾��" + Application.version);

            GUILayout.Label("Unity�汾��" + Application.unityVersion);

            GUILayout.Label("��˾���ƣ�" + Application.companyName);

            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("�˳�����"))

            {

                Application.Quit();

            }

            GUILayout.EndHorizontal();

        }

        #endregion

    }

    private void ShrinkGUIWindow(int windowId)

    {

        GUI.DragWindow(new Rect(0, 0, 10000, 20));

        GUI.contentColor = _fpsColor;

        if (GUILayout.Button("FPS:" + _fps, GUILayout.Width(80), GUILayout.Height(30)))

        {

            _expansion = true;

            _windowRect.width = 600;

            _windowRect.height = 360;

        }

        GUI.contentColor = Color.white;

    }

}

public struct LogData

{

    public string time;

    public string type;

    public string message;

    public string stackTrace;

}

public enum DebugType

{

    Console,

    Memory,

    System,

    Screen,

    Quality,

    Environment
}