using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEditor))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapEditor myScript = (MapEditor)target;

        // 在Inspector窗口中显示一个按钮
        if (GUILayout.Button("刷新地图"))
        {
            myScript.ReadDate();
        }
        if (GUILayout.Button("保存"))
        {
            myScript.SaveDate();
        }
        if (GUILayout.Button("清理人物"))
        {
            myScript.ClearEntity();
        }
    }
}