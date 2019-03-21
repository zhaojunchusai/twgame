using System;
using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
public class PetInfoComponent : BaseComponent
{
    private UISprite Spt_SelectMark;
    private UILabel Lbl_PetName;
    private UILabel Lbl_PetLevel;
    private UISprite Spt_EquipedMark;
    private UISprite Spt_Quality;
    private UISprite Spt_Frame;
    private UISprite Spt_Prompt;
    private UISprite Spt_Icon;

    private PetData mPetData;
    public PetData PetData
    {
        get
        {
            return mPetData;
        }
    }

    public bool IsEquiped
    {
        get
        {
            return PetData.IsEquiped;
        }
        set
        {
            PetData.IsEquiped = value;
            Spt_EquipedMark.enabled = PetData.IsEquiped;
        }
    }

    private bool mIsSelect;
    public bool IsSelect
    {
        get
        {
            return mIsSelect;
        }
        set
        {
            mIsSelect = value;
            Spt_SelectMark.enabled = mIsSelect;
        }
    }

    public bool IsPrompt
    {
        set
        {
            Spt_Prompt.enabled = value;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_SelectMark = mRootObject.transform.Find("SelecltMark").GetComponent<UISprite>();
        Spt_Quality = mRootObject.transform.Find("PetInfo/Quality").GetComponent<UISprite>();
        Spt_Frame = mRootObject.transform.Find("PetInfo/Frame").GetComponent<UISprite>();
        Spt_Prompt = mRootObject.transform.Find("PetInfo/Prompt").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.Find("PetInfo/Icon").GetComponent<UISprite>();
        Lbl_PetName = mRootObject.transform.Find("PetName").GetComponent<UILabel>();
        Lbl_PetLevel = mRootObject.transform.Find("PetLevel").GetComponent<UILabel>();
        Spt_EquipedMark = mRootObject.transform.Find("EquipedMark").GetComponent<UISprite>();
        Spt_EquipedMark.enabled = false;
        IsSelect = false;
        IsPrompt = false;
    }

    public void UpdateCompInfo(PetData petdata)
    {
        mPetData = petdata;
        if (mPetData == null)
        {
            return;
        }
        if (mPetData == null)
        {
            IsPrompt = false;
        }
        UpdateInfo();
        UpdateCompStatus();
        UpdatePromptStatus();
    }


    private void UpdateInfo()
    {
        Lbl_PetName.text = PetData.PetInfo.name;
        Lbl_PetLevel.text = string.Format(ConstString.LIFESPIRIT_LIFESOUL_LEVEL, PetData.Level.ToString());
        CommonFunction.SetSpriteName(Spt_Icon, PetData.PetInfo.icon);
        CommonFunction.SetQualitySprite(Spt_Quality, PetData.PetInfo.quality, Spt_Frame);
    }

    private void UpdateCompStatus()
    {
        CommonFunction.UpdateWidgetGray(Spt_Icon, !PetData.IsOwned);
        CommonFunction.UpdateWidgetGray(Spt_Frame, !PetData.IsOwned);
        CommonFunction.UpdateWidgetGray(Spt_Quality, !PetData.IsOwned);
        if (PetData.IsOwned)
        {
            CommonFunction.SetLabelColor_I(Lbl_PetLevel, 159, 138, 112);
            CommonFunction.SetLabelColor_I(Lbl_PetName, 255, 168, 75);
        }
        else
        {
            Lbl_PetLevel.color = Color.gray;
            Lbl_PetName.color = Color.gray;
        }
    }

    public void UpdatePromptStatus()
    {
        if (PetData.IsOwned)
        {
            if (PetData.IsNew)
            {
                IsPrompt = true;
                return;
            }
            if (PetData.UpgradeMatData != null)
            {
                if (PetData.UpgradeMatData != null && PetData.UpgradeMatData.materials != null && PetData.UpgradeMatData.materials.Count > 0)
                {
                    PetCommonMaterilData matData = PetData.UpgradeMatData.materials[0];//界面设计上仅会出现一个 add by taiwei
                    int ownUpgradeMatCount = PlayerData.Instance.GetItemCountByID(matData.materailID);
                    if (ownUpgradeMatCount >= matData.num)
                    {
                        if (CommonFunction.CheckMoneyEnough((ECurrencyType)PetData.UpgradeMatData.cerrencyType, PetData.UpgradeMatData.cerrencyNum, false))
                        {
                            IsPrompt = true;
                            return;
                        }
                    }
                }
            }

            if (PetData.SkillMatData != null)
            {
                if (PetData.SkillMatData.MaterialList != null && PetData.SkillMatData.MaterialList.Count > 0)
                {
                    KeyValuePair<uint, int> mat_value = new KeyValuePair<uint, int>();
                    foreach (KeyValuePair<uint, int> tmp in PetData.SkillMatData.MaterialList)
                    {
                        mat_value = tmp;
                        break;
                    }
                    int ownSkillmatCount = PlayerData.Instance.GetItemCountByID(mat_value.Key);
                    if (ownSkillmatCount >= mat_value.Value)
                    {
                        if (CommonFunction.CheckMoneyEnough((ECurrencyType)PetData.SkillMatData.costType, PetData.SkillMatData.Cost, false))
                        {
                            IsPrompt = true;
                            return;
                        }
                    }
                }
            }
            IsPrompt = false;
        }
    }
}
