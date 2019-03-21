using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;
public class RecruitViewController : UIBase
{
    private int _limitVipLv = 7;
    private RecruitView view;
    private Vector2 _propIconSize = new Vector2(45, 45);
    private Vector2 _goldIconSize = new Vector2(50, 50);
    private RecruitType _CurrentChooseType = RecruitType.RT_Brave;
    private long _braveRecuitCountTime = 0;
    private long _riotRecuitCountTime = 0;
    private string _braveFreeStr = "";
    private string _riotFreeStr = "";
    private bool _isFree = false;
    private long _tempTime = 0;
    private int ItemState = 1;
    public bool IsBlueItemOpen = false;
    private RecruitCardRotation CardRotation;
    private string _fromUI = "";
    public override void Initialize()
    {
        if (view == null)
        {
            view = new RecruitView();
            view.Initialize();
            BtnEventBinding();
            CardRotation = view._uiRoot.AddComponent<RecruitCardRotation>();
        }
        _fromUI = "";
        _limitVipLv = ConfigManager.Instance.mRecruitData.GetRecruitData(3, 1).LimitVipLV;
        PlayerData.Instance.UpdateVipEvent += InitMatchless;
        UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        //UpdateViewByIsGuide(false);
        InitPn();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        view.Gobj_matchlessCollider.enabled = true;
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_TOPFUNC);
        InitItemStateSort();
        UnionPrisonModule.Instance.SendQueryAltarRecruitReq();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenRecruit);
    }
    public void SetFromUI(string uiname)
    {
        _fromUI = uiname;
    }
    public void SetUnionPrison(QueryAltarRecruitResp vInfo)
    {
        if(vInfo.result == 0)
        {
            CommonFunction.SetSpriteName(this.view.Spt_IconTexture, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(vInfo.icon));
            this.view.Spt_LevelBG.gameObject.SetActive(true);
            this.view.Lbl_Label_Level.text = vInfo.union_level.ToString();
            this.view.Spt_state.gameObject.SetActive(vInfo.status == AltarFlameStatus.DEPEND_STATUS);
            float Num = ((float)vInfo.all_up_probability / 10000f);
            if (ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.UnionPrison, false))
            {
                this.view.Spt_Mask_UnionPrison.gameObject.SetActive(false);
                this.view.Lbl_Dec.text = "";
                if (vInfo.status == AltarFlameStatus.NORMAL_STATUS)
                {
                    if (vInfo.have_depend > 0)
                    {
                        this.view.Lbl_Num.text = string.Format(ConstString.UNION_PRISON_GREEN, Num);
                    }
                    else
                        this.view.Lbl_Num.text = string.Format(ConstString.UNION_PRISON, Num);
                }
                else
                    this.view.Lbl_Num.text = string.Format(ConstString.UNION_PRISON_RED, Num);

            }
            else
            {
                this.view.Spt_state.gameObject.SetActive(false);
                this.view.Spt_Mask_UnionPrison.gameObject.SetActive(true);
                this.view.Lbl_Dec.text = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.UnionPrison, ConstString.GATE_ESCORT_LOCKTIP_UNLOCK);
                this.view.Lbl_Num.text = string.Format(ConstString.UNION_PRISON_RED, 0);
                this.view.Lbl_Dec.color = new Color(255f/255,214f/255,101f/255);
                this.view.Lbl_Dec.width = 70;
            }
        }
        else
        {
            this.view.Spt_LevelBG.gameObject.SetActive(false);
            this.view.Spt_state.gameObject.SetActive(false);
            this.view.Spt_Mask_UnionPrison.gameObject.SetActive(true);
            this.view.Lbl_Dec.text = ConstString.NOUNION;
            this.view.Lbl_Dec.color = new Color(0.56f,0.56f,0.56f);
            this.view.Lbl_Num.text = string.Format(ConstString.UNION_PRISON, 0);
            CommonFunction.SetSpriteName(this.view.Spt_IconTexture, GlobalConst.SpriteName.QUESTION);
        }
    }
    public override void ReturnTop()
    {
        if (CardRotation.isMoving)
        {
            return;
        }
        //if (GuideManager.Instance.GuideIsRunning())
        //{ InitItemSort(true ); }
        //else { InitItemSort(false); }
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_TOPFUNC);
        InitItemStateSort();
        //InitCloseEffect();
    }
    public void InitItemStateSort()
    {
        InitItemSort(ItemState);
        CardRotation.SetRotationPos(ItemState);

    }
    public void InitCloseEffect()
    {
        InitItemSort(ItemState);
    }
    public void InitItemSort(int _Color)
    {
        if (_Color == 1)//绿色
        {
            if (CardRotation._itemIndex_3 == 1)
            {
                CardRotation.Panel_BraveLab.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_BraveFreeSp.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_Brave.depth = CardRotation.MaxDepth;
            }
            else
            {
                CardRotation.Panel_BraveLab.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_BraveFreeSp.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_Brave.depth = CardRotation.MinDepth;
            }
            if (CardRotation._itemIndex_2 == 1)
            {
                CardRotation.Panel_MatchlessLab.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_PutDownRiot.depth = CardRotation.MaxDepth;
            }
            else
            {
                CardRotation.Panel_MatchlessLab.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_PutDownRiot.depth = CardRotation.MinDepth;
                //Debug.LogError("CardRotation.Panel_PutDownRiot.depth " + CardRotation.Panel_PutDownRiot.depth);
            }
            if (CardRotation._itemIndex_1 == 1)
            {
                CardRotation.Panel_PutDownRiotLab.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_PDRFreeSp.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_MatchlessPn.depth = CardRotation.MaxDepth;
            }
            else
            {
                CardRotation.Panel_PutDownRiotLab.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_PDRFreeSp.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_MatchlessPn.depth = CardRotation.MinDepth;

            }
            //view.TopFuncPanel.sortingOrder = 22;
        }
        else if (_Color == 2)//蓝色
        {
            if (CardRotation._itemIndex_1 == CardRotation.ItemIndex)
            {
                CardRotation.Panel_Brave.depth = CardRotation.MaxDepth;
                CardRotation.Panel_BraveLab.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_BraveFreeSp.sortingOrder = CardRotation.MaxSortOder2;
            }
            else
            {
                CardRotation.Panel_BraveLab.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_BraveFreeSp.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_Brave.depth = CardRotation.MinDepth;

            }
            if (CardRotation._itemIndex_2 == CardRotation.ItemIndex)
            {
                CardRotation.Panel_PutDownRiotLab.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_PDRFreeSp.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_MatchlessPn.depth = CardRotation.MaxDepth;


            }
            else
            {
                CardRotation.Panel_PutDownRiotLab.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_PDRFreeSp.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_MatchlessPn.depth = CardRotation.MinDepth;


            }
            if (CardRotation._itemIndex_3 == CardRotation.ItemIndex)
            {
                CardRotation.Panel_MatchlessLab.sortingOrder = CardRotation.MaxSortOder2;
                CardRotation.Panel_PutDownRiot.depth = CardRotation.MaxDepth;

            }
            else
            {
                CardRotation.Panel_MatchlessLab.sortingOrder = CardRotation.MinSortOder2;
                CardRotation.Panel_PutDownRiot.depth = CardRotation.MinDepth;

            }

        }
        else if (_Color == 3)//紫色
        {

        }
    }
    public void InitPn()
    {
        IsBlueItemOpen = false;
        ItemState = 1;
        InitBravePn();
        InitRiotPn();
        InitMatchless();
    }

    public void InitMatchless()
    {
        if (PlayerData.Instance._VipLv < _limitVipLv)
        {
            view.Gobj_lizi.SetActive(false);
            SetMatchlessState(false);
        }
        else
        {
            view.Gobj_lizi.SetActive(true);
            SetMatchlessState(true);
        }
    }

    public void InitBravePn()
    {
        if (PlayerData.Instance.RecruitFreeCount > 0)
        {
            SetBraveFreeVisible(true);
            if (!CommonFunction.IsEndTime((long)PlayerData.Instance.LastBraveRecruitTime))
            {
                _braveRecuitCountTime = CommonFunction.GetLfetTime((long)PlayerData.Instance.LastBraveRecruitTime);
                BraveCountTime();
                Scheduler.Instance.AddTimer(1.0F, true, BraveCountTime);
                view.Spt_BraveFreeSp.gameObject.SetActive(false);
                view.Lbl_BraveFreeTimeLb.gameObject.SetActive(true);
            }
            else
            {
                ItemState = 1;

                _braveRecuitCountTime = 0;
                BraveCountTime();
            }
            SetBraveFreeCount();
        }
        else
        {
            SetBraveFreeVisible(false);
        }

        if ((PlayerData.Instance.RecruitFreeCount > 0 && _braveRecuitCountTime != 0 && PlayerData.Instance.GetItemCountByID(GlobalCoefficient.RecruitItemIDList[0]) > 0)
            || (PlayerData.Instance.RecruitFreeCount <= 0 && PlayerData.Instance.GetItemCountByID(GlobalCoefficient.RecruitItemIDList[0]) > 0))
        {
            ItemState = 1;
            view.Spt_BraveFreeSp.gameObject.SetActive(true);

            //SetBraveFreeVisible(true);
            CommonFunction.SetSpriteName(view.Spt_BraveFreeSp, GlobalConst.SpriteName.RecruitCard);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_BraveFreeSp, GlobalConst.SpriteName.RecruitFree);
        }
    }

    public void InitRiotPn()
    {
        if (!CommonFunction.IsEndTime((long)PlayerData.Instance.LastRiotRecruitTime))
        {
            _riotRecuitCountTime = CommonFunction.GetLfetTime((long)PlayerData.Instance.LastRiotRecruitTime);
            RiotCountTime();
            Scheduler.Instance.AddTimer(1.0F, true, RiotCountTime);
        }
        else
        {
            _riotRecuitCountTime = 0;
            RiotCountTime();
        }
    }

    public void UpdateChooseFree()
    {
        switch (_CurrentChooseType)
        {
            case RecruitType.RT_Brave:
                if (PlayerData.Instance.RecruitFreeCount > 0)
                {
                    view.Gobj_OnceFree.SetActive(true);
                    _tempTime = _braveRecuitCountTime;
                    view.Lbl_OnceFreeLb.text = _braveFreeStr.ToString();
                    if (_tempTime <= 0)
                    {
                        _isFree = true;
                        view.Lbl_OnceFreeLb.gameObject.SetActive(false);
                        view.Spt_OnceFreeSP.gameObject.SetActive(true);
                    }
                    else
                    {
                        _isFree = false;
                        view.Lbl_OnceFreeLb.gameObject.SetActive(true);
                        view.Spt_OnceFreeSP.gameObject.SetActive(false);
                        UpdateChooseFreeLb();
                        Scheduler.Instance.AddTimer(1.0F, true, UpdateChooseFreeLb);
                    }
                }
                else
                {
                    _isFree = false;
                    view.Gobj_OnceFree.SetActive(false);
                }
                break;
            case RecruitType.RT_PutDownRiot:
                view.Gobj_OnceFree.SetActive(true);
                _tempTime = _riotRecuitCountTime;
                view.Lbl_OnceFreeLb.text = _riotFreeStr.ToString();
                if (_tempTime <= 0)
                {
                    _isFree = true;
                    view.Lbl_OnceFreeLb.gameObject.SetActive(false);
                    view.Spt_OnceFreeSP.gameObject.SetActive(true);
                }
                else
                {
                    _isFree = false;
                    view.Lbl_OnceFreeLb.gameObject.SetActive(true);
                    view.Spt_OnceFreeSP.gameObject.SetActive(false);
                    UpdateChooseFreeLb();
                    Scheduler.Instance.AddTimer(1.0F, true, UpdateChooseFreeLb);
                }
                break;
            default:
                _isFree = false;
                view.Gobj_OnceFree.SetActive(false);
                break;

        }

    }

    private void SetBraveFreeVisible(bool isVisible)
    {
        view.Gobj_BraveFree.SetActive(isVisible);
        view.Spt_BraveFreeSp.gameObject.SetActive(isVisible);
    }

    private void BraveCountTime()
    {
        if (_braveRecuitCountTime > 0)
        {
            _braveFreeStr = string.Format(ConstString.RECRUITTIMELATERFREE, CommonFunction.GetTimeString(_braveRecuitCountTime));
            _braveRecuitCountTime--;
            SetBraveFreeInfo(_braveFreeStr);
        }
        else
        {
            view.Spt_BraveFreeSp.gameObject.SetActive(true);
            view.Lbl_BraveFreeTimeLb.gameObject.SetActive(false);
            Scheduler.Instance.RemoveTimer(BraveCountTime);
        }

    }

    public void SetBraveFreeCount()
    {
        view.Lbl_BraveFreeLb.text = string.Format(ConstString.RECRUITFREECOUNT, PlayerData.Instance.RecruitFreeCount);
    }

    private void RiotCountTime()
    {
        if (_riotRecuitCountTime > 0)
        {
            _riotFreeStr = string.Format(ConstString.RECRUITTIMELATERFREE, CommonFunction.GetTimeString(_riotRecuitCountTime));
            _riotRecuitCountTime--;
            view.Gobj_PDRFree.SetActive(true);
            SetRiotFreeInfo(_riotFreeStr);

            if (PlayerData.Instance.GetItemCountByID(GlobalCoefficient.RecruitItemIDList[1]) > 0)
            {
                view.Spt_PDRFreeSp.gameObject.SetActive(true);
                CommonFunction.SetSpriteName(view.Spt_PDRFreeSp, GlobalConst.SpriteName.RecruitCard);

                if (!IsBlueItemOpen)
                {
                    ItemState = 2;
                    IsBlueItemOpen = true;
                }
            }
            else
            {
                view.Spt_PDRFreeSp.gameObject.SetActive(false);
            }
        }
        else
        {
            ItemState = 2;

            SetRiotFreeInfo("");
            view.Gobj_PDRFree.SetActive(false);
            view.Spt_PDRFreeSp.gameObject.SetActive(true);
            CommonFunction.SetSpriteName(view.Spt_PDRFreeSp, GlobalConst.SpriteName.RecruitFree);
            Scheduler.Instance.RemoveTimer(RiotCountTime);
        }
    }

    private void UpdateChooseFreeLb()
    {
        switch (_CurrentChooseType)
        {
            case RecruitType.RT_Brave:
                view.Lbl_OnceFreeLb.text = _braveFreeStr.ToString();
                break;
            case RecruitType.RT_PutDownRiot:
                view.Lbl_OnceFreeLb.text = _riotFreeStr.ToString();
                break;

        }
        if (_tempTime <= 0)
        {
            view.Lbl_OnceFreeLb.gameObject.SetActive(true);
            view.Spt_OnceFreeSP.gameObject.SetActive(false);
            _isFree = true;
            Scheduler.Instance.RemoveTimer(UpdateChooseFreeLb);
            UpdateCurrency();
        }

    }

    private void SetBraveFreeInfo(string str)
    {
        view.Lbl_BraveFreeTimeLb.text = str;
    }

    private void SetRiotFreeInfo(string str)
    {
        view.Lbl_PDRFreeLb.text = str;
    }

    private void SetMatchlessState(bool isOpen)
    {
        view.Spt_MatchlessSp.gameObject.SetActive(false);
        if (isOpen)
        {
            view.Lbl_UnlockConditionsLb.gameObject.SetActive(false);
            // view.Spt_gobj_Matchless.color = Color.white;
            if (PlayerData.Instance.GetItemCountByID(GlobalCoefficient.RecruitItemIDList[2]) > 0)
            {
                view.Spt_MatchlessSp.gameObject.SetActive(true);
                CommonFunction.SetSpriteName(view.Spt_MatchlessSp, GlobalConst.SpriteName.RecruitCard);
                ItemState = 3;


            }
        }
        else
        {
            view.Lbl_UnlockConditionsLb.text = string.Format(ConstString.RECRUITLIMITVIPLEVEL, _limitVipLv);
            //view.Lbl_UnlockConditionsLb.text = string.Format(ConstString.RECRUITLIMITVIPLEVEL, LIMITVIPLEVEL);
            view.Lbl_UnlockConditionsLb.gameObject.SetActive(true);
            //view.Spt_gobj_Matchless.color = Color.gray;
        }
    }

    private void SetChooseCountPn(bool isVisible)
    {
        if (!isVisible)
        {
            Scheduler.Instance.RemoveTimer(UpdateChooseFreeLb);
            CardRotation.ResetMousePos();
            CardRotation.isChooseCountPnOpen = false;

        }
        else
        {
            CardRotation.ResetMousePos();
            CardRotation.isChooseCountPnOpen = true;

        }
        view.UIPanel_ChooseCountPn.gameObject.SetActive(isVisible);


    }

    private void OpenChooseCountPn(RecruitType type)
    {

        _CurrentChooseType = type;
        switch (type)
        {
            case RecruitType.RT_Brave:

                view.Spt_ChooseCountTitle.text = ConstString.RECRUIT_BRAVENAME;
                view.Lbl_ChooseTenDesc.text = ConstString.RECRUIT_CHOOSETEN_BRAVE;
                break;

            case RecruitType.RT_PutDownRiot:
                view.Spt_ChooseCountTitle.text = ConstString.RECRUIT_PUTDOWNRIOT;
                view.Lbl_ChooseTenDesc.text = ConstString.RECRUIT_CHOOSETEN_PUTDOWNROIT;
                break;

            case RecruitType.RT_Matchless:
                view.Spt_ChooseCountTitle.text = ConstString.RECRUIT_MATCHLESS;
                view.Lbl_ChooseTenDesc.text = ConstString.RECRUIT_CHOOSETEN_MATCHLESS;
                break;

        }
        view.Gobj_matchlessCollider.enabled = false;//
        SetChooseCountPn(true);// PlayOpenChooseCountPnAnim();
        UpdateChooseFree();
        RecruitData data = ConfigManager.Instance.mRecruitData.GetRecruitData((int)_CurrentChooseType, 0);
        SetCurrentInfo(data);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenRecruitSecond);

    }

    public void SetCurrentInfo(RecruitData data)
    {
        bool isprop = false;
        view.Spt_Dis.gameObject.SetActive(true);
        Item item = PlayerData.Instance.GetOwnItemByID((uint)data.PropID);
        ItemInfo iteminfo = null;
        if (item != null && item.num >= data.NeedPropCount)
        {
            isprop = true;
            iteminfo = ConfigManager.Instance.mItemData.GetItemInfoByID((uint)data.PropID);
        }

        if (_isFree)
        {
            view.Lbl_CurrencyNum.text = ConstString.FREE;
            if (isprop && (item.num >= data.NeedPropCount * 10))
            {
                view.Lbl_TenCurrencyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
                                                             data.NeedPropCount * 10);// "X" + data.NeedPropCount * 10;
                view.Spt_Dis.gameObject.SetActive(false);
                CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, iteminfo.icon);
            }
            else
            {
                view.Lbl_TenCurrencyNum.text = (data.CurrencyCount * 9).ToString();
            }
            switch (data.CurrencyType)
            {
                case ECurrencyType.Gold:
                    CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, GlobalConst.SpriteName.Gold);
                    if (!isprop || (item.num < data.NeedPropCount * 10))
                    {
                        CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GlobalConst.SpriteName.Gold);
                    }
                    break;
                case ECurrencyType.Diamond:
                    CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, GlobalConst.SpriteName.Diamond);
                    if (!isprop || (item.num < data.NeedPropCount * 10))
                        CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GlobalConst.SpriteName.Diamond);
                    break;

            }

        }
        else if (isprop)
        {

            view.Lbl_CurrencyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
                                                             data.NeedPropCount);// "X" + data.NeedPropCount;
            CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, iteminfo.icon);
            if (item.num >= data.NeedPropCount * 10)
            {

                view.Lbl_TenCurrencyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, item.num,
                                                             data.NeedPropCount * 10);//"X" + data.NeedPropCount * 10;
                view.Spt_Dis.gameObject.SetActive(false);
                CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, iteminfo.icon);
            }
            else
            {
                switch (data.CurrencyType)
                {
                    case ECurrencyType.Gold:
                        CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GlobalConst.SpriteName.Gold);
                        break;
                    case ECurrencyType.Diamond:
                        CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GlobalConst.SpriteName.Diamond);
                        break;
                }
                view.Lbl_TenCurrencyNum.text = (data.CurrencyCount * 9).ToString();
            }
        }
        else
        {
            view.Lbl_CurrencyNum.text = data.CurrencyCount.ToString();
            view.Lbl_TenCurrencyNum.text = (data.CurrencyCount * 9).ToString();
            switch (data.CurrencyType)
            {
                case ECurrencyType.Gold:
                    CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, GlobalConst.SpriteName.Gold);
                    CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GlobalConst.SpriteName.Gold);
                    break;
                case ECurrencyType.Diamond:
                    CommonFunction.SetSpriteName(view.Spt_CurrencyIcon, GlobalConst.SpriteName.Diamond);
                    CommonFunction.SetSpriteName(view.Spt_TenCurrencyIcon, GlobalConst.SpriteName.Diamond);
                    break;
            }

        }
        if (isprop)
        {
            SetWidgetSize(view.Spt_CurrencyIcon, _propIconSize);
            SetWidgetSize(view.Spt_TenCurrencyIcon, _propIconSize);
        }
        else
        {
            SetWidgetSize(view.Spt_CurrencyIcon, _goldIconSize);
            SetWidgetSize(view.Spt_TenCurrencyIcon, _goldIconSize);
        }
    }

    public void UpdateCurrency()
    {
        view.Lbl_CurrencyNum.text = ConstString.FREE;
    }

    public void ButtonEvent_BackBtn(GameObject btn)
    {
        CardRotation.StopMove();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(RecruitView.UIName);
        if(!string.IsNullOrEmpty(_fromUI))
            UISystem.Instance.ShowGameUI(_fromUI);
        //GuideManager.Instance.CheckTrigger(GuideTrigger.OpenMainCity);

    }

    public void ButtonEvent_CloseBtn(GameObject btn)
    {
        CardRotation.StopMove();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        SetChooseCountPn(false);

        view.Gobj_matchlessCollider.enabled = true;
    }

    public void ButtonEvent_ChooseBrave(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));

        OpenChooseCountPn(RecruitType.RT_Brave);
    }

    public void ButtonEvent_ChooseGood(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));

        OpenChooseCountPn(RecruitType.RT_PutDownRiot);
    }

    public void ButtonEvent_ChooseGod(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        if (PlayerData.Instance._VipLv < _limitVipLv)
        {
            CommonFunction.ShowVipLvNotEnoughTip(String.Format(ConstString.RECRUIT_VIP_LIMIT, _limitVipLv));
            return;
        }
        OpenChooseCountPn(RecruitType.RT_Matchless);
    }

    public void ButtonEvent_Single(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        if (PlayerData.Instance.IsSoldierFull())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RECRUIT_LIMIT_EMAX);
            return;
        }
        view.Gobj_matchlessCollider.enabled = true;
        SendSigleRecruit();
    }

    public void ButtonEvent_Multiple(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        if (PlayerData.Instance.IsSoldierFull(10))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RECRUIT_LIMIT_EMAX);
            return;
        }
        view.Gobj_matchlessCollider.enabled = true;
        SendMultipleRecruit();
    }

    private void SendSigleRecruit()
    {
        RecruitModule.Instance.SendOneRecruit((int)_CurrentChooseType);
    }

    private void SendMultipleRecruit()
    {
        RecruitModule.Instance.SendTenRecruit((int)_CurrentChooseType);
    }

    public void ReceiveOneRecruit(OneRecruitResp data)
    {
        if (data.result == 0)
            SetChooseCountPn(false);

        List<Soldier> _soldierData = new List<Soldier>();
        if (data.soldier_info != null)
            _soldierData.Add(PlayerData.Instance._SoldierDepot.oneAdd(data.soldier_info));
        PlayerData.Instance.UpdateItem(data.update_chip);
        InitPn();
        RecruitData _recruitData = ConfigManager.Instance.mRecruitData.GetRecruitData((int)_CurrentChooseType, 0);
        UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECRUITRESULTVIEW);
        if (data.recruit_type == RecruitType.RT_Brave)
        {
            LocalNotificationManager.Instance.GreenAgainst();
        }
        if (data.recruit_type == RecruitType.RT_PutDownRiot)
        {
            LocalNotificationManager.Instance.BlueAgainst();
        }
        if (data.over_chip != null && data.over_chip.Count > 0)
        {
            data.update_chip.AddRange(data.over_chip);
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.SoldierChipMaxNum);
        }
        UISystem.Instance.RecruitResultView.ShowTenAnim(_soldierData, data.update_chip, _recruitData, ButtonEvent_Single, ButtonEvent_Multiple);

    }

    public void ReceiveMultipleRecruit(MultipleRecruitResp data)
    {
        if (data.result == 0)
            SetChooseCountPn(false);
        List<Soldier> _soldierData = new List<Soldier>();
        if (data.soldier_info != null)
            _soldierData.AddRange(PlayerData.Instance._SoldierDepot.multipleAdd(data.soldier_info));
        PlayerData.Instance.UpdateItem(data.update_chip);
        InitPn();
        RecruitData _recruitData = ConfigManager.Instance.mRecruitData.GetRecruitData((int)_CurrentChooseType, 0);
        UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECRUITRESULTVIEW);
        if (data.over_chip != null && data.over_chip.Count > 0)
        {
            data.update_chip.AddRange(data.over_chip);
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.SoldierChipMaxNum);
        }
        UISystem.Instance.RecruitResultView.ShowTenAnim(_soldierData, data.update_chip, _recruitData, ButtonEvent_Single, ButtonEvent_Multiple);
    }

    public void SetWidgetSize(UIWidget widget, Vector2 size)
    {
        widget.width = (int)size.x;
        widget.height = (int)size.y;
    }

    public override void Uninitialize()
    {
        Scheduler.Instance.RemoveTimer(BraveCountTime);
        Scheduler.Instance.RemoveTimer(RiotCountTime);
        Scheduler.Instance.RemoveTimer(UpdateChooseFreeLb);
        PlayerData.Instance.UpdateVipEvent -= InitMatchless;
    }

    public override void Destroy()
    {
        Scheduler.Instance.RemoveTimer(BraveCountTime);
        Scheduler.Instance.RemoveTimer(RiotCountTime);
        Scheduler.Instance.RemoveTimer(UpdateChooseFreeLb);
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_BackBtn.gameObject).onClick = ButtonEvent_BackBtn;
        UIEventListener.Get(view.Btn_CloseBtn.gameObject).onClick = ButtonEvent_CloseBtn;
        UIEventListener.Get(view.Gobj_Brave).onClick = ButtonEvent_ChooseBrave;
        UIEventListener.Get(view.Gobj_PutDownRiot).onClick = ButtonEvent_ChooseGood;
        UIEventListener.Get(view.Gobj_matchless).onClick = ButtonEvent_ChooseGod;
        UIEventListener.Get(view.Spt_ChooseOnce.gameObject).onClick = ButtonEvent_Single;
        UIEventListener.Get(view.Spt_ChooseTen.gameObject).onClick = ButtonEvent_Multiple;

    }
    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}
    //public void PlayOpenChooseCountPnAnim()
    //{
    //    view.ChooseCountPn_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.ChooseCountPn_TScale.Restart();
    //    view.ChooseCountPn_TScale.PlayForward();
    //}


    /// <summary>
    /// 卡牌排序
    /// </summary>
    /// <param name="ItemColor">True：1绿色 2蓝色 3紫色</param>
    public void UpdateViewByIsGuide(int ItemColor)
    {
        ItemState = ItemColor;
        InitItemSort(ItemState);
        CardRotation.SetRotationPos(ItemState);

    }
    /// <summary>
    /// 开启向右划
    /// </summary>
    /// <param name="IsScrollRight">False:禁止向右滑动</param>
    public void OpenGuideScrollRight(bool IsScrollRight)
    {
        CardRotation.isNewbieGuide = IsScrollRight;
    }
    /// <summary>
    /// 开启向左划
    /// </summary>
    /// <param name="IsScrollLeft">False:禁止向左滑动</param>
    public void OpenGuideScrollLeft(bool IsScrollLeft)
    {
        CardRotation.isScrollRight = IsScrollLeft;
    }
}
