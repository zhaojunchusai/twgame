using UnityEngine;
using System;
using System.Collections;
using System.Text.RegularExpressions;

public class CreateCharacterViewController : UIBase 
{
    public CreateCharacterView view;

    public override void Initialize()
    {
        if (view == null)
            view = new CreateCharacterView();
        view.Initialize();
        BtnEventBinding(); 
        //PlayOpenAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
    }

    /// <summary>
    /// 设置昵称
    /// </summary>
    public void SetNickName(string nickname)
    {
        if (view != null && view.Ipt_Name != null)
        {
            view.Ipt_Name.value = nickname;
            
        }
    }
    
    public void ButtonEvent_Random(GameObject btn)
    {
        LoginModule.Instance.SendCharname(LoginModule.Instance.CurServerId);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click , view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        
        if (!LoginModule.Instance.CheckNameRule(view.Ipt_Name.value))
            return;

        uint gender = (uint)(view.Tog_Boy.value ? EHeroGender.ehgMale : EHeroGender.ehgFamale);
        LoginModule.Instance.SendCreateCharacter(gender, view.Ipt_Name.value, 0);
    }
    public void ButtonEvent_Cancel(GameObject Btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        //SDKManager.Instance.SDKOperate_NormalLogout();
        Main.Instance.LoginOut();
    }
    public override void Uninitialize()
    {

    }
    public override void Destroy()
    {
        view = null;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Cancel.gameObject).onClick = ButtonEvent_Cancel;
        UIEventListener.Get(view.Btn_Random.gameObject).onClick = ButtonEvent_Random;
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
    }


    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();

    //}
}
