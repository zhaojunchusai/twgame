using UnityEngine;
using System.Collections;
public class RankView
{
    public static string UIName = "RankView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_RankView;
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
    public UIPanel UIPanel_RankTypeScrollView;
    public UIScrollView ScrView_RankTypeScrollView;
    public UITable UITable_UITable;
    public GameObject Obj_SelfRankObj;
    public RankItem RankItem_SelfRankItem;
    public UILabel Lbl_RefreshLabel;

    public Transform Trans_SelectedSprite;
    public GameObject Gobj_TypeBtn;
    public GameObject Gobj_RankViewSubTypeObj;
    public GameObject Gobj_SubTypeBtn;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/RankView");
        UIPanel_RankView = _uiRoot.GetComponent<UIPanel>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Spt_TitleRighBG2 = _uiRoot.transform.FindChild("Anim/TitleObj/TitleRighBG2").gameObject.GetComponent<UISprite>();
        Spt_TitleLeftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLeftBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Spt_RankBGSprite = _uiRoot.transform.FindChild("Anim/RankBGSprite").gameObject.GetComponent<UISprite>();
        Spt_RankBG = _uiRoot.transform.FindChild("Anim/RankBG").gameObject.GetComponent<UISprite>();
        Spt_LightSprite = _uiRoot.transform.FindChild("Anim/LightSprite").gameObject.GetComponent<UISprite>();
        UIPanel_ContentScrollView = _uiRoot.transform.FindChild("Anim/ContentScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ContentScrollView = _uiRoot.transform.FindChild("Anim/ContentScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid = _uiRoot.transform.FindChild("Anim/ContentScrollView/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid = _uiRoot.transform.FindChild("Anim/ContentScrollView/UIGrid").gameObject.GetComponent<UIWrapContent>();
        UIPanel_RankTypeScrollView = _uiRoot.transform.FindChild("Anim/RankTypeScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_RankTypeScrollView = _uiRoot.transform.FindChild("Anim/RankTypeScrollView").gameObject.GetComponent<UIScrollView>();
        UITable_UITable = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable").gameObject.GetComponent<UITable>();
        RankItem_SelfRankItem = _uiRoot.transform.FindChild("Anim/Source/RankItem").gameObject.AddComponent<RankItem>();

        Trans_SelectedSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/SelectedSprite");
        Gobj_TypeBtn = _uiRoot.transform.FindChild("Anim/Source/TypeBtn").gameObject;
        Gobj_RankViewSubTypeObj = _uiRoot.transform.FindChild("Anim/Source/RankViewSubTypeObj").gameObject;
        Gobj_SubTypeBtn = _uiRoot.transform.FindChild("Anim/Source/SubTypeBtn").gameObject;

        RankItem_SelfRankItem.IsSelf = true;
        Lbl_RefreshLabel = _uiRoot.transform.FindChild("Anim/RefreshLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text = ConstString.RANK_LABLE_TITLE;
        Lbl_RefreshLabel.text = ConstString.RANK_LABEL_REFRESHATTIME;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
