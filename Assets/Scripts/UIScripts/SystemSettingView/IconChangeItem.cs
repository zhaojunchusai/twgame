using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using System.Collections.Generic;
using Assets.Script.Common;

public class IconChangeItem : MonoBehaviour
{
    [HideInInspector]
    public UIButton Btn_itemButton;
    [HideInInspector]
    public UISprite HeadTex;
    [HideInInspector]
    public UISprite HeadIcon;
    [HideInInspector]
    public UISprite IconIcon;
    [HideInInspector]
    public UISprite IconTex;
    [HideInInspector]
    public UILabel Lab;
    [HideInInspector]
    public GameObject IsClick;
    [HideInInspector]
    public GameObject IsLock;
    [HideInInspector]
    public UILabel Lbl_TimeLabel;
    [HideInInspector]
    public UILabel Lbl_EffectLabel;

    private string _colorGreen = "[3EFD49]"; //new Color(62 / 255f, 253 / 255f, 73 / 255f);
    private string _colorBrown = "[E3A94B]";//new Color(227 / 255f, 169 / 255f, 75 / 255f);
    private int _status;
    private int _resettime;
    private uint _type;

    private uint _itemID;    
    public uint ItemID
    {
        get { return _itemID; }

    }
    public void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        Btn_itemButton = transform.gameObject.GetComponent<UIButton>();
        HeadTex = transform.FindChild("Head/HeadTex").gameObject.GetComponent<UISprite>();
        HeadIcon = transform.FindChild("Head/HeadIcon").gameObject.GetComponent<UISprite>();
        IconTex = transform.FindChild("Icon/HeadIcon").gameObject.GetComponent<UISprite>();
        IconIcon = transform.FindChild("Icon/BGTex").gameObject.GetComponent<UISprite>();
        Lab = transform.FindChild("Label").gameObject.GetComponent<UILabel>();
        IsClick = transform.FindChild("Click").gameObject;
        IsLock = transform.FindChild("Lock").gameObject;
        Lbl_TimeLabel = transform.FindChild("TimeLabel").gameObject.GetComponent<UILabel>();
        Lbl_EffectLabel = transform.FindChild("EffectLabel").gameObject.GetComponent<UILabel>();

        BtnEventBinding();
    }
	
    /// <summary>
    /// 刷新头像框信息
    /// </summary>
    public void InitItem(FrameInfo vdata)
    {
        if (vdata != null)
        {
            FrameData data = ConfigManager.Instance.mFrameConfig.GetDateByID((uint)vdata.id);
            //Debug.LogError(vdata.id + "  " + vdata.status + "   " + vdata.resettime+" ///  " +data.id+"  "+data.type);
            _status = vdata.status;
            _resettime = vdata.resettime;
            _type = data.type;
            if (data != null)
            {
                StopTimeUpdate();

                _itemID = data.id;
                CommonFunction.SetSpriteName(HeadTex, CommonFunction.GetHeroIconNameByID(PlayerData.Instance.HeadID, true));
                CommonFunction.SetSpriteName(IconTex, CommonFunction.GetHeroIconNameByID(PlayerData.Instance.HeadID, false));

                SetIconSpritAndLabName(data);

                if (data.type == 1)
                {
                    IsLock.SetActive(false);
                    Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
                    Lbl_EffectLabel.text = CommonFunction.ReplaceEscapeChar(/*_colorBrown +*/ string.Format(data.effect_desc, SetEffectLabel(data)));
                }
                else if (data.type == 2)
                {
                    //int VipNum = PlayerData.Instance._VipLv;
                    //int UnLockNum = (int)data.num;
                    //if (VipNum >= UnLockNum)
                    //{
                    //    IsLock.SetActive(false);
                    //}
                    //else IsLock.SetActive(true);
                    if (vdata.status == 0)
                        IsLock.SetActive(true);
                    else if (vdata.status == 1)
                        IsLock.SetActive(false);
                    else
                        Debug.LogError("frame status error");
                    Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
                    Lbl_EffectLabel.text = CommonFunction.ReplaceEscapeChar(/*_colorBrown +*/ string.Format(data.effect_desc, SetEffectLabel(data)));

                }
                else if (data.type == 3)
                {
                    if (vdata.status == 0)
                    {
                        IsLock.SetActive(true);
                        if(data.const_time==0)
                            Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
                        else
                            Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen,
                                string.Format(ConstString.ACHIEVEMENT_CONSTTIME, (ConfigManager.Instance.mFrameConfig.GetConstTimeByID((uint)data.id))/3600));

                    }
                    else if (vdata.status == 1)
                    {
                        IsLock.SetActive(false);
                        if (data.const_time > 0)                         
                        {                        
                            TimeUpdate();
                            Scheduler.Instance.AddTimer(1, true, TimeUpdate);
                        }
                        else
                        {
                            Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
                        }
                    }
                    else
                        Debug.LogError("frame status error");

                    Lbl_EffectLabel.text = CommonFunction.ReplaceEscapeChar(/*_colorBrown + */string.Format(data.effect_desc, SetEffectLabel(data)));

                }
                else
                {
                    Debug.LogError("unlock type error");
                }
            }
            else
            {
                Debug.LogError("config info error");
            }

        }
        else
        {
            Debug.LogError("frameinfo lost");
        }
       
    }
    private void SetIconSpritAndLabName(FrameData data)
    {
        int id = CommonFunction.FilterFramId((int)PlayerData.Instance.FrameID);
        string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A,data.id);
        string Frame_B = string.Format(GlobalConst.SpriteName.Frame_Name_B, data.id);
        string LabName = data.name;
        Lab.text = LabName;
        CommonFunction.SetSpriteName(HeadIcon, Frame_A);
        CommonFunction.SetSpriteName(IconIcon, Frame_B);
    }
    public void  BtnEventBinding()
    {
        UIEventListener.Get(Btn_itemButton.gameObject).onClick = ButtonEvent_ChangeFrame;
    }
    /*
    public void ButtonEvent_ChangeIcon(GameObject Btn)
    {
        FrameData data = new FrameData();
        data = ConfigManager.Instance.mFrameConfig.GetFrameDataByID(_itemID);
        int VipNum = PlayerData.Instance._VipLv;
        int UnLockNum = (int)data.num;
        if (VipNum>=UnLockNum)//解锁
        {
            SystemSettingModule.Instance.SendIconChangeRequset(_itemID);
        }
        else
        {
            string HintString = data.desc;//解锁提示
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, HintString);
        }
    }
    */
    /// <summary>
    /// 效果描述变量
    /// </summary>
    private string[] SetEffectLabel(FrameData data)
    {
        List<string> strList = new List<string>();
        /*      if (data.hp != 0)
              {
                  strList.Add(_colorGreen + data.hp.ToString() + _colorBrown);
              }
              if (data.attack != 0)
              {
                  strList.Add(_colorGreen + data.attack.ToString() + _colorBrown);
              }
              if (data.hit != 0)
              {
                  strList.Add(_colorGreen + data.hit.ToString() + _colorBrown);
              }
              if (data.dodge != 0)
              {
                  strList.Add(_colorGreen + data.dodge.ToString() + _colorBrown);
              }
              if (data.crit != 0)
              {
                  strList.Add(_colorGreen + data.crit.ToString() + _colorBrown);
              }
              if (data.crit_def != 0)
              {
                  strList.Add(_colorGreen + data.crit_def.ToString() + _colorBrown);
              }
              if ( ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill) != null && (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill)).Name != "")
              {
                  strList.Add(_colorGreen + (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill)).Name + _colorBrown);
              }*/

        if (data.hp != 0)
        {
            strList.Add(data.hp.ToString());
        }
        if (data.attack != 0)
        {
            strList.Add(data.attack.ToString());
        }
        if (data.hit != 0)
        {
            strList.Add(data.hit.ToString());
        }
        if (data.dodge != 0)
        {
            strList.Add(data.dodge.ToString());
        }
        if (data.crit != 0)
        {
            strList.Add( data.crit.ToString());
        }
        if (data.crit_def != 0)
        {
            strList.Add(data.crit_def.ToString());
        }
        if (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill) != null && (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill)).Name != "")
        {
            strList.Add((ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill)).Name);
        }

        if (strList.Count == 0)
            Lbl_EffectLabel.gameObject.SetActive(false);
        else
            Lbl_EffectLabel.gameObject.SetActive(true);

        string[] items = new string[strList.Count];
        for(int i = 0; i < strList.Count; i++)
        {
            items[i] = strList[i];
        }
        return items;
    }
    /// <summary>
    /// 点击选择头像框
    /// </summary>
    private void ButtonEvent_ChangeFrame(GameObject Btn)
    {
        if (_status == 0)
        {
            if (_type == 2)
            {
                string str = string.Format(ConfigManager.Instance.mFrameConfig.GetFrameDataByID(_itemID).desc, ConfigManager.Instance.mFrameConfig.GetFrameDataByID(_itemID).num);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, str);
            }
            else if (_type == 3)
            {
                string str = string.Format(ConfigManager.Instance.mFrameConfig.GetFrameDataByID(_itemID).desc, 
                    ConfigManager.Instance.mAchievementConfig.GetNameByID((int)ConfigManager.Instance.mFrameConfig.GetFrameDataByID(_itemID).num));
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, str);
            }
        }
        else if (_status == 1)
        {
            SystemSettingModule.Instance.selfChangeFrame = true;
            SystemSettingModule.Instance.SendIconChangeRequset(_itemID);
        }
        else
        {
            Debug.LogError("status error");
        }
    }
    /// <summary>
    /// 重置时间跳动
    /// </summary>
    private void TimeUpdate()
    {
        //Debug.LogError(reset_time - Main.mTime);
        if (_resettime > Main.mTime)
        {
            Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen, CommonFunction.GetTimeString(_resettime - Main.mTime));
        }
        else
        {
            //Debug.LogError("end and remove");
            if (PlayerData.Instance.FrameID ==ItemID)
            {
                uint defaultNum = 10001;              
                SystemSettingModule.Instance.SendIconChangeRequset(defaultNum);
                //PlayerData.Instance.FrameID = defaultNum;
                //UISystem.Instance.MainCityView.ShowIconChange();
                //UISystem.Instance.TopFuncView.ShowIconBG();
                //UISystem.Instance.SystemSettingView.SetSystemPlayerIcon();
            }
            StopTimeUpdate();
            ResetToLocked();
        }
    }
    /// <summary>
    /// 结束时间跳动
    /// </summary>
    public void StopTimeUpdate()
    {
        Scheduler.Instance.RemoveTimer(TimeUpdate);
    }
    /// <summary>
    /// 时间结束，重置回锁定
    /// </summary>
    private void ResetToLocked()
    {
        _status = 0;
        IsLock.SetActive(true);
        Lbl_TimeLabel.text = string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, _colorBrown, _colorGreen,
                          string.Format(ConstString.ACHIEVEMENT_CONSTTIME, (ConfigManager.Instance.mFrameConfig.GetConstTimeByID(ItemID) / 3600)));
    }
}
