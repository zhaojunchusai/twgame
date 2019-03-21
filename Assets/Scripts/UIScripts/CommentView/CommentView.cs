using UnityEngine;
using System;
using System.Collections;

public class CommentView
{
    public static string UIName ="CommentView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_CommentView;
    public UIButton Btn_NoRemind;
    public UISprite Spt_BtnNoRemindBackground;
    public UILabel Lbl_BtnNoRemindLabel;
    public UIButton Btn_Refuse;
    public UISprite Spt_BtnRefuseBackground;
    public UILabel Lbl_BtnRefuseLabel;
    public UIButton Btn_Go;
    public UISprite Spt_BtnGoBackground;
    public UILabel Lbl_BtnGoLabel;
    public UIButton Btn_Confirm;
    public UISprite Spt_BtnConfirmBackground;
    public UILabel Lbl_BtnConfirmLabel;
    public UISprite Spt_BG;
    public UISprite Spt_Title1;
    public UISprite Spt_Title2;
    public UISprite Spt_People;
    public UILabel Lbl_Content;
    public UISprite Spt_Mask;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CommentView");
        UIPanel_CommentView = _uiRoot.GetComponent<UIPanel>();
        Btn_NoRemind = _uiRoot.transform.FindChild("Btns/NoRemind").gameObject.GetComponent<UIButton>();
        Spt_BtnNoRemindBackground = _uiRoot.transform.FindChild("Btns/NoRemind/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnNoRemindLabel = _uiRoot.transform.FindChild("Btns/NoRemind/Label").gameObject.GetComponent<UILabel>();
        Btn_Refuse = _uiRoot.transform.FindChild("Btns/Refuse").gameObject.GetComponent<UIButton>();
        Spt_BtnRefuseBackground = _uiRoot.transform.FindChild("Btns/Refuse/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnRefuseLabel = _uiRoot.transform.FindChild("Btns/Refuse/Label").gameObject.GetComponent<UILabel>();
        Btn_Go = _uiRoot.transform.FindChild("Btns/Go").gameObject.GetComponent<UIButton>();
        Spt_BtnGoBackground = _uiRoot.transform.FindChild("Btns/Go/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnGoLabel = _uiRoot.transform.FindChild("Btns/Go/Label").gameObject.GetComponent<UILabel>();
        Btn_Confirm = _uiRoot.transform.FindChild("Btns/Confirm").gameObject.GetComponent<UIButton>();
        Spt_BtnConfirmBackground = _uiRoot.transform.FindChild("Btns/Confirm/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirmLabel = _uiRoot.transform.FindChild("Btns/Confirm/Label").gameObject.GetComponent<UILabel>();
        Spt_BG = _uiRoot.transform.FindChild("Common/BG").gameObject.GetComponent<UISprite>();
        Spt_Title1 = _uiRoot.transform.FindChild("Common/Title1").gameObject.GetComponent<UISprite>();
        Spt_Title2 = _uiRoot.transform.FindChild("Common/Title2").gameObject.GetComponent<UISprite>();
        Spt_People = _uiRoot.transform.FindChild("Common/People").gameObject.GetComponent<UISprite>();
        Lbl_Content = _uiRoot.transform.FindChild("Common/Content").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_BtnNoRemindLabel.text = "不再提醒";
        //Lbl_BtnRefuseLabel.text = "残忍拒绝";
        //Lbl_BtnGoLabel.text = "欣然前往";
        //Lbl_BtnConfirmLabel.text = "确定";
        //Lbl_Content.text = "主公主公，给一个5星评定吧，人家会好好报答主公的~~~~";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
