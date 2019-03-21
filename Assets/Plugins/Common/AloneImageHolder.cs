using UnityEngine;

public class AloneImageHolder : MonoBehaviour
{
    public Texture Texture;     //图片对象
    public bool SaveToBytes;    //是否保存为原始二进制数据.如果启用原始二进制支持，则以下属性可用


    [HideInInspector]
    public byte[] ImageData;         //图片原始二进制数据
    [HideInInspector]
    public string Desc_ImageName;    //图片名称
    [HideInInspector]
    public int Desc_ImageWidth;      //图片宽度描述
    [HideInInspector]
    public int Desc_ImageHeight;     //图片高度描述
}