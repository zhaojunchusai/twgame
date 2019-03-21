using UnityEngine;
using System;
using System.Collections;

public class PlayerInfoView
{
    public static string UIName = "PlayerInfoView";
    public GameObject _uiRoot;
    public UIGrid Grd_EnemyGrid;
    public GameObject Gobj_EnemyInfoComp;
    public GameObject Gobj_PlayerInfoComp;
    public UILabel Lbl_NameTitle;
    public UILabel Lbl_NameLabel;
    public UILabel Lbl_CorpsTitle;
    public UILabel Lbl_CorpsLabel;
    public GameObject Gobj_Rank;
    public UILabel Lbl_RankTitle;
    public UILabel Lbl_RankLabel;
    public UILabel Lbl_DefenseTitle;
    public UILabel Lbl_DefenseLabel;
    public GameObject Gobj_SlaveHolder;
    public UISprite Spt_SlaverHolder;
    public UILabel Lbl_SlaverHolderTitle;
    public UILabel Lbl_SlaverHolder;
    public UISprite Spt_MaskSprite;
    //public UIButton Btn_CloseButton;
    public TweenScale Anim_TScale;
    public UISprite Spt_CenterDecorative;
    public UISprite Spt_DynamicBG;
    public GameObject Gobj_EnemyGroup;
    public GameObject Gobj_CorpsGroup;
    public UIButton Btn_Visit;
    public UIButton Btn_Slave;

    public GameObject Gobj_SlaveGroup;
    public UIButton Btn_CorpsHelp;
    public UISprite Spt_CorpsHelpBg;
    public UILabel Lbl_CorpsHelpTip;
    public UIButton Btn_WorldHelp;
    public UISprite Spt_WorldHelpBg;
    public UILabel Lbl_WorldHelpTip;
    public UIButton Btn_Revolt;

    public GameObject Gobj_AttackGroup;
    public UIButton Btn_Attack;
    public UILabel Lbl_BtnAttack;
    public UILabel Lbl_SPNum;
    public UILabel Lbl_ViewTitle;
    public UILabel Lbl_LineupTilte;
    public GameObject Gobj_QualifyingGroup;
    public UIButton Btn_QualifyingChallenge;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/PlayerInfoView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Grd_EnemyGrid = _uiRoot.transform.FindChild("Anim/EnemyGroup/EnemyGrid").gameObject.GetComponent<UIGrid>();
        Gobj_PlayerInfoComp = _uiRoot.transform.FindChild("Anim/Information/gobj_PlayerInfoComp").gameObject;
        Gobj_EnemyInfoComp = _uiRoot.transform.FindChild("Anim/EnemyGroup/EnemyGrid/gobj_EnemyInfoComp").gameObject;
        Lbl_ViewTitle = _uiRoot.transform.FindChild("Anim/BG/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = _uiRoot.transform.FindChild("Anim/Information/Name/Label").gameObject.GetComponent<UILabel>();
        Lbl_NameTitle = _uiRoot.transform.FindChild("Anim/Information/Name/Title").gameObject.GetComponent<UILabel>();
        Lbl_CorpsLabel = _uiRoot.transform.FindChild("Anim/Information/Union/Label").gameObject.GetComponent<UILabel>();
        Lbl_CorpsTitle = _uiRoot.transform.FindChild("Anim/Information/Union/Title").gameObject.GetComponent<UILabel>();
        Gobj_Rank = _uiRoot.transform.FindChild("Anim/Information/Rank").gameObject;
        Lbl_RankLabel = _uiRoot.transform.FindChild("Anim/Information/Rank/Label").gameObject.GetComponent<UILabel>();
        Lbl_RankTitle = _uiRoot.transform.FindChild("Anim/Information/Rank/Title").gameObject.GetComponent<UILabel>();
        Lbl_DefenseLabel = _uiRoot.transform.FindChild("Anim/Information/Defense/Label").gameObject.GetComponent<UILabel>();
        Lbl_DefenseTitle = _uiRoot.transform.FindChild("Anim/Information/Defense/Title").gameObject.GetComponent<UILabel>();
        Gobj_SlaveHolder = _uiRoot.transform.FindChild("Anim/Information/SlaveHolder").gameObject;
        Spt_SlaverHolder = _uiRoot.transform.FindChild("Anim/Information/SlaveHolder/Sprite").gameObject.GetComponent<UISprite>();
        Lbl_SlaverHolder = _uiRoot.transform.FindChild("Anim/Information/SlaveHolder/Label").gameObject.GetComponent<UILabel>();
        Lbl_SlaverHolderTitle = _uiRoot.transform.FindChild("Anim/Information/SlaveHolder/Title").gameObject.GetComponent<UILabel>();
        //Btn_CloseButton = _uiRoot.transform.FindChild("Anim/CloseButton").gameObject.GetComponent<UIButton>();
        Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Gobj_EnemyGroup = _uiRoot.transform.FindChild("Anim/EnemyGroup").gameObject;
        Spt_CenterDecorative = _uiRoot.transform.FindChild("Anim/CenterDecorative").gameObject.GetComponent<UISprite>();
        Spt_DynamicBG = _uiRoot.transform.FindChild("Anim/DynamicBG").gameObject.GetComponent<UISprite>();
        Gobj_CorpsGroup = _uiRoot.transform.FindChild("Anim/CorpsGroup").gameObject;
        Btn_Visit = _uiRoot.transform.FindChild("Anim/CorpsGroup/Visit").gameObject.GetComponent<UIButton>();
        Btn_Slave = _uiRoot.transform.FindChild("Anim/CorpsGroup/Slave").gameObject.GetComponent<UIButton>();
        Gobj_SlaveGroup = _uiRoot.transform.FindChild("Anim/SlaveGroup").gameObject;
        Btn_CorpsHelp = _uiRoot.transform.FindChild("Anim/SlaveGroup/CorpsHelp").gameObject.GetComponent<UIButton>();
        Spt_CorpsHelpBg = _uiRoot.transform.FindChild("Anim/SlaveGroup/CorpsHelp/Background").gameObject.GetComponent<UISprite>();
        Lbl_CorpsHelpTip = _uiRoot.transform.FindChild("Anim/SlaveGroup/CorpsHelp/Label").gameObject.GetComponent<UILabel>();
        Btn_WorldHelp = _uiRoot.transform.FindChild("Anim/SlaveGroup/WorldHelp").gameObject.GetComponent<UIButton>();
        Spt_WorldHelpBg = _uiRoot.transform.FindChild("Anim/SlaveGroup/WorldHelp/Background").gameObject.GetComponent<UISprite>();
        Lbl_WorldHelpTip = _uiRoot.transform.FindChild("Anim/SlaveGroup/WorldHelp/Label").gameObject.GetComponent<UILabel>();
        Btn_Revolt = _uiRoot.transform.FindChild("Anim/SlaveGroup/Revolt").gameObject.GetComponent<UIButton>();
        Gobj_AttackGroup = _uiRoot.transform.FindChild("Anim/AttackGroup").gameObject;
        Btn_Attack = _uiRoot.transform.FindChild("Anim/AttackGroup/Attack").gameObject.GetComponent<UIButton>();
        Lbl_BtnAttack = _uiRoot.transform.FindChild("Anim/AttackGroup/Attack/Label").gameObject.GetComponent<UILabel>();
        Lbl_SPNum = _uiRoot.transform.FindChild("Anim/AttackGroup/SPGroup/SPNum").gameObject.GetComponent<UILabel>();
        Lbl_LineupTilte = _uiRoot.transform.FindChild("Anim/EnemyGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Gobj_QualifyingGroup = _uiRoot.transform.FindChild("Anim/QualifyingGroup").gameObject;
        Btn_QualifyingChallenge = _uiRoot.transform.FindChild("Anim/QualifyingGroup/Challenge").gameObject.GetComponent<UIButton>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_NameLabel.text = string.Empty;
        Lbl_CorpsLabel.text = string.Empty;
        Lbl_RankLabel.text = string.Empty;
        Lbl_DefenseLabel.text = string.Empty;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
