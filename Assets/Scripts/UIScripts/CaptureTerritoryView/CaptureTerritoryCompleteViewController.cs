using UnityEngine;
using System;
using System.Collections;

public class CaptureTerritoryCompleteViewController : UIBase 
{
    public CaptureTerritoryCompleteView view;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new CaptureTerritoryCompleteView();
            view.Initialize();
            BtnEventBinding();
        }
        Assets.Script.Common.Scheduler.Instance.AddTimer(2f, false, CloseUI);
    }

    private void CloseUI()
    {
        UISystem.Instance.CloseGameUI(CaptureTerritoryCompleteView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {

    }


}
