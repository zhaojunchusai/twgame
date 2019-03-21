using System.Xml;
using System.IO;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GuideData
{
    public uint mID;
    public uint mGuideType;
    public GuideTrigger mTrigger;
    public GuideLimitType mLimitType;
    public uint mLimitValue;
    public List<uint> mSteps;
}

public class GuideStep
{
    public uint mID;
    public uint mGuideID;
    public GuideTrigger mTrigger;
    public uint mNextStep;
    public List<GuideStepType> mStepType;

    public uint mDialogueID;
    public string mSound;
    public Vector2 mDescPos = Vector2.zero;
    public string mDescContent;
    public GuideMaskType mMaskType;
    public List<Vector4> mActionRect = new List<Vector4>();
    public Vector4 mCollider = Vector4.zero;
    public List<int> mArrowParas = new List<int>();
    public List<int> mFingerParas = new List<int>();
    public bool mScreenEffectForward;
    public Vector2 mMainCityLocation = Vector2.zero;
    public int mPauseGamePara;
    public uint mGateGift;
    public List<uint> mFightSoldier;
    public EHeroMoveType mSpecialFightLimitMoveDir;
    public bool mNotTriggerAgain;
    public float mDelay;
}

public class GuideConfig : BaseConfig
{
    private Dictionary<uint, GuideData> _guideDatas;
    private Dictionary<uint,GuideStep> _guideSteps;

    public GuideConfig()
    {
        _guideDatas = new Dictionary<uint, GuideData>();
        _guideSteps = new Dictionary<uint, GuideStep>();
        Initialize(GlobalConst.Config.DIR_XML_GUIDE_DATA, ParseGuideData);
        Initialize(GlobalConst.Config.DIR_XML_GUIDE_STEP, ParseGuideStep);
    }
    
    private void ParseGuideData(string str)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                GuideData data = new GuideData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "type")
                    {
                        data.mGuideType = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "trigger")
                    {
                        data.mTrigger = (GuideTrigger)int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "limit_type")
                    {
                        data.mLimitType = (GuideLimitType)int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "limit_value")
                    {
                        data.mLimitValue = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "steps")
                    {
                        data.mSteps = CommonFunction.GetParseStrToUint(xe.InnerText, ',');
                    }
                }
                _guideDatas.Add(data.mID, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

    }

    private void ParseGuideStep(string str)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(str);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                GuideStep data = new GuideStep();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "guide_id")
                    {
                        data.mGuideID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "trigger")
                    {
                        data.mTrigger = (GuideTrigger)int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "next_step")
                    {
                        data.mNextStep = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "step_types")
                    {
                        List<uint> list = CommonFunction.GetParseStrToUint(xe.InnerText, ',');
                        List<GuideStepType> typeList;
                        if (list == null || list.Count < 1)
                             typeList = null;
                        else
                        {
                            typeList = new List<GuideStepType>();
                            for (int i = 0; i < list.Count; i++)
                            {
                                typeList.Add((GuideStepType)list[i]);
                            }
                        }
                        data.mStepType = typeList;
                    }
                    if (xe.Name == "arrow_para")
                    {
                        xe.InnerText = xe.InnerText.Replace(';', ',');
                        data.mArrowParas = CommonFunction.GetParseStrToInt(xe.InnerText,',');
                    }
                    if (xe.Name == "finger_para")
                    {
                        data.mFingerParas = CommonFunction.GetParseStrToInt(xe.InnerText, ',');
                    }
                    if (xe.Name == "mask_type")
                    {
                        data.mMaskType = (GuideMaskType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "desc_pos")
                    {
                        data.mDescPos = GetVector2(xe.InnerText);
                    }
                    if (xe.Name == "desc_content")
                    {
                        data.mDescContent = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    if (xe.Name == "action_rect")
                    {
                        data.mActionRect = ParseActionRect(xe.InnerText);
                    }
                    if (xe.Name == "collider_rect")
                    {
                        List<Vector4> list = ParseActionRect(xe.InnerText);
                        data.mCollider = list.Count > 0 ? list[0]: Vector4.zero;
                    }
                    if (xe.Name == "dialogue_id")
                    {
                        data.mDialogueID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "sound")
                    {
                        data.mSound = xe.InnerText;
                    }
                    if (xe.Name == "scr_eff_forward")
                    {
                        data.mScreenEffectForward = (int.Parse(xe.InnerText) == 1);
                    }
                    if (xe.Name == "main_city_loc")
                    {
                        data.mMainCityLocation = GetVector2(xe.InnerText);
                    }
                    if (xe.Name == "pause_game")
                    {
                        data.mPauseGamePara = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "gate_gift")
                    {
                        data.mGateGift = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "spawn_soldier")
                    {
                        data.mFightSoldier = CommonFunction.GetParseStrToUint(xe.InnerText, ',');
                    }
                    if (xe.Name == "move_dir")
                    {
                        int para = int.Parse(xe.InnerText);
                        data.mSpecialFightLimitMoveDir = para == 0 ? EHeroMoveType.ehmtNone : (EHeroMoveType)para;
                    }
                    if (xe.Name == "not_trigger")
                    {
                        data.mNotTriggerAgain = int.Parse(xe.InnerText) == 1;
                    }
                    if (xe.Name == "delay")
                    {
                        data.mDelay = float.Parse(xe.InnerText);
                    }
                }
                _guideSteps.Add(data.mID, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private Vector2 GetVector2(string str)
    {
        List<int> list = CommonFunction.GetParseStrToInt(str, ',');
        if (list != null && list.Count > 1)
        {
            return new Vector2(list[0],list[1]);
        }
        else
        {
            return Vector2.zero;
        }
    }

    public GuideData GetGuideByID(uint id)
    {
        if (_guideDatas.ContainsKey(id))
            return _guideDatas[id];

        return null;
    }

    public GuideStep GetStepByID(uint id)
    {
        if (_guideSteps.ContainsKey(id))
            return _guideSteps[id];

        return null;
    }

    public List<GuideData> GetAllGuideData()
    {
        return new List<GuideData>(_guideDatas.Values);
    }
    private List<Vector4> ParseActionRect(string str)
    {
         List<Vector4> list = new List<Vector4>();
        string[] array = str.Split(';');
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(ParseEmptyRect(array[i]));
        }
        return list;
    }

    private Vector4 ParseEmptyRect(string str)
    {
        Vector4 vector4 = Vector4.zero;
        if (CommonFunction.XmlStringIsNull(str))
        {
            return vector4;
        }
        string[] array = str.Split(',');
        if (array != null && array.Length == 4)
            vector4 = new Vector4(float.Parse(array[0]), float.Parse(array[1]), float.Parse(array[2]), float.Parse(array[3]));
        return vector4;
    }

    public void Uninitialize() 
    {
        if(_guideDatas != null)
        {
            _guideDatas.Clear();
            _guideDatas = null;
        }

        if (_guideSteps != null)
        {
            _guideSteps.Clear();
            _guideSteps = null;
        }
    }
}
