using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
using System.Collections.Generic;

/// <summary>
/// 军团功能类型
/// </summary>
public enum EUnionFuncType
{
    None = 0,
    Hall,            //軍團大廳//
    Store,           //軍團商店//
    Advanture,       //異域探險//
    Fight,           //群雄爭霸//
    Prison,          //信仰神殿//
    CaptureTerritory, //攻城略地//
    CrossServerWar,  //跨服战场
}

public class UnionViewController : UIBase
{
    private const float INIT_NOTICE_BG_HEIGHT = 115;
    private const float INIT_NOTICE_INFO_HEIGHT = 30;


    public UnionView view;
    private Union _union;
    private Dictionary<EUnionFuncType, UnionBtnItem> dicSingleFuncItem = new Dictionary<EUnionFuncType, UnionBtnItem>();
    private List<string> listShowNoticeTipBtnInfo = new List<string>();



    public override void Initialize()
    {
        if (view == null)
        {
            view = new UnionView();
            view.Initialize();
            if (view.Tex_BG != null)
            {
                ResourceLoadManager.Instance.LoadAloneImage("image_JTGL_bg_bjt.assetbundle", texture =>
                //ResourceLoadManager.Instance.LoadAloneImage("image_UnionBack.assetbundle", texture =>
                {
                    view.Tex_BG.mainTexture = texture;
                    view.Tex_BG.MakePixelPerfect();
                });
            }
        }
        if (dicSingleFuncItem == null)
        {
            dicSingleFuncItem = new Dictionary<EUnionFuncType, UnionBtnItem>();
        }
        if (listShowNoticeTipBtnInfo == null)
        {
            listShowNoticeTipBtnInfo = new List<string>();
        }
        BtnEventBinding();
        view.MaskEffect.gameObject.SetActive(false);
        InitFuncDics();
        InitUI();
        if(GlobalConst.ISOPENGM)
            Scheduler.Instance.AddUpdator(HegemonyGMUpdator);
       
    }
    public override void Destroy()
    {
        base.Destroy();
        PlayerData.Instance.NotifyRefreshEvent -= Instance_NotifyRefreshEvent;
        view = null;
        if (dicSingleFuncItem != null)
        {
            dicSingleFuncItem.Clear();
            dicSingleFuncItem = null;
        }
        if (listShowNoticeTipBtnInfo != null)
        {
            listShowNoticeTipBtnInfo.Clear();
            listShowNoticeTipBtnInfo = null;
        }
    }
    private void InitFuncDics()
    {
        if (view != null)
        {
            if ((view.Btn_Hall != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.Hall)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.Hall, new UnionBtnItem(view.Btn_Hall.transform, view.UIPanel_UnionView));
            }
            if ((view.Btn_Store != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.Store)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.Store, new UnionBtnItem(view.Btn_Store.transform, view.UIPanel_UnionView));
            }
            if ((view.Btn_Advanture != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.Advanture)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.Advanture, new UnionBtnItem(view.Btn_Advanture.transform, view.UIPanel_UnionView));
            }
            if ((view.Btn_Fight != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.Fight)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.Fight, new UnionBtnItem(view.Btn_Fight.transform, view.UIPanel_UnionView));
            }
            if ((view.Btn_UnionPrison != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.Prison)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.Prison, new UnionBtnItem(view.Btn_UnionPrison.transform, view.UIPanel_UnionView));
            }
            if ((view.Btn_CaptureTerritory != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.CaptureTerritory)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.CaptureTerritory, new UnionBtnItem(view.Btn_CaptureTerritory.transform, view.UIPanel_UnionView));
            }
            if ((view.Btn_CrossServerWar != null) && (!dicSingleFuncItem.ContainsKey(EUnionFuncType.CrossServerWar)))
            {
                dicSingleFuncItem.Add(EUnionFuncType.CrossServerWar, new UnionBtnItem(view.Btn_CrossServerWar.transform, view.UIPanel_UnionView));
            }
        }
        listShowNoticeTipBtnInfo.Add(ConstString.UNION_NOTICEBTN_CLOSE);
        listShowNoticeTipBtnInfo.Add(ConstString.UNION_NOTICEBTN_OPEN);
    }
    public void InitUI()
    {
        _union = UnionModule.Instance.UnionInfo;
        UnionLvUpData lvdata = ConfigManager.Instance.mUnionConfig.GetUnionLvUpData(_union.base_info.level);
        //军团名
        view.Lbl_UnionName.text = _union.base_info.name;
        //军团图标
        CommonFunction.SetSpriteName(view.Spt_Icon, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(_union.base_info.icon));
        //军团状态
        this.view.Spt_State.gameObject.SetActive(_union.base_info.altar_status == AltarFlameStatus.DEPEND_STATUS);
        //军团等级
        view.Btn_LvUp.gameObject.SetActive(UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN);
        if (_union.base_info.level >= ConfigManager.Instance.mUnionConfig.MaxUnionLevel) 
        {
            view.Lbl_UnionLv.text = string.Format("{0}\n\n",string.Format(ConstString.CHATVIEW_LABEL_PLAYRELV, _union.base_info.level));
            view.Spt_MaxSprite.gameObject.SetActive(true);
        }
        else
        {
            view.Lbl_UnionLv.text = string.Format(ConstString.CHATVIEW_LABEL_PLAYRELV, _union.base_info.level);
            view.Spt_MaxSprite.gameObject.SetActive(false);
        }
        //军团ID
        view.Lbl_UnionID.text = string.Format(ConstString.FORMAT_UNION_ID, _union.base_info.id);
        //军团物资
        if ((lvdata != null) && (_union != null))
        {
            view.Slider_SupplyProgress.value = (float)_union.resource / lvdata.mCost;
            view.Lbl_SupplyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, _union.resource, lvdata.mCost);
        }
        else
        {
            view.Slider_SupplyProgress.value = 1;
            view.Lbl_SupplyNum.text = string.Format(ConstString.FORMAT_NUMBER_FRACTION, 0, 0);
        }
        //军团公告
        view.Lbl_NoticeContent.text = UnionModule.Instance.CheckUnionNotice(_union.desc);
        view.Lbl_NoticeContent.maxLineCount = 1;
        ShowNoticeContent();

        ShowBtnInfo_Hall(lvdata);
        ShowBtnInfo_Store();
        ShowBtnInfo_Advanture();
        ShowBtnInfo_Fight();
        ShowBtnInfo_UnionPrison();
        ShowBtnInfo_CaptureTerritory();//?
        ShowBtnInfo_CrossServerWar();
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(UnionView.UIName);
    }

    /// <summary>
    /// 军团捐献
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_UnionDonation(GameObject btn)
    {
        UnionModule.Instance.OpenUnionDonation();
    }
    /// <summary>
    /// 军团升级
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_LvUp(GameObject btn)
    {
        if (UnionModule.Instance.MyUnionMemberInfo.position != UnionPosition.UP_CHAIRMAN)
            return;
        //如果当前等级已经是最高等级 就不升级了
        if (_union.base_info.level >= ConfigManager.Instance.mUnionConfig.MaxUnionLevel)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_TIP_UNIONLEVELMAX);
            return;
        }

        if (_union.resource < ConfigManager.Instance.mUnionConfig.GetUnionLvUpData(_union.base_info.level).mCost)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_UNION_RESOURCE_NOT_ENOUGH);
            return;
        }

        UnionModule.Instance.OnSendLevelUpUnion();
    }
    /// <summary>
    /// 军团规则
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_UnionRule(GameObject btn)
    {
        UISystem.Instance.HintView.ShowRuleHintView(ConstString.UNION_RULE_TITLE, ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mUnionRule);
    }
    /// <summary>
    /// 关闭按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(UnionView.UIName);
        Scheduler.Instance.RemoveUpdator(HegemonyGMUpdator);
    }

    /// <summary>
    /// 军团大厅
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Hall(GameObject btn)
    {
        UnionModule.Instance.OpenUnionHall();
    }
    /// <summary>
    /// 军团商店
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Store(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(StoreView.UIName);
        UISystem.Instance.StoreView.ShowStore(ShopType.ST_UnionShop);
    }
    /// <summary>
    /// 异域探险
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Advanture(GameObject btn)
    {
        UnionModule.Instance.OnSendOpenUnionPveDgn();
    }
    /// <summary>
    /// 群雄争霸
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Fight(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GUILDHEGEMONYVIEW);
    }
    /// <summary>
    /// 信仰神殿
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_UnionPrison(GameObject btn)
    {
        if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.UnionPrison, true))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONVIEW);
    }
    /// <summary>
    /// 攻城略地
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_CaptureTerritory(GameObject btn)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.GongCheng))
            return;
        CaptureTerritoryModule.Instance.OpenCaptureTerritory();
    }
    /// <summary>
    /// 显示公告
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ShowNoticeTip(GameObject btn)
    {
        if (view.Lbl_NoticeContent.maxLineCount == 1)
        {
            view.Lbl_NoticeContent.maxLineCount = 0;
        }
        else
        {
            view.Lbl_NoticeContent.maxLineCount = 1;
        }
        ShowNoticeContent();
    }
    /// <summary>
    /// 打开跨服战场
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_CrossServerWar(GameObject btn)
    {
        if (!UnionModule.Instance.HasUnion)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONVIEW);
            return;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CROSSSERVERWAR);

    }

    public override void Uninitialize()
    {
        PlayerData.Instance.NotifyRefreshEvent -= Instance_NotifyRefreshEvent;
        Scheduler.Instance.RemoveTimer(ShowHintInfo_CrossServerWar);
        Scheduler.Instance.RemoveTimer(ShowHint_CaptureTerritory);
    }

    public void BtnEventBinding()
    {
        PlayerData.Instance.NotifyRefreshEvent += Instance_NotifyRefreshEvent;

        UIEventListener.Get(view.Btn_UnionDonation.gameObject).onClick = ButtonEvent_UnionDonation;
        UIEventListener.Get(view.Btn_LvUp.gameObject).onClick = ButtonEvent_LvUp;
        UIEventListener.Get(view.Btn_UnionRule.gameObject).onClick = ButtonEvent_UnionRule;
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;

        UIEventListener.Get(view.Btn_Hall.gameObject).onClick = ButtonEvent_Hall;
        UIEventListener.Get(view.Btn_Store.gameObject).onClick = ButtonEvent_Store;
        UIEventListener.Get(view.Btn_Advanture.gameObject).onClick = ButtonEvent_Advanture;
        UIEventListener.Get(view.Btn_Fight.gameObject).onClick = ButtonEvent_Fight;
        UIEventListener.Get(view.Btn_UnionPrison.gameObject).onClick = ButtonEvent_UnionPrison;
        UIEventListener.Get(view.Btn_CaptureTerritory.gameObject).onClick = ButtonEvent_CaptureTerritory;
        UIEventListener.Get(view.Btn_ShowNoticeTip.gameObject).onClick = ButtonEvent_ShowNoticeTip;
        UIEventListener.Get(view.Btn_CrossServerWar.gameObject).onClick = ButtonEvent_CrossServerWar;
    }

    void Instance_NotifyRefreshEvent(NotifyRefresh data)
    {
        if(data.type == NotifyType.ALTAR_UNION)
        {
            if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.Prison)))
            {
                dicSingleFuncItem[EUnionFuncType.Prison].RefreshNoticeStatus(data.num > 0);
            }
        }
    }

    public void UpdateCaptureTerritoryNotice()
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.CaptureTerritory)))
        {
            bool tmpIsNotice = false;
            if (UnionModule.Instance.HasUnion)
            {
                tmpIsNotice = CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting;
            }
            dicSingleFuncItem[EUnionFuncType.CaptureTerritory].RefreshNoticeStatus(tmpIsNotice);
        }
    }

    public void HegemonyGMUpdator()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 1 300 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 2 300 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 3 300 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 4 86400 1");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 5 86400 1");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 1 300 0");
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 2 300  0");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 3 300 0");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 4 3000 0");
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            GMModule.Instance.SendGMCommandReq("UNIONCLASHSTATE 5 3000 0");
        }

    }

    //===========================================================================
    GameObject Go_LvUPEffect;
    public void PlayLVUPEffect()
    {
        if (Go_LvUPEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_UNIONLVUP, (GameObject gb) => { Go_LvUPEffect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_LvUPEffect, view .Lbl_SupplyNum.transform );
        Main.Instance.StartCoroutine(EffectMask(1F));
    }
    public IEnumerator EffectMask(float time)
    {
        view.MaskEffect.gameObject.SetActive(true);
        view.MaskEffect.depth = 30;
        yield return new WaitForSeconds(time);
        view.MaskEffect.gameObject.SetActive(false);
    }


    //显示单个按钮信息//
    private void ShowBtnInfo_Hall(UnionLvUpData vLVData)//軍團大廳//
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.Hall)))
        {
            string tmpHint = "";
            bool tmpIsNotice = false;
            if ((_union != null) && (vLVData != null))
            {
                tmpHint = string.Format(ConstString.FORMAT_UNION_MEMBER_COUINT, _union.members.Count + 1, vLVData.mMemberCount);
            }
            else
            {
                tmpHint = string.Format(ConstString.FORMAT_UNION_MEMBER_COUINT, 0, 0);
            }
            if (UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_VICE_CHAIRMAN || UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_CHAIRMAN)
            {
                tmpIsNotice = UnionModule.Instance.UnionInfo.pending_members.Count > 0;
            }
            dicSingleFuncItem[EUnionFuncType.Hall].RefreshInfos(tmpHint, false, tmpIsNotice);
        }
    }
    private void ShowBtnInfo_Store()            //軍團商店//
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.Store)))
        {
            dicSingleFuncItem[EUnionFuncType.Store].RefreshInfos("", false, false);
        }
    }
    private void ShowBtnInfo_Advanture()        //異域探險//
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.Advanture)))
        {
            dicSingleFuncItem[EUnionFuncType.Advanture].RefreshInfos("", false, false);
        }
    }
    private void ShowBtnInfo_Fight()            //群雄爭霸//
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.Fight)))
        {
            string tmpHint = "";
            //if (UnionModule.Instance.UnionPvpState == UnionPvpState.UPS_COOLING || UnionModule.Instance.UnionPvpState == UnionPvpState.UPS_CANCEL)
            //{
            //    tmpHint = string.Format(
            //    ConstString.FORMAT_UNION_FIGHT_RANK, UnionModule.Instance.UnionFightRank > 10000 ?
            //    ConstString.RANK_LABEL_OUTRANK : UnionModule.Instance.UnionFightRank.ToString());
            //}
            //else
            //{
            //    tmpHint = CommonFunction.GetUnionPvpStateStr(UnionModule.Instance.UnionPvpState);
            //}
            //dicSingleFuncItem[EUnionFuncType.Fight].RefreshInfos(tmpHint, false, false);

            if (ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.UnionHegemony, false))
            {
                if (UnionModule.Instance.UnionPvpState == UnionPvpState.UPS_COOLING || UnionModule.Instance.UnionPvpState == UnionPvpState.UPS_CANCEL)
                {
                    tmpHint = string.Format(
                    ConstString.FORMAT_UNION_FIGHT_RANK, UnionModule.Instance.UnionFightRank > 10000 ?
                    ConstString.RANK_LABEL_OUTRANK : UnionModule.Instance.UnionFightRank.ToString());
                }
                else
                {
                    tmpHint = CommonFunction.GetUnionPvpStateStr(UnionModule.Instance.UnionPvpState);
                }
                dicSingleFuncItem[EUnionFuncType.Fight].RefreshInfos(tmpHint, false, false);
            }
            else
            {
                tmpHint = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.UnionHegemony, ConstString.GATE_ESCORT_LOCKTIP_UNLOCK);
                dicSingleFuncItem[EUnionFuncType.Fight].RefreshInfos(tmpHint, true, false);
            }
        }
    }
    private void ShowBtnInfo_UnionPrison()      //信仰神殿//
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.Prison)))
        {
            string tmpHint = "";
            bool tmpIsLock = false;
            bool tmpIsNotice = false;
            bool tmpIsOpen = ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.UnionPrison, false);
            if (tmpIsOpen)
            {
                tmpIsLock = false;
                tmpHint = string.Format(ConstString.UNIONPRISONBUFF, (float)UnionModule.Instance.UnionPrisonBuffNum / 10000f);
            }
            else
            {
                tmpIsLock = true;
                tmpHint = ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.UnionPrison, ConstString.GATE_ESCORT_LOCKTIP_UNLOCK);
            }
            tmpIsNotice = (UnionPrisonModule.Instance.Prompt && tmpIsOpen);
            dicSingleFuncItem[EUnionFuncType.Prison].RefreshInfos(tmpHint, tmpIsLock, tmpIsNotice);
        }
    }
    private void ShowBtnInfo_CaptureTerritory() //攻城略地//
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.CaptureTerritory)))
        {
            //dicSingleFuncItem[EUnionFuncType.CaptureTerritory].RefreshInfos("", !CommonFunction.CheckFuncIsOpen(OpenFunctionType.GongCheng, false), false);
            //UpdateCaptureTerritoryNotice();
            //ShowHint_CaptureTerritory();
            //Scheduler.Instance.AddTimer(1, true, ShowHint_CaptureTerritory);
            if (ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.GongCheng, false))
            {
                dicSingleFuncItem[EUnionFuncType.CaptureTerritory].RefreshInfos("", false, false);
                UpdateCaptureTerritoryNotice();
                ShowHint_CaptureTerritory();
                Scheduler.Instance.AddTimer(1, true, ShowHint_CaptureTerritory);
            }
            else
            {
                dicSingleFuncItem[EUnionFuncType.CaptureTerritory].RefreshInfos(ConfigManager.Instance.mOpenLevelConfig.GetOpenStr(OpenFunctionType.GongCheng, ConstString.GATE_ESCORT_LOCKTIP_UNLOCK), true, false);
            }
        }
    }
    private void ShowHint_CaptureTerritory()
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.CaptureTerritory)))
        {
            string tmpHint = "";
            if (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting)
            {
                tmpHint = string.Format(ConstString.TIP_CAMPAIGN_TIMER_END, CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer));
            }
            else
            {
                tmpHint = string.Format(ConstString.TIP_CAMPAIGN_TIMER_START, CommonFunction.GetTimeString(CaptureTerritoryModule.Instance.CampaignTimer));
            }

            dicSingleFuncItem[EUnionFuncType.CaptureTerritory].RefreshHintInfo(tmpHint);
        }
    }
    /// <summary>
    /// 跨服战入口
    /// </summary>
    private void ShowBtnInfo_CrossServerWar()
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.CrossServerWar)))
        {
            bool isOpen = CommonFunction.CheckFuncIsOpen(OpenFunctionType.CrossServerWar,false);
            if (isOpen)
            {
                dicSingleFuncItem[EUnionFuncType.CrossServerWar].RefreshInfos("", false, false);
                ShowHintInfo_CrossServerWar();
                Scheduler.Instance.AddTimer(1, true, ShowHintInfo_CrossServerWar);
            }
            else
            {
                dicSingleFuncItem[EUnionFuncType.CrossServerWar].RefreshInfos(String.Format(ConstString.BACKPACK_LEVELLOCKTIP,
                    ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.CrossServerWar).openLevel), true, false);
            }
            UpdateCrossServerWarNotice();
        }
    }
    /// <summary>
    /// 跨服战时间显示
    /// </summary>
    private void ShowHintInfo_CrossServerWar()
    {
        string tmpHint = "";
        if (CrossServerWarModule.Instance.isBattleStart)
        {
            tmpHint = string.Format(ConstString.CROSSSERVER_BATTLE_START_TIME, CommonFunction.GetTimeString(Math.Max(0, CrossServerWarModule.Instance.time_Battle - Main.mTime)));
        }
        else
        {
            tmpHint = string.Format(ConstString.CROSSSERVER_BATTLE_END_TIME, CommonFunction.GetTimeString(Math.Max(0, CrossServerWarModule.Instance.time_Battle - Main.mTime)));
        }
        dicSingleFuncItem[EUnionFuncType.CrossServerWar].RefreshHintInfo(tmpHint);
    }
    /// <summary>
    /// 跨服战推送相应
    /// </summary>
    public void UpdateCrossServerWarNotice()
    {
        if ((dicSingleFuncItem != null) && (dicSingleFuncItem.ContainsKey(EUnionFuncType.CrossServerWar))&& CommonFunction.CheckFuncIsOpen(OpenFunctionType.CrossServerWar, false))
        {
            dicSingleFuncItem[EUnionFuncType.CrossServerWar].RefreshNoticeStatus(CrossServerWarModule.Instance.isBattleStart);
        }
    }
    private void ShowNoticeContent()
    {
        float tmpDisHeight = view.Lbl_NoticeContent.localSize.y - INIT_NOTICE_INFO_HEIGHT;
        if (view.Spt_InfoBG != null)
        {
            view.Spt_InfoBG.height = (int)(INIT_NOTICE_BG_HEIGHT + tmpDisHeight);
        }
        if (view.Trans_MoveContent != null)
        {
            view.Trans_MoveContent.localPosition = new Vector3(0, -tmpDisHeight, 0);
        }
        if (view.Lbl_ShowNoticeTip != null)
        {
            view.Lbl_ShowNoticeTip.text = listShowNoticeTipBtnInfo[view.Lbl_NoticeContent.maxLineCount];
        }
    }
}
