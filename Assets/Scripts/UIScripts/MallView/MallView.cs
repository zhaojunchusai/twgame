using UnityEngine;
using System;
using System.Collections;

public class MallView
{
    public static string UIName ="MallView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_MallView;
    public UISprite Spt_MaskBGSprite;
    public UISprite Spt_TitleRighBG2;
    public UISprite Spt_TitleLeftBG;
    public UILabel Lbl_TitleLb;
    public UISprite Spt_RankBGSprite;
    public UISprite Spt_RankBG;
    public UISprite Spt_LightSprite;
    public UIPanel UIPanel_ContentScrollView;
    public UIScrollView ScrView_ContentScrollView;
    public UIGrid Grd_MallContent;
    public UIPanel UIPanel_RankTypeScrollView;
    public UITable UITable_UITable;
    public UIButton Btn_SkillBook;
    public UISprite Spt_BtnSkillBookButtonSprite;
    public UILabel Lbl_BtnSkillBookLabel;
    public UIButton Btn_WeaponMat;
    public UISprite Spt_BtnWeaponMatButtonSprite;
    public UILabel Lbl_BtnWeaponMatLabel;
    public GameObject Gobj_WeaponMatObj;
    public UIButton Btn_WeaponMatWhite;
    public UISprite Spt_BtnWeaponMatWhiteBG;
    public UILabel Lbl_BtnWeaponMatWhiteLabel;
    public UISprite Spt_SubMatSelected;
    public UIButton Btn_WeaponMatGreen;
    public UISprite Spt_BtnWeaponMatGreenBG;
    public UILabel Lbl_BtnWeaponMatGreenLabel;
    public UIButton Btn_WeaponMatBlue;
    public UISprite Spt_BtnWeaponMatBlueBG;
    public UILabel Lbl_BtnWeaponMatBlueLabel;
    public UIButton Btn_RingMat;
    public UISprite Spt_BtnRingMatButtonSprite;
    public UILabel Lbl_BtnRingMatLabel;
    public GameObject Gobj_RingMatObj;
    public UIButton Btn_RingMatWhite;
    public UISprite Spt_BtnRingMatWhiteBG;
    public UILabel Lbl_BtnRingMatWhiteLabel;
    public UIButton Btn_RingMatGreen;
    public UISprite Spt_BtnRingMatGreenBG;
    public UILabel Lbl_BtnRingMatGreenLabel;
    public UIButton Btn_RingMatBlue;
    public UISprite Spt_BtnRingMatBlueBG;
    public UILabel Lbl_BtnRingMatBlueLabel;
    public UIButton Btn_NecklaceMat;
    public UISprite Spt_BtnNecklaceMatButtonSprite;
    public UILabel Lbl_BtnNecklaceMatLabel;
    public UIButton Btn_OtherMat;
    public UISprite Spt_BtnOtherMatButtonSprite;
    public UILabel Lbl_BtnOtherMatLabel;
    public GameObject Gobj_StoreItem;

    public GameObject Gobj_NecklaceMatObj;
    public UIButton Btn_NecklaceMatWhite;
    public UISprite Spt_BtnNecklaceMatWhiteBG;
    public UILabel Lbl_BtnNecklaceMatWhiteLabel;
    public UIButton Btn_NecklaceMatGreen;
    public UISprite Spt_BtnNecklaceMatGreenBG;
    public UILabel Lbl_BtnNecklaceMatGreenLabel;
    public UIButton Btn_NecklaceMatBlue;
    public UISprite Spt_BtnNecklaceMatBlueBG;
    public UILabel Lbl_BtnNecklaceMatBlueLabel;

    public UIButton Btn_CommonMat;
    public UISprite Spt_BtnCommonMatButtonSprite;
    public UILabel Lbl_BtnCommonMatLabel;

    public UIButton Btn_EquipBag;
    public UISprite Spt_BtnEquipBagButtonSprite;
    public UILabel Lbl_BtnEquipBagLabel;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/MallView");
        UIPanel_MallView = _uiRoot.GetComponent<UIPanel>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Spt_TitleRighBG2 = _uiRoot.transform.FindChild("Anim/TitleObj/TitleRighBG2").gameObject.GetComponent<UISprite>();
        Spt_TitleLeftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLeftBG").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Spt_RankBGSprite = _uiRoot.transform.FindChild("Anim/RankBGSprite").gameObject.GetComponent<UISprite>();
        Spt_RankBG = _uiRoot.transform.FindChild("Anim/RankBG").gameObject.GetComponent<UISprite>();
        Spt_LightSprite = _uiRoot.transform.FindChild("Anim/LightSprite").gameObject.GetComponent<UISprite>();
        UIPanel_ContentScrollView = _uiRoot.transform.FindChild("Anim/ContentScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ContentScrollView = _uiRoot.transform.FindChild("Anim/ContentScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_MallContent = _uiRoot.transform.FindChild("Anim/ContentScrollView/MallContent").gameObject.GetComponent<UIGrid>();
        UIPanel_RankTypeScrollView = _uiRoot.transform.FindChild("Anim/RankTypeScrollView").gameObject.GetComponent<UIPanel>();
        UITable_UITable = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable").gameObject.GetComponent<UITable>();
        Btn_SkillBook = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/SkillBook").gameObject.GetComponent<UIButton>();
        Spt_BtnSkillBookButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/SkillBook/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnSkillBookLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/SkillBook/Label").gameObject.GetComponent<UILabel>();
        Btn_WeaponMat = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/WeaponMat").gameObject.GetComponent<UIButton>();
        Spt_BtnWeaponMatButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/WeaponMat/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnWeaponMatLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/WeaponMat/Label").gameObject.GetComponent<UILabel>();
        Gobj_WeaponMatObj = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj").gameObject;
        Btn_WeaponMatWhite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatWhite").gameObject.GetComponent<UIButton>();
        Spt_BtnWeaponMatWhiteBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatWhite/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnWeaponMatWhiteLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatWhite/Label").gameObject.GetComponent<UILabel>();
        Spt_SubMatSelected = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatWhite/SelectedSprite").gameObject.GetComponent<UISprite>();
        Btn_WeaponMatGreen = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatGreen").gameObject.GetComponent<UIButton>();
        Spt_BtnWeaponMatGreenBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatGreen/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnWeaponMatGreenLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatGreen/Label").gameObject.GetComponent<UILabel>();
        Btn_WeaponMatBlue = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatBlue").gameObject.GetComponent<UIButton>();
        Spt_BtnWeaponMatBlueBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatBlue/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnWeaponMatBlueLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_WeaponMatObj/Grid/WeaponMatBlue/Label").gameObject.GetComponent<UILabel>();
        Btn_RingMat = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/RingMat").gameObject.GetComponent<UIButton>();
        Spt_BtnRingMatButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/RingMat/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnRingMatLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/RingMat/Label").gameObject.GetComponent<UILabel>();
        Gobj_RingMatObj = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj").gameObject;
        Btn_RingMatWhite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatWhite").gameObject.GetComponent<UIButton>();
        Spt_BtnRingMatWhiteBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatWhite/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnRingMatWhiteLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatWhite/Label").gameObject.GetComponent<UILabel>();
        Btn_RingMatGreen = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatGreen").gameObject.GetComponent<UIButton>();
        Spt_BtnRingMatGreenBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatGreen/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnRingMatGreenLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatGreen/Label").gameObject.GetComponent<UILabel>();
        Btn_RingMatBlue = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatBlue").gameObject.GetComponent<UIButton>();
        Spt_BtnRingMatBlueBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatBlue/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnRingMatBlueLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_RingMatObj/Grid/RingMatBlue/Label").gameObject.GetComponent<UILabel>();
        Btn_NecklaceMat = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/NecklaceMat").gameObject.GetComponent<UIButton>();
        Spt_BtnNecklaceMatButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/NecklaceMat/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnNecklaceMatLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/NecklaceMat/Label").gameObject.GetComponent<UILabel>();
        Btn_OtherMat = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/OtherMat").gameObject.GetComponent<UIButton>();
        Spt_BtnOtherMatButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/OtherMat/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnOtherMatLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/OtherMat/Label").gameObject.GetComponent<UILabel>();
        Gobj_StoreItem = _uiRoot.transform.FindChild("Anim/Pre/gobj_StoreItem").gameObject;

        Gobj_NecklaceMatObj = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj").gameObject;
        Btn_NecklaceMatWhite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatWhite").gameObject.GetComponent<UIButton>();
        Spt_BtnNecklaceMatWhiteBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatWhite/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnNecklaceMatWhiteLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatWhite/Label").gameObject.GetComponent<UILabel>();
        Btn_NecklaceMatGreen = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatGreen").gameObject.GetComponent<UIButton>();
        Spt_BtnNecklaceMatGreenBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatGreen/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnNecklaceMatGreenLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatGreen/Label").gameObject.GetComponent<UILabel>();
        Btn_NecklaceMatBlue = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatBlue").gameObject.GetComponent<UIButton>();
        Spt_BtnNecklaceMatBlueBG = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatBlue/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnNecklaceMatBlueLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/gobj_NecklaceMatObj/Grid/NecklaceMatBlue/Label").gameObject.GetComponent<UILabel>();

        Btn_CommonMat = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/CommonMat").gameObject.GetComponent<UIButton>();
        Spt_BtnCommonMatButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/CommonMat/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnCommonMatLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/CommonMat/Label").gameObject.GetComponent<UILabel>();

        Btn_EquipBag = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/EquipBag").gameObject.GetComponent<UIButton>();
        Spt_BtnEquipBagButtonSprite = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/EquipBag/ButtonSprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnEquipBagLabel = _uiRoot.transform.FindChild("Anim/RankTypeScrollView/UITable/EquipBag/Label").gameObject.GetComponent<UILabel>();


        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text = "商城";
        Lbl_BtnSkillBookLabel.text = "技能书";
        Lbl_BtnWeaponMatLabel.text = "武器材料";
        Lbl_BtnWeaponMatWhiteLabel.text = "白色";
        Lbl_BtnWeaponMatGreenLabel.text = "绿色";
        Lbl_BtnWeaponMatBlueLabel.text = "蓝色";
        Lbl_BtnRingMatLabel.text = "戒指材料";
        Lbl_BtnRingMatWhiteLabel.text = "白色";
        Lbl_BtnRingMatGreenLabel.text = "绿色";
        Lbl_BtnRingMatBlueLabel.text = "蓝色";
        Lbl_BtnNecklaceMatLabel.text = "项链材料";
        Lbl_BtnOtherMatLabel.text = "其他材料";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
