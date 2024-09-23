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
    public float dragSpeed = 2; // 相机拖动速度

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

    public Vector3 center = Vector3.zero; // 矩形框的中心点
    public Vector3 size = new Vector3(1, 1, 1); // 矩形框的尺寸

    private void OnDrawGizmos()
    {
        // 计算矩形框的四个顶点
        Vector3 halfSize = size * 0.5f;
        Vector3 topLeft = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        Vector3 topRight = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        Vector3 bottomLeft = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        Vector3 bottomRight = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);

        // 绘制矩形框的四条边
        Debug.DrawLine(topLeft, topRight, Color.red);
        Debug.DrawLine(topRight, bottomRight, Color.red);
        Debug.DrawLine(bottomRight, bottomLeft, Color.red);
        Debug.DrawLine(bottomLeft, topLeft, Color.red);
    }

    public void dragCamera()
    {
        // 获取鼠标滚轮的滚动量
        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize += -scrollAmount * 2;


        // 检测鼠标左键是否按下
        if (Input.GetMouseButtonDown(2))
        {
            // 记录鼠标按下时的位置
            dragOrigin = Input.mousePosition;
            Debug.Log("Input.GetMouseButtonDown(2)");
            return;
        }

        // 检测鼠标左键是否持续按住
        if (!Input.GetMouseButton(2)) return;

        // 计算鼠标移动的偏移量
        Vector3 pos = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        Vector3 move = new Vector3(pos.x * dragSpeed, pos.y * dragSpeed, 0);

        // 移动相机
        Camera.main.transform.Translate(move, Space.World);
    }

    //---------画光卡-------------
    public void DrawMap()
    {

        if (Input.GetMouseButton(0))
        {
            Vector3 position_target = Tool.ScreenHelper.GetMouseWorldPosition2D();
            MapeController.Instance.SetBlock((int)position_target.x, (int)position_target.y, BrashType);
            UpdateMape();
            //Debug.Log("刷新区域" + (int)position_target.x + "," + (int)position_target.y);
        }
    }

    public void DrawEntity()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 position_target = Tool.ScreenHelper.GetMouseWorldPosition2D();
            var entity = EntityFactory.ins.GetEntity (entityType , (int)position_target.x, (int)position_target.y);
            
            //Debug.Log("刷新区域" + (int)position_target.x + "," + (int)position_target.y);
        }
    }

    public void ClearEntity()
    {
        EntityController.Instance.Clear();
    }
    //--------存储功能-------


    public void UpdateMape()
    {
        MapeController.Instance.ShaderMap();
    }

    //写入存储类角色位置
    public void WriteEntity()
    {

        info.InPutEntityData(EntityController.Instance.copeAll());
    }

    //写入存储类地图位置
    public void WriteMap()
    {

        info.InPutMapBlock(MapeController.Instance.GetBlockData());
    }

    /// <summary>
    /// 从文件生成角色
    /// </summary>
    public void ReadLoadEntitys()
    {
        var MapConfig = ConfigController.Instance.ReadConfigMape(name == "" ? "Mape-1" : name);
        foreach (var data in MapConfig.EntityData)
        {

            var humanEntityFirst = EntityFactory.ins.GetEntity((SummoneEntityType)data.EntityType,data.x,data .y);
            humanEntityFirst.gamobject.transform.position = new Vector3(data.x, data.y, 0);
            Debug.Log("刷新角色" );
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


