using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

public class ChangeUnionIconViewController : UIBase 
{
    public ChangeUnionIconView view;
    private List<UnionIconItem> _iconItems = new List<UnionIconItem>();
    private GameObject _selectedMark;
    private string _iconID;
    public override void Initialize()
    {
        if (view == null)
            view = new ChangeUnionIconView();
        view.Initialize();
        BtnEventBinding();
        ShowIcons();
        view.Lbl_CostNumLabel.text = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mUpdateUnionIconCost.ToString();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _iconItems.Clear();
    }
    private void ShowIcons()
    {
        if (_iconItems.Count <= 0)
        {
            List<UnionIconData> list = ConfigManager.Instance.mUnionConfig.GetUnionIconList();
            for (int i = 0; i < list.Count; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionIcon, view.Grd_Icons.transform);
                go.name = list[i].mID.ToString();
                _iconItems.Add(go.AddComponent<UnionIconItem>());
                _iconItems[i].Init(list[i].mIcon, ClickUnionIcon);
            }
            view.Grd_Icons.Reposition();
            _selectedMark = view._uiRoot.transform.FindChild("Pre/Selected").gameObject;
            ClickUnionIcon(_iconItems[0].transform.GetChild(0).gameObject);
        }
    }

    private void ClickUnionIcon(GameObject icon)
    {
        _selectedMark.transform.SetParent(icon.transform);
        _selectedMark.transform.localPosition = Vector3.zero;
        _iconID = icon.transform.parent.gameObject.name;
    }

    public void ButtonEvent_CancelButton(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ChangeUnionIconView.UIName);
    }

    public void ButtonEvent_ConfirmButton(GameObject btn)
    {
        if (PlayerData.Instance._Diamonds < ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mUpdateUnionIconCost)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_DIAMOND);
            return;
        }

        if (_iconID != UnionModule.Instance.UnionInfo.base_info.icon)
            UnionModule.Instance.OnSendUpdateUnionIcon(_iconID);
        else
        {
            UISystem.Instance.CloseGameUI(ChangeUnionIconView.UIName);
        }
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
