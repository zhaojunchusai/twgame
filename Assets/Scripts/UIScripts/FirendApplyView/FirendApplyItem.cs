using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class FirendApplyItem : MonoBehaviour
{
    public UILabel Lbl_Name;
    public UILabel Lbl_OfflineTime;
    public UILabel Lbl_FightPower;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UISprite Spt_Icon;
    public UILabel Lbl_CharLv;
    public UIButton Btn_Agree;
    public UIButton Btn_Refuse;

    private bool _initialized = false;
    private AppendFriends _member;
    public AppendFriends MemberInfo
    {
        get { return _member; }
    }

    public void Initialize()
    {
        if(_initialized)
            return;
        _initialized = true;

        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_OfflineTime = transform.FindChild("OfflineTime").gameObject.GetComponent<UILabel>();
        Lbl_FightPower = transform.FindChild("FightPower").gameObject.GetComponent<UILabel>();
        Spt_IconBG = transform.FindChild("Head/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = transform.FindChild("Head/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Head/Icon").gameObject.GetComponent<UISprite>();
        Lbl_CharLv = transform.FindChild("CharLv").gameObject.GetComponent<UILabel>();
        Btn_Agree = transform.FindChild("Agree").gameObject.GetComponent<UIButton>();
        Btn_Refuse = transform.FindChild("Refuse").gameObject.GetComponent<UIButton>();
        SetLabelValues();

        UIEventListener.Get(Btn_Agree.gameObject).onClick = ButtonEvent_Agree;
        UIEventListener.Get(Btn_Refuse.gameObject).onClick = ButtonEvent_Refuse;
    }

    public void InitItem(AppendFriends member)
    {
        Initialize();

        _member = member;
        if (_member == null || _member.append_friends == null)
            return;

        Lbl_Name.text = _member.append_friends.charname;
        Lbl_CharLv.text = _member.append_friends.level.ToString();
        CommonFunction.SetHeadAndFrameSprite(Spt_Icon, Spt_IconFrame, (uint)_member.append_friends.icon, (uint)_member.append_friends.icon_frame, true);
        Lbl_FightPower.text = _member.append_friends.combat_power.ToString();
        Lbl_OfflineTime.text = string.Format(ConstString.REMOVEBLOCKLEGION, _member.append_friends.union_name);
    }
    
    public void SetLabelValues()
    {
    }

    public void ButtonEvent_Agree(GameObject btn)
    {
        if (this.MemberInfo == null || this.MemberInfo.append_friends == null)
            return;

        AccnameToAccid tmpInfo = new AccnameToAccid();
        tmpInfo.accid = this.MemberInfo.append_friends.accid;
        tmpInfo.accname = this.MemberInfo.append_friends.accname;

        FriendModule.Instance.SendAuditAppendFriendsReq(AuditFriendsOpType.AFO_ACCPET, FriendsOpType.FO_ONE, tmpInfo);

    }

    public void ButtonEvent_Refuse(GameObject btn)
    {
        if (this.MemberInfo == null || this.MemberInfo.append_friends == null)
            return;

        AccnameToAccid tmpInfo = new AccnameToAccid();
        tmpInfo.accid = this.MemberInfo.append_friends.accid;
        tmpInfo.accname = this.MemberInfo.append_friends.accname;

        FriendModule.Instance.SendAuditAppendFriendsReq(AuditFriendsOpType.AFO_REFUSE, FriendsOpType.FO_ONE, tmpInfo);
    }

    public void Uninitialize()
    {

    }


}
