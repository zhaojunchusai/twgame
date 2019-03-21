using UnityEngine;
using System;
using System.Collections;

public class UnionReadinessView
{
    public static string UIName ="UnionReadinessView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionReadinessView;
    public UISprite Spt_BG;
    public UILabel Lbl_TeamNumLabel;
    public UISprite Spt_ScrollBGSprite;
    public UISprite Spt_ViewBG;
    public UIPanel UIPanel_ReadinessSrollView;
    public UIScrollView ScrView_ReadinessSrollView;
    public UIGrid Grd_UIGrid;
    public UISprite Spt_TitleLiftBG;
    public UISprite Spt_TitleRightBG;
    public UILabel Lbl_TitleLb;
    public UILabel Lbl_CurrentCityLabel;
    public UISprite Spt_Mask;
    public GameObject Obj_ReadinessItem;

    public bool isJoinIn;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionReadinessView");
        UIPanel_UnionReadinessView = _uiRoot.GetComponent<UIPanel>();
        Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Lbl_TeamNumLabel = _uiRoot.transform.FindChild("Anim/TeamNumLabel").gameObject.GetComponent<UILabel>();
        Spt_ScrollBGSprite = _uiRoot.transform.FindChild("Anim/ScrollBGSprite").gameObject.GetComponent<UISprite>();
        Spt_ViewBG = _uiRoot.transform.FindChild("Anim/ViewBG").gameObject.GetComponent<UISprite>();
        UIPanel_ReadinessSrollView = _uiRoot.transform.FindChild("Anim/ReadinessSrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ReadinessSrollView = _uiRoot.transform.FindChild("Anim/ReadinessSrollView").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid = _uiRoot.transform.FindChild("Anim/ReadinessSrollView/UIGrid").gameObject.GetComponent<UIGrid>();
        Spt_TitleLiftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLiftBG").gameObject.GetComponent<UISprite>();
        Spt_TitleRightBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleRightBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Lbl_CurrentCityLabel = _uiRoot.transform.FindChild("Anim/CurrentCityLabel").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();

        Obj_ReadinessItem = _uiRoot.transform.FindChild("Source/ReadinessItem").gameObject;
        SetLabelValues();
      
     
    }

    public void SetLabelValues()
    {
        Lbl_TeamNumLabel.text = string.Format(ConstString.UNIONREADINESS_TAMENUM, 0);
        Lbl_TitleLb.text = ConstString.UNIONREADINESS_TITLE;
        Lbl_CurrentCityLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
