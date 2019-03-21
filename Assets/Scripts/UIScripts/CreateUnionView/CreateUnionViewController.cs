using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class CreateUnionViewController : UIBase 
{
    public CreateUnionView view;
    private List<UnionIconItem> _iconItems = new List<UnionIconItem>();
    //private GameObject _selectedMark;
    private string _iconID;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new CreateUnionView();
        }
        view.Initialize();
        BtnEventBinding();
        ShowIcons();

        view.Lbl_CostNum.text = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mCreateUnionCost.ToString();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _iconItems.Clear();
        //_selectedMark = null;
    }
    private void ShowIcons()
    {
        //if (_iconItems.Count <= 0)
        //{
        //    List<UnionIconData> list = ConfigManager.Instance.mUnionConfig.GetUnionIconList();
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        GameObject go = CommonFunction.InstantiateObject(view.Gobj_UnionIcon, view.Grd_Icons.transform);
        //        go.name = list[i].mID.ToString();
        //        _iconItems.Add(go.AddComponent<UnionIconItem>());
        //        _iconItems[i].Init(list[i].mIcon, ClickUnionIcon);
        //    }
        //    view.Grd_Icons.Reposition();
        //    //_selectedMark = view._uiRoot.transform.FindChild("Pre/Selected").gameObject;
        //    ClickUnionIcon(_iconItems[0].transform.GetChild(0).gameObject);
        //}
        if ((view.Spt_UnionIcon != null) && (ConfigManager.Instance.mUnionConfig.GetMinUnionIconData() != null))
        {
            CommonFunction.SetSpriteName(view.Spt_UnionIcon, ConfigManager.Instance.mUnionConfig.GetMinUnionIconData().mIcon);
        }
        _iconID = ConfigManager.Instance.mUnionConfig.minUnionBadgeID.ToString();
    }

    private void ClickUnionIcon(GameObject icon)
    {
        //_selectedMark.transform.SetParent(icon.transform);
        //_selectedMark.transform.localPosition = Vector3.zero;
        _iconID = icon.transform.parent.gameObject.name;
    }

    public void ButtonEvent_CreateUnion(GameObject btn)
    {
        if (string.IsNullOrEmpty(view.Ipt_UnionName.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_UNION_NAME_EMPTY);
            return;
        }

        if (!CommonFunction.CheckStringRule(view.Ipt_UnionName.value, 4, 12, ConstString.ERR_UNION_NAME_LENGTH))
            return;

        if(PlayerData.Instance._Diamonds < ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mCreateUnionCost)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_DIAMOND);
            return;
        }
        string notice = UnionModule.Instance.CheckUnionNotice(view.Ipt_Notice.value);
        //if (!CommonFunction.CheckStringRule(notice, 0, 50, ConstString.ERR_UNION_NOTICE_LENGTH))
        //    return;
        Union union = new Union();
        union.base_info = new BaseUnion();
        union.base_info.name = view.Ipt_UnionName.value;
        union.base_info.icon = _iconID;
        union.desc = notice;
        UnionModule.Instance.OnSendCreateUnion(union);
    }

    public void ButtonEvent_ChooseBadge(GameObject btn)
    {
        UnionModule.Instance.OnSendQueryUnionIcon();
    }

    public void CloseUI(GameObject go)
    {
        UISystem.Instance.CloseGameUI(CreateUnionView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_CreateUnion.gameObject).onClick = ButtonEvent_CreateUnion;
        UIEventListener.Get(view.Btn_ChooseBadge.gameObject).onClick = ButtonEvent_ChooseBadge;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = CloseUI;
    }

    public void RefreshBadge(uint vID)
    {
        UnionIconData tmpData = ConfigManager.Instance.mUnionConfig.GetUnionIconByID(vID);
        if (tmpData != null)
        {
            _iconID = vID.ToString();
            if ((view.Spt_UnionIcon != null))
            {
                CommonFunction.SetSpriteName(view.Spt_UnionIcon, tmpData.mIcon);
            }
        }
    }
}
