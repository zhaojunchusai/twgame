using UnityEngine;
using System;
using System.Collections;

public class RecruitResultView
{
    public static string UIName ="RecruitResultView";
    #region Old
    public GameObject _uiRoot;
    public UIPanel UIPanel_RecruitResultView;
    //public UISprite Spt_BG;
    public UIButton Btn_BackBtn;
    //public UISprite Spt_BtnBackBtnBG;
    ////public UILabel Lbl_BtnBackBtnLb;
    public UIButton Btn_SingleBtn;
    //public UISprite Spt_BtnSingleBtnBG;
    //public UISprite Spt_BtnSingleBtnLb;
    public UIButton Btn_MultipleBtn;
    //public UISprite Spt_BtnMultipleBtnBG;
    //public UISprite Spt_BtnMultipleBtnLb;
    public UISprite Spt_BtnMultipleBtnDis;
    public UISprite Spt_SingleCurrencyIcon;
    //public UISprite Spt_SingleCurrencyBG;
    public UILabel Lbl_SingleCurrencyNum;
    public UISprite Spt_MultipleCurrencyIcon;
    //public UISprite Spt_MultipleCurrencyBG;
    public UILabel Lbl_MultipleCurrencyNum;
    //public GameObject Gobj_Single;
    //public GameObject Gobj_Multiple;
    //public UIGrid Grd_gobj_Multiple;

    //public GameObject OnesEffect;
    //public TweenAlpha Talpha_OnesSpt1;
    //public TweenScale Tscale_OnesSpt1;
    ////public TweenAlpha Trotat_OnesSpt1;
    //public TweenScale Tscale_Oneslizi;
    //public TweenRotation Trotate_Oneslizi;
    //public TweenAlpha Talpha_OnesSpt2;
    //public TweenScale  TScale_OnesSpt2;
    //public GameObject OnesOne;
    //public GameObject OnesOne_R;
    //public GameObject OnesOne_Z;
    //public GameObject OnesOne_Y;
    //public TweenScale SingleItem;
    //public TweenScale SingleGobj;

    //public GameObject TensEffect1;
    ////public TweenAlpha Talpha_TensSpt1;
    ////public TweenScale Tscale_TensSpt1;
    //public TweenScale Tscale_TensLizi;
    //public TweenRotation Trotat_TensLizi;
    ////public TweenAlpha Talpha_TensSpt2;
    ////public GameObject TensOne;
    ////public TweenScale Tscale_TensOne1;
 
    //public GameObject Gobj_MultiName;
    //public GameObject Go_Name0;
    //public GameObject Go_Name1;
    //public GameObject Go_Name2;
    //public GameObject Go_Name3;
    //public GameObject Go_Name4;
    //public GameObject Go_Name5;
    //public GameObject Go_Name6;
    //public GameObject Go_Name7;
    //public GameObject Go_Name8;
    //public GameObject Go_Name9;

    //public RecruitResultItem RecruitResultItem0;
    //public RecruitResultItem RecruitResultItem1;
    //public RecruitResultItem RecruitResultItem2;
    //public RecruitResultItem RecruitResultItem3;
    //public RecruitResultItem RecruitResultItem4;
    //public RecruitResultItem RecruitResultItem5;
    //public RecruitResultItem RecruitResultItem6;
    //public RecruitResultItem RecruitResultItem7;
    //public RecruitResultItem RecruitResultItem8;
    //public RecruitResultItem RecruitResultItem9;

    //public GameObject Gobj_SingleName;
    //public UISprite Spt_SingleLight;
    //public UILabel Lbl_SingleName;
    //public UILabel Lbl_SingleNum;
    //public UISprite Spt_SingleType;
    //public UISprite Spt_SingleQuality;
    //public UISprite Spt_SingleChip;
    //public GameObject Gobj_SingleEffRed;
    //public GameObject Gobj_SingleEffYellow;
    //public GameObject Gobj_SingleEffPurple;

    ////十招
    //public  GameObject Go_Effect;
    //public GameObject TensEffect;
    //public TweenScale Ts_TenLizi;
    //public TweenRotation Tr_TenLizi;
    //public GameObject TensMask;
    //public TweenScale Ts_Name0;
    //public TweenScale Ts_Name1;
    //public TweenScale Ts_Name2;
    //public TweenScale Ts_Name3;
    //public TweenScale Ts_Name4;
    //public TweenScale Ts_Name5;
    //public TweenScale Ts_Name6;
    //public TweenScale Ts_Name7;
    //public TweenScale Ts_Name8;
    //public TweenScale Ts_Name9;
    //public UISprite Spt_OnesSprite2;
    //public GameObject EffectGo;
    #endregion
    public UIButton TenNameAnim_MaskSPT;
    public GameObject TenNameAnim_Objet;
    public GameObject TenNameAnim_Name0;
    public GameObject TenNameAnim_Name1;
    public GameObject TenNameAnim_Name2;
    public GameObject TenNameAnim_Name3;
    public GameObject TenNameAnim_Name4;
    public GameObject TenNameAnim_Name5;
    public GameObject TenNameAnim_Name6;
    public GameObject TenNameAnim_Name7;
    public GameObject TenNameAnim_Name8;
    public GameObject TenNameAnim_Name9;
    public RecruitResultItem TenNameAnim_Name0_Item;
    public RecruitResultItem TenNameAnim_Name1_Item;
    public RecruitResultItem TenNameAnim_Name2_Item;
    public RecruitResultItem TenNameAnim_Name3_Item;
    public RecruitResultItem TenNameAnim_Name4_Item;
    public RecruitResultItem TenNameAnim_Name5_Item;
    public RecruitResultItem TenNameAnim_Name6_Item;
    public RecruitResultItem TenNameAnim_Name7_Item;
    public RecruitResultItem TenNameAnim_Name8_Item;
    public RecruitResultItem TenNameAnim_Name9_Item;


    public GameObject TenNameResult_Objet;
    public GameObject TenNameResult_Name0;
    public GameObject TenNameResult_Name1;
    public GameObject TenNameResult_Name2;
    public GameObject TenNameResult_Name3;
    public GameObject TenNameResult_Name4;
    public GameObject TenNameResult_Name5;
    public GameObject TenNameResult_Name6;
    public GameObject TenNameResult_Name7;
    public GameObject TenNameResult_Name8;
    public GameObject TenNameResult_Name9;
    public RecruitResultItem TenNameResult_Name0_Item;
    public RecruitResultItem TenNameResult_Name1_Item;
    public RecruitResultItem TenNameResult_Name2_Item;
    public RecruitResultItem TenNameResult_Name3_Item;
    public RecruitResultItem TenNameResult_Name4_Item;
    public RecruitResultItem TenNameResult_Name5_Item;
    public RecruitResultItem TenNameResult_Name6_Item;
    public RecruitResultItem TenNameResult_Name7_Item;
    public RecruitResultItem TenNameResult_Name8_Item;
    public RecruitResultItem TenNameResult_Name9_Item;

    public GameObject OnceAnim;
    public GameObject OnceResult;
    public GameObject OnceAnimSingle;
    public GameObject OnceResultSingle;
    public UIButton OnceAnimMask;


    public UISprite AnimChip;
    public UISprite AnimQuality;
    public UILabel AnimLabel;
    public UILabel AnimNum;
    public UISprite ResultChip;
    public UISprite ResultQuality;
    public UILabel ResultLabel;
    public UILabel ResultNum;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/RecruitResultView");
        #region Old
        AnimChip = _uiRoot.transform.FindChild("OnceAnim/ones/Sprite/Item/Chip").gameObject.GetComponent<UISprite>();
        AnimQuality = _uiRoot.transform.FindChild("OnceAnim/ones/Sprite/Item/Quality").gameObject.GetComponent<UISprite>();
        AnimLabel = _uiRoot.transform.FindChild("OnceAnim/ones/Sprite/Item/Name").gameObject.GetComponent<UILabel>();
        AnimNum = _uiRoot.transform.FindChild("OnceAnim/ones/Sprite/Item/Num").gameObject.GetComponent<UILabel>();

        ResultLabel = _uiRoot.transform.FindChild("OnceResult/ones/Sprite/Item/Name").gameObject.GetComponent<UILabel>();
        ResultNum = _uiRoot.transform.FindChild("OnceResult/ones/Sprite/Item/Num").gameObject.GetComponent<UILabel>();
        ResultChip = _uiRoot.transform.FindChild("OnceResult/ones/Sprite/Item/Chip").gameObject.GetComponent<UISprite>();
        ResultQuality = _uiRoot.transform.FindChild("OnceResult/ones/Sprite/Item/Quality").gameObject.GetComponent<UISprite>();


        OnceAnimMask = _uiRoot.transform.FindChild("OnceAnim/Mask/Sprite").gameObject.GetComponent<UIButton>();
        OnceAnim = _uiRoot.transform.FindChild("OnceAnim").gameObject;
        OnceResult = _uiRoot.transform.FindChild("OnceResult").gameObject;
        UIPanel_RecruitResultView = _uiRoot.GetComponent<UIPanel>();
       // ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_TRAILBLUE, (GameObject gb) => { Go_Effect = gb; });
       // TensEffect = _uiRoot.transform.FindChild("Name/TensEffect").gameObject;
       // Ts_TenLizi = TensEffect.transform.FindChild("lizi").gameObject.GetComponent<TweenScale>();
       // Tr_TenLizi = TensEffect.transform.FindChild("lizi").gameObject.GetComponent<TweenRotation>();
       // TensMask = _uiRoot.transform.FindChild("Name/Mask").gameObject;
       // Spt_OnesSprite2 = _uiRoot.transform.FindChild("Effect/Ones/Sprite2").gameObject.GetComponent<UISprite>();
       // SingleItem = _uiRoot.transform.FindChild("SingleName/Item").gameObject.GetComponent<TweenScale>();
       // SingleGobj = _uiRoot.transform.FindChild("gobj_Single").gameObject.GetComponent<TweenScale>();
       // Go_Name0 = _uiRoot.transform.FindChild("Name/Name0").gameObject;
       // Go_Name1 = _uiRoot.transform.FindChild("Name/Name1").gameObject;
       // Go_Name2 = _uiRoot.transform.FindChild("Name/Name2").gameObject;
       // Go_Name3 = _uiRoot.transform.FindChild("Name/Name3").gameObject;
       // Go_Name4 = _uiRoot.transform.FindChild("Name/Name4").gameObject;
       // Go_Name5 = _uiRoot.transform.FindChild("Name/Name5").gameObject;
       // Go_Name6 = _uiRoot.transform.FindChild("Name/Name6").gameObject;
       // Go_Name7 = _uiRoot.transform.FindChild("Name/Name7").gameObject;
       // Go_Name8 = _uiRoot.transform.FindChild("Name/Name8").gameObject;
       // Go_Name9 = _uiRoot.transform.FindChild("Name/Name9").gameObject;
       // Ts_Name0 = Go_Name0.GetComponent<TweenScale>();
       // Ts_Name1 = Go_Name1.GetComponent<TweenScale>();
       // Ts_Name2 = Go_Name2.GetComponent<TweenScale>();
       // Ts_Name3 = Go_Name3.GetComponent<TweenScale>();
       // Ts_Name4 = Go_Name4.GetComponent<TweenScale>();
       // Ts_Name5 = Go_Name5.GetComponent<TweenScale>();
       // Ts_Name6 = Go_Name6.GetComponent<TweenScale>();
       // Ts_Name7 = Go_Name7.GetComponent<TweenScale>();
       // Ts_Name8 = Go_Name8.GetComponent<TweenScale>();
       // Ts_Name9 = Go_Name9.GetComponent<TweenScale>();
       // RecruitResultItem0 = GetItemComponent(Go_Name0);
       // RecruitResultItem1 = GetItemComponent(Go_Name1);
       // RecruitResultItem2 = GetItemComponent(Go_Name2);
       // RecruitResultItem3 = GetItemComponent(Go_Name3);
       // RecruitResultItem4 = GetItemComponent(Go_Name4);
       // RecruitResultItem5 = GetItemComponent(Go_Name5);
       // RecruitResultItem6 = GetItemComponent(Go_Name6);
       // RecruitResultItem7 = GetItemComponent(Go_Name7);
       // RecruitResultItem8 = GetItemComponent(Go_Name8);
       // RecruitResultItem9 = GetItemComponent(Go_Name9);
       // EffectGo = _uiRoot.transform.FindChild("Effect").gameObject;
       // TensEffect1 = _uiRoot.transform.FindChild("Effect/Tens").gameObject;
       // //Talpha_TensSpt1 = _uiRoot.transform.FindChild("Effect/Tens/Sprite1").gameObject.GetComponent <TweenAlpha >();
       // //Tscale_TensSpt1 = _uiRoot.transform.FindChild("Effect/Tens/Sprite1").gameObject.GetComponent <TweenScale>();
       // Trotat_TensLizi = _uiRoot.transform.FindChild("Effect/Tens/lizi").gameObject.GetComponent <TweenRotation >();
       // Tscale_TensLizi = _uiRoot.transform.FindChild("Effect/Tens/lizi").gameObject.GetComponent <TweenScale >();
       // //Talpha_TensSpt2 = _uiRoot.transform.FindChild("Effect/Tens/Sprite2").gameObject.GetComponent <TweenAlpha >();
       // //TensOne = _uiRoot.transform.FindChild("Effect/Tens/One").gameObject;
       // //Tscale_TensOne1 = _uiRoot.transform.FindChild("Effect/Tens/One/1").gameObject.GetComponent <TweenScale >();

       // OnesEffect = _uiRoot.transform.FindChild("Effect/Ones").gameObject;
       // Talpha_OnesSpt1 = _uiRoot.transform.FindChild("Effect/Ones/Sprite1").gameObject.GetComponent<TweenAlpha>();
       // //Trotat_OnesSpt1 = _uiRoot.transform.FindChild("Effect/Ones/Sprite1").gameObject.GetComponent<TweenAlpha >();
       // Tscale_OnesSpt1 = _uiRoot.transform.FindChild("Effect/Ones/Sprite1").gameObject.GetComponent<TweenScale>();
       // Trotate_Oneslizi = _uiRoot.transform.FindChild("Effect/Ones/lizi").gameObject.GetComponent<TweenRotation>();
       // Tscale_Oneslizi = _uiRoot.transform.FindChild("Effect/Ones/lizi").gameObject.GetComponent<TweenScale>();
       // Talpha_OnesSpt2 = _uiRoot.transform.FindChild("Effect/Ones/Sprite2").gameObject.GetComponent<TweenAlpha>();
       // TScale_OnesSpt2 = _uiRoot.transform.FindChild("Effect/Ones/Sprite2").gameObject.GetComponent<TweenScale >();

       // OnesOne = _uiRoot.transform.FindChild("Effect/Ones/One").gameObject;
       // OnesOne_R = _uiRoot.transform.FindChild("Effect/Ones/One/R").gameObject;
       // OnesOne_Y = _uiRoot.transform.FindChild("Effect/Ones/One/Y").gameObject;
       // OnesOne_Z = _uiRoot.transform.FindChild("Effect/Ones/One/Z").gameObject;
       // //Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
       Btn_BackBtn = _uiRoot.transform.FindChild("Btn/BackBtn").gameObject.GetComponent<UIButton>();
       // //Spt_BtnBackBtnBG = _uiRoot.transform.FindChild("BackBtn/BG").gameObject.GetComponent<UISprite>();
       // //Lbl_BtnBackBtnLb = _uiRoot.transform.FindChild("BackBtn/Lb").gameObject.GetComponent<UILabel>();
       Btn_SingleBtn = _uiRoot.transform.FindChild("Btn/SingleBtn").gameObject.GetComponent<UIButton>();
       //// Spt_BtnSingleBtnBG = _uiRoot.transform.FindChild("SingleBtn/BG").gameObject.GetComponent<UISprite>();
       // //Spt_BtnSingleBtnLb = _uiRoot.transform.FindChild("SingleBtn/Lb").gameObject.GetComponent<UISprite>();
       Btn_MultipleBtn = _uiRoot.transform.FindChild("Btn/MultipleBtn").gameObject.GetComponent<UIButton>();
       // //Spt_BtnMultipleBtnBG = _uiRoot.transform.FindChild("MultipleBtn/BG").gameObject.GetComponent<UISprite>();
       // //Spt_BtnMultipleBtnLb = _uiRoot.transform.FindChild("MultipleBtn/Lb").gameObject.GetComponent<UISprite>();
       Spt_BtnMultipleBtnDis = _uiRoot.transform.FindChild("Btn/MultipleBtn/Dis").gameObject.GetComponent<UISprite>();
       Spt_SingleCurrencyIcon = _uiRoot.transform.FindChild("SingleCurrencyIcon").gameObject.GetComponent<UISprite>();
       // //Spt_SingleCurrencyBG = _uiRoot.transform.FindChild("SingleCurrencyIcon/SingleCurrencyBG").gameObject.GetComponent<UISprite>();
       Lbl_SingleCurrencyNum = _uiRoot.transform.FindChild("SingleCurrencyIcon/SingleCurrencyNum").gameObject.GetComponent<UILabel>();
       Spt_MultipleCurrencyIcon = _uiRoot.transform.FindChild("MultipleCurrencyIcon").gameObject.GetComponent<UISprite>();
       // //Spt_MultipleCurrencyBG = _uiRoot.transform.FindChild("MultipleCurrencyIcon/MultipleCurrencyBG").gameObject.GetComponent<UISprite>();
       Lbl_MultipleCurrencyNum = _uiRoot.transform.FindChild("MultipleCurrencyIcon/MultipleCurrencyNum").gameObject.GetComponent<UILabel>();
       // Gobj_Single = _uiRoot.transform.FindChild("gobj_Single").gameObject;
       // Gobj_Multiple = _uiRoot.transform.FindChild("gobj_Multiple").gameObject;
       // Grd_gobj_Multiple = _uiRoot.transform.FindChild("gobj_Multiple").gameObject.GetComponent<UIGrid>();

       // Gobj_SingleName = _uiRoot.transform.FindChild("SingleName").gameObject;
       // Gobj_MultiName = _uiRoot.transform.FindChild("Name").gameObject;
       // Spt_SingleLight = _uiRoot.transform.FindChild("SingleName/Item/Light").gameObject.GetComponent<UISprite>();
       // Lbl_SingleName = _uiRoot.transform.FindChild("SingleName/Item/Name").gameObject.GetComponent<UILabel>();
       // Lbl_SingleNum = _uiRoot.transform.FindChild("SingleName/Item/Num").gameObject.GetComponent<UILabel>();
       // Spt_SingleType = _uiRoot.transform.FindChild("SingleName/Item/Type").gameObject.GetComponent<UISprite>();
       // Spt_SingleQuality = _uiRoot.transform.FindChild("SingleName/Item/Quality").gameObject.GetComponent<UISprite>();
       // Spt_SingleChip = _uiRoot.transform.FindChild("SingleName/Item/Chip").gameObject.GetComponent<UISprite>();
       // Gobj_SingleEffRed = _uiRoot.transform.FindChild("SingleName/Lizi_R").gameObject;
       // Gobj_SingleEffYellow = _uiRoot.transform.FindChild("SingleName/Lizi_Y").gameObject;
       // Gobj_SingleEffPurple = _uiRoot.transform.FindChild("SingleName/Lizi_Z").gameObject;

        //SetLabelValues();
        #endregion

       TenNameAnim_Objet = _uiRoot.transform.FindChild("TenNameAnim").gameObject;
       TenNameAnim_MaskSPT = TenNameAnim_Objet.transform.FindChild("Mask/Sprite").gameObject.GetComponent<UIButton>();
       TenNameAnim_Name0 = TenNameAnim_Objet.transform.FindChild("Name0").gameObject;
       TenNameAnim_Name1 = TenNameAnim_Objet.transform.FindChild("Name1").gameObject;
       TenNameAnim_Name2 = TenNameAnim_Objet.transform.FindChild("Name2").gameObject;
       TenNameAnim_Name3 = TenNameAnim_Objet.transform.FindChild("Name3").gameObject;
       TenNameAnim_Name4 = TenNameAnim_Objet.transform.FindChild("Name4").gameObject;
       TenNameAnim_Name5 = TenNameAnim_Objet.transform.FindChild("Name5").gameObject;
       TenNameAnim_Name6 = TenNameAnim_Objet.transform.FindChild("Name6").gameObject;
       TenNameAnim_Name7 = TenNameAnim_Objet.transform.FindChild("Name7").gameObject;
       TenNameAnim_Name8 = TenNameAnim_Objet.transform.FindChild("Name8").gameObject;
       TenNameAnim_Name9 = TenNameAnim_Objet.transform.FindChild("Name9").gameObject;
       TenNameAnim_Name0_Item = GetItemComponent(TenNameAnim_Name0);
       TenNameAnim_Name1_Item = GetItemComponent(TenNameAnim_Name1);
       TenNameAnim_Name2_Item = GetItemComponent(TenNameAnim_Name2);
       TenNameAnim_Name3_Item = GetItemComponent(TenNameAnim_Name3);
       TenNameAnim_Name4_Item = GetItemComponent(TenNameAnim_Name4);
       TenNameAnim_Name5_Item = GetItemComponent(TenNameAnim_Name5);
       TenNameAnim_Name6_Item = GetItemComponent(TenNameAnim_Name6);
       TenNameAnim_Name7_Item = GetItemComponent(TenNameAnim_Name7);
       TenNameAnim_Name8_Item = GetItemComponent(TenNameAnim_Name8);
       TenNameAnim_Name9_Item = GetItemComponent(TenNameAnim_Name9);


       TenNameResult_Objet = _uiRoot.transform.FindChild("TenNameResult").gameObject;
       TenNameResult_Name0 = TenNameResult_Objet.transform.FindChild("Name0").gameObject;
       TenNameResult_Name1 = TenNameResult_Objet.transform.FindChild("Name1").gameObject;
       TenNameResult_Name2 = TenNameResult_Objet.transform.FindChild("Name2").gameObject;
       TenNameResult_Name3 = TenNameResult_Objet.transform.FindChild("Name3").gameObject;
       TenNameResult_Name4 = TenNameResult_Objet.transform.FindChild("Name4").gameObject;
       TenNameResult_Name5 = TenNameResult_Objet.transform.FindChild("Name5").gameObject;
       TenNameResult_Name6 = TenNameResult_Objet.transform.FindChild("Name6").gameObject;
       TenNameResult_Name7 = TenNameResult_Objet.transform.FindChild("Name7").gameObject;
       TenNameResult_Name8 = TenNameResult_Objet.transform.FindChild("Name8").gameObject;
       TenNameResult_Name9 = TenNameResult_Objet.transform.FindChild("Name9").gameObject;
       TenNameResult_Name0_Item = GetItemComponent(TenNameResult_Name0);
       TenNameResult_Name1_Item = GetItemComponent(TenNameResult_Name1);
       TenNameResult_Name2_Item = GetItemComponent(TenNameResult_Name2);
       TenNameResult_Name3_Item = GetItemComponent(TenNameResult_Name3);
       TenNameResult_Name4_Item = GetItemComponent(TenNameResult_Name4);
       TenNameResult_Name5_Item = GetItemComponent(TenNameResult_Name5);
       TenNameResult_Name6_Item = GetItemComponent(TenNameResult_Name6);
       TenNameResult_Name7_Item = GetItemComponent(TenNameResult_Name7);
       TenNameResult_Name8_Item = GetItemComponent(TenNameResult_Name8);
       TenNameResult_Name9_Item = GetItemComponent(TenNameResult_Name9);

       OnceAnimSingle = _uiRoot.transform.FindChild("OnceAnim/Single").gameObject;
       OnceResultSingle = _uiRoot.transform.FindChild("OnceResult/Single").gameObject;

    }

    private RecruitResultItem GetItemComponent(GameObject go)
    {
        if (go.GetComponent<RecruitResultItem>() == null)
            return go.AddComponent<RecruitResultItem>();
        return go.GetComponent<RecruitResultItem>();
    }

    public void SetLabelValues()
    {
        //Lbl_BtnBackBtnLb.text = "";
        //Lbl_SingleCurrencyNum.text = "";
        //Lbl_MultipleCurrencyNum.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
