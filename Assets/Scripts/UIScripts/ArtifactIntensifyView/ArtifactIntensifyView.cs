using UnityEngine;
using System;
using System.Collections;

public class ArtifactIntensifyView
{
    public static string UIName = "ArtifactIntensifyView";
    public GameObject _uiRoot;
    public UISprite Spt_ArtifactIntensifyQuality;
    public UISpriteAnimation SptA_ArtifactIntensifyQualityEffect;
    public UISprite Tex_ArtifactIntensifyIcon;
    public UISprite Spt_ArtifactIntensifyItemBg;
    public UIGrid Grd_IntensifyMaterial;
    public GameObject Gobj_IntensifyMaterialComp;
    public UIButton Btn_ArtifactIntensify;
    public UILabel Lbl_IntensifyArtifactName;
    public GameObject Btn_CloseArtifactIntensify;
    public GameObject Item_IntensifyEffect;
    public UILabel Lbl_ArtifactIntensifyConsume;
    public GameObject AttGroup;
    //public UIGrid Grd_BeforeGroup;
    //public GameObject Gobj_BeforeAttGroup;
    public UIGrid Grd_IntensifiedGroup;
    public GameObject Gobj_IntensifiedAttGroup;
    public GameObject skillBefor;
    public UILabel Lbl_Label_skillBefor;
    public UILabel Lbl_Label_Lv_Befor;
    public UILabel Lbl_Label_Lv_After;
    public GameObject AfterGroup;
    public GameObject MaxLevel;

    public  GameObject Go_FlyEffect;
    public  GameObject Go_ItemEffect;
    public TweenScale ArtifactIntensify_TScale;
    public QuickMatItem QuickMatItem;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ArtifactIntensifyView");
        ArtifactIntensify_TScale = _uiRoot.gameObject.GetComponent<TweenScale>();
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_EQINTENSIFYITEMTRAIL, (GameObject gb) => { Go_FlyEffect = gb; });
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_EQINTENSIFYITEM, (GameObject gb) => { Go_ItemEffect = gb; });
        Item_IntensifyEffect = _uiRoot.transform.FindChild("IntensifyLabelItem").gameObject;
        Spt_ArtifactIntensifyQuality = _uiRoot.transform.FindChild("ArtifactComp/ArtifactIntensifyQuality").gameObject.GetComponent<UISprite>();
        SptA_ArtifactIntensifyQualityEffect = _uiRoot.transform.FindChild("ArtifactComp/ArtifactIntensifyQuality/ArtifactIntensifyQualityEffect").gameObject.GetComponent<UISpriteAnimation>();        
        Tex_ArtifactIntensifyIcon = _uiRoot.transform.FindChild("ArtifactComp/ArtifactIntensifyIcon").gameObject.GetComponent<UISprite>();
        Spt_ArtifactIntensifyItemBg = _uiRoot.transform.FindChild("ArtifactComp/ArtifactIntensifyItemBg").gameObject.GetComponent<UISprite>();
        Grd_IntensifyMaterial = _uiRoot.transform.FindChild("ArtifactIntensifyMaterial/IntensifyMaterial").gameObject.GetComponent<UIGrid>();
        Gobj_IntensifyMaterialComp = _uiRoot.transform.FindChild("ArtifactIntensifyMaterial/IntensifyMaterial/gobj_IntensifyMaterialComp").gameObject;
        Btn_ArtifactIntensify = _uiRoot.transform.FindChild("ArtifactIntensify").gameObject.GetComponent<UIButton>();
        Lbl_IntensifyArtifactName = _uiRoot.transform.FindChild("NameGroup/IntensifyArtifactName").gameObject.GetComponent<UILabel>();
        Btn_CloseArtifactIntensify = _uiRoot.transform.FindChild("MaskSprite").gameObject;
        Lbl_ArtifactIntensifyConsume = _uiRoot.transform.FindChild("IntensifyGroup/ArtifactIntensifyConsume").gameObject.GetComponent<UILabel>();

        AttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/AttGroup").gameObject;

        //Grd_BeforeGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/AttGroup/BeforeGroup").gameObject.GetComponent<UIGrid>();
        //Gobj_BeforeAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/AttGroup/BeforeGroup/BeforeAttGroup").gameObject;
        Grd_IntensifiedGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/AttGroup/IntensifiedGroup").gameObject.GetComponent<UIGrid>();
        Gobj_IntensifiedAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/AttGroup/IntensifiedGroup/IntensifiedAttGroup").gameObject;
        skillBefor = _uiRoot.transform.FindChild("AttributeComparisonGroup/BeforeSkill").gameObject;
        Lbl_Label_skillBefor = _uiRoot.transform.FindChild("AttributeComparisonGroup/BeforeSkill/Label").gameObject.GetComponent<UILabel>();

        Lbl_Label_Lv_Befor = _uiRoot.transform.FindChild("LevelGroup/LevelLabelLeft").gameObject.GetComponent<UILabel>();
        Lbl_Label_Lv_After = _uiRoot.transform.FindChild("LevelGroup/AfterGroup/LevelLabelRight").gameObject.GetComponent<UILabel>();
        AfterGroup = _uiRoot.transform.FindChild("LevelGroup/AfterGroup").gameObject;
        MaxLevel = _uiRoot.transform.FindChild("LevelGroup/MaxLevel").gameObject;
        QuickMatItem = _uiRoot.transform.FindChild("QuickMatItem").gameObject.GetComponent<QuickMatItem>();
        if (QuickMatItem == null)
            QuickMatItem = _uiRoot.transform.FindChild("QuickMatItem").gameObject.AddComponent<QuickMatItem>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_IntensifyArtifactName.text = "";
        Lbl_ArtifactIntensifyConsume.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
