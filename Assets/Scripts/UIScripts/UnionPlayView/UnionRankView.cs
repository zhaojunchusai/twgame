using UnityEngine;
using System;
using System.Collections;

public class UnionRankView
{
    public static string UIName ="UnionRankView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionRankView;
    public UISprite Spt_MaskBGSprite;
    public UISprite Spt_TitleRighBG2;
    public UISprite Spt_TitleLeftBG;
    public UILabel Lbl_TitleLb;
    public UISprite Spt_RankBGSprite;
    public UISprite Spt_RankBG;
    public UISprite Spt_LightSprite;
    public UIPanel UIPanel_ContentScrollView;
    public UIScrollView ScrView_ContentScrollView;
    public UIGrid Grd_UIGrid;
    public UIWrapContent UIWrapContent_UIGrid;
    public GameObject Obj_SelfItem;
    public GameObject Obj_SelfRankItem;
    public UnionRankItem UnionRankItem_SelfItem;
    public UILabel Lbl_EmptyLabel;

    public GameObject Obj_SeasonInfo;
    public GameObject Obj_Scroll;
    public UILabel SeasonDes;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionRankView");
        UIPanel_UnionRankView = _uiRoot.GetComponent<UIPanel>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Spt_TitleRighBG2 = _uiRoot.transform.FindChild("Anim/TitleObj/TitleRighBG2").gameObject.GetComponent<UISprite>();
        Spt_TitleLeftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLeftBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Spt_RankBGSprite = _uiRoot.transform.FindChild("Anim/RankBGSprite").gameObject.GetComponent<UISprite>();
        Spt_RankBG = _uiRoot.transform.FindChild("Anim/RankBG").gameObject.GetComponent<UISprite>();
        Spt_LightSprite = _uiRoot.transform.FindChild("Anim/LightSprite").gameObject.GetComponent<UISprite>();
        UIPanel_ContentScrollView = _uiRoot.transform.FindChild("Anim/Scroll/ContentScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ContentScrollView = _uiRoot.transform.FindChild("Anim/Scroll/ContentScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid = _uiRoot.transform.FindChild("Anim/Scroll/ContentScrollView/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid = _uiRoot.transform.FindChild("Anim/Scroll/ContentScrollView/UIGrid").gameObject.GetComponent<UIWrapContent>();
        Obj_SelfItem = _uiRoot.transform.Find("Anim/Source").gameObject;
        Obj_SelfRankItem = _uiRoot.transform.Find("Anim/Source/UnionRankItem").gameObject;
        Lbl_EmptyLabel = _uiRoot.transform.Find("Anim/EmptyLabel").gameObject.GetComponent<UILabel>();
        Obj_SeasonInfo = _uiRoot.transform.Find("Anim/SeasonInfo").gameObject;
        SeasonDes = _uiRoot.transform.Find("Anim/SeasonInfo/Des").gameObject.GetComponent<UILabel>();
        Obj_Scroll = _uiRoot.transform.FindChild("Anim/Scroll").gameObject;
        UnionRankItem_SelfItem = Obj_SelfRankItem.AddComponent<UnionRankItem>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text = ConstString.UNIONRANK_TITLE_KILL;
        Lbl_EmptyLabel.text = ConstString.UNIONRANK_LABEL_KILLEMPTY;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
