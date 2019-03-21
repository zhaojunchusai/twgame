using System;
using System.Collections.Generic;
using UnityEngine;
public class HintTipLabelComponent : BaseComponent
{
    private UILabel Lbl_CommonTip;
    private UISprite Spt_CommonBG;
    private TweenAlpha TweenAlpha_Obj;
    private TweenPosition TweenPosition_Obj;
    private System.Action callBack;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_CommonTip = mRootObject.transform.FindChild("CommonTipLabel").GetComponent<UILabel>();
        Spt_CommonBG = mRootObject.transform.FindChild("TipBg").GetComponent<UISprite>();
        TweenAlpha_Obj = mRootObject.GetComponent<TweenAlpha>();
        if (TweenAlpha_Obj == null)
            TweenAlpha_Obj = mRootObject.AddComponent<TweenAlpha>();
        TweenPosition_Obj = mRootObject.GetComponent<TweenPosition>();
        if (TweenPosition_Obj == null)
            TweenPosition_Obj = mRootObject.AddComponent<TweenPosition>();
    }

    public void UpdateContent(string content, int depth, System.Action action)
    {
        callBack = action;
        UpdateWidgetDepth(depth);
        Lbl_CommonTip.text = content;
        TweenAlpha_Obj.enabled = true;
        TweenAlpha_Obj.ResetToBeginning();
        TweenAlpha_Obj.PlayForward();
        TweenPosition_Obj.enabled = true;
        TweenPosition_Obj.ResetToBeginning();
        TweenPosition_Obj.PlayForward();
        TweenPosition_Obj.SetOnFinished(SetActive);
        mRootObject.SetActive(true);
    }

    public void UpdateWidgetDepth(int depth)
    {
        Spt_CommonBG.depth = depth;
        Lbl_CommonTip.depth = depth + 1;
    }

    private void SetActive()
    {
        mRootObject.SetActive(false);
        if (callBack != null)
            callBack();
    }
}
