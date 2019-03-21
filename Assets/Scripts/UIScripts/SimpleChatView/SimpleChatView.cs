using UnityEngine;
using System;
using System.Collections;

public class SimpleChatView
{
    public static string UIName = "SimpleChatView";
    public GameObject _uiRoot;
    public UIWidget UIWidget_SimpleChat;
    public UILabel Lbl_ChatInfo;
    public UISprite Spt_Notify;
    public UISprite Spt_BG;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/SimpleChatView");
        UIWidget_SimpleChat = _uiRoot.transform.FindChild("SimpleChat").gameObject.GetComponent<UIWidget>();
        Lbl_ChatInfo = _uiRoot.transform.FindChild("SimpleChat/Panel/ChatInfo").gameObject.GetComponent<UILabel>();
        Spt_Notify = _uiRoot.transform.FindChild("SimpleChat/Notify").gameObject.GetComponent<UISprite>();
    }


    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
