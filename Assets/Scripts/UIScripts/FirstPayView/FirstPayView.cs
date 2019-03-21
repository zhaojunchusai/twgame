using UnityEngine;
using System;
using System.Collections;

public class FirstPayView
{
    public static string UIName ="FirstPayView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_FirstPayView;
    //public UISprite Spt_BG;
    public UIButton Btn_BackBtn;
    public UILabel Lbl_Title;
    public UISprite Spt_FG;
    public UIButton Btn_Recharge;
    public UILabel Lbl_BtnRechargeLabel;
    public UIButton Btn_GetReward;
    public UILabel Lbl_BtnGetRewardLabel;

    public UISprite[] Spt_Icons = new UISprite[3];
    public UISprite[] Spt_IconFrames = new UISprite[3];
    public UILabel[] Lbl_Names = new UILabel[3];

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/FirstPayView");
        UIPanel_FirstPayView = _uiRoot.GetComponent<UIPanel>();
        Btn_BackBtn = _uiRoot.transform.FindChild("Anim/BackBtn").gameObject.GetComponent<UIButton>();
        Lbl_Title = _uiRoot.transform.FindChild("Anim/BGS/Title").gameObject.GetComponent<UILabel>();
        Spt_FG = _uiRoot.transform.FindChild("Anim/BGS/Sprite1").gameObject.GetComponent<UISprite>();
        Btn_Recharge = _uiRoot.transform.FindChild("Anim/Recharge").gameObject.GetComponent<UIButton>();
        Lbl_BtnRechargeLabel = _uiRoot.transform.FindChild("Anim/Recharge/Label").gameObject.GetComponent<UILabel>();

        Spt_IconFrames[0] = _uiRoot.transform.FindChild("Anim/Prop1/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icons[0] = _uiRoot.transform.FindChild("Anim/Prop1/Icon").gameObject.GetComponent<UISprite>();
        Lbl_Names[0] = _uiRoot.transform.FindChild("Anim/Prop1/Name").gameObject.GetComponent<UILabel>();

        Spt_IconFrames[1] = _uiRoot.transform.FindChild("Anim/Prop2/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icons[1] = _uiRoot.transform.FindChild("Anim/Prop2/Icon").gameObject.GetComponent<UISprite>();
        Lbl_Names[1] = _uiRoot.transform.FindChild("Anim/Prop2/Name").gameObject.GetComponent<UILabel>();

        Spt_IconFrames[2] = _uiRoot.transform.FindChild("Anim/Prop3/IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icons[2] = _uiRoot.transform.FindChild("Anim/Prop3/Icon").gameObject.GetComponent<UISprite>();
        Lbl_Names[2] = _uiRoot.transform.FindChild("Anim/Prop3/Name").gameObject.GetComponent<UILabel>();

        Btn_GetReward = _uiRoot.transform.FindChild("Anim/GetReward").gameObject.GetComponent<UIButton>();
        Lbl_BtnGetRewardLabel = _uiRoot.transform.FindChild("Anim/GetReward/Label").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "";
        Lbl_BtnRechargeLabel.text = "快速充值";
        Lbl_BtnGetRewardLabel.text = "领取礼包";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
