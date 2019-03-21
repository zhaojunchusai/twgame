using UnityEngine;
using System.Globalization;
using fogs.proto.msg;


public class GameActivitesItem : MonoBehaviour
{
    private GameAcitvityStateEnum _state = GameAcitvityStateEnum.NotStart;
    public GameAcitvityStateEnum State
    {
        get { return _state; }
    }

    [HideInInspector]
    public UISprite Spt_ItemBG_1;
    [HideInInspector]
    public UISprite Spt_TypeSprite;
    [HideInInspector]
    public UISprite Spt_TypeBGSprite;
    [HideInInspector]
    public UILabel Lbl_NameLabel;
    [HideInInspector]
    public UILabel Lbl_DateLabel;
    [HideInInspector]
    public UISprite Spt_IconSprite;
    [HideInInspector]
    public GameObject Obj_Notify;
    [HideInInspector]
    public UISprite Spt_Notify;
    [HideInInspector]
    public UILabel Lbl_NotifyNumLabel;
    [HideInInspector]
    public UISprite Spt_ChooseChooseSprite;

    private int _id = 0;
    public int ID
    {
        get { return _id; }
    }


    private GameActivityType _type;
    public GameActivityType Type
    {
        get { return _type; }

    }

    private bool _isNewActivity = false;
    public bool IsNewActivity
    {
        get { return _isNewActivity; }
        set
        {
            //Debug.LogWarning("In item "+this.transform.name +" ID = " + _id + " isNewActivity = " + _isNewActivity);
            _isNewActivity = value;
            Obj_Notify.gameObject.SetActive(_isNewActivity);
            Lbl_NotifyNumLabel.text = "";
        }
    }

    private bool _isSelect = false;
    public bool IsSelect
    {
        get { return _isSelect; }
        set
        {
            _isSelect = value;
            Spt_ChooseChooseSprite.gameObject.SetActive(value);
        }
    }

    private bool _isInit = false;

    public int days;

    public void Initialize()
    {
        if (_isInit)
            return;
        _isInit = true;

        Spt_ItemBG_1 = transform.FindChild("ItemBG_1").gameObject.GetComponent<UISprite>();
        Spt_TypeSprite = transform.FindChild("TypeSprite").gameObject.GetComponent<UISprite>();
        Spt_TypeBGSprite = transform.FindChild("TypeBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_DateLabel = transform.FindChild("DateLabel").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Obj_Notify = transform.FindChild("Notify").gameObject;
        Spt_Notify = transform.FindChild("Notify").gameObject.GetComponent<UISprite>();
        Lbl_NotifyNumLabel = transform.FindChild("Notify/NotifyNum").gameObject.GetComponent<UILabel>();
        Spt_ChooseChooseSprite = transform.FindChild("ChooseSprite").gameObject.GetComponent<UISprite>();
        IsSelect = false;
        IsNewActivity = false;
        SetLabelValues();
    }

    void Awake()
    {
        Initialize();
    }

    public void SetLabelValues()
    {
        Lbl_DateLabel.text = "";
        Lbl_NotifyNumLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void UpdateItemInfo(ActivityTimeInfo data)
    {
        Initialize();

        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            CommonFunction.SetSpriteName(Spt_Notify, GlobalConst.SpriteName.NOTICE_RED_POINT);
            Spt_Notify.width = 32;
            Spt_Notify.height = 32;
        }
        else
        {
            CommonFunction.SetSpriteName(Spt_Notify, GlobalConst.SpriteName.NOTICE_NEW);
            Spt_Notify.width = 56;
            Spt_Notify.height = 48;
        }

        _id = data.id;
        _type = (GameActivityType)data.activity_type;
        _state = CommonFunction.GetActivityStateByTime((ulong)data.start_time, (ulong)data.end_time, Main.mTime, out days);
        Lbl_NameLabel.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityNameByType(_type);
        if (_state == GameAcitvityStateEnum.Eternal)
        {
            Lbl_DateLabel.text = ConstString.GAMEACTIVTIY_STATE_ETERNAL;
        }
        else
        {
            Lbl_DateLabel.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityEndTimeByType(_type, (long)data.end_time);
        }
        UpdateItemTypeSp(ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivitySpriteByType(_type));
    }

    private void UpdateState(GameAcitvityStateEnum state, int days)
    {
        string desc = "";

        switch (state)
        {
            case GameAcitvityStateEnum.NotStart:
                desc = string.Format(ConstString.GAMEACTIVTIY_STATE_STARTAT, days);
                break;
            case GameAcitvityStateEnum.InProgress:
                desc = string.Format(ConstString.GAMEACTIVTIY_STATE_ENDSAT, days);
                break;
            case GameAcitvityStateEnum.Expired:
                desc = string.Format(ConstString.GAMEACTIVTIY_STATE_EXPIREDAT, days);
                break;
            case GameAcitvityStateEnum.Eternal:
                desc = string.Format(ConstString.GAMEACTIVTIY_STATE_ETERNAL);
                break;
        }
        Lbl_DateLabel.text = desc;
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateItemTypeSp(string sp)
    {
        bool isShowTypeSP = !CommonFunction.XmlStringIsNull(sp);
        Spt_TypeBGSprite.gameObject.SetActive(isShowTypeSP);
        Spt_TypeSprite.gameObject.SetActive(isShowTypeSP);
        CommonFunction.SetSpriteName(Spt_TypeSprite, sp);
    }

}
