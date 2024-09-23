using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using model;

public class MapEditor : MonoBehaviour
{
    public string name;
    private bool Drag;
    public model.BlockType BrashType = model.BlockType.Grass;
    public model.SummoneEntityType entityType = model.SummoneEntityType.Human;
    public float dragSpeed = 2; // ����϶��ٶ�

    private Vector3 dragOrigin;
    ConfigMap info = new ConfigMap();

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<EntityController>();
        center =new  Vector3(MapeController.Instance.mapx / 2, MapeController.Instance.mapy / 2,0);
        size = new  Vector3(MapeController.Instance.mapx , MapeController.Instance.mapy ,0);

    }

    // Update is called once per frame
    void Update()
    {
        DrawMap();
        DrawEntity();
        dragCamera();
    }

    public Vector3 center = Vector3.zero; // ���ο�����ĵ�
    public Vector3 size = new Vector3(1, 1, 1); // ���ο�ĳߴ�

    private void OnDrawGizmos()
    {
        // ������ο���ĸ�����
        Vector3 halfSize = size * 0.5f;
        Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);

        // ���ƾ��ο��������
        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);
    }

    public void dragCamera()
    {
        // ��ȡ�����ֵĹ�����
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += -scrollAmount * 2;


        // ����������Ƿ���
        if (Input.GetMouseButtonDown(2))
        {
            // ��¼��갴��ʱ��λ��
            dragOrigin = Input.mousePosition;
            Debug.Log("Input.GetMouseButtonDown(2)");
            return;
        }

        // ����������Ƿ������ס
        if (!Input.GetMouseButton(2)) return;

        // ��������ƶ���ƫ����
        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        // �ƶ����
        Camera.main.transform.Translate(move, Space.World);
    }

    //---------���⿨-------------
    public void DrawMap()
    {

        if (Input.GetMouseButton(0))
        {
            Vector3 position_target = Tool.ScreenHelper.GetMouseWorldPosition2D();
            MapeController.Instance.SetBlock((int)position_target.x, (int)position_target.y, BrashType);
            UpdateMape();
            //Debug.Log("ˢ������" + (int)position_target.x + "," + (int)position_target.y);
        }
    }

    public void DrawEntity()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position_target = Tool.ScreenHelper.GetMouseWorldPosition2D();
            var entity = EntityFactory.ins.GetEntity (entityType , (int)position_target.x, (int)position_target.y);
            
            //Debug.Log("ˢ������" + (int)position_target.x + "," + (int)position_target.y);
        }
    }

    public void ClearEntity()
    {
        EntityController.Instance.Clear();
    }
    //--------�洢����-------


    public void UpdateMape()
    {
        MapeController.Instance.ShaderMap();
    }

    //д��洢���ɫλ��
    public void WriteEntity()
    {

        info.InPutEntityData(EntityController.Instance.copeAll());
    }

    //д��洢���ͼλ��
    public void WriteMap()
    {

        info.InPutMapBlock(MapeController.Instance.GetBlockData());
    }

    /// <summary>
    /// ���ļ����ɽ�ɫ
    /// </summary>
    public void ReadLoadEntitys()
    {
        var MapConfig = ConfigController.Instance.ReadConfigMape(name == "" ? "Mape-1" : name);
        foreach (var data in MapConfig.EntityData)
        {

            var humanEntityFirst = EntityFactory.ins.GetEntity((SummoneEntityType)data.EntityType,data.x,data .y);
            humanEntityFirst.gamobject.transform.position = new Vector3(data.x, data.y, 0);
            Debug.Log("ˢ�½�ɫ" );
        }
    }
    

    public void ReadDate()
    {
        var blockConfig= ConfigController.Instance.ReadConfigMape( name == "" ? "Mape-1" : name);
        var blockdata = blockConfig.OutPutMapBlock();
        MapeController.Instance.setBlockData(blockdata);
        UpdateMape();
        ReadLoadEntitys();
    }

    public void SaveDate()
    {
        WriteEntity();
        WriteMap();
        model.ConfigController.Instance.SaveConfigMape(info, name == "" ? "Mape-1" : name);
    }

}


