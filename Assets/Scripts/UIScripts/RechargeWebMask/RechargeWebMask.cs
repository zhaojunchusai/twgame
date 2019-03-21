using UnityEngine;
using System;
using System.Collections;

public class RechargeWebMask
{
    public const string UIName ="RechargeWebMask";
    public GameObject _uiRoot;
    public UIButton Btn_Close;
    public UISprite Spt_Mask;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/RechargeWebMask");
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Btn_Close = _uiRoot.transform.FindChild("Anchor/Close").gameObject.GetComponent<UIButton>();
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
