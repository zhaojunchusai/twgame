using UnityEngine;
using System;
using System.Collections;

public class EndlessResultListView
{
    public static string UIName ="EndlessResultListView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_Content;
    public UISprite Spt_Content_BGDown;
    public UISprite Spt_Title_SpriteL;
    public UISprite Spt_Title_SpriteR;
    public UIPanel UIPanel_RewardScrollView;
    public UIScrollView ScrView_RewardScrollView;
    public UIGrid Grd_Grid;
    public UIWrapContent UIWrapContent_Grid;
    public UISprite Spt_Content_BGMiddle;
    public UISprite Spt_Mask;
    public UIButton Btn_Mask;
    public Transform Obj_Reward_Item;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/EndlessResultListView");
        UIPanel_Content = _uiRoot.transform.FindChild("Scale/Content").gameObject.GetComponent<UIPanel>();
        Spt_Content_BGDown = _uiRoot.transform.FindChild("Scale/Content/Content_BGDown").gameObject.GetComponent<UISprite>();
        Spt_Title_SpriteL = _uiRoot.transform.FindChild("Scale/Content/Title/Title_SpriteL").gameObject.GetComponent<UISprite>();
        Spt_Title_SpriteR = _uiRoot.transform.FindChild("Scale/Content/Title/Title_SpriteR").gameObject.GetComponent<UISprite>();
        UIPanel_RewardScrollView = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_RewardScrollView = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        Spt_Content_BGMiddle = _uiRoot.transform.FindChild("Scale/Content/Content_BGMiddle").gameObject.GetComponent<UISprite>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Btn_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UIButton>();
        Obj_Reward_Item = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView/Grid/Reward_Item");
    }

    public void SetLabelValues()
    {

    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
