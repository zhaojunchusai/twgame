using UnityEngine;
using System;
using System.Collections;

public class UnionIconItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_IconBG;
    [HideInInspector]public UISprite Spt_IconFrame;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_BG;

    public void Initialize()
    {
        Spt_IconBG = transform.FindChild("IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconFrame = transform.FindChild("IconFrame").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();

    }

    public void Init(string icon, UIEventListener.VoidDelegate clickEvent)
    {
        Initialize();
        CommonFunction.SetSpriteName(Spt_Icon, icon);
        UIEventListener.Get(Spt_IconBG.gameObject).onClick = clickEvent;
    }

}
