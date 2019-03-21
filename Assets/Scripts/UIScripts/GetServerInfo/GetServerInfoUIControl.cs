using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;

public class GetServerInfoUIControl : UIBase 
{
    public GetServerInfoUI _ui;

    public override void Initialize()
    {
        if (_ui == null)
            _ui = new GetServerInfoUI();
        _ui.Initialize();
    }

    public override UIBoundary GetUIBoundary()
    {
        return _ui.Boundary;
    }

    public override void Uninitialize()
    {

    }



    public void setProgress(int progress)
    {
        _ui.Slider_progressBar.value = progress / 100;
    }


    public void setTips(string  value)
    {
        _ui.Lbl_TipsLB.text = value;
    }



    public void showProgressBar()
    {
        _ui.Slider_progressBar.gameObject.SetActive(true);
    }


    public void dismissProgressbar()
    {
        _ui.Slider_progressBar.gameObject.SetActive(false);
    }

    public void updateProgressBar(double value)
    {
        _ui.Slider_progressBar.value = (float)value;
    }

}
