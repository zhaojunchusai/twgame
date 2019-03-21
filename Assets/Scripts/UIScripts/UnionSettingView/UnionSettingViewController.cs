using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionSettingViewController : UIBase 
{
    public UnionSettingView view;
    private int _indexType;
    private int _limitLv;
    private int MAX_LIMIT_LV = 100;
    private int MIN_LIMIT_LV = 0;
    private int INTERAL_LIMIT_LV = 5;
    public override void Initialize()
    {
        if (view == null)
            view = new UnionSettingView();
        view.Initialize();
        BtnEventBinding();
        InitUI();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }
    public void InitUI()
    {
        Union _union = UnionModule.Instance.UnionInfo;
        CommonFunction.SetSpriteName(view.Spt_Icon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_union.base_info.icon));
        MIN_LIMIT_LV = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel;
        view.Lbl_Name.text = _union.base_info.name;
        view.Ipt_Notice.value = _union.desc;
        _limitLv = _union.base_info.limit_level;
        _indexType = (int)_union.base_info.limit_type;
        FixLimitLv();
        FixLimitType();
    }

    private void FixLimitType()
    {
        _indexType = Mathf.Clamp(_indexType, 3, 4);
        view.Lbl_LimitType.text = UnionModule.Instance.GetUnionLimitTypeString((UnionLimitType)_indexType);
    }

    private void FixLimitLv()
    {
        _limitLv = Mathf.Clamp(_limitLv, MIN_LIMIT_LV, MAX_LIMIT_LV);

        if (_limitLv % INTERAL_LIMIT_LV != 0)
        {
            _limitLv = Mathf.RoundToInt((float)_limitLv / INTERAL_LIMIT_LV) * INTERAL_LIMIT_LV;
        }

        view.Lbl_LimitLv.text = _limitLv.ToString();
    }

    public void ButtonEvent_ChangeIcon(GameObject btn)
    {
        //UISystem.Instance.ShowGameUI(ChangeUnionIconView.UIName);
        UnionModule.Instance.OnSendQueryUnionIcon();
    }

    public void ButtonEvent_ChangeName(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ChangeUnionNameView.UIName);
    }

    public void ButtonEvent_LimitTypeRight(GameObject btn)
    {
        _indexType++;
        FixLimitType();
    }

    public void ButtonEvent_LimitTypeLeft(GameObject btn)
    {
        _indexType--;
        FixLimitType();
    }

    public void ButtonEvent_LimitLvRight(GameObject btn)
    {
        _limitLv += INTERAL_LIMIT_LV;
        FixLimitLv();
    }

    public void ButtonEvent_LimitLvLeft(GameObject btn)
    {
        _limitLv -= INTERAL_LIMIT_LV;
        FixLimitLv();
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        UnionModule.Instance.OnSendUpdateUnionSetting((UnionLimitType)_indexType, _limitLv,
        UnionModule.Instance.CheckUnionNotice(view.Ipt_Notice.value));
       // Debug.LogError(_indexType);
    }

    public void ButtonEvent_Mask(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(UnionSettingView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_Mask;
        UIEventListener.Get(view.Btn_ChangeIcon.gameObject).onClick = ButtonEvent_ChangeIcon;
        UIEventListener.Get(view.Btn_ChangeName.gameObject).onClick = ButtonEvent_ChangeName;
        UIEventListener.Get(view.Btn_LimitTypeRight.gameObject).onClick = ButtonEvent_LimitTypeRight;
        UIEventListener.Get(view.Btn_LimitTypeLeft.gameObject).onClick = ButtonEvent_LimitTypeLeft;
        UIEventListener.Get(view.Btn_LimitLvRight.gameObject).onClick = ButtonEvent_LimitLvRight;
        UIEventListener.Get(view.Btn_LimitLvLeft.gameObject).onClick = ButtonEvent_LimitLvLeft;
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
    }


}
