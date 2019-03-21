using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;


public class DLCNetWork :Singleton<DLCNetWork>
{

    public void SendVersionNum()
    {
        //Debug.Log("SendVersionNum");
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        //VersionReq data = new VersionReq();
        //NetWorkManager.Instance.mHttpDLCNetWork.SendMsg<VersionReq>(0, data, MessageID.VersionReqID, 0);
    }
    
    public void ReceiveVersionNum(Packet data)
    {
        //Debug.Log("ReceiveVersionNum");
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        //VersionResp tData = Serializer.Deserialize<VersionResp>(data.ms);
        //DLCModule.Instance.ReceiveVersionNum(tData);
    }


    public void RegisterMsg()
    {

        NetWorkManager.Instance.RegisterEvent(MessageID.VersionRespID, ReceiveVersionNum);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.VersionRespID);
    }

}


