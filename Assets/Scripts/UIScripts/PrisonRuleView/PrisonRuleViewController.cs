using UnityEngine;
using System;
using System.Collections;

public class PrisonRuleViewController : UIBase
{
    public PrisonRuleView view;

    public override void Initialize()
    {
        if (view == null)
            view = new PrisonRuleView();
        view.Initialize();
        BtnEventBinding();
    }
    public void SetInfo(string rule,string title)
    {
        this.view.Lbl_Label_Rule.text = rule;
        this.view.Lbl_label_PrisonRule.text = title;
    }
    public void ButtonEvent_Button_close(GameObject btn)
    {
        this.Close(null,null);
    }

    public override void Uninitialize()
    {

    }
    public override void Destroy()
    {
        base.Destroy();
        this.view = null;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Marsk.gameObject).onClick = ButtonEvent_Button_close;
    }
}
