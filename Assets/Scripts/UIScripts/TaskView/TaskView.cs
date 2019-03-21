using UnityEngine;
using System;
using System.Collections;

public class TaskView
{
    public static string UIName ="TaskView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_TaskView;
    //public UISprite Spt_TaskViewBG;
    public UISprite Spt_BG;
    public UISprite Spt_Title;
    public UISprite Spt_Sprite;
    public UIPanel UIPanel_TaskGroup;
    public UIScrollView ScrView_TaskGroup;
    public UIGrid Grd_UIGrid;
    public UIWrapContent UIWrapContent_UIGrid;
    public TweenScale Anim_TScale;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/TaskView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        //UIPanel_TaskView = _uiRoot.GetComponent<UIPanel>();
        //Btn_BackBtn = _uiRoot.transform.FindChild("Anim/BackBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnBackBtnBG = _uiRoot.transform.FindChild("Anim/BackBtn/BG").gameObject.GetComponent<UISprite>();
        Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        //Spt_Title = _uiRoot.transform.FindChild("Anim/TaskViewBG/Title").gameObject.GetComponent<UISprite>();
        //Spt_BottomDRB = _uiRoot.transform.FindChild("Anim/TaskViewBG/BottomDRB").gameObject.GetComponent<UISprite>();
        //Spt_TopDRB = _uiRoot.transform.FindChild("Anim/TaskViewBG/TopDRB").gameObject.GetComponent<UISprite>();
        //Spt_Sprite = _uiRoot.transform.FindChild("Anim/TaskViewBG/Sprite").gameObject.GetComponent<UISprite>();
        //UIPanel_TaskGroup = _uiRoot.transform.FindChild("Anim/TaskGroup").gameObject.GetComponent<UIPanel>();
        ScrView_TaskGroup = _uiRoot.transform.FindChild("Anim/TaskGroup").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid = _uiRoot.transform.FindChild("Anim/TaskGroup/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid = _uiRoot.transform.FindChild("Anim/TaskGroup/UIGrid").gameObject.GetComponent<UIWrapContent>();
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
