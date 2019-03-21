using UnityEngine;
using System;
using System.Collections;

public class AchievementView
{
    public static string UIName ="AchievementView";
    public GameObject _uiRoot;
    public UISprite Spt_Quit;
    public UIScrollView ScrView_AchievementGroup;
    public UIGrid Grd_UIGrid;
    public UIWrapContent UIWrapContent_UIGrid;

    public GameObject Gobj_GetAchievemenet;
    public UISprite Spt_BGMask;
    public TweenScale TS_AWard_Scale;
    public UISprite Spt_AwardIconFrame;
    public UISprite Spt_AwardIcon;
    public UILabel Lbl_UnlockLabel;
    public UILabel Lbl_TimeLabel;
    public TweenScale TS_Effect_Scale;
    public TweenRotation TR_Effect_Rotation;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/AchievementView");
        Spt_Quit = _uiRoot.transform.FindChild("Quit").gameObject.GetComponent<UISprite>();
        ScrView_AchievementGroup = _uiRoot.transform.FindChild("Anim/AchievementGroup").gameObject.GetComponent<UIScrollView>();
        Grd_UIGrid = _uiRoot.transform.FindChild("Anim/AchievementGroup/UIGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_UIGrid = _uiRoot.transform.FindChild("Anim/AchievementGroup/UIGrid").gameObject.GetComponent<UIWrapContent>();

        Gobj_GetAchievemenet = _uiRoot.transform.FindChild("GetAchievemenet").gameObject;
        Spt_BGMask = _uiRoot.transform.FindChild("GetAchievemenet/BG_Mask").gameObject.GetComponent<UISprite>();
        TS_AWard_Scale = _uiRoot.transform.FindChild("GetAchievemenet/Anim").gameObject.GetComponent<TweenScale>();
        Spt_AwardIconFrame = _uiRoot.transform.FindChild("GetAchievemenet/Anim/AchieveSprite/AwardIconFrame").gameObject.GetComponent<UISprite>();
        Spt_AwardIcon = _uiRoot.transform.FindChild("GetAchievemenet/Anim/AchieveSprite/AwardIconFrame/AwardIcon").gameObject.GetComponent<UISprite>();
        Lbl_UnlockLabel = _uiRoot.transform.FindChild("GetAchievemenet/Anim/AchieveSprite/UnlockLabel").gameObject.GetComponent<UILabel>();
        Lbl_TimeLabel = _uiRoot.transform.FindChild("GetAchievemenet/Anim/AchieveSprite/TimeLabel").gameObject.GetComponent<UILabel>();
        TS_Effect_Scale = _uiRoot.transform.FindChild("GetAchievemenet/Effect/lizi").gameObject.GetComponent<TweenScale>();
        TR_Effect_Rotation = _uiRoot.transform.FindChild("GetAchievemenet/Effect/lizi").gameObject.GetComponent<TweenRotation>();

    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
