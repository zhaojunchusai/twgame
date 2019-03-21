using UnityEngine;
using System;
using System.Collections;

public class CaptureTokenView
{
    public static string UIName ="CaptureTokenView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_CaptureTokenView;
    public UILabel Lbl_MidDesc1;
    public UILabel Lbl_MidDesc2;
    public UILabel Lbl_MidDesc3;
    public UILabel Lbl_MidPrice;
    public UISprite Spt_MidBG;
    public UISprite Spt_MidToken;
    public UISprite Spt_Line3;
    public UISprite Spt_Line4;
    public UISprite Spt_MoneyIcon2;
    public UILabel Lbl_LeftDesc1;
    public UILabel Lbl_LeftDesc2;
    public UILabel Lbl_LeftDesc3;
    public UILabel Lbl_LeftPrice;
    public UISprite Spt_LeftBG;
    public UISprite Spt_LeftToken;
    public UISprite Spt_Line;
    public UISprite Spt_Line2;
    public UISprite Spt_MoneyIcon1;
    public UILabel Lbl_RightDesc1;
    public UILabel Lbl_RightDesc2;
    public UILabel Lbl_RightDesc3;
    public UILabel Lbl_RightPrice;
    public UISprite Spt_RightBG;
    public UISprite Spt_RightToken;
    public UISprite Spt_Line5;
    public UISprite Spt_Line6;
    public UISprite Spt_MoneyIcon3;
    public UISprite Spt_BG;
    public UISprite Spt_FG;
    public UISprite Spt_Flower;
    public UISprite Spt_Flower1;
    public UILabel Lbl_TitleLb;
    public UISprite Spt_TitleLiftBG;
    public UISprite Spt_TitleRightBG;
    public UILabel Lbl_TokenDesc;
    public UISprite Spt_Mask;
    public UISpriteAnimation Effect1;
    public UISpriteAnimation Effect2;
    public UISpriteAnimation Effect3;


    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CaptureTokenView");
        UIPanel_CaptureTokenView = _uiRoot.GetComponent<UIPanel>();
        Lbl_MidDesc1 = _uiRoot.transform.FindChild("Anim/Mid/MidDesc1").gameObject.GetComponent<UILabel>();
        Lbl_MidDesc2 = _uiRoot.transform.FindChild("Anim/Mid/MidDesc2").gameObject.GetComponent<UILabel>();
        Lbl_MidDesc3 = _uiRoot.transform.FindChild("Anim/Mid/MidDesc3").gameObject.GetComponent<UILabel>();
        Lbl_MidPrice = _uiRoot.transform.FindChild("Anim/Mid/MidPrice").gameObject.GetComponent<UILabel>();
        Spt_MidBG = _uiRoot.transform.FindChild("Anim/Mid/MidBG").gameObject.GetComponent<UISprite>();
        Spt_MidToken = _uiRoot.transform.FindChild("Anim/Mid/MidToken").gameObject.GetComponent<UISprite>();
        Spt_Line3 = _uiRoot.transform.FindChild("Anim/Mid/Line3").gameObject.GetComponent<UISprite>();
        Spt_Line4 = _uiRoot.transform.FindChild("Anim/Mid/Line4").gameObject.GetComponent<UISprite>();
        Spt_MoneyIcon2 = _uiRoot.transform.FindChild("Anim/Mid/MoneyIcon2").gameObject.GetComponent<UISprite>();
        Lbl_LeftDesc1 = _uiRoot.transform.FindChild("Anim/Left/LeftDesc1").gameObject.GetComponent<UILabel>();
        Lbl_LeftDesc2 = _uiRoot.transform.FindChild("Anim/Left/LeftDesc2").gameObject.GetComponent<UILabel>();
        Lbl_LeftDesc3 = _uiRoot.transform.FindChild("Anim/Left/LeftDesc3").gameObject.GetComponent<UILabel>();
        Lbl_LeftPrice = _uiRoot.transform.FindChild("Anim/Left/LeftPrice").gameObject.GetComponent<UILabel>();
        Spt_LeftBG = _uiRoot.transform.FindChild("Anim/Left/LeftBG").gameObject.GetComponent<UISprite>();
        Spt_LeftToken = _uiRoot.transform.FindChild("Anim/Left/LeftToken").gameObject.GetComponent<UISprite>();
        Spt_Line = _uiRoot.transform.FindChild("Anim/Left/Line").gameObject.GetComponent<UISprite>();
        Spt_Line2 = _uiRoot.transform.FindChild("Anim/Left/Line2").gameObject.GetComponent<UISprite>();
        Spt_MoneyIcon1 = _uiRoot.transform.FindChild("Anim/Left/MoneyIcon1").gameObject.GetComponent<UISprite>();
        Lbl_RightDesc1 = _uiRoot.transform.FindChild("Anim/Right/RightDesc1").gameObject.GetComponent<UILabel>();
        Lbl_RightDesc2 = _uiRoot.transform.FindChild("Anim/Right/RightDesc2").gameObject.GetComponent<UILabel>();
        Lbl_RightDesc3 = _uiRoot.transform.FindChild("Anim/Right/RightDesc3").gameObject.GetComponent<UILabel>();
        Lbl_RightPrice = _uiRoot.transform.FindChild("Anim/Right/RightPrice").gameObject.GetComponent<UILabel>();
        Spt_RightBG = _uiRoot.transform.FindChild("Anim/Right/RightBG").gameObject.GetComponent<UISprite>();
        Spt_RightToken = _uiRoot.transform.FindChild("Anim/Right/RightToken").gameObject.GetComponent<UISprite>();
        Spt_Line5 = _uiRoot.transform.FindChild("Anim/Right/Line5").gameObject.GetComponent<UISprite>();
        Spt_Line6 = _uiRoot.transform.FindChild("Anim/Right/Line6").gameObject.GetComponent<UISprite>();
        Spt_MoneyIcon3 = _uiRoot.transform.FindChild("Anim/Right/MoneyIcon3").gameObject.GetComponent<UISprite>();
        Spt_BG = _uiRoot.transform.FindChild("Anim/BGS/BG").gameObject.GetComponent<UISprite>();
        Spt_FG = _uiRoot.transform.FindChild("Anim/BGS/FG").gameObject.GetComponent<UISprite>();
        Spt_Flower = _uiRoot.transform.FindChild("Anim/BGS/Flower").gameObject.GetComponent<UISprite>();
        Spt_Flower1 = _uiRoot.transform.FindChild("Anim/BGS/Flower1").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Spt_TitleLiftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleBGLow/TitleLiftBG").gameObject.GetComponent<UISprite>();
        Spt_TitleRightBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleBGLow/TitleRightBG").gameObject.GetComponent<UISprite>();
        Lbl_TokenDesc = _uiRoot.transform.FindChild("Anim/TokenDesc").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Effect1 = _uiRoot.transform.FindChild("Anim/Left/Effect1").gameObject.GetComponent<UISpriteAnimation>();
        Effect2 = _uiRoot.transform.FindChild("Anim/Mid/Effect2").gameObject.GetComponent<UISpriteAnimation>();
        Effect3 = _uiRoot.transform.FindChild("Anim/Right/Effect3").gameObject.GetComponent<UISpriteAnimation>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_MidDesc1.text = "";
        Lbl_MidDesc2.text = "";
        Lbl_MidDesc3.text = "";
        Lbl_MidPrice.text = "";
        Lbl_LeftDesc1.text = "";
        Lbl_LeftDesc2.text = "";
        Lbl_LeftDesc3.text = "";
        Lbl_LeftPrice.text = "";
        Lbl_RightDesc1.text = "";
        Lbl_RightDesc2.text = "";
        Lbl_RightDesc3.text = "";
        Lbl_RightPrice.text = "";
        Lbl_TitleLb.text = ConstString.LBL_CAMPAIN_TOKEN;
        Lbl_TokenDesc.text = ConstString.LBL_CAMPAIN_TOKEN_GET_BACK;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
