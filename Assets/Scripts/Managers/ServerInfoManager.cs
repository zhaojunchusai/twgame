using UnityEngine;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class ServerInfoManager : MonoSingleton<ServerInfoManager>
{
    public ServerinfoConfig _serverInfoConfig;
    public ServerInfo _currentServerinfo;

    public void Initialize() 
    {
        _serverInfoConfig = new ServerinfoConfig();
        _currentServerinfo = _serverInfoConfig.GetServerInfoByType((byte)GlobalConst.SERVER_TYPE);
    }


    public void SetServerInfo() 
    {
        GlobalConst.SERVER_TYPE = (EServerType)_currentServerinfo.serverType;
        GlobalConst.VersionName = _currentServerinfo.versionName;
        if (_currentServerinfo.isOpenGuide > 0)
            GlobalConst.IS_OPEN_GUIDE = true;
        else
            GlobalConst.IS_OPEN_GUIDE = false;

        if (_currentServerinfo.isOpenGM > 0)
            GlobalConst.ISOPENGM = true;
        else
            GlobalConst.ISOPENGM = false;
        ResPath.CHECKVERSIONADDRESS = _currentServerinfo.resPath;
        BuglyManager.Instance.SetConfigDefault(GlobalConst.PLATFORM.ToString(), GlobalConst.VersionName, "", 10000);
    }

    public void UnInitialize()
    {

    }

}

public class ServerinfoConfig
{
    private List<ServerInfo> _serverinfoList;

    public ServerinfoConfig()
    {
        _serverinfoList = new List<ServerInfo>();
        LoadLocalConfig();
    }

    ~ServerinfoConfig()
    {
        _serverinfoList.Clear();
        _serverinfoList = null;
    }

    public void LoadLocalConfig()
    {
        try
        {
            TextAsset textAsset = Resources.Load("Config/ServerInfoConfig") as TextAsset;
            string xmlstr = textAsset.text;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                ServerInfo data = new ServerInfo();
                data.versionName = CommonFunction.GetXmlElementStr(Element, "versionname");
                data.isOpenGM = byte.Parse(CommonFunction.GetXmlElementStr(Element, "isopengm"));
                data.isOpenGuide = byte.Parse(CommonFunction.GetXmlElementStr(Element, "isopenguide"));
                data.resPath = CommonFunction.GetXmlElementStr(Element, "respath");
                data.serverAddress = CommonFunction.GetXmlElementStr(Element, "serveraddress");
                data.loginAddress = CommonFunction.GetXmlElementStr(Element, "loginaddress");
                data.serverType = byte.Parse(CommonFunction.GetXmlElementStr(Element, "servertype"));
                data.serverName = CommonFunction.GetXmlElementStr(Element, "servername");
                _serverinfoList.Add(data);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public List<ServerInfo> mServerInfoList
    {
        get { return _serverinfoList; }
    }

    public ServerInfo GetServerInfo(int index)
    {
        if (index >= _serverinfoList.Count || index < 0)
            return new ServerInfo();
        return _serverinfoList[index];
    }

    public ServerInfo GetServerInfoByType(byte st) 
    {
        for (int i = 0; i < _serverinfoList.Count;i++ )
        {
            if (_serverinfoList[i].serverType == st)
            {
                return _serverinfoList[i];
            }
        }
        return new ServerInfo();
    }

    public void ChangeServerData(int index, ServerInfo data)
    {
        if (index >= _serverinfoList.Count || index < 0)
            return;

        _serverinfoList.RemoveAt(index);
        _serverinfoList.Insert(index, data);
    }

}

public struct ServerInfo
{
    public string serverName;
    public string serverAddress;
    public string loginAddress;
    public byte serverType;
    public string resPath;
    public byte isOpenGM;
    public byte isOpenGuide;
    public string versionName;
}
