using UnityEngine;
using System;
using System.Collections;

public class AdvanceTipViewController : UIBase 
{
    public AdvanceTipView view;

    public override void Initialize()
    {
        if (view == null)
            view = new AdvanceTipView();
        view.Initialize();
        BtnEventBinding();
    }
    public void SetContent(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            UISystem.Instance.CloseGameUI(AdvanceTipView.UIName);
            return;
        }
        view.Lbl_Content.text = content;
    }
    public void ButtonEvent_Accept(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(AdvanceTipView.UIName);
    }
    public override void Uninitialize()
    {

    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Accept.gameObject).onClick = ButtonEvent_Accept;
    }


}
