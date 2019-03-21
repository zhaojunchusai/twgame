using UnityEngine;
using System;
using System.Collections;

public class FriendInviteView
{
    public static string UIName = "FriendInviteView";
    public GameObject _uiRoot;
    public UISprite Spt_Mask;
    public UIScrollView ScrView_Content;
    public UIGrid Grd_Content;
    public UIWrapContent wrapContent;
    public GameObject Gobj_UnionApplyItem;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/FriendInviteView");
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        ScrView_Content = _uiRoot.transform.FindChild("MissonList").gameObject.GetComponent<UIScrollView>();
        Grd_Content = _uiRoot.transform.FindChild("MissonList/Grid").gameObject.GetComponent<UIGrid>();
        wrapContent = _uiRoot.transform.FindChild("MissonList/Grid").gameObject.GetComponent<UIWrapContent>();
        Gobj_UnionApplyItem = _uiRoot.transform.FindChild("MissonList/FriendMemberItem").gameObject;
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
