using UnityEngine;
using System;
using System.Collections;

public class FirendAddView
{
    public static string UIName = "FriendAddView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionApplyView;
    public UISprite Spt_Mask;
    public UIButton Btn_Refresh;
    public UILabel Lbl_Btn_RefreshLabel;
    public UIButton Btn_Find;
    public UILabel Lbl_Btn_FindLabel;
    public UIPanel UIPanel_Content;
    public UIScrollView ScrView_Content;
    public UIGrid Grd_Content;
    public GameObject Gobj_UnionApplyItem;
    public UIInput input;
    public GameObject Effect_1;
    public GameObject Effect_2;
    public GameObject Effect_3;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/FriendAddView");

        UIPanel_UnionApplyView = _uiRoot.GetComponent<UIPanel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Btn_Refresh = _uiRoot.transform.FindChild("OneKeyRefuse").gameObject.GetComponent<UIButton>();
        Lbl_Btn_RefreshLabel = _uiRoot.transform.FindChild("OneKeyRefuse/Label").gameObject.GetComponent<UILabel>();
        Btn_Find = _uiRoot.transform.FindChild("OneKeyAgree").gameObject.GetComponent<UIButton>();
        Lbl_Btn_FindLabel = _uiRoot.transform.FindChild("OneKeyAgree/Label").gameObject.GetComponent<UILabel>();
        UIPanel_Content = _uiRoot.transform.FindChild("Content").gameObject.GetComponent<UIPanel>();
        ScrView_Content = _uiRoot.transform.FindChild("Content").gameObject.GetComponent<UIScrollView>();
        Grd_Content = _uiRoot.transform.FindChild("Content/Content").gameObject.GetComponent<UIGrid>();
        Gobj_UnionApplyItem = _uiRoot.transform.FindChild("Content/Pre/UnionApplyItem").gameObject;
        input = _uiRoot.transform.FindChild("InputGroup/Input").gameObject.GetComponent<UIInput>();
        Effect_1 = _uiRoot.transform.FindChild("Pos/1").gameObject;
        Effect_2 = _uiRoot.transform.FindChild("Pos/2").gameObject;
        Effect_3 = _uiRoot.transform.FindChild("Pos/3").gameObject;
        Gobj_UnionApplyItem.SetActive(false);
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
