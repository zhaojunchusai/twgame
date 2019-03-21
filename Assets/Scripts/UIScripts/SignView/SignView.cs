using UnityEngine;
using System;
using System.Collections;

public class SignView
{
    public static string UIName ="SignView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_SignView;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseBackground;
    public UILabel Lbl_BtnCloseLabel;
    public UISprite Spt_Title;
    public UISprite Spt_TitleUp;
    public UISprite Spt_DecorationUL;
    public UISprite Spt_DecorationUR;
    public UISprite Spt_TitleDown;
    public UISprite Spt_DecorationDL;
    public UISprite Spt_DecorationDR;
    public UISprite Spt_StoreBG;
    public UISprite Spt_StoreFG;
    public UISprite Spt_StoreFG1;
    public UISprite Spt_DecorationDown;
    public UISprite Spt_DecorationUp;
    public UISprite Spt_TotalLoginTip;
    public UILabel Lbl_TotalLoginCount;
    public UISprite Spt_ContinuousLoginTip;
    public UILabel Lbl_ContinuousLoginCount;
    public UITexture Tex_BG;
    public UIPanel UIPanel_ContinuousLogin;
    public UIGrid Grd_ContinuousLogin;
    public UIPanel UIPanel_TotalLogin;
    public UIGrid Grd_TotalLogin;
    public GameObject gobj_SignItem;
    public UILabel Lbl_ExtraAwardTip;
    public UILabel Lbl_ExtraAwardNum;
    public UITable Table_ExtraAward;
    public GameObject Gobj_ExtraAward;
    public TweenScale Anim_TScale;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SignView");
        UIPanel_SignView = _uiRoot.GetComponent<UIPanel>();
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Other/Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnCloseBackground = _uiRoot.transform.FindChild("Anim/Other/Close/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnCloseLabel = _uiRoot.transform.FindChild("Anim/Other/Close/Label").gameObject.GetComponent<UILabel>();
        //Spt_Title = _uiRoot.transform.FindChild("Anim/Other/Title").gameObject.GetComponent<UISprite>();
        //Spt_TitleUp = _uiRoot.transform.FindChild("Anim/Other/TitleUp").gameObject.GetComponent<UISprite>();
        //Spt_DecorationUL = _uiRoot.transform.FindChild("Anim/Other/TitleUp/DecorationUL").gameObject.GetComponent<UISprite>();
        //Spt_DecorationUR = _uiRoot.transform.FindChild("Anim/Other/TitleUp/DecorationUR").gameObject.GetComponent<UISprite>();
        //Spt_TitleDown = _uiRoot.transform.FindChild("Anim/Other/TitleDown").gameObject.GetComponent<UISprite>();
        //Spt_DecorationDL = _uiRoot.transform.FindChild("Anim/Other/TitleDown/DecorationDL").gameObject.GetComponent<UISprite>();
        //Spt_DecorationDR = _uiRoot.transform.FindChild("Anim/Other/TitleDown/DecorationDR").gameObject.GetComponent<UISprite>();
        //Spt_StoreBG = _uiRoot.transform.FindChild("Anim/Other/StoreBG").gameObject.GetComponent<UISprite>();
        //Spt_StoreFG = _uiRoot.transform.FindChild("Anim/Other/StoreFG").gameObject.GetComponent<UISprite>();
        //Spt_StoreFG1 = _uiRoot.transform.FindChild("Anim/Other/StoreFG1").gameObject.GetComponent<UISprite>();
        //Spt_DecorationDown = _uiRoot.transform.FindChild("Anim/Other/DecorationDown").gameObject.GetComponent<UISprite>();
        //Spt_DecorationUp = _uiRoot.transform.FindChild("Anim/Other/DecorationUp").gameObject.GetComponent<UISprite>();
        //Spt_TotalLoginTip = _uiRoot.transform.FindChild("Anim/Other/TotalLoginTip").gameObject.GetComponent<UISprite>();
        Lbl_TotalLoginCount = _uiRoot.transform.FindChild("Anim/Other/TotalLoginCount").gameObject.GetComponent<UILabel>();
        //Spt_ContinuousLoginTip = _uiRoot.transform.FindChild("Anim/Other/ContinuousLoginTip").gameObject.GetComponent<UISprite>();
       Lbl_ContinuousLoginCount = _uiRoot.transform.FindChild("Anim/Other/ContinuousLoginR").gameObject.GetComponent<UILabel>();
        //Tex_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UITexture>();
        //UIPanel_ContinuousLogin = _uiRoot.transform.FindChild("Anim/ContinuousLogin").gameObject.GetComponent<UIPanel>();
        Grd_ContinuousLogin = _uiRoot.transform.FindChild("Anim/ContinuousLogin/ContinuousLogin").gameObject.GetComponent<UIGrid>();
        //UIPanel_TotalLogin = _uiRoot.transform.FindChild("Anim/TotalLogin").gameObject.GetComponent<UIPanel>();
        Grd_TotalLogin = _uiRoot.transform.FindChild("Anim/TotalLogin/TotalLogin").gameObject.GetComponent<UIGrid>();
        gobj_SignItem = _uiRoot.transform.FindChild("Anim/Pre/SignItem").gameObject;
        Lbl_ExtraAwardTip = _uiRoot.transform.FindChild("Anim/Other/ExtraAward/ExtraAward/ExtraAwardTip").gameObject.GetComponent<UILabel>();
        Lbl_ExtraAwardNum = _uiRoot.transform.FindChild("Anim/Other/ExtraAward/ExtraAward/ExtraAwardNum").gameObject.GetComponent<UILabel>();
        Table_ExtraAward = _uiRoot.transform.FindChild("Anim/Other/ExtraAward/ExtraAward").gameObject.GetComponent<UITable>();
        Gobj_ExtraAward = _uiRoot.transform.FindChild("Anim/Other/ExtraAward").gameObject;
        //SetLabelValues();
        gobj_SignItem.SetActive(false);
    }

    public void SetLabelValues()
    {
        Lbl_BtnCloseLabel.text = "";
        Lbl_TotalLoginCount.text = "66";
        Lbl_ContinuousLoginCount.text = "66";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
