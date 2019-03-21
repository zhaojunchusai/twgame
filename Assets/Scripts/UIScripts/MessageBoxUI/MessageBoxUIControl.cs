using UnityEngine;
using System;
using System.Collections;

public class MessageBoxUIControl : UIBase 
{
    public MessageBoxUI _ui;
    private UIEventListener.VoidDelegate ConfirmEvent;

    public override void Initialize()
    {
        if (_ui == null)
            _ui = new MessageBoxUI();
        _ui.Initialize();

        BtnEventBinding();
    }

    public void ShowMessage(string content, string title, UIEventListener.VoidDelegate confirmEvent = null, UIWidget.Pivot anchor = UIWidget.Pivot.Center)
    {
        _ui.Lbl_Content.text = content;
        _ui.Lbl_Title.text = title;
        if (confirmEvent != null)
            ConfirmEvent = confirmEvent;

        switch (anchor)
        {
            case UIWidget.Pivot.Center:
                {
                    _ui.Lbl_Content.pivot = anchor;
                    _ui.Lbl_Content.transform.localPosition = Vector3.zero;
                    break;
                }
            case UIWidget.Pivot.TopLeft:
                {
                    _ui.Lbl_Content.pivot = anchor;
                    _ui.Lbl_Content.transform.localPosition = new Vector3(-160,60,0);
                    break;
                }
        }
    }

    private void CloseThis(GameObject btn)
    {
        Close(null, null);
    }

    private void ButtonEvent_Confirm(GameObject btn)
    {
        CloseThis(btn);

        if (ConfirmEvent != null)
            ConfirmEvent(btn);
    }

    public override UIBoundary GetUIBoundary()
    {
        return _ui.Boundary;
    }

    public override void Uninitialize()
    {
        ConfirmEvent = null;
    }


    private void BtnEventBinding()
    {
        UIEventListener.Get(_ui.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
        UIEventListener.Get(_ui.Spt_Mask.gameObject).onClick = CloseThis;
    }

}

