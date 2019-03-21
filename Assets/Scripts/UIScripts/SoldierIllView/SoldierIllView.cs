using UnityEngine;
using System;
using System.Collections;

public class SoldierIllView
{
    public static string UIName ="SoldierIllView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_SoldierIllView;
    public UIScrollView ScrView_EquipAndSkillScrollView;
    public UIGrid Grd_Grid;
    public UIWrapContent UIWrapContent_Grid;
    public UISprite Spt_mark;
    public UIButton Btn_Button_close;
    public GameObject item;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SoldierIllView");
        UIPanel_SoldierIllView = _uiRoot.GetComponent<UIPanel>();
        ScrView_EquipAndSkillScrollView = _uiRoot.transform.FindChild("EquipAndSkillScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        Btn_Button_close = _uiRoot.transform.FindChild("Button_close").gameObject.GetComponent<UIButton>();
        item = _uiRoot.transform.FindChild("EquipAndSkillScrollView/item").gameObject;
        SetLabelValues();
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
