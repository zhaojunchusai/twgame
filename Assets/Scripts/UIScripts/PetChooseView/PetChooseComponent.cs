using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PetChooseComponent : BaseComponent
{
    private UILabel Lbl_Name;
    private UISprite Spt_EquipedMark;
    private UILabel Lbl_SkillName;
    private UISprite Spt_Icon;
    private UISprite Spt_Frame;
    private UISprite Spt_Shading;
    private UILabel Lbl_Level;
    private GameObject Gobj_InfoGroup;

    private PetData mPetData;
    public PetData PetData
    {
        get
        {
            return mPetData;
        }
    }

    private bool mIsEquip;
    public bool IsEquip
    {
        get
        {
            return mIsEquip;
        }
        set
        {
            mIsEquip = value;
            Spt_EquipedMark.enabled = mIsEquip;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_Name = mRootObject.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Spt_EquipedMark = mRootObject.transform.FindChild("EquipedMark").gameObject.GetComponent<UISprite>();
        Lbl_SkillName = mRootObject.transform.FindChild("AttributeComparisonGroup/SkillDesc").gameObject.GetComponent<UILabel>();
        Gobj_InfoGroup = mRootObject.transform.FindChild("InfoGroup").gameObject;
        Spt_Icon = mRootObject.transform.FindChild("InfoGroup/Icon").gameObject.GetComponent<UISprite>();
        Spt_Frame = mRootObject.transform.FindChild("InfoGroup/Frame").gameObject.GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.FindChild("InfoGroup/Shading").gameObject.GetComponent<UISprite>();
        Lbl_Level = mRootObject.transform.FindChild("InfoGroup/LevelGroup/Level").gameObject.GetComponent<UILabel>();
        UIEventListener.Get(Gobj_InfoGroup).onPress = OnObjPress;
    }

    public void UpdateInfo(PetData petdata)
    {
        mPetData = petdata;
        if (mPetData == null)
            return;
        Lbl_SkillName.text = string.Format(ConstString.PETCHOOSE_SKILLNAME, mPetData.Skill.Att.Name);
        Lbl_Level.text = mPetData.Level.ToString();
        CommonFunction.SetSpriteName(Spt_Icon, mPetData.PetInfo.icon);
        CommonFunction.SetQualitySprite(Spt_Frame, mPetData.PetInfo.quality, Spt_Shading);
        Lbl_Name.text = mPetData.PetInfo.name;
    }


    private void OnObjPress(GameObject go, bool isPress)
    {
        if (mPetData == null)
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SEEDETAIL);
        UISystem.Instance.SeeDetailView.SeeDetail(go, isPress, mPetData.Skill.GetDescript(mPetData.Level));
    }
}
