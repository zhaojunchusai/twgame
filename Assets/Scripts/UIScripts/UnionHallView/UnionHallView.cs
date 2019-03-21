using UnityEngine;
using System;
using System.Collections;

public class UnionHallView
{
    public static string UIName ="UnionHallView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionHallView;
    public UILabel Lbl_Title;
    public UIButton Btn_Close;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UISprite Spt_Icon;
    public UILabel Lbl_CharLv;
    public UILabel Lbl_CharJob;
    public UILabel Lbl_WeekLiveness;
    public UILabel Lbl_GetHp;
    public UILabel Lbl_SendHpTimes;
    public UISprite Spt_Vip;
    public UILabel Lbl_VipLv;
    public UIGrid Grd_Btns;
    public UIButton Btn_OneKeyGetHp;
    public UILabel Lbl_BtnOneKeyGetHpLabel;
    public UIButton Btn_UnionManage;
    public UILabel Lbl_BtnUnionManageLabel;
    public UISprite Spt_BtnUnionManageNotice;
    public UIButton Btn_UnionSetting;
    public UILabel Lbl_BtnUnionSettingLabel;
    public UIButton Btn_QuitUnion;
    public UILabel Lbl_BtnQuitUnionLabel;
    public UIScrollView ScrView_Members;
    public UIGrid Grd_Members;
    public GameObject Gobj_SecondMenu;
    public GameObject Gobj_Menu;
    public UIButton Btn_SlaveControl;
    public UILabel Lbl_BtnSlaveControlLb;
    public UIButton Btn_Kick;
    public UILabel Lbl_BtnKickLb;
    public UIButton Btn_Job;
    public UILabel Lbl_BtnJobLb;
    public UIButton Btn_Nominate;
    public UILabel Lbl_BtnNominateLb;
    public UISprite Spt_SecMask;
    public UISprite Spt_SecBG;

    public GameObject Gobj_UnionMemberItem;
    
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionHallView");
        UIPanel_UnionHallView = _uiRoot.GetComponent<UIPanel>();
        Lbl_Title = _uiRoot.transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Btn_Close = _uiRoot.transform.FindChild("Close").gameObject.GetComponent<UIButton>();
        Spt_IconBG = _uiRoot.transform.FindChild("Personal/Head/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = _uiRoot.transform.FindChild("Personal/Head/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = _uiRoot.transform.FindChild("Personal/Head/Icon").gameObject.GetComponent<UISprite>();
        Lbl_CharLv = _uiRoot.transform.FindChild("Personal/CharLv").gameObject.GetComponent<UILabel>();
        Lbl_CharJob = _uiRoot.transform.FindChild("Personal/CharJob").gameObject.GetComponent<UILabel>();
        Lbl_WeekLiveness = _uiRoot.transform.FindChild("Personal/WeekLiveness").gameObject.GetComponent<UILabel>();
        Lbl_GetHp = _uiRoot.transform.FindChild("Personal/GetHp").gameObject.GetComponent<UILabel>();
        Lbl_SendHpTimes = _uiRoot.transform.FindChild("Personal/SendHpTimes").gameObject.GetComponent<UILabel>();
        Spt_Vip = _uiRoot.transform.FindChild("Personal/Vip").gameObject.GetComponent<UISprite>();
        Lbl_VipLv = _uiRoot.transform.FindChild("Personal/VipLv").gameObject.GetComponent<UILabel>();
        Grd_Btns = _uiRoot.transform.FindChild("Btns").gameObject.GetComponent<UIGrid>();
        Btn_OneKeyGetHp = _uiRoot.transform.FindChild("Btns/OneKeyGetHp").gameObject.GetComponent<UIButton>();
        Lbl_BtnOneKeyGetHpLabel = _uiRoot.transform.FindChild("Btns/OneKeyGetHp/Label").gameObject.GetComponent<UILabel>();
        Btn_UnionManage = _uiRoot.transform.FindChild("Btns/UnionManage").gameObject.GetComponent<UIButton>();
        Lbl_BtnUnionManageLabel = _uiRoot.transform.FindChild("Btns/UnionManage/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnUnionManageNotice = _uiRoot.transform.FindChild("Btns/UnionManage/Notice").gameObject.GetComponent<UISprite>();
        Btn_UnionSetting = _uiRoot.transform.FindChild("Btns/UnionSetting").gameObject.GetComponent<UIButton>();
        Lbl_BtnUnionSettingLabel = _uiRoot.transform.FindChild("Btns/UnionSetting/Label").gameObject.GetComponent<UILabel>();
        Btn_QuitUnion = _uiRoot.transform.FindChild("Btns/QuitUnion").gameObject.GetComponent<UIButton>();
        Lbl_BtnQuitUnionLabel = _uiRoot.transform.FindChild("Btns/QuitUnion/Label").gameObject.GetComponent<UILabel>();
        ScrView_Members = _uiRoot.transform.FindChild("Members").gameObject.GetComponent<UIScrollView>();
        Grd_Members = _uiRoot.transform.FindChild("Members/Members").gameObject.GetComponent<UIGrid>();
        Gobj_SecondMenu = _uiRoot.transform.FindChild("gobj_SecondMenu").gameObject;
        Gobj_Menu = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu").gameObject;
        Btn_SlaveControl = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/SlaveControl").gameObject.GetComponent<UIButton>();
        Lbl_BtnSlaveControlLb = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/SlaveControl/Lb").gameObject.GetComponent<UILabel>();
        Btn_Kick = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/Kick").gameObject.GetComponent<UIButton>();
        Lbl_BtnKickLb = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/Kick/Lb").gameObject.GetComponent<UILabel>();
        Btn_Job = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/Job").gameObject.GetComponent<UIButton>();
        Lbl_BtnJobLb = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/Job/Lb").gameObject.GetComponent<UILabel>();
        Btn_Nominate = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/Nominate").gameObject.GetComponent<UIButton>();
        Lbl_BtnNominateLb = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/Nominate/Lb").gameObject.GetComponent<UILabel>();
        Spt_SecMask = _uiRoot.transform.FindChild("gobj_SecondMenu/Mask").gameObject.GetComponent<UISprite>();
        Spt_SecBG = _uiRoot.transform.FindChild("gobj_SecondMenu/gobj_Menu/BG").gameObject.GetComponent<UISprite>();

        Gobj_UnionMemberItem = _uiRoot.transform.FindChild("Pre/UnionMemberItem").gameObject;
        
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "军团大厅";
        Lbl_CharLv.text = "等级:99";
        Lbl_CharJob.text = "职位:成员成员";
        Lbl_WeekLiveness.text = "本周活跃:    99999";
        Lbl_GetHp.text = "今日领取:    49/50";
        Lbl_SendHpTimes.text = "今日拜访:    6/10";
        Lbl_VipLv.text = "1";
        Lbl_BtnOneKeyGetHpLabel.text = "一键领取";
        Lbl_BtnUnionManageLabel.text = "入会申请";
        Lbl_BtnUnionSettingLabel.text = "军团设置";
        Lbl_BtnQuitUnionLabel.text = "退出军团";
        Lbl_BtnSlaveControlLb.text = "奴役";
        Lbl_BtnKickLb.text = "开除";
        Lbl_BtnJobLb.text = "降职";
        Lbl_BtnNominateLb.text = "让位";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
