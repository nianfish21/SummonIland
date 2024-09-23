using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//Õ³ÎÄ¼þ
public struct GuleSliceFile
{
    public string name;
    public Dictionary<long, SliceSettings> SliceSettingDic;
    public Dictionary<long, string> SlicesPath;
    public Dictionary<long, byte> SlicesImage;
}




public struct SliceSettings
{
    public string name;
    public long id;
    public bool isBindParent;
    public long parentId;
    public int layer;

    public float size;
    public float[] pivot;
    public float[] localPosition;
    public float rotateRate;
    public float[] moveDirect;
    public float moveLag;
    public float vertexMoveLag;

    public float cycal_size;
    public float[] cycal_MoveDirect;
    public float cycal_MoveDirect_Speed;
    public float cycal_RotateRate;
    public float cycal_RotateRate_Speed;

    public bool usePhysical;
    public float vertexSpringHardness;
    public float pivotSpringHardness;


}
