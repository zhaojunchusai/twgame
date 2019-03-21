using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SoldierEquipChoosePanel : UIBase
{
    public SoldierEquipChoose view;
    public static SoldierEquipChoosePanel create(Soldier sd)
    {
        SoldierEquipChoosePanel temp = new SoldierEquipChoosePanel();
        temp.Initialize();
        temp.view.soldier = sd;
        return temp;
    }
    public void SetParent(Transform father)
    {
        this.view.parent = father.gameObject;
        this.view._uiRoot.transform.SetParent(father);
        this.view._uiRoot.transform.localScale = new Vector3(1, 1, 1);
        this.view._uiRoot.transform.position = new Vector3(0, 0, 0);
    }
    public override void Initialize()
    {
        if (view == null)
            view = new SoldierEquipChoose();
        view.Initialize();

        BtnEventBinding();
    }
    public void ButtonEvent_Button_close(GameObject btn)
    {
        GameObject.Destroy(this.view._uiRoot);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view.parent.transform));
    }
    public override UIBoundary GetUIBoundary()
    {
        return view.Boundary;
    }
    public override void Uninitialize()
    {
        view.Uninitialize();
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
    }
}

