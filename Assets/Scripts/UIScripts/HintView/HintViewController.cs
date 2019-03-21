using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using Assets.Script.Common;
public class HintViewController : UIBase
{
    public HintView view;

    private List<HintTipLabelComponent> tipLabel_dic = new List<HintTipLabelComponent>();
    private List<HintTipLabelComponent> tipActivedList = new List<HintTipLabelComponent>();
    private int ShowHintNum = 0;

    private System.Action yesBtnCallBack;
    private System.Action noBtnCallBack;
    private System.Action okBtnCallBack;
    public delegate void markCallBack(bool markValue);
    private markCallBack matkBtnCallBack;
    private MessageBoxType _currentType = MessageBoxType.None;
    private RichTextProcessing _richTextProcessing;
    private bool _closeUI = true;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new HintView();
            view.Initialize();
            BtnEventBinding();
        }
        InitPanel();
    }

    public void InitPanel()
    {
        view.Gobj_Loading.SetActive(false);
        view.Gobj_Ok.SetActive(false);
        view.Gobj_YesNo.SetActive(false);
        view.Gobj_RichText.SetActive(false);
        view.Spt_LoadingMask.enabled = false;
        view.Gobj_CommonTipLabelGroup.SetActive(false);
        view.Spt_Loading.gameObject.SetActive(false);
        view.gobj_YesNo_Mark.SetActive(false);
        if (_richTextProcessing == null)
            _richTextProcessing = view.Lbl_RichTextInfoLb.GetComponent<RichTextProcessing>();
        if (_richTextProcessing == null)
            _richTextProcessing = view.Lbl_RichTextInfoLb.gameObject.AddComponent<RichTextProcessing>();



    }

    /// <summary>
    /// 显示提示框
    /// </summary>
    /// <param name="msgType">消息类型</param>
    /// <param name="content">需要显示的提示内容</param>
    /// <param name="yesCallBack">左侧确认按钮回调</param>
    /// <param name="noCallBakc">右侧取消按钮回调</param>
    /// <param name="yesBtnStr">左侧确认按钮文本</param>
    /// <param name="noBtnStr">右侧取消按钮文本</param>
    /// <param name="isClose">是否关闭</param>
    public void ShowMessageBox(MessageBoxType msgType, string content = "", Action yesCallBack = null, Action noCallBakc = null, string yesBtnStr = "", string noBtnStr = "", markCallBack call = null, bool markValue = false, bool isClose = true)
    {
        //CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        if (msgType != _currentType)
            InitPanel();
        _closeUI = isClose;
        switch (msgType)
        {
            case MessageBoxType.mb_Hint:
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Operate_Fail, view._uiRoot.transform.parent.transform));
                view.Gobj_CommonTipParent.SetActive(true);
                UpdateCommonTipLabel(content);
                break;
            case MessageBoxType.mb_Loading:
                view.Gobj_Loading.SetActive(true);
                break;
            case MessageBoxType.mb_Ok:
                // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Operate_Fail, view._uiRoot.transform.parent.transform));

                view.Lbl_OkInfoLb.text = content;
                if (string.IsNullOrEmpty(yesBtnStr))
                    view.Lbl_BtnOkYesBtnLb.text = ConstString.MESSAGEBOXBTN_YES;
                else
                    view.Lbl_BtnOkYesBtnLb.text = yesBtnStr;
                yesBtnCallBack = yesCallBack;
                // PlayOpenOKAnim();
                view.Gobj_Ok.SetActive(true);

                break;
            case MessageBoxType.mb_YesNo_Mark:
                if (string.IsNullOrEmpty(yesBtnStr))
                    view.rightBtnLbl.text = ConstString.MESSAGEBOXBTN_YES;
                else
                    view.rightBtnLbl.text = yesBtnStr;

                if (string.IsNullOrEmpty(noBtnStr))
                    view.leftBtnLbl.text = ConstString.MESSAGEBOXBTN_NO;
                else
                    view.leftBtnLbl.text = noBtnStr;
                view.DescriptLabel.text = content;
                yesBtnCallBack = yesCallBack;
                noBtnCallBack = noCallBakc;
                matkBtnCallBack = call;
                this.SetMarkValue(markValue);
                //PlayOpenYesNoAnim();
                view.gobj_YesNo_Mark.SetActive(true);

                break;
            case MessageBoxType.mb_RichText:
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Operate_Fail, view._uiRoot.transform.parent.transform));

                _richTextProcessing.ShowStr(content);
                if (string.IsNullOrEmpty(yesBtnStr))
                    view.Lbl_BtnRichTextYesBtnLb.text = ConstString.MESSAGEBOXBTN_YES;
                else
                    view.Lbl_BtnOkYesBtnLb.text = yesBtnStr;

                if (string.IsNullOrEmpty(noBtnStr))
                    view.Lbl_BtnRichTextNoBtnLb.text = ConstString.MESSAGEBOXBTN_NO;
                else
                    view.Lbl_BtnRichTextNoBtnLb.text = noBtnStr;
                yesBtnCallBack = yesCallBack;
                noBtnCallBack = noCallBakc;
                // PlayOpenRichTextAnim();
                view.Gobj_RichText.SetActive(true);

                break;
            case MessageBoxType.mb_YesNo:
                //CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Operate_Success, view._uiRoot.transform.parent.transform));

                if (string.IsNullOrEmpty(yesBtnStr))
                    view.Lbl_BtnYesBtnLb.text = ConstString.MESSAGEBOXBTN_YES;
                else
                    view.Lbl_BtnYesBtnLb.text = yesBtnStr;

                if (string.IsNullOrEmpty(noBtnStr))
                    view.Lbl_BtnNoBtnLb.text = ConstString.MESSAGEBOXBTN_NO;
                else
                    view.Lbl_BtnNoBtnLb.text = noBtnStr;
                view.Lbl_YesNoInfoLb.text = content;
                yesBtnCallBack = yesCallBack;
                noBtnCallBack = noCallBakc;
                //PlayOpenYesNoAnim();
                view.Gobj_YesNo.SetActive(true);

                break;
        }
    }

    private void UpdateCommonTipLabel(string content)
    {
        HintTipLabelComponent comp = GetHintTipComponent();
        if (comp == null)
            return;
        ShowHintNum += 2;
        if (ShowHintNum > 100)
        {
            ShowHintNum = 0;
            for (int i = 0; i < tipActivedList.Count; i++)
            {
                HintTipLabelComponent m_comp = tipActivedList[i];
                if (m_comp == null)
                    continue;
                m_comp.UpdateWidgetDepth(ShowHintNum);
                ShowHintNum += 2;
            }
        }
        tipActivedList.Add(comp);
        comp.UpdateContent(content, ShowHintNum, () =>
        {
            tipActivedList.RemoveAt(0);
            if (tipActivedList.Count == 0)
            {
                ShowHintNum = 0;
            }
        });
    }

    private HintTipLabelComponent GetHintTipComponent()
    {
        for (int i = 0; i < tipLabel_dic.Count; i++)
        {
            HintTipLabelComponent comp = tipLabel_dic[i];
            if (comp == null)
                continue;
            if (!comp.mRootObject.activeSelf)
            {
                return comp;
            }
        }
        GameObject go = CommonFunction.InstantiateObject(view.Gobj_CommonTipLabelGroup, view.Gobj_CommonTipLabelGroup.transform.parent);
        HintTipLabelComponent tmpComp = new HintTipLabelComponent();
        go.name = tipActivedList.Count.ToString();
        tmpComp.MyStart(go);
        tipLabel_dic.Add(tmpComp);
        return tmpComp;
    }

    /// <summary>
    /// 开启一个大型的提示窗口
    /// </summary>
    /// <param name="titleValue">窗口名字</param>
    /// <param name="ruleValue">文本内容</param>
    public void ShowRuleHintView(string titleValue, string ruleValue)
    {
        view.Gobj_Rule.SetActive(true);
        view.ScrollView.ResetPosition();
        view.Lbl_RuleValue.text = ruleValue;
        view.Lbl_TitleValue.text = titleValue;
    }

    private void SetMarkValue(bool markValue)
    {
        this.view.BG.SetActive(!markValue);
        this.view.HL.SetActive(markValue);
    }
    public void SetLoadingVisible(bool isVisible)
    {
        return;
        view.Gobj_Loading.SetActive(isVisible);
        view.Spt_LoadingMask.enabled = isVisible;
        if (isVisible)
        {
            view.Spt_LoadingMask.alpha = 0.01f;
            view.Spt_Loading.gameObject.SetActive(false);
            Scheduler.Instance.AddTimer(0.5f, false, UpdateLoading);
        }
        else
        {
            Scheduler.Instance.RemoveTimer(UpdateLoading);
            view.Spt_LoadingMask.alpha = 0.01f;
            view.Spt_Loading.gameObject.SetActive(false);
        }
    }
    #region 转菊花
    private float _appearTimer;
    public void ShowLoading(bool isVisible)
    {
        if (isVisible)
        {
            view.Gobj_Loading.SetActive(true);
            view.Spt_LoadingMask.enabled = true;
            view.Spt_LoadingMask.alpha = 0.01f;
            view.Spt_Loading.gameObject.SetActive(false);
            view.Lbl_Label.gameObject.SetActive(false);
            Scheduler.Instance.AddTimer(0.5f, false, UpdateLoading);
        }
        else
        {
            if (Time.time - _appearTimer > 1)
            {
                CloseLoading();
            }
            else
            {
                float closetime = Mathf.Max(_appearTimer - Time.time + 1, 0.1f);
                Scheduler.Instance.AddTimer(closetime, false, CloseLoading);
            }
        }
    }

    private void UpdateLoading()
    {
        view.Spt_LoadingMask.alpha = 0.5f;
        view.Spt_Loading.gameObject.SetActive(true);
        view.Lbl_Label.gameObject.SetActive(true);
        _appearTimer = Time.time;
    }

    private void CloseLoading()
    {
        Scheduler.Instance.RemoveTimer(UpdateLoading);
        view.Gobj_Loading.SetActive(false);
        view.Spt_LoadingMask.alpha = 0.01f;
        view.Spt_Loading.gameObject.SetActive(false);
        view.Lbl_Label.gameObject.SetActive(false);
    }
    #endregion

    #region 战斗加载Loading
    //private string[] _fightLoadingTip = new string[]{ConstString.FIGHT_LOADING_TIP,string.Format("{0}.",ConstString.FIGHT_LOADING_TIP)
    //                                    ,string.Format("{0}..",ConstString.FIGHT_LOADING_TIP),string.Format("{0}...",ConstString.FIGHT_LOADING_TIP)};
    //private int _fightTipIndex = 0;
    private float _progressWaiting = 0.7f;
    private float _progressFactor = 1;
    private float _progress = 0;
    private bool _fightIsReady = false;
    private bool _startFightLoading = false;
    public bool StartFightLoading
    {
        get { return _startFightLoading; }
    }
    public delegate void FightLoadingCompletedEventHandler();
    public FightLoadingCompletedEventHandler FightLoadingCompletedEvent;

    public void ShowFightLoading(FightLoadingCompletedEventHandler vightLoadingCompletedEvent = null, bool isMainCityLoading = false)
    {
        CloseLoading();
        _startFightLoading = true;
        FightLoadingCompletedEvent = vightLoadingCompletedEvent;
        //_fightTipIndex = 0;
        _progressFactor = 0.1f;
        _progress = 0.02f;
        view.Slider_Progress.value = _progress;
        _fightIsReady = false;
        _progressWaiting = UnityEngine.Random.Range(0.85f, 0.95f);
        //Scheduler.Instance.AddTimer(0.3f, true, UpdateTipTimer);
        Scheduler.Instance.AddUpdator(UpdateProgress);
        LoadTip();
        Scheduler.Instance.AddTimer(3, true, LoadTip);
        SetFightLoadingTex(isMainCityLoading);
        view.Gobj_FightLoading.SetActive(true);
    }

    private void LoadTip()
    {
        view.Lbl_FightLoadingTip.text = ConfigManager.Instance.mTipsConfig.GetTipsStr();
    }

    public void FightIsReady()
    {
        _fightIsReady = true;
        _progressFactor = 1;
    }

    private void SetFightLoadingTex(bool isMainCityLoading)
    {
        string texName = "";
        int index = 0;
        if (!isMainCityLoading)
        {
            index = UnityEngine.Random.Range(0, GlobalConst.SpriteName.FightLoaingBgs.Length);
            texName = GlobalConst.SpriteName.FightLoaingBgs[index];
        }
        else
        {
            index = UnityEngine.Random.Range(0, GlobalConst.SpriteName.MainCityLoaingBgs.Length);
            texName = GlobalConst.SpriteName.MainCityLoaingBgs[index];
        }

        view.Tex_FightLoadingBG.mainTexture = ResourceLoadManager.Instance.LoadResources(
                string.Format(GlobalConst.PATH_INTERNAL_TEXTURE, texName)) as Texture2D;
    }

    private void CloseFightLoading()
    {
        //Scheduler.Instance.RemoveTimer(UpdateTipTimer);
        //Scheduler.Instance.RemoveUpdator(UpdateProgress);
        if (FightLoadingCompletedEvent != null)
        {
            FightLoadingCompletedEvent();
            FightLoadingCompletedEvent = null;
        }
        view.Gobj_FightLoading.SetActive(false);
        _startFightLoading = false;
        Scheduler.Instance.RemoveTimer(LoadTip);
    }
    private void UpdateProgress()
    {
        if (!_startFightLoading)
            return;

        if (!_fightIsReady && _progress >= _progressWaiting)
        {
            _progressFactor = 0;
            return;
        }

        if (!_fightIsReady && _progress < _progressWaiting)
        {
            _progressFactor = UnityEngine.Random.Range(0.01f, 0.4f);

        }
        _progress += _progressFactor * Time.deltaTime;
        if (_progress >= 1)
        {
            CloseFightLoading();
        }
        view.Slider_Progress.value = _progress;
    }

    //private void UpdateTipTimer()
    //{
    //++_fightTipIndex;
    //_fightTipIndex = _fightTipIndex % _fightLoadingTip.Length;
    //view.Lbl_FightLoadingTip.text = _fightLoadingTip[_fightTipIndex];
    //}

    #endregion

    public void ButtonEvent_YesBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (_closeUI)     //先关闭界面  有可能出现连续提示   modify by taiwei
            view.Gobj_YesNo.SetActive(false);
        if (yesBtnCallBack != null)
            yesBtnCallBack();
    }

    public void ButtonEvent_NoBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.Gobj_YesNo.SetActive(false);
        if (noBtnCallBack != null)
            noBtnCallBack();
    }

    public void ButtonEvent_OkYesBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.Gobj_Ok.SetActive(false);
        if (yesBtnCallBack != null)
            yesBtnCallBack();
    }

    public void ButtonEvent_RichTextYesBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (_closeUI)
            view.Gobj_RichText.SetActive(false);
        if (yesBtnCallBack != null)
            yesBtnCallBack();
    }

    public void ButtonEvent_RichTextNoBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.Gobj_RichText.SetActive(false);
        if (noBtnCallBack != null)
            noBtnCallBack();
    }

    public void ButtonEvent_CloseRuleBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.Gobj_Rule.SetActive(false);
        view.Lbl_TitleValue.text = "";
        view.Lbl_RuleValue.text = "";
    }
    public void ButtonEvent_MarkNoBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.gobj_YesNo_Mark.SetActive(false);
        if (noBtnCallBack != null)
            noBtnCallBack();
    }
    public void ButtonEvent_MarkYesBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (_closeUI)     //先关闭界面  有可能出现连续提示   modify by taiwei
            view.gobj_YesNo_Mark.SetActive(false);
        if (matkBtnCallBack != null)
            matkBtnCallBack(this.view.HL.activeSelf);
        if (yesBtnCallBack != null)
            yesBtnCallBack();

    }
    public void ButtonEvent_MarkToggle(GameObject btn)
    {
        SetMarkValue(!this.view.HL.activeSelf);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_CloseMask.gameObject).onClick = ButtonEvent_CloseRuleBtn;
        UIEventListener.Get(view.Btn_YesBtn.gameObject).onClick = ButtonEvent_YesBtn;
        UIEventListener.Get(view.Btn_NoBtn.gameObject).onClick = ButtonEvent_NoBtn;
        UIEventListener.Get(view.Btn_OkYesBtn.gameObject).onClick = ButtonEvent_OkYesBtn;
        UIEventListener.Get(view.Btn_RichTextYesBtn.gameObject).onClick = ButtonEvent_RichTextYesBtn;
        UIEventListener.Get(view.Btn_RichTextNoBtn.gameObject).onClick = ButtonEvent_RichTextNoBtn;
        UIEventListener.Get(view.leftBtn.gameObject).onClick = ButtonEvent_MarkNoBtn;
        UIEventListener.Get(view.rightBtn.gameObject).onClick = ButtonEvent_MarkYesBtn;
        UIEventListener.Get(view.Toggle.gameObject).onClick = ButtonEvent_MarkToggle;

    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        tipLabel_dic.Clear();
        _currentType = MessageBoxType.None;
    }
    //界面动画
    //public void PlayOpenYesNoAnim()
    //{
    //    view.YesNo_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.YesNo_TScale.Restart();
    //    view.YesNo_TScale.PlayForward();
    //}
    //public void PlayOpenOKAnim()
    //{
    //    view.OK_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.OK_TScale.Restart();
    //    view.OK_TScale.PlayForward();
    //}
    //public void PlayOpenRichTextAnim()
    //{
    //    view.RichText_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.RichText_TScale.Restart();
    //    view.RichText_TScale.PlayForward();

    //}
}

