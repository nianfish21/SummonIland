using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapEditor))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MapEditor myScript = (MapEditor)target;

        // ��Inspector��������ʾһ����ť
        if (GUILayout.Button("ˢ�µ�ͼ"))
        {
            myScript.ReadDate();
        }
        if (GUILayout.Button("����"))
        {
            myScript.SaveDate();
        }
        if (GUILayout.Button("��������"))
        {
            myScript.ClearEntity();
        }
    }
}