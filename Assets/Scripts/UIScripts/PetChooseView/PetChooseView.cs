using UnityEngine;
using System;
using System.Collections;

public class PetChooseView
{
    public static string UIName ="PetChooseView";
    public GameObject _uiRoot;
    public UISprite Spt_MarkSprite;
    public UIScrollView ScrView_PetChooseScrollView;
    public UIGrid Grd_Grid;
    public UIWrapContent UIWrapContent_Grid;
    public GameObject Gobj_PetChooseComp;
    public UIButton Btn_UninstallButton;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/PetChooseView");
        Spt_MarkSprite = _uiRoot.transform.FindChild("MarkSprite").gameObject.GetComponent<UISprite>();
        ScrView_PetChooseScrollView = _uiRoot.transform.FindChild("Anim/PetChooseScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("Anim/PetChooseScrollView/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("Anim/PetChooseScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        Gobj_PetChooseComp = _uiRoot.transform.FindChild("Anim/PetChooseScrollView/Grid/gobj_PetChooseComp").gameObject;
         Btn_UninstallButton = _uiRoot.transform.FindChild("Anim/UninstallButton").gameObject.GetComponent<UIButton>();
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
