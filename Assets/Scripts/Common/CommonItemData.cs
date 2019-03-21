using UnityEngine;
using System.Collections.Generic;
using System;
public class CommonItemData : IComparable
{
    public UInt64 UID;
    public uint ID;
    public int Num;
    public string Name;
    public string Icon;
    public ItemQualityEnum Quality;
    public string Desc;
    public IDType Type;
    public ItemTypeEnum SubType;
    public int LifeSoulGodEquip;
    public int Level;
    public int StarLv;
    public int Step;
    public string SuitName;
    public MoneyFlowData RecyclePrice;
    public CommonItemData()
    {

    }
    public CommonItemData(Soldier info)
    {
        SetValue(info);
    }
    public CommonItemData(Weapon info)
    {
        SetValue(info);
    }
    public CommonItemData(LifeSoulData info)
    {
        SetValue(info);
    }
    public CommonItemData(EquipAttributeInfo info)
    {
        SetValue(info);
    }
    public CommonItemData(ItemInfo info)
    {
        SetValue(info);
    }
    public CommonItemData(SoldierAttributeInfo info)
    {
        SetValue(info);
    }

    public CommonItemData(CombatPetInfo info) 
    {
        SetValue(info);
    }

    public void SetValue(Soldier info)
    {
        SetValue(info.Att);
        UID = info.uId;
        Level = info.Level;
        StarLv = info.Att.Star;
        Step = info.StepNum;        
        RecyclePrice.Number = info.Att.recyclePrice.Number * info.StarWorth();
    }
    public void SetValue(Weapon info)
    {
        SetValue(info.Att);
        UID = info.uId;
        Level = info.Level;
        StarLv = info.Att.star;
    }
    public void SetValue(LifeSoulData info)
    {
        SetValue(info.SoulInfo);
        UID = info.SoulPOD.uid;
        Level = info.SoulPOD.level;
        StarLv = 0;
    }
    private void SetValue(EquipAttributeInfo info)
    {
        if (info == null)
        {
            Debug.LogError(string.Format("commonitemdata error {0}", ID == 0 ? "" : ID.ToString()));
            return;
        }
        ID = info.id;
        Name = info.name;
        Icon = info.icon;
        Quality = (ItemQualityEnum)info.quality;
        Desc = info.descript;
        Type = IDType.EQ;
        SubType = ItemTypeEnum.None;
        LifeSoulGodEquip = 0;
        if (RecyclePrice == null)
            RecyclePrice = new MoneyFlowData();
        RecyclePrice.CopyValue(info.recyclePrice);
        Step = 0;
        if (info.CoordID == 0)
        {
            SuitName = string.Empty;
        }
        else
        {
            EquipCoordinatesInfo tmp = ConfigManager.Instance.mEquipCoordinatesConfig.GetEquipCoordinatesInfoByID(info.CoordID);
            if (tmp == null)
            {
                SuitName = string.Empty;
            }
            else
            {
                SuitName = tmp.name;
            }

        }

    }

    public void SetValue(ItemInfo info)
    {
        if (info == null)
        {
            Debug.LogError(string.Format("commonitemdata error {0}", ID == 0 ? "" : ID.ToString()));
            return;
        }
        ID = info.id;
        Name = info.name;
        Icon = info.icon;
        Quality = (ItemQualityEnum)info.quality;
        Desc = info.desc;
        Type = IDType.Prop;
        SubType = (ItemTypeEnum)info.type;
        LifeSoulGodEquip = 0;
        if (RecyclePrice == null)
            RecyclePrice = new MoneyFlowData();
        RecyclePrice.CopyValue(info.recyclePrice);
        Level = 1;
        StarLv = 0;
        Step = 0;
    }

    private void SetValue(SoldierAttributeInfo info)
    {
        if (info == null)
        {
            Debug.LogError(string.Format("commonitemdata error {0}", ID == 0 ? "" : ID.ToString()));
            return;
        }
        ID = info.id;
        Name = info.Name;
        Icon = info.Icon;
        Quality = (ItemQualityEnum)info.quality;
        Desc = info.SoldierPos;
        Type = IDType.Soldier;
        SubType = ItemTypeEnum.None;
        LifeSoulGodEquip = 0;
        if (RecyclePrice == null)
            RecyclePrice = new MoneyFlowData();
        RecyclePrice.CopyValue(info.recyclePrice);
    }

    private void SetValue(LifeSoulConfigInfo info)
    {
        if(info == null)
        {
            Debug.LogError(string.Format("commonitemdata error {0}", ID == 0 ? "" : ID.ToString()));
            return;
        }
        ID = info.id;
        Name = info.name;
        Icon = info.icon;
        Quality = (ItemQualityEnum)info.quality;
        Desc = info.desc;
        Type = IDType.LifeSoul;
        SubType = ItemTypeEnum.None;
        LifeSoulGodEquip = info.godEquip;
        if (RecyclePrice == null)
            RecyclePrice = new MoneyFlowData();
        RecyclePrice.CopyValue(info.recyclePrice);
        Step = 0;
    }

    private void SetValue(CombatPetInfo info) 
    {
        if (info == null)
        {
            Debug.LogError(string.Format("commonitemdata error {0}", ID == 0 ? "" : ID.ToString()));
            return;
        }
        ID = info.id;
        Name = info.name;
        Icon = info.icon;
        Quality = (ItemQualityEnum)info.quality;
        Desc = info.pet_desc;
        Type = IDType.Pet;
        SubType = ItemTypeEnum.None;
    }

    public void SetValue(uint id, int num, bool completeData = false, bool setNumByPlayerData = false)
    {
        ID = id;
        Num = num;
        if (!completeData)
            return;
        Level = 1;
        StarLv = 0;
        LifeSoulGodEquip = 0;
        IDType type = CommonFunction.GetTypeOfID(id.ToString());
        Type = type;
        switch (type)
        {
            case IDType.Gold:
                {
                    Name = ConstString.NAME_GOLD;
                    Icon = GlobalConst.SpriteName.Gold;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_GOLD;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._Gold;
                    break;
                }
            case IDType.Diamond:
                {
                    Name = ConstString.NAME_DIAMOND;
                    Icon = GlobalConst.SpriteName.Diamond;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_DIAMOND;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._Diamonds;
                    break;
                }
            case IDType.SP:
                {
                    Name = ConstString.NAME_SP;
                    Icon = GlobalConst.SpriteName.SP;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_SP;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._Physical;
                    break;
                }
            case IDType.Honor:
                {
                    Name = ConstString.NAME_HONOR;
                    Icon = GlobalConst.SpriteName.Honor;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_HONOR;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._Honor;
                    break;
                }
            case IDType.Medal:
                {
                    Name = ConstString.NAME_MEDAL;
                    Icon = GlobalConst.SpriteName.Medal;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_MEDAL;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._Medal;
                    break;
                }
            case IDType.Prop:
                {
                    SetValue(ConfigManager.Instance.mItemData.GetItemInfoByID(id));
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance.GetItemCountByID(id);
                    break;
                }
            case IDType.Soldier:
                {
                    SetValue(ConfigManager.Instance.mSoldierData.FindById(id));
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._SoldierDepot.FindById(id).Count;
                    break;
                }
            case IDType.EQ:
                {
                    SetValue(ConfigManager.Instance.mEquipData.FindById(id));
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance.GetEquipCount(id);
                    break;
                }
            case IDType.UnionToken:
                {
                    Name = ConstString.NAME_UNIONTOKEN;
                    Icon = GlobalConst.SpriteName.UnionToken;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_UNIONTOKEN;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance.UnionToken;
                    break;
                }
            case IDType.RecycleCoin:
                {
                    Name = ConstString.NAME_RECYCLECOIN;
                    Icon = GlobalConst.SpriteName.RecycleCoin;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_RECYCLECOIN;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance.RecycleCoin;
                    break;
                }
            case IDType.Exp:
                {
                    Name = ConstString.NAME_EXP;
                    Icon = GlobalConst.SpriteName.HeroExp;
                    Quality = ItemQualityEnum.White;
                    Desc = ConstString.DESC_EXP;
                    SubType = ItemTypeEnum.None;
                    if (setNumByPlayerData)
                        Num = (int)PlayerData.Instance._CurrentExp;
                    break;
                }
            case IDType.LifeSoul:
                {
                    SetValue(ConfigManager.Instance.mLifeSoulConfig.GetLifeDataByID((int)id));
                    if (setNumByPlayerData)
                        Num = PlayerData.Instance._LifeSoulDepot.GetLifeSoulCountByID((int)id);
                    break;
                }
            case IDType.Pet:
                {
                    SetValue(ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID((uint)id));
                    if (setNumByPlayerData)
                    {
                        Num = 0;
                        PetData pet =  PlayerData.Instance._PetDepot.GetPetDataByID((uint)id);
                        if(pet != null)
                            Num = 1;

                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="num"></param>
    /// <param name="completeData">是否是完整的物品信息</param>
    /// <param name="setNumByPlayerData">是否从playerdata里面获取物品个数</param>
    public CommonItemData(uint id, int num, bool completeData = false, bool setNumByPlayerData = false)
    {
        SetValue(id, num, completeData, setNumByPlayerData);
    }

    public CommonItemData(uint id, string name, ItemQualityEnum quality, int num, string icon, IDType type, string desc, int lifeSoulGodEquip = 0)
    {
        ID = id;
        Num = num;
        Name = name;
        Icon = icon;
        Type = type;
        Desc = desc;
        SubType = ItemTypeEnum.None;
        Quality = quality;
        LifeSoulGodEquip = lifeSoulGodEquip;
    }

    public int CompareTo(object obj)
    {
        int result = 0;
        try
        {
            CommonItemData item = obj as CommonItemData;

            if (this.Quality > item.Quality)
            {
                result = -1;
            }
            else if (this.Quality < item.Quality)
            {
                result = 1;
            }
            else
            {
                result = this.ID.CompareTo(item.ID);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        return result;
    }

}


public class CommonRuleAwardItem
{
    public string Rank;

    public List<CommonItemData> Awards;

    public CommonRuleAwardItem(string rank, List<CommonItemData> list)
    {
        Rank = rank;
        Awards = list;
    }
}