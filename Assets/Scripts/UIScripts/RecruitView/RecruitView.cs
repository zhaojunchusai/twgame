using UnityEngine;
using System;
using System.Collections;

public class RecruitView
{
    public static string UIName ="RecruitView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_RecruitView;
    public UISprite Spt_BG;
    public UIButton Btn_BackBtn;
    public UISprite Spt_BtnBackBtnBG;
    public UISprite Spt_RecruitViewBG;
    public UISprite Spt_Title;
    public UISprite Spt_BottomDRB;
    public UISprite Spt_TopDRB;
    public GameObject Gobj_Brave;
    public UISprite Spt_gobj_Brave;
    public GameObject Gobj_BraveFree;
    public UISprite Spt_gobj_BraveFree;
    public UILabel Lbl_BraveFreeLb;
    public UISprite Spt_BraveFreeSp;
    public UILabel Lbl_BraveFreeTimeLb;
    public GameObject Gobj_PutDownRiot;
    public UISprite Spt_gobj_PutDownRiot;
    public GameObject Gobj_PDRFree;
    public UISprite Spt_gobj_PDRFree;
    public UILabel Lbl_PDRFreeLb;
    public UISprite Spt_PDRFreeSp;
    public GameObject Gobj_MatchlessPn;
    public UIPanel UIPanel_gobj_MatchlessPn;
    public GameObject Gobj_matchless;
    public BoxCollider Gobj_matchlessCollider;
    public GameObject Gobj_lizi;
    public UILabel Lbl_UnlockConditionsLb;
    public UIPanel UIPanel_ChooseCountPn;
    public UISprite Spt_ChooseCountBG2;
    public UISprite Spt_ChooseCountBG;
    public UIButton Btn_CloseBtn;
    public UISprite Spt_BtnCloseBtnBG;
    public UISprite Spt_TitleBG;
    public UISprite Spt_TitleDRBLeft;
    public UISprite Spt_TitleDRBRight;
    public UILabel  Spt_ChooseCountTitle;
    public UISprite Spt_ChooseOnce;
    public GameObject Gobj_OnceFree;
    public UISprite Spt_gobj_OnceFree;
    public UILabel Lbl_OnceFreeLb;
    public UISprite Spt_OnceFreeSP;
    public UISprite Spt_OnceTitle;
    public UISprite Spt_ChooseTen;
    public UISprite Spt_TenTitle;
    public GameObject Gobj_ChooseTenSp;
    public UISprite Spt_gobj_ChooseTenSp;
    public UILabel Lbl_ChooseTenDesc;
    public UISprite Spt_OnceCurrencyBG;
    public UISprite Spt_CurrencyIcon;
    public UILabel Lbl_CurrencyNum;
    public UISprite Spt_TenCurrencyBG;
    public UISprite Spt_TenCurrencyIcon;
    public UILabel Lbl_TenCurrencyNum;
    public UISprite Spt_Dis;
    public UISprite Spt_ChoosePnDRB;
    public TweenScale Anim_TScale, ChooseCountPn_TScale;
    public UIPanel TopFuncPanel;

    public UIPanel UIPanel_UnionPrison;
    public UISprite Spt_QualitySprite;
    public UISprite Spt_IconTexture;
    public UISprite Spt_LevelBG;
    public UILabel Lbl_Label_Level;
    public UISprite Spt_state;
    public UISprite Spt_Mask_UnionPrison;
    public UILabel Lbl_Dec;
    public UILabel Lbl_Num;

    public UISprite Spt_Mask;

    public UISprite Spt_MatchlessSp;
    
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/RecruitView");
       // UIPanel_RecruitView = _uiRoot.GetComponent<UIPanel>();
        TopFuncPanel = GameObject.Find("UISystem/UICamera/Anchor/Panel/TopFuncView").gameObject.GetComponent<UIPanel>();
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        ChooseCountPn_TScale = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim").gameObject.GetComponent<TweenScale>();
        //Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Btn_BackBtn = _uiRoot.transform.FindChild("Anim/BackButton/BackBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnBackBtnBG = _uiRoot.transform.FindChild("Anim/BackBtn/BG").gameObject.GetComponent<UISprite>();
        //Spt_RecruitViewBG = _uiRoot.transform.FindChild("Anim/RecruitViewBG").gameObject.GetComponent<UISprite>();
        //Spt_Title = _uiRoot.transform.FindChild("Anim/RecruitViewBG/Title").gameObject.GetComponent<UISprite>();
        //Spt_BottomDRB = _uiRoot.transform.FindChild("Anim/RecruitViewBG/BottomDRB").gameObject.GetComponent<UISprite>();
        //Spt_TopDRB = _uiRoot.transform.FindChild("Anim/RecruitViewBG/TopDRB").gameObject.GetComponent<UISprite>();
        Gobj_Brave = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave").gameObject;
        //Spt_gobj_Brave = _uiRoot.transform.FindChild("Anim/gobj_Brave").gameObject.GetComponent<UISprite>();
        Gobj_BraveFree = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Lab/gobj_BraveFree").gameObject;
        //Spt_gobj_BraveFree = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_BraveFree").gameObject.GetComponent<UISprite>();
        Lbl_BraveFreeLb = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Lab/gobj_BraveFree/BraveFreeLb").gameObject.GetComponent<UILabel>();
        Spt_BraveFreeSp = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Lab/BraveFreeSp/BraveFreeSp").gameObject.GetComponent<UISprite>();

        Spt_MatchlessSp = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/Lab/MatchlessSp").gameObject.GetComponent<UISprite>();
        
        Lbl_BraveFreeTimeLb = _uiRoot.transform.FindChild("Anim/gobj_Brave/gobj_Brave/Lab/gobj_BraveFree/BraveFreeTimeLb").gameObject.GetComponent<UILabel>();
        Gobj_PutDownRiot = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot").gameObject;
        //Spt_gobj_PutDownRiot = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot").gameObject.GetComponent<UISprite>();
        Gobj_PDRFree = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Lab/gobj_PDRFree").gameObject;
        //Spt_gobj_PDRFree = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PDRFree").gameObject.GetComponent<UISprite>();
        Lbl_PDRFreeLb = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Lab/gobj_PDRFree/PDRFreeLb").gameObject.GetComponent<UILabel>();
        Spt_PDRFreeSp = _uiRoot.transform.FindChild("Anim/gobj_PutDownRiot/gobj_PutDownRiot/Lab/PDRFreeSp/PDRFreeSp").gameObject.GetComponent<UISprite>();
        Gobj_MatchlessPn = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn").gameObject;
        //UIPanel_gobj_MatchlessPn = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn").gameObject.GetComponent<UIPanel>();
        Gobj_matchless = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless").gameObject;
        Gobj_lizi = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/gobj_lizi").gameObject;
        Gobj_matchlessCollider = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless").gameObject.GetComponent<BoxCollider>();
        Lbl_UnlockConditionsLb = _uiRoot.transform.FindChild("Anim/gobj_MatchlessPn/gobj_matchless/Lab/UnlockConditionsLb").gameObject.GetComponent<UILabel>();
        UIPanel_ChooseCountPn = _uiRoot.transform.FindChild("Anim/ChooseCountPn").gameObject.GetComponent<UIPanel>();
        //Spt_ChooseCountBG2 = _uiRoot.transform.FindChild("Anim/ChooseCountPn/ChooseCountBG2").gameObject.GetComponent<UISprite>();
        //Spt_ChooseCountBG = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseCountBG").gameObject.GetComponent<UISprite>();
        Btn_CloseBtn = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/CloseBtn").gameObject.GetComponent<UIButton>();
       // Spt_BtnCloseBtnBG = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/CloseBtn/BG").gameObject.GetComponent<UISprite>();
        //Spt_TitleBG = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TitleBG").gameObject.GetComponent<UISprite>();
        //Spt_TitleDRBLeft = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TitleBG/TitleDRBLeft").gameObject.GetComponent<UISprite>();
        //Spt_TitleDRBRight = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TitleBG/TitleDRBRight").gameObject.GetComponent<UISprite>();
        Spt_ChooseCountTitle = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TitleBG/ChooseCountTitle").gameObject.GetComponent<UILabel >();
        Spt_ChooseOnce = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseOnce").gameObject.GetComponent<UISprite>();
        Gobj_OnceFree = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseOnce/gobj_OnceFree").gameObject;
        //Spt_gobj_OnceFree = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseOnce/gobj_OnceFree").gameObject.GetComponent<UISprite>();
        Lbl_OnceFreeLb = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseOnce/gobj_OnceFree/OnceFreeLb").gameObject.GetComponent<UILabel>();
        Spt_OnceFreeSP = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseOnce/gobj_OnceFree/OnceFreeSP").gameObject.GetComponent<UISprite>();
        //Spt_OnceTitle = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseOnce/OnceTitle").gameObject.GetComponent<UISprite>();
        Spt_ChooseTen = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseTen").gameObject.GetComponent<UISprite>();
        //Spt_TenTitle = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseTen/TenTitle").gameObject.GetComponent<UISprite>();
        Gobj_ChooseTenSp = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseTen/gobj_ChooseTenSp").gameObject;
        //Spt_gobj_ChooseTenSp = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseTen/gobj_ChooseTenSp").gameObject.GetComponent<UISprite>();
        Lbl_ChooseTenDesc = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChooseTen/gobj_ChooseTenSp/ChooseTenDesc").gameObject.GetComponent<UILabel>();
        //Spt_OnceCurrencyBG = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/OnceCurrencyBG").gameObject.GetComponent<UISprite>();
        Spt_CurrencyIcon = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/OnceCurrencyBG/CurrencyIcon").gameObject.GetComponent<UISprite>();
        Lbl_CurrencyNum = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/OnceCurrencyBG/CurrencyNum").gameObject.GetComponent<UILabel>();
        //Spt_TenCurrencyBG = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TenCurrencyBG").gameObject.GetComponent<UISprite>();
        Spt_TenCurrencyIcon = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TenCurrencyBG/TenCurrencyIcon").gameObject.GetComponent<UISprite>();
        Lbl_TenCurrencyNum = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TenCurrencyBG/TenCurrencyNum").gameObject.GetComponent<UILabel>();
        Spt_Dis = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/TenCurrencyBG/Dis").gameObject.GetComponent<UISprite>();

        UIPanel_UnionPrison = _uiRoot.transform.FindChild("Anim/UnionPrison").gameObject.GetComponent<UIPanel>();
        Spt_QualitySprite = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_LevelBG = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/LevelBG").gameObject.GetComponent<UISprite>();
        Lbl_Label_Level = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
        Spt_state = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/state").gameObject.GetComponent<UISprite>();
        Spt_Mask_UnionPrison = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/Mask").gameObject.GetComponent<UISprite>();
        Lbl_Dec = _uiRoot.transform.FindChild("Anim/UnionPrison/ArtifactComp/Dec").gameObject.GetComponent<UILabel>();
        Lbl_Num = _uiRoot.transform.FindChild("Anim/UnionPrison/Num").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();


        //Spt_ChoosePnDRB = _uiRoot.transform.FindChild("Anim/ChooseCountPn/Anim/ChoosePnDRB").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BraveFreeLb.text = "";
        Lbl_BraveFreeTimeLb.text = "";
        Lbl_PDRFreeLb.text = "";
        Lbl_UnlockConditionsLb.text = "";
        Lbl_OnceFreeLb.text = "";
        Lbl_ChooseTenDesc.text = "十次招募必出至少一个蓝色品质武将";
        Lbl_CurrencyNum.text = "";
        Lbl_TenCurrencyNum.text = "";
    }

    public void Uninitialize()
    {

    }
   

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
