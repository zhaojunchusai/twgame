using ProtoBuf;
using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class CrossServerWarNetworK  {
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterCrossServerWarRespID, ReceiveEnterCrossServerWar);
        NetWorkManager.Instance.RegisterEvent(MessageID.TileRankRespID, ReceiveTileRankResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterServerAndAwardRespID, ReceiveEnterServerAndAwardResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyCDClearRespID, ReceiveBuyCDClearResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GotoBattleRespID, ReceiveGotoBattleResp);

    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterCrossServerWarRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.TileRankRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterServerAndAwardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyCDClearRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GotoBattleRespID);
    }

    public void SendEnterCrossServerWar(EnterCrossServerWarReq req)
    {
        Debug.Log("SendEnterCrossServerWar");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterCrossServerWarReq>(0, req, MessageID.EnterCrossServerWarReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnterCrossServerWar(Packet resp)
    {
        Debug.Log("ReceiveEnterCrossServerWar");
        EnterCrossServerWarResp data = Serializer.Deserialize<EnterCrossServerWarResp>(resp.ms);
        CrossServerWarModule.Instance.OnReceiveEnterCrossServerWar(data);
    }

    public void SendTileRankReq(TileRankReq req)
    {
        Debug.Log("SendTileRankReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<TileRankReq>(0, req, MessageID.TileRankReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveTileRankResp(Packet resp)
    {
        Debug.Log("ReceiveTileRankResp");
        TileRankResp data = Serializer.Deserialize<TileRankResp>(resp.ms);
        CrossServerWarModule.Instance.OnReceiveTileRankReq(data);
    }

    public void SendEnterServerAndAwardReq(EnterServerAndAwardReq req)
    {
        Debug.Log("SendEnterServerAndAwardReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterServerAndAwardReq>(0, req, MessageID.EnterServerAndAwardReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnterServerAndAwardResp(Packet resp)
    {
        Debug.Log("ReceiveEnterServerAndAwardResp");
        EnterServerAndAwardResp data = Serializer.Deserialize<EnterServerAndAwardResp>(resp.ms);
        CrossServerWarModule.Instance.OnReceiveEnterServerAndAwardResp(data);
    }

    public void SendBuyCDClearReq(BuyCDClearReq req)
    {
        Debug.Log("SendBuyCDClearReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyCDClearReq>(0, req, MessageID.BuyCDClearReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveBuyCDClearResp(Packet resp)
    {
        Debug.Log("ReceiveBuyCDClearResp");
        BuyCDClearResp data = Serializer.Deserialize<BuyCDClearResp>(resp.ms);
        CrossServerWarModule.Instance.OnReceiveBuyCDClear(data);
    }

    public void SendGotoBattleReq(GotoBattleReq req)
    {
        Debug.Log("SendGotoBattleReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GotoBattleReq>(0, req, MessageID.GotoBattleReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGotoBattleResp(Packet resp)
    {
        Debug.Log("ReceiveGotoBattleResp");
        GotoBattleResp data = Serializer.Deserialize<GotoBattleResp>(resp.ms);
        CrossServerWarModule.Instance.OnReceiveGotoBattle(data);
    }


}
