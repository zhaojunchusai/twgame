using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;

public class SupermacyViewController : UIBase 
{

    private const float INIT_ORDER_BOXCOLLIDER_CENTER_Y = 0;
    private const float INIT_ORDER_BOXCOLLIDER_SIZE_X = 455;
    private const float INIT_ORDER_BOXCOLLIDER_SIZE_Y = 135;
    private const float DISTANCE_ORDERITEM = 28;
    private const float INIT_AWARD_BOXCOLLIDER_CENTER_Y = 0;
    private const float INIT_AWARD_BOXCOLLIDER_SIZE_X = 455;
    private const float INIT_AWARD_BOXCOLLIDER_SIZE_Y = 100;
    private const float DISTANCE_AWARDITEM = 27;

    private List<OrderItem> listOrder;
    private List<AwardItem> listAward;
    private BoxCollider tableBoxOrder;
    private BoxCollider tableBoxAward;
    private List<SingleRankItem> listRankItem;

    public SupermacyView view;

    private ServerHegemonyInfo serverHegemonyInfo;

    private uint CDTime;
    private ArenaPlayer supermacyPlayer;
    //private long startTime;
    private int waitTime;

    public long restTime;

    private int reqcount;

    /// <summary>
    /// 是否自动战斗
    /// </summary>
    private bool isAutoBattle;
    /// <summary>
    /// 是否自动清除CD
    /// </summary>
    private bool isAutoClearCD;
    /// <summary>
    /// 是否准备进入战斗
    /// </summary>
    private bool isReadyToBattle;

    private PVPGetAwradsGroupComponet selfAwardItem;
    private PVPGetAwradsGroupComponet unionAwardItem;
    private PVPRankAwardDescGroupComponent selfRankAwardsItem;
    private PVPRankAwardDescGroupComponent unionRankAwardsItem;

    //---------------------------------初始化与按键响应函数--------------------------------------------//
    public override void Initialize()
    {
        if (view == null)
        {
            view = new SupermacyView();
            serverHegemonyInfo = ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo();
            isAutoBattle = false;
            isAutoClearCD = false;
        }
        view.Initialize();

        if (view.UITable_Order != null)
        {
            tableBoxOrder = view.UITable_Order.transform.GetComponent<BoxCollider>();
        }
        if (view.UITable_Award != null)
        {
            tableBoxAward = view.UITable_Award.transform.GetComponent<BoxCollider>();
        }


        BtnEventBinding();

        InitInfo();

        SupermacyModule.Instance.SendEnterOverlord();
    }
    public override void Uninitialize()
    {
        Scheduler.Instance.RemoveTimer(UpdateChallengeCD);
        Scheduler.Instance.RemoveTimer(HpUpdate);
        Scheduler.Instance.RemoveTimer(TimeUpdate);
        Scheduler.Instance.RemoveTimer(AutoBattleCheck);
        Scheduler.Instance.RemoveTimer(AutoClearCDCheck);
        Scheduler.Instance.RemoveTimer(SupermacyModule.Instance.SendChallenge);
    }

    private void InitRule()
    {
        if (serverHegemonyInfo != null)
        {
            view.Lbl_RuleDesc.text = CommonFunction.ReplaceEscapeChar(serverHegemonyInfo.hegemony_rule_content);
        }
        else
        {
            view.Lbl_RuleDesc.text = "";
        }
    }
    private void InitRankAward()
    {
        if (listRankItem != null)
        {
            foreach (SingleRankItem tmpInfo in listRankItem)
            {
                if (tmpInfo != null)
                {
                    tmpInfo.InitInfo();
                }
            }
        }
        if (view.Gobj_RankAward != null)
        {
            view.Gobj_RankAward.SetActive(false);
        }
    }
    private void InitInfo()
    {
        if (listOrder == null)
        {
            listOrder = new List<OrderItem>();
        }
        if (listAward == null)
        {
            listAward = new List<AwardItem>();
        }
        if (supermacyPlayer == null)
        {
            supermacyPlayer = new ArenaPlayer();
        }
        if (listRankItem == null)
        {
            listRankItem = new List<SingleRankItem>();
        }

        InitRankAward();
        InitRule();

        CDTime = (uint)Main.mTime;
        //startTime = 0;
        waitTime = 0;
        restTime = 0;
        reqcount = 0;
    }

    private IEnumerator InitRankAwardUI()
    {
        view.ScrView_RankAwardDesc.ResetPosition();
        yield return null;
        InitMyAward();
        yield return null;
        InitSelfRankAwardItem();
        view.Gobj_UnionRankAward.gameObject.SetActive(false);
        //yield return null;
        //InitUnionRankAwardItem();  
    }

    private void InitMyAward()
    {
        view.Spt_RankSprite.height = 80;

        if (selfAwardItem == null)
        {
            selfAwardItem = new PVPGetAwradsGroupComponet();
            selfAwardItem.MyStart(view.Gobj_SelfGetAwards);
        }
        if (unionAwardItem == null)
        {
            unionAwardItem = new PVPGetAwradsGroupComponet();
            unionAwardItem.MyStart(view.Gobj_UnionGetAwards);
        }

        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
        {
            selfAwardItem.mRootObject.SetActive(false);
            unionAwardItem.mRootObject.SetActive(false);
            view.Gobj_BattleTime.SetActive(true);
            view.Spt_RankSprite.height += 70;
        }
        else
        {
            view.Gobj_BattleTime.SetActive(false);
            selfAwardItem.mRootObject.SetActive(true);
            /////////////////////////////注释全服霸主的军团奖励
            unionAwardItem.mRootObject.SetActive(false);

            int selfRank = (int)SupermacyModule.Instance.SelfRankInfo.Rank;
            if (selfRank > 0)
            {
                view.Lbl_SelfRankDesc.text = string.Format(ConstString.SUPERMACY_SELFRANK, selfRank, "");
                selfAwardItem.Gobj_Award.SetActive(true);
                /////////////////////
                selfAwardItem.UpdateInfo(ConfigManager.Instance.mServerHegemonyData.GetSelfRankAwardInfoByRank(selfRank));
                //////////////////////
                view.Spt_RankSprite.height += 180;
            }
            else
            {
                view.Lbl_SelfRankDesc.text = string.Format(ConstString.SUPERMACY_SELFRANK, ConstString.RANK_LABEL_OUTRANK, ConstString.HINT_NO);
                selfAwardItem.Gobj_Award.SetActive(false);
                view.Spt_RankSprite.height += 80;

                unionAwardItem.mRootObject.transform.localPosition = new Vector3(0, -100, 0);
            }
            /*
            int unionRank = (int)SupermacyModule.Instance.unionRank;
            if (unionRank > 0)
            {
                view.Lbl_UnionRankDesc.text = string.Format(ConstString.SUPERMACY_UNIONRANK, unionRank, "");
                unionAwardItem.Gobj_Award.SetActive(true);
                unionAwardItem.UpdateInfo(ConfigManager.Instance.mServerHegemonyData.GetUnionRankAwardInfoByRank(unionRank));
                view.Spt_RankSprite.height += 180;
            }
            else
            {
                view.Lbl_UnionRankDesc.text = string.Format(ConstString.SUPERMACY_UNIONRANK, ConstString.HINT_NO, ConstString.HINT_NO);
                unionAwardItem.Gobj_Award.SetActive(false);
                view.Spt_RankSprite.height += 80;
            }
            */
        }

        view.Tbl_RankAward.Reposition();
    }

    public void InitSelfRankAwardItem()
    {
        if (selfRankAwardsItem == null)
        {
            selfRankAwardsItem = new PVPRankAwardDescGroupComponent();
            selfRankAwardsItem.MyStart(view.Gobj_SelfRankGetAwards);
        }
        ///////////////////////////
        selfRankAwardsItem.UpdateInfo(ConfigManager.Instance.mServerHegemonyData.GetSelfRankAwardList());
        //////////////////////////
        view.Tbl_RankAward.Reposition();
    }

    public void InitUnionRankAwardItem()
    {
        if (unionRankAwardsItem == null)
        {
            unionRankAwardsItem = new PVPRankAwardDescGroupComponent();
            unionRankAwardsItem.MyStart(view.Gobj_UnionRankGetAwards);
        }
        ///////////////////////////
        unionRankAwardsItem.UpdateInfo(ConfigManager.Instance.mServerHegemonyData.GetUnionRankAwardList());
        //////////////////////////
        view.Tbl_RankAward.Reposition();
    }

    /// <summary>
    /// 主信息更新函数
    /// </summary>
    public void UpdateInfo()
    {
        isReadyToBattle = false;
        CDTime = SupermacyModule.Instance.ChallengesCDTime;
        supermacyPlayer = SupermacyModule.Instance.SupermacyPlayer;
        //startTime= CommonFunction.GetTimeInt(SupermacyModule.Instance.OpenTime);
        //Debug.LogError(startTime+"  "+CurrentTime);
        HpUpdate();
        SupermacyUpdate();
        UpdateChallengeCD();
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
        {
            AutoBattleCheck();
            Scheduler.Instance.AddTimer(15f, true, HpUpdate);
            if (Main.mTime < CDTime)
            {
                Scheduler.Instance.AddTimer(1f,true,UpdateChallengeCD);
            }
            else
            {
                Scheduler.Instance.RemoveTimer(UpdateChallengeCD);
            }
        }
        else
        {
            ShowEndInfo();
        }

        Scheduler.Instance.AddTimer(1f, true, TimeUpdate);
        if (view != null)
        {
            ShowSelfOrder();
            ShowOrderList();
            ShowAwardList();
            ShowUIStatus();
        }

    }
    /// <summary>
    /// 退出界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Exit(GameObject btn)
    {
        //if (isAutoClearCD)
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_AUTOCLEARCDLOSEEFFECT);
        DisAutoBattle();
        DisAutoClearCD();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SUPERMACY);
    }
    /// <summary>
    /// 打开规则界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_RuleDesc(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.Gobj_SupermacyRule.SetActive(true);
        view.Lbl_RuleDesc.transform.localPosition = Vector3.up * (view.UIPanel_RuleDesc.height - view.Lbl_RuleDesc.height-20) / 2;
    }
    /// <summary>
    /// 关闭规则界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_RuleExit(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.Gobj_SupermacyRule.SetActive(false);
    }
    /// <summary>
    /// 朝拜霸主
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_WorshipBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        SupermacyModule.Instance.SendWorship();
    }
    /// <summary>
    /// 挑战霸主
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ChallengeBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        //Debug.LogError(SupermacyModule.Instance.StartResult);
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
        {
            if (CDTime <= Main.mTime)
            {
                SupermacyModule.Instance.SendChallenge();
            }
            else
            {
                if (isAutoClearCD)
                {
                    AutoClearCDCheck();
                }
                else
                {
                    if (serverHegemonyInfo != null)
                    {
                        string tmpStr = "";
                        if (serverHegemonyInfo.clear_cd_diamond.coinType == 2)
                            tmpStr = string.Format(ConstString.PVP_CLEARCHALENGECD, serverHegemonyInfo.clear_cd_diamond.price);
                        else if (serverHegemonyInfo.clear_cd_diamond.coinType == 1)
                            tmpStr = string.Format(ConstString.PVP_CLEARCHALENGECDBYGOLD, serverHegemonyInfo.clear_cd_diamond.price);
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, tmpStr, () =>
                        {
                            if (CDTime <= Main.mTime)
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CLEARCD_TIMEOUT);
                            }
                            else
                            {
                                if (CommonFunction.CheckMoneyEnough((ECurrencyType)serverHegemonyInfo.clear_cd_diamond.coinType, serverHegemonyInfo.clear_cd_diamond.price, true))
                                    SupermacyModule.Instance.SendClearSupermacyCD();
                            }
                        });
                    }
                }               
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_NOTSTART);
        }
            

    }
    /// <summary>
    /// 铜钱购买buff
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ByMoneyBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
            SupermacyModule.Instance.SendBuyBuff(ECurrencyType.Gold);
        else UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_CANTBUYBUFF);
    }
    /// <summary>
    /// 元宝购买buff
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ByDiamondBtn(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        //SupermacyModule.Instance.SendBuyBuff(ECurrencyType.Diamond);
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
            SupermacyModule.Instance.SendBuyBuff(ECurrencyType.Diamond);
        else UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_CANTBUYBUFF);
    }
    /// <summary>
    /// 勾选或取消自动战斗
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_BattleCheck(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
        {
            AutoBattle();
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_NOTSTART);
        }

    }
    /// <summary>
    /// 勾选或取消自动清理CD
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ClearCDCheck(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
        {
            AutoClearCD();
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_NOTSTART);
        }
        
    }
    /// <summary>
    /// 打開排行獎勵界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_OpenRankAward(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        //if (view.Gobj_RankAward != null)
        //{
        //    ShowRandAward();
        //    view.Gobj_RankAward.SetActive(true);          
        //}
        if (view.Gobj_RankAwardNew != null)
        {
            //InitRankAwardUI();
            Main.Instance.StartCoroutine(InitRankAwardUI());
            view.Gobj_RankAwardNew.SetActive(true);
        }
    }
    /// <summary>
    /// 關閉排行獎勵界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ExitRankAward(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        //InitRankAward();
        //InitRankAwardUI();
        Main.Instance.StartCoroutine(InitRankAwardUI());
        view.Gobj_RankAwardNew.SetActive(false);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Exit.gameObject).onClick = ButtonEvent_Exit;
        UIEventListener.Get(view.Btn_RuleDesc.gameObject).onClick = ButtonEvent_RuleDesc;
        UIEventListener.Get(view.Btn_WorshipBtn.gameObject).onClick = ButtonEvent_WorshipBtn;
        UIEventListener.Get(view.Btn_ChallengeBtn.gameObject).onClick = ButtonEvent_ChallengeBtn;
        UIEventListener.Get(view.Btn_ByGoldBtn.gameObject).onClick = ButtonEvent_ByMoneyBtn;
        UIEventListener.Get(view.Btn_ByDiamondBtn.gameObject).onClick = ButtonEvent_ByDiamondBtn;
        UIEventListener.Get(view.Btn_BattleCheck.gameObject).onClick = ButtonEvent_BattleCheck;
        UIEventListener.Get(view.Btn_ClearCDCheck.gameObject).onClick = ButtonEvent_ClearCDCheck;
        //UIEventListener.Get(view.Btn_RuleExit.gameObject).onClick = ButtonEvent_RuleExit;
        UIEventListener.Get(view.Gobj_RuleMask.gameObject).onClick = ButtonEvent_RuleExit;
        UIEventListener.Get(view.Btn_Rank.gameObject).onClick = ButtonEvent_OpenRankAward;
        //UIEventListener.Get(view.Gobj_RA_Mask.gameObject).onClick = ButtonEvent_ExitRankAward;
        UIEventListener.Get(view.Gobj_Mask.gameObject).onClick = ButtonEvent_ExitRankAward;
    }

    //------------------------------------------功能函数---------------------------------------------//

    /// <summary>
    /// 自动战斗状态检查
    /// </summary>
    public void AutoBattleCheck()
    {
        SupermacyModule.Instance.isAutoBattle = isAutoBattle;
        view.Gobj_BattleGetCheck.SetActive(isAutoBattle);
        if (isAutoBattle)
        {
            if (serverHegemonyInfo == null) return;
            //Debug.LogError(CDTime);
            if (isAutoClearCD && CommonFunction.CheckMoneyEnough((ECurrencyType)serverHegemonyInfo.clear_cd_diamond.coinType, serverHegemonyInfo.clear_cd_diamond.price, true))
            {
                if (CDTime > Main.mTime)
                {
                    Scheduler.Instance.RemoveTimer(AutoBattleCheck);
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.SUPERMACY_AUTOBATTLE, serverHegemonyInfo.wait_time));
                    Scheduler.Instance.AddTimer(serverHegemonyInfo.wait_time, false, AutoClearCDCheck);
                    isReadyToBattle = true;
                    waitTime = serverHegemonyInfo.wait_time;
                    Main.Instance.StartCoroutine(WaitTimeCount());
                }
                else
                {
                    Scheduler.Instance.RemoveTimer(AutoBattleCheck);
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.SUPERMACY_AUTOBATTLE, serverHegemonyInfo.wait_time));
                    Scheduler.Instance.AddTimer(serverHegemonyInfo.wait_time, false, SupermacyModule.Instance.SendChallenge);
                    waitTime = serverHegemonyInfo.wait_time;
                    Main.Instance.StartCoroutine(WaitTimeCount());
                }
                            
            }
            else
            {
                DisAutoClearCD();
                if (CDTime <= Main.mTime)
                {
                    Scheduler.Instance.RemoveTimer(AutoBattleCheck);
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.SUPERMACY_AUTOBATTLE, serverHegemonyInfo.wait_time));
                    Scheduler.Instance.AddTimer(serverHegemonyInfo.wait_time, false, SupermacyModule.Instance.SendChallenge);
                    waitTime = serverHegemonyInfo.wait_time;
                    Main.Instance.StartCoroutine(WaitTimeCount());
                }
                else
                {
                    Scheduler.Instance.AddTimer(1f, true, AutoBattleCheck);
                }
            }

        }
        else
        {
            isReadyToBattle = false;
            Scheduler.Instance.RemoveTimer(AutoBattleCheck);
            Scheduler.Instance.RemoveTimer(AutoClearCDCheck);
            Scheduler.Instance.RemoveTimer(SupermacyModule.Instance.SendChallenge);
            waitTime = 0;
            Main.Instance.StartCoroutine(WaitTimeCount());
        }

    }
    /// <summary>
    /// 自动战斗勾选
    /// </summary>
    private void AutoBattle()
    {
        if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.SupermacyAutoBattle, true))
        {
            if (isAutoBattle)
            {
                DisAutoBattle();
            }
            else
            {
                if (SupermacyModule.Instance.soldiers != null && SupermacyModule.Instance.soldiers.Count > 0)
                {
                    isAutoBattle = true;
                    AutoBattleCheck();
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_NOSOLDIERS);
                }
            }          
        }    
    }
    /// <summary>
    /// 取消自动战斗勾选
    /// </summary>
    public void DisAutoBattle()
    {
        isAutoBattle = false;
        if(view.Gobj_BattleGetCheck!=null&&view.Gobj_BattleGetCheck.activeSelf)
            view.Gobj_BattleGetCheck.SetActive(false);
        SupermacyModule.Instance.isAutoBattle = false;
        isReadyToBattle = false;
        Scheduler.Instance.RemoveTimer(AutoBattleCheck);
        Scheduler.Instance.RemoveTimer(AutoClearCDCheck);
        Scheduler.Instance.RemoveTimer(SupermacyModule.Instance.SendChallenge);
        waitTime = 0;
        Main.Instance.StartCoroutine(WaitTimeCount());
    }

    /// <summary>
    /// 自动清除CD状态检查
    /// </summary>
    private void AutoClearCDCheck()
    {
        if (isAutoClearCD)
        {
            if (serverHegemonyInfo == null) return;
            if(CDTime > Main.mTime)
            {
                if (CommonFunction.CheckMoneyEnough((ECurrencyType)serverHegemonyInfo.clear_cd_diamond.coinType, serverHegemonyInfo.clear_cd_diamond.price, true))
                {
                    SupermacyModule.Instance.SendClearSupermacyCD();
                }
                else
                {
                    isAutoClearCD = false;
                    view.Gobj_CDGetCheck.SetActive(false);
                }
            }
            else
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CLEARCD_TIMEOUT);
                SupermacyModule.Instance.SendChallenge();
            }
        }
        else
        {
            Scheduler.Instance.RemoveTimer(AutoClearCDCheck);
            if (isReadyToBattle && isAutoBattle )
            {
                Scheduler.Instance.AddTimer(1, true, AutoBattleCheck);
                isReadyToBattle = false;
            }
        }
    }
    /// <summary>
    /// 自动清除CD勾选
    /// </summary>
    private void AutoClearCD()
    {
        if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.supermacyAutoClearCD, true))
        {
            isAutoClearCD = isAutoClearCD ? false : true;
        }
        view.Gobj_CDGetCheck.SetActive(isAutoClearCD);
        ShowCDTIMEInfo();
        if (isReadyToBattle && isAutoBattle && !isAutoClearCD && CDTime > Main.mTime)
        {
            waitTime = 0;
            Main.Instance.StartCoroutine(WaitTimeCount());
        }
    }
    /// <summary>
    /// 取消自动清除CD勾选
    /// </summary>
    public void DisAutoClearCD()
    {
        isAutoClearCD = false;
        if (view.Gobj_CDGetCheck.activeSelf)
            view.Gobj_CDGetCheck.SetActive(false);
        Scheduler.Instance.RemoveTimer(AutoClearCDCheck);
    }


    /// <summary>
    /// 等待进入战斗倒计时
    /// </summary>
    /// <param name="t"></param>
    /// <returns></returns>
    public IEnumerator WaitTimeCount()
    {
        if (serverHegemonyInfo != null)
        {
            while (waitTime > 0)
            {
                CommonFunction.SetBtnState(view.Btn_ChallengeBtn.gameObject, false);
                view.Lbl_BtnChallengeBtnChallengDesc.text = string.Format(ConstString.SUPERMACT_WAITFORBATTLE, waitTime);
                yield return new WaitForSeconds(1);
                waitTime--;
            }
            CommonFunction.SetBtnState(view.Btn_ChallengeBtn.gameObject, true);
            view.Lbl_BtnChallengeBtnChallengDesc.text = ConstString.SUPERMACY_CHALLENGE;
            yield return null;
        }
        else
        {
            Debug.LogError("config_severhegemony has mistake");
            CommonFunction.SetBtnState(view.Btn_ChallengeBtn.gameObject, true);
            view.Lbl_BtnChallengeBtnChallengDesc.text = ConstString.SUPERMACY_CHALLENGE;
            yield return null;
        }
    }

    /// <summary>
    /// 清除CD并进入战斗
    /// </summary>
    public void ClearCDTimeSuccess()
    {
        CDTime = (uint)Main.mTime;
        SupermacyModule.Instance.SendChallenge();
    }

    public void BuyBuffOperate(BuyBuffResp vData)
    {
        if (vData != null)
        {
            if (vData.result == (int)ErrorCodeEnum.SUCCESS)
            {
                for (int i = 0; i < serverHegemonyInfo.clear_buff_diamond.Count; i++)
                {
                    if (serverHegemonyInfo.clear_buff_diamond[i] != null)
                    {
                        if (serverHegemonyInfo.clear_buff_diamond[i].CoinType == (int)ECurrencyType.Gold && SupermacyModule.Instance.BuyBuffCoinType == ECurrencyType.Gold)
                        {
                            if (serverHegemonyInfo.clear_buff_diamond[i].Num == SupermacyModule.Instance.BUFFBuyTime_Gold)
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_GOLDBUYBUFFFINISHED);
                            else
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUY_SUCCESS);
                        }
                        else if (serverHegemonyInfo.clear_buff_diamond[i].CoinType == (int)ECurrencyType.Diamond && SupermacyModule.Instance.BuyBuffCoinType == ECurrencyType.Diamond)
                        {
                            if (serverHegemonyInfo.clear_buff_diamond[i].Num == SupermacyModule.Instance.BUFFBuyTime_Diamond)
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_DIAMONDBUYBUFFFINISHED);
                            else
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUY_SUCCESS);
                        }
                    }
                }
                ShowBuff();
                if (SupermacyModule.Instance.BuyBuffCoinType == ECurrencyType.Gold)
                {
                    PlayerData.Instance.UpdateGold(vData.money_value);
                }
                else if (SupermacyModule.Instance.BuyBuffCoinType == ECurrencyType.Diamond)
                {
                    PlayerData.Instance.UpdateDiamond(vData.money_value);
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(vData.result);
            }
        }
    }
    public void WorshipOperate(WorshipResp vData)
    {
        if (vData != null)
        {
            if (vData.result == (int)ErrorCodeEnum.SUCCESS)
            {
                //CommonFunction.SetBtnState(view.Btn_WorshipBtn.gameObject, false);
                if (vData.drop_list != null)
                {
                    PlayerData.Instance.UpdateDropData(vData.drop_list);
                    List<CommonItemData> tmpListInfo = CommonFunction.GetCommonItemDataList(vData.drop_list);
                    if (tmpListInfo != null)
                    {
                        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
                        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(tmpListInfo);
                    }
                    if ((vData.drop_list.mail_list != null) && (vData.drop_list.mail_list.Count > 0))
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SUPERMACY_ITEMTOMAIL);
                    }
                }
            }
            else
            {
                ErrorCode.ShowErrorTip(vData.result);
            }
        }
        ShowWorship();
    }

    //---------------------------------------数据更新函数--------------------------------------------//

    /// <summary>
    /// 霸主信息更新
    /// </summary>
    private void SupermacyUpdate()
    {
        //Debug.LogError(supermacyPlayer.hero);
        
        try 
        {
            if (supermacyPlayer.hero.gender == 1)// 男性
            {
                view.Gobj_SupermacyRole.SetActive(true);
                view.Gobj_FemaleRole.SetActive(false);
            }
            else// 女性或无性别
            {
                view.Gobj_SupermacyRole.SetActive(false);
                view.Gobj_FemaleRole.SetActive(true);
            }
            view.Lbl_NameText.text = supermacyPlayer.hero.charname;
            if (supermacyPlayer.hero.accid == PlayerData.Instance._AccountID)
            {
                view.Lbl_BtnWorshipBtnWorshipDesc.text = ConstString.SUPERMACY_WORSHIPSELF;
                view.Lbl_WDes.text = serverHegemonyInfo.tribute_award_desc;
                view.Lbl_WTittle.text = ConstString.SUPERMACY_TRIBUTEAWARD;
            }
            else
            {
                view.Lbl_BtnWorshipBtnWorshipDesc.text = ConstString.SUPERMACY_WORSHIPSUPERMACY;
                view.Lbl_WDes.text = serverHegemonyInfo.worship_award_desc;
                view.Lbl_WTittle.text = ConstString.SUPERMACY_WORSHIPAWARD;
            }
        }
        catch
        {
            Debug.LogError("supermacy info has something wrong");
            view.Gobj_SupermacyRole.SetActive(true);
            view.Gobj_FemaleRole.SetActive(false);
            view.Lbl_NameText.text = ConstString.SUPERMACY_SUPERMACY;
            view.Lbl_BtnWorshipBtnWorshipDesc.text = ConstString.SUPERMACY_WORSHIPSUPERMACY;
            view.Lbl_WDes.text = serverHegemonyInfo.worship_award_desc;
            view.Lbl_WTittle.text = ConstString.SUPERMACY_WORSHIPAWARD;
        }
    }
    /// <summary>
    /// 更新倒计时及倒计时图片
    /// </summary>
    private void TimeUpdate()
    {
        ShowTimeInfo();
        if (restTime <= 0  && UISystem.Instance.UIIsOpen(SupermacyView.UIName))
        {
            reqcount++;
            if (reqcount > 4) return;
            SupermacyModule.Instance.SendEnterOverlord();
            if (SupermacyModule.Instance.StartResult != (int)ErrorCodeEnum.SUCCESS)
            {
                ShowEndInfo();
            }
        }

        if (SupermacyModule.Instance.needRefresh)
        {
            SupermacyModule.Instance.SendEnterOverlord();
            if (SupermacyModule.Instance.StartResult != (int)ErrorCodeEnum.SUCCESS)
            {
                ShowEndInfo();
            }
        }
    }
    /// <summary>
    /// CD倒计时
    /// </summary>
    private void UpdateChallengeCD()
    {
        ShowCDTIMEInfo();
        if (CDTime <= Main.mTime)
            Scheduler.Instance.RemoveTimer(UpdateChallengeCD);
    }
    /// <summary>
    /// 更新霸主血条
    /// </summary>
    private void HpUpdate()
    {
        if (SupermacyModule.Instance.StartResult!=(int)ErrorCodeEnum.SUCCESS)
        {
            view.Lbl_CurHP.text = ConstString.SUPERMACY_MAXHP;
            view.Slider_HPSlider.value = 1;
        }
        else
        {
            view.Slider_HPSlider.value = SupermacyModule.Instance.HPValue;
            view.Lbl_CurHP.text = ((int)(100 * SupermacyModule.Instance.HPValue)).ToString() + "%";
        }
    }

    //---------------------------------------界面显示函数--------------------------------------------//

    /// <summary>
    /// 顯示界面
    /// </summary>
    private void ShowUIStatus()
    {
        ShowBuff();
        ShowWorship();
        ShowChallenge();
        ShowTimeInfo();
        ShowChatView();
    }
    private void ShowSelfOrder()
    {
        if (view.Lbl_MyOrder != null)
        {
            if (SupermacyModule.Instance.SelfRankInfo.Rank == 0)
            {
                view.Lbl_MyOrder.text = ConstString.RANK_LABEL_OUTRANK;
            }
            else
            {
                view.Lbl_MyOrder.text = string.Format(ConstString.SUPERMACY_RANK, SupermacyModule.Instance.SelfRankInfo.Rank);
            }
        }
        if (view.Lbl_MyNick != null)
        {
            view.Lbl_MyNick.text = SupermacyModule.Instance.SelfRankInfo.Nick;
        }
        if (view.Lbl_MyDamage != null)
        {
            view.Lbl_MyDamage.text = string.Format(ConstString.SUPERMACY_HURT, SupermacyModule.Instance.SelfRankInfo.Hurt);
        }
    }
    private void ShowOrderList()
    {
        if (listOrder == null)
        {
            listOrder = new List<OrderItem>();
        }
        List<int> tmpNullIndex = new List<int>();
        for (int i = 0; i < listOrder.Count; i++)
        {
            if (listOrder[i] != null)
            {
                listOrder[i].InitInfo();
            }
            else
            {
                tmpNullIndex.Add(i);
            }
        }
        if (tmpNullIndex.Count > 0)
        {
            for (int i = tmpNullIndex.Count - 1; i >= 0; i--)
            {
                if ((tmpNullIndex[i] >= 0) && (tmpNullIndex[i] < listOrder.Count))
                {
                    listOrder.RemoveAt(tmpNullIndex[i]);
                }
            }
        }

        if (view.Obj_Order != null)
        {
            view.Obj_Order.gameObject.SetActive(false);
            int tmpIndex = 0;
            int tmpCount = SupermacyModule.Instance.ListRankInfo.Count;
            for (int i = 0; i < tmpCount; i++)
            {
                if (tmpIndex >= listOrder.Count)
                {
                    tmpIndex = listOrder.Count;
                    OrderItem tmpItem = new OrderItem(view.Obj_Order);
                    listOrder.Add(tmpItem);
                }
                if (listOrder[tmpIndex].ReSetInfo(tmpIndex, SupermacyModule.Instance.ListRankInfo[i].Rank, SupermacyModule.Instance.ListRankInfo[i].Nick, SupermacyModule.Instance.ListRankInfo[i].Hurt))
                {
                    tmpIndex++;
                }
            }

            //修改BoxCollider//
            if (tableBoxOrder != null)
            {
                float tmpPoxY = INIT_ORDER_BOXCOLLIDER_CENTER_Y;
                float tmpSizeY = INIT_ORDER_BOXCOLLIDER_SIZE_Y;
                if (tmpIndex > 0)
                {
                    tmpSizeY = DISTANCE_ORDERITEM * (tmpIndex + 1);
                    if (tmpSizeY < INIT_ORDER_BOXCOLLIDER_SIZE_Y)
                    {
                        tmpSizeY = INIT_ORDER_BOXCOLLIDER_SIZE_Y;
                    }
                    else
                    {
                        tmpPoxY = (INIT_ORDER_BOXCOLLIDER_SIZE_Y - tmpSizeY) / 2;
                    }
                }
                tableBoxOrder.center = new Vector3(0, tmpPoxY, 0);
                tableBoxOrder.size = new Vector3(INIT_ORDER_BOXCOLLIDER_SIZE_X, tmpSizeY, 0);
            }
        }
    }
    private void ShowAwardList()
    {
        if (listAward == null)
        {
            listAward = new List<AwardItem>();
        }
        List<int> tmpNullIndex = new List<int>();
        for (int i = 0; i < listAward.Count; i++)
        {
            if (listAward[i] != null)
            {
                listAward[i].InitInfo();
            }
            else
            {
                tmpNullIndex.Add(i);
            }
        }
        if (tmpNullIndex.Count > 0)
        {
            for (int i = tmpNullIndex.Count - 1; i >= 0; i--)
            {
                if ((tmpNullIndex[i] >= 0) && (tmpNullIndex[i] < listAward.Count))
                {
                    listAward.RemoveAt(tmpNullIndex[i]);
                }
            }
        }

        if (view.Obj_Award != null)
        {
            view.Obj_Award.gameObject.SetActive(false);
            int tmpIndex = 0;
            int tmpCount = SupermacyModule.Instance.ListAwardInfo.Count;
            for (int i = 0; i < tmpCount; i++)
            {
                if (tmpIndex >= listAward.Count)
                {
                    tmpIndex = listAward.Count;
                    AwardItem tmpItem = new AwardItem(view.Obj_Award);
                    listAward.Add(tmpItem);
                }
                if (listAward[tmpIndex].ReSetInfo(tmpIndex, SupermacyModule.Instance.ListAwardInfo[i], (i == tmpCount - 1)))
                {
                    tmpIndex++;
                }
            }

            //修改BoxCollider//
            if (tableBoxAward != null)
            {
                float tmpPoxY = INIT_AWARD_BOXCOLLIDER_CENTER_Y;
                float tmpSizeY = INIT_AWARD_BOXCOLLIDER_SIZE_Y;
                if (tmpIndex > 0)
                {
                    tmpSizeY = DISTANCE_AWARDITEM * (tmpIndex + 1);
                    if (tmpSizeY < INIT_AWARD_BOXCOLLIDER_SIZE_Y)
                    {
                        tmpSizeY = INIT_AWARD_BOXCOLLIDER_SIZE_Y;
                    }
                    else
                    {
                        tmpPoxY = (INIT_AWARD_BOXCOLLIDER_SIZE_Y - tmpSizeY) / 2;
                    }
                }
                tableBoxAward.center = new Vector3(0, tmpPoxY, 0);
                tableBoxAward.size = new Vector3(INIT_AWARD_BOXCOLLIDER_SIZE_X, tmpSizeY, 0);
            }
        }
    }
    private void ShowRandAward()
    {
        if (listRankItem == null)
        {
            listRankItem = new List<SingleRankItem>();
        }
        List<int> tmpNullIndex = new List<int>();
        for (int i = 0; i < listRankItem.Count; i++)
        {
            if (listRankItem[i] != null)
            {
                listRankItem[i].InitInfo();
            }
            else
            {
                tmpNullIndex.Add(i);
            }
        }
        if (tmpNullIndex.Count > 0)
        {
            for (int i = tmpNullIndex.Count - 1; i >= 0; i--)
            {
                if ((tmpNullIndex[i] >= 0) && (tmpNullIndex[i] < listRankItem.Count))
                {
                    listRankItem.RemoveAt(tmpNullIndex[i]);
                }
            }
        }

        if (view.Obj_RankItem != null)
        {
            view.Obj_RankItem.gameObject.SetActive(false);
            int tmpIndex = 0;
            int tmpCount = SupermacyModule.Instance.ListAwardInfo.Count;
            int tmpPosY = 140;
            for (int i = 0; i < tmpCount; i++)
            {
                if (tmpIndex >= listRankItem.Count)
                {
                    tmpIndex = listRankItem.Count;
                    SingleRankItem tmpItem = new SingleRankItem(view.Obj_RankItem);
                    listRankItem.Add(tmpItem);
                }

                if (i == tmpCount - 1)
                {
                    tmpPosY -= listRankItem[tmpIndex].ReSetValue(-1, SupermacyModule.Instance.ListAwardInfo[i], tmpPosY);
                }
                else
                {
                    tmpPosY -= listRankItem[tmpIndex].ReSetValue(tmpIndex, SupermacyModule.Instance.ListAwardInfo[i], tmpPosY);
                }
                tmpIndex++;
            }
        }
    }
    private void ShowBuff()
    {
        if (serverHegemonyInfo != null)
        {
            if (serverHegemonyInfo.clear_buff_diamond != null)
            {
                for (int i = 0; i < serverHegemonyInfo.clear_buff_diamond.Count; i++)
                {
                    if (serverHegemonyInfo.clear_buff_diamond[i] != null)
                    {
                        if (serverHegemonyInfo.clear_buff_diamond[i].CoinType == (int)ECurrencyType.Gold)
                        {
                            view.Lbl_BtnByGoldBtnGoldPrice.text = serverHegemonyInfo.clear_buff_diamond[i].Price.ToString();
                            view.Lbl_BtnByGoldBtnGoldHint.text = string.Format(ConstString.SUPERMACY_BUYCOUNT, SupermacyModule.Instance.BUFFBuyTime_Gold, serverHegemonyInfo.clear_buff_diamond[i].Num);
                            if (serverHegemonyInfo.clear_buff_diamond[i].Num > SupermacyModule.Instance.BUFFBuyTime_Gold&& SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
                            {
                                if(view.Btn_ByGoldBtn.gameObject!=null)
                                    CommonFunction.SetBtnState(view.Btn_ByGoldBtn.gameObject, true);
                            }
                            else
                            {
                                if (view.Btn_ByGoldBtn.gameObject != null)
                                    CommonFunction.SetBtnState(view.Btn_ByGoldBtn.gameObject, false);
                            }
                        }
                        else if (serverHegemonyInfo.clear_buff_diamond[i].CoinType == (int)ECurrencyType.Diamond)
                        {
                            view.Lbl_BtnByDiamondBtnDiamondPrice.text = serverHegemonyInfo.clear_buff_diamond[i].Price.ToString();
                            view.Lbl_BtnByDiamondBtnDiamondHint.text = string.Format(ConstString.SUPERMACY_BUYCOUNT, SupermacyModule.Instance.BUFFBuyTime_Diamond, serverHegemonyInfo.clear_buff_diamond[i].Num);
                            if (serverHegemonyInfo.clear_buff_diamond[i].Num > SupermacyModule.Instance.BUFFBuyTime_Diamond&& SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
                            {
                                if (view.Btn_ByDiamondBtn.gameObject != null)
                                    CommonFunction.SetBtnState(view.Btn_ByDiamondBtn.gameObject, true);
                            }
                            else
                            {
                                if (view.Btn_ByDiamondBtn.gameObject != null)
                                    CommonFunction.SetBtnState(view.Btn_ByDiamondBtn.gameObject, false);
                            }
                        }
                    }
                }
            }
            view.Lbl_EffectLabel.text = string.Format("{0}%", SupermacyModule.Instance.BUFFLevel * serverHegemonyInfo.buff_effect_rate);
        }
    }
    private void ShowWorship()
    {
        CommonFunction.SetBtnState(view.Btn_WorshipBtn.gameObject, !SupermacyModule.Instance.IsWorship());
        view.Gobj_GotTip.gameObject.SetActive(SupermacyModule.Instance.IsWorship());
        view.Gobj_Notice.gameObject.SetActive(!SupermacyModule.Instance.IsWorship());
    }
    private void ShowChallenge()
    {
        if(SupermacyModule.Instance.StartResult==(int)ErrorCodeEnum.SUCCESS)
            CommonFunction.SetBtnState(view.Btn_ChallengeBtn.gameObject, true);
        else CommonFunction.SetBtnState(view.Btn_ChallengeBtn.gameObject, false);
    }
    /// <summary>
    /// 显示右上角时间信息
    /// </summary>
    /// <param name="intStartTime"></param>
    private void ShowTimeInfo()
    {
        /*
        if (SupermacyModule.Instance.CurrentTime <= intStartTime)
        {
            view.Lbl_TimeLabel.text = CommonFunction.GetTimeString(intStartTime - SupermacyModule.Instance.CurrentTime);
            view.Lbl_StateLabel.text = ConstString.SUPERMACY_TIMESTATE1;
        }
        else if (SupermacyModule.Instance.CurrentTime > intStartTime + SupermacyModule.Instance.ContTime)
        {
            view.Lbl_TimeLabel.text = CommonFunction.GetTimeString(86400 + intStartTime - SupermacyModule.Instance.CurrentTime);
            view.Lbl_StateLabel.text = ConstString.SUPERMACY_TIMESTATE1;
        }
        else
        {
            view.Lbl_TimeLabel.text = CommonFunction.GetTimeString(intStartTime + SupermacyModule.Instance.ContTime - SupermacyModule.Instance.CurrentTime);
            view.Lbl_StateLabel.text = ConstString.SUPERMACY_TIMESTATE2;
        }
        */
        //DateTime timePoint = CommonFunction.GetTimeByLong(SupermacyModule.Instance.ContTime);
        //DateTime currentTime = CommonFunction.GetTimeByLong(Main.mTime);
        //TimeSpan ts = timePoint.Subtract(currentTime);
        restTime = SupermacyModule.Instance.ContTime - Main.mTime;
        view.Lbl_TimeLabel.text = CommonFunction.GetTimeString(restTime);

        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS)
        {
            view.Lbl_StateLabel.text = ConstString.SUPERMACY_TIMESTATE2;
        }
        else
        {
            view.Lbl_StateLabel.text = ConstString.SUPERMACY_TIMESTATE1;
        }
    }
    /// <summary>
    /// 活动结束信息刷新
    /// </summary>
    private void ShowEndInfo()
    {
        DisAutoBattle();
        DisAutoClearCD();       
        CDTime = (uint)Main.mTime;
        UpdateChallengeCD();
    }
    /// <summary>
    /// 显示CD时间
    /// </summary>
    private void ShowCDTIMEInfo()
    {
        if (SupermacyModule.Instance.StartResult == (int)ErrorCodeEnum.SUCCESS && CDTime > Main.mTime)
        {
            if (isAutoClearCD)
                view.Lbl_CDTIME.color = Color.green;
            else view.Lbl_CDTIME.color = Color.red;
            view.Lbl_CDTIME.text = string.Format(ConstString.SUPERMACT_CDTIME, CDTime - Main.mTime);
        }
        else view.Lbl_CDTIME.text = "";
    }
    /// <summary>
    /// 显示聊天窗口
    /// </summary>
    private void ShowChatView()
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.World);
    }
    //---------------------------------------------------------------------------------------//
    public override void Destroy()
    {
        selfAwardItem = null;
        unionAwardItem = null;
        selfRankAwardsItem = null;
        unionRankAwardsItem = null;

        base.Destroy();
     
        view.Gobj_BattleGetCheck = null;
        view.Gobj_CDGetCheck = null;
        view.Gobj_FemaleRole = null;
        view.Gobj_MainGroup = null;
        view.Gobj_SupermacyRole = null;
        view.Gobj_SupermacyRule = null;
        view.Gobj_RuleMask = null;
        view.Gobj_RankAward = null;
        view.Gobj_RA_Mask = null;
        view.Gobj_GotTip = null;
        view.Gobj_Notice = null;
        view.Gobj_RankAwardNew = null;
        view.Gobj_Mask = null;
        view.Gobj_UnionRankGetAwards = null;
        view.Gobj_SelfRankGetAwards = null;
        view.Gobj_SelfGetAwards = null;
        view.Gobj_UnionGetAwards = null;
        view.Gobj_BattleTime = null;

        view = null;
        if (listOrder != null)
        {
            listOrder.Clear();
            listOrder = null;
        }
        if (listAward != null)
        {
            listAward.Clear();
            listAward = null;
        }
        if (supermacyPlayer != null)
        {
            supermacyPlayer = null;
        }
        if (listRankItem != null)
        {
            listRankItem.Clear();
            listRankItem = null;
        }

    }
}
