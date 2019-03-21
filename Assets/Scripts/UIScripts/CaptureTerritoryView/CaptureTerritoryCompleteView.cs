using UnityEngine;
using System;
using System.Collections;

public class CaptureTerritoryCompleteView
{
    public static string UIName ="CaptureTerritoryCompleteView";
    public GameObject _uiRoot;
    public UISprite Spt_Mask;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CaptureTerritoryCompleteView");
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
