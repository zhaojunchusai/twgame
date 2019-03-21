using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionHallViewController : UIBase
{
    public UnionHallView view;
    private UnionMember _selectedMember;
    private List<UnionMemberItem> _memberItems = new List<UnionMemberItem>();
    private Dictionary<uint, UnionMemberItem> _memberItemDic = new Dictionary<uint, UnionMemberItem>();
    private int _page;
    private const int COUNT_PER_PAGE = 15;
    private Union _unionInfo;
    private UnionMember _myUnionMemberInfo;
    
    public override void Initialize()
    {
        if (view == null)
            view = new UnionHallView();
        view.Initialize();
        BtnEventBinding();
		view.Grd_Members.hideInactive = true;
        view.ScrView_Members.onDragFinishedDown = ReqNewPage;
        ShowPersonalInfo();
        ShowMembersOriginal();
        SetNotice();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _memberItems.Clear();
        _memberItemDic.Clear();
        _unionInfo = null;
        _myUnionMemberInfo = null;
        _selectedMember = null;
    }

    public void SetNotice()
    {
        view.Spt_BtnUnionManageNotice.gameObject.SetActive(_unionInfo.pending_members.Count >0);
    }

    public void ShowPersonalInfo()
    {
        _myUnionMemberInfo = UnionModule.Instance.MyUnionMemberInfo;
        CommonFunction.SetHeadAndFrameSprite(view.Spt_Icon, view.Spt_IconFrame, PlayerData.Instance.HeadID,
                                             PlayerData.Instance.FrameID, true);
        view.Lbl_CharLv.text = string.Format(ConstString.BACKPACK_SOLDIEREQUIPLEVEL, PlayerData.Instance._Level);
        view.Lbl_CharJob.text = string.Format(ConstString.FORMAT_UNION_JOB,
                                              CommonFunction.GetUnionMemberJobString(_myUnionMemberInfo.position));
        view.Lbl_VipLv.text = PlayerData.Instance._VipLv.ToString();
        view.Lbl_WeekLiveness.text = string.Format(ConstString.FORMAT_THIS_WEEK_LIVENESS_PERSONAL,
                                                   _myUnionMemberInfo.vitality);
        ShowHpRecord();
        SetBtns();
    }

    public void ShowHpRecord()
    {
        view.Lbl_GetHp.text = string.Format(ConstString.FORMAT_TODAY_HP_GET_COUNT,
            UnionModule.Instance.CharUnionInfo.get_union_hp_times,
            ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mGetHpTimesLimit);
        view.Lbl_SendHpTimes.text = string.Format(ConstString.FORMAT_TODAY_HP_SEND_COUNT,
                                                  _myUnionMemberInfo.interviewees.Count,
                                                  ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mVisitTimesLimit);
    }

    public void DelOne(int index)
    {
        int maxIndex = Mathf.Min(_page * COUNT_PER_PAGE , _unionInfo.members.Count +1);
        _memberItemDic.Remove(_memberItems[index].Member.charid);
        for (int i = index; i < maxIndex - 1; i++)
        {
            _memberItems[i].InitItem(_memberItems[i + 1].Member);
            if(_memberItemDic.ContainsKey(_memberItems[i].Member.charid))
            {
                _memberItemDic[_memberItems[i].Member.charid] = _memberItems[i];
            }
        }
        _memberItems[maxIndex - 1].gameObject.SetActive(false);
        view.ScrView_Members.ResetPosition();
    }

    private void SetBtns()
    {
        if (_myUnionMemberInfo.position == UnionPosition.UP_MEMBER)
        {
            view.Btn_UnionManage.gameObject.SetActive(false);
            view.Btn_UnionSetting.gameObject.SetActive(false);
        }
        if (_myUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN)
        {
            view.Btn_UnionManage.gameObject.SetActive(true);
            view.Btn_UnionSetting.gameObject.SetActive(false);
        }
        if (_myUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN)
        {
            view.Btn_UnionManage.gameObject.SetActive(true);
            view.Btn_UnionSetting.gameObject.SetActive(true);
        }
    }

    public void ShowMembersOriginal()
    {
        _unionInfo = UnionModule.Instance.UnionInfo;
        _page = 1;
        _memberItemDic.Clear();
        view.ScrView_Members.ResetPosition();
        ShowMembersByPage();
    }

    private void ShowMembersByPage()
    {
        int minIndex = _page*COUNT_PER_PAGE - COUNT_PER_PAGE;
        int maxIndex = Mathf.Min(_page*COUNT_PER_PAGE , _unionInfo.members.Count);
        for (int i = minIndex; i < maxIndex; i++)
        {
            if (i >= _memberItems.Count)
            {
                _memberItems.Add(InstantiateMemberItem());
            }
            _memberItems[i].gameObject.SetActive(true);
            _memberItems[i].InitItem(_unionInfo.members[i]);
            _memberItemDic.Add(_unionInfo.members[i].charid, _memberItems[i]);
        }

        for (int i = maxIndex; i < _memberItems.Count; i++)
        {
            _memberItems[i].gameObject.SetActive(false);
        }
        view.Grd_Members.Reposition();
    }

    private UnionMemberItem InstantiateMemberItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionMemberItem, view.Grd_Members.transform);
        return go.AddComponent<UnionMemberItem>();
    }

    private void ReqNewPage()
    {
        if (_unionInfo.members.Count <= _page*COUNT_PER_PAGE)
        {
            return;
        }
        ++_page;
        ShowMembersByPage();
    }

    public void RefreashItemAll()
    {
        for (int i = 0; i < _memberItems.Count; i++)
        {
            _memberItems[i].Refreash();
        }
    }

    public UnionMember RefreashItem(uint charid)
    {
        if (_memberItemDic.ContainsKey(charid))
        {
            _memberItemDic[charid].Refreash();
            return _memberItemDic[charid].Member;
        }
        else
        {            
            return null;
        }
    }

    public void OpenSecondMenu(GameObject item,UnionMember member)
    {
        view.Gobj_SecondMenu.SetActive(true);
        float y = item.transform.localPosition.y + view.Grd_Members.transform.localPosition.y +
                  view.ScrView_Members.transform.localPosition.y;
        _selectedMember = member;

        if (_myUnionMemberInfo.position == UnionPosition.UP_MEMBER)
        {
            ButtonEvent_CloseSecondMenu(null);
            return;
        }
        else if (_myUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN)
        {
            if (MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Slave))
            {
                ButtonEvent_CloseSecondMenu(null);
                return;
            }
            view.Btn_SlaveControl.gameObject.SetActive(true);
            view.Btn_Kick.gameObject.SetActive(true);
            view.Btn_Job.gameObject.SetActive(false);
            view.Btn_Nominate.gameObject.SetActive(false);
            view.Spt_SecBG.width = 160;
            view.Spt_SecBG.height = 160;
            if (76 + y -160 < -288)
            {
                view.Gobj_Menu.transform.localPosition = new Vector3(274, 76 + y + 160 +35, 0);                
            }
            else
            {
                view.Gobj_Menu.transform.localPosition = new Vector3(274, 76 + y, 0);                
            }
        }
        else if (_myUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN)
        {
            view.Btn_Kick.gameObject.SetActive(true);
            view.Btn_Job.gameObject.SetActive(true);
            view.Btn_Nominate.gameObject.SetActive(true);
            view.Spt_SecBG.width = 160;
            view.Spt_SecBG.height = 300;
            view.Gobj_Menu.transform.localPosition = new Vector3(274, 76, 0);

            if(MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Slave))
            {
                view.Btn_SlaveControl.gameObject.SetActive(false);
                view.Spt_SecBG.height = 230;
                view.Btn_Kick.transform.localPosition = Vector3.zero;
                view.Btn_Job.transform.localPosition = new Vector3(0, -70, 0);
                view.Btn_Nominate.transform.localPosition = new Vector3(0, -140, 0);
            }
            else
            {
                view.Btn_SlaveControl.gameObject.SetActive(true);
                view.Btn_Kick.transform.localPosition = new Vector3(0,-70,0);
                view.Btn_Job.transform.localPosition = new Vector3(0,-140,0);
                view.Btn_Nominate.transform.localPosition = new Vector3(0,-210,0);
            }
        }

        if (_selectedMember.position == UnionPosition.UP_MEMBER)
            view.Lbl_BtnJobLb.text = ConstString.UNION_JOB_UP;

        if (_selectedMember.position == UnionPosition.UP_VICE_CHAIRMAN)
            view.Lbl_BtnJobLb.text = ConstString.UNION_JOB_DOWN;

		if (_selectedMember.slave_state == (int)UnionMemberItem.SlaveState.Slave)
		{
			view.Lbl_BtnSlaveControlLb.text = ConstString.UNION_SLAVE_RESCUE;
		}
		else
		{
			view.Lbl_BtnSlaveControlLb.text = ConstString.UNION_SLAVE_ENSLAVED;
		}
    }

    public void ButtonEvent_CloseSecondMenu(GameObject btn)
    {
        view.Gobj_SecondMenu.SetActive(false);
    }

    private void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(UnionHallView.UIName);
        UnionModule.Instance.OpenUnion();
    }

    private void ButtonEvent_OneKeyGetHp(GameObject btn)
    {
        UnionModule.Instance.OnSendOneKeyGetUnionHp();
    }

    private void ButtonEvent_UnionManage(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(UnionApplyView.UIName);
    }

    private void ButtonEvent_UnionSetting(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(UnionSettingView.UIName);
    }

    private void ButtonEvent_QuitUnion(GameObject btn)
    {

        if (UnionModule.Instance.UnionInfo.members.Count+1 == 1)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.UNION_QUIT,
                UnionModule.Instance.OnSendLeaveUnion);
        }
        else 
        {
            uint timer = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().LeaveUnionCD;
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,
                string.Format(ConstString.UNION_TIP_QUIT, timer/3600),
                UnionModule.Instance.OnSendLeaveUnion);
        }
    }

    private void ButtonEvent_SlaveControl(GameObject btn)
    {
        if (_selectedMember.slave_state == (int)UnionMemberItem.SlaveState.Slave)
        {
            PlayerData.Instance._Prison.GetOtherPlayerInfo(_selectedMember.accname, _selectedMember.area_id);
        }
        else
        {
            PlayerData.Instance._Prison.GetOtherPlayerInfo(_selectedMember.accname, _selectedMember.area_id);
        }
        ButtonEvent_CloseSecondMenu(null);
    }

    private void ButtonEvent_Kick(GameObject btn)
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.FORMAT_UNION_KICK, _selectedMember.charname),
                () => UnionModule.Instance.OnSendManageUnionMember(ManageUnionType.MUT_KICK, _selectedMember.charid)
                );
    }

    private void ButtonEvent_Job(GameObject btn)
    {
        if (_selectedMember.position == UnionPosition.UP_MEMBER)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.FORMAT_UNION_PROMOTION, _selectedMember.charname),
                () => UnionModule.Instance.OnSendManageUnionMember(ManageUnionType.MUT_PROMOTION, _selectedMember.charid)
                );
        }

        if (_selectedMember.position == UnionPosition.UP_VICE_CHAIRMAN)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,string.Format(ConstString.FORMAT_UNION_DEMOTION, _selectedMember.charname),
                () => UnionModule.Instance.OnSendManageUnionMember(ManageUnionType.MUT_DEMOTION, _selectedMember.charid)
                );
        }
    }
    
    private void ButtonEvent_Nominate(GameObject btn)
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,
            string.Format(ConstString.FORMAT_UNION_NOMINATE, _selectedMember.charname, CommonFunction.GetUnionMemberJobString(_selectedMember.position)),
          () => UnionModule.Instance.OnSendManageUnionMember(ManageUnionType.MUT_ABDICATE, _selectedMember.charid)
          ); 
    }
    
    public override void Uninitialize()
    {
        view.ScrView_Members.onDragFinishedDown = null;
    }

    private void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_SecMask.gameObject).onClick = ButtonEvent_CloseSecondMenu;
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_OneKeyGetHp.gameObject).onClick = ButtonEvent_OneKeyGetHp;
        UIEventListener.Get(view.Btn_UnionManage.gameObject).onClick = ButtonEvent_UnionManage;
        UIEventListener.Get(view.Btn_UnionSetting.gameObject).onClick = ButtonEvent_UnionSetting;
        UIEventListener.Get(view.Btn_QuitUnion.gameObject).onClick = ButtonEvent_QuitUnion;
        UIEventListener.Get(view.Btn_SlaveControl.gameObject).onClick = ButtonEvent_SlaveControl;
        UIEventListener.Get(view.Btn_Kick.gameObject).onClick = ButtonEvent_Kick;
        UIEventListener.Get(view.Btn_Job.gameObject).onClick = ButtonEvent_Job;
        UIEventListener.Get(view.Btn_Nominate.gameObject).onClick = ButtonEvent_Nominate;
        
    }


}
