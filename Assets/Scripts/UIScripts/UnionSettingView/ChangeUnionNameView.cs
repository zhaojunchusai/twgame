using UnityEngine;
using System;
using System.Collections;

public class ChangeUnionNameView
{
    public static string UIName ="ChangeUnionNameView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_ChangeUnionNameView;
    public UILabel Lbl_Title;
    public UISprite Spt_BGSprite;
    public UISprite Spt_ContentSprite;
    public UIInput Ipt_NameInput;
    public UISprite Spt_InputNameInputNameInputBGSprite;
    public UILabel Lbl_InputNameInputNameInputLabel;
    public UILabel Lbl_CostLabel;
    public UISprite Spt_CostSprite;
    public UILabel Lbl_CostNumLabel;
    public UIButton Btn_CancelButton;
    public UILabel Lbl_BtnCancelButtonCancelFGSprite;
    public UISprite Spt_BtnCancelButtonCancelBGSprite;
    public UIButton Btn_ConfirmButton;
    public UILabel Lbl_BtnConfirmButtonConfirmFGSprite;
    public UISprite Spt_BtnConfirmButtonConfirmBGSprite;
    public UISprite Spt_MaskBGSprite;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/ChangeUnionNameView");
        UIPanel_ChangeUnionNameView = _uiRoot.GetComponent<UIPanel>();
        Lbl_Title = _uiRoot.transform.FindChild("OffsetRoot/Title").gameObject.GetComponent<UILabel>();
        Spt_BGSprite = _uiRoot.transform.FindChild("OffsetRoot/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_ContentSprite = _uiRoot.transform.FindChild("OffsetRoot/ContentSprite").gameObject.GetComponent<UISprite>();
        Ipt_NameInput = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/NameInput").gameObject.GetComponent<UIInput>();
        Spt_InputNameInputNameInputBGSprite = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/NameInput/NameInputBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_InputNameInputNameInputLabel = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/NameInput/NameInputLabel").gameObject.GetComponent<UILabel>();
        Lbl_CostLabel = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/CostLabel").gameObject.GetComponent<UILabel>();
        Spt_CostSprite = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/CostSprite").gameObject.GetComponent<UISprite>();
        Lbl_CostNumLabel = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/CostNumLabel").gameObject.GetComponent<UILabel>();
        Btn_CancelButton = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/CancelButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnCancelButtonCancelFGSprite = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/CancelButton/CancelFGSprite").gameObject.GetComponent<UILabel>();
        Spt_BtnCancelButtonCancelBGSprite = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/CancelButton/CancelBGSprite").gameObject.GetComponent<UISprite>();
        Btn_ConfirmButton = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/ConfirmButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnConfirmButtonConfirmFGSprite = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/ConfirmButton/ConfirmFGSprite").gameObject.GetComponent<UILabel>();
        Spt_BtnConfirmButtonConfirmBGSprite = _uiRoot.transform.FindChild("OffsetRoot/InuptNameGroup/ConfirmButton/ConfirmBGSprite").gameObject.GetComponent<UISprite>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_Title.text = "请输入军团名";
        //Lbl_InputNameInputNameInputLabel.text = "最长六个字";
        //Lbl_CostLabel.text = "修改军团名将消耗";
        //Lbl_CostNumLabel.text = "200000";
        //Lbl_BtnCancelButtonCancelFGSprite.text = "取消";
        //Lbl_BtnConfirmButtonConfirmFGSprite.text = "确定";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
