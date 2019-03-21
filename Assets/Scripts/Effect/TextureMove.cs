using UnityEngine;
using System.Collections;

/// <summary>
/// 修改UItextureUV循环播放图片
/// Add by zyx
/// </summary>
public class TextureMove : MonoBehaviour

{
public UITexture player2bg;
public float uvSpeed = 0.1f;
public float Width = 0.3f;
public float height = 1.0f;
private float offset_x = 0f;


void Update ()
{

offset_x += Time.deltaTime * uvSpeed;
if (player2bg == null) return;
    player2bg.uvRect = new Rect(offset_x, 0, Width, height);  

}

}
