using UnityEngine;
using System.Collections;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class DLCModule : Singleton<DLCModule>
{
    public DLCNetWork DlcNetwork;

    /// <summary>
    /// 当前游戏版本
    /// </summary>
    public string GameVersion;
    /// <summary>
    /// 当前资源版本
    /// </summary>
    public string ResVersion;
    /// <summary>
    /// 资源下载路径
    /// </summary>
    public string ResURL;
    
    public void Initialize() 
    {
        if (DlcNetwork == null) 
        {
            DlcNetwork = new DLCNetWork();
            DlcNetwork.RegisterMsg();
        }
    }

    /// <summary>
    /// 版本检查
    /// </summary>
    public void SendVersionNum()   
    {
        DlcNetwork.SendVersionNum();
    }

    public void ReceiveVersionNum(VersionResp data)
    {
        GameVersion = data.game_version;
        ResVersion = data.res_url;
        ResURL = data.res_version;
        //UISystem.Instance.DLCView.CallBackCheckVervison();
    }


    public void Uninitialize()
    {
        if (DlcNetwork != null)
            DlcNetwork.RemoveMsg();
        DlcNetwork = null;
    }
}