using UnityEngine;
using System.Collections.Generic;

public class GuideManager : Singleton<GuideManager>
{
    private class GuideContent
    {
        public int StepIndex = -1;
        public GuideData GuideData;
        public GuideStep GuideStep;

        public GuideContent()
        {
            StepIndex = -1;
            GuideData = null;
            GuideStep = null;
        }

        public void Copy(GuideContent cnt)
        {
            StepIndex = cnt.StepIndex;
            GuideData = cnt.GuideData;
            GuideStep = cnt.GuideStep;
        }

        public void Clear()
        {
            StepIndex = -1;
            GuideData = null;
            GuideStep = null;
        }
    }
    private const int MAX_GUIDE_COUNT = 100;
    private List<GuideData> _undoGuides = null;
    private bool _guideIsRunning = false;

    private GuideContent _curGuide = new GuideContent();
    private GuideContent _waitGuide = new GuideContent();

    //private int _stepIndex = -1;
    //private GuideData _curGuide;
    //private GuideStep _curStep;

    private string _guideSound;
    private ScaleMainCity _scaleGuide = null;
    private bool _showDebug = false;
    public void Uninitialize()
    {
        _guideIsRunning = false;
        _curGuide.Clear();
        _waitGuide.Clear();
        _undoGuides = null;
    }

    public void ClearAndRestartGuide()
    {
        _guideIsRunning = false;
        _curGuide.Clear();
        _waitGuide.Clear();
        _undoGuides = null;
        CloseGuideShade();
        GuideManager.Instance.CheckTrigger(GuideTrigger.NeedSpecialFight);
    }

    public void CheckTrigger(GuideTrigger trigger)
    {
        if (!GlobalConst.IS_OPEN_GUIDE)
            return;
        //if (PlayerData.Instance.DoneNewbieGuideList.Count >= MAX_GUIDE_COUNT)
        //    return;
        GuideDebug("Guide id running= " + _guideIsRunning + " trigger =" + trigger.ToString());
        if (_guideIsRunning)
        {
            //检查是否需要插入半血引导
            if (trigger == GuideTrigger.HeroBlood60 && AllGuideItemIsClose())
            {
                _waitGuide.Copy(_curGuide);
                _curGuide.Clear();
                if (!GetStartGuide(trigger))
                {
                    _curGuide.Copy(_waitGuide);
                    _waitGuide.Clear();
                }
            }
            GetGuideStep(trigger);
        }
        else
        {
            // 遍历未完成的引导
            // 获取可执行的引导
            if (GetStartGuide(trigger))
            {
                MainCityModule.Instance.StartNewbieGuideReq(_curGuide.GuideData.mGuideType);
                GuideOfflineClearData();
                _guideIsRunning = true;
                GetGuideStep(trigger);

                if (UISystem.Instance.MainCityView != null)
                {
                    UISystem.Instance.MainCityView.EnableMainUIDrag(false);
                }
            }
        }
    }

    /// <summary>
    /// 获取可做引导：
    /// true 获取到可做引导 false -未获取到可做引导
    /// </summary>
    /// <param name="trigger"></param>
    /// <returns></returns>
    private bool GetStartGuide(GuideTrigger trigger)
    {
        GuideDebug("GetStartGuide _undoGuides==null :" + (_undoGuides == null));
        if (_undoGuides == null)
        {
            _undoGuides = new List<GuideData>();
            List<GuideData> allGuides = ConfigManager.Instance.mGuideConfig.GetAllGuideData();
            for (int i = 0; i < allGuides.Count; i++)
            {
                if (!PlayerData.Instance.DoneNewbieGuideList.Contains(allGuides[i].mGuideType))
                {
                    _undoGuides.Add(allGuides[i]);
                }
            }
        }

        for (int i = 0; i < _undoGuides.Count; i++)
        {
            if (_undoGuides[i].mTrigger == trigger)
            {
                GuideDebug("un do guide id=" + _undoGuides[i].mID + " " + CheckGuideLimit(_undoGuides[i]));
                if (CheckGuideLimit(_undoGuides[i]))
                {
                    _curGuide.GuideData = _undoGuides[i];
                    return true;
                }
            }
        }
        GuideDebug("GetStartGuide false");

        return false;
    }

    private void GetGuideStep(GuideTrigger trigger)
    {
        ++_curGuide.StepIndex;
        if (_curGuide.StepIndex >= _curGuide.GuideData.mSteps.Count)
        {
            Debug.LogError(_curGuide.GuideData.mID + " _curGuide.StepIndex >  " + _curGuide.GuideData.mSteps.Count);
            --_curGuide.StepIndex;
            return;
        }
        GuideDebug("_curGuide.StepIndex =" + _curGuide.StepIndex + " id=" + _curGuide.GuideData.mSteps[_curGuide.StepIndex]);
        GuideStep step = ConfigManager.Instance.mGuideConfig.GetStepByID(_curGuide.GuideData.mSteps[_curGuide.StepIndex]);

        if (step != null && step.mTrigger == trigger)
        {
            ShowGuide(step);
        }
        else
        {
            --_curGuide.StepIndex;
        }
    }

    // 显示引导的步骤
    private void ShowGuide(GuideStep step)
    {
        _curGuide.GuideStep = step;
        UISystem.Instance.ShowGameUI(GuideView.UIName);
        CloseGuideShade();
        GuideDebug("step =" + step.mID.ToString() + " delay=" + step.mDelay);
        ScaleGuideView(true);
        if (step.mDelay > 0)
        {
            UISystem.Instance.GuideView.SetCollider(Vector4.zero/*_curGuide.GuideStep.mActionRect[0]*/);
            Assets.Script.Common.Scheduler.Instance.AddTimer(step.mDelay, false, ShowGuideImmediately);
        }
        else
        {
            ShowGuideImmediately();
        }

        if (_curGuide.GuideStep.mNotTriggerAgain)
        {
            GuideDebug("guide finish =" + _curGuide.GuideStep.mGuideID + " next step =" + _curGuide.GuideStep.mNextStep);
            uint type = ConfigManager.Instance.mGuideConfig.GetGuideByID(_curGuide.GuideStep.mGuideID).mGuideType;
            PlayerData.Instance.AddGuideFinishList(type);
            MainCityModule.Instance.FinishNewbieGuideReq(type);

            for (int i = _undoGuides.Count - 1; i >= 0; i--)
            {
                if (_undoGuides[i].mGuideType == type)
                {
                    _undoGuides.RemoveAt(i);
                }
            }
        }

        if (_curGuide.GuideStep.mNextStep == 0)
        {
            if (_waitGuide != null && _waitGuide.StepIndex != -1)
            {
                _curGuide.Copy(_waitGuide);
                _waitGuide.Clear();
            }
            else
            {
                _guideIsRunning = false;
                _curGuide.StepIndex = -1;
                if (UISystem.Instance.MainCityView != null)
                {
                    UISystem.Instance.MainCityView.EnableMainUIDrag(true);
                }
            }

            if(_curGuide.GuideData.mGuideType == 
                uint.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.FORCE_GUIDE_LAST_ID)))
            {
                MainCityModule.Instance.DelayShowCommentView(false);
            }

        }
    }

    private void ShowGuideImmediately()
    {
        if (!UISystem.Instance.UIIsOpen(GuideView.UIName) || _curGuide == null)
            return;
        CloseGuideShade();
        CloseLastGuideSound();

        for (int i = 0; i < _curGuide.GuideStep.mStepType.Count; i++)
        {
            GuideDebug(_curGuide.GuideStep.mStepType[i].ToString());
            switch (_curGuide.GuideStep.mStepType[i])
            {
                case GuideStepType.Dialogue:
                    {
                        if (!UISystem.Instance.UIIsOpen(DialogueView.UIName))
                        {
                            UISystem.Instance.ShowGameUI(DialogueView.UIName);
                        }
                        UISystem.Instance.DialogueView.OpenDialogue(_curGuide.GuideStep.mDialogueID, DialogDone);
                        break;
                    }
                case GuideStepType.Desc:
                    {
                        UISystem.Instance.GuideView.SetDesc(_curGuide.GuideStep.mDescPos, _curGuide.GuideStep.mDescContent);
                        break;
                    }
                case GuideStepType.HighLight:
                    {
                        UISystem.Instance.GuideView.SetHighLight(_curGuide.GuideStep.mActionRect);
                        break;
                    }
                case GuideStepType.Arrow:
                    {
                        UISystem.Instance.GuideView.SetArrow(_curGuide.GuideStep.mArrowParas);
                        break;
                    }
                case GuideStepType.Finger:
                    {
                        UISystem.Instance.GuideView.SetFinger(_curGuide.GuideStep.mFingerParas);
                        break;
                    }
                case GuideStepType.Mask:
                    {
                        UISystem.Instance.GuideView.SetMaskPan(_curGuide.GuideStep.mActionRect[0], _curGuide.GuideStep.mMaskType);
                        break;
                    }
                case GuideStepType.Collider:
                    {
                        UISystem.Instance.GuideView.SetCollider(_curGuide.GuideStep.mCollider);
                        break;
                    }
                case GuideStepType.ScreenEffect:
                    {
                        UISystem.Instance.GuideView.SetWhiteScreen(_curGuide.GuideStep.mScreenEffectForward);
                        break;
                    }
                case GuideStepType.MainCityLocation:
                    {
                        if (UISystem.Instance.UIIsOpen(MainCityView.UIName))
                            UISystem.Instance.MainCityView.LocateMainCity(_curGuide.GuideStep.mMainCityLocation);
                        break;
                    }
                case GuideStepType.PauseFighting:
                    {
                        if (UISystem.Instance.UIIsOpen(FightView.UIName))
                        {
                            UISystem.Instance.FightView.SetFightPauseStatus((EFightPauseType)_curGuide.GuideStep.mPauseGamePara != EFightPauseType.noPause, (EFightPauseType)_curGuide.GuideStep.mPauseGamePara);
                        }
                        break;
                    }
                case GuideStepType.Close:
                    {
                        CloseGuideShade();
                        break;
                    }
                case GuideStepType.SpecialFightMovie:
                    {
                        PlaySpecialFightMovie();
                        break;
                    }
                case GuideStepType.SpecialFight:
                    {
                        EnterSpecialFight();
                        break;
                    }
                case GuideStepType.SpecialFightLimitMoveDir:
                    {
                        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ReSetMoveStatus, _curGuide.GuideStep.mSpecialFightLimitMoveDir);
                        break;
                    }
                case GuideStepType.SpecialFightSetSkillCanUse:
                    {
                        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_NewGuideCanUseSkill);
                        break;
                    }
                case GuideStepType.PlaySound:
                    {
                        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(_curGuide.GuideStep.mSound));
                        _guideSound = _curGuide.GuideStep.mSound;
                        break;
                    }
                case GuideStepType.GateGift:
                    {
                        GateModule.Instance.SendGuideGateGiftReq(_curGuide.GuideStep.mGateGift);
                        break;
                    }
                case GuideStepType.FightTalk:
                    {
                        TalkManager.Instance.OpenFightTalk(EChatType.ectNewGuide);
                        break;
                    }
                case GuideStepType.ShowFightSoldier:
                    {
                        if (_curGuide.GuideStep.mFightSoldier.Count < 2)
                            return;

                        SoldierAttributeInfo tmpSoldierInfo = ConfigManager.Instance.mSoldierData.FindById(_curGuide.GuideStep.mFightSoldier[0]);
                        if (tmpSoldierInfo == null)
                            return;
                        ShowInfoSoldiers tmpShowInfo = new ShowInfoSoldiers();
                        tmpShowInfo.ReSetFightAttribute(_curGuide.GuideStep.mFightSoldier[0], tmpSoldierInfo.hp_max, (int)tmpSoldierInfo.u_hp, tmpSoldierInfo.phy_atk, CommonFunction.GetSecondTimeByMilliSecond(tmpSoldierInfo.atk_interval),
            tmpSoldierInfo.atk_space, tmpSoldierInfo.acc_rate, tmpSoldierInfo.crt_rate, tmpSoldierInfo.ddg_rate, tmpSoldierInfo.tnc_rate, tmpSoldierInfo.speed);
                        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(tmpShowInfo, 0, SceneManager.Instance.Get_CurScene.Get_OtherIndex,
                ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcSelf, EFightType.eftMain, (int)_curGuide.GuideStep.mFightSoldier[1], null, FightSoldierFinish));

                        break;
                    }
                case GuideStepType.MainCityClickBuilding:
                    {
                        ScaleGuideView(false);
                        break;
                    }
                case GuideStepType.RecruitGuideMode:
                    {
                        if (UISystem.Instance.RecruitView != null)
                            UISystem.Instance.RecruitView.UpdateViewByIsGuide(1);
                        break;
                    }
                case GuideStepType.RecruitCloseLeft:
                    {
                        if (UISystem.Instance.RecruitView != null)
                        {
                            if (_curGuide.GuideStep.mStepType.Contains(GuideStepType.RecruitCloseRight))
                            {
                                UISystem.Instance.RecruitView.OpenGuideScrollLeft(false);
                                UISystem.Instance.RecruitView.OpenGuideScrollRight(false);
                            }
                            else
                            {
                                UISystem.Instance.RecruitView.OpenGuideScrollLeft(false);
                                UISystem.Instance.RecruitView.OpenGuideScrollRight(true);
                                UISystem.Instance.GuideView.SetDragArrow(true);
                            }
                        }
                        break;
                    }
                case GuideStepType.RecruitCloseRight:
                    {
                        if (UISystem.Instance.RecruitView != null)
                        {
                            if (!_curGuide.GuideStep.mStepType.Contains(GuideStepType.RecruitCloseLeft))
                            {
                                UISystem.Instance.RecruitView.OpenGuideScrollRight(false);
                                UISystem.Instance.RecruitView.OpenGuideScrollLeft(true);
                                UISystem.Instance.GuideView.SetDragArrow(false);
                            }
                        }
                        break;
                    }
                case GuideStepType.OpenMenuTool:
                    {
                        if (UISystem.Instance.UIIsOpen(MenuView.UIName))
                        {
                            UISystem.Instance.MenuView.OpenMenu();
                        }
                        break;
                    }
            }
        }
    }

    private void FightSoldierFinish(RoleBase role)
    {
        CheckTrigger(GuideTrigger.ShowFightSoldierFinish);
    }

    public void CloseGuideShade()
    {
        if (UISystem.Instance.UIIsOpen(GuideView.UIName))
        {
            UISystem.Instance.GuideView.CloseAllGuideItem();
        }
    }

    private void CloseLastGuideSound()
    {
        if (!string.IsNullOrEmpty(_guideSound))
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_DeleteSingle_Audio, new ShowAudioInfo(_guideSound));
        _guideSound = string.Empty;
    }

    public void StopGuide()
    {
        _guideIsRunning = false;
        _curGuide.StepIndex = -1;
        if (UISystem.Instance.MainCityView != null)
        {
            UISystem.Instance.MainCityView.EnableMainUIDrag(true);
        }
    }

    // 检查该引导可否开启
    private bool CheckGuideLimit(GuideData guide)
    {
        bool result = true;
        switch (guide.mLimitType)
        {
            case GuideLimitType.CharLevel:
                {
                    result = PlayerData.Instance._Level >= guide.mLimitValue;
                    break;
                }
            case GuideLimitType.NeedPreGuide:
                {
                    result = PlayerData.Instance.DoneNewbieGuideList.Contains(guide.mLimitValue);
                    break;
                }
            case GuideLimitType.PassLevel:
                {
                    result = PlayerData.Instance.IsPassedGate(guide.mLimitValue);
                    break;
                }
            case GuideLimitType.GetTastAward:
                {
                    result = guide.mLimitValue != 0 && guide.mLimitValue == TaskModule.Instance.LastTaskAwardID;
                    break;
                }
            case GuideLimitType.PassLevelAndFirstGodWeaponEquip:
                {
                    result = PlayerData.Instance.IsPassedGate(guide.mLimitValue) && PlayerData.Instance.GetIsEquipByIndex();
                    break;
                }
            case GuideLimitType.PassLevelAndFirstGodWeaponEquipNot:
                {
                    result = PlayerData.Instance.IsPassedGate(guide.mLimitValue) && !PlayerData.Instance.GetIsEquipByIndex();
                    break;
                }
            case GuideLimitType.ChooseLevelWithID:
                {
                    result = !UISystem.Instance.UIIsOpen(GateInfoView.UIName) || UISystem.Instance.GateInfoView.GuideCheckID((int)guide.mLimitValue);
                    break;
                }
            case GuideLimitType.ChooseLevelWithIDEquip21610003:
                {
                    result = !UISystem.Instance.UIIsOpen(GateInfoView.UIName) || UISystem.Instance.GateInfoView.GuideCheckID((int)guide.mLimitValue);
                    result = result && PlayerData.Instance.GetIsEquipById(21610003);
                    break;
                }
            case GuideLimitType.ChooseLevelWithIDNoEquip21610003:
                {
                    result = !UISystem.Instance.UIIsOpen(GateInfoView.UIName) || UISystem.Instance.GateInfoView.GuideCheckID((int)guide.mLimitValue);
                    result = result && !PlayerData.Instance.GetIsEquipById(21610003);
                    break;
                }
            case GuideLimitType.EnterLevelWithID:
                {
                    result = (SceneManager.Instance.Get_CurSceneType == EFightType.eftMain || SceneManager.Instance.Get_CurSceneType == EFightType.eftActivity)
                        && SceneManager.Instance.Get_FightID == guide.mLimitValue;
                    break;
                }
        }

        GuideDebug("CheckGuideLimit =" + result);
        return result;
    }

    private void DialogDone()
    {
        CheckTrigger(GuideTrigger.DialogueClose);
    }

    public bool GuideIsRunning()
    {
        return _guideIsRunning;
    }

    public void GuideOfflineClearData()
    {
        /*
        if (_curGuide.GuideData.mGuideType == 102 && PlayerPrefsTool.HasKey(AppPrefEnum.GateReadySoldier))
        {
            PlayerPrefsTool.DeleteLocalData(AppPrefEnum.GateReadySoldier.ToString());
        }
        if (_curGuide.GuideData.mGuideType == 104 && PlayerPrefsTool.HasKey(AppPrefEnum.GateReadySoldier))
        {
            List<ulong> list = PlayerPrefsTool.ReadString<List<ulong>>(AppPrefEnum.GateReadySoldier);

            if (list.Count == 2)
            {
                list.RemoveAt(0);
            }
            PlayerPrefsTool.WriteString<List<ulong>>(AppPrefEnum.GateReadySoldier, list);
        }
        if (_curGuide.GuideData.mGuideType == 105 && PlayerPrefsTool.HasKey(AppPrefEnum.GateReadySoldier))
        {
            List<ulong> list = PlayerPrefsTool.ReadString<List<ulong>>(AppPrefEnum.GateReadySoldier);

            if (list.Count == 3)
            {
                list.RemoveAt(2);
            }
            PlayerPrefsTool.WriteString<List<ulong>>(AppPrefEnum.GateReadySoldier, list);
        }*/
    }

    public bool IsGuideFinish(uint guideType)
    {
        return PlayerData.Instance.DoneNewbieGuideList.Contains(guideType);
    }

    public bool IsSpecialFightFinish()
    {
        if (!GlobalConst.IS_OPEN_GUIDE)
            return true;

        uint specialFightType = 101;
        return IsGuideFinish(specialFightType);
    }

    private void PlaySpecialFightMovie()
    {

    }

    private void EnterSpecialFight()
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        UISystem.Instance.HintView.ShowFightLoading(UISystem.Instance.FightView.ShowFightTitleInfo);
        Assets.Script.Common.Scheduler.Instance.AddFrame(2, false, EnterSpecialFightDelay);
    }

    private void EnterSpecialFightDelay()
    {
        StageInfo stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(GlobalConst.FIRST_STAGE_ID);
        UISystem.Instance.FightView.SetViewInfo(EFightType.eftNewGuide, stageInfo, PlayerData.Instance._SoldierDepot._soldierList);
    }

    private void ScaleGuideView(bool normalOne)
    {
        if (_scaleGuide == null)
        {
            _scaleGuide = UISystem.Instance.GuideView._ui._uiRoot.GetComponent<ScaleMainCity>();
            _scaleGuide.scaleMainCityType = ScaleMainCity.ScaleMainCityType.Guide;
        }
        _scaleGuide.SetGuideViewScale(normalOne);

    }

    private bool AllGuideItemIsClose()
    {
        if (UISystem.Instance.UIIsOpen(GuideView.UIName))
        {
            return UISystem.Instance.GuideView.AllGuideItemIsClose();
        }
        return false;
    }

    private void GuideDebug(string str)
    {
        if (!_showDebug)
            return;
        Debug.LogWarning(str);
    }
}
