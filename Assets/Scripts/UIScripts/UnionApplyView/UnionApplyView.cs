using UnityEngine;
using System;
using System.Collections;

public class UnionApplyView
{
    public static string UIName ="UnionApplyView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionApplyView;
    public UILabel Lbl_Title;
    public UISprite Spt_BG;
    public UISprite Spt_FG;
    public UISprite Spt_Mask;
    public UIButton Btn_OneKeyRefuse;
    public UILabel Lbl_BtnOneKeyRefuseLabel;
    public UISprite Spt_Decoration1;
    public UISprite Spt_Decoration2;
    public UIButton Btn_OneKeyAgree;
    public UILabel Lbl_BtnOneKeyAgreeLabel;
    public UILabel Lbl_ApplyCount;
    public UIPanel UIPanel_Content;
    public UIScrollView ScrView_Content;
    public UIGrid Grd_Content;
    public GameObject Gobj_UnionApplyItem;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionApplyView");
        UIPanel_UnionApplyView = _uiRoot.GetComponent<UIPanel>();
        Lbl_Title = _uiRoot.transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_FG = _uiRoot.transform.FindChild("FG").gameObject.GetComponent<UISprite>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Btn_OneKeyRefuse = _uiRoot.transform.FindChild("OneKeyRefuse").gameObject.GetComponent<UIButton>();
        Lbl_BtnOneKeyRefuseLabel = _uiRoot.transform.FindChild("OneKeyRefuse/Label").gameObject.GetComponent<UILabel>();
        Spt_Decoration1 = _uiRoot.transform.FindChild("Decoration1").gameObject.GetComponent<UISprite>();
        Spt_Decoration2 = _uiRoot.transform.FindChild("Decoration2").gameObject.GetComponent<UISprite>();
        Btn_OneKeyAgree = _uiRoot.transform.FindChild("OneKeyAgree").gameObject.GetComponent<UIButton>();
        Lbl_BtnOneKeyAgreeLabel = _uiRoot.transform.FindChild("OneKeyAgree/Label").gameObject.GetComponent<UILabel>();
        Lbl_ApplyCount = _uiRoot.transform.FindChild("ApplyCount").gameObject.GetComponent<UILabel>();
        UIPanel_Content = _uiRoot.transform.FindChild("Content").gameObject.GetComponent<UIPanel>();
        ScrView_Content = _uiRoot.transform.FindChild("Content").gameObject.GetComponent<UIScrollView>();
        Grd_Content = _uiRoot.transform.FindChild("Content/Content").gameObject.GetComponent<UIGrid>();
        Gobj_UnionApplyItem = _uiRoot.transform.FindChild("Pre/UnionApplyItem").gameObject;
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "入会申请";
        Lbl_BtnOneKeyRefuseLabel.text = "一键拒绝";
        Lbl_BtnOneKeyAgreeLabel.text = "一键同意";
        Lbl_ApplyCount.text = "申请人数:9";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
