using UnityEngine;
using System;
using System.Collections;

public class CaptureCityInfoView
{
    public static string UIName ="CaptureCityInfoView";
    public GameObject _uiRoot;
    public UISprite Spt_Mask;
    public UISprite Spt_BG;
    public UISprite Spt_Left1;
    public UISprite Spt_Right1;
    public UISprite Spt_Left2;
    public UISprite Spt_Right2;
    public UISprite Spt_Flower1;
    public UISprite Spt_Flower2;
    public UILabel Lbl_TitleLb;
    public UIScrollView Scl_ScoreItems;
    public UIGrid Grd_Grid;
    public UILabel Lbl_TitleScore;
    public UILabel Lbl_TitleName;
    public UILabel Lbl_TitleRank;
    public UILabel Lbl_TitleAward;
    public UISprite Spt_Selected;

    public GameObject Gobj_MyScoreItem;

    public UILabel Lbl_MyRank;
    public UILabel Lbl_MyName;
    public UILabel Lbl_MyScore;

    public UIButton Btn_UnionRank;
    public UISprite Spt_BtnUnionRankBackground;
    public UILabel Lbl_BtnUnionRankLabel;
    public UISprite Spt_BtnUnionRankfg;
    public UISprite Spt_BtnUnionRankfg1;
    public UIButton Btn_PersonRank;
    public UISprite Spt_BtnPersonRankBackground;
    public UILabel Lbl_BtnPersonRankLabel;
    public UISprite Spt_BtnPersonRankfg;
    public UISprite Spt_BtnPersonRankfg1;
    public UILabel Lbl_AwardDesc;
    public UILabel Lbl_AwardTitle;
    public UILabel Lbl_BoxAwardTitle;
    public UIGrid Grd_Award;

    //public UISprite Spt_PropBG;
    //public UISprite Spt_Quality;
    //public UISprite Spt_Icon;
    //public UISprite Spt_Mark;
    //public UILabel Lbl_Num;

    //public UISprite Spt_PropBG;
    //public UISprite Spt_Quality;
    //public UISprite Spt_Icon;
    //public UISprite Spt_Mark;
    //public UILabel Lbl_Num;

    //public UISprite Spt_PropBG;
    //public UISprite Spt_Quality;
    //public UISprite Spt_Icon;
    //public UISprite Spt_Mark;
    //public UILabel Lbl_Num;

    //public UISprite Spt_PropBG;
    //public UISprite Spt_Quality;
    //public UISprite Spt_Icon;
    //public UISprite Spt_Mark;
    //public UILabel Lbl_Num;

    //public UISprite Spt_PropBG;
    //public UISprite Spt_Quality;
    //public UISprite Spt_Icon;
    //public UISprite Spt_Mark;
    //public UILabel Lbl_Num;

    //public UISprite Spt_PropBG;
    //public UISprite Spt_Quality;
    //public UISprite Spt_Icon;
    //public UISprite Spt_Mark;
    //public UILabel Lbl_Num;

    public UILabel Lbl_FightCDTip;
    public UIButton Btn_Fight;
    public UISprite Spt_BtnFightBackground;
    public UILabel Lbl_BtnFightLabel;

    public GameObject Gobj_TitleLow;
    public GameObject Gobj_TitleHigh;
    public GameObject Gobj_CaptureScoreItem;

    public GameObject[] Gobj_Props = new GameObject[6];
    public UISprite[] Spt_PropBGs = new UISprite[6];
    public UISprite[] Spt_Qualitys = new UISprite[6];
    public UISprite[] Spt_Icons = new UISprite[6];
    public UISprite[] Spt_Marks = new UISprite[6];
    public UILabel[] Lbl_Nums = new UILabel[6];

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CaptureCityInfoView");
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Spt_BG = _uiRoot.transform.FindChild("BGS/BG").gameObject.GetComponent<UISprite>();
        Spt_Left1 = _uiRoot.transform.FindChild("BGS/Left1").gameObject.GetComponent<UISprite>();
        Spt_Right1 = _uiRoot.transform.FindChild("BGS/Right1").gameObject.GetComponent<UISprite>();
        Spt_Left2 = _uiRoot.transform.FindChild("BGS/Left2").gameObject.GetComponent<UISprite>();
        Spt_Right2 = _uiRoot.transform.FindChild("BGS/Right2").gameObject.GetComponent<UISprite>();
        Spt_Flower1 = _uiRoot.transform.FindChild("BGS/Flower1").gameObject.GetComponent<UISprite>();
        Spt_Flower2 = _uiRoot.transform.FindChild("BGS/Flower2").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("TitleObj/TitleLb").gameObject.GetComponent<UILabel>();

        Gobj_TitleLow = _uiRoot.transform.FindChild("TitleObj/TitleBGLow").gameObject;
        Gobj_TitleHigh = _uiRoot.transform.FindChild("TitleObj/TitleBGHigh").gameObject;

        Scl_ScoreItems = _uiRoot.transform.FindChild("ScoreItems").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("ScoreItems/Grid").gameObject.GetComponent<UIGrid>();
        Lbl_TitleScore = _uiRoot.transform.FindChild("TitleLabels/TitleScore").gameObject.GetComponent<UILabel>();
        Lbl_TitleName = _uiRoot.transform.FindChild("TitleLabels/TitleName").gameObject.GetComponent<UILabel>();
        Lbl_TitleRank = _uiRoot.transform.FindChild("TitleLabels/TitleRank").gameObject.GetComponent<UILabel>();
        Lbl_TitleAward = _uiRoot.transform.FindChild("TitleLabels/TitleAward").gameObject.GetComponent<UILabel>();

        Gobj_CaptureScoreItem = _uiRoot.transform.FindChild("Pre/CaptureScoreItem").gameObject;

        Spt_Selected = _uiRoot.transform.FindChild("Left/Selected").gameObject.GetComponent<UISprite>();

        Gobj_MyScoreItem = _uiRoot.transform.FindChild("Left/MyScore").gameObject;

        Lbl_MyRank = _uiRoot.transform.FindChild("Left/MyScore/Rank").gameObject.GetComponent<UILabel>();
        Lbl_MyName = _uiRoot.transform.FindChild("Left/MyScore/Name").gameObject.GetComponent<UILabel>();
        Lbl_MyScore = _uiRoot.transform.FindChild("Left/MyScore/Score").gameObject.GetComponent<UILabel>();

        Btn_UnionRank = _uiRoot.transform.FindChild("Left/UnionRank").gameObject.GetComponent<UIButton>();
        Spt_BtnUnionRankBackground = _uiRoot.transform.FindChild("Left/UnionRank/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnUnionRankLabel = _uiRoot.transform.FindChild("Left/UnionRank/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnUnionRankfg = _uiRoot.transform.FindChild("Left/UnionRank/fg").gameObject.GetComponent<UISprite>();
        Spt_BtnUnionRankfg1 = _uiRoot.transform.FindChild("Left/UnionRank/fg1").gameObject.GetComponent<UISprite>();
        Btn_PersonRank = _uiRoot.transform.FindChild("Left/PersonRank").gameObject.GetComponent<UIButton>();
        Spt_BtnPersonRankBackground = _uiRoot.transform.FindChild("Left/PersonRank/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnPersonRankLabel = _uiRoot.transform.FindChild("Left/PersonRank/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnPersonRankfg = _uiRoot.transform.FindChild("Left/PersonRank/fg").gameObject.GetComponent<UISprite>();
        Spt_BtnPersonRankfg1 = _uiRoot.transform.FindChild("Left/PersonRank/fg1").gameObject.GetComponent<UISprite>();
        Lbl_AwardDesc = _uiRoot.transform.FindChild("Right/AwardDesc").gameObject.GetComponent<UILabel>();
        Lbl_AwardTitle = _uiRoot.transform.FindChild("Right/AwardTitle").gameObject.GetComponent<UILabel>();
        Lbl_BoxAwardTitle = _uiRoot.transform.FindChild("Right/BoxAwardTitle").gameObject.GetComponent<UILabel>();
        Grd_Award = _uiRoot.transform.FindChild("Right/Award").gameObject.GetComponent<UIGrid>();

        Gobj_Props[0] = _uiRoot.transform.FindChild("Right/Award/Prop1").gameObject;
        Spt_PropBGs[0] = _uiRoot.transform.FindChild("Right/Award/Prop1/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Qualitys[0] = _uiRoot.transform.FindChild("Right/Award/Prop1/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icons[0]    = _uiRoot.transform.FindChild("Right/Award/Prop1/Icon").gameObject.GetComponent<UISprite>();
        Spt_Marks[0]    = _uiRoot.transform.FindChild("Right/Award/Prop1/Mark").gameObject.GetComponent<UISprite>();
        Lbl_Nums[0]     = _uiRoot.transform.FindChild("Right/Award/Prop1/Num").gameObject.GetComponent<UILabel>();

        Gobj_Props[1] = _uiRoot.transform.FindChild("Right/Award/Prop2").gameObject;
        Spt_PropBGs[1] = _uiRoot.transform.FindChild("Right/Award/Prop2/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Qualitys[1] = _uiRoot.transform.FindChild("Right/Award/Prop2/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icons[1] = _uiRoot.transform.FindChild("Right/Award/Prop2/Icon").gameObject.GetComponent<UISprite>();
        Spt_Marks[1] = _uiRoot.transform.FindChild("Right/Award/Prop2/Mark").gameObject.GetComponent<UISprite>();
        Lbl_Nums[1] = _uiRoot.transform.FindChild("Right/Award/Prop2/Num").gameObject.GetComponent<UILabel>();

        Gobj_Props[2] = _uiRoot.transform.FindChild("Right/Award/Prop3").gameObject;
        Spt_PropBGs[2] = _uiRoot.transform.FindChild("Right/Award/Prop3/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Qualitys[2] = _uiRoot.transform.FindChild("Right/Award/Prop3/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icons[2] = _uiRoot.transform.FindChild("Right/Award/Prop3/Icon").gameObject.GetComponent<UISprite>();
        Spt_Marks[2] = _uiRoot.transform.FindChild("Right/Award/Prop3/Mark").gameObject.GetComponent<UISprite>();
        Lbl_Nums[2] = _uiRoot.transform.FindChild("Right/Award/Prop3/Num").gameObject.GetComponent<UILabel>();

        Gobj_Props[3] = _uiRoot.transform.FindChild("Right/Award/Prop4").gameObject;
        Spt_PropBGs[3] = _uiRoot.transform.FindChild("Right/Award/Prop4/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Qualitys[3] = _uiRoot.transform.FindChild("Right/Award/Prop4/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icons[3] = _uiRoot.transform.FindChild("Right/Award/Prop4/Icon").gameObject.GetComponent<UISprite>();
        Spt_Marks[3] = _uiRoot.transform.FindChild("Right/Award/Prop4/Mark").gameObject.GetComponent<UISprite>();
        Lbl_Nums[3] = _uiRoot.transform.FindChild("Right/Award/Prop4/Num").gameObject.GetComponent<UILabel>();

        Gobj_Props[4] = _uiRoot.transform.FindChild("Right/Award/Prop5").gameObject;
        Spt_PropBGs[4] = _uiRoot.transform.FindChild("Right/Award/Prop5/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Qualitys[4] = _uiRoot.transform.FindChild("Right/Award/Prop5/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icons[4] = _uiRoot.transform.FindChild("Right/Award/Prop5/Icon").gameObject.GetComponent<UISprite>();
        Spt_Marks[4] = _uiRoot.transform.FindChild("Right/Award/Prop5/Mark").gameObject.GetComponent<UISprite>();
        Lbl_Nums[4] = _uiRoot.transform.FindChild("Right/Award/Prop5/Num").gameObject.GetComponent<UILabel>();

        Gobj_Props[5] = _uiRoot.transform.FindChild("Right/Award/Prop6").gameObject;
        Spt_PropBGs[5] = _uiRoot.transform.FindChild("Right/Award/Prop6/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Qualitys[5] = _uiRoot.transform.FindChild("Right/Award/Prop6/Quality").gameObject.GetComponent<UISprite>();
        Spt_Icons[5] = _uiRoot.transform.FindChild("Right/Award/Prop6/Icon").gameObject.GetComponent<UISprite>();
        Spt_Marks[5] = _uiRoot.transform.FindChild("Right/Award/Prop6/Mark").gameObject.GetComponent<UISprite>();
        Lbl_Nums[5] = _uiRoot.transform.FindChild("Right/Award/Prop6/Num").gameObject.GetComponent<UILabel>();

        Lbl_FightCDTip = _uiRoot.transform.FindChild("Right/FightCDTip").gameObject.GetComponent<UILabel>();
        Btn_Fight = _uiRoot.transform.FindChild("Right/Fight").gameObject.GetComponent<UIButton>();
        Spt_BtnFightBackground = _uiRoot.transform.FindChild("Right/Fight/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnFightLabel = _uiRoot.transform.FindChild("Right/Fight/Label").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text = "";
        Lbl_TitleScore.text = ConstString.LBL_SCORE;
        Lbl_TitleName.text = ConstString.LBL_MEMBER;
        Lbl_TitleRank.text = ConstString.LBL_RANK;
        Lbl_TitleAward.text = ConstString.LBL_CAPTURE_AWARD;
        Lbl_MyRank.text = "";
        Lbl_MyName.text = "";
        Lbl_MyScore.text = "";
        Lbl_BtnUnionRankLabel.text = ConstString.LBL_UNION_RANK;
        Lbl_BtnPersonRankLabel.text = ConstString.LBL_PERSON_RANK;
        Lbl_AwardDesc.text = "";
        Lbl_AwardTitle.text = ConstString.LBL_CAMPAIGN_AWARD;
        Lbl_BoxAwardTitle.text = "";
        Lbl_FightCDTip.text = "";
        Lbl_BtnFightLabel.text = ConstString.CAPTURETERRITORY_FIGHT_BTN;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
