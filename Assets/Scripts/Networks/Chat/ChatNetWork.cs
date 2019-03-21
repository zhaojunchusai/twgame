using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class ChatNetWork 
{

    public void SendChat(SendChatMessageReq data) 
    {
        Debug.Log("SendChat");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SendChatMessageReq>(0, data, MessageID.SendChatReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveChat(Packet data)
    {
        Debug.Log("ReceiveChat");
        SendChatMessageResp tData = Serializer.Deserialize<SendChatMessageResp>(data.ms);
        ChatModule.Instance.ReceiveChat(tData);
    }

    public void SendReadChatInfo(ReadChatInfoReq data)
    {
        Debug.Log("SendReadChatInfo");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ReadChatInfoReq>(0, data, MessageID.ReadChatInfoReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveReadChatInfo(Packet data)
    {
        Debug.Log("ReceiveReadChatInfo");
        ReadChatInfoResp tData = Serializer.Deserialize<ReadChatInfoResp>(data.ms);
        ChatModule.Instance.ReceiveReadChatInfo(tData);
    }

    public void SendGetPlayerInfo(GetPlayerInfoReq data)
    {
        Debug.Log("GetPlayerInfo");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetPlayerInfoReq>(0, data, MessageID.GetPlayerInfoReqID, PlayerData.Instance._SessionID);
    }

    public void ReceivePlayerInfo(Packet data)
    {
        Debug.Log("ReceivePlayerInfo");
        GetPlayerInfoResp tData = Serializer.Deserialize<GetPlayerInfoResp>(data.ms);
        ChatModule.Instance.ReceviePlayerInfo(tData);
    }
    

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.SendChatRespID, ReceiveChat);
        NetWorkManager.Instance.RegisterEvent(MessageID.ReadChatInfoRespID, ReceiveReadChatInfo);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetPlayerInfoRespID, ReceivePlayerInfo);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.SendChatRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ReadChatInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetPlayerInfoRespID);
    }
}
