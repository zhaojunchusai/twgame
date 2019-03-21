using UnityEngine;
using System;
using System.Collections;

public class PrisonRuleView
{
    public static string UIName ="PrisonRuleView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_PrisonRuleView;
    public UIButton Btn_Button_close;
    public UILabel Lbl_Label_Rule;
    public UILabel Lbl_label_PrisonRule;
    public GameObject Marsk;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/PrisonRuleView");
        UIPanel_PrisonRuleView = _uiRoot.GetComponent<UIPanel>();
        Btn_Button_close = _uiRoot.transform.FindChild("Button_close").gameObject.GetComponent<UIButton>();
        Lbl_Label_Rule = _uiRoot.transform.FindChild("RulelScrollView/Table/Label_Rule").gameObject.GetComponent<UILabel>();
        Lbl_label_PrisonRule = _uiRoot.transform.FindChild("Title/Label").gameObject.GetComponent<UILabel>();
        Marsk = _uiRoot.transform.FindChild("BackGroundGroup/MarkSprite").gameObject;
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Label_Rule.text = "";
    }

    public void Uninitialize()
    {

    }
    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
