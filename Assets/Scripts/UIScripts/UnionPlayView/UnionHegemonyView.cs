using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 军团争霸界面
/// </summary>
public class UnionHegemonyView
{
    public static string UIName ="UnionHegemonyView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionHegemonyView;
    public UISprite Spt_BackBG;
    public UISprite Spt_MaskBG;
    public UISprite Spt_TitleLiftBG;
    public UISprite Spt_TitleRightBG;
    public UILabel Lbl_TitleLb;
    public UISprite Spt_ViewBG;
    public UISprite Spt_ScrollBGSprite_Up;
    public UISprite Spt_ScrollBGSprite_Down;
    public UISprite Spt_LightSP;
    public UISprite Spt_VSSprite;
    public UISprite Spt_SwordSprite;
    public UILabel Lbl_RestLabel;
    public UIPanel UIPanel_RestTeamSrollView_Self;
    public UIScrollView ScrView_RestTeamSrollView_Self;
    public UIGrid Grd_UIGrid_Self;
    public UIWrapContent UIWrapContent_UIGrid_Self;
    public UIPanel UIPanel_RestTeamSrollView_Enemy;
    public UIScrollView ScrView_RestTeamSrollView_Enemy;
    public UIGrid Grd_UIGrid_Enemy;
    public UIWrapContent UIWrapContent_UIGrid_Enemy;
    public UnionHUnionInfoItem UnionHUnionInfoItem_Self;
    public UnionHUnionInfoItem UnionHUnionInfoItem_Enemy;
    public UnionHPlayerInfoItem UnionHPlayerInfoItem_Self;
    public UnionHPlayerInfoItem UnionHPlayerInfoItem_Enemy;
    public GameObject Obj_UnionHTeamItemObj;
    public GameObject EndObject;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionHegemonyView");
        UIPanel_UnionHegemonyView = _uiRoot.GetComponent<UIPanel>();
        Spt_BackBG = _uiRoot.transform.FindChild("BackBG").gameObject.GetComponent<UISprite>();
        Spt_MaskBG = _uiRoot.transform.FindChild("MaskBG").gameObject.GetComponent<UISprite>();
        Spt_TitleLiftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLiftBG").gameObject.GetComponent<UISprite>();
        Spt_TitleRightBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleRightBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Spt_ViewBG = _uiRoot.transform.FindChild("Anim/ViewBG").gameObject.GetComponent<UISprite>();
        Spt_ScrollBGSprite_Up = _uiRoot.transform.FindChild("Anim/ScrollBGSprite_Up").gameObject.GetComponent<UISprite>();
        Spt_ScrollBGSprite_Down = _uiRoot.transform.FindChild("Anim/ScrollBGSprite_Down").gameObject.GetComponent<UISprite>();
        Spt_LightSP = _uiRoot.transform.FindChild("Anim/LightSP").gameObject.GetComponent<UISprite>();
        Spt_VSSprite = _uiRoot.transform.FindChild("Anim/VSSprite").gameObject.GetComponent<UISprite>();
        Spt_SwordSprite = _uiRoot.transform.FindChild("Anim/SwordSprite").gameObject.GetComponent<UISprite>();
        Lbl_RestLabel = _uiRoot.transform.FindChild("Anim/RestLabel").gameObject.GetComponent<UILabel>();
        
        UIPanel_RestTeamSrollView_Self = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Self").gameObject.GetComponent<UIPanel>();
        ScrView_RestTeamSrollView_Self = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Self").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid_Self = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Self/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid_Self = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Self/UIGrid").gameObject.GetComponent<UIWrapContent>();
        
        UIPanel_RestTeamSrollView_Enemy = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Enemy").gameObject.GetComponent<UIPanel>();
        ScrView_RestTeamSrollView_Enemy = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Enemy").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid_Enemy = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Enemy/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid_Enemy = _uiRoot.transform.FindChild("Anim/RestTeamSrollView_Enemy/UIGrid").gameObject.GetComponent<UIWrapContent>();
        
        UnionHUnionInfoItem_Self = _uiRoot.transform.FindChild("Anim/UnionHInfoItem_Self").gameObject.AddComponent<UnionHUnionInfoItem>();
        UnionHUnionInfoItem_Enemy = _uiRoot.transform.FindChild("Anim/UnionHInfoItem_Enemy").gameObject.AddComponent<UnionHUnionInfoItem>();
        UnionHPlayerInfoItem_Self = _uiRoot.transform.FindChild("Anim/UnionHPlayerInfoItem_Self").gameObject.AddComponent<UnionHPlayerInfoItem>();
        UnionHPlayerInfoItem_Enemy = _uiRoot.transform.FindChild("Anim/UnionHPlayerInfoItem_Enemy").gameObject.AddComponent<UnionHPlayerInfoItem>();
        if(UnionHPlayerInfoItem_Self != null)
        {
            UnionHPlayerInfoItem_Self.LoadEffect(GlobalConst.DIR_EFFECT_HEGEMONYLEFT);
        }
        if (UnionHPlayerInfoItem_Enemy != null)
        {
            UnionHPlayerInfoItem_Enemy.LoadEffect(GlobalConst.DIR_EFFECT_HEGEMONYRIGHT);
        }
        Obj_UnionHTeamItemObj = _uiRoot.transform.FindChild("Anim/Source/UnionHTeamItem_0").gameObject;
        EndObject = _uiRoot.transform.FindChild("EndTouch").gameObject;
        Spt_SwordSprite.gameObject.SetActive(false);
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text =  ConstString.UNIONBATTLE_LABEL_CITY;
        Lbl_RestLabel.text = ConstString.UNIONBATTLE_LABEL_RESTEAM;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
