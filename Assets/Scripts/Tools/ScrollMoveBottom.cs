using UnityEngine;
using System.Collections;

public class ScrollMoveBottom : MonoBehaviour
{
    public Vector3 Direction;
    public float IntervalDistance;
    public float time;
    public iTween.EaseType easeType = iTween.EaseType.linear;
    public UIScrollView scroll;
    private bool _needComplete = false;

    public void DragBottom(float value)
    {
        if (scroll == null)
        {
            scroll = GetComponent<UIScrollView>();
        }

        iTween.ValueTo(gameObject, iTween.Hash(
            "from", value,
            "to", 1f,
            "time", time,
            "easeType", "linear",//easeType.ToString(),
            "onupdate", "ValueUpdate",
            "oncomplete", "Completed"
            ));
    }

    public void ValueUpdate(float value)
    {
        scroll.SetDragAmount(0, value, false);
    }
    public void Completed()
    {
        if (_needComplete)
        {
            scroll.Scroll(-3);
        }
    }
}
