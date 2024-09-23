using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DrawLines))]
public class DrawLinesEditor : Editor
{
    private Vector3? _lastPosition;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorGUILayout.HelpBox("Click in the Scene view to draw lines.", MessageType.Info);
    }

    void OnSceneGUI()
    {
        DrawLines drawLines = (DrawLines)target;

        // 开启Handles的绘制
        Handles.BeginGUI();

        // 监听鼠标左键点击事件
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector3 mousePosition = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;

            // 获取鼠标点击位置的世界坐标
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPosition = hit.point;

                // 绘制线条
                if (_lastPosition != null)
                {
                    Handles.DrawLine(_lastPosition.Value, worldPosition);
                }

                _lastPosition = worldPosition;

                // 标记场景视图为已更改，以便Unity保存更改
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(drawLines);
                }
            }
        }

        // 结束Handles的绘制
        Handles.EndGUI();
    }
}