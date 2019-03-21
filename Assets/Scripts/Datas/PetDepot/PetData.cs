using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using UnityEngine;
public class PetData
{
    private fogs.proto.msg.Pet mPetPOD;
    public fogs.proto.msg.Pet PetPOD
    {
        get
        {
            return mPetPOD;
        }
    }

    private CombatPetInfo mPetInfo;
    public CombatPetInfo PetInfo
    {
        get
        {
            return mPetInfo;
        }
    }

    private int mLevel;
    public int Level
    {
        get
        {
            return mLevel;
        }
    }

    /// <summary>
    /// 是否已拥有
    /// </summary>
    public bool IsOwned
    {
        get
        {
            if (mPetPOD != null)
                return true;
            return false;
        }
    }

    private bool mIsEquiped;
    /// <summary>
    /// 是否已装备
    /// </summary>
    public bool IsEquiped
    {
        get
        {
            return mIsEquiped;
        }
        set
        {
            if (IsOwned)
            {
                mIsEquiped = value;
            }
            else
            {
                mIsEquiped = false;
            }
        }
    }

    private Skill mSkill;
    /// <summary>
    /// 携带的技能
    /// </summary>
    public Skill Skill
    {
        get
        {
            return mSkill;
        }
    }

    private CombatPetsUpdateInfo mUpgradeMatData;
    public CombatPetsUpdateInfo UpgradeMatData
    {
        get
        {
            return mUpgradeMatData;
        }
    }

    private MaterialsBagAttributeInfo mSkillMatData;
    public MaterialsBagAttributeInfo SkillMatData
    {
        get
        {
            return mSkillMatData;
        }
    }

    private bool mIsNew;
    public bool IsNew
    {
        get
        {
            return mIsNew;
        }
        set
        {
            mIsNew = value;
        }
    }

    public void Init(fogs.proto.msg.Pet pod)
    {
        mPetPOD = pod;
        if (mPetPOD == null)
            return;
        CombatPetInfo info = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(mPetPOD.id);
        Init(pod, info);
    }

    public void Init(fogs.proto.msg.Pet pod, CombatPetInfo info)
    {
        mPetPOD = pod;
        mPetInfo = info;
        if (mPetPOD == null)
        {
            mLevel = 1;
        }
        else
        {
            mLevel = mPetPOD.level;
        }
        if (info == null)
        {
            mPetInfo = null;
            mSkill = null;
            mUpgradeMatData = null;
            mSkillMatData = null;
            return;
        }

        PetCommonMaterilData upgrade_matdata = mPetInfo.strengthCostBag.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.num == mLevel;
        });
        if (upgrade_matdata != null)
        {
            mUpgradeMatData = ConfigManager.Instance.mCombatPetsUpdateConfig.GetInfoByID(upgrade_matdata.materailID);
        }
        else
        {
            mUpgradeMatData = null;
        }
        mIsEquiped = PlayerData.Instance.EquipPetID.Equals(mPetInfo.id);
        mSkill = Skill.createByID(mPetInfo.skillID);
        if (mPetPOD != null && mPetPOD.skill != null && mPetPOD.skill.Count > 0)
            mSkill.Serialize(mPetPOD.skill[0]);
        int index = mSkill.Level - mSkill.Att.initLevel;
        if (mSkill.Att.materialBag != null && mSkill.Att.materialBag.Count > index)
        {
            uint skillMatID = mSkill.Att.materialBag[mSkill.Level - mSkill.Att.initLevel + 1];
            mSkillMatData = ConfigManager.Instance.mMaterialsBagData.FindBynId(skillMatID);
        }
        else
        {
            mSkillMatData = null;
        }
    }


    public void UpdateSkill(List<fogs.proto.msg.Skill> skills)
    {
        if (skills == null || skills.Count == 0)
        {
            return;
        }
        fogs.proto.msg.Skill skill = skills[0];
        mSkill = Skill.createByID(skill.id);
        if (mSkill == null)
            return;
        mSkill.Serialize(skill);
        int index = mSkill.Level - mSkill.Att.initLevel;
        if (mSkill.Att.materialBag != null && mSkill.Att.materialBag.Count > index)
        {
            uint skillMatID = mSkill.Att.materialBag[mSkill.Level - mSkill.Att.initLevel + 1];
            mSkillMatData = ConfigManager.Instance.mMaterialsBagData.FindBynId(skillMatID);
        }
        else
        {
            mSkillMatData = null;
        }
    }

    //public void Init(fogs.proto.msg.Pet pod, CombatPetInfo info)
    //{
    //    mPetPOD = pod;
    //    mPetInfo = info;
    //    if (pod == null)
    //        mLevel = 1;
    //    else
    //    {
    //        mLevel = pod.level;
    //    }
    //    mSkill = Skill.createByID(info.skillID);
    //    mSkill.Serialize(skill);
    //    mIsEquiped = PlayerData.Instance.EquipPetID.Equals(mPetInfo.id);
    //    int index = mSkill.Level - mSkill.Att.initLevel + 1;
    //    if (mSkill.Att.materialBag != null && mSkill.Att.materialBag.Count > index)
    //    {
    //        uint skillMatID = mSkill.Att.materialBag[mSkill.Level - mSkill.Att.initLevel + 1];
    //        mSkillMatData = ConfigManager.Instance.mMaterialsBagData.FindBynId(skillMatID);
    //    }

    //    PetCommonMaterilData upgrade_matdata = mPetInfo.strengthCostBag.Find((tmp) =>
    //    {
    //        if (tmp == null)
    //            return false;
    //        return tmp.num == mLevel;
    //    });
    //    if (upgrade_matdata != null)
    //    {
    //        mUpgradeMatData = ConfigManager.Instance.mCombatPetsUpdateConfig.GetInfoByID(upgrade_matdata.materailID);
    //    }
    //}

    //public void UpdateSkill(List<fogs.proto.msg.Skill> skill)
    //{
    //    mSkill = Skill.createByID(mPetInfo.skillID);
    //    mSkill.Serialize(skill);
    //    int index = mSkill.Level - mSkill.Att.initLevel + 1;
    //    if (mSkill.Att.materialBag != null && mSkill.Att.materialBag.Count > index)
    //    {
    //        uint skillMatID = mSkill.Att.materialBag[mSkill.Level - mSkill.Att.initLevel + 1];
    //        mSkillMatData = ConfigManager.Instance.mMaterialsBagData.FindBynId(skillMatID);
    //    }
    //}
}
