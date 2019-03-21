using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class SystemSettingNetWork : Singleton<SystemSettingNetWork> 
{
    public void SendNewNameReq(ChangeCharnameReq req)
    {
        Debug.Log("SendNewNameReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ChangeCharnameReq>(0, req, MessageID.ChangeCharnameReqID, PlayerData.Instance._SessionID);
    }
    public void  ReceiveNewNameResp(Packet data)
    {
        ChangeCharnameResp tData = Serializer.Deserialize<ChangeCharnameResp>(data.ms);
        if (tData.result != 0)
        {
            ErrorCode.ShowErrorTip(tData.result);
            return;
        }
        SystemSettingModule.Instance.ReceiveChangeNameResp(tData);
    }


    public void SendFeedBackReq(SuggestReq req)
    {
        Debug.Log("SendFeedBackReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SuggestReq>(0, req, MessageID.SuggestReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveFeedBackResp(Packet data)
    {
        SuggestResp tData = Serializer.Deserialize<SuggestResp>(data.ms);
        if (tData.result != 0)
        {
            ErrorCode.ShowErrorTip(tData.result);
            return;
        }
        SystemSettingModule.Instance.ReceiveFeedBackResp(tData);
    }
    
    public void SendCDKeyReq(UseCDKeyReq req)
    {
        Debug.Log("SendCDKeyReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UseCDKeyReq>(0, req, MessageID.UseCDKeyReqID, PlayerData.Instance._SessionID);

    }
    public void ReceiveCDKeyResp(Packet data)
    {
        UseCDKeyResp tData = Serializer.Deserialize<UseCDKeyResp>(data.ms);
        if (tData.result != 0)
        {
            ErrorCode.ShowErrorTip(tData.result);
            return;
        }
        SystemSettingModule.Instance.ReceiveCDKeyResp(tData);
    }

    public void SendSetLocalNotificationReq(SetNotifyReq req)
    {
        Debug.Log("SendSetLocalNotificationReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SetNotifyReq>(0, req, MessageID.SetNotifyReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveSetLocalNotificationResp(Packet data)
    {

        SetNotifyResp tData = Serializer.Deserialize<SetNotifyResp>(data.ms);
        SystemSettingModule.Instance.ReceiveSetLocalNotificationResp(tData);
    
    }


    public void SendExitReq(PlayerLogoutReq req)
    {
        //Debug.Log("SendExitReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PlayerLogoutReq>(0, req, MessageID.PlayerLogoutReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveExitResp(Packet data)
    {
        NotifyPlayerOffline tData = Serializer.Deserialize<NotifyPlayerOffline>(data.ms);
        SystemSettingModule.Instance.ReceiveExitResp(tData);
    }
    public void SendChangeReq(ChangeIconReq req)
    {
        Debug.Log("SendChangeReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ChangeIconReq>(0, req, MessageID.ChangeIconReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveChangeResp(Packet data)
    {
        
        ChangeIconResp tData = Serializer.Deserialize<ChangeIconResp>(data.ms);
        SystemSettingModule.Instance.ReceiveChangeResp(tData);
    }
    public void SendGetIconListReq(GetIconListReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetIconListReq>(0, req, MessageID.GetIconListReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetIconListResp(Packet data )
    {
        GetIconListResp tData = Serializer.Deserialize<GetIconListResp >(data.ms);
        SystemSettingModule.Instance.GetIconListResp(tData);
    }

    //屏蔽
    public void SendBlockItemReq()//初始化屏蔽界面
    {
        Debug.Log("SendBlockItemReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetScreenInfoListReq>(0, null, MessageID.GetScreenInfoListReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveBlockItemResp(Packet data)
    {
        GetScreenInfoListResp tData = Serializer.Deserialize<GetScreenInfoListResp>(data.ms);
        //Debug.LogError("OpenBlockView   tData Result = " + tData.result + "     tData.blocked_info_list.Count = " + tData.blocked_info_list.Count);
        SystemSettingModule.Instance.ReceiveBlockItemResp(tData);
      
    }

    public void SendRemoveBlockReq(UnblockedPlayerReq  req)//解除屏蔽
    {
        Debug.Log("SendRemoveBlockReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnblockedPlayerReq>(0, req, MessageID.UnblockedPlayerReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveRemoveBlockResp(Packet data)
    {
        UnblockedPlayerResp tData = Serializer.Deserialize<UnblockedPlayerResp>(data.ms);
        SystemSettingModule.Instance.ReceiveRemoveBlockResp(tData);

    }

    public void SendShieldingPlayersReq(BlockedPlayerReq req)//屏蔽玩家
    {
        Debug.Log("SendShieldingPlayersReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BlockedPlayerReq>(0, req, MessageID.BlockedPlayerReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveShieldingPlayersResp(Packet data)
    {
       BlockedPlayerResp tData = Serializer.Deserialize<BlockedPlayerResp>(data.ms);
       SystemSettingModule.Instance.ReceiveShieldingPlayerResp(tData);

    }
    /// <summary>
    /// 请求头像框列表
    /// </summary>
    /// <param name="req"></param>
    public void SendEnterFrameReq(EnterFrameReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterFrameReq>(0, req, MessageID.EnterFrameReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 服务器返回头像框列表
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveEnterFrameResp(Packet data)
    {
        EnterFrameResp rdata = Serializer.Deserialize<EnterFrameResp>(data.ms);
        SystemSettingModule.Instance.ReceiveEnterFrameResp(rdata);
    }
    /// <summary>
    /// 请求头像列表
    /// </summary>
    /// <param name="req"></param>
    public void SendEnterIconReq(EnterIconReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterIconReq>(0, req, MessageID.EnterIconReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 服务器返回头像列表
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveEnterIconResp(Packet data)
    {
        EnterIconResp rdata = Serializer.Deserialize<EnterIconResp>(data.ms);
        SystemSettingModule.Instance.ReceiveEnterIconResp(rdata);
    }



    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.BlockedPlayerRespID, ReceiveShieldingPlayersResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnblockedPlayerRespID, ReceiveRemoveBlockResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetScreenInfoListRespID, ReceiveBlockItemResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetIconListRespID, ReceiveGetIconListResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.ChangeIconRespID, ReceiveChangeResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.ChangeCharnameRespID, ReceiveNewNameResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SuggestRespID, ReceiveFeedBackResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.UseCDKeyRespID, ReceiveCDKeyResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyPlayerOfflineID, ReceiveExitResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SetNotifyRespID, ReceiveSetLocalNotificationResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterFrameRespID, ReceiveEnterFrameResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterIconRespID, ReceiveEnterIconResp);

    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.BlockedPlayerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UnblockedPlayerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetScreenInfoListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetIconListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ChangeIconRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ChangeCharnameRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SuggestRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UseCDKeyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyPlayerOfflineID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SetNotifyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterFrameRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterIconRespID);

    }
}
