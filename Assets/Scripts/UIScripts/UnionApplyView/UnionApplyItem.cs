using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class UnionApplyItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UISprite Spt_Line;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UILabel Lbl_OfflineTime;
    [HideInInspector]public UILabel Lbl_FightPower;
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_IconFrame;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_LvBg;
    [HideInInspector]public UILabel Lbl_CharLv;
    [HideInInspector]public UIButton Btn_Agree;
    [HideInInspector]public UISprite Spt_BtnAgreeBackground;
    [HideInInspector]public UILabel Lbl_BtnAgreeLabel;
    [HideInInspector]public UIButton Btn_Refuse;
    [HideInInspector]public UISprite Spt_BtnRefuseBackground;
    [HideInInspector]public UILabel Lbl_BtnRefuseLabel;

    private bool _initialized = false;
    private PendingUnionMember _member;
    public PendingUnionMember MemberInfo
    {
        get { return _member; }
    }

    public void Initialize()
    {
        if(_initialized)
            return;
        _initialized = true;

        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_Line = transform.FindChild("Line").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_OfflineTime = transform.FindChild("OfflineTime").gameObject.GetComponent<UILabel>();
        Lbl_FightPower = transform.FindChild("FightPower").gameObject.GetComponent<UILabel>();
        Spt_IconBG = transform.FindChild("Head/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = transform.FindChild("Head/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Head/Icon").gameObject.GetComponent<UISprite>();
        Spt_LvBg = transform.FindChild("LvBg").gameObject.GetComponent<UISprite>();
        Lbl_CharLv = transform.FindChild("CharLv").gameObject.GetComponent<UILabel>();
        Btn_Agree = transform.FindChild("Agree").gameObject.GetComponent<UIButton>();
        Spt_BtnAgreeBackground = transform.FindChild("Agree/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnAgreeLabel = transform.FindChild("Agree/Label").gameObject.GetComponent<UILabel>();
        Btn_Refuse = transform.FindChild("Refuse").gameObject.GetComponent<UIButton>();
        Spt_BtnRefuseBackground = transform.FindChild("Refuse/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnRefuseLabel = transform.FindChild("Refuse/Label").gameObject.GetComponent<UILabel>();
        //SetLabelValues();

        UIEventListener.Get(Btn_Agree.gameObject).onClick = ButtonEvent_Agree;
        UIEventListener.Get(Btn_Refuse.gameObject).onClick = ButtonEvent_Refuse;
    }

    public void InitItem(PendingUnionMember member)
    {
        Initialize();
        _member = member;
        Lbl_Name.text = _member.member.charname;
        Lbl_CharLv.text = _member.member.level.ToString();
        CommonFunction.SetHeadAndFrameSprite(Spt_Icon, Spt_IconFrame, _member.member.icon, _member.member.icon_frame,true);
        Lbl_FightPower.text = string.Format(ConstString.FORMAT_UNION_MEMBER_FIGHT, _member.member.combat_power.ToString());
        //UnionModule.Instance.SetOfflineTime(Lbl_OfflineTime, _member.member.offline_tick);
        CommonFunction.SetOfflineTime(Lbl_OfflineTime, _member.member.offline_tick);

    }
    
    public void SetLabelValues()
    {
        Lbl_Name.text = "今天我不在";
        Lbl_OfflineTime.text = "最后离线:10分钟前";
        Lbl_FightPower.text = "最强战力:999999";
        Lbl_CharLv.text = "99";
        Lbl_BtnAgreeLabel.text = "同意";
        Lbl_BtnRefuseLabel.text = "拒绝";
    }

    public void ButtonEvent_Agree(GameObject btn)
    {
        UnionModule.Instance.OnSendAuditUnionMembers(UnionModule.UnionAuditType.Single,
                                                  UnionModule.UnionAuditPassType.Pass, _member.member.charid);
    }

    public void ButtonEvent_Refuse(GameObject btn)
    {
        UnionModule.Instance.OnSendAuditUnionMembers(UnionModule.UnionAuditType.Single,
                                                  UnionModule.UnionAuditPassType.Refuse, _member.member.charid);
    }

    public void Uninitialize()
    {

    }


}
