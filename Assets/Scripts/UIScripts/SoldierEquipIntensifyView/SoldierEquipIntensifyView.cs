using UnityEngine;
using System;
using System.Collections;

public class SoldierEquipIntensifyView
{
    public static string UIName = "SoldierEquipIntensifyView";
    public GameObject _uiRoot;

    public UIButton Btn_SoldierEquipIntensify;
    public UISprite Spt_BtnSoldierEquipIntensifyBG;
    public UILabel Lbl_BtnSoldierEquipIntensifyLabel;

    public UILabel Lbl_ArtifactName;
    public UISprite Spt_SoldierEquipIntensifyQuality;
    public UISprite Spt_SoldierEquipIntensifyIcon;
    public UISprite Spt_SoldierEquipIntensifyBg;
    public UILabel Lbl_LevelLabelLeft;
    public UILabel Lbl_LevelLabelRight;
    public UIButton Btn_CloseArtifact;
    public UIGrid Grd_StarLevel;
    public UISprite Spt_ArtifactStar;
    public UIGrid Grd_BeforeGroup;
    public GameObject Gobj_BeforeAttGroup;
    public UIGrid Grd_IntensifiedGroup;
    public GameObject Gobj_IntensifiedAttGroup;
    public UILabel Lbl_CountLabel;
    public GameObject AfterLevelGroup;
    public GameObject MaxLevel;
    public UIButton Btn_QuickSoldierEquipIntensify;
    public UISprite Spt_BtnQuickSoldierEquipIntensifyBG;
    public UILabel Lbl_BtnQuickSoldierEquipIntensifyLabel;

    public GameObject Item_IntensifyEffect;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/SoldierEquipIntensifyView");
        Item_IntensifyEffect = _uiRoot.transform.FindChild("IntensifyLabelItem").gameObject;
        Btn_SoldierEquipIntensify = _uiRoot.transform.FindChild("SoldierEquipIntensify").gameObject.GetComponent<UIButton>();
        Spt_BtnSoldierEquipIntensifyBG = _uiRoot.transform.FindChild("SoldierEquipIntensify/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnSoldierEquipIntensifyLabel = _uiRoot.transform.FindChild("SoldierEquipIntensify/Label").gameObject.GetComponent<UILabel>();
        Lbl_ArtifactName = _uiRoot.transform.FindChild("NameGroup/ArtifactName").gameObject.GetComponent<UILabel>();
        Spt_SoldierEquipIntensifyQuality = _uiRoot.transform.FindChild("SoldierEquipComp/SoldierEquipIntensifyQuality").gameObject.GetComponent<UISprite>();
        Spt_SoldierEquipIntensifyIcon = _uiRoot.transform.FindChild("SoldierEquipComp/SoldierEquipIntensifyIcon").gameObject.GetComponent<UISprite>();
        Spt_SoldierEquipIntensifyBg = _uiRoot.transform.FindChild("SoldierEquipComp/SoldierEquipIntensifyBg").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabelLeft = _uiRoot.transform.FindChild("LevelGroup/LevelLabelLeft").gameObject.GetComponent<UILabel>();
        Lbl_LevelLabelRight = _uiRoot.transform.FindChild("LevelGroup/AfterGroup/LevelLabelRight").gameObject.GetComponent<UILabel>();
        Btn_CloseArtifact = _uiRoot.transform.FindChild("CloseArtifact").gameObject.GetComponent<UIButton>();
        Grd_StarLevel = _uiRoot.transform.FindChild("StarLevel").gameObject.GetComponent<UIGrid>();
        Spt_ArtifactStar = _uiRoot.transform.FindChild("StarLevel/ArtifactStar").gameObject.GetComponent<UISprite>();
        Grd_BeforeGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/BeforeGroup").gameObject.GetComponent<UIGrid>();
        Gobj_BeforeAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/BeforeGroup/BeforeAttGroup").gameObject;
        Grd_IntensifiedGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/IntensifiedGroup").gameObject.GetComponent<UIGrid>();
        Gobj_IntensifiedAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/IntensifiedGroup/IntensifiedAttGroup").gameObject;
        Lbl_CountLabel = _uiRoot.transform.FindChild("CostComp/CountLabel").gameObject.GetComponent<UILabel>();
        AfterLevelGroup = _uiRoot.transform.FindChild("LevelGroup/AfterGroup").gameObject;
        MaxLevel = _uiRoot.transform.FindChild("LevelGroup/MaxLevel").gameObject;
        Btn_QuickSoldierEquipIntensify = _uiRoot.transform.FindChild("QuickSoldierEquipIntensify").gameObject.GetComponent<UIButton>();
        Spt_BtnQuickSoldierEquipIntensifyBG = _uiRoot.transform.FindChild("QuickSoldierEquipIntensify/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnQuickSoldierEquipIntensifyLabel = _uiRoot.transform.FindChild("QuickSoldierEquipIntensify/Label").gameObject.GetComponent<UILabel>();

        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ArtifactName.text = "";
        Lbl_LevelLabelLeft.text = "";
        Lbl_LevelLabelRight.text = "";
        Lbl_CountLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
