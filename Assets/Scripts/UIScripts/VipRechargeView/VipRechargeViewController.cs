using System.Collections.Generic;
using Assets.Script.Common;
using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class VipRechargeViewController : UIBase
{
    public enum VipRechargeState
    {
        Vip,
        Recharge,
    }
    public VipRechargeView view;
    private Dictionary<uint, RechargeItem> _rechargeItems = new Dictionary<uint, RechargeItem>();
    private bool _initializedVipPris = false;
    private VipRechargeState _curState;
    public VipRechargeState CurState
    {
        get { return _curState; }
    }
    private int _curVipPage = 0;
    private int _maxVipPage = 0;
    private bool _pageMove_start = false;
    private float _pageMove_factor = 0;
    private Vector3 _pageMove_orig;
    private Vector3 _pageMove_dest;
    private float _pageMove_time = 0.5f;

    private bool _newVipPage = true;
    private CommonItemData[] _curGift = new CommonItemData[6];

    public override void Initialize()
    {
        if (view == null)
        {
            view = new VipRechargeView();
            view.Initialize();
            BtnEventBinding();
        }
        InitVipLv(); //PlayOpenVipRechargeAnim();
        //PlayerDataManager.Instance.OnRechargeInfoUpdateEvent += RefrashUI;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        if(_newVipPage)
            _initializedVipPris = true;
        else
        {
            Scheduler.Instance.AddUpdator(UpdateVipPage);
        }
        //view.Btn_GetDay.duration = 0;
        //view.Btn_GetLvUp.duration = 0;
        view.Btn_GetDay.enabled = false;
        view.Btn_GetLvUp.enabled = false;
    }
    //public void PlayOpenVipRechargeAnim()
    //{
    //    view.VipRecharge_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.VipRecharge_TScale.Restart();
    //    view.VipRecharge_TScale.PlayForward();
    //}
    public override void Destroy()
    {
        view = null;
        if (_rechargeItems != null)
            _rechargeItems.Clear();
        _initializedVipPris = false;
        _curVipPage = 0;
        _maxVipPage = 0;
        _pageMove_start = false;
        _pageMove_factor = 0;
    }

    public void ShowRecharge()
    {
        SwitchState(VipRechargeState.Recharge);
    }
    public void ShowVipPrivilege(int vip = -1)
    {
        if (vip == -1)
            vip = PlayerData.Instance._VipLv;

        SwitchState(VipRechargeState.Vip);
        ShowVipPrivilegePage(vip, true);
    }
    private void SwitchState(VipRechargeState state)
    {
        _curState = state;
        if (state == VipRechargeState.Vip)
        {
            view.Spt_BtnVipRechargeFG.text = ConstString.FORMAT_RECHARGE;//GlobalConst.SpriteName.BtnRecharge;
            view.Spt_Title.text = ConstString.FORMAT_PRIVILEGE;
            ShowVipPrivileges();
        }
        else
        {
            view.Spt_BtnVipRechargeFG.text = ConstString.FORMAT_PRIVILEGE;//GlobalConst.SpriteName.BtnVipPrivilege;
            view.Spt_Title.text = ConstString.FORMAT_RECHARGE;
            ShowRechargeItems();
        }
    }

    public void RefreshGetBtn()
    {
        SetGetBtnState();
    }

    private void SetGiftData()
    {
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(
            ConfigManager.Instance.mVipConfig.GetVipDataByLv((uint) _curVipPage).LvUpGift);
        int count = Mathf.Min(list.Count, 3);
        for (int i = 0; i < count; i++)
        {
            _curGift[i] = list[i];
        }

        List<CommonItemData> list2 = CommonFunction.GetCommonItemDataList(
            ConfigManager.Instance.mVipConfig.GetVipDataByLv((uint)_curVipPage).DayGift);
        count = Mathf.Min(list2.Count, 3);
        for (int i = 0; i < count; i++)
        {
            _curGift[i + 3] = list2[i];
        }
        SetProp();
    }
    
    private void SetProp()
    {
        for (int i = 0; i < _curGift.Length; i++)
        {
            if (!GiftIsNull(i))
            {
                view.Gobj_Props[i].SetActive(true);
                CommonFunction.SetSpriteName(view.Spt_PrepIcons[i], _curGift[i].Icon);
                CommonFunction.SetQualitySprite(view.Spt_PrepQualitys[i], _curGift[i].Quality);
                view.Lbl_PrepNums[i].text = string.Format(ConstString.FORMAT_NUM_X, _curGift[i].Num);
                if(_curGift[i].Type == IDType.Prop && _curGift[i].SubType == ItemTypeEnum.SoldierChip)
                {
                    CommonFunction.SetSpriteName(view.Spt_PrepMarks[i], GlobalConst.SpriteName.MarkSoldierChip);
                    view.Spt_PrepMarks[i].gameObject.SetActive(true);
                    view.Spt_PrepMarks[i].width = 32;
                    view.Spt_PrepMarks[i].height = 28;
                    view.Spt_PrepMarks[i].transform.localPosition = new Vector3(-27,29,0);
                }
                else if (_curGift[i].Type == IDType.Prop && _curGift[i].SubType == ItemTypeEnum.EquipChip)
                {
                    CommonFunction.SetSpriteName(view.Spt_PrepMarks[i], GlobalConst.SpriteName.MarkEquipChip);
                    view.Spt_PrepMarks[i].gameObject.SetActive(true);
                    view.Spt_PrepMarks[i].width = 30;
                    view.Spt_PrepMarks[i].height = 36;
                    view.Spt_PrepMarks[i].transform.localPosition = new Vector3(-28, 25, 0);
                }
                else
                {
                    view.Spt_PrepMarks[i].gameObject.SetActive(false);
                }
            }
            else
            {
                view.Gobj_Props[i].SetActive(false);
            }
        }
    }

    private void PressGiftProp(GameObject go,bool press)
    {
        string name = go.transform.parent.gameObject.name;
        //预制名为 prop1
        int index = int.Parse(name.Substring(4, 1));
        if (GiftIsNull(index - 1))
        {
            view.Gobj_Props[index - 1].SetActive(false);
            return;
        }

        HintManager.Instance.SeeDetail(go,press,_curGift[index -1].ID);
    }

    private bool GiftIsNull(int index)
    {
        return _curGift[index] == null || _curGift[index].ID == 0;
    }

    private void SetGetBtnState()
    {
        VipRewardInfo info = PlayerData.Instance.VipRewardInfo;
        if (PlayerData.Instance._VipLv >= _curVipPage)
        {
            if (info.level.Contains(_curVipPage))
            {
                //view.Lbl_BtnGetLvUp.text = ConstString.GATE_HADGETAWARDS;
                SetBtnState(view.Btn_GetLvUp, view.Lbl_BtnGetLvUp, view.Spt_BtnGetLvUp, false);
            }
            else
            {
                //view.Lbl_BtnGetLvUp.text = ConstString.EMAIL_RECEIVE;
                SetBtnState(view.Btn_GetLvUp, view.Lbl_BtnGetLvUp, view.Spt_BtnGetLvUp, true);
            }
        }
        else
        {
            //view.Btn_GetLvUp.GetComponent<BoxCollider>().enabled = false;
            //view.Lbl_BtnGetLvUp.text = ConstString.EMAIL_RECEIVE;
            //view.Spt_BtnGetLvUp.color = Color.black;
            SetBtnState(view.Btn_GetLvUp, view.Lbl_BtnGetLvUp, view.Spt_BtnGetLvUp, false);
            view.Lbl_BtnGetLvUp.text = ConstString.EMAIL_RECEIVE;
        }
        
        if (PlayerData.Instance._VipLv == _curVipPage)
        {
            DateTime now = CommonFunction.GetDateTime(Main.mTime);
            DateTime gotTime = CommonFunction.GetDateTime((long) info.draw_time);
            bool passDay = now.Year > gotTime.Year || now.Month > gotTime.Month || now.Day > gotTime.Day;
            if (info.draw_time == 0 || passDay)
            {
                //view.Lbl_BtnGetDay.text = ConstString.EMAIL_RECEIVE;
                SetBtnState(view.Btn_GetDay, view.Lbl_BtnGetDay, view.Spt_BtnGetDay, true);
            }
            else
            {
                //view.Lbl_BtnGetDay.text = ConstString.GATE_HADGETAWARDS;
                SetBtnState(view.Btn_GetDay, view.Lbl_BtnGetDay, view.Spt_BtnGetDay, false);
            }
        }
        else
        {
            SetBtnState(view.Btn_GetDay, view.Lbl_BtnGetDay, view.Spt_BtnGetDay,false);
            view.Lbl_BtnGetDay.text = ConstString.EMAIL_RECEIVE;
        }
    }

    private void SetBtnState(UIButton btn,UILabel btnLbl,UISprite btnSp,bool state)
    {
        btn.GetComponent<BoxCollider>().enabled = state;
        btnLbl.text = state ? ConstString.EMAIL_RECEIVE : ConstString.GATE_HADGETAWARDS;
        btnSp.color = state ? Color.white : Color.black;
    }

    private void ShowRechargeItems()
    {
        view.Gobj_RechargePage.SetActive(true);
        view.Gobj_VipPage.SetActive(false);

        if (_rechargeItems.Count <= 0)
        {
            List<RechargeShowData> lists = ConfigManager.Instance.mRechargeConfig.GetRechargeShowList();
            for (int i = 0; i < lists.Count; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_RechargeItem, view.Grd_Recharge.transform);
                go.SetActive(true);
                RechargeItem item = go.AddComponent<RechargeItem>();
                item.InitItem(lists[i]);
                _rechargeItems.Add(lists[i].ID, item);
            }
            view.Grd_Recharge.Reposition();
        }
    }

    private void ShowVipPrivileges()
    {
        view.Gobj_RechargePage.SetActive(false);
        view.Gobj_VipPage.SetActive(true);
        if (_initializedVipPris)
            return;
        _initializedVipPris = true;
        _pageMove_dest = view.Grd_VipDese.transform.localPosition;
        List<VipData> list = ConfigManager.Instance.mVipConfig.GetVipInfoList();
        _maxVipPage = list.Count;
        for (int i = 0; i < list.Count; i++)
        {
            GameObject go = CommonFunction.InstantiateObject(view.Gobj_VipDesc, view.Grd_VipDese.transform);
            go.SetActive(true);
            go.GetComponent<UILabel>().text = list[i].Description;
        }
        view.Grd_VipDese.Reposition();
    }

    private void ShowVipPrivilegePage(int page, bool direct = false)
    {
        if (_newVipPage)
            _maxVipPage = ConfigManager.Instance.mVipConfig.GetVipInfoList().Count;

        page = Mathf.Clamp(page, 1, _maxVipPage - 1);
        if (page == 1)
        {
            view.Btn_Pre.gameObject.SetActive(false);
            view.Btn_Next.gameObject.SetActive(true);
        }
        else if (page == _maxVipPage - 1)
        {
            view.Btn_Pre.gameObject.SetActive(true);
            view.Btn_Next.gameObject.SetActive(false);
        }
        else
        {
            view.Btn_Pre.gameObject.SetActive(true);
            view.Btn_Next.gameObject.SetActive(true);
        }

        if(_newVipPage)
        {
            view.ScrlView_VipContent.ResetPosition();
            view.Lbl_SeeVipLv.text = page.ToString();
            view.Lbl_VipContent.text = ConfigManager.Instance.mVipConfig.GetVipInfoList()[page].Description;
            _curVipPage = page;
            view.Lbl_TitleDayGift.text = string.Format(ConstString.FORMAT_VIP_DAY_GIFT,_curVipPage);
            SetGiftData();
            SetGetBtnState();
            return;
        }

        view.Lbl_SeeVipLv.text = page.ToString();
        view.ScrView_DescPan.ResetPosition();
        if (page != _curVipPage)
        {
            if (direct)
            {
                _pageMove_orig = _pageMove_dest;
                _pageMove_dest = _pageMove_orig - new Vector3((page - _curVipPage) * view.Grd_VipDese.cellWidth, 0, 0);
                view.Grd_VipDese.transform.localPosition = _pageMove_dest;
            }
            else
            {
                _pageMove_orig = _pageMove_dest;
                _pageMove_dest = _pageMove_orig - new Vector3((page - _curVipPage) * view.Grd_VipDese.cellWidth, 0, 0);
                _pageMove_start = true;
            }
            _curVipPage = page;
        }
    }

    private void UpdateVipPage()
    {
        if (!_pageMove_start)
            return;

        _pageMove_factor += Time.deltaTime;
        if (_pageMove_factor <= _pageMove_time)
        {
            view.Grd_VipDese.transform.localPosition = _pageMove_orig +
                                          (_pageMove_dest - _pageMove_orig) * (_pageMove_factor / _pageMove_time);
        }
        else
        {
            _pageMove_factor = 0;
            _pageMove_start = false;
            view.Grd_VipDese.transform.localPosition = _pageMove_dest;
        }

    }

    private void InitVipLv()
    {

        uint curlv = PlayerData.Instance._VipLv;
        if (curlv >= VipConfig.VipMaxLevel)
        {
            curlv = VipConfig.VipMaxLevel;
            SetVipLvUpTipEnable(false);
            view.Lbl_CurVipLv.text = curlv.ToString();
            view.Slider_EXP.value = 1;
        }
        else
        {
            VipData curVipInfo = ConfigManager.Instance.mVipConfig.GetVipDataByLv(curlv);
            SetVipLvUpTipEnable(true);
            view.Lbl_CurVipLv.text = curlv.ToString();
            view.Lbl_NextVipLv.text = (curlv + 1).ToString();
            //UpdateExp(curVipInfo.TotalExp, PlayerData.Instance.VipExp);
            view.Slider_EXP.value = (float)PlayerData.Instance.VipExp / curVipInfo.TotalExp;
            view.Lbl_LvUpDiaCount.text = ((curVipInfo.TotalExp - PlayerData.Instance.VipExp) /
                long.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.RECHARGE_TO_VIP_EXP))).ToString();
        }
    }
    public void UpdateExp(uint TotalExp, int VipExp)
    {
        float CurValue = 0.0F;
        float FinalTweenTime = 0.5F;
        //float TargetExpAmount = (float)TotalExp / VipExp ;
        float TargetExpAmount = (float)VipExp / TotalExp;
        Main.Instance.StartCoroutine(UpdateExpAmount(CurValue, FinalTweenTime, TargetExpAmount));
    }
    public IEnumerator UpdateExpAmount(float CurValue, float FinalTweenTime, float TargetExpAmount)
    {
        if (TargetExpAmount == 0) { view.Slider_EXP.value = 0; yield break; }
        float curTime = 0.0F;
        float UnityBalue = TargetExpAmount * Time.deltaTime / FinalTweenTime;
        while (curTime <= FinalTweenTime)
        {
            curTime += Time.deltaTime;
            CurValue += UnityBalue;
            view.Slider_EXP.value = CurValue;
            yield return null;
        }
    }

    private void SetVipLvUpTipEnable(bool enable)
    {
        view.Spt_RechargeTip1.gameObject.SetActive(enable);
        //view.Spt_RechargeTip2.gameObject.SetActive(enable);
        view.Spt_NextVipLvTip.gameObject.SetActive(enable);
        view.Lbl_LvUpDiaCount.gameObject.SetActive(enable);
        view.Lbl_NextVipLv.gameObject.SetActive(enable);
    }

    public void RefrashUI()
    {
        InitVipLv();
        UpdateRechargeItems();
    }

    public void UpdateRechargeItems()
    {
        List<RechargeItem> list = new List<RechargeItem>(_rechargeItems.Values);
        for (int i = 0; i < list.Count; i++)
        {
            list[i].InitItem(null);
        }
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(VipRechargeView.UIName);

        if(UISystem.Instance.UIIsOpen(StoreView.UIName))
        {
            UISystem.Instance.StoreView.RefreshUI();
        }
    }

    public void ButtonEvent_Pre(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowVipPrivilegePage(_curVipPage - 1);
    }

    public void ButtonEvent_Next(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        ShowVipPrivilegePage(_curVipPage + 1);
    }

    public void ButtonEvent_VipRecharge(GameObject btn)
    {
        if (_curState == VipRechargeState.Vip)
        {
            ShowRecharge();
        }
        else
        {
            ShowVipPrivilege(PlayerData.Instance._VipLv);

        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_GetLvUpGift(GameObject btn)
    {
        StoreModule.Instance.SendGetVipUpReward(_curVipPage);
    }
    public void ButtonEvent_GetDayGift(GameObject btn)
    {
        StoreModule.Instance.SendVipDailyReward();
    }

    public override void Uninitialize()
    {
        view.Grd_VipDese.transform.localPosition = _pageMove_dest;
        Scheduler.Instance.RemoveUpdator(UpdateVipPage);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_Pre.gameObject).onClick = ButtonEvent_Pre;
        UIEventListener.Get(view.Btn_Next.gameObject).onClick = ButtonEvent_Next;
        UIEventListener.Get(view.Btn_VipRecharge.gameObject).onClick = ButtonEvent_VipRecharge;
        UIEventListener.Get(view.Btn_GetLvUp.gameObject).onClick = ButtonEvent_GetLvUpGift;
        UIEventListener.Get(view.Btn_GetDay.gameObject).onClick = ButtonEvent_GetDayGift;
        for (int i = 0; i < view.Gobj_PropBGs.Length; i++)
        {
            UIEventListener.Get(view.Gobj_PropBGs[i].gameObject).onPress = PressGiftProp;            
        }
    }


    //充值粒子飞行
    public void RechargeFlayItweenAnim(uint id)
    {
        //上次点击item 目标item
        RechargeItem item = null;
        _rechargeItems.TryGetValue(id, out item);
        if (item == null)
        {
            Debug.LogError("Cant find recharge item !!!!!!!!! " + id);
        }
        //Debug.LogError("item.Spt_ActIcon.gameObject" + item.Spt_ActIcon.gameObject);
        //Debug.LogError("_TopFuncView.Spt_BtnGemIcon.gameObject" +view . GO_TopFuncIcon);
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_TOPFUNC))
        {
            Main.Instance.StartCoroutine(ItweenAnimInpspector(0, item.Spt_ActIcon.gameObject, UISystem.Instance.TopFuncView.view.Spt_BtnGemIcon.gameObject, 1.0F));
        }
    }
    public IEnumerator ItweenAnimInpspector(float time, GameObject From, GameObject To, float FlyTimes)
    {
        yield return new WaitForSeconds(time);
        if (view.Go_VipTrail != null && view.Go_TopFuncFX != null)
        {
            GameObject Go1 = ShowEffectManager.Instance.ShowEffect(view.Go_VipTrail, From.transform);
            if (Go1) iTween.MoveTo(Go1, To.transform.position, FlyTimes);
            //iTween.MoveTo(Go1, iTween.Hash("position", To.transform.position, "time", FlyTimes,"easetype",iTween .EaseType.easeOutQuad ));
            yield return new WaitForSeconds(time + 0.1F);
            GameObject Go2 = ShowEffectManager.Instance.ShowEffect(view.Go_VipTrail, From.transform);
            if (Go2) iTween.MoveTo(Go2, To.transform.position, FlyTimes);
            //iTween.MoveTo(Go1, iTween.Hash("position", To.transform.position, "time", FlyTimes, "easetype", iTween.EaseType.easeOutQuad));
            yield return new WaitForSeconds(time + 0.1F);
            GameObject Go3 = ShowEffectManager.Instance.ShowEffect(view.Go_VipTrail, From.transform);
            if (Go3) iTween.MoveTo(Go3, To.transform.position, FlyTimes);
            //iTween.MoveTo(Go1, iTween.Hash("position", To.transform.position, "time", FlyTimes,"easetype",iTween .EaseType.easeOutQuad));
            yield return new WaitForSeconds(time + 0.3F);
            GameObject Go4 = ShowEffectManager.Instance.ShowEffect(view.Go_TopFuncFX, To.transform);
            yield return new WaitForSeconds(time + 0.1F);
            GameObject Go5 = ShowEffectManager.Instance.ShowEffect(view.Go_TopFuncFX, To.transform);
            yield return new WaitForSeconds(time + 0.1F);
            GameObject Go6 = ShowEffectManager.Instance.ShowEffect(view.Go_TopFuncFX, To.transform);
        }

    }
}
