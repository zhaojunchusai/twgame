using UnityEngine;
using System;
using System.Collections;

public class LoginView
{
    public static string UIName = "LoginView";
    public GameObject _uiRoot;
    //public GameObject Gobj_Server;
    //public UIPanel UIPanel_gobj_Server;
    //public UIButton Btn_Last;
    //public UISprite Spt_BtnLastBG;
    //public UILabel Lbl_BtnLastStatus;
    //public UILabel Lbl_BtnLastName;
    //public UISprite Spt_BG;
    //public UISprite Spt_Sprite1;
    //public UISprite Spt_Sprite2;
    //public UILabel Lbl_Label;
    public UISprite Spt_Lock;
    public UISprite Spt_MainLogo;
    public UILabel Lbl_ClickChooseServer;
    //public UISprite Spt_Sprite4;
    //public UISprite Spt_Sprite5;
    //public UISprite Spt_Sprite;
    //public UIPanel UIPanel_ScrollView;
    //public UIScrollView ScrView_ScrollView;
    //public UIGrid Grd_ListGrid;
    //public UIButton Btn_Comp;
    //public UISprite Spt_BtnCompBG;
    //public UILabel Lbl_BtnCompName;
    //public UILabel Lbl_BtnCompStatus;
    //public UISprite Spt_Mask;
    public GameObject Gobj_Main;
    //public UIPanel UIPanel_gobj_Main;
    public UIButton Btn_Start;
    //public UISprite Spt_BtnStartBG;
    //public UISprite Spt_BtnStartLabel;
    public UIButton Btn_Server;
    //public UISprite Spt_BtnServerBG;
    //public UISprite Spt_Sprite1;
    //public UISprite Spt_Sprite1;
    public UILabel Lbl_Label;
    public UILabel Lbl_BtnServerLabel;
    public UILabel Lbl_BtnServerState;
    public GameObject Gobj_Login;
    public GameObject Gobj_LoginChild;
    //public UIPanel UIPanel_gobj_Login;
    public UIInput Ipt_Account;
    //public UISprite Spt_InputAccountBG;
    public UILabel Lbl_InputAccountAccountlb;
    //public UISprite Spt_InputAccountTip;
    //public UISprite Spt_Bg;
    //public UISprite Spt_Line;
    public UIInput Ipt_Pwd;
    //public UISprite Spt_InputPwdBG;
    public UILabel Lbl_InputPwdLabel;
    //public UISprite Spt_InputPwdTip;
    public UIButton Btn_Login;
    //public UISprite Spt_BtnLoginBG;
    public UISprite Spt_BtnLoginLabel;
    public UIButton Btn_Register;
    //public UISprite Spt_BtnRegisterBG;
    public UISprite Spt_BtnRegisterLabel;
    public UIButton Btn_AutoLogin;
    //public UISprite Spt_BtnAutoLoginFG;
    //public UISprite Spt_BtnAutoLoginBG;
    public UISprite Spt_BtnAutoLoginCheckMark;
    public UIButton Btn_RememberPwd;
    //public UISprite Spt_BtnRememberPwdFG;
    //public UISprite Spt_BtnRememberPwdBG;
    public UISprite Spt_BtnRememberPwdCheckMark;
    public UIPopupList UIPopupList_AccountList;
    //public UIButton Btn_AccountList;
    //public UISprite Spt_BtnAccountListSprite;
    public UITexture Tex_LoginBG;
    //public UITexture Tex_Texture;
    //public TextureMove TextureMove_Texture;
    public GameObject Gobj_AutoLogin;
    //public UIPanel UIPanel_gobj_AutoLogin;
    //public UISprite Spt_AutoLoginlb;
    //public UISprite Spt_Bg;
    public UIButton Btn_CancelAutoLogin;
    //public UISprite Spt_BtnCancelAutoLoginBG;
    //public UISprite Spt_BtnCancelAutoLoginLabel;

    public TweenPosition Tpos_LoginBGL;
    public TweenPosition Tpos_LoginBGR;
    public TweenScale Tscale_LoginLogin;
    public TweenPosition Tpos_LoginLogo;
    public TweenAlpha Talpha_LoginLogo;

    public UISprite Logo_Login;
    public UISprite Logo_AutoLogin;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/LoginView");
        //Gobj_Server = _uiRoot.transform.FindChild("gobj_Server").gameObject;

        Tpos_LoginBGL = _uiRoot.transform.FindChild("gobj_Login/BG_1/L").gameObject.GetComponent<TweenPosition>();
        Tpos_LoginBGR = _uiRoot.transform.FindChild("gobj_Login/BG_1/R").gameObject.GetComponent<TweenPosition>();
        Tscale_LoginLogin = _uiRoot.transform.FindChild("gobj_Login/Login").gameObject.GetComponent<TweenScale>();
        Talpha_LoginLogo = _uiRoot.transform.FindChild("gobj_Login/Login/Logo").gameObject.GetComponent<TweenAlpha>();
        Logo_Login = _uiRoot.transform.FindChild("gobj_Login/Login/Logo").GetComponent<UISprite>();
        Logo_AutoLogin = _uiRoot.transform.FindChild("gobj_AutoLogin/Logo").GetComponent<UISprite>();
        Tpos_LoginLogo = _uiRoot.transform.FindChild("gobj_Login/Login/Logo").gameObject.GetComponent<TweenPosition>();
        Spt_MainLogo = _uiRoot.transform.FindChild("gobj_Main/MainLogo").gameObject.GetComponent<UISprite>();
        //UIPanel_gobj_Server = _uiRoot.transform.FindChild("gobj_Server").gameObject.GetComponent<UIPanel>();
        //Btn_Last = _uiRoot.transform.FindChild("gobj_Server/Last").gameObject.GetComponent<UIButton>();
        //Spt_BtnLastBG = _uiRoot.transform.FindChild("gobj_Server/Last/BG").gameObject.GetComponent<UISprite>();
        //Lbl_BtnLastStatus = _uiRoot.transform.FindChild("gobj_Server/Last/Status").gameObject.GetComponent<UILabel>();
        //Lbl_BtnLastName = _uiRoot.transform.FindChild("gobj_Server/Last/Name").gameObject.GetComponent<UILabel>();
        //Spt_BG = _uiRoot.transform.FindChild("gobj_Server/BG").gameObject.GetComponent<UISprite>();
        //Spt_Sprite1 = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite1").gameObject.GetComponent<UISprite>();
        //Spt_Sprite2 = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite2").gameObject.GetComponent<UISprite>();
        //Lbl_Label = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite2/Label").gameObject.GetComponent<UILabel>();
        //Spt_Sprite3 = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite3").gameObject.GetComponent<UISprite>();
        //Lbl_Label = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite3/Label").gameObject.GetComponent<UILabel>();
        //Spt_Sprite4 = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite4").gameObject.GetComponent<UISprite>();
        //Spt_Sprite5 = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite5").gameObject.GetComponent<UISprite>();
        //Spt_Sprite = _uiRoot.transform.FindChild("gobj_Server/BG/Sprite").gameObject.GetComponent<UISprite>();
        //UIPanel_ScrollView = _uiRoot.transform.FindChild("gobj_Server/ScrollView").gameObject.GetComponent<UIPanel>();
        //ScrView_ScrollView = _uiRoot.transform.FindChild("gobj_Server/ScrollView").gameObject.GetComponent<UIScrollView>();
        //Grd_ListGrid = _uiRoot.transform.FindChild("gobj_Server/ScrollView/ListGrid").gameObject.GetComponent<UIGrid>();
        //Btn_Comp = _uiRoot.transform.FindChild("gobj_Server/ScrollView/ListGrid/Comp").gameObject.GetComponent<UIButton>();
        //Spt_BtnCompBG = _uiRoot.transform.FindChild("gobj_Server/ScrollView/ListGrid/Comp/BG").gameObject.GetComponent<UISprite>();
        //Lbl_BtnCompName = _uiRoot.transform.FindChild("gobj_Server/ScrollView/ListGrid/Comp/Name").gameObject.GetComponent<UILabel>();
        //Lbl_BtnCompStatus = _uiRoot.transform.FindChild("gobj_Server/ScrollView/ListGrid/Comp/Status").gameObject.GetComponent<UILabel>();
        //Spt_Mask = _uiRoot.transform.FindChild("gobj_Server/Mask").gameObject.GetComponent<UISprite>();
        Gobj_Main = _uiRoot.transform.FindChild("gobj_Main").gameObject;
        //UIPanel_gobj_Main = _uiRoot.transform.FindChild("gobj_Main").gameObject.GetComponent<UIPanel>();
        Btn_Start = _uiRoot.transform.FindChild("gobj_Main/Start").gameObject.GetComponent<UIButton>();
        //Spt_BtnStartBG = _uiRoot.transform.FindChild("gobj_Main/Start/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnStartLabel = _uiRoot.transform.FindChild("gobj_Main/Start/Label").gameObject.GetComponent<UISprite>();
        Btn_Server = _uiRoot.transform.FindChild("gobj_Main/Server").gameObject.GetComponent<UIButton>();
        //Spt_BtnServerBG = _uiRoot.transform.FindChild("gobj_Main/Server/BG").gameObject.GetComponent<UISprite>();
        //Spt_Sprite1 = _uiRoot.transform.FindChild("gobj_Main/Server/BG/Sprite1").gameObject.GetComponent<UISprite>();
        Spt_Lock = _uiRoot.transform.FindChild("gobj_Main/Server/BG/Lock").gameObject.GetComponent<UISprite>();
        Lbl_ClickChooseServer = _uiRoot.transform.FindChild("gobj_Main/Server/BG/Lock/Label").gameObject.GetComponent<UILabel>();
        Lbl_BtnServerLabel = _uiRoot.transform.FindChild("gobj_Main/Server/Label").gameObject.GetComponent<UILabel>();
        Lbl_BtnServerState = _uiRoot.transform.FindChild("gobj_Main/Server/State").gameObject.GetComponent<UILabel>();
        Gobj_Login = _uiRoot.transform.FindChild("gobj_Login").gameObject;
        Gobj_LoginChild = _uiRoot.transform.FindChild("gobj_Login/Login").gameObject;
        //UIPanel_gobj_Login = _uiRoot.transform.FindChild("gobj_Login").gameObject.GetComponent<UIPanel>();
        Ipt_Account = _uiRoot.transform.FindChild("gobj_Login/Login/Account").gameObject.GetComponent<UIInput>();
        //Spt_InputAccountBG = _uiRoot.transform.FindChild("gobj_Login/Login/Account/BG").gameObject.GetComponent<UISprite>();
        Lbl_InputAccountAccountlb = _uiRoot.transform.FindChild("gobj_Login/Login/Account/Accountlb").gameObject.GetComponent<UILabel>();
        //Spt_InputAccountTip = _uiRoot.transform.FindChild("gobj_Login/Login/Account/Tip").gameObject.GetComponent<UISprite>();
        //Spt_Bg = _uiRoot.transform.FindChild("gobj_Login/Bg").gameObject.GetComponent<UISprite>();
        //Spt_Line = _uiRoot.transform.FindChild("gobj_Login/Login/Bg/Line").gameObject.GetComponent<UISprite>();
        Ipt_Pwd = _uiRoot.transform.FindChild("gobj_Login/Login/Pwd").gameObject.GetComponent<UIInput>();
        //Spt_InputPwdBG = _uiRoot.transform.FindChild("gobj_Login/Login/Pwd/BG").gameObject.GetComponent<UISprite>();
        Lbl_InputPwdLabel = _uiRoot.transform.FindChild("gobj_Login/Login/Pwd/Label").gameObject.GetComponent<UILabel>();
        //Spt_InputPwdTip = _uiRoot.transform.FindChild("gobj_Login/Login/Pwd/Tip").gameObject.GetComponent<UISprite>();
        Btn_Login = _uiRoot.transform.FindChild("gobj_Login/Login/Login").gameObject.GetComponent<UIButton>();
        //Spt_BtnLoginBG = _uiRoot.transform.FindChild("gobj_Login/Login/Login/BG").gameObject.GetComponent<UISprite>();
        Spt_BtnLoginLabel = _uiRoot.transform.FindChild("gobj_Login/Login/Login/Label").gameObject.GetComponent<UISprite>();
        Btn_Register = _uiRoot.transform.FindChild("gobj_Login/Login/Register").gameObject.GetComponent<UIButton>();
        //Spt_BtnRegisterBG = _uiRoot.transform.FindChild("gobj_Login/Login/Register/BG").gameObject.GetComponent<UISprite>();
        Spt_BtnRegisterLabel = _uiRoot.transform.FindChild("gobj_Login/Login/Register/Label").gameObject.GetComponent<UISprite>();
        Btn_AutoLogin = _uiRoot.transform.FindChild("gobj_Login/Login/AutoLogin").gameObject.GetComponent<UIButton>();
        //Spt_BtnAutoLoginFG = _uiRoot.transform.FindChild("gobj_Login/Login/AutoLogin/FG").gameObject.GetComponent<UISprite>();
        //Spt_BtnAutoLoginBG = _uiRoot.transform.FindChild("gobj_Login/Login/AutoLogin/BG").gameObject.GetComponent<UISprite>();
        Spt_BtnAutoLoginCheckMark = _uiRoot.transform.FindChild("gobj_Login/Login/AutoLogin/CheckMark").gameObject.GetComponent<UISprite>();
        Btn_RememberPwd = _uiRoot.transform.FindChild("gobj_Login/Login/RememberPwd").gameObject.GetComponent<UIButton>();
        //Spt_BtnRememberPwdFG = _uiRoot.transform.FindChild("gobj_Login/Login/RememberPwd/FG").gameObject.GetComponent<UISprite>();
        //Spt_BtnRememberPwdBG = _uiRoot.transform.FindChild("gobj_Login/Login/RememberPwd/BG").gameObject.GetComponent<UISprite>();
        Spt_BtnRememberPwdCheckMark = _uiRoot.transform.FindChild("gobj_Login/Login/RememberPwd/CheckMark").gameObject.GetComponent<UISprite>();
        UIPopupList_AccountList = _uiRoot.transform.FindChild("gobj_Login/Login/AccountList").gameObject.GetComponent<UIPopupList>();
        //Btn_AccountList = _uiRoot.transform.FindChild("gobj_Login/Login/AccountList").gameObject.GetComponent<UIButton>();
        //Spt_BtnAccountListSprite = _uiRoot.transform.FindChild("gobj_Login/Login/AccountList/Sprite").gameObject.GetComponent<UISprite>();
        Tex_LoginBG = _uiRoot.transform.FindChild("LoginBG").gameObject.GetComponent<UITexture>();
        //Tex_Texture = _uiRoot.transform.FindChild("LoginBG/Texture").gameObject.GetComponent<UITexture>();
        //TextureMove_Texture = _uiRoot.transform.FindChild("LoginBG/Texture").gameObject.GetComponent<TextureMove>();
        Gobj_AutoLogin = _uiRoot.transform.FindChild("gobj_AutoLogin").gameObject;
        //UIPanel_gobj_AutoLogin = _uiRoot.transform.FindChild("gobj_AutoLogin").gameObject.GetComponent<UIPanel>();
        //Spt_AutoLoginlb = _uiRoot.transform.FindChild("gobj_AutoLogin/AutoLoginlb").gameObject.GetComponent<UISprite>();
        //Spt_Bg = _uiRoot.transform.FindChild("gobj_AutoLogin/Bg").gameObject.GetComponent<UISprite>();
        Btn_CancelAutoLogin = _uiRoot.transform.FindChild("gobj_AutoLogin/CancelAutoLogin").gameObject.GetComponent<UIButton>();
        //Spt_BtnCancelAutoLoginBG = _uiRoot.transform.FindChild("gobj_AutoLogin/CancelAutoLogin/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnCancelAutoLoginLabel = _uiRoot.transform.FindChild("gobj_AutoLogin/CancelAutoLogin/Label").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_BtnLastStatus.text = "爆满";
        //Lbl_BtnLastName.text = "999区   川香钵钵鸡";
        //Lbl_Label.text = "上次登陆";
        //Lbl_Label.text = "所有服务器";
        //Lbl_BtnCompName.text = "999区   川香钵钵鸡";
        //Lbl_BtnCompStatus.text = "爆满";
        //Lbl_Label.text = "点击换区";
        Lbl_BtnServerLabel.text = "99区 川香钵钵鸡";
        Lbl_InputAccountAccountlb.text = "";
        Lbl_InputPwdLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
