using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.Script.Common;
public class MallViewController : UIBase 
{
    private enum EShowSubType{
        None,
        Weapon,
        Ring,
        Necklace
    }

    public MallView view;
    private List<StoreItem> _storeItems = new List<StoreItem>();
    private bool _firstOpen = true;
    private EShowSubType _showSubType = EShowSubType.None;
    private EMallPageType _curPageType = EMallPageType.SkillBook;
    private Color _lblUnSelectColor = new Color(35 / 255f, 19 / 255f, 0f);
    private Color _lblSelectColor = new Color(1f, 1f, 197 / 255f);
    public override void Initialize()
    {
        if (view == null)
        {
            view = new MallView();
            view.Initialize();
        }
        BtnEventBinding();
        if (_firstOpen)
            ShowMall(EMallPageType.SkillBook);
        _firstOpen = false;
        view.Grd_MallContent.enabled = false;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenMall);
    }

    public override void Destroy()
    {
        view = null;
        _firstOpen = true;
        _storeItems.Clear();
        _showSubType = EShowSubType.None;
        _curPageType = EMallPageType.SkillBook;
    }

    private void ShowMall(EMallPageType type)
    {
        UpdateTypeSelectedState(type);
        _curPageType = type;
        UpdateSubTypeObj(GetSubType(type));
        ShowMall(ConfigManager.Instance.mMallConfig.GetMallGoodsByPageType(type));
    }

    private void ShowMall(List<MallGoodsData> list)
    {
        if (list == null)
        {
            return;
        }

        //view.ScrView_ContentScrollView.ResetPosition();
        view.UIPanel_ContentScrollView.transform.localPosition = new Vector3(65, -54, 0);
        view.UIPanel_ContentScrollView.clipOffset = Vector2.zero;

        //二次需求，填哪在哪
        for (int i = 0; i < list.Count; i++)
        {
            if (_storeItems.Count <= i)
            {
                _storeItems.Add(InstanceStoreItem());
            }
            _storeItems[i].InitItem(list[i]);
            //_storeItems[i].gameObject.name = string.Format("item{0:D3}", list[i].mPos);
            _storeItems[i].transform.localPosition = GetStoreItemPos(list[i].mPos);
            _storeItems[i].gameObject.SetActive(true);
        }

        for (int i = list.Count; i < _storeItems.Count; i++)
        {
            _storeItems[i].gameObject.SetActive(false);
        }
    }

    private Vector3 GetStoreItemPos(uint vpos)
    {
        int pos = (int) vpos;
        return new Vector3(((pos - 1) / 2) * view.Grd_MallContent.cellWidth, (pos % 2 - 1) * view.Grd_MallContent.cellHeight,0);
    }

    private StoreItem InstanceStoreItem()
    {
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_StoreItem, view.Grd_MallContent.transform);
        return go.AddComponent<StoreItem>();
    }

    private void SetSubSelectedMark()
    {
        Transform trans = null;

        switch (_curPageType)
        {
                case EMallPageType.WeaponWhite:
                {
                    trans = view.Btn_WeaponMatWhite.transform;
                    break;
                }
                case EMallPageType.WeaponGreen:
                {
                    trans = view.Btn_WeaponMatGreen.transform;
                    break;
                }
                case EMallPageType.WeaponBlue:
                {
                    trans = view.Btn_WeaponMatBlue.transform;
                    break;
                }
                case EMallPageType.RingBlue:
                {
                    trans = view.Btn_RingMatBlue.transform;
                    break;
                }
                case EMallPageType.RingGreen:
                {
                    trans = view.Btn_RingMatGreen.transform;
                    break;
                }
                case EMallPageType.RingWhite:
                {
                    trans = view.Btn_RingMatWhite.transform;
                    break;
                }
                case EMallPageType.NecklaceWhite:
                {
                    trans = view.Btn_NecklaceMatWhite.transform;
                    break;
                }
                case EMallPageType.NecklaceGreen:
                {
                    trans = view.Btn_NecklaceMatGreen.transform;
                    break;
                }
                case EMallPageType.NecklaceBlue:
                {
                    trans = view.Btn_NecklaceMatBlue.transform;
                    break;
                }
        }

        if(trans == null)
            return;

        view.Spt_SubMatSelected.transform.SetParent(trans);
        view.Spt_SubMatSelected.transform.localPosition = Vector3.zero;
        view.Spt_SubMatSelected.transform.localScale = Vector3.one;

    }

    private EShowSubType GetSubType(EMallPageType pageType)
    {
        switch (pageType)
        {
            case EMallPageType.RingBlue:
            case EMallPageType.RingGreen:
            case EMallPageType.RingWhite:
                {
                    return EShowSubType.Ring;
                }
            case EMallPageType.WeaponBlue:
            case EMallPageType.WeaponGreen:
            case EMallPageType.WeaponWhite:
                {
                    return EShowSubType.Weapon;
                }
            case EMallPageType.NecklaceBlue:
            case EMallPageType.NecklaceGreen:
            case EMallPageType.NecklaceWhite:
                {
                    return EShowSubType.Necklace;
                }
        }

        return EShowSubType.None;
    }

    private void UpdateTypeSelectedState(EMallPageType type)
    {
        if (type == _curPageType)
        {
            return;
        }
        UISprite spt = null;
        UILabel lbl = null;

        GetTypeSptAndLbl(type,out spt,out lbl);
        if (spt != null)
            CommonFunction.SetSpriteName(spt, GlobalConst.SpriteName.RANK_BTN_SELECT);
        if (lbl != null)
            lbl.color = _lblSelectColor;

        GetTypeSptAndLbl(_curPageType, out spt, out lbl);
        if (spt != null)
            CommonFunction.SetSpriteName(spt, GlobalConst.SpriteName.RANK_BTN_NOTSELECT);
        if (lbl != null)
            lbl.color = _lblUnSelectColor;
    }

    private void GetTypeSptAndLbl(EMallPageType type ,out UISprite spt,out UILabel lbl)
    {
        switch (type)
        {
            case EMallPageType.SkillBook:
                {
                    spt = view.Spt_BtnSkillBookButtonSprite;
                    lbl = view.Lbl_BtnSkillBookLabel;
                }                
                break;
            case EMallPageType.WeaponWhite:
            case EMallPageType.WeaponGreen:
            case EMallPageType.WeaponBlue:
                {
                    spt = view.Spt_BtnWeaponMatButtonSprite;
                    lbl = view.Lbl_BtnWeaponMatLabel;
                }                
                break;
            case EMallPageType.RingWhite:
            case EMallPageType.RingGreen:
            case EMallPageType.RingBlue:
                {
                    spt = view.Spt_BtnRingMatButtonSprite;
                    lbl = view.Lbl_BtnRingMatLabel;
                }
                break;
            case EMallPageType.HorseMat:
                {
                    spt = view.Spt_BtnOtherMatButtonSprite;
                    lbl = view.Lbl_BtnOtherMatLabel;
                }
                break;
            case EMallPageType.Necklace:
            case EMallPageType.NecklaceWhite:
            case EMallPageType.NecklaceGreen:
            case EMallPageType.NecklaceBlue:
                {
                    spt = view.Spt_BtnNecklaceMatButtonSprite;
                    lbl = view.Lbl_BtnNecklaceMatLabel;
                }
                break;
            case EMallPageType.CommonMat:
                {
                    spt = view.Spt_BtnCommonMatButtonSprite;
                    lbl = view.Lbl_BtnCommonMatLabel;
                }
                break;
            case EMallPageType.EquipBag:
                {
                    spt = view.Spt_BtnEquipBagButtonSprite;
                    lbl = view.Lbl_BtnEquipBagLabel;
                }
                break;
            default:
                spt = null;
                lbl = null;
                break;
        }
    }

    private void UpdateSubTypeObj(EShowSubType type)
    {
        if(type == EShowSubType.None && _showSubType == EShowSubType.Ring)
        {
            SetRingObj(false);
        }
        else if (type == EShowSubType.None && _showSubType == EShowSubType.Weapon)
        {
            SetWeaponObj(false);
        }
        else if (type == EShowSubType.None && _showSubType == EShowSubType.Necklace)
        {
            SetNecklaceObj(false);
        }

        else if (type == EShowSubType.Weapon && _showSubType == EShowSubType.None)
        {
            SetWeaponObj(true);
        }
        else if (type == EShowSubType.Ring && _showSubType == EShowSubType.None)
        {
            SetRingObj(true);
        }
        else if (type == EShowSubType.Necklace && _showSubType == EShowSubType.None)
        {
            SetNecklaceObj(true);
        }

        else if (type == EShowSubType.Ring && _showSubType == EShowSubType.Weapon)
        {
            SetRingObj(true);
            SetWeaponObj(false);
        }
        else if (type == EShowSubType.Ring && _showSubType == EShowSubType.Necklace)
        {
            SetNecklaceObj(false);
            SetRingObj(true);
        }

        else if (type == EShowSubType.Weapon && _showSubType == EShowSubType.Ring)
        {
            SetRingObj(false);
            SetWeaponObj(true);
        }
        else if (type == EShowSubType.Weapon && _showSubType == EShowSubType.Necklace)
        {
            SetNecklaceObj(false);
            SetWeaponObj(true);
        }

        else if (type == EShowSubType.Necklace && _showSubType == EShowSubType.Ring)
        {
            SetRingObj(false);
            SetNecklaceObj(true);
        }
        else if (type == EShowSubType.Necklace && _showSubType == EShowSubType.Weapon)
        {
            SetWeaponObj(false);
            SetNecklaceObj(true);
        }

        else
        {
            SetSubSelectedMark();
        }

        _showSubType = type;
    }

    private void SetRingObj(bool showOut)
    {
        view.Gobj_RingMatObj.SetActive(true);
        Scheduler.Instance.RemoveUpdator(RepositionTable);
        Scheduler.Instance.AddUpdator(RepositionTable);
        Scheduler.Instance.RemoveTimer(ItweenCompleted);
        Scheduler.Instance.AddTimer(0.35f, false, ItweenCompleted);
        
        iTween.ScaleTo(view.Gobj_RingMatObj.gameObject, 
            iTween.Hash("time", 0.25f,
            "y", showOut ? 1f : 0.001f,
            "easetype", iTween.EaseType.linear, "oncomplete", "ItweenCompleted"));
    }

    private void SetWeaponObj(bool showOut)
    {
        view.Gobj_WeaponMatObj.SetActive(true);
        Scheduler.Instance.RemoveUpdator(RepositionTable);
        Scheduler.Instance.AddUpdator(RepositionTable);
        Scheduler.Instance.RemoveTimer(ItweenCompleted);
        Scheduler.Instance.AddTimer(0.35f, false, ItweenCompleted);

        iTween.ScaleTo(view.Gobj_WeaponMatObj.gameObject,
            iTween.Hash("time", 0.25f,
            "y", showOut ? 1f : 0.001f,
            "easetype", iTween.EaseType.linear, "oncomplete", "ItweenCompleted"));
    }

    private void SetNecklaceObj(bool showOut)
    {
        view.Gobj_NecklaceMatObj.SetActive(true);
        Scheduler.Instance.RemoveUpdator(RepositionTable);
        Scheduler.Instance.AddUpdator(RepositionTable);
        Scheduler.Instance.RemoveTimer(ItweenCompleted);
        Scheduler.Instance.AddTimer(0.35f, false, ItweenCompleted);

        iTween.ScaleTo(view.Gobj_NecklaceMatObj.gameObject,
            iTween.Hash("time", 0.25f,
            "y", showOut ? 1f : 0.001f,
            "easetype", iTween.EaseType.linear, "oncomplete", "ItweenCompleted"));
    }

    private void RepositionTable()
    {
        view.UITable_UITable.Reposition();
    }

    private void ItweenCompleted()
    {
        if (!SubObjIsOpen(view.Gobj_WeaponMatObj))
        {
            view.Gobj_WeaponMatObj.SetActive(false);
        }
        if (!SubObjIsOpen(view.Gobj_RingMatObj))
        {
            view.Gobj_RingMatObj.SetActive(false);
        }
        if (!SubObjIsOpen(view.Gobj_NecklaceMatObj))
        {
            view.Gobj_NecklaceMatObj.SetActive(false);
        }
        RepositionTable();
        Scheduler.Instance.RemoveUpdator(RepositionTable);
        SetSubSelectedMark();
    }

    private bool SubObjIsOpen(GameObject go)
    {
        return go.transform.transform.localScale.y > 0.5f;
    }

    #region button event
    public void ButtonEvent_SkillBook(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.SkillBook);
    }

    public void ButtonEvent_WeaponMat(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (!SubObjIsOpen(view.Gobj_WeaponMatObj))
        {
            ShowMall(EMallPageType.WeaponWhite);
            SetWeaponObj(true);
        }
        else
        {
            SetWeaponObj(false);
        }
    }

    public void ButtonEvent_WeaponMatWhite(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.WeaponWhite);
    }

    public void ButtonEvent_WeaponMatGreen(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.WeaponGreen);
    }

    public void ButtonEvent_WeaponMatBlue(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.WeaponBlue);
    }

    public void ButtonEvent_RingMat(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (!SubObjIsOpen(view.Gobj_RingMatObj))
        {
            ShowMall(EMallPageType.RingWhite);
            SetRingObj(true);
        }
        else
        {
            SetRingObj(false);
        }
    }

    public void ButtonEvent_RingMatWhite(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.RingWhite);
    }

    public void ButtonEvent_RingMatGreen(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.RingGreen);
    }

    public void ButtonEvent_RingMatBlue(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.RingBlue);
    }

    public void ButtonEvent_NecklaceMat(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (!SubObjIsOpen(view.Gobj_NecklaceMatObj))
        {
            ShowMall(EMallPageType.NecklaceWhite);
            SetNecklaceObj(true);
        }
        else
        {
            SetNecklaceObj(false);
        }
    }

    public void ButtonEvent_NecklaceMatWhite(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.NecklaceWhite);
    }

    public void ButtonEvent_NecklaceMatGreen(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.NecklaceGreen);
    }

    public void ButtonEvent_NecklaceMatBlue(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.NecklaceBlue);
    }

    public void ButtonEvent_OtherMat(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowMall(EMallPageType.HorseMat);
    }

    public void ButtonEvent_CommonMat(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ShowMall(EMallPageType.CommonMat);
    }

    public void ButtonEvent_EquipBag(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ShowMall(EMallPageType.EquipBag);
    }
    #endregion
    public void CloseUI(GameObject go)
    {
        UISystem.Instance.CloseGameUI(MallView.UIName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SkillBook.gameObject).onClick = ButtonEvent_SkillBook;
        UIEventListener.Get(view.Btn_WeaponMat.gameObject).onClick = ButtonEvent_WeaponMat;
        UIEventListener.Get(view.Btn_WeaponMatWhite.gameObject).onClick = ButtonEvent_WeaponMatWhite;
        UIEventListener.Get(view.Btn_WeaponMatGreen.gameObject).onClick = ButtonEvent_WeaponMatGreen;
        UIEventListener.Get(view.Btn_WeaponMatBlue.gameObject).onClick = ButtonEvent_WeaponMatBlue;
        UIEventListener.Get(view.Btn_RingMat.gameObject).onClick = ButtonEvent_RingMat;
        UIEventListener.Get(view.Btn_RingMatWhite.gameObject).onClick = ButtonEvent_RingMatWhite;
        UIEventListener.Get(view.Btn_RingMatGreen.gameObject).onClick = ButtonEvent_RingMatGreen;
        UIEventListener.Get(view.Btn_RingMatBlue.gameObject).onClick = ButtonEvent_RingMatBlue;
        UIEventListener.Get(view.Btn_NecklaceMat.gameObject).onClick = ButtonEvent_NecklaceMat;
        UIEventListener.Get(view.Btn_NecklaceMatWhite.gameObject).onClick = ButtonEvent_NecklaceMatWhite;
        UIEventListener.Get(view.Btn_NecklaceMatGreen.gameObject).onClick = ButtonEvent_NecklaceMatGreen;
        UIEventListener.Get(view.Btn_NecklaceMatBlue.gameObject).onClick = ButtonEvent_NecklaceMatBlue;
        UIEventListener.Get(view.Btn_OtherMat.gameObject).onClick = ButtonEvent_OtherMat;
        UIEventListener.Get(view.Btn_CommonMat.gameObject).onClick = ButtonEvent_CommonMat;
        UIEventListener.Get(view.Btn_EquipBag.gameObject).onClick = ButtonEvent_EquipBag;
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = CloseUI;
    }


}
