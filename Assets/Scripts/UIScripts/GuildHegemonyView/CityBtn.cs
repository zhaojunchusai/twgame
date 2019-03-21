using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class CityBtn : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_CityBtn;
    [HideInInspector]public UISprite Spt_CityIntegralBG;
    [HideInInspector]public UILabel Lbl_CityIntegralLb;
    [HideInInspector]public UISprite Spt_CityTeamBG;
    [HideInInspector]public UILabel Lbl_CityTeamLb;
    [HideInInspector]public UISprite Spt_CityNameBG;
    [HideInInspector]public UILabel Lbl_CityNameLb;
    [HideInInspector]public UISprite Spt_CityGuildIcon;
    [HideInInspector]public UISprite Spt_CityQuality;
    [HideInInspector]public UISprite Spt_CityGuildState;
    [HideInInspector]public UISprite Spt_CityFailure;
    [HideInInspector]public GameObject Gobj_FightEffectObj;
    const int fihtNeedTime = 10;
    private long _coolDown = 0;
    public delegate void CityClick(int index);
    public CityClick onCityClick;
    private int _index;
    private CityScore _cityScoredata;
    private UnionCityPvpResult _cityPvpdata;
    private Color _memberColor = new Color(1.0F,214/255.0F,101/255.0F,1.0F);
    private string _myUnionIcon;
    private string _enmeyUnionIcon;
    private AltarFlameStatus selfState;
    private AltarFlameStatus enemeyState;
    public void Initialize()
    {
        Spt_CityBtn = transform.FindChild("CityBtn").GetComponent<UISprite>();
        Spt_CityIntegralBG = transform.FindChild("CityIntegralBG").gameObject.GetComponent<UISprite>();
        Lbl_CityIntegralLb = transform.FindChild("CityIntegralBG/CityIntegralLb").gameObject.GetComponent<UILabel>();
        Spt_CityTeamBG = transform.FindChild("CityTeamBG").gameObject.GetComponent<UISprite>();
        Lbl_CityTeamLb = transform.FindChild("CityTeamBG/CityTeamLb").gameObject.GetComponent<UILabel>();
        Spt_CityNameBG = transform.FindChild("CityNameBG").gameObject.GetComponent<UISprite>();
        Lbl_CityNameLb = transform.FindChild("CityNameBG/CityNameLb").gameObject.GetComponent<UILabel>();
        Spt_CityGuildIcon = transform.FindChild("CityGuildIcon").gameObject.GetComponent<UISprite>();
        Spt_CityGuildState = transform.FindChild("CityGuildIcon/state").gameObject.GetComponent<UISprite>();
        Spt_CityQuality = transform.FindChild("CityGuildIcon/CityQuality").gameObject.GetComponent<UISprite>();
        Spt_CityFailure = transform.FindChild("CityFailure").gameObject.GetComponent<UISprite>();
        Gobj_FightEffectObj = transform.FindChild("FightEffectObj").gameObject;
        UIEventListener.Get(Spt_CityBtn.gameObject).onClick = ButtonEvent_Onclick;
    }

    void Awake()
    {
        Initialize(); 
    }

    public void Init(string str,int index) 
    {
        _index = index;
        Lbl_CityTeamLb.text = "0";
        SetName(str);
        _cityScoredata = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mCityScores[index];
        Spt_CityGuildIcon.gameObject.SetActive(false);
        Spt_CityGuildState.gameObject.SetActive(false);
        Spt_CityFailure.gameObject.SetActive(false);
        Spt_CityIntegralBG.gameObject.SetActive(false);
        Gobj_FightEffectObj.gameObject.SetActive(false);
    }

    public void SetName(string str) 
    {
        Lbl_CityNameLb.text = str;
    }

    public void SetInfo(UnionCityPvpResult data, GuildFightState state) 
    {
        _cityPvpdata = data;
        if (data == null) 
        {
            Clear();
            Lbl_CityTeamLb.color = Color.white;
            Lbl_CityTeamLb.text =string.Format(ConstString.UNION_HINT_AVAILABLESCORE, _cityScoredata.mScore);
            switch (state)
            {
                case GuildFightState.Ready:
                    Spt_CityIntegralBG.gameObject.SetActive(true);
                    Lbl_CityTeamLb.color = _memberColor;
                    Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, 0);
                    Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_AVAILABLESCORE, _cityScoredata.mScore);
                    break;
            }
            return;
        }
        Spt_CityIntegralBG.gameObject.SetActive(true);
        Spt_CityFailure.gameObject.SetActive(true);
        Spt_CityGuildIcon.gameObject.SetActive(true);
        Lbl_CityTeamLb.color = _memberColor;
        switch(state)
        {
            case GuildFightState.SignUp:
                Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_SCORE, _cityPvpdata.score);
                Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
                if (data.fight_result == 1)
                {
                    CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTWIN);
                    CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_myUnionIcon));
                    Spt_CityGuildState.gameObject.SetActive(selfState == AltarFlameStatus.DEPEND_STATUS);
                }
                else
                {
                    CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTLOSE);
                    CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_enmeyUnionIcon));
                    Spt_CityGuildState.gameObject.SetActive(enemeyState == AltarFlameStatus.DEPEND_STATUS);
                }
                break;
            case GuildFightState.Signed:
                Spt_CityFailure.gameObject.SetActive(false);
                Spt_CityGuildIcon.gameObject.SetActive(false);
                Spt_CityGuildState.gameObject.SetActive(false);
                Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
                Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_AVAILABLESCORE, _cityScoredata.mScore);
                break;
            case GuildFightState.Match: 
                Spt_CityFailure.gameObject.SetActive(false);
                Spt_CityGuildIcon.gameObject.SetActive(false);
                Spt_CityGuildState.gameObject.SetActive(false);
                Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
                Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_AVAILABLESCORE, _cityScoredata.mScore);
                break;
            case GuildFightState.Fight:
                FightState();
                break;
            case GuildFightState.Ready:
                Spt_CityFailure.gameObject.SetActive(false);
                Spt_CityGuildIcon.gameObject.SetActive(false);
                Spt_CityGuildState.gameObject.SetActive(false);
                Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
                Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_AVAILABLESCORE, _cityScoredata.mScore);
                break;
            case GuildFightState.CoolDown:
                Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_SCORE, _cityPvpdata.score);
                Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
                if (data.fight_result == 1)
                {
                    CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTWIN);
                    CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_myUnionIcon));
                    Spt_CityGuildState.gameObject.SetActive(selfState == AltarFlameStatus.DEPEND_STATUS);
                }
                else
                {
                    CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTLOSE);
                    CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_enmeyUnionIcon));
                    Spt_CityGuildState.gameObject.SetActive(enemeyState == AltarFlameStatus.DEPEND_STATUS);
                }
                break;
        }
    }

    public void FightState() 
    {
        long _tempEndTime = UnionModule.Instance.FightBeginTime + _cityPvpdata.fight_num * fihtNeedTime;
        _coolDown = _tempEndTime - Main.mTime;
        if (_coolDown > 0)
        {
            Gobj_FightEffectObj.gameObject.SetActive(true);
            Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_AVAILABLESCORE, _cityPvpdata.score);
            Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
            Spt_CityFailure.gameObject.SetActive(false);
            Spt_CityGuildIcon.gameObject.SetActive(false);
            Spt_CityGuildState.gameObject.SetActive(false);
            Scheduler.Instance.AddTimer(1.0F, true, CoolDown);
        }
        else 
        {
            Gobj_FightEffectObj.gameObject.SetActive(false);
            Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_SCORE, _cityPvpdata.score);
            Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
            if (_cityPvpdata.fight_result == 1)
            {
                CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTWIN);
                CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_myUnionIcon));
                Spt_CityGuildState.gameObject.SetActive(selfState == AltarFlameStatus.DEPEND_STATUS);
            }
            else if(_cityPvpdata.fight_result == 2)
            {
                CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTLOSE);
                CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_enmeyUnionIcon));
                Spt_CityGuildState.gameObject.SetActive(enemeyState == AltarFlameStatus.DEPEND_STATUS);
            }
            else
            {
                Spt_CityFailure.gameObject.SetActive(false);
                Spt_CityGuildIcon.gameObject.SetActive(false);
                Spt_CityGuildState.gameObject.SetActive(false);
            }
        }
    }

    private void CoolDown() 
    {
        _coolDown--;
        if (_coolDown <= 0)
        {
            Scheduler.Instance.RemoveTimer(CoolDown);
            Spt_CityFailure.gameObject.SetActive(true);
            Spt_CityGuildIcon.gameObject.SetActive(true);
            Gobj_FightEffectObj.gameObject.SetActive(false);
            Lbl_CityIntegralLb.text = string.Format(ConstString.UNION_HINT_SCORE, _cityPvpdata.score);
            Lbl_CityTeamLb.text = string.Format(ConstString.UNION_HINT_JOINFIGHTNUM, _cityPvpdata.num);
            if (_cityPvpdata.fight_result == 1)
            {
                CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTWIN);
                CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_myUnionIcon));
                Spt_CityGuildState.gameObject.SetActive(selfState == AltarFlameStatus.DEPEND_STATUS);
            }
            else if (_cityPvpdata.fight_result == 2)
            {
                CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTLOSE);
                CommonFunction.SetSpriteName(Spt_CityGuildIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_enmeyUnionIcon));
                Spt_CityGuildState.gameObject.SetActive(enemeyState == AltarFlameStatus.DEPEND_STATUS);
            }
            else
            {
                Spt_CityFailure.gameObject.SetActive(false);
               // CommonFunction.SetSpriteName(Spt_CityFailure, GlobalConst.SpriteName.UNION_FIGHTLOSE);
                Spt_CityGuildIcon.gameObject.SetActive(false);
                Spt_CityGuildState.gameObject.SetActive(false);
            }

        }
    }

    public void SetIcon(string selfIcon, string enmeyUnionIcon,AltarFlameStatus selfState,AltarFlameStatus enemeyState) 
    {
        _myUnionIcon = selfIcon;
        _enmeyUnionIcon = enmeyUnionIcon;
        this.selfState = selfState;
        this.enemeyState = enemeyState;
    } 

    public void ButtonEvent_Onclick(GameObject btn) 
    {
        if (onCityClick != null)
            onCityClick(_index);
    }

    public int Index 
    {
        get { return _index; }
    }

    public void Uninitialize()
    {
        Scheduler.Instance.RemoveTimer(CoolDown);
    }

    public void Clear()
    {
        Scheduler.Instance.RemoveTimer(CoolDown);
        Lbl_CityIntegralLb.text = "";
        Lbl_CityTeamLb.text = "";
        Spt_CityFailure.gameObject.SetActive(false);
        Spt_CityIntegralBG.gameObject.SetActive(false);
        Spt_CityGuildIcon.gameObject.SetActive(false);
        Spt_CityGuildState.gameObject.SetActive(false);
        Gobj_FightEffectObj.gameObject.SetActive(false);
    }
}
