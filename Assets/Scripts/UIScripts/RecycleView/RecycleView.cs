using UnityEngine;
using System.Collections;

public class RecycleView
{
    public static string UIName = "RecycleView";
    public GameObject _uiRoot;
    public GameObject MaskBGSprite;
    public UILabel Lbl_TitleLb;
    public UIPanel UIPanel_ContentScrollView;
    public UIScrollView ScrView_ContentScrollView;
    public UIGrid Grd_UIGrid;
    public UIWrapContent UIWrapContent_UIGrid;
    public UIPanel UIPanel_RankTypeScrollView;
    public UIScrollView ScrView_RankTypeScrollView;
    public UITable UITable_UITable;

    public Transform Trans_SelectedSprite;
    public GameObject Gobj_TypeBtn;
    public GameObject Gobj_RankViewSubTypeObj;
    public GameObject Gobj_SubTypeBtn;
    public GameObject Gobj_RecycleItem;

    public UISprite Spt_MoneyIcon;
    public UILabel Lbl_MoneyNum;
    public GameObject Btn_Store;
    public GameObject Btn_Confirm;
    public GameObject Btn_OneKeyChoose;

    public UILabel Lbl_RecycleMoneyNum;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/RecycleView");
        MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject;
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        UIPanel_ContentScrollView = _uiRoot.transform.FindChild("Anim/ContentScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ContentScrollView = _uiRoot.transform.FindChild("Anim/ContentScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid = _uiRoot.transform.FindChild("Anim/ContentScrollView/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid = _uiRoot.transform.FindChild("Anim/ContentScrollView/UIGrid").gameObject.GetComponent<UIWrapContent>();
        UIPanel_RankTypeScrollView = _uiRoot.transform.FindChild("Anim/RankTypeScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_RankTypeScrollView = _uiRoot.transform.FindChild("Anim/RankTypeScrollView").gameObject.GetComponent<UIScrollView>();
        UITable_UITable = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable").gameObject.GetComponent<UITable>();

        Trans_SelectedSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/SelectedSprite");
        Gobj_TypeBtn = _uiRoot.transform.FindChild("Anim/Source/Gobj_TypeBtn").gameObject;
        Gobj_RankViewSubTypeObj = _uiRoot.transform.FindChild("Anim/Source/Gobj_RankViewSubTypeObj").gameObject;
        Gobj_SubTypeBtn = _uiRoot.transform.FindChild("Anim/Source/Gobj_SubTypeBtn").gameObject;
        Gobj_RecycleItem = _uiRoot.transform.FindChild("Anim/Source/RecycleItem").gameObject;

        Lbl_MoneyNum = _uiRoot.transform.FindChild("Anim/MoneyNum").gameObject.GetComponent<UILabel>();
        Spt_MoneyIcon = _uiRoot.transform.FindChild("Anim/MoneyIcon").gameObject.GetComponent<UISprite>();
        Btn_Store = _uiRoot.transform.FindChild("Anim/Store").gameObject;//.GetComponent<UIButton>();
        Btn_Confirm = _uiRoot.transform.FindChild("Anim/Confirm").gameObject;//.GetComponent<UIButton>();
        Btn_OneKeyChoose = _uiRoot.transform.FindChild("Anim/OneKeyChoose").gameObject;//.GetComponent<UIButton>();
        Lbl_RecycleMoneyNum = _uiRoot.transform.FindChild("Anim/RecycleMoney/RecycleMondyNum").gameObject.GetComponent<UILabel>();

    }

    public void SetLabelValues()
    {
    }

    public void Uninitialize()
    {

    }

}
