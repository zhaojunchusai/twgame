using UnityEngine;
using System;
using System.Collections;

public class NoviceTaskView
{
    public static string UIName = "NoviceTaskView";
    public GameObject _uiRoot;
    public UIButton Btn_Close;
    //public UISprite Spt_BtnCloseBackground;
    //public UISprite Spt_ActivityBG;
    //public UISprite Spt_ActivitiesBG;
    //public UIPanel UIPanel_ActivitesScrollView;
    public UIScrollView ScrView_ActivitesScrollView;
    public UIGrid Grd_ActivitesGrid;
    public UIWrapContent UIWrapContent_ActivitesGrid;
    //public UISprite Spt_BannerBGSprite;
    //public UISprite Spt_BannerIconSprite;
    //public UISprite Spt_BannerDescSprite;
    //public UISprite Spt_BannerTextBGSprite;
    public UILabel Lbl_BannerDescLabel;
    public UILabel Lbl_BannerTitleLabel;
    //public UISprite Spt_DescBG;
    //public UISprite Spt_DecorateUpLeft;
    //public UISprite Spt_DecorateUpRight;
    //public UISprite Spt_DecorateDownLeft;
    //public UISprite Spt_DecorateDownRight;
    //public UIPanel UIPanel_DescScrollView;
    public UIScrollView ScrView_DescScrollView;
    public UIGrid Grd_DescGrid;
    public UISlider Slider_Slider;
    //public UISprite Spt_SliderSliderBackground;
    //public UISprite Spt_SliderSliderForeground;
    //public UISprite Spt_SliderSliderThumb;
    //public UISprite Spt_DescLightBG;
    public GameObject Gobj_NoviceTaskItem;
    //public UISprite Spt_ItemBG_1;
    //public UILabel Lbl_NameLabel;
    //public UILabel Lbl_ProgressLabel;
    //public UISprite Spt_Notify;
    //public TweenAlpha TweenAlpha_Notify;
    //public UISprite Spt_ChooseSprite;
    public GameObject Gobj_TaskAwardsItem;
    //public UISprite Spt_AwardsBGSprite;
    //public UIButton Btn_GetAwardButton;
    //public UISprite Spt_BtnGetAwardButtonBGSprite;
    //public UILabel Lbl_BtnGetAwardButtonFGLabel;
    //public UIGrid Grd_ItemsGrid;
    //public GameObject Gobj_Item;
    //public UISprite Spt_FreamSprite;
    //public UISprite Spt_Mark;
    //public UISprite Spt_IconBGSprite;
    //public UISprite Spt_IconSprite;
    //public UILabel Lbl_NumberLabel;
    //public UILabel Lbl_DescLabel;
    //public UISprite Spt_BG;
    public UILabel Lbl_DeadLineTip;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/NoviceTaskView");
        //UIPanel_NoviceTaskView = _uiRoot.GetComponent<UIPanel>();
        Btn_Close = _uiRoot.transform.FindChild("Layer1/Close").gameObject.GetComponent<UIButton>();
        ////Spt_BtnCloseBackground = _uiRoot.transform.FindChild("Layer1/Close/Background").gameObject.GetComponent<UISprite>();
        //Spt_ActivityBG = _uiRoot.transform.FindChild("Layer1/ActivityBG").gameObject.GetComponent<UISprite>();
        //Spt_ActivitiesBG = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitiesBG").gameObject.GetComponent<UISprite>();
        //UIPanel_ActivitesScrollView = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ActivitesScrollView = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_ActivitesGrid = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView/ActivitesGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_ActivitesGrid = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView/ActivitesGrid").gameObject.GetComponent<UIWrapContent>();
        //Spt_BannerBGSprite = _uiRoot.transform.FindChild("Layer1/BannerGroup/BannerBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_BannerIconSprite = _uiRoot.transform.FindChild("Layer1/BannerGroup/BannerIconSprite").gameObject.GetComponent<UISprite>();
        //Spt_BannerDescSprite = _uiRoot.transform.FindChild("Layer1/BannerGroup/BannerDescSprite").gameObject.GetComponent<UISprite>();
        //Spt_BannerTextBGSprite = _uiRoot.transform.FindChild("Layer1/BannerGroup/BannerTextBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_BannerDescLabel = _uiRoot.transform.FindChild("Layer1/BannerGroup/BannerDescLabel").gameObject.GetComponent<UILabel>();
        Lbl_BannerTitleLabel = _uiRoot.transform.FindChild("Layer1/BannerGroup/BannerTitleLabel").gameObject.GetComponent<UILabel>();
        //Spt_DescBG = _uiRoot.transform.FindChild("Layer1/DescGroup/DescBG").gameObject.GetComponent<UISprite>();
        //Spt_DecorateUpLeft = _uiRoot.transform.FindChild("Layer1/DescGroup/DecorateUpLeft").gameObject.GetComponent<UISprite>();
        //Spt_DecorateUpRight = _uiRoot.transform.FindChild("Layer1/DescGroup/DecorateUpRight").gameObject.GetComponent<UISprite>();
        //Spt_DecorateDownLeft = _uiRoot.transform.FindChild("Layer1/DescGroup/DecorateDownLeft").gameObject.GetComponent<UISprite>();
        //Spt_DecorateDownRight = _uiRoot.transform.FindChild("Layer1/DescGroup/DecorateDownRight").gameObject.GetComponent<UISprite>();
        //UIPanel_DescScrollView = _uiRoot.transform.FindChild("Layer1/DescGroup/DescScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_DescScrollView = _uiRoot.transform.FindChild("Layer1/DescGroup/DescScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_DescGrid = _uiRoot.transform.FindChild("Layer1/DescGroup/DescScrollView/DescGrid").gameObject.GetComponent<UIGrid>();
        Slider_Slider = _uiRoot.transform.FindChild("Layer1/DescGroup/Slider").gameObject.GetComponent<UISlider>();
        //Spt_SliderSliderBackground = _uiRoot.transform.FindChild("Layer1/DescGroup/Slider/Background").gameObject.GetComponent<UISprite>();
        //Spt_SliderSliderForeground = _uiRoot.transform.FindChild("Layer1/DescGroup/Slider/Foreground").gameObject.GetComponent<UISprite>();
        //Spt_SliderSliderThumb = _uiRoot.transform.FindChild("Layer1/DescGroup/Slider/Thumb").gameObject.GetComponent<UISprite>();
        //Spt_DescLightBG = _uiRoot.transform.FindChild("Layer1/DescGroup/DescLightBG").gameObject.GetComponent<UISprite>();
        Gobj_NoviceTaskItem = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem").gameObject;
        //Spt_ItemBG_1 = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem/ItemBG_1").gameObject.GetComponent<UISprite>();
        //Lbl_NameLabel = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem/NameLabel").gameObject.GetComponent<UILabel>();
        //Lbl_ProgressLabel = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem/ProgressLabel").gameObject.GetComponent<UILabel>();
        //Spt_Notify = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem/Notify").gameObject.GetComponent<UISprite>();
        //TweenAlpha_Notify = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem/Notify").gameObject.GetComponent<TweenAlpha>();
        //Spt_ChooseSprite = _uiRoot.transform.FindChild("Layer1/Source/gobj_NoviceTaskItem/ChooseSprite").gameObject.GetComponent<UISprite>();
        Gobj_TaskAwardsItem = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem").gameObject;
        //Spt_AwardsBGSprite = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/AwardsBGSprite").gameObject.GetComponent<UISprite>();
        //Btn_GetAwardButton = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/GetAwardButton").gameObject.GetComponent<UIButton>();
        //Spt_BtnGetAwardButtonBGSprite = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/GetAwardButton/BGSprite").gameObject.GetComponent<UISprite>();
        //Lbl_BtnGetAwardButtonFGLabel = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/GetAwardButton/FGLabel").gameObject.GetComponent<UILabel>();
        //Grd_ItemsGrid = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid").gameObject.GetComponent<UIGrid>();
        //Gobj_Item = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid/gobj_Item").gameObject;
        //Spt_FreamSprite = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid/gobj_Item/FreamSprite").gameObject.GetComponent<UISprite>();
        //Spt_Mark = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid/gobj_Item/FreamSprite/Mark").gameObject.GetComponent<UISprite>();
        //Spt_IconBGSprite = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid/gobj_Item/IconBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconSprite = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid/gobj_Item/IconSprite").gameObject.GetComponent<UISprite>();
        //Lbl_NumberLabel = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/ItemsGrid/gobj_Item/NumberLabel").gameObject.GetComponent<UILabel>();
        //Lbl_DescLabel = _uiRoot.transform.FindChild("Layer1/Source/gobj_TaskAwardsItem/DescLabel").gameObject.GetComponent<UILabel>();
        //Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Lbl_DeadLineTip = _uiRoot.transform.FindChild("Layer1/DeadLineTip").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BannerDescLabel.text = "";
        Lbl_BannerTitleLabel.text = "任务描述:";
        //Lbl_NameLabel.text = "活动名称活动";
        //Lbl_ProgressLabel.text = "99/99";
        //Lbl_BtnGetAwardButtonFGLabel.text = "领取奖励";
        //Lbl_NumberLabel.text = "x20000";
        //Lbl_DescLabel.text = "奖励";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
