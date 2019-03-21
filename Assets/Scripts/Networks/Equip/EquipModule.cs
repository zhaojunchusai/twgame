using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class EquipModule : Singleton<EquipModule> 
{
    public EquipNetWork mEquipNetWork;
    public void Initialize()
    {
        if (mEquipNetWork == null) 
        {
            mEquipNetWork = new EquipNetWork();
            mEquipNetWork.RegisterMsg();
        }
    }
    
    public void Uninitialize() 
    {
        if (mEquipNetWork != null)
            mEquipNetWork.RemoveMsg();
        mEquipNetWork = null;
    }

    public void SendPutonEquipReq(UInt64 id,UInt64 owner, int pos,UInt64 other_soldierUid)
    {
        PutonEquipReq temp = new PutonEquipReq();
        
        temp.uid = id;
        temp.soldier_uid = owner;
        temp.postion = pos;
        temp.other_soldier_uid = other_soldierUid;
        mEquipNetWork.SendPutonEquipReq(temp);
    }
    public void SendGetoffEquipReq(UInt64 id,UInt64 owner)
    {
        GetoffEquipReq temp = new GetoffEquipReq();
        temp.uid = id;
        temp.soldier_uid = owner;
        mEquipNetWork.SendGetoffEquipReq(temp);
    }
    public void SendPromoteEquipReq(UInt64 id,UInt64 soldierUid, EquipActionType type)
    {
        PromoteEquipReq temp = new PromoteEquipReq();
        temp.uid = id;
        temp.type = type;
        temp.soldier_uid = soldierUid;
        mEquipNetWork.SendPromoteEquipReq(temp);
    }
    public void SendOneKeyPromoteAllReq(UInt64 soldierUid)
    {
        OneKeyPromoteAllReq temp = new OneKeyPromoteAllReq();
        temp.soldier_uid = soldierUid;
        mEquipNetWork.SendOneKeyPromoteAllReq(temp);
    }
    public void SendOneKeyPutOnAllReq(UInt64 soldierUid, List<PutEuipList> pos)
    {
        OneKeyPutOnAllReq temp = new OneKeyPutOnAllReq();
        temp.soldier_uid = soldierUid;
        temp.equip_list.AddRange(pos);
        mEquipNetWork.SendOneKeyPutOnAllReq(temp);
    }
    public void SendSoldierEquipStarReq(UInt64 equipUid,UInt64 soldierUid,List<UInt64> _material)
    {
        SoldierEquipStarReq temp = new SoldierEquipStarReq();
        temp.soldier_uid = soldierUid;
        temp.uid = equipUid;
        temp.uid_list.AddRange(_material);
        mEquipNetWork.SendSoldierEquipStarReq(temp);
    }
    public void SendSellEquipReq(UInt64 uid)
    {
        SellEquipReq temp = new SellEquipReq();
        temp.uid = uid;
        mEquipNetWork.SendSellEquipReq(temp);
    }
    public void SendOneKeyPromoteOneReq(UInt64 equipUid,UInt64 soldierUid)
    {
        OneKeyPromoteOneReq temp = new OneKeyPromoteOneReq();
        temp.equip_uid = equipUid;
        temp.soldier_uid = soldierUid;
        mEquipNetWork.SendOneKeyPromoteOneReq(temp);
    }
    public void ReceiveOneKeyPromoteOneResp(Packet data)
    {
        OneKeyPromoteOneResp tData = Serializer.Deserialize<OneKeyPromoteOneResp>(data.ms);
        if(tData.result == 0)
        {
            PlayerData.Instance._SoldierEquip.ReceiveOneKeyPromoteOneResp(tData);
            Soldier temp = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
            if (temp != null)
                temp.SerializeShowInfo(tData.attr);
            PlayerData.Instance.MoneyRefresh(tData.money_type,tData.money_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ONEKEY_STRONG_SUCCESS);
            UISystem.Instance.SoldierEquipIntensifyView.PlaySoldierEquipEffect();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.OneKeyPromoteOneResp, (int)tData.result);
    }

    public void ReceivemSellEquipResp(Packet data)
    {
        SellEquipResp tData = Serializer.Deserialize<SellEquipResp>(data.ms);
        if(tData.result == 0)
        {
            PlayerData.Instance._SoldierEquip.ReceiveSellEquipResp(tData);
            PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.SellEquipResp, (int)tData.result);
    }
    public void ReceiveSoldierEquipStarResp(Packet data)
    {
        SoldierEquipStarResp tData = Serializer.Deserialize<SoldierEquipStarResp>(data.ms);
        if(tData.result == 0)
        {
            PlayerData.Instance._SoldierEquip.ReceiveSoldierEquipStarResp(tData);
            if (tData.soldier_uid != 0)
            {
                Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
                if (sd != null)
                    sd.SerializeShowInfo(tData.attr);
            }
            UISystem.Instance.SoldierEquipAdvancedView.PlayEquipAdvancedEffect();//装备升星特效 
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.SoldierEquipStarResp, (int)tData.result);
    }
    public void ReceivePutonEquipResp(Packet data)
    {
        if (PlayerData.Instance._WeaponDepot != null)
        {
            PutonEquipResp tData = Serializer.Deserialize<PutonEquipResp>(data.ms);
            
            if (tData.result == 0)
            {
                if(tData.soldier_uid == 0)
                {
                    Weapon temp = PlayerData.Instance._WeaponDepot.FindByUid(tData.equip.uid);
                   
                    if (temp == null) return;
                    PlayerData.Instance.UpdatePlayerAttribute(tData.attr);
                    PlayerData.Instance._ArtifactedDepot.ReceivePutonEquipResp(temp, (int)tData.equip.postion - 1, tData.type, tData.other_soldier_uid, tData.other_equip);
                }
                else
                {
                    Weapon temp = PlayerData.Instance._SoldierEquip.FindByUid(tData.equip.uid);
                    if (temp == null) return;

                    Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
                    if (sd == null) return;
                    sd.SerializeShowInfo(tData.attr);

                    if(tData.other_soldier_uid != 0)
                    {
                        Soldier other = PlayerData.Instance._SoldierDepot.FindByUid(tData.other_soldier_uid);
                        if(other != null)
                        {
                            other.SerializeShowInfo(tData.other_attr);
                        }
                    }

                    sd._equippedDepot.ReceivePutonEquipResp(temp, (int)tData.equip.postion - 1,tData.type,tData.other_soldier_uid,tData.other_equip);
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(tData.result);
            }
            if (tData.soldier_uid == 0)
                PlayerData.Instance._WeaponDepot.OnErrotChange(EquipControl.PutonEquipAndArtifactResp, (int)tData.result);
            else
                PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.PutonEquipAndArtifactResp, (int)tData.result);
        }
    }
    public void ReceiveGetoffEquipResp(Packet data)
    {
        if (PlayerData.Instance._WeaponDepot != null)
        {
            GetoffEquipResp tData = Serializer.Deserialize<GetoffEquipResp>(data.ms);

            if(tData.result == 0)
            {
                if(tData.soldier_uid == 0)
                {
                    PlayerData.Instance.UpdatePlayerAttribute(tData.attr);
                    PlayerData.Instance._ArtifactedDepot.ReceiveGetoffEquipResp(tData.uid);
                }
                else
                {
                    Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
                    if (sd == null) return;
                    sd.SerializeShowInfo(tData.attr);
                    sd._equippedDepot.ReceiveGetoffEquipResp(tData.uid);
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(tData.result);
            }
            if (tData.soldier_uid == 0)
                PlayerData.Instance._WeaponDepot.OnErrotChange(EquipControl.GetoffEquipAndArtifactResp, (int)tData.result);
            else
                PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.GetoffEquipAndArtifactResp, (int)tData.result);

        }
    }
    public void ReceivePromoteEquipResp(Packet data)
    {
        if (PlayerData.Instance._WeaponDepot != null)
        {
            PromoteEquipResp tData = Serializer.Deserialize<PromoteEquipResp>(data.ms);
            if (tData.result == 0)
            {
                Weapon tp = PlayerData.Instance._WeaponDepot.FindByUid(tData.uid);
                if (tp != null)
                {
                    PlayerData.Instance._WeaponDepot.ReceivePromoteEquipResp(tData);
                    UISystem.Instance.ArtifactIntensifyView.IntensifyEffect();//播放英雄界面神器强化特效
                }
                else
                { 
                    PlayerData.Instance._SoldierEquip.ReceivePromoteEquipResp(tData);
                    UISystem.Instance.SoldierEquipIntensifyView.PlaySoldierEquipEffect();
                }
                if (tData.soldier_uid == 0)
                {
                   PlayerData.Instance.UpdatePlayerAttribute(tData.attr);
                }
                else
                {
                    if(tData.soldier_uid != 1)
                    {
                        Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
                        if (sd != null)
                            sd.SerializeShowInfo(tData.attr);
                    }
                }
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.STRENGTH_SUCESS);
                PlayerData.Instance.UpdateItem(tData.item_list);
                UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
                //PlayerData.Instance.MoneyRefresh((int)tData.money_type, (int)tData.money);
                //if (UISystem.Instance.TopFuncView != null)
                //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
               // UISystem.Instance.SoldierEquipIntensifyView.PlaySoldierEquipEffect();
            }
            else
            {
                ErrorCode.ShowErrorTip(tData.result);
            }
            Weapon tpWeapon = PlayerData.Instance._WeaponDepot.FindByUid(tData.uid);
            if (tpWeapon != null)
            {
                PlayerData.Instance._WeaponDepot.OnErrotChange(EquipControl.PromoteEquipAndArtifactResp, (int)tData.result); ;
            }
            else
            {
                PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.PromoteEquipAndArtifactResp, (int)tData.result);
            }
        }
    }
    public void ReceiveOneKeyPutOnAllResp(Packet data)
    {
        OneKeyPutOnAllResp tData = Serializer.Deserialize<OneKeyPutOnAllResp>(data.ms);
        if(tData.result == 0)
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
            if (sd == null) UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_NOTFOUND);
            sd.SerializeShowInfo(tData.attr);
            sd._equippedDepot.ReceiveOneKeyPutOnAllResp(tData);
            //if (tData.equip_list != null && tData.equip_list.Count > 0)
            //UISystem.Instance.SoldierAttView.PlayFastStrengthEffect(tData.equip_list.Count);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.OneKeyPutOnAllEquipResp, (int)tData.result);
    }
    public void ReceiveOneKeyPromoteAllResp(Packet data)
    {
        OneKeyPromoteAllResp tData = Serializer.Deserialize<OneKeyPromoteAllResp>(data.ms);
        if (tData.result == 0)
        {
            PlayerData.Instance.MoneyRefresh((int)tData.money_type, tData.money_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();

            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
            if (sd == null) UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_NOTFOUND);
            sd.SerializeShowInfo(tData.attr);
            sd._equippedDepot.ReceiveOneKeyPromoteAllResp(tData);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ONE_KEY_STRENGTH_SUCCESS);
            //if (tData.equip_list != null && tData.equip_list.Count>0)
            //UISystem.Instance.SoldierAttView.PlayFastStrengthEffect(tData.equip_list.Count);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
            if (tData.result == (int)ErrorCodeEnum.NotEnoughGold)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            }
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.OneKeyPromoteAllEquipResp, (int)tData.result);
    }
    public void SendEquipUpQualityReq(List<KeyValuePair<UInt64,UInt64>> material,SacrificialDataEnum type)
    {
        EquipUpQualityReq tmpInfo = new EquipUpQualityReq();
        EquipUpQualityType tmpType = EquipUpQualityType.RED;
        for (int i = 0; i < material.Count;++i )
        {
            KeyValuePair<UInt64, UInt64> tmp = material[i];
            OnSoldierEquip soldierEquip = new OnSoldierEquip();
            soldierEquip.equip_uid = tmp.Key;
            soldierEquip.soldier_uid = tmp.Value;
            tmpInfo.equip_list.Add(soldierEquip);
        }
        if (type == SacrificialDataEnum.EquipOrange)
            tmpType = EquipUpQualityType.ORA;
        tmpInfo.type = tmpType;
        mEquipNetWork.SendEquipUpQualityReq(tmpInfo);
    }
    public void ReceiveEquipUpQualityResp(Packet data)
    {
        EquipUpQualityResp tData = Serializer.Deserialize<EquipUpQualityResp>(data.ms);
        if(tData.result == 0)
        {
            PlayerData.Instance._SoldierEquip.MultipleDelete(tData.uid_list);
            if (tData.type == EquipUpQualityType.RED)
                PlayerData.Instance.Sacrificial_Equip_EXP = tData.equip_quality_value;
            if (tData.type == EquipUpQualityType.ORA)
                PlayerData.Instance.Sacrificial_Ora_Equip_EXP = tData.equip_quality_value;

            List<Weapon> ListData=PlayerData.Instance.MultipleAddWeapon(tData.new_equip_list);
            PlayerData.Instance._SoldierEquip.SortNow();
            if(ListData != null && ListData.Count > 0)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_GATEQUIPEFFECT);
                UISystem.Instance.GetEquipEffectView.ShowMagicalEffect(ListData);//神兵特效
                UISystem.Instance.ResortViewOrder();
            }
            for (int i = 0; i < tData.soldier_list.Count;++i )
            {
                Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_list[i].uid);
                if (tmpSoldier == null)
                    continue;
                tmpSoldier.SerializeShowInfo(tData.soldier_list[i].attr);
            }
            SacrificialSystemInfo tmpSaInfo = ConfigManager.Instance.mSacrificialData.FindByType((int)SacrificialDataEnum.Equip);
            PlayerData.Instance.MoneyRefresh(tmpSaInfo.money_type, tData.gold_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.EquipUpQualityResp, (int)tData.result);
    }
    public void SendOneKeyOffEquipReq(UInt64 vSoldierUid)
    {
        OneKeyOffEquipReq tmpInfo = new OneKeyOffEquipReq();
        tmpInfo.soldier_uid = vSoldierUid;
        mEquipNetWork.SendOneKeyOffEquipReq(tmpInfo);
    }
    public void ReceiveOneKeyOffEquipResp(Packet data)
    {
        OneKeyOffEquipResp tData = Serializer.Deserialize<OneKeyOffEquipResp>(data.ms);
        if(tData.result == 0)
        {
            Soldier sd = PlayerData.Instance._SoldierDepot.FindByUid(tData.soldier_uid);
            if (sd == null) return;
            sd.SerializeShowInfo(tData.attr);
            for (int i = 0; i < tData.equip_list.Count;++i )
            {
                if (tData.equip_list[i] == null)
                    continue;

                sd._equippedDepot.ReceiveGetoffEquipResp(tData.equip_list[i].uid);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        PlayerData.Instance._SoldierEquip.OnErrotChange(EquipControl.EquipUpQualityResp, (int)tData.result);
    }
}