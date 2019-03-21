using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class MailNetWork : Singleton<MailNetWork>
{
    public void OnSendGetMail(GetMailReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetMailReq>(0, request, MessageID.GetMailReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendGetMail ......  page = " + request.page);
    }

    public void OnReceiveGetMail(Packet data)
    {
        GetMailResp response = Serializer.Deserialize<GetMailResp>(data.ms);
        MailModule.Instance.OnReceiveGetMail(response);
        Debug.LogWarning("OnReceiveGetMail ...... result = " + response.result + "  Count = " + response.mail_list.Count);
    }

    public void OnSendReadMail(ReadMailReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ReadMailReq>(0, request, MessageID.ReadMailReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendReadMail ......  id = " + request.mail_id);
    }

    public void OnReceiveReadMail(Packet data)
    {
        ReadMailResp response = Serializer.Deserialize<ReadMailResp>(data.ms);
        Debug.LogWarning("OnReceiveReadMail ...... result = " + response.result + " mail_id =  " + response.mail_id);
        MailModule.Instance.OnReceiveReadMail(response);
    }

    public void OnSendDeleteMail(DeleteMailReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DeleteMailReq>(0, request, MessageID.DeleteMailReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendDeleteMail ......  id = " + request.mail_id);
    }

    public void OnReceiveDeleteMail(Packet data)
    {
        DeleteMailResp response = Serializer.Deserialize<DeleteMailResp>(data.ms);
        MailModule.Instance.OnReceiveDeleteMail(response);
        Debug.LogWarning("OnReceiveDeleteMail ......  result = " + response.result + " mail_id = " + response.mail_id);
    }

    public void OnSendGetMailAtt(GetMailAttReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetMailAttReq>(0, request, MessageID.GetMailAttReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendGetMailAtt ......  id = " + request.mail_id);
    }

    public void OnReceiveGetMailAtt(Packet data)
    {
        GetMailAttResp response = Serializer.Deserialize<GetMailAttResp>(data.ms);
        MailModule.Instance.OnReceiveGetMailAtt(response);
        Debug.LogWarning("OnReceiveGetMailAtt ......  result = " + response.result + " mail_id = " + response.mail_id);
    }

    public void OnSendOneKeyGetMailAtt(OneKeyReadMailReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneKeyReadMailReq>(0, request, MessageID.OneKeyReadMailReqID, PlayerData.Instance._SessionID);
        string str = " mail_id:";
        foreach (ulong id in request.mail_id_list)
        {
            str += id + ",";
        }
        Debug.LogWarning("OnSendOneKeyGetMailAtt ...... mail_id_list.Count = " + request.mail_id_list.Count + str);
    }

    public void OnReceiveOneKeyGetMailAtt(Packet data)
    {
        OneKeyReadMailResp response = Serializer.Deserialize<OneKeyReadMailResp>(data.ms);
        MailModule.Instance.OnReceiveOneKeyGetMailAtt(response);
        Debug.LogWarning("OnReceiveOneKeyGetMailAtt ...... " + CommonFunction.OneKeyReadMailRespToString(response));
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GetMailRespID, OnReceiveGetMail);
        NetWorkManager.Instance.RegisterEvent(MessageID.ReadMailRespID, OnReceiveReadMail);
        NetWorkManager.Instance.RegisterEvent(MessageID.DeleteMailRespID, OnReceiveDeleteMail);
        NetWorkManager.Instance.RegisterEvent(MessageID.OneKeyReadMailRespID, OnReceiveOneKeyGetMailAtt);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetMailAttRespID, OnReceiveGetMailAtt);
        //NetWorkManager.Instance.RegisterEvent(MessageID.NotifyNewMailID, OnNewMailNotify);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.GetMailRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ReadMailRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DeleteMailRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OneKeyReadMailRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetMailAttRespID);
        //NetWorkManager.Instance.RemoveEvent(MessageID.NotifyNewMailID);
    }
}
