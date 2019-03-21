using UnityEngine;
using System;
using System.Collections;

public class CreateCharacterView
{
    public static string UIName ="CreateCharacterView";
    public GameObject _uiRoot;
    //public UISprite Spt_BG1;
    //public UITexture Tex_BG;
    //public UISprite Spt_Title;
    //public UISprite Spt_DecorationUp;
    //public UISprite Spt_DecorationDown;
    public UIToggle Tog_Boy;
    public UIButton Btn_Boy;
    ////public UISprite Spt_BtnBoyBackground;
    //public UISprite Spt_BtnBoyForground;
    //public UISprite Spt_BtnBoyGender;
    //public UISprite Spt_BtnBoyCheckmark;
    public UIToggle Tog_Girl;
    public UIButton Btn_Girl;
    //public UISprite Spt_BtnGirlBackground;
    //public UISprite Spt_BtnGirlForground;
    //public UISprite Spt_BtnGirlGender;
    //public UISprite Spt_BtnGirlCheckmark;
    public UIInput Ipt_Name;
    //public UISprite Spt_InputNameBackground;
    public UILabel Lbl_InputNameLabel;
    public UIButton Btn_Random;
    //public UISprite Spt_BtnRandomBackground;
    //public UISprite Spt_DescGirl;
    //public UISprite Spt_DescBG;
    public UILabel Lbl_Label;
    public UIButton Btn_Confirm;
    //public UISprite Spt_BtnConfirmBG;
    //public UISprite Spt_BtnConfirmFG;

    public TweenScale Anim_TScale;
    public UIButton Btn_Cancel;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CreateCharacterView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_Cancel = _uiRoot.transform.FindChild("Anim/Cancel").gameObject.GetComponent<UIButton>();
        //Spt_BG1 = _uiRoot.transform.FindChild("Anim/BG/BG1").gameObject.GetComponent<UISprite>();
        //Tex_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UITexture>();
        //Spt_Title = _uiRoot.transform.FindChild("Anim/BG/Title").gameObject.GetComponent<UISprite>();
        //Spt_DecorationUp = _uiRoot.transform.FindChild("Anim/BG/DecorationUp").gameObject.GetComponent<UISprite>();
        //Spt_DecorationDown = _uiRoot.transform.FindChild("Anim/BG/DecorationDown").gameObject.GetComponent<UISprite>();
        Tog_Boy = _uiRoot.transform.FindChild("Anim/CharToggle/Boy").gameObject.GetComponent<UIToggle>();
        Btn_Boy = _uiRoot.transform.FindChild("Anim/CharToggle/Boy").gameObject.GetComponent<UIButton>();
        //Spt_BtnBoyBackground = _uiRoot.transform.FindChild("Anim/CharToggle/Boy/Background").gameObject.GetComponent<UISprite>();
        //Spt_BtnBoyForground = _uiRoot.transform.FindChild("Anim/CharToggle/Boy/Forground").gameObject.GetComponent<UISprite>();
        //Spt_BtnBoyGender = _uiRoot.transform.FindChild("Anim/CharToggle/Boy/Gender").gameObject.GetComponent<UISprite>();
        //Spt_BtnBoyCheckmark = _uiRoot.transform.FindChild("Anim/CharToggle/Boy/Checkmark").gameObject.GetComponent<UISprite>();
        Tog_Girl = _uiRoot.transform.FindChild("Anim/CharToggle/Girl").gameObject.GetComponent<UIToggle>();
        Btn_Girl = _uiRoot.transform.FindChild("Anim/CharToggle/Girl").gameObject.GetComponent<UIButton>();
        //Spt_BtnGirlBackground = _uiRoot.transform.FindChild("Anim/CharToggle/Girl/Background").gameObject.GetComponent<UISprite>();
        //Spt_BtnGirlForground = _uiRoot.transform.FindChild("Anim/CharToggle/Girl/Forground").gameObject.GetComponent<UISprite>();
        //Spt_BtnGirlGender = _uiRoot.transform.FindChild("Anim/CharToggle/Girl/Gender").gameObject.GetComponent<UISprite>();
        //Spt_BtnGirlCheckmark = _uiRoot.transform.FindChild("Anim/CharToggle/Girl/Checkmark").gameObject.GetComponent<UISprite>();
        Ipt_Name = _uiRoot.transform.FindChild("Anim/Name").gameObject.GetComponent<UIInput>();
        //Spt_InputNameBackground = _uiRoot.transform.FindChild("Anim/Name/Background").gameObject.GetComponent<UISprite>();
        Lbl_InputNameLabel = _uiRoot.transform.FindChild("Anim/Name/Label").gameObject.GetComponent<UILabel>();
        Btn_Random = _uiRoot.transform.FindChild("Anim/Random").gameObject.GetComponent<UIButton>();
        //Spt_BtnRandomBackground = _uiRoot.transform.FindChild("Anim/Random/Background").gameObject.GetComponent<UISprite>();
        //Spt_DescGirl = _uiRoot.transform.FindChild("Anim/Decoration/DescGirl").gameObject.GetComponent<UISprite>();
        //Spt_DescBG = _uiRoot.transform.FindChild("Anim/Decoration/DescBG").gameObject.GetComponent<UISprite>();
        Lbl_Label = _uiRoot.transform.FindChild("Anim/Decoration/Label").gameObject.GetComponent<UILabel>();
        Btn_Confirm = _uiRoot.transform.FindChild("Anim/Confirm").gameObject.GetComponent<UIButton>();
        //Spt_BtnConfirmBG = _uiRoot.transform.FindChild("Anim/Confirm/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnConfirmFG = _uiRoot.transform.FindChild("Anim/Confirm/FG").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_InputNameLabel.text = ConstString.CREATE_CHAR_NAME_TIP;
        Lbl_Label.text = ConstString.CREATE_CHAR_TIP;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
