
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UIScrollView))]
public class RollSrollViewItem : MonoBehaviour
{
    [HideInInspector]public UIScrollView mScrollView;

    public void Initialize()
    {
        mScrollView = this.GetComponent<UIScrollView>();
    }

    public void UnInitialize()
    {
    }

    void Awake()
    {
        Initialize();
    }

    public void Roll(float fromAmount, float toAmount, float time)
    {
        iTween.ValueTo(this.gameObject, iTween.Hash("time", time, "from", fromAmount, "to", toAmount, "onupdate", "SetScrollViewAmount","easetype",iTween.EaseType.linear));
    }

    private void SetScrollViewAmount(float amount)
    {
        mScrollView.SetDragAmount(0, amount, false);
    }
}
