using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class SoldierModule : Singleton<SoldierModule>
{
    public SoldierNetWork mSoldierNetWork;
    public void Initialize()
    {
        if (mSoldierNetWork == null)
        {
            mSoldierNetWork = new SoldierNetWork();
            mSoldierNetWork.RegisterMsg();
        }
    }
    public void Uninitialize()
    {
        if (mSoldierNetWork != null)
            mSoldierNetWork.RemoveMsg();
        mSoldierNetWork = null;
    }
    public void SendSoldierUpLvReq(UInt64 uId, List<UInt64> material)
    {
        SoldierUpLvReq temp = new SoldierUpLvReq();
        temp.uid = uId;
        temp.uid_list.AddRange(material);
        mSoldierNetWork.SendSoldierUpLvReq(temp);
    }
    public void ReceiveSoldierUpLvReq(Packet data)
    {
        SoldierUpLvResp tData = Serializer.Deserialize<SoldierUpLvResp>(data.ms);
        if (tData.result == 0)
        {
            PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(tData.soul_return_item);
            PlayerData.Instance._SoldierDepot.ReceiveSoldierUpLvReq(tData);
            PlayerData.Instance.UpdateItem(tData.skill_return_item);
            List<CommonItemData> list = new List<CommonItemData>(tData.skill_return_item.Count + 1);
            foreach (fogs.proto.msg.ItemInfo temp in tData.skill_return_item)
            {
                ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(temp.id);
                CommonItemData itemData = new CommonItemData(tmpInfo);
                itemData.Num = temp.change_num;
                list.Add(itemData);
            }
            if (list.Count > 0)
            {
                UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
                UISystem.Instance.RecieveResultVertView.UpdateSkillBookInfo(list);
            }
            PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();

            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.STRENGTH_SUCESS);

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierDepot.OnSoldierErrorEvent(SoldierControl.SoldierUpLvResq, tData.result);
    }
    public void SendSoldierSelectReq(List<UInt64> material)
    {
        SoldierSelectReq temp = new SoldierSelectReq();
        temp.uid_list.AddRange(material);
        mSoldierNetWork.SendSoldierSelectReq(temp);
    }
    public void ReceiveSoldierSelectResp(Packet data)
    {
        SoldierSelectResp tData = Serializer.Deserialize<SoldierSelectResp>(data.ms);
        if (tData.result == 0)
        {
            PlayerData.Instance._SoldierDepot.ReceiveSoldierSelectResp(tData);
            PlayerData.Instance.UpdateItem(tData.skill_return_item);
            List<CommonItemData> list = new List<CommonItemData>(tData.skill_return_item.Count + 1);
            foreach (fogs.proto.msg.ItemInfo temp in tData.skill_return_item)
            {
                ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(temp.id);
                CommonItemData itemData = new CommonItemData(tmpInfo);
                list.Add(itemData);
            }
            if (list.Count > 0)
            {
                UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
                UISystem.Instance.RecieveResultVertView.UpdateSkillBookInfo(list);
            }

            PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            PlayerData.Instance._SoldierDepot.OnSoldierErrorEvent(SoldierControl.SoldierSelectResp, tData.result, tData.soldier_info.uid);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
            PlayerData.Instance._SoldierDepot.OnSoldierErrorEvent(SoldierControl.SoldierSelectResp, tData.result);
        }
    }
    public void SendSoldierUpStepReq(UInt64 vSoldier, List<UInt64> vMaterial, List<UInt64> vMaterialList)
    {
        SoldierUpStepReq temp = new SoldierUpStepReq();
        temp.uid = vSoldier;
        temp.material_uid.AddRange(vMaterial);
        temp.material_uid_list.AddRange(vMaterialList);
        mSoldierNetWork.SendSoldierUpStepReq(temp);
    }

    public void ReceiveSoldierUpStepResp(Packet data)
    {
        SoldierUpStepResp tData = Serializer.Deserialize<SoldierUpStepResp>(data.ms);
        if (tData.result == 0)
        {
            PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(tData.soul_return_item);
            PlayerData.Instance._SoldierDepot.multipleAdd(tData.return_soldiers);
            PlayerData.Instance._SoldierDepot.ReceiveSoldierUpStepResp(tData);
            PlayerData.Instance.UpdateItem(tData.skill_return_item);
            List<CommonItemData> list = new List<CommonItemData>(tData.skill_return_item.Count + 1);
            foreach (fogs.proto.msg.ItemInfo temp in tData.skill_return_item)
            {
                ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(temp.id);
                CommonItemData itemData = new CommonItemData(tmpInfo);
                itemData.Num = temp.change_num;

                list.Add(itemData);
            }
            if (list.Count > 0)
            {
                Main.Instance.StartCoroutine(OpenRecieveResultVertView(3F, list));
            }
            PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            if(tData.skill_return_item.Count > 0 || tData.soul_return_item.Count > 0 || tData.equip_list.Count > 0)
            {
                if (tData.return_soldiers.Count > 0)
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR22);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTEP9);
                }
                else
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTEP8);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTEP9);
                }
            }
            else
            {
                if (tData.return_soldiers.Count > 0)
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR20);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR21);
                }
                else
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTAR14);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR21);

                }
            }

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierDepot.OnSoldierErrorEvent(SoldierControl.SoldierUpStarResp, tData.result);
    }
    private IEnumerator OpenRecieveResultVertView(float time, List<CommonItemData> data)
    {
        yield return new WaitForSeconds(time);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateSkillBookInfo(data);

    }
    public void ReceiveGetNewSoldier(Packet data)
    {
        NotifyGetNewSoldier tData = Serializer.Deserialize<NotifyGetNewSoldier>(data.ms);

        PlayerData.Instance._SoldierMap.OneDelete(tData.soldier_id);
    }
    public void SendSoldierUpLevelStarReq(UInt64 vSoldier, List<UInt64> vMaterial, List<UInt64> vMaterialList)
    {
        SoldierUpLevelStarReq tmpData = new SoldierUpLevelStarReq();
        tmpData.uid = vSoldier;
        tmpData.material_uid.AddRange(vMaterial);
        tmpData.material_uid_list.AddRange(vMaterialList);
        mSoldierNetWork.SendSoldierUpLevelStarReq(tmpData);
    }
    public void ReceiveSoldierUpLevelStarResq(Packet data)
    {
        SoldierUpLevelStarResp tData = Serializer.Deserialize<SoldierUpLevelStarResp>(data.ms);
        if (tData.result == 0)
        {
            PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(tData.soul_return_item);
            UISystem.Instance.SoldierAttView.centerPanel.soldierUpStar.ShowEffect();
            PlayerData.Instance._SoldierDepot.ReceiveSoldierUpLevelStarResq(tData);

            PlayerData.Instance.UpdateItem(tData.skill_return_item);
            PlayerData.Instance._SoldierDepot.multipleAdd(tData.return_soldiers);
            List<CommonItemData> list = new List<CommonItemData>(tData.skill_return_item.Count + 1);
            foreach (fogs.proto.msg.ItemInfo temp in tData.skill_return_item)
            {
                ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(temp.id);
                CommonItemData itemData = new CommonItemData(tmpInfo);
                itemData.Num = temp.change_num;

                list.Add(itemData);
            }

            if (list.Count > 0)
            {
                //UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
                //UISystem.Instance.RecieveResultVertView.UpdateSkillBookInfo(list);
                Main.Instance.StartCoroutine(OpenRecieveResultVertView(3F, list));
            }
            PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            if (tData.skill_return_item.Count > 0 || tData.soul_return_item.Count > 0 || tData.equip_list.Count > 0)
            {
                if (tData.return_soldiers.Count > 0)
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR23);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTEP11);
                }
                else
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTEP10);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTEP11);
                }
            }
            else
            {
                if (tData.return_soldiers.Count > 0)
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR20);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR21);
                }
                else
                {
                    if (tData.return_overflow == 0)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIERUPSTAR15);
                    else
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDUERUPSTAR21);
                }
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierDepot.OnSoldierErrorEvent(SoldierControl.SoldierUpStarResp, tData.result);
    }
    public void SendSoldierUpQualityReq(List<UInt64> material, SacrificialDataEnum type)
    {
        SoldierUpQualityReq tmpInfo = new SoldierUpQualityReq();
        tmpInfo.uid_list.AddRange(material);
        if (type == SacrificialDataEnum.SoldierOrange)
            tmpInfo.type = SoldierUpQualityType.Soldier_ORA;
        else
            tmpInfo.type = SoldierUpQualityType.Soldier_RED;
        mSoldierNetWork.SendSoldierUpQualityReq(tmpInfo);
    }
    public void ReceiveSoldierUpQualityResp(Packet data)
    {
        SoldierUpQualityResp tData = Serializer.Deserialize<SoldierUpQualityResp>(data.ms);
        if (tData.result == 0)
        {
            PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(tData.soul_return_item);
            if (tData.type == SoldierUpQualityType.Soldier_RED)
                PlayerData.Instance.Sacrificial_Soldier_EXP = tData.soldier_quality_value;
            else
                PlayerData.Instance.Sacrificial_Ora_Soldier_EXP = tData.soldier_quality_value;

            PlayerData.Instance._SoldierDepot.multipleDelete(tData.uid_list);
            PlayerData.Instance.UpdateItem(tData.item_list);
            List<Soldier> tmpSoldier = PlayerData.Instance.MultipleAddSoldier(tData.soldier_list);
            if (tmpSoldier != null && tmpSoldier.Count > 0)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_SACRIFICIALEFFECT);
                UISystem.Instance.SacrificialSystemEffectView.ShowChuanSongMengEffect(tmpSoldier);//神兵特效
                UISystem.Instance.ResortViewOrder();
            }

            foreach (fogs.proto.msg.Equip temp in tData.equip_list)
            {
                Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
                if (wp != null)
                {
                    wp.isEquiped = false;
                }
            }
            PlayerData.Instance._SoldierDepot.SortNow();
            List<CommonItemData> list = new List<CommonItemData>(tData.item_list.Count + 1);
            foreach (fogs.proto.msg.ItemInfo temp in tData.item_list)
            {
                ItemInfo tmpInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(temp.id);
                CommonItemData itemData = new CommonItemData(tmpInfo);
                itemData.Num = temp.change_num;

                list.Add(itemData);
            }
            if (list.Count > 0)
            {
                //UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
                //UISystem.Instance.RecieveResultVertView.UpdateSkillBookInfo(list);
                Main.Instance.StartCoroutine(OpenRecieveResultVertView(3F, list));
            }
            SacrificialDataEnum saType = SacrificialDataEnum.SoldierRed;
            if (tData.type == SoldierUpQualityType.Soldier_RED)
                saType = SacrificialDataEnum.SoldierRed;
            else
                saType = SacrificialDataEnum.SoldierOrange;
            SacrificialSystemInfo tmpSaInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)saType);
            PlayerData.Instance.MoneyRefresh(tmpSaInfo.money_type, tData.gold_num);
            if (UISystem.Instance.TopFuncView != null)
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        SacrificialDataEnum result = SacrificialDataEnum.SoldierRed;
        if (tData.type == SoldierUpQualityType.Soldier_RED)
            result = SacrificialDataEnum.SoldierRed;
        else
            result = SacrificialDataEnum.SoldierOrange;
        PlayerData.Instance._SoldierDepot.OnSoldierErrorEvent(SoldierControl.SoldierUpQualityResp, (int)tData.result, (uint)result);
    }

}
