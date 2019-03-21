using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PetSkillComponent : BaseComponent
{
    private UISprite Spt_Quality;
    private UISprite Spt_Icon;
    private UISprite Spt_Shade;
    private UILabel Lbl_Name;
    private UILabel Lbl_Level;

    public Transform Gobj_SkillInfo;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Gobj_SkillInfo = mRootObject.transform.Find("SkillInfo/Icon").transform;
        Spt_Quality = mRootObject.transform.Find("SkillInfo/Quality").GetComponent<UISprite>();
        Spt_Shade = mRootObject.transform.Find("SkillInfo/Frame").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.Find("SkillInfo/Icon").GetComponent<UISprite>();
        Lbl_Name = mRootObject.transform.Find("SkillName").GetComponent<UILabel>();
        Lbl_Level = mRootObject.transform.Find("SkillLevel").GetComponent<UILabel>();
    }

    public void UpdateInfo(string icon, int quality, string skillname, int level)
    {
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        CommonFunction.SetQualitySprite(Spt_Quality, quality, Spt_Shade);
        Lbl_Name.text = skillname;
        Lbl_Level.text = string.Format(ConstString.LIFESPIRIT_LIFESOUL_LEVEL, level);
    }
}
