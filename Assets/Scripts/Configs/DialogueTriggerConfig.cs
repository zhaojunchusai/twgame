
using System;
using System.Linq;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueTrigger
{
    public uint mID;
    public DialogueTriggerType mDialogueTriggerType;
    public uint mDungeonID;
    public uint mLevelID;
    public uint mNpcID;
    public int mValue;
    public List<uint> mContentID;
}

public class DialogueTriggerConfig : BaseConfig 
{
    public Dictionary<uint, DialogueTrigger> _dialogueTriggerConfig = new Dictionary<uint, DialogueTrigger>();

    public DialogueTriggerConfig()
    {
        Initialize(GlobalConst.Config.DIR_XML_DIALOGUETRIGGER, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void UnInitialize()
    {
        _dialogueTriggerConfig.Clear();
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
                DialogueTrigger data = new DialogueTrigger();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "type")
                    {
                        data.mDialogueTriggerType = (DialogueTriggerType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "dungeon_catelog")
                    {
                        data.mDungeonID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "dungeon_id")
                    {
                        data.mLevelID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "npc_id")
                    {
                        data.mNpcID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "trigger")
                    {
                        data.mValue = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "content")
                    {
                        data.mContentID = ParseContentID(xe.InnerText);
                    }

                }
                _dialogueTriggerConfig.Add(data.mID, data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load DialogueTrigger XML Error:" + ex.Message);

        }
    }

    private List<uint> ParseContentID(string contentid)
    {
        List<uint> result = new List<uint>();

        string[] values = contentid.Split(new char[] { ';' });
        for (int i = 0; i < values.Length; i++)
        {
            result.Add(uint.Parse(values[i]));
        }

        return result;
    }

    public DialogueTrigger GetDialogueTriggerConfigByID(uint id)
    {
        if (_dialogueTriggerConfig.ContainsKey(id))
        {
            return _dialogueTriggerConfig[id];
        }
        else
        {
            return null;
        }
    }

    public List<DialogueTrigger> GetDungeonConfigByType(DialogueTriggerType type)
    {
        return _dialogueTriggerConfig.Values.Where(dialoguetrigger => dialoguetrigger.mDialogueTriggerType == type).ToList();
    }
}
