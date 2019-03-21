using UnityEngine;
using System;
using System.Collections;

public class LevelUPView
{
    public static string UIName ="LevelUPView";
    public GameObject _uiRoot;
    //public UIPanel UIPanel_LevelUPView;
    //public UISprite Spt_TopCloseButton;
    public UIButton Btn_TopCloseButton;
    //public UISprite Spt_BGSprite;
    public UISprite Spt_MaskBGSprite;
    //public UISprite Spt_ContentBGSprite;
    //public UISprite Spt_ContentBGSprite2;
    //public UISprite Spt_DecorationSprite;
    //public UISprite Spt_TitleBGSprite;
    //public UISprite Spt_LVSprite;
    public UILabel Lbl_LevelNumLabel;
    //public UISprite Spt_LevelUpSprite;
    public UILabel Lbl_SPRewardLabel;
    public UILabel Lbl_SPRewardsNum;
    public UILabel Lbl_SPLimitLabel;
    public UILabel Lbl_NewSPLimitLabel;
    //public UISprite Spt_SPLimitArrowSprite;
    public UILabel Lbl_OldSPLimitLabel;
    public UILabel Lbl_LeadershipLabel;
    public UILabel Lbl_OldLeadershipLabel;
    //public UISprite Spt_LeadershipArrowSprite;
    public UILabel Lbl_NewLeadershipLabel;
    public UILabel Lbl_LifeLabel;
    public UILabel Lbl_OldLifeLabel;
    //public UISprite Spt_LifeArrowSprite;
    public UILabel Lbl_NewLifeLabel;
    public UILabel Lbl_AttackLabel;
    public UILabel Lbl_OldAttackLabel;
    //public UISprite Spt_AttackArrowSprite;
    public UILabel Lbl_NewAttackLabel;
    public TweenScale OffsetRoot_TScale;
    public UISpriteAnimation Spt_FX;


    public GameObject Go_Unlock;
    public UIGrid UnlockUIGrid;
    public GameObject Go_UnlockItem;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/LevelUPView");
        Go_Unlock = _uiRoot.transform.FindChild("OffsetRoot/Unlock").gameObject;
        Go_UnlockItem = _uiRoot.transform.FindChild("OffsetRoot/Unlock/UnlockScorllView/UnlockItem").gameObject;
        UnlockUIGrid = _uiRoot.transform.FindChild("OffsetRoot/Unlock/UnlockScorllView/Grid").gameObject.GetComponent<UIGrid>();

        //UIPanel_LevelUPView = _uiRoot.GetComponent<UIPanel>();
        Spt_FX = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/LVNum/Sprite").gameObject.GetComponent<UISpriteAnimation>();
        OffsetRoot_TScale = _uiRoot.transform.FindChild("OffsetRoot").gameObject.GetComponent<TweenScale>();
        //Spt_TopCloseButton = _uiRoot.transform.FindChild("OffsetRoot/TopCloseButton").gameObject.GetComponent<UISprite>();
        Btn_TopCloseButton = _uiRoot.transform.FindChild("OffsetRoot/TopCloseButton").gameObject.GetComponent<UIButton>();
        //Spt_BGSprite = _uiRoot.transform.FindChild("OffsetRoot/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_ContentBGSprite = _uiRoot.transform.FindChild("OffsetRoot/ContentBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_ContentBGSprite2 = _uiRoot.transform.FindChild("OffsetRoot/ContentBGSprite2").gameObject.GetComponent<UISprite>();
        //Spt_DecorationSprite = _uiRoot.transform.FindChild("OffsetRoot/DecorationSprite").gameObject.GetComponent<UISprite>();
        //Spt_TitleBGSprite = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleBGSprite").gameObject.GetComponent<UISprite>();
        //Spt_LVSprite = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/LVSprite").gameObject.GetComponent<UISprite>();
        Lbl_LevelNumLabel = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/LVNum/LevelNumLabel").gameObject.GetComponent<UILabel>();
        //Spt_LevelUpSprite = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/LevelUpSprite").gameObject.GetComponent<UISprite>();
        Lbl_SPRewardLabel = _uiRoot.transform.FindChild("OffsetRoot/SPRewardLabel").gameObject.GetComponent<UILabel>();
        Lbl_SPRewardsNum = _uiRoot.transform.FindChild("OffsetRoot/SPRewardsNum").gameObject.GetComponent<UILabel>();
        Lbl_SPLimitLabel = _uiRoot.transform.FindChild("OffsetRoot/SPLimitGroup/SPLimitLabel").gameObject.GetComponent<UILabel>();
        Lbl_NewSPLimitLabel = _uiRoot.transform.FindChild("OffsetRoot/SPLimitGroup/NewSPLimitLabel").gameObject.GetComponent<UILabel>();
        //Spt_SPLimitArrowSprite = _uiRoot.transform.FindChild("OffsetRoot/SPLimitGroup/SPLimitArrowSprite").gameObject.GetComponent<UISprite>();
        Lbl_OldSPLimitLabel = _uiRoot.transform.FindChild("OffsetRoot/SPLimitGroup/OldSPLimitLabel").gameObject.GetComponent<UILabel>();
        Lbl_LeadershipLabel = _uiRoot.transform.FindChild("OffsetRoot/LeaderGroup/LeadershipLabel").gameObject.GetComponent<UILabel>();
        Lbl_OldLeadershipLabel = _uiRoot.transform.FindChild("OffsetRoot/LeaderGroup/OldLeadershipLabel").gameObject.GetComponent<UILabel>();
        //Spt_LeadershipArrowSprite = _uiRoot.transform.FindChild("OffsetRoot/LeaderGroup/LeadershipArrowSprite").gameObject.GetComponent<UISprite>();
        Lbl_NewLeadershipLabel = _uiRoot.transform.FindChild("OffsetRoot/LeaderGroup/NewLeadershipLabel").gameObject.GetComponent<UILabel>();
        Lbl_LifeLabel = _uiRoot.transform.FindChild("OffsetRoot/LifeGroup/LifeLabel").gameObject.GetComponent<UILabel>();
        Lbl_OldLifeLabel = _uiRoot.transform.FindChild("OffsetRoot/LifeGroup/OldLifeLabel").gameObject.GetComponent<UILabel>();
        //Spt_LifeArrowSprite = _uiRoot.transform.FindChild("OffsetRoot/LifeGroup/LifeArrowSprite").gameObject.GetComponent<UISprite>();
        Lbl_NewLifeLabel = _uiRoot.transform.FindChild("OffsetRoot/LifeGroup/NewLifeLabel").gameObject.GetComponent<UILabel>();
        Lbl_AttackLabel = _uiRoot.transform.FindChild("OffsetRoot/AttGroup/AttackLabel").gameObject.GetComponent<UILabel>();
        Lbl_OldAttackLabel = _uiRoot.transform.FindChild("OffsetRoot/AttGroup/OldAttackLabel").gameObject.GetComponent<UILabel>();
        //Spt_AttackArrowSprite = _uiRoot.transform.FindChild("OffsetRoot/AttGroup/AttackArrowSprite").gameObject.GetComponent<UISprite>();
        Lbl_NewAttackLabel = _uiRoot.transform.FindChild("OffsetRoot/AttGroup/NewAttackLabel").gameObject.GetComponent<UILabel>();
       // SetLabelValues();
        OffsetRoot_TScale.gameObject.transform.localScale = Vector3.zero;
    }

    public void SetLabelValues()
    {
        Lbl_LevelNumLabel.text = "0";
        Lbl_SPRewardLabel.text = "体力奖励:";
        Lbl_SPRewardsNum.text = "0";
        Lbl_SPLimitLabel.text = "体力上限:";
        Lbl_NewSPLimitLabel.text = "9999";
        Lbl_OldSPLimitLabel.text = "9999";
        Lbl_LeadershipLabel.text = "统        御:";
        Lbl_OldLeadershipLabel.text = "9999";
        Lbl_NewLeadershipLabel.text = "9999";
        Lbl_LifeLabel.text = "生        命:";
        Lbl_OldLifeLabel.text = "9999";
        Lbl_NewLifeLabel.text = "9999";
        Lbl_AttackLabel.text = "攻        击:";
        Lbl_OldAttackLabel.text = "9999";
        Lbl_NewAttackLabel.text = "9999";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
