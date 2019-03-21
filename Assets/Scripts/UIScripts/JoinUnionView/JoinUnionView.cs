using UnityEngine;
using System;
using System.Collections;

public class JoinUnionView
{
    public static string UIName ="JoinUnionView";
    public GameObject _uiRoot;
    public UILabel Lbl_Title;
    public UIButton Btn_Close;
    public UILabel Lbl_TimesCount;
    public UIScrollView ScrView_Items;
    public UIGrid Grd_Items;
    public UIButton Btn_CreateUnion;
    public UILabel Lbl_BtnCreateUnionLabel;
    public UIButton Btn_SearchUnion;
    public UILabel Lbl_BtnSearchUnionLabel;
    public UIInput Ipt_UnionID;
    public UILabel Lbl_InputUnionIDLabel;
    public UIButton Btn_CancelSearch;
    public UILabel Lbl_BtnCancelSearchLabel;
    public GameObject Gobj_UnionItem;
    public UIPanel Mask;
    public GameObject EffectMask;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/JoinUnionView");
        Mask = _uiRoot.transform.FindChild("Anim/EffectMask").gameObject.GetComponent<UIPanel>();
        EffectMask = _uiRoot.transform.FindChild("Anim/EffectMask/Mask").gameObject;
        Lbl_Title = _uiRoot.transform.FindChild("Anim/Layer1/Title").gameObject.GetComponent<UILabel>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Layer1/Close").gameObject.GetComponent<UIButton>();
        Lbl_TimesCount = _uiRoot.transform.FindChild("Anim/Layer1/TimesCount").gameObject.GetComponent<UILabel>();
        ScrView_Items = _uiRoot.transform.FindChild("Anim/Layer2/Items").gameObject.GetComponent<UIScrollView>();
        Grd_Items = _uiRoot.transform.FindChild("Anim/Layer2/Items/Items").gameObject.GetComponent<UIGrid>();
        Btn_CreateUnion = _uiRoot.transform.FindChild("Anim/Layer3/CreateUnion").gameObject.GetComponent<UIButton>();
        Lbl_BtnCreateUnionLabel = _uiRoot.transform.FindChild("Anim/Layer3/CreateUnion/Label").gameObject.GetComponent<UILabel>();
        Btn_SearchUnion = _uiRoot.transform.FindChild("Anim/Layer3/SearchUnion").gameObject.GetComponent<UIButton>();
        Lbl_BtnSearchUnionLabel = _uiRoot.transform.FindChild("Anim/Layer3/SearchUnion/Label").gameObject.GetComponent<UILabel>();
        Ipt_UnionID = _uiRoot.transform.FindChild("Anim/Layer3/UnionID").gameObject.GetComponent<UIInput>();
        Lbl_InputUnionIDLabel = _uiRoot.transform.FindChild("Anim/Layer3/UnionID/Label").gameObject.GetComponent<UILabel>();
        Btn_CancelSearch = _uiRoot.transform.FindChild("Anim/Layer3/CancelSearch").gameObject.GetComponent<UIButton>();
        Lbl_BtnCancelSearchLabel = _uiRoot.transform.FindChild("Anim/Layer3/CancelSearch/Label").gameObject.GetComponent<UILabel>();
        Gobj_UnionItem = _uiRoot.transform.FindChild("Anim/Pre/UnionItem").gameObject;
        //SetLabelValues();
        EffectMask.SetActive(false);
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "加入军团";
        Lbl_TimesCount.text = "今日申请次数:1/5";
        Lbl_BtnCreateUnionLabel.text = "创建军团";
        Lbl_BtnSearchUnionLabel.text = "查找军团";
        Lbl_InputUnionIDLabel.text = "点击输入军团ID";
        Lbl_BtnCancelSearchLabel.text = "查看所有军团";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
