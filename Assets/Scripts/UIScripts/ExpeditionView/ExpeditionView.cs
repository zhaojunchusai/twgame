using UnityEngine;
using System;
using System.Collections;

public class ExpeditionView
{
    public static string UIName = "ExpeditionView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskBG;
    public GameObject Gobj_Content;
    public UIPanel UIPanel_gobj_Content;
    public OffestSortingOrder OffestSortingOrder_gobj_Content;
    public UITexture Tex_CurrentBG;
    public UISprite Spt_CurrentItem;
    public Transform Trans_CurrentArrow;

    public UISprite Spt_CurrentFog;
    public TweenPosition TP_CurrentFog;
    public UISprite Spt_Sprite;

    public UISprite Spt_FrontMask;
    public UIButton Btn_Exchange;
    public UILabel Lbl_BtnExchangeLabel;
    public UISprite Spt_BtnExchangeBG;
    public UILabel Lbl_ChallengeNum;
    public UILabel Lbl_RestartNum;
    public UIButton Btn_Rule;
    public UILabel Lbl_BtnRuleLabel;
    public UISprite Spt_BtnRuleBG;
    public UIButton Btn_ReStart;
    public UILabel Lbl_BtnReStartLabel;
    public UISprite Spt_BtnReStartBG;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseBG;
    public UISprite Spt_ScrollBG;
    public UISprite Spt_BattlegroundItemNameBG;
    public UISprite Spt_Sprite1;
    public UISprite Spt_Sprite2;
    public UISprite Spt_BattlegroundItemNameBG1;
    public UILabel Lbl_BattlegroundItemName;
    public UISprite Spt_BoxLight;
    public GameObject Gobj_DescGroup;
    public UILabel Lbl_AwardDesc;

    public GameObject CountDouble;
    public GameObject RewardDouble;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ExpeditionView");
        CountDouble = _uiRoot.transform.FindChild("Anim/Double/Count").gameObject;
        RewardDouble = _uiRoot.transform.FindChild("Anim/Double/Reward").gameObject;
        Spt_MaskBG = _uiRoot.transform.FindChild("MaskBG").gameObject.GetComponent<UISprite>();
        Gobj_Content = _uiRoot.transform.FindChild("Anim/gobj_Content").gameObject;
        UIPanel_gobj_Content = _uiRoot.transform.FindChild("Anim/gobj_Content").gameObject.GetComponent<UIPanel>();
        OffestSortingOrder_gobj_Content = _uiRoot.transform.FindChild("Anim/gobj_Content").gameObject.GetComponent<OffestSortingOrder>();
        Tex_CurrentBG = _uiRoot.transform.FindChild("Anim/gobj_Content/gobj_Current/CurrentBG").gameObject.GetComponent<UITexture>();
        Spt_CurrentItem = _uiRoot.transform.FindChild("Anim/gobj_Content/gobj_Current/ItemContent/CurrentItem").gameObject.GetComponent<UISprite>();
        Trans_CurrentArrow = _uiRoot.transform.FindChild("Anim/gobj_Content/gobj_Current/CurrentArrow");
        Spt_BoxLight = _uiRoot.transform.FindChild("Anim/gobj_Content/gobj_Current/ItemContent/CurrentItem/BoxLight").gameObject.GetComponent<UISprite>();
        //Spt_CurrentFog = _uiRoot.transform.FindChild("Anim/gobj_BGGroup/CurrentFog").gameObject.GetComponent<UISprite>();
        //TP_CurrentFog = _uiRoot.transform.FindChild("Anim/gobj_BGGroup/CurrentFog").gameObject.GetComponent<TweenPosition>();
        Spt_CurrentFog = _uiRoot.transform.FindChild("Anim/gobj_Content/gobj_Current/CurrentFog").gameObject.GetComponent<UISprite>();
        TP_CurrentFog = _uiRoot.transform.FindChild("Anim/gobj_Content/gobj_Current/CurrentFog").gameObject.GetComponent<TweenPosition>();

        Spt_Sprite = _uiRoot.transform.FindChild("Anim/gobj_BGGroup/Sprite").gameObject.GetComponent<UISprite>();
        Spt_FrontMask = _uiRoot.transform.FindChild("Anim/FrontMask").gameObject.GetComponent<UISprite>();
        Btn_Exchange = _uiRoot.transform.FindChild("Anim/BottomGroup/Exchange").gameObject.GetComponent<UIButton>();
        Lbl_BtnExchangeLabel = _uiRoot.transform.FindChild("Anim/BottomGroup/Exchange/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnExchangeBG = _uiRoot.transform.FindChild("Anim/BottomGroup/Exchange/BG").gameObject.GetComponent<UISprite>();
        Lbl_ChallengeNum = _uiRoot.transform.FindChild("Anim/BottomGroup/ChallengeNum").gameObject.GetComponent<UILabel>();
        Lbl_RestartNum = _uiRoot.transform.FindChild("Anim/BottomGroup/RestartNum").gameObject.GetComponent<UILabel>();
        Btn_Rule = _uiRoot.transform.FindChild("Anim/BottomGroup/Rule").gameObject.GetComponent<UIButton>();
        Lbl_BtnRuleLabel = _uiRoot.transform.FindChild("Anim/BottomGroup/Rule/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnRuleBG = _uiRoot.transform.FindChild("Anim/BottomGroup/Rule/BG").gameObject.GetComponent<UISprite>();
        Btn_ReStart = _uiRoot.transform.FindChild("Anim/BottomGroup/ReStart").gameObject.GetComponent<UIButton>();
        Lbl_BtnReStartLabel = _uiRoot.transform.FindChild("Anim/BottomGroup/ReStart/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnReStartBG = _uiRoot.transform.FindChild("Anim/BottomGroup/ReStart/BG").gameObject.GetComponent<UISprite>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Close").gameObject.GetComponent<UIButton>();
        Spt_BtnCloseBG = _uiRoot.transform.FindChild("Anim/Close/BG").gameObject.GetComponent<UISprite>();
        Spt_ScrollBG = _uiRoot.transform.FindChild("Anim/ScrollBG").gameObject.GetComponent<UISprite>();
        Spt_BattlegroundItemNameBG = _uiRoot.transform.FindChild("Anim/ScrollBG/BattlegroundItemName/BattlegroundItemNameBG").gameObject.GetComponent<UISprite>();
        Spt_Sprite1 = _uiRoot.transform.FindChild("Anim/ScrollBG/BattlegroundItemName/Sprite1").gameObject.GetComponent<UISprite>();
        Spt_Sprite2 = _uiRoot.transform.FindChild("Anim/ScrollBG/BattlegroundItemName/Sprite2").gameObject.GetComponent<UISprite>();
        Spt_BattlegroundItemNameBG1 = _uiRoot.transform.FindChild("Anim/ScrollBG/BattlegroundItemName/BattlegroundItemNameBG1").gameObject.GetComponent<UISprite>();
        Lbl_BattlegroundItemName = _uiRoot.transform.FindChild("Anim/ScrollBG/BattlegroundItemName/BattlegroundItemName").gameObject.GetComponent<UILabel>();
        Gobj_DescGroup = _uiRoot.transform.FindChild("Anim/gobj_BGGroup/DescGroup").gameObject;
        Lbl_AwardDesc = _uiRoot.transform.FindChild("Anim/gobj_BGGroup/DescGroup/AwardDesc").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnExchangeLabel.text = "兌換獎勵";
        Lbl_ChallengeNum.text = "當前進度：1/1";
        Lbl_RestartNum.text = "今日剩餘1次";
        Lbl_BtnRuleLabel.text = "規則";
        Lbl_BtnReStartLabel.text = "重新開始";
        Lbl_BattlegroundItemName.text = "遠征天下";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
