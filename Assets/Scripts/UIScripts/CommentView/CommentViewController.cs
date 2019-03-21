using UnityEngine;
using System;
using System.Collections;

public class CommentViewController : UIBase 
{
    public CommentView view;

    public override void Initialize()
    {
        if (view == null)
            view = new CommentView();
        view.Initialize();
        BtnEventBinding();

        view.Btn_Confirm.gameObject.SetActive(false);
        view.Btn_NoRemind.gameObject.SetActive(true);
        view.Btn_Refuse.gameObject.SetActive(true);
        view.Btn_Go.gameObject.SetActive(true);
        view.Lbl_Content.text = ConstString.GO_COMMENT_TIP;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }

    private void CommentFinish()
    {
        view.Btn_Confirm.gameObject.SetActive(true);
        view.Btn_NoRemind.gameObject.SetActive(false);
        view.Btn_Refuse.gameObject.SetActive(false);
        view.Btn_Go.gameObject.SetActive(false);
        view.Lbl_Content.text = ConstString.GET_COMMENT_REWARD_TIP;
    }

    public void ButtonEvent_NoRemind(GameObject btn)
    {
        PlayerData.Instance.RemindToComment = false;
        TaskModule.Instance.SendNoRemindComment();
        UISystem.Instance.CloseGameUI(CommentView.UIName);
    }

    public void ButtonEvent_Refuse(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(CommentView.UIName);
    }

    public void ButtonEvent_Go(GameObject btn)
    {
        PlayerData.Instance.RemindToComment = false;
        CommonFunction.OpenTargetView(ETaskOpenView.GPMarket);
        CommentFinish();
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(CommentView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_NoRemind.gameObject).onClick = ButtonEvent_NoRemind;
        UIEventListener.Get(view.Btn_Refuse.gameObject).onClick = ButtonEvent_Refuse;
        UIEventListener.Get(view.Btn_Go.gameObject).onClick = ButtonEvent_Go;
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
    }


}
