using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class GMNetWork
{

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GMCommandRespID, ReceiveGMCommandResp);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.GMCommandRespID);
    }

    /// <summary>
    /// GM操作-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendGMCommandReq(GMOperateReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GMOperateReq>(0, data, MessageID.GMCommandReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// GM操作-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveGMCommandResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        if (data == null)
            return;
        GMOperateResp tmpData = Serializer.Deserialize<GMOperateResp>(data.ms);
        GMModule.Instance.ReceiveGMCommandResp(tmpData);
    }

}
