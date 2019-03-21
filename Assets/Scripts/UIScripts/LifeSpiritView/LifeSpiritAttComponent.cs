using System;
using System.Collections.Generic;
using UnityEngine;
public class LifeSpiritAttComponent : BaseComponent
{
    private UILabel Lbl_AttValue;
    private UISprite Spt_AttIcon;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_AttValue = mRootObject.transform.FindChild("AttValue").GetComponent<UILabel>();
        Spt_AttIcon = mRootObject.transform.FindChild("AttIcon").GetComponent<UISprite>();
    }

    public void UpdateInfo(string icon, string att)
    {
        Lbl_AttValue.text = att.ToString();
        CommonFunction.SetSpriteName(Spt_AttIcon, icon);
        Spt_AttIcon.MakePixelPerfect();
    }

    public override void Clear()
    {
        base.Clear();
    }
}
