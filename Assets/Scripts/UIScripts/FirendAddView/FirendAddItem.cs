using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class FirendAddItem : MonoBehaviour
{
    public UILabel Lbl_Name;
    public UILabel Lbl_OfflineTime;
    public UILabel Lbl_FightPower;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UISprite Spt_Icon;
    public UILabel Lbl_CharLv;
    public UIButton Btn_Agree;
    public UILabel Lbl_Agree;
    private bool _initialized = false;
    private BasePlayerInfo _member;
    public BasePlayerInfo MemberInfo
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
        Lbl_Agree = transform.FindChild("Agree/Label").gameObject.GetComponent<UILabel>();
        SetLabelValues();

        UIEventListener.Get(Btn_Agree.gameObject).onClick = ButtonEvent_Agree;
    }

    public void InitItem(BasePlayerInfo member)
    {
        Initialize();
        _member = member;
        Lbl_Name.text = _member.charname;
        Lbl_CharLv.text = _member.level.ToString();
        CommonFunction.SetHeadAndFrameSprite(Spt_Icon, Spt_IconFrame, (uint)_member.icon, (uint)_member.icon_frame, true);
        Lbl_FightPower.text = _member.combat_power.ToString();
        Lbl_OfflineTime.text = string.Format(ConstString.REMOVEBLOCKLEGION, string.IsNullOrEmpty(_member.union_name) ? ConstString.PRISON_DO_EMPTY : _member.union_name);
        if(_member.status == 0)
        {
            Lbl_Agree.text = ConstString.FIREND_ADD;
            CommonFunction.SetGameObjectGray(this.Btn_Agree.gameObject,false);
        }
        else
        {
            Lbl_Agree.text = ConstString.FIREND_HAD_ADD;
            CommonFunction.SetGameObjectGray(this.Btn_Agree.gameObject,true);
        }
    }
    
    public void SetLabelValues()
    {
    }

    public void ButtonEvent_Agree(GameObject btn)
    {
        if (this.MemberInfo == null)
            return;
        if (_member.status == 0)
            FriendModule.Instance.SendApplyAddFriendsReq(this.MemberInfo.accname, this.MemberInfo.accid);
    }


    public void Uninitialize()
    {

    }
  

}
