using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue
{
    public uint mID;
    public DialogueTalker mTalker;
    public uint mNpcID;
    public List<int> mOffset;
    //public DialogueTalkerPos mDialogueTalkerPos;
    public string mContent;
    public List<string> mSound;
}



public class DialogueConfig : BaseConfig
{
    public Dictionary<uint, Dialogue> _dialogueConfig = new Dictionary<uint, Dialogue>();
    public Dictionary<NPCTalkType, List<Dialogue>> _npcTalksDic = new Dictionary<NPCTalkType, List<Dialogue>>();
    private int _lastIndex = -1;
    private int _currentIndex = 0;

    public DialogueConfig()
    {
        Initialize(GlobalConst.Config.DIR_XML_DIALOGUE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        _dialogueConfig.Clear(); 
        _npcTalksDic.Clear();
        _lastIndex = -1;
        _currentIndex = 0;
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
                Dialogue data = new Dialogue();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "sayer")
                    {
                        data.mTalker = (DialogueTalker)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "npc_id")
                    {
                        data.mNpcID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "offset")
                    {
                        data.mOffset = CommonFunction.GetParseStrToInt(xe.InnerText);
                    }
                    if (xe.Name == "sound")
                    {
                        data.mSound = CommonFunction.GetSplitStr(xe.InnerText,';');
                    }
                    if (xe.Name == "content")
                    {
                        data.mContent = xe.InnerText;
                    }
                }
                //参考CommonData NPCTalkType
                if (data.mNpcID <= 2)
                    _dialogueConfig.Add(data.mID, data);
                else
                {
                    AddToNPCTalksDic((NPCTalkType)data.mNpcID, data);
                }
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load Dialogue XML Error:" + ex.Message);
        }
    }

    private void AddToNPCTalksDic(NPCTalkType type, Dialogue data)
    {
        if (!_npcTalksDic.ContainsKey(type))
        {
            List<Dialogue> list1 = new List<Dialogue>() { data };
            _npcTalksDic.Add(type, list1);
        }
        else
        {
            List<Dialogue> list2 = _npcTalksDic[type];
            list2.Add(data);
        }
    }

    public Dialogue GetDialogueConfigByID(uint id)
    {
        if (_dialogueConfig.ContainsKey(id))
        {
            return _dialogueConfig[id];
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 获取商店的随机对话
    /// </summary>
    /// <returns></returns>
    public string GetRandomString(NPCTalkType type)
    {
        if (_npcTalksDic == null || _npcTalksDic.Count < 1)
        {
            Debug.LogError("Dialogue config for npc is null !!!!");
            return "";
        }
        List<Dialogue> list = null;
        _npcTalksDic.TryGetValue(type, out list);
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cant find npc dialogue info !!!! type " + type);
            return "";
        }
        while (_currentIndex == _lastIndex)
        {
            _currentIndex = UnityEngine.Random.Range(0, list.Count);
        }
        Dialogue dialogue = list[_currentIndex];
        _lastIndex = _currentIndex;
        return dialogue.mContent;
    }
}
