using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class CastleModule : Singleton<CastleModule>
{
    private CastleNetWork castleNetWork;
    private bool isCastleOpen;

    public void Initialize()
    {
        if (castleNetWork == null)
        {
            castleNetWork = new CastleNetWork();
            castleNetWork.RegisterMsg();
        }
    }

    public void Uninitialize()
    {
        if (castleNetWork != null)
            castleNetWork.RemoveMsg();
        castleNetWork = null;

        isCastleOpen = false;
    }

    //CastleLevelLow                      = 910, --城堡等级太低
    //PlyerLevelLow                       = 305,
    //MaterialNotEnough                   = 903, --材料不足

    /// <summary>
    /// 获取城堡信息-发送
    /// </summary>
    /// <param name="vAccID"></param>
    /// <param name="vIsCastleOpen">是否城堡界面开启[true-是 false-不是]</param>
    public void SendPutonEquipReq(uint vAccID, bool vIsCastleOpen = true)
    {
        Debug.Log("SendPutonEquipReq");
        GetCastleInfoReq tmpData = new GetCastleInfoReq();
        tmpData.accid = vAccID;
        isCastleOpen = vIsCastleOpen;
        castleNetWork.SendPutonEquipReq(tmpData);
    }
    /// <summary>
    /// 获取城堡信息-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveGetCastleInfoResp(GetCastleInfoResp data)
    {
        Debug.Log("ReceiveGetCastleInfoResp");
        if (data == null)
            return;

        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateCastleInfo(data.castle_info);
            if (isCastleOpen)
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_HEROATT);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CASTLEVIEW);
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ErrorCode.GetString(data.result));
        }
    }

    /// <summary>
    /// 升级城堡-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendUpgradeCastleReq(uint vID)
    {
        Debug.Log("SendUpgradeCastleReq");
        UpgradeCastleReq tmpData = new UpgradeCastleReq();
        tmpData.id = vID;
        castleNetWork.SendUpgradeCastleReq(tmpData);
    }
    /// <summary>
    /// 升级城堡-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUpgradeCastleResp(UpgradeCastleResp data)
    {
        Debug.Log("ReceiveUpgradeCastleResp");
        if (data == null)
            return;

        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //修改材料数据//
            PlayerData.Instance.UpdateItem(data.update_item);
            //修改城堡数据//
            PlayerData.Instance.mCastleInfo.ResetInfo(data.castle);
            UISystem.Instance.CastleView.ReceiveUpgradeCastleInfo();
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ErrorCode.GetString(data.result));
            if (data.result == (int)ErrorCodeEnum.NotEnoughGold)
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
        }
    }

    /// <summary>
    /// 升级射手-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendUpgradeShooterReq(uint vID)
    {
        Debug.Log("SendUpgradeShooterReq");
        UpgradeShooterReq tmpData = new UpgradeShooterReq();
        tmpData.id = vID;
        castleNetWork.SendUpgradeShooterReq(tmpData);
    }
    /// <summary>
    /// 升级射手-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUpgradeShooterResp(UpgradeShooterResp data)
    {
        Debug.Log("ReceiveUpgradeShooterResp");
        if (data == null)
            return;

        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //修改材料数据//
            PlayerData.Instance.UpdateItem(data.update_item);
            //修改射手数据//
            for (int i = 0; i < PlayerData.Instance.mShooterList.Count; i++)
            {
                if (PlayerData.Instance.mShooterList[i].mID == data.old_id)
                {
                     
                    PlayerData.Instance.mShooterList[i].ResetInfo(data.shooter);
                    UISystem.Instance.CastleView.PlayEffect((int)data.old_id);
                    UISystem.Instance.CastleView.ReceiveSingleShooterInfo(data.old_id, PlayerData.Instance.mShooterList[i]);
               
                    return;
                }
            }

         
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ErrorCode.GetString(data.result));
            if (data.result == (int)ErrorCodeEnum.NotEnoughGold)
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
        }
    }

    /// <summary>
    /// 解锁射手-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendUnlockShooterReq(uint vID)
    {
        Debug.Log("SendUnlockShooterReq");
        UnlockShooterReq tmpData = new UnlockShooterReq();
        tmpData.id = vID;
        castleNetWork.SendUnlockShooterReq(tmpData);
    }
    /// <summary>
    /// 解锁射手-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUnlockShooterResp(UnlockShooterResp data)
    {
        Debug.Log("ReceiveUnlockShooterResp" );
        if (data == null)
            return;

        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //修改材料数据//
            PlayerData.Instance.UpdateItem(data.update_item);
            //修改射手数据//
            for (int i = 0; i < PlayerData.Instance.mShooterList.Count; i++)
            {
                if (PlayerData.Instance.mShooterList[i].mID == data.old_id)
                {
                    PlayerData.Instance.mShooterList[i].ResetInfo(data.shooter);
                    UISystem.Instance.CastleView.PlayEffect((int)data.old_id);

                    UISystem.Instance.CastleView.ReceiveSingleShooterInfo(data.old_id, PlayerData.Instance.mShooterList[i]);
                    return;
                }
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ErrorCode.GetString(data.result));
            if (data.result == (int)ErrorCodeEnum.NotEnoughGold)
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
        }
    }
}