using UnityEngine;
using System.Collections.Generic;
public class LifeSpiritPlayerInfoComponent : ItemBaseComponent
{
    private UISprite Spt_SelectMark;

    public bool IsSelect
    {
        set
        {
            Spt_SelectMark.enabled = value;
        }
        get
        {
            return Spt_SelectMark.enabled;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_IconTexture = mRootObject.transform.FindChild("PlayerInfoGroup/Icon").GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("PlayerInfoGroup/BackGround").GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("PlayerInfoGroup/Quality").GetComponent<UISprite>();
        Spt_SelectMark = mRootObject.transform.FindChild("gobj_SelectMark/BG").GetComponent<UISprite>();
    }

    public void UpdateCompInfo()
    {
        CommonFunction.SetHeadAndFrameSprite(Spt_IconTexture, Spt_QualitySprite, PlayerData.Instance.HeadID, PlayerData.Instance.FrameID, true);
    }


}
