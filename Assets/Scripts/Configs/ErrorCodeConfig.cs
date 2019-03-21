using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;

public class ErrorCodeConfig : BaseConfig {

    private Dictionary<uint, string> _errTip;

    public ErrorCodeConfig() 
    {
        _errTip = new Dictionary<uint,string>();
        Initialize(GlobalConst.Config.DIR_XML_ERRORCODE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    private void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                _errTip.Add(uint.Parse(CommonFunction.GetXmlElementStr(Element, "id")), CommonFunction.ReplaceEscapeChar(CommonFunction.GetXmlElementStr(Element, "tip")));
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    
    public string GetTipString(uint code) 
    {
        if (_errTip.ContainsKey(code)) 
        {
            return _errTip[code];
        }
        return string.Empty;
    }
    public void Uninitialize()
    {
        if (_errTip != null)
        {
            _errTip.Clear();
            _errTip = null;
        }
    }
}
