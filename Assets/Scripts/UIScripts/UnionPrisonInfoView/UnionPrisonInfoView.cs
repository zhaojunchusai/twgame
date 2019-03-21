using UnityEngine;
using System;
using System.Collections;

public class UnionPrisonInfoView
{
    public static string UIName ="UnionPrisonInfoView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskBGSprite;
    public UISprite Spt_QualitySprite;
    public UISprite Spt_IconTexture;
    public UISprite Spt_back;
    public GameObject Obj_Level;
    public UILabel Lbl_Label_Level;
    public UISprite Spt_state;

    public UIGrid Lbl_Grid;

    public UILabel Lbl_Label_Name;
    public UILabel Lbl_Label_Member;
    public UILabel Lbl_Label_Leader;
    public UILabel Lbl_Label_State;
    public UILabel Lbl_Label_State_Title;
    public UILabel Lbl_Label_Buff;

    public GameObject Obj_Name;
    public GameObject Obj_Menber;
    public GameObject Obj_Leader;
    public GameObject Obj_State;
    public GameObject Obj_Buff;

    public UIButton Btn_RuleBtn;
    public UILabel Btn_RuleLbl;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionPrisonInfoView");
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = _uiRoot.transform.FindChild("Anim/ArtifactComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = _uiRoot.transform.FindChild("Anim/ArtifactComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_back = _uiRoot.transform.FindChild("Anim/ArtifactComp/back").gameObject.GetComponent<UISprite>();
        Obj_Level = _uiRoot.transform.FindChild("Anim/ArtifactComp/LevelBG").gameObject;
        Lbl_Label_Level = _uiRoot.transform.FindChild("Anim/ArtifactComp/LevelBG/Label").gameObject.GetComponent<UILabel>();
        Spt_state = _uiRoot.transform.FindChild("Anim/ArtifactComp/state").gameObject.GetComponent<UISprite>();

        Lbl_Label_Name = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Name/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Member = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Member/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Leader = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Leader/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_State_Title = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_State").gameObject.GetComponent<UILabel>();
        Lbl_Label_State = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_State/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_Buff = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Buff/Label").gameObject.GetComponent<UILabel>();

        Lbl_Grid = _uiRoot.transform.FindChild("Anim/LabelGroup").gameObject.GetComponent<UIGrid>();
        Obj_Name = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Name").gameObject;
        Obj_Menber = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Member").gameObject;
        Obj_Leader = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Leader").gameObject;
        Obj_State = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_State").gameObject;
        Obj_Buff = _uiRoot.transform.FindChild("Anim/LabelGroup/Label_Buff").gameObject;

        Btn_RuleBtn = _uiRoot.transform.FindChild("Anim/RuleBtn").gameObject.GetComponent<UIButton>();
        Btn_RuleLbl = _uiRoot.transform.FindChild("Anim/RuleBtn/Label").gameObject.GetComponent<UILabel>();
        SetLabelValues();
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
