using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class OpenLevelData
{
    public int id;
    public OpenFunctionType functionType;
    public string name;
    public int openLevel;
    public int vipLevel;
    public int type;
    public int gateId;
    public string icon;
    public string jump_icon;
    public string func_icon;
}

public class OpenLevelConfig : BaseConfig
{
    public List<OpenLevelData> _openList;

    public OpenLevelConfig()
    {
        _openList = new List<OpenLevelData>();
        Initialize(GlobalConst.Config.DIR_XML_OPENLEVEL, ParseConfig);
    }

    public bool CheckFuncIsOpen(OpenFunctionType functionType, bool showHint = true)
    {
        OpenLevelData data = GetDataByType(functionType);        
        if (data == null)
            return false;
        bool result = false;
        if (data.openLevel == -1)
        {
            result = PlayerData.Instance._VipLv >= data.vipLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_VIPLV, data.vipLevel));
        }
        else if (data.vipLevel == -1)
        {
            result = PlayerData.Instance._Level >= data.openLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV, data.openLevel));
        }
        else
        {
            result = PlayerData.Instance._Level >= data.openLevel || PlayerData.Instance._VipLv >= data.vipLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV_AND_VIPLV, data.vipLevel, data.openLevel));
        }
        return result;
    }

    public bool CheckFuncIsOpen(OpenFunctionType functionType, uint vip, uint lv, bool showHint = true)
    {
        OpenLevelData data = GetDataByType(functionType);
        bool result = false;
        if (data.openLevel == -1)
        {
            result = vip >= data.vipLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_VIPLV, data.vipLevel));
        }
        else if (data.vipLevel == -1)
        {
            result = lv >= data.openLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV, data.openLevel));
        }
        else
        {
            result = lv >= data.openLevel || vip >= data.vipLevel;
            if (showHint && !result)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_NO_LV_AND_VIPLV, data.vipLevel, data.openLevel));
        }
        return result;
    }
    public string GetOpenStr(OpenFunctionType functionType, string end = ConstString.FORMAT_TIP_OPEN_FUNC_OPEN)
    {
        OpenLevelData data = GetDataByType(functionType);
        if (data == null)
            return "";
        string Level = "";
        string vip = "";
        string gate = "";
        if (data.openLevel != -1)
        {
            if (PlayerData.Instance._Level < data.openLevel)
            {
                Level = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_LV, data.openLevel);
            }
            else
                return "";
        }
        if (data.vipLevel != -1)
        {
            if (PlayerData.Instance._VipLv < data.vipLevel)
            {
                vip = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_VIP, data.vipLevel);
            }
            else
                return "";

        }
        if (data.gateId != -1)
        {
            if (!PlayerData.Instance.IsPassedGate((uint)data.gateId))
            {
                StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.gateId);
                if (info != null)
                {
                    gate = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_GATE, info.GateSequence);
                }
            }
            else
                return "";

        }
        if (!string.IsNullOrEmpty(Level) || !string.IsNullOrEmpty(vip) || !string.IsNullOrEmpty(gate))
        {
            vip = string.IsNullOrEmpty(Level) || string.IsNullOrEmpty(vip) ? vip : ConstString.GATE_SWEEPTIP_EITHER + vip;
            gate = string.IsNullOrEmpty(vip) || string.IsNullOrEmpty(gate) ? gate : ConstString.GATE_SWEEPTIP_EITHER + gate;
            string str = Level + vip + gate + end;
            return str;
        }
        return "";
    }
    public bool CheckIsOpen(OpenFunctionType functionType,bool ShowHint = false)
    {
        OpenLevelData data = GetDataByType(functionType);
        if (data == null)
            return true;
        string Level = "";
        string vip = "";
        string gate = "";
        if (data.openLevel != -1)
        {
            if (PlayerData.Instance._Level < data.openLevel)
            {
                    Level = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_LV, data.openLevel);
            }
            else
                return true;
        }
        if (data.vipLevel != -1)
        {
            if (PlayerData.Instance._VipLv < data.vipLevel)
            {
                    vip = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_VIP, data.vipLevel);
            }
            else
                return true;

        }
        if (data.gateId != -1)
        {
            if (!PlayerData.Instance.IsPassedGate((uint)data.gateId))
            {
                    StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.gateId);
                    if (info != null)
                    {
                        gate = string.Format(ConstString.FORMAT_TIP_OPEN_FUNC_GATE, info.GateSequence);
                    }
            }
            else
                return true;

        }
        if (!string.IsNullOrEmpty(Level) || !string.IsNullOrEmpty(vip) || !string.IsNullOrEmpty(gate))
        {
            vip = string.IsNullOrEmpty(Level) || string.IsNullOrEmpty(vip) ? vip : ConstString.GATE_SWEEPTIP_EITHER + vip;
            gate = string.IsNullOrEmpty(vip) || string.IsNullOrEmpty(gate) ? gate : ConstString.GATE_SWEEPTIP_EITHER + gate;
            string str = Level + vip + gate + ConstString.FORMAT_TIP_OPEN_FUNC_OPEN;
            if (ShowHint)
            {
                if (!string.IsNullOrEmpty(vip))
                    CommonFunction.ShowVipLvNotEnoughTip(str);
                else
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, str);
            }
            return false;
        }
        return true;
    }

    /// <summary>
    /// 返回等级范围内的数据
    /// </summary>
    /// <param name="nowLv">包括</param>
    /// <param name="oldLv">不包括</param>
    /// <returns></returns>
    public List<OpenLevelData> GetOpenFuncByCharLv(int nowLv, int oldLv)
    {
        List<OpenLevelData> list = new List<OpenLevelData>();

        for (int i = 0; i < _openList.Count; i++)
        {
            if (_openList[i].openLevel <= nowLv && _openList[i].openLevel > oldLv)
            {
                list.Add(_openList[i]);
            }
        }
        if (list.Count > 0)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if ((list[i].vipLevel != -1 && PlayerData.Instance._VipLv >= list[i].vipLevel) || (list[i].icon.Equals("0")))
                //if (list[i].vipLevel != -1 && PlayerData.Instance._VipLv >= list[i].vipLevel)
                {
                    list.RemoveAt(i);
                }
            }
        }
        //Debug.LogError("listcount= " + list.Count);
        return list;
    }


    public OpenLevelData GetDataByType(OpenFunctionType functionType)
    {
        return _openList.Find(result => result.functionType == functionType);
    }

    public void Uninitialize()
    {
        if (_openList != null) 
        {
            _openList.Clear();
            _openList = null;
        }
    }

    private void ParseConfig(string xmlstr)
    {
        try
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement element in nodelist)
            {
                OpenLevelData data = new OpenLevelData();
                foreach (XmlElement xe in element)
                {
                    if (xe.Name == "id")
                    {
                        data.id = int.Parse(xe.InnerText);
                        data.functionType = (OpenFunctionType)data.id;
                    }
                    else if (xe.Name == "name")
                    {
                        data.name = xe.InnerText;
                    }
                    else if (xe.Name == "type")
                    {
                        data.type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "open_lv")
                    {
                        data.openLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "vip_lv")
                    {
                        data.vipLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "GateID")
                    {
                        data.gateId = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Icon")
                    {
                        data.icon = xe.InnerText;
                    }
                    else if (xe.Name == "jump_icon")
                    {
                        data.jump_icon = xe.InnerText;
                    }
                    else if (xe.Name == "func_icon")
                    {
                        data.func_icon = xe.InnerText;
                    }
                }
                _openList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load OpenLevelConfig XML Error:" + ex.Message);
        }
    }
}
