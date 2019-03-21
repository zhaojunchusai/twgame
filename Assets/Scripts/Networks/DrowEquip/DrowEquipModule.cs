using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class DrowEquipModule : Singleton<DrowEquipModule>
{
    public DrowEquipNetWork mEquipNetWork;
    public void Initialize()
    {
        if (mEquipNetWork == null)
        {
            mEquipNetWork = new DrowEquipNetWork();
            mEquipNetWork.RegisterMsg();
        }
    }

    public void Uninitialize()
    {
        if (mEquipNetWork != null)
            mEquipNetWork.RemoveMsg();
        mEquipNetWork = null;
    }
    public void SendOneExtractEquipReq()
    {
        OneExtractEquipReq tmpInfp = new OneExtractEquipReq();
        mEquipNetWork.SendOneExtractEquipReq(tmpInfp);
    }
    public void ReceiveOneExtractEquipResp(Packet data)
    {
        OneExtractEquipResp tData = Serializer.Deserialize<OneExtractEquipResp>(data.ms);
        if (tData.result == 0)
        {
            if (tData.cost_type == ExtractCostType.ECT_UseNothing)
            {
                PlayerData.Instance.DrowEquipFreeCount = tData.extract_count;
                PlayerData.Instance.DrowEquipFreeTime = tData.next_extract_tm;
            }
            if (tData.cost_type == ExtractCostType.ECT_UseMoney)
            {
                PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
                //if (UISystem.Instance.TopFuncView != null)
                //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            }
            List<Weapon> tmpWp = PlayerData.Instance.MultipleAddWeapon(tData.equip_infos);
            PlayerData.Instance.UpdateItem(tData.update_chip);
            PlayerData.Instance.UpdateItem(tData.update_item);
            LocalNotificationManager.Instance.DrawEquip();
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_GATEQUIPEFFECT);
            UISystem.Instance.GetEquipEffectView.ShowOnesLottery(tmpWp, tData.update_chip, tData.over_chip);
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        if (UISystem.Instance.DrowEquipView != null)
        {
            UISystem.Instance.DrowEquipView.SetTenChoose();
            UISystem.Instance.DrowEquipView.SetOneChoose();
        }
    }
    public void SendMultipleExtractReq()
    {
        MultipleExtractReq tmpInfp = new MultipleExtractReq();
        mEquipNetWork.SendMultipleExtractReq(tmpInfp);
    }
    public void ReceiveMultipleExtractResp(Packet data)
    {
        MultipleExtractResp tData = Serializer.Deserialize<MultipleExtractResp>(data.ms);
        if (tData.result == 0)
        {
            if (tData.cost_type == ExtractCostType.ECT_UseMoney)
            {
                PlayerData.Instance.MoneyRefresh(tData.money_type, tData.money_num);
                //if (UISystem.Instance.TopFuncView != null)
                //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
            }
            List<Weapon> tmpWp = PlayerData.Instance.MultipleAddWeapon(tData.equip_infos);
            //Debug.LogError(tData.equip_infos.Count);
            PlayerData.Instance.UpdateItem(tData.update_chip);
            PlayerData.Instance.UpdateItem(tData.update_item);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_GATEQUIPEFFECT);
            UISystem.Instance.GetEquipEffectView.ShowTensLottery(tmpWp, tData.update_chip, tData.over_chip);

        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        if (UISystem.Instance.DrowEquipView != null)
        {
            UISystem.Instance.DrowEquipView.SetTenChoose();
            UISystem.Instance.DrowEquipView.SetOneChoose();
        }
    }
}