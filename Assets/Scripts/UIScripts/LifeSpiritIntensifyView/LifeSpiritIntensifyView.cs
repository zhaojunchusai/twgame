using UnityEngine;
using System;
using System.Collections;

public class LifeSpiritIntensifyView
{
    public static string UIName = "LifeSpiritIntensifyView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskSprite;
    public UIGrid Grd_AttGrid;
    public GameObject Gobj_LifeSpiritAttComp;
    public UISprite Spt_LifeSpiritQuality;
    public UISprite Spt_LifeSpiritIcon;
    public UISprite Spt_LifeSpiritFrame;
    public UISprite Spt_LifeSpiritTypeMark;
    public UILabel Lbl_LevelLabelLeft;
    public UILabel Lbl_LevelLabelRight;
    public GameObject Gobj_AfterGroup;
    public UISprite Spt_LifeSpiritMaxLevel;
    public UILabel Lbl_LifeSpiritName;
    public UISlider Slider_LifeSpiritProgressBar;
    public UILabel Lbl_SliderProgressBarLabel;
    public UIButton Btn_IntensifyButton;
    public UIButton Btn_FastSelectButton;
    public UIScrollView ScrView_MaterialScroll;
    public UIGrid Grd_MaterialGrid;
    //public UIWrapContent UIWrapContent_MaterialGrid;
    public GameObject Gobj_MaterialComp;
    public GameObject Gobj_IntensifyLabelItem;
    public UILabel Lbl_IntensifyLabelItem;
    public UILabel Lbl_SkillAttDesc;

    public GameObject Go_FlyEffect;
    public GameObject Go_ItemEffect;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/LifeSpiritIntensifyView");

        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_EQINTENSIFYITEMTRAIL, (GameObject gb) => { Go_FlyEffect = gb; });
        ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_EQINTENSIFYITEM, (GameObject gb) => { Go_ItemEffect = gb; });
        Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Grd_AttGrid = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/AttributeComparisonGroup/AttGrid").gameObject.GetComponent<UIGrid>();
        Gobj_LifeSpiritAttComp = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/AttributeComparisonGroup/AttGrid/gobj_LifeSpiritAttComp").gameObject;
        Spt_LifeSpiritQuality = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/gobj_LifeSpiritComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritIcon = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/gobj_LifeSpiritComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritFrame = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/gobj_LifeSpiritComp/Frame").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritTypeMark = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/gobj_LifeSpiritComp/TypeMark").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabelLeft = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/LevelGroup/LevelLabelLeft").gameObject.GetComponent<UILabel>();
        Gobj_AfterGroup = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/LevelGroup/AfterGroup").gameObject;
        Lbl_LevelLabelRight = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/LevelGroup/AfterGroup/LevelLabelRight").gameObject.GetComponent<UILabel>();
        Spt_LifeSpiritMaxLevel = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/LevelGroup/MaxLevel").gameObject.GetComponent<UISprite>();
        Lbl_LifeSpiritName = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/NameGroup/NameLabel").gameObject.GetComponent<UILabel>();
        Slider_LifeSpiritProgressBar = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/ProgressBar").gameObject.GetComponent<UISlider>();
        Lbl_SliderProgressBarLabel = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/ProgressBar/Label").gameObject.GetComponent<UILabel>();
        Btn_IntensifyButton = _uiRoot.transform.FindChild("Anim/ButtomGroup/IntensifyButton").gameObject.GetComponent<UIButton>();
        Btn_FastSelectButton = _uiRoot.transform.FindChild("Anim/ButtomGroup/FastSelectButton").gameObject.GetComponent<UIButton>();
        ScrView_MaterialScroll = _uiRoot.transform.FindChild("Anim/CenterGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_MaterialGrid = _uiRoot.transform.FindChild("Anim/CenterGroup/MaterialScroll/Grid").gameObject.GetComponent<UIGrid>();
        //UIWrapContent_MaterialGrid = _uiRoot.transform.FindChild("Anim/CenterGroup/MaterialScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        Gobj_MaterialComp = _uiRoot.transform.FindChild("Anim/CenterGroup/MaterialScroll/gobj_MaterialComp").gameObject;
        Gobj_IntensifyLabelItem = _uiRoot.transform.FindChild("IntensifyLabelItem").gameObject;
        Lbl_IntensifyLabelItem = _uiRoot.transform.FindChild("IntensifyLabelItem/Label").gameObject.GetComponent<UILabel>();
        Lbl_SkillAttDesc = _uiRoot.transform.FindChild("Anim/LifeSpiritInfoGroup/AttributeComparisonGroup/SkillAttDesc").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_IntensifyLabelItem.text = "[c4ad87]攻擊:88889";
        Lbl_LevelLabelLeft.text = "LV.99";
        Lbl_LevelLabelRight.text = "LV.99";
        Lbl_LifeSpiritName.text = "高順";
        Lbl_SliderProgressBarLabel.text = "150/150";
        Lbl_IntensifyLabelItem.text = "防禦+15";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
