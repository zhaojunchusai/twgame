using UnityEngine;
using System;
using System.Collections;

public class CastleView
{
    public static string UIName ="CastleView";
    public GameObject _uiRoot;
    public UIButton Btn_Back;
    public UIPanel UIPanel_Content;
    public UISprite Spt_Castle_Icon_BG;
    public UITexture Tex_Castle_Icon;
    public UILabel Lbl_Castle_Level_Value;
    public UISprite Spt_Castle_Level_MAX;
    public UILabel Lbl_Castle_HP_Value;
    public UILabel Lbl_Castle_HP_Next;
    public UILabel Lbl_Castle_Price_Value;
    public UIButton Btn_Castle_UpLV;
    public UISprite Spt_BtnCastle_UpLVUpLV_BG;
    public UISprite Spt_BtnCastle_UpLVUpLV_Title;
    public UILabel Lbl_BtnCastle_UpLVUpLV_Title;
    public UILabel Lbl_Level_Value;
    public UILabel Lbl_Attack_Value;
    public UILabel Lbl_Attack_Next;
    public UILabel Lbl_Distance_Value;
    public UILabel Lbl_Center_Description;
    public UILabel Lbl_Price_Value;
    public UIButton Btn_Right_UpLV;

    //---------------自定义物件//
    public Transform Obj_Shooter_Item;
    //---------------自定义物件//
    public TweenScale Scale_TScale;
    public GameObject Item_IntensifyEffect;
    public GameObject EffectPoint;
    public UIBoundary Boundary = new UIBoundary();
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/CastleView");
        EffectPoint = _uiRoot.transform.FindChild("Scale/Content/Castle/EffectPoint").gameObject;
        Scale_TScale=_uiRoot.transform .FindChild("Scale").gameObject .GetComponent <TweenScale>();
        Item_IntensifyEffect = _uiRoot.transform.FindChild("Scale/IntensifyLabelItem").gameObject;
        Btn_Back = _uiRoot.transform.FindChild("Scale/BackGround/Back").gameObject.GetComponent<UIButton>();
        UIPanel_Content = _uiRoot.transform.FindChild("Scale/Content").gameObject.GetComponent<UIPanel>();
        Spt_Castle_Icon_BG = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Icon_BG").gameObject.GetComponent<UISprite>();
        Tex_Castle_Icon = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Icon_BG/Castle_Icon").gameObject.GetComponent<UITexture>();
        Lbl_Castle_Level_Value = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Info/Castle_Level_BG/Castle_Level_Value").gameObject.GetComponent<UILabel>();
        Spt_Castle_Level_MAX = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Info/Castle_Level_BG/Castle_Level_MAX").gameObject.GetComponent<UISprite>();
        Lbl_Castle_HP_Value = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Info/Castle_HP/Castle_HP_Value").gameObject.GetComponent<UILabel>();
        Lbl_Castle_HP_Next = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Info/Castle_HP/Castle_HP_Next").gameObject.GetComponent<UILabel>();
        Lbl_Castle_Price_Value = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_Price/Castle_Price_Value").gameObject.GetComponent<UILabel>();
        Btn_Castle_UpLV = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_UpLV").gameObject.GetComponent<UIButton>();
        Spt_BtnCastle_UpLVUpLV_BG = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_UpLV/UpLV_BG").gameObject.GetComponent<UISprite>();
        Spt_BtnCastle_UpLVUpLV_Title = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_UpLV/UpLV_Title").gameObject.GetComponent<UISprite>();
        Lbl_BtnCastle_UpLVUpLV_Title = _uiRoot.transform.FindChild("Scale/Content/Castle/Castle_UpLV/UpLV_Title").gameObject.GetComponent<UILabel>();
        Lbl_Level_Value = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Left/Left_Level/Level_Value").gameObject.GetComponent<UILabel>();
        Lbl_Attack_Value = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Center/Center_Attack/Attack_Value").gameObject.GetComponent<UILabel>();
        Lbl_Attack_Next = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Center/Center_Attack/Attack_Next").gameObject.GetComponent<UILabel>();
        Lbl_Distance_Value = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Center/Center_Distance/Distance_Value").gameObject.GetComponent<UILabel>();
        Lbl_Center_Description = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Center/Center_Description").gameObject.GetComponent<UILabel>();
        Lbl_Price_Value = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Right/Right_Price/Price_Value").gameObject.GetComponent<UILabel>();
        Btn_Right_UpLV = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item/Item_Right/Right_UpLV").gameObject.GetComponent<UIButton>();

        //---------------自定义物件//
        Obj_Shooter_Item = _uiRoot.transform.FindChild("Scale/Content/Shooter/Shooter_Item").gameObject.GetComponent<Transform>();

        //---------------自定义物件//
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Castle_Level_Value.text = "";
        Lbl_Castle_HP_Value.text = "";
        Lbl_Castle_HP_Next.text = "";
        Lbl_Castle_Price_Value.text = "";
        Lbl_Level_Value.text = "";
        Lbl_Attack_Value.text = "";
        Lbl_Attack_Next.text = "";
        Lbl_Distance_Value.text = "";
        Lbl_Center_Description.text = "";
        Lbl_Price_Value.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
