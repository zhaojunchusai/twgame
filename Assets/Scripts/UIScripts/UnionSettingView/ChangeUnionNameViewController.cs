using UnityEngine;
using System;
using System.Collections;

public class ChangeUnionNameViewController : UIBase
{
    public ChangeUnionNameView view;

    public override void Initialize()
    {
        if (view == null)
            view = new ChangeUnionNameView();
        view.Initialize();
        BtnEventBinding();
        view.Lbl_CostNumLabel.text = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mRenameUnionCost.ToString();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }
    public void ButtonEvent_CancelButton(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ChangeUnionNameView.UIName);
    }

    public void ButtonEvent_ConfirmButton(GameObject btn)
    {
        if (string.IsNullOrEmpty(view.Ipt_NameInput.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_UNION_NAME_EMPTY);
            return;
        }

        if (!CommonFunction.CheckStringRule(view.Ipt_NameInput.value, 4, 12, ConstString.ERR_UNION_NAME_LENGTH))
            return;

        if (PlayerData.Instance._Diamonds < ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mRenameUnionCost)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_DIAMOND);
            return;
        }

        UnionModule.Instance.OnSendUpdateUnionName(view.Ipt_NameInput.value);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_CancelButton.gameObject).onClick = ButtonEvent_CancelButton;
        UIEventListener.Get(view.Btn_ConfirmButton.gameObject).onClick = ButtonEvent_ConfirmButton;
    }


}
