using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
public class SkillNetWork
{
    public void SendUpgradeSkillReq(UpgradeSkillReq data)//发送技能升级请求
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpgradeSkillReq>(0, data, MessageID.SkillUpgradeReqID, PlayerData.Instance._SessionID);
    }
    
    public void SendAutoUpgradeSkillReq(AutoUpgradeSkillReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AutoUpgradeSkillReq>(0, data, MessageID.AutoUpgradeSkillReqID, PlayerData.Instance._SessionID);
    }
    
    public void ReceiveUpgradeSkillResp(Packet data)//接受技能升级请求回馈
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SkillModule.Instance.ReceiveUpgradeSkillResp(data);
    }
    public void ReceiveAutoUpgradeSkillResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SkillModule.Instance.ReceiveAutoUpgradeSkillResp(data);
    }
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.SkillUpgradeRespID, ReceiveUpgradeSkillResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.AutoUpgradeSkillRespID, ReceiveAutoUpgradeSkillResp);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.SkillUpgradeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AutoUpgradeSkillRespID);

    }

}
