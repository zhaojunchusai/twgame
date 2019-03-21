using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;
public class UnionMemberItem : MonoBehaviour
{
    public enum SlaveState
    {
        Free = 1,
        Slave = 2
    }

    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UISprite Spt_Line;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UILabel Lbl_Job;
    [HideInInspector]public UILabel Lbl_MemberCount;
    [HideInInspector]public UILabel Lbl_Liveness;
    [HideInInspector]public UIButton Btn_HpControl;
    [HideInInspector]public UISprite Spt_BtnHpControlBackground;
    [HideInInspector]public UILabel Lbl_BtnHpControlLb;
    [HideInInspector]public UIButton Btn_SlaveControl;
    [HideInInspector]public UISprite Spt_BtnSlaveControlBackground;
    [HideInInspector]public UILabel Lbl_BtnSlaveControlLb;
    [HideInInspector]public UIButton Btn_OpenMenu;
    [HideInInspector]public UISprite Spt_BtnOpenMenuBackground;
    [HideInInspector]public UILabel Lbl_BtnOpenMenuLb;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_IconFrame;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_LvBg;
    [HideInInspector]public UILabel Lbl_CharLv;
    private bool _initialized = false;
    private UnionMember _member;
    public UnionMember Member
    {
        get { return _member; }
    }

    private bool _isSlaveLock = false;

    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        
        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_Line = transform.FindChild("Line").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_Job = transform.FindChild("Job").gameObject.GetComponent<UILabel>();
        Lbl_MemberCount = transform.FindChild("MemberCount").gameObject.GetComponent<UILabel>();
        Lbl_Liveness = transform.FindChild("Liveness").gameObject.GetComponent<UILabel>();
        Btn_HpControl = transform.FindChild("HpControl").gameObject.GetComponent<UIButton>();
        Spt_BtnHpControlBackground = transform.FindChild("HpControl/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnHpControlLb = transform.FindChild("HpControl/Lb").gameObject.GetComponent<UILabel>();
        Btn_SlaveControl = transform.FindChild("SlaveControl").gameObject.GetComponent<UIButton>();
        Spt_BtnSlaveControlBackground = transform.FindChild("SlaveControl/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnSlaveControlLb = transform.FindChild("SlaveControl/Lb").gameObject.GetComponent<UILabel>();
        Btn_OpenMenu = transform.FindChild("OpenMenu").gameObject.GetComponent<UIButton>();
        Spt_BtnOpenMenuBackground = transform.FindChild("OpenMenu/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnOpenMenuLb = transform.FindChild("OpenMenu/Lb").gameObject.GetComponent<UILabel>();
        Spt_IconBG = transform.FindChild("Head/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = transform.FindChild("Head/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Head/Icon").gameObject.GetComponent<UISprite>();
        Spt_LvBg = transform.FindChild("LvBg").gameObject.GetComponent<UISprite>();
        Lbl_CharLv = transform.FindChild("CharLv").gameObject.GetComponent<UILabel>();
        SetLabelValues();

        UIEventListener.Get(Btn_HpControl.gameObject).onClick = ButtonEvent_HpControl;
        UIEventListener.Get(Btn_SlaveControl.gameObject).onClick = ButtonEvent_SlaveControl;
        UIEventListener.Get(Btn_OpenMenu.gameObject).onClick = ButtonEvent_OpenMenu;
        UIEventListener.Get(Spt_BG.gameObject).onClick = ButtonEvent_SeeDetail;
    }

    public void InitItem(UnionMember member)
    {
        Initialize();
        _isSlaveLock = MainCityModule.Instance.LockFuncs.Contains((uint) OpenFunctionType.Slave);
        _member = member;
        Lbl_Name.text = _member.charname;
        Lbl_Job.text = string.Format(ConstString.FORMAT_UNION_JOB,
                                     CommonFunction.GetUnionMemberJobString(member.position));
        Lbl_Liveness.text = string.Format(ConstString.FORMAT_THIS_WEEK_LIVENESS, member.vitality);
        Lbl_CharLv.text = _member.level.ToString();
        CommonFunction.SetHeadAndFrameSprite(Spt_Icon,Spt_IconFrame,_member.icon,_member.icon_frame,true);
        //UnionModule.Instance.SetOfflineTime(Lbl_MemberCount, _member.offline_tick);
        CommonFunction.SetOfflineTime(Lbl_MemberCount, _member.offline_tick);
        SetBtnState();
    }

    public void Refreash()
    {
        //SetBtnState();
        InitItem(UnionModule.Instance.GetUnionMember(_member.charid));
    }

    private void SetBtnState()
    {
        if(UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_MEMBER)
        {
            Btn_OpenMenu.gameObject.SetActive(false);
            Btn_SlaveControl.gameObject.SetActive(true);
            if(_isSlaveLock)
            {
                Btn_HpControl.transform.localPosition = new Vector3(235,2,0);
            }
            else
            {
                Btn_HpControl.transform.localPosition = new Vector3(180, 2, 0);                
            }
        }
        else
        {
            Btn_OpenMenu.gameObject.SetActive(true);
            Btn_SlaveControl.gameObject.SetActive(false);

            if(_isSlaveLock && UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN)
            {
                Lbl_BtnOpenMenuLb.text = ConstString.UNION_BTN_KICK;
            }
            else
            {
                Lbl_BtnOpenMenuLb.text = ConstString.UNION_BTN_OPEN;
            }
        }
        Btn_HpControl.gameObject.SetActive(true);

        SetHpBtn();
        SetSlaveBtn();
    }

    private enum HpBtnState 
    {
        Visit,
        Get
    }

    private HpBtnState _hpBtnState;
    private void SetHpBtn()
    {
        _hpBtnState = HpBtnState.Visit;
        Lbl_BtnHpControlLb.text = ConstString.UNION_MEMBER_HPBTNSTATE_VISIT;
        UnionMember me = UnionModule.Instance.MyUnionMemberInfo;
        bool hasVisitMe = false;
        for (int i = 0; i < me.visitors.Count; i++)
        {
            //Debug.Log("visitors ["+i+"]:" + me.visitors[i].charid + " this item id=" + _member.charid);
            if (me.visitors[i].charid == _member.charid)
            {
                hasVisitMe = true;
                //Debug.Log(me.visitors[i].charid + " " + me.visitors[i].state);
                if (me.visitors[i].state == (int)UnionModule.UnionGetHpState.Get)//0表示未领取奖励， 1表示领取过奖励
                {
                    if (me.interviewees.Contains(_member.charid))
                    {
                        Btn_HpControl.gameObject.SetActive(false);
                    }
                    else
                    {
                        Lbl_BtnHpControlLb.text = ConstString.UNION_MEMBER_HPBTNSTATE_VISIT_BACK;
                    }
                }
                else
                {
                    Lbl_BtnHpControlLb.text = ConstString.UNION_MEMBER_HPBTNSTATE_GET;
                    _hpBtnState = HpBtnState.Get;
                }
                break;
            }
        }
        if (!hasVisitMe && me.interviewees.Contains(_member.charid))
        {
            Btn_HpControl.gameObject.SetActive(false);
        }


    }

    private void SetSlaveBtn()
    {
        if (_isSlaveLock)
        {
            Btn_SlaveControl.gameObject.SetActive(false);
            return;
        }

        //是否被奴役（2表示被奴役，1表示自由）
        if (_member.slave_state == (int)SlaveState.Slave)
        {
            Lbl_BtnSlaveControlLb.text = ConstString.UNION_SLAVE_RESCUE;
        }
        else
        {
            Lbl_BtnSlaveControlLb.text = ConstString.UNION_SLAVE_ENSLAVED;
        }
    }

    public void SetLabelValues()
    {
        //Lbl_Name.text = "今天我不在";
        //Lbl_Job.text = "职位:成员";
        //Lbl_MemberCount.text = "最后离线:10分钟前";
        //Lbl_Liveness.text = "本周活跃:5000";
        //Lbl_BtnHpControlLb.text = "领取";
        //Lbl_BtnSlaveControlLb.text = "奴役";
        //Lbl_BtnOpenMenuLb.text = "展开";
        //Lbl_CharLv.text = "";
    }
    public void ButtonEvent_SeeDetail(GameObject btn)
    {
        ChatModule.Instance.SendGetPlayerInfo(_member.area_id, _member.accname, ShowMemberInfo);
    }

    void ShowMemberInfo(ArenaPlayer player)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
        UISystem.Instance.PlayerInfoView.UpdateViewInfo(PlayerInfoTypeEnum.Corps, player,_member.slave_state == (int)SlaveState.Slave,_member.position);
    }

    public void ButtonEvent_HpControl(GameObject btn)
    {
        if(_hpBtnState == HpBtnState.Visit)
        {
            UnionModule.Instance.OnSendVisitUnionMember(_member.charid);
        }
        else if(_hpBtnState == HpBtnState.Get)
        {
            UnionModule.Instance.OnSendGetUnionMemberHp(_member.charid);
        }
    }

    public void ButtonEvent_SlaveControl(GameObject btn)
    {
        if (_isSlaveLock)
        {
            Btn_SlaveControl.gameObject.SetActive(false);
            return;
        }

        if (_member.slave_state == (int)SlaveState.Slave)
        {
            PlayerData.Instance._Prison.GetOtherPlayerInfo(_member.accname, _member.area_id);
        }
        else
        {
            PlayerData.Instance._Prison.GetOtherPlayerInfo(_member.accname, _member.area_id);
        }
    }

    public void ButtonEvent_OpenMenu(GameObject btn)
    {
        if (_isSlaveLock && UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.FORMAT_UNION_KICK, _member.charname),
                () => UnionModule.Instance.OnSendManageUnionMember(ManageUnionType.MUT_KICK, _member.charid));
            return;
        }
        UISystem.Instance.UnionHallView.OpenSecondMenu(gameObject,_member);
    }

    public void Uninitialize()
    {

    }


}
