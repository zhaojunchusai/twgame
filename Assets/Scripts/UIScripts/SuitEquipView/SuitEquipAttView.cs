using System.Collections.Generic;
using UnityEngine;
using System;
public class SuitEquipAttView
{
    public GameObject _uiRoot;
    public UISprite Spt_PanelBg;
    //public UISprite Spt_SuitBg;
    public UIGrid Grid_Att;
    public GameObject Gobj_SuitAttComp;
    public UISprite Spt_Attribute;
    public GameObject Gobj_SuitGroup;
    public UISprite Spt_Light;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/SuitEquipAttView");
        Spt_PanelBg = _uiRoot.transform.FindChild("Bg").GetComponent<UISprite>();
        //Spt_SuitBg = _uiRoot.transform.FindChild("SuitGroup/Bg_Suit").GetComponent<UISprite>();
        Spt_Attribute = _uiRoot.transform.FindChild("AttributeBias/Att").GetComponent<UISprite>();
        Grid_Att = _uiRoot.transform.FindChild("SuitGroup/Grid").GetComponent<UIGrid>();
        Gobj_SuitAttComp = _uiRoot.transform.FindChild("SuitGroup/SuitAttComp").gameObject;
        Gobj_SuitGroup = _uiRoot.transform.FindChild("SuitGroup").gameObject;
        Spt_Light = _uiRoot.transform.FindChild("Separator").gameObject.GetComponent<UISprite>();
        Gobj_SuitAttComp.SetActive(false);
    }
}