using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class HeroAttributeView
{
    public WeaponsDepot weaponsDepot;
    public int ChooseSlot = -1;
    public static string UIName = "HeroAttributeView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_Achor;

    public UIButton Btn_Button_close;
    public UIButton Btn_Button_Castle;
    public UILabel Lbl_name;
    public UILabel Lbl_level;
    public UIPanel UIPanel_Panel_player;
    public UISlider Slider_ProgressBar;
    public UILabel Lbl_SliderProgressBarLabel;
    public UILabel Lbl_Label_attribute1;
    public UILabel Lbl_Label_attribute2;
    public UILabel Lbl_Label_attribute3;
    public UILabel Lbl_Label_attribute4;
    public UILabel Lbl_Label_attribute5;
    public UILabel Lbl_Label_attribute6;
    public UILabel Lbl_Label_attribute7;
    public UILabel Lbl_Label_attribute8;
    public UILabel Lbl_Label_attribute9;
    public UILabel Lbl_Label_attribute10;
    public UITexture Texture_Tower;

    public UIBoundary Boundary = new UIBoundary();

    public TweenScale Anim_TScale,Skill_TScale;

    public delegate void EquipTouchDelegate(int index);
    public EquipTouchDelegate TouchDelegate;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("HeroAttributeView");
        UIPanel_Achor = _uiRoot.GetComponent<UIPanel>();
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_Button_close = _uiRoot.transform.FindChild("Anim/Button_close").gameObject.GetComponent<UIButton>();
        Btn_Button_Castle = _uiRoot.transform.FindChild("Anim/left/Button_close").gameObject.GetComponent<UIButton>();
        Texture_Tower = _uiRoot.transform.FindChild("Anim/left/tower_icon").gameObject.GetComponent<UITexture>();
        Lbl_name = _uiRoot.transform.FindChild("Anim/center/name").gameObject.GetComponent<UILabel>();
        Lbl_level = _uiRoot.transform.FindChild("Anim/center/level").gameObject.GetComponent<UILabel>();
        UIPanel_Panel_player = _uiRoot.transform.FindChild("Anim/center/Panel_player").gameObject.GetComponent<UIPanel>();
        Slider_ProgressBar = _uiRoot.transform.FindChild("Anim/center/Progress Bar").gameObject.GetComponent<UISlider>();
        Lbl_SliderProgressBarLabel = _uiRoot.transform.FindChild("Anim/center/Progress Bar/Label").gameObject.GetComponent<UILabel>();
        Btn_Button_close.pressedSprite = "Cmn_Icon_Return01";
        SetLabelValues();
        SetBoundary();
    }
    public void SetBoundary()
    {
        Boundary.left = -512f;
        Boundary.right = 512f;
        Boundary.up = 287f;
        Boundary.down = -287f;
    }
    public void SetLabelValues()
    {
        var playerAtt = PlayerData.Instance;

        Lbl_name.text = playerAtt._NickName;
        Lbl_level.text = playerAtt._Level.ToString();
        string exp = String.Format("{0}/{1}", playerAtt._CurrentExp, playerAtt._NextLvExp);
        Lbl_SliderProgressBarLabel.text = exp;
    }
    public void Uninitialize()
    {

    }
    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
