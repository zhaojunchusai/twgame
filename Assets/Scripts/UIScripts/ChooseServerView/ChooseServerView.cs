using UnityEngine;
using System;
using System.Collections;

public class ChooseServerView
{
    public static string UIName = "ChooseServerView";
    public GameObject _uiRoot;
    //public UIPanel UIPanel_ChooseServerView;
    //public UISprite Spt_BG;
    //public UISprite Spt_Sprite1;
    //public UISprite Spt_Sprite2;
    //public UISprite Spt_Sprite3;
    //public UISprite Spt_Sprite4;
    //public UISprite Spt_Sprite5;
    //public UISprite Spt_Sprite;
    public UIButton Btn_LastLoginSvr;
    public UISprite Spt_BtnLastLoginSvrBG;
    public UILabel Lbl_BtnLastLoginSvrStatus;
    public UILabel Lbl_BtnLastLoginSvrName;
    public UISprite Spt_ServerTypeSelectedMark;
    public UISprite Spt_ServerItemSelectedMark;
    public UIButton Btn_RecomendServer;
    public UILabel Lbl_BtnRecomendServerName;
    //public UISprite Spt_BtnRecomendServerLine;
    public UILabel Lbl_ServerTypeTitle;
    public UILabel Lbl_LastLoginTip;
    public UILabel Lbl_ServerListTitle;
    public UIGrid Grd_ServerList;
    public GameObject ServerItem0;
    public GameObject ServerItem1;
    public GameObject ServerItem2;
    public GameObject ServerItem3;
    public GameObject ServerItem4;
    public GameObject ServerItem5;
    public GameObject ServerItem6;
    public GameObject ServerItem7;
    public GameObject ServerItem8;
    public GameObject ServerItem9;
    //public UIPanel UIPanel_ServerTypeList;
    public UIGrid Grd_ServerTypeList;
    public UISprite Spt_Mask;
    public GameObject Gobj_ServerTypeItem;
    public GameObject Gobj_SelectedRecommend;
    public UITexture Tex_MaskTex;

    public TweenScale Anim_TScale;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ChooseServerView");
        //UIPanel_ChooseServerView = _uiRoot.GetComponent<UIPanel>();
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        //Spt_BG = _uiRoot.transform.FindChild("Anim/BG").gameObject.GetComponent<UISprite>();
        //Spt_Sprite1 = _uiRoot.transform.FindChild("Anim/BG/Sprite1").gameObject.GetComponent<UISprite>();
        //Spt_Sprite2 = _uiRoot.transform.FindChild("Anim/BG/Sprite2").gameObject.GetComponent<UISprite>();
        //Spt_Sprite3 = _uiRoot.transform.FindChild("Anim/BG/Sprite3").gameObject.GetComponent<UISprite>();
        //Spt_Sprite4 = _uiRoot.transform.FindChild("Anim/BG/Sprite4").gameObject.GetComponent<UISprite>();
        //Spt_Sprite5 = _uiRoot.transform.FindChild("Anim/BG/Sprite5").gameObject.GetComponent<UISprite>();
        //Spt_Sprite = _uiRoot.transform.FindChild("Anim/BG/Sprite").gameObject.GetComponent<UISprite>();
        Tex_MaskTex = _uiRoot.transform.FindChild("MaskTex").gameObject.GetComponent<UITexture>();
        Btn_LastLoginSvr = _uiRoot.transform.FindChild("Anim/LastLoginSvr").gameObject.GetComponent<UIButton>();
        Spt_BtnLastLoginSvrBG = _uiRoot.transform.FindChild("Anim/LastLoginSvr/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnLastLoginSvrStatus = _uiRoot.transform.FindChild("Anim/LastLoginSvr/Status").gameObject.GetComponent<UILabel>();
        Lbl_BtnLastLoginSvrName = _uiRoot.transform.FindChild("Anim/LastLoginSvr/Name").gameObject.GetComponent<UILabel>();
        Btn_RecomendServer = _uiRoot.transform.FindChild("Anim/RecomendServer").gameObject.GetComponent<UIButton>();
        Lbl_BtnRecomendServerName = _uiRoot.transform.FindChild("Anim/RecomendServer/Name").gameObject.GetComponent<UILabel>();
        //Spt_BtnRecomendServerLine = _uiRoot.transform.FindChild("Anim/RecomendServer/Line").gameObject.GetComponent<UISprite>();
        Gobj_SelectedRecommend = _uiRoot.transform.FindChild("Anim/RecomendServer/Selected").gameObject;
        Lbl_ServerTypeTitle = _uiRoot.transform.FindChild("Anim/ServerTypeTitle").gameObject.GetComponent<UILabel>();
        Lbl_LastLoginTip = _uiRoot.transform.FindChild("Anim/LastLoginTip").gameObject.GetComponent<UILabel>();
        Lbl_ServerListTitle = _uiRoot.transform.FindChild("Anim/ServerListTitle").gameObject.GetComponent<UILabel>();
        Grd_ServerList = _uiRoot.transform.FindChild("Anim/ServerList/ServerList").gameObject.GetComponent<UIGrid>();
        ServerItem0 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem0").gameObject;
        ServerItem1 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem1").gameObject;
        ServerItem2 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem2").gameObject;
        ServerItem3 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem3").gameObject;
        ServerItem4 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem4").gameObject;
        ServerItem5 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem5").gameObject;
        ServerItem6 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem6").gameObject;
        ServerItem7 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem7").gameObject;
        ServerItem8 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem8").gameObject;
        ServerItem9 = _uiRoot.transform.FindChild("Anim/ServerList/ServerList/ServerItem9").gameObject;
        //UIPanel_ServerTypeList = _uiRoot.transform.FindChild("Anim/ServerTypeList").gameObject.GetComponent<UIPanel>();
        Grd_ServerTypeList = _uiRoot.transform.FindChild("Anim/ServerTypeList/ServerTypeList").gameObject.GetComponent<UIGrid>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Gobj_ServerTypeItem = _uiRoot.transform.FindChild("Anim/Pre/Gobj_ServerTypeItem").gameObject;
        //Gobj_ServerTypeItem.SetActive(false);
        //SetLabelValues();

    }

    public void GetSelectedMarkOnce()
    {
        Spt_ServerTypeSelectedMark = _uiRoot.transform.FindChild("Anim/ServerTypeSelectedMark").gameObject.GetComponent<UISprite>();
        Spt_ServerItemSelectedMark = _uiRoot.transform.FindChild("Anim/ServerItemSelectedMark").gameObject.GetComponent<UISprite>();
    }

    public void SetLabelValues()
    {
        Lbl_BtnLastLoginSvrStatus.text = "";
        Lbl_BtnLastLoginSvrName.text = "";
        Lbl_BtnRecomendServerName.text = "推荐服";
        Lbl_ServerTypeTitle.text = "所有服务器";
        Lbl_LastLoginTip.text = "上次登录";
        Lbl_ServerListTitle.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
