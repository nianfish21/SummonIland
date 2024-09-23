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

        // ����Handles�Ļ���
        Handles.BeginGUI();

        // ��������������¼�
        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Vector3 mousePosition = Event.current.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;

            // ��ȡ�����λ�õ���������
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPosition = hit.point;

                // ��������
                if (_lastPosition != null)
                {
                    Handles.DrawLine(_lastPosition.Value, worldPosition);
                }

                _lastPosition = worldPosition;

                // ��ǳ�����ͼΪ�Ѹ��ģ��Ա�Unity�������
                if (GUI.changed)
                {
                    EditorUtility.SetDirty(drawLines);
                }
            }
        }

        // ����Handles�Ļ���
        Handles.EndGUI();
    }
}