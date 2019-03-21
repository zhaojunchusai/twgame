/**********************
 * By Mao 2015-2-10
 * 1.滑动时变换大小 (中间的大小为 CenterScale ，两边依次递减，每PositionInterval米，缩小ScaleFactor)
 * 2.确定他的显示优先级 （子物体需继承自UIScrollContentItem ，根据UIScrollContentItem.Depth 来改变其的widget depth）
 * *************************/

using UnityEngine;
using System.Collections.Generic;

public class UIScrollContentScale : MonoBehaviour
{
    public float CenterScale = 1.5f;
    public float ScaleFactor = 0.3f;
    public float PositionInterval = 100f;
    public bool Horiaontal = true;
    public int MaxLayer = 3;
    private UIPanel _panel;
    private List<Transform> _children = new List<Transform>();
    private void Start()
    {
        _panel = NGUITools.FindInParents<UIPanel>(gameObject);
        if (GetComponent<UIWrapContent>() == null)
        {
            _panel.onClipMove = OnMove;
        }
        else
        {
            //GetComponent<UIWrapContent>().OnClipMove = OnMove;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != null)
            {
                _children.Add(transform.GetChild(i));
            }
        }
    }


    public void InitScale()
    {
        OnMove(null);
    }

    private void OnMove(UIPanel panel)
    {
        for (int i = 0; i < _children.Count; i++)
        {
            CalculateScale(_children[i]);
        }
    }

    private void CalculateScale(Transform trans)
    {
        if (!trans.gameObject.activeSelf)
        {
            return;
        }
        float interval = 0;
        interval = Horiaontal ? Mathf.Abs(trans.localPosition.x + _panel.transform.localPosition.x) : Mathf.Abs(trans.localPosition.y + _panel.transform.localPosition.y);
        int dep = MaxLayer - Mathf.RoundToInt(interval / PositionInterval);
        trans.GetComponent<UIScrollContentItem>().Depth = dep;
        float deltaScale = interval / PositionInterval * ScaleFactor;
        float scale = CenterScale - deltaScale;
        trans.localScale = new Vector3(scale, scale,1);
    }
}
