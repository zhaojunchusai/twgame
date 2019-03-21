using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;
public class GuildHegemonyViewController : UIBase 
{
    public GuildHegemonyView view;

    private GuildFightState _state = GuildFightState.CoolDown;

    private int _singleBGSize = 30;

    private int _doubleBGSize = 52;

    private List<CityBtn> _cityItem = new List<CityBtn>(); 

    private UISprite _otherIcon;

    private UISprite _mySelfIcon;

    private UISprite _mySelfState;

    private UISprite _otherState;

    private UILabel _otherName;

    private UILabel _mySelfName;

    private UILabel _otherMemberNum;

    private UILabel _myMemberNum;

    private UILabel _hintLb;

    private UISprite _otherNameBG;

    private long _coolDown = 0;

    private string _hintStr = "";

    private bool isSignup = false;

    private int _currentRound = 0;

    private int _currentSeason = 0;

    private BaseUnion _enmeyUnion;

    private int _mirror;

    public override void Initialize()
    {
        if (view == null) 
        {
            view = new GuildHegemonyView();
            view.Initialize();
            Init();
            BtnEventBinding();
        }
        OpenHegemonyView();

    }

    private void Init()
    {
        if (_cityItem.Count == 0) 
        {
            _cityItem.Add(view.CityBtn3.GetComponent<CityBtn>());
            _cityItem.Add(view.CityBtn4.GetComponent<CityBtn>());
            _cityItem.Add(view.CityBtn2.GetComponent<CityBtn>());
            _cityItem.Add(view.CityBtn5.GetComponent<CityBtn>());
            _cityItem.Add(view.CityBtn1.GetComponent<CityBtn>());
        }
        _otherIcon = view.Spt_OtherGuildIcon;
        _mySelfIcon = view.Spt_MyselfGuildIcon;
        _mySelfState = view.Spt_MyselfGuidState;
        _otherState = view.Spt_OtherGuildState;
        _otherName = view.Lbl_OtherGuildName;
        _mySelfName = view.Lbl_SelfGuildName;
        _otherMemberNum = view.Lbl_OtherGuildNum;
        _myMemberNum = view.Lbl_SelfGuildNum;
        _otherNameBG = view.Spt_OtherGuildBG;
        _hintLb = view.Lbl_IntegralLb;
        _hintLb.text = "";
        for (int i = 0; i < _cityItem.Count;i++ )
        {
            _cityItem[i].onCityClick = OnCityClick; 
            _cityItem[i].Init(ConstString.GUILDCITYNAME[i],i);
        }
        SetGuildQuaInt("0");
        SetGuildRoundInt("0");
        SetMySelfInfo();
    }

    private void OpenHegemonyView() 
    {
        UnionModule.Instance.OnSendOpenUnionPVP();
    }

    public void UpdateHegemonyPn(OpenUnionPvpResp data) 
    {
        _mirror = data.mirror;
        switch (data.state)
        {
            case UnionPvpState.UPS_APPLY:

                if (data.join_pvp == 1)
                {
                    isSignup = true;
                    _state = GuildFightState.Signed;
                }
                else 
                {
                    isSignup = false;
                    _state = GuildFightState.SignUp;
                }
                break;
            case UnionPvpState.UPS_MATCH:
                _state = GuildFightState.Match;
                break;
            case UnionPvpState.UPS_FIGHTING:
                _state = GuildFightState.Fight;
                break;
            case UnionPvpState.UPS_READY:
                _state = GuildFightState.Ready;
                break;
            case UnionPvpState.UPS_COOLING:
                _state = GuildFightState.CoolDown;
                break;

            case UnionPvpState.UPS_CANCEL:
                _state = GuildFightState.Cancel;
                break;

        }
        if (data.join_pvp == 1)
            isSignup = true;
        else
            isSignup = false;
        _enmeyUnion = data.enemy_union;
        _coolDown = (long)data.end_tick - data.cur_tick + 3;
        _currentRound = data.game_index;
        _currentSeason = data.season;
        SetMySelfInfo();
        SetGuildQuaInt(data.season_score.ToString());
        SetGuildRoundInt(data.current_score.ToString());
        SetCityInfo(data.pvp_result,_state);
        SetStateHint();
    }

    private void SetMySelfInfo() 
    {
        view.Gobj_SignUp.SetActive(false);
        view.Spt_MyselfGuildBG.gameObject.SetActive(true);
        switch (_state) 
        {
            case GuildFightState.CoolDown:
                _mySelfIcon.gameObject.SetActive(false);
                _mySelfState.gameObject.SetActive(false);
                _otherNameBG.height = _singleBGSize;
                _otherIcon.gameObject.SetActive(false);
                _otherState.gameObject.SetActive(false);
                _otherMemberNum.gameObject.SetActive(false);
                _mySelfName.text = ConstString.GUILD_BTN_MYGUILD;
                _otherName.text = ConstString.GUILD_BTN_OTHER;
                _myMemberNum.text = ConstString.GUILD_BTN_COOLDOWN;

                break;
            case GuildFightState.SignUp:
                _mySelfIcon.gameObject.SetActive(false);
                _mySelfState.gameObject.SetActive(false);
                _otherNameBG.height = _singleBGSize;
                _otherIcon.gameObject.SetActive(false);
                _otherState.gameObject.SetActive(false);
                _otherMemberNum.gameObject.SetActive(false);
                _mySelfName.text = ConstString.GUILD_BTN_MYGUILD;
                _otherName.text = ConstString.GUILD_BTN_OTHER;
                if (UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN || UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN)
                {
                    _myMemberNum.text = ConstString.GUILD_BTN_SIGNUP;
                     view.Spt_MyselfGuildBG.gameObject.SetActive(false);
                    view.Gobj_SignUp.SetActive(true);
                }
                else
                    _myMemberNum.text = ConstString.GUILD_BTN_UNSIGNUP;
                break;
            case GuildFightState.Signed:
                _mySelfIcon.gameObject.SetActive(true);
                _otherNameBG.height = _singleBGSize;
                _otherIcon.gameObject.SetActive(false);
                _otherState.gameObject.SetActive(false);
                _otherMemberNum.gameObject.SetActive(false);
                CommonFunction.SetSpriteName(_mySelfIcon,ConfigManager.Instance.mUnionConfig.GetUnionIconByID(UnionModule.Instance.UnionInfo.base_info.icon));
                _mySelfState.gameObject.SetActive(UnionModule.Instance.UnionInfo.base_info.altar_status == AltarFlameStatus.DEPEND_STATUS);
                _mySelfName.text = string.Format(ConstString.GUILD_COLOR_MEMBER, UnionModule.Instance.UnionInfo.base_info.name);
                _otherName.text = ConstString.GUILD_BTN_OTHER;
                _myMemberNum.text = string.Format(ConstString.GUILD_HINT_MEMBERSNUM, UnionModule.Instance.UnionInfo.base_info.member_num);
                break;
            case GuildFightState.Match:
                if (isSignup)
                {
                    if(_enmeyUnion != null)
                         SignOfMatch();
                    else
                        UnMatch();
                } 
                else
                    UnSignOfMatch();
                break;
            case GuildFightState.Ready:
                if (isSignup)
                {
                    if (_enmeyUnion != null)
                        SignOfMatch();
                    else
                        UnMatch();
                }
                else
                    UnSignOfMatch();
                break;
            case GuildFightState.Fight:
                if (isSignup)
                {
                    if (_enmeyUnion != null)
                        SignOfMatch();
                    else
                        UnMatch();
                }
                else
                    UnSignOfMatch();
                break;
        }
    }

    public void UnSignOfMatch() 
    {
        _mySelfIcon.gameObject.SetActive(false);
        _mySelfState.gameObject.SetActive(false);
        _otherNameBG.height = _singleBGSize;
        _otherIcon.gameObject.SetActive(false);
        _otherState.gameObject.SetActive(false);
        _otherMemberNum.gameObject.SetActive(false);
        _mySelfName.text = ConstString.GUILD_BTN_MYGUILD;
        _otherName.text = ConstString.GUILD_BTN_OTHER;
        _myMemberNum.text = ConstString.GUILD_BTN_UNSIGNUP;
    }

    public void SignOfMatch()
    {
        _mySelfIcon.gameObject.SetActive(true);
        _otherNameBG.height = _doubleBGSize;
        _otherIcon.gameObject.SetActive(true);
        _otherMemberNum.gameObject.SetActive(true);
        _mySelfName.text =string.Format(ConstString.GUILD_COLOR_MEMBER,UnionModule.Instance.UnionInfo.base_info.name);
        CommonFunction.SetSpriteName(_mySelfIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(UnionModule.Instance.UnionInfo.base_info.icon));
        _mySelfState.gameObject.SetActive(UnionModule.Instance.UnionInfo.base_info.altar_status == AltarFlameStatus.DEPEND_STATUS);
        _myMemberNum.text = string.Format(ConstString.GUILD_HINT_MEMBERSNUM, UnionModule.Instance.UnionInfo.base_info.member_num) ;
        _otherName.text = string.Format(ConstString.GUILD_COLOR_MEMBER, _enmeyUnion.name);
        _otherMemberNum.text = string.Format(ConstString.GUILD_HINT_MEMBERSNUM, _enmeyUnion.member_num);
        if (_mirror == 0)
        {
            CommonFunction.SetSpriteName(_otherIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_enmeyUnion.icon));
            _otherState.gameObject.SetActive(_enmeyUnion.altar_status == AltarFlameStatus.DEPEND_STATUS);
        }
        else
        {
            CommonFunction.SetSpriteName(_otherIcon, GlobalConst.SpriteName.UNION_MIRRORING);
            _otherState.gameObject.SetActive(false);
        }
    }

    public void UnMatch() 
    {
        _mySelfIcon.gameObject.SetActive(false);
        _mySelfState.gameObject.SetActive(false);
        _otherNameBG.height = _singleBGSize;
        _otherIcon.gameObject.SetActive(false);
        _otherState.gameObject.SetActive(false);
        _otherMemberNum.gameObject.SetActive(false);
        _mySelfName.text = string.Format(ConstString.GUILD_COLOR_MEMBER, UnionModule.Instance.UnionInfo.base_info.name);
         CommonFunction.SetSpriteName(_mySelfIcon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(UnionModule.Instance.UnionInfo.base_info.icon));
        _myMemberNum.text = string.Format(ConstString.GUILD_HINT_MEMBERSNUM, UnionModule.Instance.UnionInfo.members.Count);
        _otherName.text = ConstString.GUILD_BTN_OTHER;
    }

    public void SetCityInfo(List<UnionCityPvpResult> datas,GuildFightState _state) 
    {
        List<int> indexList = new List<int>();
        for (int i = 0; i < datas.Count;i++ )
        {
            int index = datas[i].city - 1;
            if (index < 0) continue;
            indexList.Add(index);
            if (_enmeyUnion == null)
                _cityItem[index].SetIcon(UnionModule.Instance.UnionInfo.base_info.icon, UnionModule.Instance.UnionInfo.base_info.icon, UnionModule.Instance.UnionInfo.base_info.altar_status, UnionModule.Instance.UnionInfo.base_info.altar_status);
            else
                _cityItem[index].SetIcon(UnionModule.Instance.UnionInfo.base_info.icon, _enmeyUnion.icon,UnionModule.Instance.UnionInfo.base_info.altar_status,_enmeyUnion.altar_status);
            _cityItem[index].SetInfo(datas[i], _state);
          
        }

        for (int i = 0; i < _cityItem.Count; i++)
        {
            
            if (!indexList.Contains(i)) 
            {
                _cityItem[i].SetInfo(null, _state);
            }
        }
    }

    public void UpdateCityInfo() 
    {
        List<UnionCityPvpResult> datas = UnionModule.Instance.UnionCityPvpArrayList;
        for (int i = 0; i <  datas.Count; i++)
        {
            if (_enmeyUnion == null)
                _cityItem[datas[i].city].SetIcon(UnionModule.Instance.UnionInfo.base_info.icon, UnionModule.Instance.UnionInfo.base_info.icon, UnionModule.Instance.UnionInfo.base_info.altar_status, UnionModule.Instance.UnionInfo.base_info.altar_status);
            else
                _cityItem[datas[i].city].SetIcon(UnionModule.Instance.UnionInfo.base_info.icon, _enmeyUnion.icon, UnionModule.Instance.UnionInfo.base_info.altar_status,_enmeyUnion.altar_status);
            _cityItem[datas[i].city].SetInfo(datas[i], _state);
        }
    }

    public void SetGuildQuaInt(string str) 
    {
        view.Lbl_SelfIntegralNum.text = str;
    }

    public void SetGuildRoundInt(string str) 
    {
        view.Lbl_OtherIntegralNum.text = str;
    }

    private void SetStateHint() 
    {
        switch(_state)
        {
            case GuildFightState.CoolDown:
                _hintStr =  ConstString.FORMAT_HINT_COOLDOWNSTATE;
                break;
            case GuildFightState.SignUp:
                _hintStr = ConstString.FORMAT_HINT_SIGNUPSTATE;
                break;
            case GuildFightState.Signed:
                _hintStr = ConstString.FORMAT_HINT_SIGNEDSTATE;
                break;
            case GuildFightState.Match:
                _hintStr = ConstString.FORMAT_HINT_MATCHSTATE;
                break;
            case GuildFightState.Ready:
                _hintStr = ConstString.FORMAT_HINT_FIGHTREAD;
                break;
            case GuildFightState.Fight:
                _hintStr = ConstString.FORMAT_HINT_FIGHTINGSTATE;
                break;
            case GuildFightState.Cancel:
                _hintStr = ConstString.FORMAT_HINT_CANCELSTATE;
                break;
        }
       // CoolDown();
        Scheduler.Instance.AddTimer(1.0F,true,CoolDown);
    }

    private void ChangeState() 
    {
        OpenHegemonyView();
    }

    public void OnCityClick(int index) 
    {
       
        switch (_state)
        {
            case GuildFightState.CoolDown:
                
                break;
            case GuildFightState.SignUp:
                
                break;
            case GuildFightState.Signed:
                
                break;
            case GuildFightState.Match:
                
                break;
            case GuildFightState.Ready:
                if (!isSignup) break;
                UnionModule.Instance.OnSendOpenUnionCity(index + 1);
                break;
            case GuildFightState.Fight:
                if (!isSignup) break;
                UnionModule.Instance.OnSendOpenUnionCity(index + 1);
                break;
        }
    }

    public void ButtonEvent_BackBtn(GameObject btn)
    {
        Close(null, null);
        UnionModule.Instance.OpenUnion();
    }

    public void ButtonEvent_RuleBtn(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RULEVIEW);
        UISystem.Instance.RuleView.UpdateViewInfo(2);  
    }

    public void ButtonEvent_HegemonyRankBtn(GameObject btn)
    {
        UnionModule.Instance.OnSendUnionPvpRank();
    }

    public void ButtonEvent_KillRank(GameObject btn)
    {
        if (_state == GuildFightState.Fight)
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNIONRANK_TITLE_FIGHTING);
        else
            UnionModule.Instance.OnSendUnionPvpKillRank();
    }

    public void ButtonEvent_MyselfGuild(GameObject btn) 
    {
        if (_state== GuildFightState.SignUp)
        {
            if (UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN || UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GUILD_HINT_JOINFIGHT, () =>
                {
                    UnionModule.Instance.OnSendApplyUnionPVP();
                });

            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GUILD_HINT_CANTJOINFIGHT);
            }
        }
    }

    public void ButtonEvent_OtherGuild(GameObject btn) 
    {
       
    }

    public void CoolDown() 
    {
        if (_coolDown > 1)
        {
            _coolDown--;
            _hintLb.text = string.Format(_hintStr, _currentSeason,_currentRound,CommonFunction.GetTimeStringByDay(_coolDown));
            //if (_state == GuildFightState.CoolDown)
            //    _hintLb.text = string.Format(_hintStr, _currentRound, CommonFunction.GetTimeStringByDay(_coolDown));
            //else
            //    _hintLb.text = string.Format(_hintStr, CommonFunction.GetTimeStringByDay(_coolDown));
        }
        else 
        {
            Scheduler.Instance.RemoveTimer(CoolDown);
            ChangeState();
            //切换状态
        }
    }

    public override void Uninitialize()
    {
        Scheduler.Instance.RemoveTimer(CoolDown);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_BackBtn.gameObject).onClick = ButtonEvent_BackBtn;
        UIEventListener.Get(view.Btn_RuleBtn.gameObject).onClick = ButtonEvent_RuleBtn;
        UIEventListener.Get(view.Btn_HegemonyRankBtn.gameObject).onClick = ButtonEvent_HegemonyRankBtn;
        UIEventListener.Get(view.Btn_KillRank.gameObject).onClick = ButtonEvent_KillRank;
        UIEventListener.Get(view.Spt_MyselfGuild.gameObject).onClick = ButtonEvent_MyselfGuild;
        UIEventListener.Get(view.Spt_OtherGuild.gameObject).onClick = ButtonEvent_OtherGuild;
    }

    public override void Destroy()
    {
    
        Scheduler.Instance.RemoveTimer(CoolDown);
        view = null;
        _coolDown = 0;
        _state = GuildFightState.CoolDown;
        _hintStr = "";
        isSignup = false;
        _currentRound = 0;
        _enmeyUnion = null;
        _cityItem.Clear();
        base.Destroy();
    }

}
