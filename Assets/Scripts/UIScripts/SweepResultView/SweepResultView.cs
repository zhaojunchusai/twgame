using UnityEngine;
using System;
using System.Collections;

public class SweepResultView
{
    public static string UIName ="SweepResultView";
    public GameObject _uiRoot;
    public UIButton Btn_Mask;
    public UIPanel UIPanel_RewardScrollView;
    public UIScrollView ScrView_RewardScrollView;
    public TweenPosition TweenPos_RewardScrollView;
    public UISprite Spt_Hint;
    public UILabel Lbl_Title_Info;
    //自定义组件--------------------------------------------------------------//
    public Transform Obj_Reward_Item;
    //自定义组件--------------------------------------------------------------//
    public TweenScale Scale_TScale;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SweepResultView");
        Scale_TScale = _uiRoot.transform.FindChild("Scale").gameObject.GetComponent<TweenScale>();
        Btn_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UIButton>();
        UIPanel_RewardScrollView = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_RewardScrollView = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView").gameObject.GetComponent<UIScrollView>();
        TweenPos_RewardScrollView = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView").gameObject.GetComponent<TweenPosition>();
        Spt_Hint = _uiRoot.transform.FindChild("Scale/Content/Hint").gameObject.GetComponent<UISprite>();
        Lbl_Title_Info = _uiRoot.transform.FindChild("Scale/Content/Title/Title_Info").gameObject.GetComponent<UILabel>();
        Obj_Reward_Item = _uiRoot.transform.FindChild("Scale/Content/RewardScrollView/Grid/Reward_Item").gameObject.GetComponent<Transform>();
    }

    public void SetLabelValues()
    {
        //Lbl_Title_Info.text = string.Empty;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }
}
