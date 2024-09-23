using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Tool
{
    /// <summary>
    /// 窗口
    /// </summary>
    public class EditorWindow : UnityEditor.EditorWindow
    {
        EditorTool tool = new EditorTool();
        EditorWindow()
        {
            this.titleContent = new GUIContent("Bug Reporter");
        }

        [MenuItem("zpyTools/工具窗口")]
        static void ShowWindow()
        {
            UnityEditor.EditorWindow.GetWindow(typeof(EditorWindow));
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            GUILayout.Space(10);
            GUI.skin.label.fontSize = 10;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;
            GUILayout.Label("Bug Reporter");
            tool.Main();

        }
    }


}