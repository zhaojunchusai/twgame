using System;
using System.Collections.Generic;
using UnityEngine;
public class LifeSpiritExchangeComponent : LifeSpiritBaseComponent
{
    private UILabel Lbl_Name;
    private UILabel Lbl_SoldierName;
    private UISprite Spt_EquipedMark;
    private UIGrid Grd_AttGrid;
    private GameObject Gobj_AttComp;
    private UILabel Lbl_SkillDesc;
    private UISprite Spt_SimilarEquipedMark;
    private List<LifeSpiritAttComponent> att_dic;

    private bool mIsEquiped;
    public bool IsEquiped
    {
        get
        {
            return mIsEquiped;
        }
        set
        {
            mIsEquiped = value;
            Spt_EquipedMark.enabled = mIsEquiped;
        }
    }

    private bool mIsSimilarEquiped;
    public bool IsSimilarEquiped
    {
        set
        {
            mIsSimilarEquiped = value;
            Spt_SimilarEquipedMark.enabled = mIsSimilarEquiped;
        }
        get
        {
            return mIsSimilarEquiped;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_Name = mRootObject.transform.FindChild("Name").GetComponent<UILabel>();
        Spt_EquipedMark = mRootObject.transform.FindChild("EquipedMark").GetComponent<UISprite>();
        Grd_AttGrid = mRootObject.transform.FindChild("AttributeComparisonGroup/BeforeGroup").GetComponent<UIGrid>();
        Gobj_AttComp = mRootObject.transform.FindChild("AttributeComparisonGroup/BeforeGroup/gobj_AttComp").gameObject;
        Lbl_SkillDesc = mRootObject.transform.FindChild("AttributeComparisonGroup/SkillDesc").GetComponent<UILabel>();
        Lbl_SoldierName = mRootObject.transform.FindChild("SoldierName").GetComponent<UILabel>();
        Spt_Icon = mRootObject.transform.FindChild("InfoGroup/Icon").GetComponent<UISprite>();
        Spt_Frame = mRootObject.transform.FindChild("InfoGroup/Frame").GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.FindChild("InfoGroup/Shading").GetComponent<UISprite>();
        Lbl_Level = mRootObject.transform.FindChild("InfoGroup/LevelGroup/Level").GetComponent<UILabel>();
        Spt_SimilarEquipedMark = mRootObject.transform.FindChild("SimilarEquipedMark").GetComponent<UISprite>();
        Gobj_AttComp.SetActive(false);
        if (att_dic == null)
            att_dic = new List<LifeSpiritAttComponent>();
        IsSimilarEquiped = false;
        IsEquiped = false;
    }

    public void UpdateCompInfo(LifeSoulData data)
    {
        mLifeSoulData = data;
        if (mLifeSoulData == null)
            return;
        base.UpdateLifeSpiritInfo();
        UpdateAttInfo();
        if (mLifeSoulData.IsEquipedPlayer || mLifeSoulData.IsEquipedSoldier)
        {
            IsEquiped = true;
        }
        else
        {
            IsEquiped = false;
        }
        if (mLifeSoulData.IsEquipedSoldier)
        {
            Lbl_SoldierName.enabled = true;
            Lbl_SoldierName.text = mLifeSoulData.EquipedSoldier.Att.Name;
        }
        else if (mLifeSoulData.IsEquipedPlayer)
        {
            Lbl_SoldierName.enabled = true;
            Lbl_SoldierName.text = ConstString.LIFESPIRIT_EQUIPEDPLAYER;
        }
        else
        {
            Lbl_SoldierName.text = string.Empty;
            Lbl_SoldierName.enabled = false;
        }
        Lbl_Name.text = mLifeSoulData.SoulInfo.name;
    }

    private void UpdateAttInfo()
    {
        if (mLifeSoulData == null)
            return;
        if (mLifeSoulData.SoulInfo.skillID == 0)
        {
            Grd_AttGrid.gameObject.SetActive(true);
            Lbl_SkillDesc.enabled = false;
            UpdateAttComps();
        }
        else
        {
            Grd_AttGrid.gameObject.SetActive(false);
            Lbl_SkillDesc.enabled = true;
            Lbl_SkillDesc.text = mLifeSoulData.Skill.GetDescript(mLifeSoulData.Skill.Lv);
        }

    }

    private void UpdateAttComps()
    {
        ShowInfoWeapon data = new ShowInfoWeapon();
        data.HP = mLifeSoulData.SoulInfo.hp_initial + mLifeSoulData.SoulInfo.hp_up * (mLifeSoulData.SoulPOD.level - 1);
        data.Attack = mLifeSoulData.SoulInfo.attack_initial + mLifeSoulData.SoulInfo.attack_up * (mLifeSoulData.SoulPOD.level - 1);
        data.Accuracy = mLifeSoulData.SoulInfo.accrate_initial + mLifeSoulData.SoulInfo.accrate_up * (mLifeSoulData.SoulPOD.level - 1);
        data.Dodge = mLifeSoulData.SoulInfo.ddgrate_initial + mLifeSoulData.SoulInfo.ddgrate_up * (mLifeSoulData.SoulPOD.level - 1);
        data.Crit = mLifeSoulData.SoulInfo.crt_initial + mLifeSoulData.SoulInfo.crt_up * (mLifeSoulData.SoulPOD.level - 1);
        data.Tenacity = mLifeSoulData.SoulInfo.tenacity_initial + mLifeSoulData.SoulInfo.tenacity_up * (mLifeSoulData.SoulPOD.level - 1);
        data.HP = Mathf.CeilToInt(data.HP / 10000);
        data.Attack = Mathf.CeilToInt(data.Attack / 10000);
        data.Accuracy = Mathf.CeilToInt(data.Accuracy / 10000);
        data.Dodge = Mathf.CeilToInt(data.Dodge / 10000);
        data.Crit = Mathf.CeilToInt(data.Crit / 10000);
        data.Tenacity = Mathf.CeilToInt(data.Tenacity / 10000);

        List<KeyValuePair<string, string>> attributeList = CommonFunction.GetWeaponAttributeDescNoWord(data);
        int count = att_dic.Count;
        if (attributeList.Count < count)
        {
            for (int i = attributeList.Count; i < count; i++)
            {
                LifeSpiritAttComponent comp = att_dic[i];
                if (comp != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < attributeList.Count; i++)
        {
            KeyValuePair<string, string> tmpInfo = attributeList[i];
            LifeSpiritAttComponent comp = null;
            if (i < count)
            {
                comp = att_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(Gobj_AttComp, Grd_AttGrid.transform);
                comp = new LifeSpiritAttComponent();
                comp.MyStart(go);
                att_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateInfo(tmpInfo.Key, tmpInfo.Value);
            comp.mRootObject.SetActive(true);
        }
        Grd_AttGrid.Reposition();
    }
}
