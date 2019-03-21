using UnityEngine;
using System;
using System.Collections;

public class UnionView
{
    public static string UIName = "UnionView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionView;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconFrame;
    public UISprite Spt_Icon;
    public UISprite Spt_State;
    public UILabel Lbl_UnionName;
    public UILabel Lbl_UnionLv;
    public UIButton Btn_UnionDonation;
    public UILabel Lbl_UnionID;
    public UILabel Lbl_SupplyTip;
    public UILabel Lbl_SupplyNum;
    public UISlider Slider_SupplyProgress;
    public UIButton Btn_LvUp;
    public UIButton Btn_Hall;
    public UIButton Btn_Store;
    public UIButton Btn_Advanture;
    public UIButton Btn_Fight;
    public UIButton Btn_UnionPrison;
    public UIButton Btn_CaptureTerritory;
    public UIButton Btn_ShowNoticeTip;
    public UILabel Lbl_ShowNoticeTip;

    public UIButton Btn_UnionRule;
    public UILabel Lbl_NoticeContent;
    public UISprite Spt_InfoBG;
    public UIButton Btn_Close;
    public UISprite Spt_MaxSprite;
    public UIPanel MaskEffect;
    public Transform Trans_MoveContent;
    public UITexture Tex_BG;

    public UIButton Btn_CrossServerWar;

    public void Initialize()
    {
        //_uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionView");
        //UIPanel_UnionView = _uiRoot.GetComponent<UIPanel>();
        //MaskEffect = _uiRoot.transform.FindChild("Effect").gameObject.GetComponent<UIPanel>();
        //Btn_Close = _uiRoot.transform.FindChild("Anim/Up/Close").gameObject.GetComponent<UIButton>();
        //Spt_IconBG = _uiRoot.transform.FindChild("Anim/Up/Icon/IconBG").gameObject.GetComponent<UISprite>();
        //Spt_IconFrame = _uiRoot.transform.FindChild("Anim/Up/Icon/IconFrame").gameObject.GetComponent<UISprite>();
        //Spt_State = _uiRoot.transform.FindChild("Anim/Up/Icon/state").gameObject.GetComponent<UISprite>();
        //Spt_Icon = _uiRoot.transform.FindChild("Anim/Up/Icon/Icon").gameObject.GetComponent<UISprite>();
        //Lbl_UnionName = _uiRoot.transform.FindChild("Anim/Up/Icon/UnionName").gameObject.GetComponent<UILabel>();
        //Lbl_UnionID = _uiRoot.transform.FindChild("Anim/Up/Icon/UnionID").gameObject.GetComponent<UILabel>();
        //Btn_UnionDonation = _uiRoot.transform.FindChild("Anim/Up/Btn/UnionDonation").gameObject.GetComponent<UIButton>();
        //Btn_UnionRule = _uiRoot.transform.FindChild("Anim/Up/Btn/UnionRule").gameObject.GetComponent<UIButton>();
        //Spt_InfoBG = _uiRoot.transform.FindChild("Anim/Up/Info/InfoBG").gameObject.GetComponent<UISprite>();
        //Lbl_UnionLv = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/UnionLv").gameObject.GetComponent<UILabel>();
        //Spt_MaxSprite = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/MaxSprite").gameObject.GetComponent<UISprite>();
        //Lbl_SupplyTip = _uiRoot.transform.FindChild("Anim/Up/Info/SupplyTip").gameObject.GetComponent<UILabel>();
        //Lbl_SupplyNum = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/SupplyNum").gameObject.GetComponent<UILabel>();
        //Slider_SupplyProgress = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/SupplyProgress").gameObject.GetComponent<UISlider>();
        //Btn_LvUp = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/LvUp").gameObject.GetComponent<UIButton>();
        //Lbl_NoticeContent = _uiRoot.transform.FindChild("Anim/Up/Info/NoticeContent").gameObject.GetComponent<UILabel>();
        //Btn_ShowNoticeTip = _uiRoot.transform.FindChild("Anim/Up/Info/ShowNoticeTip").gameObject.GetComponent<UIButton>();
        //Lbl_ShowNoticeTip = _uiRoot.transform.FindChild("Anim/Up/Info/ShowNoticeTip/Label").gameObject.GetComponent<UILabel>();
        //Btn_Hall = _uiRoot.transform.FindChild("Anim/Mid/Hall").gameObject.GetComponent<UIButton>();
        //Btn_Store = _uiRoot.transform.FindChild("Anim/Mid/Store").gameObject.GetComponent<UIButton>();
        //Btn_Advanture = _uiRoot.transform.FindChild("Anim/Mid/Advanture").gameObject.GetComponent<UIButton>();
        //Btn_Fight = _uiRoot.transform.FindChild("Anim/Mid/Fight").gameObject.GetComponent<UIButton>();
        //Btn_UnionPrison = _uiRoot.transform.FindChild("Anim/Mid/Prison").gameObject.GetComponent<UIButton>();
        //Btn_CaptureTerritory = _uiRoot.transform.FindChild("Anim/Mid/CaptureTerritory").gameObject.GetComponent<UIButton>();
        //Trans_MoveContent = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent");
        //Tex_BG = _uiRoot.transform.FindChild("BG/BGPic").gameObject.GetComponent<UITexture>();


        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionView");
        UIPanel_UnionView = _uiRoot.GetComponent<UIPanel>();
        MaskEffect = _uiRoot.transform.FindChild("Effect").gameObject.GetComponent<UIPanel>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Up/Close").gameObject.GetComponent<UIButton>();
        Spt_IconBG = _uiRoot.transform.FindChild("Anim/Up/Icon/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = _uiRoot.transform.FindChild("Anim/Up/Icon/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_State = _uiRoot.transform.FindChild("Anim/Up/Icon/state").gameObject.GetComponent<UISprite>();
        Spt_Icon = _uiRoot.transform.FindChild("Anim/Up/Icon/Icon").gameObject.GetComponent<UISprite>();
        Lbl_UnionName = _uiRoot.transform.FindChild("Anim/Up/Icon/UnionName").gameObject.GetComponent<UILabel>();
        Lbl_UnionID = _uiRoot.transform.FindChild("Anim/Up/Icon/UnionID").gameObject.GetComponent<UILabel>();
        Btn_UnionDonation = _uiRoot.transform.FindChild("Anim/Up/Btn/UnionDonation").gameObject.GetComponent<UIButton>();
        Btn_UnionRule = _uiRoot.transform.FindChild("Anim/Up/Btn/UnionRule").gameObject.GetComponent<UIButton>();
        Spt_InfoBG = _uiRoot.transform.FindChild("Anim/Up/Info/InfoBG").gameObject.GetComponent<UISprite>();
        Lbl_UnionLv = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/UnionLv").gameObject.GetComponent<UILabel>();
        Spt_MaxSprite = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/MaxSprite").gameObject.GetComponent<UISprite>();
        Lbl_SupplyTip = _uiRoot.transform.FindChild("Anim/Up/Info/SupplyTip").gameObject.GetComponent<UILabel>();
        Lbl_SupplyNum = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/SupplyNum").gameObject.GetComponent<UILabel>();
        Slider_SupplyProgress = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/SupplyProgress").gameObject.GetComponent<UISlider>();
        Btn_LvUp = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent/LvUp").gameObject.GetComponent<UIButton>();
        Lbl_NoticeContent = _uiRoot.transform.FindChild("Anim/Up/Info/NoticeContent").gameObject.GetComponent<UILabel>();
        Btn_ShowNoticeTip = _uiRoot.transform.FindChild("Anim/Up/Info/ShowNoticeTip").gameObject.GetComponent<UIButton>();
        Lbl_ShowNoticeTip = _uiRoot.transform.FindChild("Anim/Up/Info/ShowNoticeTip/Label").gameObject.GetComponent<UILabel>();
        Btn_Hall = _uiRoot.transform.FindChild("Content/Mid/Hall").gameObject.GetComponent<UIButton>();
        Btn_Store = _uiRoot.transform.FindChild("Content/Mid/Store").gameObject.GetComponent<UIButton>();
        Btn_Advanture = _uiRoot.transform.FindChild("Content/Mid/Advanture").gameObject.GetComponent<UIButton>();
        Btn_Fight = _uiRoot.transform.FindChild("Content/Mid/Fight").gameObject.GetComponent<UIButton>();
        Btn_UnionPrison = _uiRoot.transform.FindChild("Content/Mid/Prison").gameObject.GetComponent<UIButton>();
        Btn_CaptureTerritory = _uiRoot.transform.FindChild("Content/Mid/CaptureTerritory").gameObject.GetComponent<UIButton>();
        Trans_MoveContent = _uiRoot.transform.FindChild("Anim/Up/Info/MoveContent");
        Tex_BG = _uiRoot.transform.FindChild("Content/BG/BGPic").gameObject.GetComponent<UITexture>();
        Btn_CrossServerWar = _uiRoot.transform.FindChild("Content/Mid/CrossServerWar").gameObject.GetComponent<UIButton>();

    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}