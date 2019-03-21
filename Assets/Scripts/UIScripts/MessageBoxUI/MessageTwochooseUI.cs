using UnityEngine;
using System;
using System.Collections;

public class MessageTwoChooseUI
{
    public static string UIName ="MessageTwoChooseUI";
    public GameObject _uiRoot;
    public UIButton Btn_Close;
    //public UISprite Spt_BtnCloseBG;
    public UIButton Btn_Confirm;
    public UISprite Spt_BtnConfirmBackground;
    public UILabel Lbl_BtnConfirmLabel;
    //public UISprite Spt_FG;
    //public UISprite Spt_BG;
    public UILabel Lbl_Info;
    public UIButton Btn_Cancel;
    //public UISprite Spt_BtnCancelBackground;
    public UILabel Lbl_BtnCancelLabel;
    //public UISprite Spt_Line1;
    //public UISprite Spt_Line2;
    public UIBoundary Boundary = new UIBoundary();
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/MessageTwoChooseUI");
        Btn_Close = _uiRoot.transform.FindChild("Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnCloseBG = _uiRoot.transform.FindChild("Close/BG").gameObject.GetComponent<UISprite>();
        Btn_Confirm = _uiRoot.transform.FindChild("Confirm").gameObject.GetComponent<UIButton>();
        Spt_BtnConfirmBackground = _uiRoot.transform.FindChild("Confirm/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirmLabel = _uiRoot.transform.FindChild("Confirm/Label").gameObject.GetComponent<UILabel>();
        //Spt_FG = _uiRoot.transform.FindChild("BG/FG").gameObject.GetComponent<UISprite>();
        //Spt_BG = _uiRoot.transform.FindChild("BG/BG").gameObject.GetComponent<UISprite>();
        Lbl_Info = _uiRoot.transform.FindChild("Info").gameObject.GetComponent<UILabel>();
        Btn_Cancel = _uiRoot.transform.FindChild("Cancel").gameObject.GetComponent<UIButton>();
        //Spt_BtnCancelBackground = _uiRoot.transform.FindChild("Cancel/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnCancelLabel = _uiRoot.transform.FindChild("Cancel/Label").gameObject.GetComponent<UILabel>();
        //Spt_Line1 = _uiRoot.transform.FindChild("Line1").gameObject.GetComponent<UISprite>();
        //Spt_Line2 = _uiRoot.transform.FindChild("Line2").gameObject.GetComponent<UISprite>();

        //SetLabelValues();
        SetBoundary();
    }


    public void SetBoundary()
    {
        Boundary.left = -250f;
        Boundary.right = 262f;
        Boundary.up = 162f;
        Boundary.down = -153.5f;
    }

    public void SetLabelValues()
    {
        Lbl_BtnConfirmLabel.text = "确  定";
        Lbl_Info.text = "我是卖报的小行家擦擦擦";
        Lbl_BtnCancelLabel.text = "取  消";
    }

    public void Uninitialize()
    {

    }



    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
