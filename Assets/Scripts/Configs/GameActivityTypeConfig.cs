using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;

public class GameActivityTypeData
{

    public GameActivityType mType;
    public string mName;
    public string mDesc;
    public string mBannerBG;
    public string mBannerIcon;
    public string mBannerDesc;
    public string mItemDescText;
    public string mItemTypeSp;
    public uint mPriority;
    //public List<uint> mParameterList;
    public string mDisplayEndTime;
}

public class GameActivityTypeConfig :BaseConfig
{
    private Dictionary<GameActivityType, GameActivityTypeData> _typeDic;

    public GameActivityTypeConfig()
    {
        _typeDic = new Dictionary<GameActivityType, GameActivityTypeData>();
        Initialize(GlobalConst.Config.DIR_XML_GAMEACTIVITYTYPE, ParseConfig);
    }

    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Unintialize()
    {
        if (_typeDic != null)
        {
            _typeDic.Clear();
            _typeDic = null;
        }
    }

    public void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                GameActivityTypeData data = new GameActivityTypeData();
                data.mType=(GameActivityType) int.Parse(CommonFunction.GetXmlElementStr(Element, "type"));
                data.mName = CommonFunction.GetXmlElementStr(Element, "name");
                data.mDesc= CommonFunction.GetXmlElementStr(Element, "description");
                data.mPriority= uint.Parse(CommonFunction.GetXmlElementStr(Element, "priority"));
                //data.mParameterList= CommonFunction.GetParseStrToUint(CommonFunction.GetXmlElementStr(Element, "params"));
                data.mBannerBG= CommonFunction.GetXmlElementStr(Element, "banner_bg");
                data.mBannerIcon= CommonFunction.GetXmlElementStr(Element, "banner_icon");
                data.mBannerDesc= CommonFunction.GetXmlElementStr(Element, "banner_desc");
                data.mItemTypeSp= CommonFunction.GetXmlElementStr(Element, "type_sp");
                data.mItemDescText= CommonFunction.GetXmlElementStr(Element, "item_desc");
                data.mDisplayEndTime= CommonFunction.GetXmlElementStr(Element, "display_end_time");

                _typeDic.Add(data.mType, data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load GameActivityConfig  XML Error:" + ex.Message);
        }
    }
    
    public GameActivityTypeData GetGameActivityTypeDataByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
            return _typeDic[type];
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }
    
    public string GetGameActivityItemDescByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            if (CommonFunction.XmlStringIsNull(_typeDic[type].mItemDescText))
                return "";
            else
                return _typeDic[type].mItemDescText;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public string GetGameActivityNameByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            if (string.IsNullOrEmpty(_typeDic[type].mName))
                Debug.LogError("GameActivity's name is none");
            else
                return _typeDic[type].mName;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public string GetGameActivitySpriteByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            return _typeDic[type].mItemTypeSp;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public string GetGameActivityEndTimeByType(GameActivityType type,long endTime)
    {
        if (_typeDic[type] != null)
        {
            if (string.IsNullOrEmpty(_typeDic[type].mDisplayEndTime))
                Debug.LogError("GameActivity's DisplayEndTime is none");
            else
            {
                if (CommonFunction.XmlStringIsNull(endTime.ToString()))
                    return _typeDic[type].mDisplayEndTime;
                else
                {
                    DateTime time = CommonFunction.GetTimeByLong(endTime);
                    return string.Format(_typeDic[type].mDisplayEndTime, time.Month, time.Day,time.Hour,time.Minute);                       
                }
            }
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    //public List<uint> GetGameActivityParamsByType(GameActivityType type)
    //{
    //    if (_typeDic[type] != null)
    //    {
    //        return _typeDic[type].mParameterList;
    //    }
    //    else
    //        Debug.LogError("There isn't a GameActivity " + type);
    //    return null;
    //}

    public string GetGameActivityBannerBgByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            return _typeDic[type].mBannerBG;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public string GetGameActivityBannerIconByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            return _typeDic[type].mBannerIcon;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public string GetGameActivityBannerDescByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            //if (CommonFunction.XmlStringIsNull(_typeDic[type].mBannerDesc))
            //    return "";
            //else
                return _typeDic[type].mBannerDesc;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public string GetGameActivityDescriptionByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
        {
            if (CommonFunction.XmlStringIsNull(_typeDic[type].mDesc))
                return "";
            else
                return _typeDic[type].mDesc;
        }
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return null;
    }

    public uint GetGameActivityPriotityByType(GameActivityType type)
    {
        if (_typeDic[type] != null)
            return _typeDic[type].mPriority;
        else
            Debug.LogError("There isn't a GameActivity " + type);
        return 0;
    }
}

public class GameActivityReqData
{
    public uint rewards_id;
    public List<ActivityDropItems> mDrops;
    public int condition;

    //public override string ToString()
    //{
    //    return " num = " + mNumber + "; dropID = " + mDropID + "; requiremnetNum = " + mReqNum;
    //}
}

public class ActivityDropItems
{
    public uint drop_id;
    public int drop_number;
    public uint drop_type;
}
