using UnityEngine;
using System;
using System.Collections;

public class PrisonMarkView
{
    public static string UIName ="PrisonMarkView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_PrisonMarkView;
    public UIButton Btn_Button_close;
    public UIScrollView ScrView_MaterialScroll;
    public UIGrid Grd_Grid;
    public UIWrapContent UIWrapContent_Grid;
    public UILabel Title;
    public GameObject Item;
    public GameObject Mark;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/PrisonMarkView");
        UIPanel_PrisonMarkView = _uiRoot.GetComponent<UIPanel>();
        Btn_Button_close = _uiRoot.transform.FindChild("Button_close").gameObject.GetComponent<UIButton>();
        ScrView_MaterialScroll = _uiRoot.transform.FindChild("MarkGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("MarkGroup/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("MarkGroup/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        Title = _uiRoot.transform.FindChild("Title/Label").gameObject.GetComponent<UILabel>();
        Item = _uiRoot.transform.FindChild("MarkGroup/MaterialScroll/item").gameObject;
        Mark = _uiRoot.transform.FindChild("BackGroundGroup/MarkSprite").gameObject;
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
