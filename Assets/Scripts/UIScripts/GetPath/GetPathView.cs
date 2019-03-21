using UnityEngine;
using System.Collections;

public class GetPathView
{
    public static string UIName = "GetPathView";
    public GameObject _uiRoot;
    public UIButton Btn_ButtonClose;
    public UIScrollView ScrollView_GetPathScrollView;
    public UIGrid Grd_Grid;
    public GameObject Gobj_GetPathItem;
    public UILabel Lab_GetPathDesc;
    public UIBoundary Boundary = new UIBoundary();
    public TweenScale Anim_TScale;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/GetPathView");
        ScrollView_GetPathScrollView = _uiRoot.transform.FindChild("Anim/GetPathScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("Anim/GetPathScrollView/Grid").gameObject.GetComponent<UIGrid>();
        Gobj_GetPathItem = _uiRoot.transform.FindChild("Anim/GetPathScrollView/Grid/gobj_GetPathItem").gameObject;
        Btn_ButtonClose = _uiRoot.transform.FindChild("Anim/Button_close").gameObject.GetComponent<UIButton>();
        Lab_GetPathDesc = _uiRoot.transform.FindChild("Anim/GetPathDesc_Lab").gameObject.GetComponent<UILabel>();
        Lab_GetPathDesc.gameObject.SetActive(false);
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        SetLabelValues();
    }
    public void SetLabelValues()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }
}
