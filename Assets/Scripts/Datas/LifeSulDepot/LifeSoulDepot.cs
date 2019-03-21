using System;
using System.Collections.Generic;
using fogs.proto.msg;
using UnityEngine;

public class LifeSoulDepot
{

    public delegate void LifeSoulChangeDelgate();
    public event LifeSoulChangeDelgate LifeSoulChangeEvent;



    private List<LifeSoulData> mLifeSoulList;
    public List<LifeSoulData> LifeSoulList
    {
        get
        {
            return mLifeSoulList;
        }
    }

    public LifeSoulDepot()
    {
        mLifeSoulList = new List<LifeSoulData>();
    }

    public void Add(LifeSoulData data)
    {
        if (data == null)
            return;
        LifeSoulData tmpData = mLifeSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return data.SoulPOD.uid == tmp.SoulPOD.uid;
        });
        if (tmpData == null)
        {
            mLifeSoulList.Add(data);
        }
        else
        {
            tmpData = data;
        }

        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void AddRange(List<LifeSoulData> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            LifeSoulData data = list[i];
            if (data == null)
                continue;
            LifeSoulData tmpData = mLifeSoulList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return data.SoulPOD.uid == tmp.SoulPOD.uid;
            });
            if (tmpData == null)
            {
                mLifeSoulList.Add(data);
            }
            else
            {
                tmpData = data;
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }
    public void Add(fogs.proto.msg.LifeSoul pod)
    {
        if (pod == null)
            return;
        LifeSoulData data = new LifeSoulData(pod);
        LifeSoulData tmpData = mLifeSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return data.SoulPOD.uid == tmp.SoulPOD.uid;
        });
        if (tmpData == null)
        {
            mLifeSoulList.Add(data);
        }
        else
        {
            tmpData = data;
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void Update(fogs.proto.msg.LifeSoul pod)
    {
        if (pod == null)
            return;
        LifeSoulData tmpData = mLifeSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return pod.uid == tmp.SoulPOD.uid;
        });
        if (tmpData != null)
        {
            tmpData.CopyPOD(pod);
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }
    public void AddSoldierEquipedSouls(List<Soldier> soldiers)
    {
        if (soldiers != null)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                Soldier sd = soldiers[i];
                if (sd != null && sd._lifeSoulDepot != null && sd._lifeSoulDepot.EquipedSoulList != null)
                    PlayerData.Instance._LifeSoulDepot.AddRange(sd._lifeSoulDepot.EquipedSoulList);
            }
        }
    }

    public void AddPackLifeSouls(List<fogs.proto.msg.LifeSoul> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            fogs.proto.msg.LifeSoul tmp = list[i];
            if (tmp == null)
                continue;
            LifeSoulData tmpData = mLifeSoulList.Find((tmp2) =>
            {
                if (tmp2 == null)
                    return false;
                return tmp.uid == tmp2.SoulPOD.uid;
            });
            if (tmpData == null)
            {
                LifeSoulData data = new LifeSoulData(tmp);
                mLifeSoulList.Add(data);
            }
            else
            {
                tmpData.CopyPOD(tmp);
                tmpData.EquipedSoldier = null;
                tmpData.IsEquipedPlayer = false;
                tmpData.IsEquipedSoldier = false;
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void AddPlayerLifeSouls(List<fogs.proto.msg.LifeSoul> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            fogs.proto.msg.LifeSoul tmp = list[i];
            if (tmp == null)
                continue;
            LifeSoulData data = new LifeSoulData(tmp, true);
            LifeSoulData tmpData = mLifeSoulList.Find((tmp2) =>
            {
                if (tmp2 == null)
                    return false;
                return data.SoulPOD.uid == tmp2.SoulPOD.uid;
            });
            if (tmpData == null)
            {
                mLifeSoulList.Add(data);
            }
            else
            {
                tmpData = data;
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void DeleteLifeSouls(List<ulong> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            ulong tmp = list[i];
            LifeSoulData data = mLifeSoulList.Find((tmp2) =>
            {
                if (tmp == null)
                    return false;
                return tmp2.SoulPOD.uid == tmp;
            });
            if (data == null)
                return;
            mLifeSoulList.Remove(data);
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public LifeSoulData GetLifeSoulDataByUID(ulong uid)
    {
        LifeSoulData data = mLifeSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.SoulPOD.uid == uid;
        });
        return data;
    }

    public void DeleteLifeSoul(ulong uid)
    {
        if (mLifeSoulList == null)
            return;
        LifeSoulData data = mLifeSoulList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.SoulPOD.uid == uid;
        });
        if (data == null)
            return;
        mLifeSoulList.Remove(data);
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void TakeOffPlayerLifeSoul(List<ulong> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            ulong uid = list[i];
            for (int j = 0; j < mLifeSoulList.Count; j++)
            {
                LifeSoulData data = mLifeSoulList[j];
                if (data == null)
                    continue;
                if (data.SoulPOD.uid == uid)
                {
                    data.IsEquipedPlayer = false;
                    break;
                }
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void AddPlayerLifeSouls(List<EquipedSoul> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            EquipedSoul soul = list[i];
            if (soul == null)
                continue;
            LifeSoulData data = mLifeSoulList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulPOD.uid == soul.uid;
            });
            if (data == null)  //正常来说 不存在找不到数据的情况 
            {
                break;
            }
            else
            {
                data.IsEquipedPlayer = true;
                data.SoulPOD.position = soul.position;
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void TakeOffSoldierLifeSoul(List<ulong> list)
    {
        if (list == null)
            return;
        for (int i = 0; i < list.Count; i++)
        {
            ulong uid = list[i];
            for (int j = 0; j < mLifeSoulList.Count; j++)
            {
                LifeSoulData data = mLifeSoulList[j];
                if (data == null)
                    continue;
                if (data.SoulPOD.uid == uid)
                {
                    data.IsEquipedSoldier = false;
                    data.EquipedSoldier = null;
                    break;
                }
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    private void UpdateSoldierLifeSoul(List<EquipedSoul> list, fogs.proto.msg.Soldier soldier, fogs.proto.msg.Attribute att, ulong soldierUID, List<ulong> takeoffList, List<ulong> self_takeoffList)
    {
        if (list == null)
            return;
        Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(soldierUID);
        if (tmpSoldier == null)
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            EquipedSoul soul = list[i];
            if (soul == null)
                continue;
            LifeSoulData data = mLifeSoulList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulPOD.uid == soul.uid;
            });
            if (data == null)  //正常来说 不存在找不到数据的情况 
            {
                break;
            }
            else
            {
                data.IsEquipedSoldier = true;
                data.SoulPOD.position = soul.position;
                data.EquipedSoldier = tmpSoldier;
                if (data.EquipedSoldier == null)
                    break;
                data.EquipedSoldier.SerializeShowInfo(att);
                data.EquipedSoldier._lifeSoulDepot.Update(soul.uid, data.EquipedSoldier);
            }
        }
        if (soldier != null)
        {
            Soldier reduceSoldier = PlayerData.Instance._SoldierDepot.FindByUid(soldier.uid);
            if (reduceSoldier == null)
                return;
            reduceSoldier.Serialize(soldier);
            for (int i = 0; i < takeoffList.Count; i++)
            {
                ulong uid = takeoffList[i];
                reduceSoldier._lifeSoulDepot.Delete(uid);
                tmpSoldier._lifeSoulDepot.Delete(uid);
            }
        }

        if (self_takeoffList != null)
        {
            for (int i = 0; i < self_takeoffList.Count; i++)
            {
                ulong uid = self_takeoffList[i];
                for (int j = 0; j < mLifeSoulList.Count; j++)
                {
                    LifeSoulData data = mLifeSoulList[j];
                    if (data == null)
                        continue;
                    if (data.SoulPOD.uid == uid)
                    {
                        data.IsEquipedSoldier = false;
                        data.EquipedSoldier = null;
                        break;
                    }
                }
            }
        }

        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    private void UpdatePlayerSoldierLifeSoul(List<EquipedSoul> equipedList, List<ulong> selfOffList)
    {
        if (selfOffList != null)
        {
            List<LifeSoulData> playerEquipedList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
            for (int i = 0; i < selfOffList.Count; i++)
            {
                ulong uid = selfOffList[i];
                for (int j = 0; j < playerEquipedList.Count; j++)
                {
                    LifeSoulData data = playerEquipedList[j];
                    if (data == null)
                        continue;
                    if (data.SoulPOD.uid == uid)
                    {
                        data.IsEquipedPlayer = false;
                        break;
                    }
                }
            }
        }
        if (equipedList != null)
        {
            for (int i = 0; i < equipedList.Count; i++)
            {
                EquipedSoul soul = equipedList[i];
                if (soul == null)
                    continue;
                LifeSoulData data = mLifeSoulList.Find((tmp) =>
                {
                    if (tmp == null)
                        return false;
                    return tmp.SoulPOD.uid == soul.uid;
                });
                if (data == null)  //正常来说 不存在找不到数据的情况 
                {
                    break;
                }
                else
                {
                    data.IsEquipedPlayer = true;
                    data.SoulPOD.position = soul.position;
                }
            }
        }
        if (LifeSoulChangeEvent != null)
            LifeSoulChangeEvent();
    }

    public void PutOnSoulResp(PutOnSoulResp resp)
    {
        if (resp == null)
            return;
        if (resp.c_uid == 0)
        {
            PlayerData.Instance.UpdatePlayerAttribute(resp.attr);
            UpdatePlayerSoldierLifeSoul(resp.soul_list, resp.self_off_uid);
        }
        else
        {
            UpdateSoldierLifeSoul(resp.soul_list, resp.victim, resp.attr, resp.c_uid, resp.victim_off_uid, resp.self_off_uid);
            //TakeOffSoldierLifeSoul(resp.off_soul_uid);
        }
    }



    public List<LifeSoulData> GetPlayerEquipedLifeSoul()
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.IsEquipedPlayer)
            {
                list.Add(data);
            }
        }
        return list;
    }


    public List<LifeSoulData> GetSoldierLifeSoulByUID(ulong soldierUID)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.IsEquipedSoldier)
            {
                if ((data.EquipedSoldier != null) && (data.EquipedSoldier.uId == soldierUID))
                {
                    list.Add(data);
                }
            }
        }
        return list;
    }



    public List<LifeSoulData> GetAllSoldiersLifeSoul()
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.SoulInfo.godEquip == 2 || data.SoulInfo.godEquip == 0)
            {
                list.Add(data);
            }
        }
        return list;
    }

    public List<LifeSoulData> GetAllSoldiersLifeSoulExcept(ulong uid)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.SoulPOD.uid == uid)
                continue;
            if (data.SoulInfo.godEquip == 2 || data.SoulInfo.godEquip == 0)
            {
                list.Add(data);
            }
        }
        return list;
    }

    public List<LifeSoulData> GetAllLifeSoulsExcept(ulong uid)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.IsEquipedPlayer || data.IsEquipedSoldier)
                continue;
            if (data.SoulPOD.uid != uid)
            {
                list.Add(data);
            }
        }
        return list;
    }



    public List<LifeSoulData> GetAllPlayersLifeSoul()
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.SoulInfo.godEquip == 1 || data.SoulInfo.godEquip == 0)
            {
                list.Add(data);
            }
        }
        return list;
    }
    public List<LifeSoulData> GetAllPlayersLifeSoulExcept(ulong uid)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.SoulPOD.uid == uid)
                continue;
            if (data.SoulInfo.godEquip == 1 || data.SoulInfo.godEquip == 0)
            {
                list.Add(data);
            }
        }
        return list;
    }


    public List<LifeSoulData> GetPackLifeSoul()
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (!data.IsEquipedSoldier && !data.IsEquipedPlayer)
            {
                list.Add(data);
            }
        }
        return list;
    }

    public List<LifeSoulData> GetAllExchangeLifeSouls(bool isPlayer)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.IsEquipedPlayer || data.IsEquipedSoldier)
                continue;
            if (isPlayer)
            {
                if (data.SoulInfo.godEquip == 0 || data.SoulInfo.godEquip == 1)
                {
                    list.Add(data);
                }
            }
            else
            {
                if (data.SoulInfo.godEquip == 0 || data.SoulInfo.godEquip == 2)
                {
                    list.Add(data);
                }
            }
        }
        return list;
    }

    public List<LifeSoulData> GetAllExchangeLifeSoulsExcept(bool isPlayer, ulong uid)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.IsEquipedPlayer || data.IsEquipedSoldier)
                continue;
            if (data.SoulPOD.uid == uid)
                continue;
            if (isPlayer)
            {
                if (data.SoulInfo.godEquip == 0 || data.SoulInfo.godEquip == 1)
                {
                    list.Add(data);
                }
            }
            else
            {
                if (data.SoulInfo.godEquip == 0 || data.SoulInfo.godEquip == 2)
                {
                    list.Add(data);
                }
            }
        }
        return list;
    }


    public bool IsPackNotFull(bool showHint = true)
    {
        if (PlayerData.Instance._LifeSoulDepot.GetPackLifeSoul().Count >= PlayerData.Instance._PreyLifeSoulInfo.grid_num)
        {
            if (showHint)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_LIFESPIRIT_COUNTFULL);
            return false;
        }
        return true;
    }

    public bool IsPackWillFull(int count, bool showHint = true)
    {
        if (PlayerData.Instance._LifeSoulDepot == null || PlayerData.Instance._PreyLifeSoulInfo == null)
            return true;

        if ((PlayerData.Instance._LifeSoulDepot.GetPackLifeSoul().Count + count) > PlayerData.Instance._PreyLifeSoulInfo.grid_num)
        {
            if (showHint)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_LIFESPIRIT_COUNTWILLFULL);
            return false;
        }
        return true;
    }

    public int GetLifeSoulCountByID(int id)
    {
        int count = 0;
        if (mLifeSoulList == null || mLifeSoulList.Count <= 0)
            return count;
        for (int i = 0; i < mLifeSoulList.Count; i++)
        {
            LifeSoulData data = mLifeSoulList[i];
            if (data == null)
                continue;
            if (data.SoulInfo.id == id)
                count++;
        }
        return count;
    }

    public int IsSoldierEequpedLifeSoul(ulong soldierUID)
    {
        List<LifeSoulData> list = GetSoldierLifeSoulByUID(soldierUID);
        if (list == null || list.Count <= 0)
            return 0;
        return list.Count;
    }


    public void Clear()
    {
        if (mLifeSoulList != null)
            mLifeSoulList.Clear();
    }
}
