using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class PlayerPortraitData
{
    public uint id;
    public string name;
    public string icon;
    public string icon2;
    public uint quality;
    public uint type;
    public uint soldierid;

    public int unlock_type;
    public int const_time;
    public int hp;
    public int attack;
    public int hit;
    public int dodge;
    public int crit;
    public int crit_def;
    public int skill;
    public string unlock_desc;
    public string effect_desc;
}



public class PlayerPortraitConfig : BaseConfig
{
    private List<PlayerPortraitData> _PlayerPortrait = new List<PlayerPortraitData>();
    private List<PlayerPortraitData> _PlayerPortrait2 = new List<PlayerPortraitData>();
    public PlayerPortraitConfig()
    {
        Initialize(GlobalConst.Config.DIR_XML_PLAYERPORTAIT, ParseConfig);
    }
    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }
	public void Uninitialize()
    {
        _PlayerPortrait.Clear();
        _PlayerPortrait2.Clear();
    }
    private void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);//加载XML
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                PlayerPortraitData data = new PlayerPortraitData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "icon")
                    {
                        data.icon = xe.InnerText;
                    }
                    if (xe.Name =="icon2")
                    {
                        data.icon2 = xe.InnerText;
                    }
                    if (xe.Name == "quality")
                    {
                        data.quality = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "type")
                    {
                        data.type = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "soldierid")
                    {
                        data.soldierid = uint.Parse(xe.InnerText);
                    }
                }

                data.unlock_type = int.Parse(CommonFunction.GetXmlElementStr(Element, "unlock_type"));
                data.name = CommonFunction.GetXmlElementStr(Element, "name");
                data.const_time = int.Parse(CommonFunction.GetXmlElementStr(Element, "const_time"));
                data.hp = int.Parse(CommonFunction.GetXmlElementStr(Element, "hp"))/100;
                data.attack = int.Parse(CommonFunction.GetXmlElementStr(Element, "attack"))/100;
                data.hit = int.Parse(CommonFunction.GetXmlElementStr(Element, "hit"))/100;
                data.dodge = int.Parse(CommonFunction.GetXmlElementStr(Element, "dodge"))/100;
                data.crit = int.Parse(CommonFunction.GetXmlElementStr(Element, "crit"))/100;
                data.crit_def = int.Parse(CommonFunction.GetXmlElementStr(Element, "crit_def"))/100;
                data.skill = int.Parse(CommonFunction.GetXmlElementStr(Element, "skill"));
                data.unlock_desc = CommonFunction.GetXmlElementStr(Element, "unlock_desc");
                data.effect_desc = CommonFunction.GetXmlElementStr(Element, "effect_desc");

                _PlayerPortrait.Add(data);
                if (data.quality <= 1) {
                    _PlayerPortrait2.Add(data);
                }

            }
            base.LoadCompleted();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);

        }
    }
    public List<PlayerPortraitData> GetPlayerPortraitList()
    {
        return _PlayerPortrait;
    }

    public List<PlayerPortraitData> GetPTList() {
        return _PlayerPortrait2;
    }

    public PlayerPortraitData GetPlayerPortraitByID(uint id)
    {
        PlayerPortraitData info = _PlayerPortrait.Find((data) => { return data.id == id; });
        if (info == null)
        {
            Debug.LogError("can not get playerportrait by id:" + id);
        }
        return info;
    }

    public int GetConstTimeByID(uint id)
    {
        return _PlayerPortrait.Find((data) => { return data.id == id; }).const_time;
    }

    public bool IsIdCorrect(uint id)
    {
        foreach(PlayerPortraitData data in _PlayerPortrait)
        {
            if (data.id == id) return true;
        }
        return false;
    }

    public string GetUnlockDescByID(uint id)
    {
        return _PlayerPortrait.Find((data) => { return data.id == id; }).unlock_desc;
    }

    public uint GetSoldierId(uint id)
    {
        return _PlayerPortrait.Find((data) => { return data.id == id; }).soldierid;
    }
}
