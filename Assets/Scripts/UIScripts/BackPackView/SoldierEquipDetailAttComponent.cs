using UnityEngine;
using System.Collections.Generic;
public class SoldierEquipDetailAttComponent :BaseComponent
{
    public UISprite Spt_Icon;
    public UILabel Lbl_Att;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Icon = mRootObject.transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
        Lbl_Att = mRootObject.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
    }

    public void UpdateInfo(string icon,string att) 
    {
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        Spt_Icon.MakePixelPerfect();
        Lbl_Att.text = att;
    }

    public override void Clear()
    {
        base.Clear();
        Spt_Icon.spriteName = string.Empty;
        Lbl_Att.text = string.Empty;
    }

}

