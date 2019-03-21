using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;

public class GMViewController : UIBase 
{
    public GMView view;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new GMView();
            
        }
        view.Initialize();
        BtnEventBinding();
        
    }

    private void UnlockGuide()
    {
        uint[] list1 = new uint[]{101,102,103,104,105,106,150,107,117,151,
            152,153,201,202,203,204,205,206,207,208,209,210,211,212,213,214,
            301,302,303,304,305,306,307,308,309,310,311,312,313,314};

        for (int i = 0; i < list1.Length; i++)
        {
            Debug.LogError(i + " " + list1[i]);
            int k = i;
            Scheduler.Instance.AddTimer(k * 2, false, () =>
            {
                MainCityModule.Instance.StartNewbieGuideReq(list1[k]);
            });
            Scheduler.Instance.AddTimer(k * 2 + 1, false, () =>
            {
                MainCityModule.Instance.FinishNewbieGuideReq(list1[k]);
            });
        }
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if(view.Ipt_Input.value != null)
        {
            if (view.Ipt_Input.value.Contains("guide"))
            {
                UnlockGuide();
            }
            
            else
            {
                GMModule.Instance.SendGMCommandReq(view.Ipt_Input.value);
            }
        }
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));

        Close(null,null);
    }
    
    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;

    }

    
}
