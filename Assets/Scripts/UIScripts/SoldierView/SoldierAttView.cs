using UnityEngine;
using System;
using System.Collections;

public class SoldierAttView
{
    public static string UIName = "SoldierAttView";
    public GameObject _uiRoot;
    public UIButton Btn_Button_close;

    public UIButton Btn_StrengthButton;
    public UIButton Btn_SelectButton;
    public UIButton Btn_FastStrengthButton;
    public UIButton Btn_FastEquipButton;
    public UILabel Lbl_FastEquipButton;
    public UIButton Btn_CloseButton;
    public UIBoundary Boundary = new UIBoundary();
    public GameObject SelectShow_Panel;
    public UISprite SelectShow_Sprite;
    public UIButton Btn_SelectShowButton;
    public GameObject Gobj_UpStarEffect;
    public GameObject Go_EffectMask;
    public GameObject FilterEffectPos_0;
    public GameObject FilterEffectPos_1;
    public GameObject FilterEffectPos_2;
    public GameObject FilterEffectPos_3;
    public GameObject FilterEffectPos_4;
    public GameObject FilterEffectPos_5;
    public GameObject Btn_SuitEquipButton;
    public UISprite Spt_SuitEquipButton;
    public GameObject ItemPoint1, ItemPoint2, ItemPoint3, ItemPoint4, ItemPoint5, ItemPoint6;
    public TweenScale Anim_TScale, EquipInfo_TScale, Intensify_TScale, Select_TScale, Skil_TScale, Artifact_TScale, SoldierUPstar_TScale, EquipUPstar_TScale;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/SoldierAttView");
        Go_EffectMask = _uiRoot.transform.FindChild("FilterEffect/Mask").gameObject;
        Go_EffectMask.SetActive(false);
        Btn_SuitEquipButton = _uiRoot.transform.FindChild("Anim/center/SuitEquipButton").gameObject;
        Spt_SuitEquipButton = _uiRoot.transform.FindChild("Anim/center/SuitEquipButton/Sprite").gameObject.GetComponent<UISprite>();
        FilterEffectPos_0 = _uiRoot.transform.FindChild("FilterEffect/EffectPos/0").gameObject;
        FilterEffectPos_1 = _uiRoot.transform.FindChild("FilterEffect/EffectPos/1").gameObject;
        FilterEffectPos_2 = _uiRoot.transform.FindChild("FilterEffect/EffectPos/2").gameObject;
        FilterEffectPos_3 = _uiRoot.transform.FindChild("FilterEffect/EffectPos/3").gameObject;
        FilterEffectPos_4 = _uiRoot.transform.FindChild("FilterEffect/EffectPos/4").gameObject;
        FilterEffectPos_5 = _uiRoot.transform.FindChild("FilterEffect/EffectPos/5").gameObject;
        ItemPoint1 = _uiRoot.transform.FindChild("Anim/right/ItemPoint/ItemPoint1").gameObject;
        ItemPoint2 = _uiRoot.transform.FindChild("Anim/right/ItemPoint/ItemPoint2").gameObject;
        ItemPoint3 = _uiRoot.transform.FindChild("Anim/right/ItemPoint/ItemPoint3").gameObject;
        ItemPoint4 = _uiRoot.transform.FindChild("Anim/right/ItemPoint/ItemPoint4").gameObject;
        ItemPoint5 = _uiRoot.transform.FindChild("Anim/right/ItemPoint/ItemPoint5").gameObject;
        ItemPoint6 = _uiRoot.transform.FindChild("Anim/right/ItemPoint/ItemPoint6").gameObject;
        Gobj_UpStarEffect = _uiRoot.transform.FindChild("Anim/back_center").gameObject;
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Select_TScale = _uiRoot.transform.FindChild("SoldierSelectPanel/Anim").gameObject.GetComponent<TweenScale>();
        Skil_TScale = _uiRoot.transform.FindChild("SkillIntensifyPanel/Anim").gameObject.GetComponent<TweenScale>();
        SoldierUPstar_TScale = _uiRoot.transform.FindChild("SoldierUpStarPanel/Anim").gameObject.GetComponent<TweenScale>();

        Btn_Button_close = _uiRoot.transform.FindChild("Anim/Button_close").gameObject.GetComponent<UIButton>();
        Btn_StrengthButton = _uiRoot.transform.FindChild("Anim/left/SoldierOb/StrengthButton").gameObject.GetComponent<UIButton>();
        Btn_SelectButton = _uiRoot.transform.FindChild("Anim/left/SoldierOb/SelectButton").gameObject.GetComponent<UIButton>();
        Btn_FastStrengthButton = _uiRoot.transform.FindChild("Anim/left/FastStrengthButton").gameObject.GetComponent<UIButton>();
        Btn_FastEquipButton = _uiRoot.transform.FindChild("Anim/left/FastEquipButton").gameObject.GetComponent<UIButton>();
        Lbl_FastEquipButton = _uiRoot.transform.FindChild("Anim/left/FastEquipButton/Label").gameObject.GetComponent<UILabel>();
        SelectShow_Sprite = _uiRoot.transform.FindChild("SelectShow/MarkSprite").gameObject.GetComponent<UISprite>();
        SelectShow_Panel = _uiRoot.transform.FindChild("SelectShow").gameObject;
        Btn_SelectShowButton = _uiRoot.transform.FindChild("SelectShow/close").gameObject.GetComponent<UIButton>();
        //Btn_SuitEquipButton.gameObject.SetActive(true);
        //SetLabelValues();
        //SetBoundary();
    }


    public void SetBoundary()
    {
        Boundary.left = -535f;
        Boundary.right = 535f;
        Boundary.up = 287f;
        Boundary.down = -294f;
    }

    public void SetLabelValues()
    {

    }

    public void Uninitialize()
    {

    }



    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
