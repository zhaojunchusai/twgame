using UnityEngine;
using System;
using System.Collections;

public class ExoticAdvantureInfoView
{
    public static string UIName = "ExoticAdvantureInfoView";
    public GameObject _uiRoot;
    public GameObject Gobj_StageInfoGroup;
    public UIButton Btn_Close;
    public UILabel Lbl_TitleLabel;
    public UILabel Lbl_StageDesc;
    public UIGrid Grd_EnemyGrid;
    public GameObject Gobj_EnemyInfoComp;
    public UIGrid Grd_AwardsGrid;
    public GameObject Gobj_AwardsInfoComp;
    public GameObject Gobj_SPGroup;
    public UILabel Lbl_SPLabel;
    public UIButton Btn_ReadyBattle;
    public GameObject Gobj_GateCountGroup;
    public UILabel Lbl_GateCountLabel;
    public GameObject Gobj_ChallengeGroup;
    public UISprite Spt_PlayerQuality;
    public UISprite Spt_PlayerIcon;
    public UISprite Spt_PlayerBG;
    public UILabel Lbl_PlayerName;
    public UILabel Lbl_ChallengeState;
    public GameObject Gobj_BossDescGroup;
    public UILabel Lbl_BossDesc;
    public UISlider Slider_BossBloodSlider;
    public UILabel Lbl_BossBlood;
    public UILabel Lbl_SurplusCount;
    public UIButton Btn_Purchase;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ExoticAdvantureInfoView");
        Gobj_StageInfoGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup").gameObject;
        Btn_Close = _uiRoot.transform.FindChild("gobj_StageInfoGroup/Close").gameObject.GetComponent<UIButton>();
        Lbl_TitleLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        Lbl_StageDesc = _uiRoot.transform.FindChild("gobj_StageInfoGroup/DescGroup/StageDesc").gameObject.GetComponent<UILabel>();
        Grd_EnemyGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyGrid").gameObject.GetComponent<UIGrid>();
        Gobj_EnemyInfoComp = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyGrid/gobj_EnemyInfoComp").gameObject;

        Grd_AwardsGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsGrid").gameObject.GetComponent<UIGrid>();
        Gobj_AwardsInfoComp = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsGrid/gobj_AwardsInfoComp").gameObject;
        Gobj_SPGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/gobj_SPGroup").gameObject;
        Lbl_SPLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/gobj_SPGroup/SPLabel").gameObject.GetComponent<UILabel>();
        Btn_ReadyBattle = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/ReadyBattle").gameObject.GetComponent<UIButton>();
        Gobj_GateCountGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_GateCountGroup").gameObject;
        Lbl_GateCountLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_GateCountGroup/GateCountLabel").gameObject.GetComponent<UILabel>();
        Gobj_ChallengeGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_ChallengeGroup").gameObject;
        Spt_PlayerQuality = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_ChallengeGroup/PlayerQuality").gameObject.GetComponent<UISprite>();
        Spt_PlayerIcon = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_ChallengeGroup/PlayerIcon").gameObject.GetComponent<UISprite>();
        Spt_PlayerBG = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_ChallengeGroup/PlayerBG").gameObject.GetComponent<UISprite>();
        Lbl_PlayerName = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_ChallengeGroup/PlayerName").gameObject.GetComponent<UILabel>();
        Lbl_ChallengeState = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_ChallengeGroup/ChallengeState").gameObject.GetComponent<UILabel>();
        Gobj_BossDescGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_BossDescGroup").gameObject;
        Lbl_BossDesc = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_BossDescGroup/BossDesc").gameObject.GetComponent<UILabel>();
        Slider_BossBloodSlider = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_BossDescGroup/BossBloodSlider").gameObject.GetComponent<UISlider>();
        Lbl_BossBlood = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_BossDescGroup/BossBlood").gameObject.GetComponent<UILabel>();
        Lbl_SurplusCount = _uiRoot.transform.FindChild("gobj_StageInfoGroup/SurplusGroup/SurplusCount").gameObject.GetComponent<UILabel>();
        Btn_Purchase = _uiRoot.transform.FindChild("gobj_StageInfoGroup/Purchase").gameObject.GetComponent<UIButton>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLabel.text = "";
        Lbl_StageDesc.text = "";
        Lbl_SPLabel.text = "30";
        Lbl_GateCountLabel.text = "";
        Lbl_PlayerName.text = "";
        //Lbl_ChallengeState.text = "New Label";
        Lbl_BossDesc.text = "";
        Lbl_BossBlood.text = "80";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
