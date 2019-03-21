using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
public class SEAStarComponent : BaseComponent 
{
    public UISprite Spt_SelectSprite;
    public UISprite Spt_DefaultSprite;

    public bool IsSelect 
    {
        set 
        {
            Spt_SelectSprite.enabled = value;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Spt_DefaultSprite = mRootObject.transform.FindChild("BgSprite").gameObject.GetComponent<UISprite>();
    }

    public override void Clear()
    {
        base.Clear();
    } 
}

public class SEAAttributeComponent : BaseComponent
{
    public UISprite Spt_Icon;
    public UILabel Lbl_Att;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Icon = mRootObject.transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
        Lbl_Att = mRootObject.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
    }

    public void UpdateInfo(string icon, string att)
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

public class SEAEquipInfoComponent : BaseComponent
{
    public UISprite Spt_QualitySprite;
    public UISprite Spt_IconTexture;
    public UISprite Spt_ItemBG;
    public UILabel Label_Level;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_IconTexture = mRootObject.transform.FindChild("SoldierEquipAdvancedIcon").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("SoldierEquipAdvancedQuality").gameObject.GetComponent<UISprite>();
        Spt_ItemBG = mRootObject.transform.FindChild("SoldierEquipAdvancedBg").gameObject.GetComponent<UISprite>();
        Label_Level = mRootObject.transform.FindChild("LevelGroup/Label").gameObject.GetComponent<UILabel>();
    }

    public void UpdateInfo(Weapon pod) 
    {
        if (pod == null)
        {
            Clear();
            return;
        }
        if (Label_Level != null)
        {
            Label_Level.text = pod.Level.ToString();
        }
        CommonFunction.SetQualitySprite(Spt_QualitySprite, pod.Att.quality, Spt_ItemBG);
        CommonFunction.SetSpriteName(Spt_IconTexture, pod.Att.icon);
    }

    public override void Clear()
    {
        base.Clear();
        Label_Level.text = string.Empty;
        Spt_IconTexture.spriteName = string.Empty;
        Spt_ItemBG.spriteName = string.Empty;
        Spt_QualitySprite.spriteName = string.Empty;
    }
}