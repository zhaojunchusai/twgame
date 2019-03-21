using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class EquipChoosePanel : UIBase
{
    public EquipChoose view;
    public static EquipChoosePanel create()
    {
        EquipChoosePanel temp = new EquipChoosePanel();
        temp.Initialize();

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
            view = new EquipChoose();
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
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
    }

    //界面动画
    public void PlayOpenAnim()
    {
        view.Anim_TScale.gameObject.transform.localScale = Vector3.zero;
        view.Anim_TScale.Restart();
        view.Anim_TScale.PlayForward();

    }

}

