using System;
using System.Collections.Generic;
using UnityEngine;
public class PetDepot
{
    public delegate void PetAddDelegate();
    public event PetAddDelegate PetAddEvent;
    private List<PetData> mPetList;

    public void Init(List<fogs.proto.msg.Pet> list)
    {
        mPetList = new List<PetData>();
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                fogs.proto.msg.Pet pod = list[i];
                if (pod == null)
                    continue;
                PetData data = new PetData();
                data.Init(pod);
                mPetList.Add(data);
            }
        }

        List<CombatPetInfo> infos = ConfigManager.Instance.mCombatPetsConfig.GetInitPetInfos();
        for (int i = 0; i < infos.Count; i++)
        {
            CombatPetInfo info = infos[i];
            if (info == null)
                continue;
            PetData data = mPetList.Find((tmp) =>
            {
                if (tmp == null || tmp.PetInfo == null)
                    return false;
                return tmp.PetInfo.PetID == info.PetID;
            });
            if (data == null)  //为空 则说明未拥有
            {
                data = new PetData();
                data.Init(null, info);
                mPetList.Add(data);
            }
        }
        SortPet();
        UpdatePetStatus();
    }

    public void UpdatePetStatus()
    {
        if (mPetList == null || mPetList.Count == 0)
            return;
        List<uint> newPetList = null;
        if (PlayerPrefsTool.HasKey(AppPrefEnum.NewPet))
        {
            newPetList = PlayerPrefsTool.ReadObject<List<uint>>(AppPrefEnum.NewPet);
        }
        if (newPetList == null)
            newPetList = new List<uint>();
        for (int i = 0; i < mPetList.Count; i++)
        {
            PetData petData = mPetList[i];
            if (petData == null)
                continue;
            if (newPetList.Contains(petData.PetInfo.PetID))
            {
                petData.IsNew = true;
            }
            else
            {
                petData.IsNew = false;
            }
        }

        if (PetAddEvent != null)
            PetAddEvent();
    }

    public void SortPet()
    {
        mPetList.Sort((left, right) =>
        {
            if (left == null || right == null)
                return 0;
            if (left.IsOwned != right.IsOwned)
            {
                if (left.IsOwned)
                    return -1;
                else
                {
                    return 1;
                }
            }
            if (left.IsEquiped != right.IsEquiped)
            {
                if (left.IsEquiped && !right.IsEquiped)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            if (left.Level != right.Level)
            {
                if (left.Level >= right.Level)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            if (left.PetInfo.quality != right.PetInfo.quality)
            {
                if (left.PetInfo.quality > right.PetInfo.quality)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            if (left.PetInfo.id != right.PetInfo.id)
            {
                if (left.PetInfo.id < right.PetInfo.id)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        });
    }

    /// <summary>
    /// 取得已经拥有的宠物
    /// </summary>
    /// <returns></returns>
    public List<PetData> GetOwnedPets()
    {
        List<PetData> list = mPetList.FindAll((tmp) =>
        {
            if (tmp == null) return false;
            return tmp.IsOwned;
        });
        if (list == null)
            list = new List<PetData>();
        return list;
    }

    /// <summary>
    /// 取得已经装备的宠物
    /// </summary>
    /// <returns></returns>
    public PetData GetEquipedPet()
    {
        PetData data = mPetList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.PetInfo.id == PlayerData.Instance.EquipPetID;
        });
        return data;
    }
    /// <summary>
    /// 取得当前所有宠物数据（包含未拥有的）
    /// </summary>
    /// <returns></returns>
    public List<PetData> GetPetList()
    {
        return mPetList;
    }

    public PetData GetPetDataByID(uint id)
    {
        PetData data = mPetList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.PetInfo.id == id;
        });
        return data;
    }

    public PetData GetPetDataByTypeID(uint petID)
    {
        PetData data = mPetList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.PetInfo.PetID == petID;
        });
        return data;
    }

    public void UpdatePetData(fogs.proto.msg.Pet data, bool isNew = false)
    {
        CombatPetInfo info = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(data.id);
        if (info == null)
            return;
        PetData oldPet = mPetList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.PetInfo.PetID == info.PetID;
        });
        if (oldPet == null)
            return;
        oldPet.Init(data, info);
        oldPet.IsNew = isNew;
        SortPet();
    }

    public void AddPetList(List<fogs.proto.msg.Pet> list)
    {
        if (list == null)
            return;
        List<uint> newPetList = null;
        if (PlayerPrefsTool.HasKey(AppPrefEnum.NewPet))
        {
            newPetList = PlayerPrefsTool.ReadObject<List<uint>>(AppPrefEnum.NewPet);
        }
        if (newPetList == null)
            newPetList = new List<uint>();

        for (int i = 0; i < list.Count; i++)
        {
            fogs.proto.msg.Pet pet = list[i];
            if (pet == null)
                continue;
            CombatPetInfo info = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(pet.id);
            PetData oldPet = mPetList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.PetInfo.PetID == info.PetID;
            });
            if (oldPet == null)
                return;
            if (pet.skill != null && pet.skill.Count > 0)
            {
                oldPet.Init(pet, info);
                newPetList.Add(oldPet.PetInfo.PetID);
            }
        }
        PlayerPrefsTool.WriteObject<List<uint>>(AppPrefEnum.NewPet, newPetList);
        SortPet();
        UpdatePetStatus();
    }

    public void UpdatePetSkill(uint id, List<fogs.proto.msg.Skill> skills)
    {
        PetData data = mPetList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.PetInfo.id == id;
        });
        if (data == null)
            return;

        data.UpdateSkill(skills);
    }

    public void Clear()
    {
        if (mPetList != null)
            mPetList.Clear();
    }
}
