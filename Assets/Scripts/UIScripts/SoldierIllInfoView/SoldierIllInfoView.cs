using UnityEngine;
using System;
using System.Collections;

public class SoldierIllInfoView
{
    public static string UIName ="SoldierIllInfoView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_SoldierIllInfoView;

    public UIButton Btn_Button_close;
    public UIButton Btn_Button_left;
    public UIButton Btn_Button_right;

    public UILabel Lbl_Label_SoldierPos;
    public UILabel Lbl_Label_SoldierStory;
    public UISprite Spt_player;
    public UILabel Lbl_Label_Fighting;
    public UILabel Lbl_name;
    public UILabel Lbl_Talent;
    public UIGrid Grd_Grid_BaseAtt;
    public UILabel Lbl_Label_attribute1;
    public UILabel Lbl_Label_attribute2;
    public UILabel Lbl_Label_attribute3;
    public UIGrid Grd_Grid_OtherAtt;
    public UILabel Lbl_Label_attribute4;
    public UILabel Lbl_Label_attribute5;
    public UILabel Lbl_Label_attribute6;
    public UIScrollView ScrView_EquipAndSkillScrollView;
    public UIGrid Grd_Grid_EquipAndSkillScrollView;
    public UIWrapContent UIWrapContent_Grid;
    public UITable DetailTabel;
    public GameObject item;
    public UIButton SuitEquipButton;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SoldierIllInfoView");
        UIPanel_SoldierIllInfoView = _uiRoot.GetComponent<UIPanel>();
        Btn_Button_close = _uiRoot.transform.FindChild("Anim/Button_close").gameObject.GetComponent<UIButton>();
        Btn_Button_left = _uiRoot.transform.FindChild("Anim/Button_Left").gameObject.GetComponent<UIButton>();
        Btn_Button_right = _uiRoot.transform.FindChild("Anim/Button_Right").gameObject.GetComponent<UIButton>();

        DetailTabel = _uiRoot.transform.FindChild("Anim/left/DetaillScrollView/Table").gameObject.GetComponent<UITable>();
        Lbl_Label_SoldierPos = _uiRoot.transform.FindChild("Anim/left/DetaillScrollView/Table/Label_SoldierPos").gameObject.GetComponent<UILabel>();
        Lbl_Label_SoldierStory = _uiRoot.transform.FindChild("Anim/left/DetaillScrollView/Table/Label_SoldierStory").gameObject.GetComponent<UILabel>();
        Spt_player = _uiRoot.transform.FindChild("Anim/center/Panel_player/player").gameObject.GetComponent<UISprite>();
        Lbl_Label_Fighting = _uiRoot.transform.FindChild("Anim/center/fighting/Label_Fighting").gameObject.GetComponent<UILabel>();
        Lbl_name = _uiRoot.transform.FindChild("Anim/center/name").gameObject.GetComponent<UILabel>();
        Lbl_Talent = _uiRoot.transform.FindChild("Anim/center/Talent").gameObject.GetComponent<UILabel>();
        Grd_Grid_BaseAtt = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid").gameObject.GetComponent<UIGrid>();
        Lbl_Label_attribute1 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_1/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute2 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_2/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute3 = _uiRoot.transform.FindChild("Anim/center/AttBack/BaseAtt/Grid/Label_3/Label_attribute").gameObject.GetComponent<UILabel>();
        Grd_Grid_OtherAtt = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid").gameObject.GetComponent<UIGrid>();
        Lbl_Label_attribute4 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_1/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute5 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_2/Label_attribute").gameObject.GetComponent<UILabel>();
        Lbl_Label_attribute6 = _uiRoot.transform.FindChild("Anim/center/AttBack/OtherAtt/Grid/Label_3/Label_attribute").gameObject.GetComponent<UILabel>();
        ScrView_EquipAndSkillScrollView = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid_EquipAndSkillScrollView = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        SuitEquipButton = _uiRoot.transform.FindChild("Anim/center/SuitEquipButton").gameObject.GetComponent<UIButton>();
        item = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/item").gameObject;
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Label_SoldierPos.text = "";
        Lbl_Label_SoldierStory.text = "";
        Lbl_Label_Fighting.text = "";
        Lbl_name.text = "";
        Lbl_Talent.text = "222";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }
}
