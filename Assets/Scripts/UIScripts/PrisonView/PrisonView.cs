using UnityEngine;
using System;
using System.Collections;

public class PrisonView
{
    public static string UIName ="PrisonView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_Player;
    public UIPanel UIPanel_PrisonView;
    public UISprite Spt_Player_Back;
    public UILabel Lbl_Label_Level;
    public UILabel Lbl_Label_Name;
    public UIPanel UIPanel_Free_Type;
    public UIPanel UIPanel_Slavery_Type;
    public UISprite Spt_Slavery_Back;
    public UILabel Lbl_Label_Slavery;
    public UILabel Lbl_Label_slave;
    public UILabel Lbl_Label_get;
    public UILabel Lbl_Label_get_h;
    public UILabel Lbl_Label_get_had;
    public UIButton Btn_Button_close;
    public UIButton Btn_Button_OneKeyGet;
    public UIButton Btn_Button_PrisonRule;
    public UIButton Btn_Button_PrisonMark;
    public UIScrollView ScrView_PrisonScroll;
    public UIGrid Grd_Grid;
    public UIWrapContent UIWrapContent_Grid;
    public GameObject item;
    public GameObject MarkNotify;
    public UIPanel EffectPanel;
    public GameObject EffectMask;
    public GameObject Go_EffectPos_1;
    public GameObject Go_EffectPos_2;
    public GameObject Go_EffectPos_3;
    public GameObject Go_EffectPos_4;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/PrisonView");
        UIPanel_PrisonView = _uiRoot.GetComponent<UIPanel>();
        Spt_Player_Back = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/BackGround/Sprite_Back").gameObject.GetComponent<UISprite>();
        UIPanel_Player = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/Player").gameObject.GetComponent<UIPanel>();
        EffectPanel = _uiRoot.transform.FindChild("Effect").gameObject.GetComponent<UIPanel>();
        EffectMask = _uiRoot.transform.FindChild("Effect/EffectMask").gameObject;
        Go_EffectPos_1 = _uiRoot.transform.FindChild("Effect/EffectPos/1").gameObject;
        Go_EffectPos_2 = _uiRoot.transform.FindChild("Effect/EffectPos/2").gameObject;
        Go_EffectPos_3 = _uiRoot.transform.FindChild("Effect/EffectPos/3").gameObject;
        Go_EffectPos_4 = _uiRoot.transform.FindChild("Effect/EffectPos/4").gameObject;

        Lbl_Label_Level = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/NameGroup/Label_Level").gameObject.GetComponent<UILabel>();
        Lbl_Label_Name = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/NameGroup/Label_Name").gameObject.GetComponent<UILabel>();

        UIPanel_Free_Type = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/Free_Type").gameObject.GetComponent<UIPanel>();
        UIPanel_Slavery_Type = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/Slavery_Type").gameObject.GetComponent<UIPanel>();
        Spt_Slavery_Back = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/Slavery_Type/Sprite_Back").gameObject.GetComponent<UISprite>();
        Lbl_Label_Slavery = _uiRoot.transform.FindChild("LeftGroup/PlayerGroup/Slavery_Type/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_slave = _uiRoot.transform.FindChild("LeftGroup/InfoGroup/Label_slave/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_get = _uiRoot.transform.FindChild("LeftGroup/InfoGroup/Label_get/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_get_h = _uiRoot.transform.FindChild("LeftGroup/InfoGroup/Label_get_h/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_get_had = _uiRoot.transform.FindChild("LeftGroup/InfoGroup/Label_get_had/Label").gameObject.GetComponent<UILabel>();

        Btn_Button_close = _uiRoot.transform.FindChild("ButtonGroup/Button_close").gameObject.GetComponent<UIButton>();
        Btn_Button_OneKeyGet = _uiRoot.transform.FindChild("ButtonGroup/Button_OneKeyGet").gameObject.GetComponent<UIButton>();
        Btn_Button_PrisonRule = _uiRoot.transform.FindChild("ButtonGroup/Button_PrisonRule").gameObject.GetComponent<UIButton>();
        Btn_Button_PrisonMark = _uiRoot.transform.FindChild("ButtonGroup/Button_PrisonMark").gameObject.GetComponent<UIButton>();

        ScrView_PrisonScroll = _uiRoot.transform.FindChild("PrisonScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("PrisonScroll/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("PrisonScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        item = _uiRoot.transform.FindChild("PrisonScroll/item").gameObject;
        MarkNotify = _uiRoot.transform.FindChild("NotifyGroup/MarkNotify").gameObject;
        item.SetActive(false);
        EffectMask.SetActive(false);
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Label_Level.text = "";
        Lbl_Label_Name.text = "";
        Lbl_Label_slave.text = "";
        Lbl_Label_get.text = "";
        Lbl_Label_get_h.text = "";
        Lbl_Label_get_had.text = "";
        Lbl_Label_Level.text = "";
        Lbl_Label_Name.text = "";
        Lbl_Label_get_had.text = "";
        Lbl_Label_get_h.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
