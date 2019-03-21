using UnityEngine;
using System;
using System.Collections;

public class UnionSettingView
{
    public static string UIName ="UnionSettingView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionSettingView;
    public UIButton Btn_ChangeIcon;
    public UISprite Spt_BtnChangeIconBackground;
    public UILabel Lbl_BtnChangeIconLabel;
    public UIButton Btn_ChangeName;
    public UISprite Spt_BtnChangeNameBackground;
    public UILabel Lbl_BtnChangeNameLabel;

    public UILabel Lbl_Title;

    public UISprite Spt_Mask;

    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UISprite Spt_Icon;
    public UILabel Lbl_IconTip;
    public UILabel Lbl_NameTip;
    public UILabel Lbl_Name;
    public UILabel Lbl_LimitTypeTip;
    public UILabel Lbl_LimitType;
    public UIButton Btn_LimitTypeRight;
    public UIButton Btn_LimitTypeLeft;
    public UILabel Lbl_LimitLvTip;
    public UILabel Lbl_LimitLv;
    public UIButton Btn_LimitLvRight;
    public UIButton Btn_LimitLvLeft;
    public UIButton Btn_Confirm;
    public UILabel Lbl_BtnConfirmLabel;
    public UILabel Lbl_NoticeTip;
    public UIInput Ipt_Notice;
    public UILabel Lbl_InputNoticeLabel;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionSettingView");
        UIPanel_UnionSettingView = _uiRoot.GetComponent<UIPanel>();
        Btn_ChangeIcon = _uiRoot.transform.FindChild("ChangeIcon").gameObject.GetComponent<UIButton>();
        Spt_BtnChangeIconBackground = _uiRoot.transform.FindChild("ChangeIcon/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnChangeIconLabel = _uiRoot.transform.FindChild("ChangeIcon/Label").gameObject.GetComponent<UILabel>();
        Btn_ChangeName = _uiRoot.transform.FindChild("ChangeName").gameObject.GetComponent<UIButton>();
        Spt_BtnChangeNameBackground = _uiRoot.transform.FindChild("ChangeName/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnChangeNameLabel = _uiRoot.transform.FindChild("ChangeName/Label").gameObject.GetComponent<UILabel>();
        Lbl_Title = _uiRoot.transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Spt_IconBG = _uiRoot.transform.FindChild("Icon/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = _uiRoot.transform.FindChild("Icon/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = _uiRoot.transform.FindChild("Icon/Icon").gameObject.GetComponent<UISprite>();
        Lbl_IconTip = _uiRoot.transform.FindChild("IconTip").gameObject.GetComponent<UILabel>();
        Lbl_NameTip = _uiRoot.transform.FindChild("NameTip").gameObject.GetComponent<UILabel>();
        Lbl_Name = _uiRoot.transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_LimitTypeTip = _uiRoot.transform.FindChild("LimitType/LimitTypeTip").gameObject.GetComponent<UILabel>();
        Lbl_LimitType = _uiRoot.transform.FindChild("LimitType/LimitType").gameObject.GetComponent<UILabel>();
        Btn_LimitTypeRight = _uiRoot.transform.FindChild("LimitType/LimitTypeRight").gameObject.GetComponent<UIButton>();
        Btn_LimitTypeLeft = _uiRoot.transform.FindChild("LimitType/LimitTypeLeft").gameObject.GetComponent<UIButton>();
        Lbl_LimitLvTip = _uiRoot.transform.FindChild("LimitLv/LimitLvTip").gameObject.GetComponent<UILabel>();
        Lbl_LimitLv = _uiRoot.transform.FindChild("LimitLv/LimitLv").gameObject.GetComponent<UILabel>();
        Btn_LimitLvRight = _uiRoot.transform.FindChild("LimitLv/LimitLvRight").gameObject.GetComponent<UIButton>();
        Btn_LimitLvLeft = _uiRoot.transform.FindChild("LimitLv/LimitLvLeft").gameObject.GetComponent<UIButton>();
        Btn_Confirm = _uiRoot.transform.FindChild("Confirm").gameObject.GetComponent<UIButton>();
        Lbl_BtnConfirmLabel = _uiRoot.transform.FindChild("Confirm/Label").gameObject.GetComponent<UILabel>();
        Lbl_NoticeTip = _uiRoot.transform.FindChild("NoticeTip").gameObject.GetComponent<UILabel>();
        Ipt_Notice = _uiRoot.transform.FindChild("Notice").gameObject.GetComponent<UIInput>();
        Lbl_InputNoticeLabel = _uiRoot.transform.FindChild("Notice/Label").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnChangeIconLabel.text = "修改";
        Lbl_BtnChangeNameLabel.text = "修改";
        Lbl_Title.text = "军团设置";
        Lbl_IconTip.text = "军团徽章";
        Lbl_NameTip.text = "军团名称";
        Lbl_Name.text = "说不在就不在";
        Lbl_LimitTypeTip.text = "限制类型";
        Lbl_LimitType.text = "说不在就不在";
        Lbl_LimitLvTip.text = "等级限制";
        Lbl_LimitLv.text = "60";
        Lbl_BtnConfirmLabel.text = "确认";
        Lbl_NoticeTip.text = "修改公告";
        Lbl_InputNoticeLabel.text = "点击输入公告";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
