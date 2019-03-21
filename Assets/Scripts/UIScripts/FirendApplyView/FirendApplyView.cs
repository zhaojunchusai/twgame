using UnityEngine;
using System;
using System.Collections;

public class FirendApplyView
{
    public static string UIName = "FriendApplyView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionApplyView;
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
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/FriendApplyView");
        UIPanel_UnionApplyView = _uiRoot.GetComponent<UIPanel>();
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
        Gobj_UnionApplyItem = _uiRoot.transform.FindChild("Content/Pre/UnionApplyItem").gameObject;
        Gobj_UnionApplyItem.gameObject.SetActive(false);
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
