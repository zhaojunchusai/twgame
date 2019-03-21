using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class ServerItem : MonoBehaviour
{
    [HideInInspector]public UIButton Btn_ServerItem;
    [HideInInspector]public UILabel Lbl_BtnServerItemName;
    [HideInInspector]public UILabel Lbl_BtnServerItemStatus;
    [HideInInspector]public UISprite Spt_BtnServerItemLine;

    private bool _initialized = false;
    private ServerDetail _detail;
    public void Initialize()
    {
        if(_initialized)
            return;
        _initialized = true;

        Btn_ServerItem = GetComponent<UIButton>();
        Lbl_BtnServerItemName = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Lbl_BtnServerItemStatus = transform.FindChild("Status").gameObject.GetComponent<UILabel>();
        Spt_BtnServerItemLine = transform.FindChild("Line").gameObject.GetComponent<UISprite>();
        SetLabelValues();

        UIEventListener.Get(Btn_ServerItem.gameObject).onClick = ButtonEvent_ServerItem;
    }

    public void InitItem(ServerDetail detail)
    {
        Initialize();
        _detail = detail;
        Lbl_BtnServerItemName.text = detail.desc;
        Lbl_BtnServerItemStatus.text = LoginModule.Instance.GetServerStateString(_detail.status);
        CheckSelectedMark();
    }
    
    public void ButtonEvent_ServerItem(GameObject btn)
    {
        LoginModule.Instance.CurServer = _detail;
        CheckSelectedMark();
        UISystem.Instance.ChooseServerView.CloseUI();
    }

    private void CheckSelectedMark()
    {
        if (LoginModule.Instance.CurServer != null && _detail.area_id == LoginModule.Instance.CurServerId)
            UISystem.Instance.ChooseServerView.SetSelectedServerItemMark(transform);
    }

    public void SetLabelValues()
    {
        Lbl_BtnServerItemName.text = "";
        Lbl_BtnServerItemStatus.text = "";
    }
}
