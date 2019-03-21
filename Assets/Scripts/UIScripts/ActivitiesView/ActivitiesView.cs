using UnityEngine;
using System.Collections;

public class ActivitiesView
{
    public static string UIName = "ActivitiesView";
    public GameObject _uiRoot;
    public UIButton Btn_RightActivity;
    public UILabel Lbl_BtnRightChallengeTime;
    public UISprite Tex_BtnRightActivity;
    public UISprite Spt_BtnRightActivitiesTip;
    public UIButton Btn_LeftActivity;
    public UILabel Lbl_BtnLeftChallengeTime;
    public UISprite Tex_BtnLeftActivity;
    public UISprite Spt_BtnLeftActivitiesTip;
    public UIButton Btn_CloseView;
    public GameObject Gobj_SelectDifficult;
    public UIButton Btn_CloseDifficult;
    public UIGrid Grd_DifficultGrid;
    public GameObject Gobj_DifficultComp;
    public UIButton Btn_gobj_DifficultComp;
    public UILabel Lbl_SurplusChanllenge;

    public UIButton Btn_Purchase;

    public TweenScale MainGroup_TScale;
    public TweenScale SelectDifficult_TScale;
    public GameObject DoubleCount;
    public GameObject DoubleReward;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ActivitiesView");
        DoubleCount = _uiRoot.transform.FindChild("Double/Count").gameObject;
        DoubleReward = _uiRoot.transform.FindChild("Double/Reward").gameObject;
        MainGroup_TScale = _uiRoot.transform.FindChild("gobj_MainGroup").gameObject.GetComponent<TweenScale>();
        SelectDifficult_TScale = _uiRoot.transform.FindChild("gobj_SelectDifficult").gameObject.GetComponent<TweenScale>();
        Btn_RightActivity = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/RightActivity").gameObject.GetComponent<UIButton>();
        Lbl_BtnRightChallengeTime = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/RightActivity/RightChallengeTime").gameObject.GetComponent<UILabel>();
        Tex_BtnRightActivity = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/RightActivity/RightActivities").gameObject.GetComponent<UISprite>();
        Spt_BtnRightActivitiesTip = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/RightActivity/RightActivitiesTip").gameObject.GetComponent<UISprite>();
        Btn_LeftActivity = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/LeftActivity").gameObject.GetComponent<UIButton>();
        Lbl_BtnLeftChallengeTime = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/LeftActivity/LeftChallengeTime").gameObject.GetComponent<UILabel>();
        Tex_BtnLeftActivity = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/LeftActivity/LeftActivities").gameObject.GetComponent<UISprite>();
        Spt_BtnLeftActivitiesTip = _uiRoot.transform.FindChild("gobj_MainGroup/Activities/LeftActivity/LeftActivitiesTip").gameObject.GetComponent<UISprite>();
        Btn_CloseView = _uiRoot.transform.FindChild("gobj_MainGroup/CloseView").gameObject.GetComponent<UIButton>();
        Gobj_SelectDifficult = _uiRoot.transform.FindChild("gobj_SelectDifficult").gameObject;
        Btn_CloseDifficult = _uiRoot.transform.FindChild("gobj_SelectDifficult/CloseDifficult").gameObject.GetComponent<UIButton>();
        Grd_DifficultGrid = _uiRoot.transform.FindChild("gobj_SelectDifficult/DifficultGroup/DifficultGrid").gameObject.GetComponent<UIGrid>();
        Gobj_DifficultComp = _uiRoot.transform.FindChild("gobj_SelectDifficult/DifficultGroup/DifficultGrid/gobj_DifficultComp").gameObject;
        Btn_gobj_DifficultComp = _uiRoot.transform.FindChild("gobj_SelectDifficult/DifficultGroup/DifficultGrid/gobj_DifficultComp").gameObject.GetComponent<UIButton>();
        Lbl_SurplusChanllenge = _uiRoot.transform.FindChild("gobj_SelectDifficult/SurplusGroup/SurplusChanllenge").gameObject.GetComponent<UILabel>();
        Btn_Purchase = _uiRoot.transform.FindChild("gobj_SelectDifficult/Purchase").gameObject.GetComponent<UIButton>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnRightChallengeTime.text = "活动时间：周一周三周五周日";
        Lbl_BtnLeftChallengeTime.text = "活动时间：周一周三周五周日";
        Lbl_SurplusChanllenge.text = "2";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
