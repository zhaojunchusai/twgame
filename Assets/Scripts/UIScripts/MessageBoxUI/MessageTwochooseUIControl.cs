using Assets.Script.Common;
using UnityEngine;
using System.Collections;

public class MessageTwoChooseUIControl : UIBase
{

    public MessageTwoChooseUI _ui;
    public UIEventListener.VoidDelegate FunctionConfirm;
    public UIEventListener.VoidDelegate FunctionCancel;
    private bool confirmIsClose = true;
    private GameObject mSendObj;
    public int _countDownSeconds = 0;
    public override void Initialize()
    {
        if (_ui == null)
            _ui = new MessageTwoChooseUI();
        _ui.Initialize();
        UIEventListener.Get(_ui.Btn_Confirm.gameObject).onClick = OnClickConfirm;
        UIEventListener.Get(_ui.Btn_Close.gameObject).onClick = OnClickClose;
        UIEventListener.Get(_ui.Btn_Cancel.gameObject).onClick = OnClickClose;
    }

    public override void Uninitialize()
    {
        _ui.Uninitialize();
        UIEventListener.Get(_ui.Btn_Confirm.gameObject).onClick = null;
        UIEventListener.Get(_ui.Btn_Close.gameObject).onClick = null;
        UIEventListener.Get(_ui.Btn_Cancel.gameObject).onClick = null;
    }
    public void SetInfo(string vContent, UIEventListener.VoidDelegate vFunctionConfirm = null, UIEventListener.VoidDelegate vFunctionCancel = null, GameObject vSendObj = null, bool isClose = true)
    {
        if (_ui == null)
            return;
        _ui.Lbl_Info.gameObject.SetActive(true);
        confirmIsClose = isClose;
        FunctionConfirm = null;
        FunctionCancel = null;
        _ui.Lbl_Info.text = vContent;
        FunctionConfirm = vFunctionConfirm;
        FunctionCancel = vFunctionCancel;
        mSendObj = vSendObj;
    }
 
    public void OnClickConfirm(GameObject go)
    {
        if (FunctionConfirm != null)
            FunctionConfirm(mSendObj);
        //if (confirmIsClose)
        //    UISystem.Instance.CloseGameUI(GlobalConst.DIR_UINAME_MESSAGETWOCHOOSEBOX);
    }
    public void OnClickClose(GameObject go)
    {
        //if (FunctionCancel != null)
        //    FunctionCancel(mSendObj);
        //UISystem.Instance.CloseGameUI(GlobalConst.DIR_UINAME_MESSAGETWOCHOOSEBOX);
    }
    
}
