using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Collections;

/* File: AppConfig.cs
 * Desc: 游戏配置数据
 * Date: 2015-04-27 14:44
 * Add by taiwei
 */

public class AppInfo
{
    /// <summary>
    /// 当前版本号
    /// </summary>
    public string current_ver;
    /// <summary>
    /// 游戏资源版本号
    /// </summary>
    public string versionId;
    /// <summary>
    /// 游戏版本类型 0:内网  1:外网 2:私服
    /// </summary>
    public int verType;
    /// <summary>
    ///  是否开启使用DLC下载的资源, 当此开关打开后才会使用DLC已下载的资源
    /// </summary>
    public bool open_dlc;
    /// <summary>
    /// 是否开启GM工具
    /// </summary>
    public bool open_gm;
    /// <summary>
    /// 内网web IP地址
    /// </summary>
    public string webip_local;
    /// <summary>
    /// 外网web IP地址
    /// </summary>
    public string webip;

    /// <summary>
    /// 内网DLC IP地址
    /// </summary>
    public string dlcip_local;

    /// <summary>
    /// 外网DLC IP地址
    /// </summary>
    public string dlcip;
    /// <summary>
    /// 私服IP
    /// </summary>
    public string webip_SF;
    /// <summary>
    /// 私服DLC IP地址
    /// </summary>
    public string dlcip_SF;

    /// <summary>
    /// 是否开启网络连接
    /// </summary>
    public bool open_net;

    //public string GetWebIP()
    //{
    //    switch (verType)
    //    {
    //        case 0: return webip_local;
    //        case 1:
    //            {
    //                if (GlobalConst.IS_TESTAPK)
    //                {
    //                    if (GlobalConst.IsConnectFormal)
    //                    {
    //                        return "http://61.219.16.52:10201/";
    //                    }
    //                    else
    //                    {
    //                        return "http://61.219.16.52:30201/";
    //                    }
    //                }
    //                else
    //                {
    //                    return webip;
    //                }
    //            } break;
    //        case 2: return webip_SF;
    //        default: return webip_local;
    //    }
    //}

    public string GetDlcIP()
    {
        switch (verType)
        {
            case 0: return dlcip_local;
            case 1: return dlcip;
            case 2: return dlcip_SF;
            default: return dlcip_local;
        }
    }


}

public class AppConfig : BaseConfig
{
    private static List<AppInfo> _AppList;

    public AppConfig()
    {
        _AppList = new List<AppInfo>();
        //  Initialize(GlobalConst.Config.DIR_XML_APPCONFIG, ParseConfig);  //暂时注释
    }
    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void LoadLocalConfig()
    {
        //try
        //{
        _AppList = new List<AppInfo>();
        TextAsset textAsset = Resources.Load("Config/AppConfig") as TextAsset;// asset.Load("AppConfig", typeof(TextAsset)) as TextAsset;
        string xmlstr = textAsset.text;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlstr);
        XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
        foreach (XmlElement element in nodelist)
        {
            AppInfo data = new AppInfo();

            foreach (XmlElement xe in element)
            {
                if (xe.Name == "current_ver")
                {
                    data.current_ver = xe.InnerText;
                }
                else if (xe.Name == "dlcip")
                {
                    data.dlcip = xe.InnerText;
                }
                else if (xe.Name == "dlcip_local")
                {
                    data.dlcip_local = xe.InnerText;
                }
                else if (xe.Name == "open_dlc")
                {
                    int tmp = int.Parse(xe.InnerText);
                    if (tmp == 0)
                    {
                        data.open_dlc = false;
                    }
                    else
                    {
                        data.open_dlc = true;
                    }
                }
                else if (xe.Name == "open_gm")
                {
                    int tmp = int.Parse(xe.InnerText);
                    if (tmp == 0)
                    {
                        data.open_gm = false;
                    }
                    else
                    {
                        data.open_gm = true;
                    }
                }
                else if (xe.Name == "open_net")
                {
                    int tmp = int.Parse(xe.InnerText);
                    if (tmp == 0)
                    {
                        data.open_net = false;
                    }
                    else
                    {
                        data.open_net = true;
                    }
                }
                else if (xe.Name == "versionId")
                {
                    data.versionId = xe.InnerText;
                }
                else if (xe.Name == "verType")
                {
                    int tmp = int.Parse(xe.InnerText);
                    data.verType = tmp;
                }
                else if (xe.Name == "webip")
                {
                    data.webip = xe.InnerText;
                }
                else if (xe.Name == "webip_local")
                {

                    data.webip_local = xe.InnerText;
                }
                else if (xe.Name == "webip_SF")
                {

                    data.webip_SF = xe.InnerText;
                }
                else if (xe.Name == "dlcip_SF")
                {

                    data.dlcip_SF = xe.InnerText;
                }
            }
            _AppList.Add(data);
            //base.LoadCompleted();
        }
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError(ex.Message);
        //}
    }

    public void ParseConfig(WWW www)
    {
        try
        {
            AssetBundle asset = www.assetBundle;
            TextAsset textAsset = asset.Load("AppConfig", typeof(TextAsset)) as TextAsset;
            string xmlstr = textAsset.text;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement element in nodelist)
            {
                AppInfo data = new AppInfo();
                foreach (XmlElement xe in element)
                {
                    if (xe.Name == "current_ver")
                    {
                        data.current_ver = xe.InnerText;
                    }
                    else if (xe.Name == "dlcip")
                    {
                        data.dlcip = xe.InnerText;
                    }
                    else if (xe.Name == "dlcip_local")
                    {
                        data.dlcip_local = xe.InnerText;
                    }
                    else if (xe.Name == "open_dlc")
                    {
                        int tmp = int.Parse(xe.InnerText);
                        if (tmp == 0)
                        {
                            data.open_dlc = false;
                        }
                        else
                        {
                            data.open_dlc = true;
                        }
                    }
                    else if (xe.Name == "open_gm")
                    {
                        int tmp = int.Parse(xe.InnerText);
                        if (tmp == 0)
                        {
                            data.open_gm = false;
                        }
                        else
                        {
                            data.open_gm = true;
                        }
                    }
                    else if (xe.Name == "open_net")
                    {
                        int tmp = int.Parse(xe.InnerText);
                        if (tmp == 0)
                        {
                            data.open_net = false;
                        }
                        else
                        {
                            data.open_net = true;
                        }
                    }
                    else if (xe.Name == "versionId")
                    {
                        data.versionId = xe.InnerText;
                    }
                    else if (xe.Name == "verType")
                    {
                        int tmp = int.Parse(xe.InnerText);
                        data.verType = tmp;
                    }
                    else if (xe.Name == "webip")
                    {
                        data.webip = xe.InnerText;
                    }
                    else if (xe.Name == "webip_local")
                    {

                        data.webip_local = xe.InnerText;
                    }
                    else if (xe.Name == "webip_SF")
                    {

                        data.webip_SF = xe.InnerText;
                    }
                    else if (xe.Name == "dlcip_SF")
                    {

                        data.dlcip_SF = xe.InnerText;
                    }
                }
                _AppList.Add(data);
                base.LoadCompleted();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public AppInfo GetAppInfo()
    {
        return _AppList[0];
    }

    public void Uninitialize()
    {
        if (_AppList != null)
        {
            _AppList.Clear();
            _AppList = null;
        }
    }

    public List<AppInfo> GetAppInfoList()
    {
        return _AppList;
    }

}