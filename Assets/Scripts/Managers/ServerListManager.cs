#define SIMPLE_JSON_NO_LINQ_EXPRESSION

using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using MiniJSON;
using fogs.proto.msg;

public class ServerListManager : MonoSingleton<ServerListManager>
{
    public int LastLoginSvrID = 0;
    public ServerDetail CurServer = null;
    public List<int> RecommendSvrIDs = new List<int>();
    public List<ServerDetail> ServerList = new List<ServerDetail>();
    public string NoticeTitle;
    public string NoticeContent;
    public System.Action OnDownloadFinish;
    public bool IsDownloading = false;
    private bool _downloadServerListFinish = false; //用于判断只显示一次公告
    private string AccName = "";
    public void StartDownloadServerList(string accname)
    {
        AccName = accname;
        RecommendSvrIDs.Clear();
        ServerList.Clear();
        IsDownloading = true;
        UISystem.Instance.HintView.ShowLoading(true);
        StartCoroutine(GetServer());
    }

    private IEnumerator GetServer()
    {
        string url = "";
        //switch (GlobalConst.SERVER_TYPE)
        //{
        //    case EServerType.Nei251_8201:
        //    case EServerType.Nei251_9201:
        //    case EServerType.Nei251_10201:
        //    case EServerType.Nei251_18201:
        //    case EServerType.Nei252_10201:
        //    case EServerType.Nei249_10201:
        //        {//内网默认端口//
        //            url = "http://192.168.0.251/ltd/ServerList.php?accname={0}&mark={1}";
        //            break;
        //        }
        //    case EServerType.Wai_Sifu:
        //        {
        //            url = "http://218.93.248.115:8080/ltd/ServerList.php?accname={0}&mark={1}";
        //            break;
        //        }
        //    case EServerType.Wai_BeiJing:
        //        {
        //            url = "http://123.57.7.132:8080/ltd/ServerList.php?accname={0}&mark={1}";
        //            break;
        //        }
        //    case EServerType.Wai_SifuB:
        //        {
        //            url = "http://218.93.248.115:8080/ltd/ServerList.php?accname={0}&mark={1}";
        //            break;
        //        }
        //    case EServerType.Wai_7725:
        //        {
        //            if (GlobalConst.IS_TESTAPK && !GlobalConst.IsConnectFormal)
        //            {
        //                url = "http://61.219.16.52:8080/ltd/ServerList.php?accname={0}&mark=12";
        //            }
        //            else
        //            {
        //                url = "http://61.219.16.52:8080/ltd/ServerList.php?accname={0}&mark={1}";
        //            }
        //            break;
        //        }
        //    case EServerType.Wai_7725Test:
        //        {
        //            url = "http://61.219.16.52:8080/ltd/ServerList.php?accname={0}&mark={1}";
        //            break;
        //        }
        //}

        url = ServerInfoManager.Instance._currentServerinfo.loginAddress;
        WWW www = new WWW(string.Format(url, AccName, (int)GlobalConst.SERVER_TYPE));
        yield return www;
        if (!string.IsNullOrEmpty(www.error))
        {
            Debug.LogError(www.error);
        }
        else
        {
            ParseJsonData(www.text);
        }
        DownloadFinish();
    }

    private void DownloadFinish()
    {
        if (_downloadServerListFinish)
        {
            NoticeTitle = "";
        }
        LoginModule.Instance.CurServer = ServerListManager.Instance.CurServer;
        LoginModule.Instance.RecommendSvrIDs = ServerListManager.Instance.RecommendSvrIDs;
        LoginModule.Instance.ServerList = ServerListManager.Instance.ServerList;

        if (OnDownloadFinish != null)
        {
            OnDownloadFinish();
            OnDownloadFinish = null;
        }
        IsDownloading = false;
        UISystem.Instance.HintView.ShowLoading(false);
        _downloadServerListFinish = true;
    }

    private void ParseJsonData(string data)
    {
        Dictionary<string, object> dic = MiniJSON.Json.Deserialize(data) as Dictionary<string, object>;
        LastLoginSvrID = int.Parse(dic["last_login_server_id"].ToString());
        ParseRecommendSvr(dic["recommend_area_id"] as List<object>);
        ParseServerList(dic["server_list"] as List<object>);
        NoticeTitle = dic["notice_title"].ToString();
        NoticeContent = dic["notice_body"].ToString();
    }

    private void ParseRecommendSvr(List<object> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            RecommendSvrIDs.Add(int.Parse(data[i].ToString()));
        }
    }

    private void ParseServerList(List<object> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            ServerList.Add(ParseSingleDetail(data[i] as Dictionary<string, object>));
        }
    }

    private ServerDetail ParseSingleDetail(Dictionary<string, object> dic)
    {
        ServerDetail serverDetail = new ServerDetail();
        serverDetail.area_id = int.Parse(dic["area_id"].ToString());
        serverDetail.desc = dic["desc"].ToString();
        serverDetail.url = dic["url"].ToString();
        serverDetail.status = int.Parse(dic["status"].ToString());
        if (serverDetail.area_id == LastLoginSvrID)
        {
            CurServer = serverDetail;
        }
        return serverDetail;
    }

    private void DebugFinalData()
    {
        Debug.Log("last id=" + LastLoginSvrID);
        string str = "recommend id:";
        for (int i = 0; i < RecommendSvrIDs.Count; i++)
        {
            str += RecommendSvrIDs[i] + " ";
        }
        Debug.Log(str);
        str = "all servers : ";
        for (int i = 0; i < ServerList.Count; i++)
        {
            str += ("\nid=" + ServerList[i].area_id + " desc=" + ServerList[i].desc
                + " url=" + ServerList[i].url + " status=" + ServerList[i].status);
        }
        Debug.Log(str);
    }
}
