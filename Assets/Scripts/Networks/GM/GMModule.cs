using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class GMModule : Singleton<GMModule>
{
    private GMNetWork gmNetWork;

    public void Initialize()
    {
        if (gmNetWork == null)
        {
            gmNetWork = new GMNetWork();
            gmNetWork.RegisterMsg();
        }
    }

    public void Uninitialize()
    {
        if (gmNetWork != null)
            gmNetWork.RemoveMsg();
        gmNetWork = null;
    }

    /// <summary>
    /// 获取城堡信息-发送
    /// </summary>
    /// <param name="data"></param>
    /// 加金币-GMModule.Instance.SendGMCommandReq("gold 90000000");
    public void SendGMCommandReq(string vContent)
    {
        Debug.Log("SendGMCommandReq");
        GMOperateReq tmpData = new GMOperateReq();
        tmpData.content = vContent;
        gmNetWork.SendGMCommandReq(tmpData);
    }
    /// <summary>
    /// 获取城堡信息-接收
    /// </summary>
    /// <param name="data"></param>
    /// 目前只做了金币增加
    public void ReceiveGMCommandResp(GMOperateResp data)
    {
        if (data == null)
            return;

        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (data.item != null)
                PlayerData.Instance.UpdateItem(data.item);

            if (data.equip != null && data.equip.Count >0)
                PlayerData.Instance.MultipleAddWeapon(data.equip);

            if (data.soldier != null && data.soldier.Count > 0)
                PlayerData.Instance.MultipleAddSoldier(data.soldier);

            if (data.new_level_info != null)
                PlayerData.Instance.UpdateExpInfo(data.new_level_info);

            UISystem.Instance.CloseGameUI(GMView.UIName);
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format("GM指令处理失败,错误码:{0},详情咨询服务器", data.result));
        }
    }
}
