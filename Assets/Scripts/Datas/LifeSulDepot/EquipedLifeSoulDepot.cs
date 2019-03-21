using System;
using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
public class LifeSoulData
{

    private bool mIsEquipedSoldier;
    public bool IsEquipedSoldier
    {
        get
        {
            return mIsEquipedSoldier;
        }
        set
        {
            mIsEquipedSoldier = value;
            if (mIsEquipedSoldier)
            {
                mIsEquipedPlayer = false;
            }
        }
    }

    private Soldier mEquipedSoldier;
    public Soldier EquipedSoldier
    {
        get
        {
            return mEquipedSoldier;
        }
        set
        {
            mEquipedSoldier = value;
        }
    }

    private bool mIsEquipedPlayer;
    public bool IsEquipedPlayer
    {
        get
        {
            return mIsEquipedPlayer;
        }
        set
        {
            mIsEquipedPlayer = value;
            if (mIsEquipedPlayer)
            {
                mIsEquipedSoldier = false;
            }
        }
    }

    private LifeSoul mSoulPOD;
    public LifeSoul SoulPOD
    {
        get
        {
            return mSoulPOD;
        }
    }

    /// <summary>
    /// 配置表数据
    /// </summary>
    private LifeSoulConfigInfo mSoulInfo;
    public LifeSoulConfigInfo SoulInfo
    {
        get
        {
            return mSoulInfo;
        }
    }

    /// <summary>
    /// 技能效果器
    /// </summary>
    private Skill mSkill;
    public Skill Skill
    {
        get
        {
            return mSkill;
        }
    }

    public LifeSoulData(LifeSoul pod)
    {
        mSoulPOD = new LifeSoul();
        mSoulInfo = new LifeSoulConfigInfo();
        CopyPOD(pod);
        EquipedSoldier = null;
        mIsEquipedSoldier = false;
        mIsEquipedPlayer = false;
    }

    public LifeSoulData(LifeSoul pod, fogs.proto.msg.Soldier soldier)
    {
        mSoulPOD = new LifeSoul();
        mSoulInfo = new LifeSoulConfigInfo();
        CopyPOD(pod);
        EquipedSoldier = Soldier.createByID(soldier.id);
        EquipedSoldier.Serialize(soldier);
        mIsEquipedSoldier = true;
        mIsEquipedPlayer = false;
    }

    public LifeSoulData(LifeSoul pod, Soldier soldier)
    {
        mSoulPOD = new LifeSoul();
        mSoulInfo = new LifeSoulConfigInfo();
        CopyPOD(pod);
        EquipedSoldier = soldier;
        mIsEquipedSoldier = true;
        mIsEquipedPlayer = false;
    }

    public LifeSoulData(LifeSoul pod, bool isPlayer)
    {
        mSoulPOD = new LifeSoul();
        mSoulInfo = new LifeSoulConfigInfo();
        CopyPOD(pod);
        EquipedSoldier = null;
        mIsEquipedSoldier = false;
        mIsEquipedPlayer = true;
    }

    public void CopyPOD(LifeSoul pod)
    {
        mSoulPOD.exp = pod.exp;
        mSoulPOD.id = pod.id;
        mSoulPOD.level = pod.level;
        mSoulPOD.position = pod.position;
        mSoulPOD.uid = pod.uid;
        mSoulInfo = ConfigManager.Instance.mLifeSoulConfig.GetLifeDataByID((int)pod.id);
        if (mSoulInfo.skillID != 0 && pod.skill != null)
        {
            mSkill = null;
            mSkill = Skill.createByID(pod.skill.id);
            if (mSkill == null)
                return;
            mSkill.Serialize(pod.skill);
        }
    }

    public bool IsAbleToUpgrade(bool showHint = true)
    {
        if (this.SoulPOD.level > PlayerData.Instance._Level)
        {
            if (showHint)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_PLAYERLEVELLIMIT);
            }
            return false;
        }
        if (this.SoulPOD.level > this.SoulInfo.levelLimit)
        {
            if (showHint)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_LEVELLIMIT);
            }
            return false;
        }
        return true;
    }

}


public class EquipedLifeSoulDepot
{
    private List<LifeSoulData> mEquipSoulList;
    public List<LifeSoulData> EquipedSoulList
    {
        get
        {
            return mEquipSoulList;
        }
    }
    public EquipedLifeSoulDepot()
    {
        mEquipSoulList = new List<LifeSoulData>();
    }
    public void Serialize(List<fogs.proto.msg.LifeSoul> list, Soldier soldier)
    {
        if (list == null)
        {
            mEquipSoulList.Clear();
        }
        for (int i = 0; i < list.Count; i++)
        {
            fogs.proto.msg.LifeSoul data = list[i];
            if (data == null)
                continue;
            LifeSoulData tmp = new LifeSoulData(data, soldier);
            mEquipSoulList.Add(tmp);
        }
    }

    public void Delete(ulong uid)
    {
        if (mEquipSoulList == null)
            return;
        LifeSoulData data = mEquipSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return uid == tmp.SoulPOD.uid;
        });
        if (data != null)
            mEquipSoulList.Remove(data);
    }

    public void Delete(LifeSoulData data)
    {
        if (mEquipSoulList == null || data == null)
            return;
        LifeSoulData tmpData = mEquipSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return data.SoulPOD.uid == tmp.SoulPOD.uid;
        });
        if (tmpData != null)
            mEquipSoulList.Remove(tmpData);
    }
    public void Delete(List<ulong> uids)
    {
        if (mEquipSoulList == null)
            return;
        for (int i = 0; i < uids.Count; i++)
        {
            ulong uid = uids[i];
            Delete(uid);
        }
    }
    public void Add(LifeSoulData data)
    {
        if (mEquipSoulList == null)
            mEquipSoulList = new List<LifeSoulData>();
        LifeSoulData tmpData = mEquipSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return data.SoulPOD.uid == tmp.SoulPOD.uid;
        });
        if (tmpData == null)
        {
            mEquipSoulList.Add(data);
        }
        else
        {
            tmpData = data;
        }
    }

    public void Update(ulong uid, Soldier soldier)
    {
        LifeSoulData data = PlayerData.Instance._LifeSoulDepot.LifeSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.SoulPOD.uid == uid;
        });
        if (data == null)
            return;
        data.EquipedSoldier = soldier;
    }
}
