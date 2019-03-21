using UnityEngine;
using System;
using System.Collections;

public class RechargeWebMaskController : UIBase 
{
    public RechargeWebMask view;

    public override void Initialize()
    {
        if (view == null)
            view = new RechargeWebMask();
        view.Initialize();
        BtnEventBinding();
        SetState();
    }

    public void SetState(bool isRecharge = true)
    {
        view.Spt_Mask.color = isRecharge ? new Color(1, 1, 1, 0.5f) : new Color(1, 1, 1, 1/255f);
        view.Btn_Close.gameObject.SetActive(isRecharge);
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(RechargeWebMask.UIName);
        SFRecharge.Instance.SetVisibility(false);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
    }
    
}
