using UnityEngine;
using System;
using System.Collections;

public class ArtifactDetailView
{
    public static string UIName ="ArtifactDetailView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_ArtifactDetailView;
    public GameObject Gobj_Detail;
    public UISprite Spt_BgSprite;
    public GameObject Gobj_ItemBaseComp;
    public UISprite Spt_ItemBg;
    public UISprite Spt_ItemIcon;
    public UISprite Spt_ItemQuality;
    public UIGrid Grd_AttGroup;
    public GameObject Gobj_EquipAttComp;
    //public UISprite Spt_Icon;
    //public UILabel Lbl_Label;
    //public UISprite Spt_BgSprite2;
    public UILabel Lbl_ItemDesc;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/ArtifactDetailView");
        UIPanel_ArtifactDetailView = _uiRoot.GetComponent<UIPanel>();
        Gobj_Detail = _uiRoot.transform.FindChild("gobj_Detail").gameObject;
        Spt_BgSprite = _uiRoot.transform.FindChild("gobj_Detail/BgSprite").gameObject.GetComponent<UISprite>();
        Gobj_ItemBaseComp = _uiRoot.transform.FindChild("gobj_Detail/gobj_ItemBaseComp").gameObject;
        Spt_ItemBg = _uiRoot.transform.FindChild("gobj_Detail/gobj_ItemBaseComp/ItemBg").gameObject.GetComponent<UISprite>();
        Spt_ItemIcon = _uiRoot.transform.FindChild("gobj_Detail/gobj_ItemBaseComp/ItemIcon").gameObject.GetComponent<UISprite>();
        Spt_ItemQuality = _uiRoot.transform.FindChild("gobj_Detail/gobj_ItemBaseComp/ItemQuality").gameObject.GetComponent<UISprite>();
        Grd_AttGroup = _uiRoot.transform.FindChild("gobj_Detail/AttGroup").gameObject.GetComponent<UIGrid>();
        Gobj_EquipAttComp = _uiRoot.transform.FindChild("gobj_Detail/AttGroup/gobj_EquipAttComp").gameObject;
        //Spt_Icon = _uiRoot.transform.FindChild("gobj_Detail/AttGroup/EquipAttComp/Icon").gameObject.GetComponent<UISprite>();
        //Lbl_Label = _uiRoot.transform.FindChild("gobj_Detail/AttGroup/EquipAttComp/Label").gameObject.GetComponent<UILabel>();
        //Spt_BgSprite2 = _uiRoot.transform.FindChild("gobj_Detail/BgSprite2").gameObject.GetComponent<UISprite>();
        Lbl_ItemDesc = _uiRoot.transform.FindChild("gobj_Detail/ItemDesc").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ItemDesc.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
