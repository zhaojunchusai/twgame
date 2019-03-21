using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;

public class PlayerInfoViewController : UIBase
{
    public PlayerInfoView view;
    private PlayerInfoComponent playerInfoComp;
    private List<LineupItemComponent> defenseSoldier_Dic;
    private ArenaPlayer playerInfo;
    private ulong worldHelpCD;
    private ulong corpsHelpCD;
    private PlayerInfoTypeEnum playerInfoType;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new PlayerInfoView();
            view.Initialize();
            BtnEventBinding();
        }
        // PlayOpenAnim();
        InitView();
        if (playerInfoComp == null)
            playerInfoComp = new PlayerInfoComponent(view.Gobj_PlayerInfoComp);
        if (defenseSoldier_Dic == null)
            defenseSoldier_Dic = new List<LineupItemComponent>();
        PlayerData.Instance._Prison.PrisonControlEvent += OnReceiveHelp;
    }

    private void InitView()
    {
        view.Gobj_EnemyInfoComp.SetActive(false);
        view.Spt_DynamicBG.height = 390;
        view.Spt_CenterDecorative.enabled = true;
        view.Gobj_EnemyGroup.transform.localPosition = new Vector3(0, 60, 0);
        view.Gobj_CorpsGroup.SetActive(false);
        view.Gobj_SlaveGroup.SetActive(false);
        view.Gobj_SlaveHolder.SetActive(false);
        view.Gobj_AttackGroup.SetActive(false);
    }

    #region Button Event
    public void ButtonEvent_CloseButton(GameObject btn)
    {
        //if (playerInfoType == PlayerInfoTypeEnum.QualifyingAttack)
        //{
        //    return;
        //}
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));

        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
    }

    public void ButtonEvent_CorpsHelp(GameObject btn)
    {
        if ((ulong)Main.mTime < corpsHelpCD)
        {
            return;
        }
        bool result = ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Sociaty, (uint)playerInfo.hero.vip_lv, (uint)playerInfo.hero.level, false);
        if (result)
        {
            PlayerData.Instance._Prison.StartUnionRescue(playerInfo.hero.charname, (uint)playerInfo.hero.area_id);
        }
    }

    public void ButtonEvent_WorldHelp(GameObject btn)
    {
        if ((ulong)Main.mTime < worldHelpCD)
        {
            return;
        }
        PlayerData.Instance._Prison.StartWorldRescue(playerInfo.hero.charname, (uint)playerInfo.hero.area_id);
    }
    public void ButtonEvent_Revolt(GameObject btn)
    {
        PlayerData.Instance._Prison.SendEnslaveFightBeforeReq();
    }
    public void ButtonEvent_Visit(GameObject btn)
    {
    }
    public void ButtonEvent_Slave(GameObject btn)
    {
    }
    public void ButtonEvent_Attack(GameObject btn)
    {
        //OnReadySlaveBattle();
        PlayerData.Instance._Prison.SendEnslaveFightBeforeReq();
    }

    public void ButtonEvent_QualifyingChallenge(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
        PrepareCommonData data = new PrepareCommonData();
        data.FightType = EFightType.eftQualifying;
        data.IsAdjust = false;
        data.ArenaPlayer = playerInfo;
        data.Other = 1;
        data.AccountID = playerInfo.hero.accid;
        UISystem.Instance.PrepareBattleView.UpdateViewInfo(data);
    }
    #endregion

    #region Update Event

    private void UpdateWorldHelpCD()
    {
        if ((ulong)Main.mTime >= worldHelpCD)
        {
            CommonFunction.SetLabelColor_I(view.Lbl_WorldHelpTip, 115, 71, 37, 255, 255, 255, 255, 255);
            CommonFunction.UpdateWidgetGray(view.Spt_WorldHelpBg, false);
            view.Lbl_WorldHelpTip.text = ConstString.PLAYERINFO_BUTTON_WORLDHELP;
            Scheduler.Instance.RemoveUpdator(UpdateWorldHelpCD);
        }
        else
        {
            ulong cd = worldHelpCD - (ulong)Main.mTime;
            CommonFunction.SetLabelColor_I(view.Lbl_WorldHelpTip, 104, 92, 77, 255, 255, 255, 255, 255);
            view.Lbl_WorldHelpTip.text = string.Format(ConstString.FORMAT_TIME_NOHOURS, (cd % 3600) / 60, (cd % 3600) % 60);
            CommonFunction.UpdateWidgetGray(view.Spt_WorldHelpBg, true);
        }
    }

    private void UpdateCorpsHelpCD()
    {
        if ((ulong)Main.mTime >= corpsHelpCD)
        {
            CommonFunction.SetLabelColor_I(view.Lbl_CorpsHelpTip, 115, 71, 37, 255, 255, 255, 255, 255);
            //view.Lbl_CorpsHelpTip.color = new Color(115, 71, 37);
            CommonFunction.UpdateWidgetGray(view.Spt_CorpsHelpBg, false);
            view.Lbl_CorpsHelpTip.text = ConstString.PLAYERINFO_BUTTON_CORPSHELP;
            Scheduler.Instance.RemoveUpdator(UpdateCorpsHelpCD);
        }
        else
        {
            CommonFunction.SetLabelColor_I(view.Lbl_CorpsHelpTip, 104, 92, 77, 255, 255, 255, 255, 255);
            ulong cd = corpsHelpCD - (ulong)Main.mTime;
            //view.Lbl_CorpsHelpTip.color = new Color(104, 92, 77);
            view.Lbl_CorpsHelpTip.text = string.Format(ConstString.FORMAT_TIME_NOHOURS, (cd % 3600) / 60, (cd % 3600) % 60);
            CommonFunction.UpdateWidgetGray(view.Spt_CorpsHelpBg, true);
        }
    }

    /// <summary>
    /// 玩家信息类型    
    /// isSlave(true: 被奴役，false：自由人）
    /// corpsOffice(军团职位： 1-成员 2-副团长 3-军团长)
    /// </summary>
    public void UpdateViewInfo(PlayerInfoTypeEnum infoType, ArenaPlayer info, bool isSlave, UnionPosition corpsOffice)
    {
        playerInfoType = infoType;
        switch (infoType)
        {
            case PlayerInfoTypeEnum.MostLineup:
            case PlayerInfoTypeEnum.EndlessPlayerInfo:
            case PlayerInfoTypeEnum.VIP:
            case PlayerInfoTypeEnum.Arena:
                {
                    view.Spt_DynamicBG.height = 390;
                    view.Spt_CenterDecorative.enabled = true;
                    view.Gobj_EnemyGroup.transform.localPosition = Vector3.zero;
                    view.Gobj_AttackGroup.SetActive(false);
                    view.Gobj_CorpsGroup.SetActive(false);
                    view.Gobj_SlaveGroup.SetActive(false);
                    view.Gobj_SlaveHolder.SetActive(false);
                    view.Gobj_QualifyingGroup.SetActive(false);
                    view.Gobj_Rank.SetActive(true);
                } break;
            case PlayerInfoTypeEnum.Integral:
            case PlayerInfoTypeEnum.Corps:
                {
                    view.Spt_DynamicBG.height = 390;
                    view.Spt_CenterDecorative.enabled = true;
                    view.Gobj_EnemyGroup.transform.localPosition = Vector3.zero;
                    //view.Gobj_EnemyGroup.transform.localPosition = new Vector3(0, 60, 0);
                    view.Gobj_SlaveGroup.SetActive(false);
                    view.Gobj_SlaveHolder.SetActive(false);
                    view.Gobj_AttackGroup.SetActive(false);
                    view.Gobj_QualifyingGroup.SetActive(false);
                    view.Gobj_Rank.SetActive(true);
                    view.Gobj_CorpsGroup.SetActive(false);
                } break;
            case PlayerInfoTypeEnum.SlaveAttack:
                {
                    view.Spt_DynamicBG.height = 315;
                    view.Spt_CenterDecorative.enabled = false;
                    view.Gobj_EnemyGroup.transform.localPosition = new Vector3(0, 60, 0);
                    view.Gobj_SlaveGroup.SetActive(false);
                    view.Gobj_AttackGroup.SetActive(true);
                    view.Gobj_CorpsGroup.SetActive(false);
                    view.Gobj_QualifyingGroup.SetActive(false);
                    view.Gobj_Rank.SetActive(!isSlave);
                    view.Gobj_SlaveHolder.SetActive(isSlave);
                }
                break;
            case PlayerInfoTypeEnum.Revolt:
                {
                    view.Spt_DynamicBG.height = 315;
                    view.Spt_CenterDecorative.enabled = false;
                    view.Gobj_EnemyGroup.transform.localPosition = new Vector3(0, 60, 0);
                    view.Gobj_SlaveGroup.SetActive(true);
                    view.Gobj_AttackGroup.SetActive(false);
                    view.Gobj_CorpsGroup.SetActive(false);
                    view.Gobj_QualifyingGroup.SetActive(false);
                    view.Gobj_Rank.SetActive(!isSlave);
                    view.Gobj_SlaveHolder.SetActive(isSlave);
                } break;
        }
        UpdateInfo(infoType, info, isSlave, corpsOffice);
        if (view.Gobj_SlaveGroup.activeSelf)
        {
            ulong world_last_time = PlayerData.Instance._Prison.GetPrisonInfo().last_time;
            ulong worldCD = 60;
            if (!ulong.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_WORLD_WORLDHELPCD), out worldCD))
            {
                worldCD = 60;
            }
            if ((world_last_time - (ulong)Main.mTime) > worldCD)
            {
                worldHelpCD = (ulong)Main.mTime + worldCD;
            }
            else
            {
                worldHelpCD = world_last_time;
            }
            ulong corps_last_time = PlayerData.Instance._Prison.GetPrisonInfo().union_last_time;
            ulong corpsCD = 60;
            if (!ulong.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_HELP_CORPSHELPCD), out corpsCD))
            {
                corpsCD = 60;
            }
            if ((corps_last_time - (ulong)Main.mTime) > corpsCD)
            {
                corpsHelpCD = (ulong)Main.mTime + corpsCD;
            }
            else
            {
                corpsHelpCD = corps_last_time;
            }
            Scheduler.Instance.AddUpdator(UpdateWorldHelpCD);
            Scheduler.Instance.AddUpdator(UpdateCorpsHelpCD);
        }
    }

    /// <summary>
    /// 玩家信息类型    
    /// isSlaveHolder(true: 奴隶主，false：自由人）
    /// </summary>
    public void UpdateViewInfo(PlayerInfoTypeEnum infoType, ArenaPlayer info, bool isSlaveHolder)
    {
        UpdateViewInfo(infoType, info, isSlaveHolder, UnionPosition.UP_MEMBER);
    }

    public void UpdateViewInfo(PlayerInfoTypeEnum infoType, ArenaPlayer info)
    {
        UpdateViewInfo(infoType, info, false, UnionPosition.UP_MEMBER);
    }

    public void UpdateQualifyingViewInfo(PlayerInfoTypeEnum infoType, ArenaPlayer info)
    {
        playerInfoType = infoType;
        playerInfo = info;
        if (info == null) return;
        switch (infoType)
        {
            case PlayerInfoTypeEnum.QualifyingLog:
            case PlayerInfoTypeEnum.QualifyingAttack:
                {

                    view.Gobj_SlaveGroup.SetActive(false);
                    view.Gobj_AttackGroup.SetActive(false);
                    view.Gobj_CorpsGroup.SetActive(false);
                    view.Gobj_QualifyingGroup.SetActive(true);
                    view.Gobj_Rank.SetActive(true);
                    view.Gobj_SlaveHolder.SetActive(false);

                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ENEMYVIEW;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_DEFENSE;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_INTEGRAL;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_DIVISION;
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_QUALIFYINGLINEUP;
                    QualifyingRankData rankData = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByPoint(info.score);
                    if (rankData == null)
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = rankData.divisionName;
                    }
                    view.Lbl_NameLabel.text = info.hero.charname;
                    view.Lbl_DefenseLabel.text = info.combat_power.ToString();
                    view.Lbl_RankLabel.text = info.score.ToString();
                    //=============更新头像================//
                    string frame = string.Empty;
                    QualifyingRankData data = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByPoint(playerInfo.score);
                    if (data != null)
                        frame = data.frame;
                    playerInfoComp.UpdateQualifyingInfo(info.hero.icon, frame, info.hero.level);

                    view.Gobj_EnemyGroup.SetActive(true);
                    UpdatePVPDefenseCast(playerInfo);
                    UpdateLineupInfo();
                } break;
        }
    }

    private void UpdateLineupInfo()
    {
        switch (playerInfoType)
        {
            case PlayerInfoTypeEnum.QualifyingAttack:
                {
                    view.Spt_DynamicBG.height = 315;
                    view.Spt_CenterDecorative.enabled = false;
                    view.Gobj_EnemyGroup.transform.localPosition = new Vector3(0, 60, 0);
                    view.Gobj_QualifyingGroup.SetActive(true);
                    //view.Lbl_QualifyingCD.text = string.Format(ConstString.PLAYERINOF_QUALIFYING_ENTERBATLE, qualifyingCD);
                    //Scheduler.Instance.AddTimer(1, true, UpdateQualifyCD);
                } break;
            case PlayerInfoTypeEnum.QualifyingLog:
                {
                    view.Spt_DynamicBG.height = 390;
                    view.Spt_CenterDecorative.enabled = true;
                    view.Gobj_EnemyGroup.transform.localPosition = Vector3.zero;
                    view.Gobj_QualifyingGroup.SetActive(false);
                    //view.Lbl_QualifyingCD.text = string.Empty;
                } break;
        }
    }

    private void UpdateInfo(PlayerInfoTypeEnum infoType, ArenaPlayer info, bool isSlave = false, UnionPosition corpsOffice = UnionPosition.UP_MEMBER)
    {
        playerInfo = info;
        if (info == null) return;
        switch (infoType)
        {
            case PlayerInfoTypeEnum.EndlessPlayerInfo:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ARENAVIEW;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_RANK;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_ENDLESS;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_CORPS;
                    view.Gobj_SlaveHolder.SetActive(false);
                    if (CommonFunction.CheckPVPRank(info.rank))
                    {
                        view.Lbl_RankLabel.text = info.rank.ToString();
                    }
                    else
                    {
                        view.Lbl_RankLabel.text = ConstString.HINT_NO;
                    }
                    if (string.IsNullOrEmpty(info.hero.unionname))
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = info.hero.unionname;
                    }
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_ENDLESS;
                } break;
            case PlayerInfoTypeEnum.MostLineup:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ARENAVIEW;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_RANK;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_MOSTLINEUP;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_CORPS;
                    view.Gobj_SlaveHolder.SetActive(false);
                    if (CommonFunction.CheckPVPRank(info.rank))
                    {
                        view.Lbl_RankLabel.text = info.rank.ToString();
                    }
                    else
                    {
                        view.Lbl_RankLabel.text = ConstString.HINT_NO;
                    }
                    if (string.IsNullOrEmpty(info.hero.unionname))
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = info.hero.unionname;
                    }
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_MOSTLINEUP;
                } break;
            case PlayerInfoTypeEnum.Arena:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ARENAVIEW;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_RANK;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_DEFENSE;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_CORPS;
                    view.Gobj_SlaveHolder.SetActive(false);
                    if (CommonFunction.CheckPVPRank(info.rank))
                    {
                        view.Lbl_RankLabel.text = info.rank.ToString();
                    }
                    else
                    {
                        view.Lbl_RankLabel.text = ConstString.HINT_NO;
                    }
                    if (string.IsNullOrEmpty(info.hero.unionname))
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = info.hero.unionname;
                    }
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_ARENA;
                } break;
            case PlayerInfoTypeEnum.Corps:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_CORPSVIEW;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_DEFENSE;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_OFFICE;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_SLAVESTATUS;
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_ARENA;
                    if (isSlave)
                    {
                        view.Lbl_RankLabel.text = ConstString.PLAYERINFO_SLAVESTATUS_SLAVE;
                    }
                    else
                    {
                        view.Lbl_RankLabel.text = ConstString.PLAYERINFO_SLAVESTATUS_FREE;
                    }
                    switch (corpsOffice)
                    {
                        case UnionPosition.UP_MEMBER:
                            view.Lbl_CorpsLabel.text = ConstString.PLAYERINFO_CORPSOFFICE_MEMBER;
                            break;
                        case UnionPosition.UP_VICE_CHAIRMAN:
                            view.Lbl_CorpsLabel.text = ConstString.PLAYERINFO_CORPSOFFICE_DEPUTYHEAD;
                            break;
                        case UnionPosition.UP_CHAIRMAN:
                            view.Lbl_CorpsLabel.text = ConstString.PLAYERINFO_CORPSOFFICE_HEADS;
                            break;
                        default:
                            view.Lbl_CorpsLabel.text = string.Empty;
                            break;
                    }
                } break;
            case PlayerInfoTypeEnum.Integral:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ARENAVIEW;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_RANK;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_MOSTLINEUP;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_INTEGRAL;
                    view.Lbl_CorpsLabel.text = info.hero.vip_lv.ToString();
                    if (CommonFunction.CheckPVPRank(info.rank))
                    {
                        view.Lbl_RankLabel.text = info.rank.ToString();
                    }
                    else
                    {
                        view.Lbl_RankLabel.text = ConstString.HINT_NO;
                    }
                    view.Gobj_SlaveHolder.SetActive(false);
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_MOSTLINEUP;
                } break;
            case PlayerInfoTypeEnum.SlaveAttack:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ENEMYVIEW;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_DEFENSE;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_CORPS;
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_ARENA;
                    if (string.IsNullOrEmpty(info.hero.unionname))
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = info.hero.unionname;
                    }
                    if (isSlave)
                    {
                        view.Lbl_SlaverHolderTitle.text = ConstString.PLAYERINFO_TITLE_SLAVESTATUS;
                        view.Lbl_SlaverHolder.text = ConstString.PLAYERINFO_SLAVESTATUS_SLAVESHOLDER;
                        view.Lbl_BtnAttack.text = ConstString.PLAYERINFO_BUTTON_ATTACK;
                    }
                    else
                    {
                        view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_SLAVESTATUS;
                        view.Lbl_RankLabel.text = ConstString.PLAYERINFO_SLAVESTATUS_FREE;
                        view.Lbl_BtnAttack.text = ConstString.PLAYERINFO_BUTTON_ENSLAVE;
                    }
                    view.Lbl_SPNum.text = GlobalCoefficient.SlaveSPConsume.ToString();
                }
                break;
            case PlayerInfoTypeEnum.Revolt:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ENEMYVIEW;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_DEFENSE;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_CORPS;
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_ARENA;
                    if (isSlave)
                    {
                        view.Lbl_SlaverHolderTitle.text = ConstString.PLAYERINFO_TITLE_SLAVESTATUS;
                        view.Lbl_SlaverHolder.text = ConstString.PLAYERINFO_SLAVESTATUS_SLAVESHOLDER;
                    }
                    else
                    {
                        view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_SLAVESTATUS;
                        view.Lbl_RankLabel.text = ConstString.PLAYERINFO_SLAVESTATUS_FREE;
                    }
                    if (string.IsNullOrEmpty(info.hero.unionname))
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = info.hero.unionname;
                    }
                } break;
            case PlayerInfoTypeEnum.VIP:
                {
                    view.Lbl_ViewTitle.text = ConstString.PLAYERINFO_TITLE_ARENAVIEW;
                    view.Lbl_RankTitle.text = ConstString.PLAYERINFO_TITLE_RANK;
                    view.Lbl_NameTitle.text = ConstString.PLAYERINFO_TITLE_NAME;
                    view.Lbl_DefenseTitle.text = ConstString.PLAYERINFO_TITLE_MOSTLINEUP;
                    view.Lbl_CorpsTitle.text = ConstString.PLAYERINFO_TITLE_CORPS;
                    view.Gobj_SlaveHolder.SetActive(false);
                    view.Lbl_RankTitle.text = "VIP:";
                    view.Lbl_RankLabel.text = info.hero.vip_lv.ToString();
                    if (string.IsNullOrEmpty(info.hero.unionname))
                    {
                        view.Lbl_CorpsLabel.text = ConstString.HINT_NO;
                    }
                    else
                    {
                        view.Lbl_CorpsLabel.text = info.hero.unionname;
                    }
                    view.Lbl_LineupTilte.text = ConstString.PLAYERINFO_LINEUPTITLE_MOSTLINEUP;
                } break;
        }
        view.Lbl_NameLabel.text = info.hero.charname;
        view.Lbl_DefenseLabel.text = info.combat_power.ToString();
        playerInfoComp.UpdateInfo(info.hero.icon, info.hero.icon_frame, info.hero.level);
        view.Gobj_EnemyGroup.SetActive(true);
        UpdatePVPDefenseCast(info);
    }
    private void UpdatePVPDefenseCast(ArenaPlayer playerInfo)
    {
        List<ArenaSoldier> list = new List<ArenaSoldier>(playerInfo.soldiers);
        if (list.Count <= defenseSoldier_Dic.Count)
        {
            for (int i = 0; i < defenseSoldier_Dic.Count; i++)
            {
                LineupItemComponent comp = defenseSoldier_Dic[i];
                if (i < list.Count)
                {
                    ArenaSoldier _arenaSoldier = list[i];
                    if (_arenaSoldier == null || _arenaSoldier.soldier == null)
                        continue;
                    Soldier _soldier = Soldier.createByID(_arenaSoldier.soldier.id);
                    if (_soldier == null) continue;
                    _soldier.Serialize(_arenaSoldier.soldier);
                    comp.UpdateInfo(_soldier);
                    comp.UpdateNum(_arenaSoldier.num);
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int goCount = defenseSoldier_Dic.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ArenaSoldier _arenaSoldier = list[i];
                if (_arenaSoldier == null) continue;
                LineupItemComponent comp = null;
                if (i < goCount)
                {
                    comp = defenseSoldier_Dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyInfoComp, view.Grd_EnemyGrid.transform);
                    go.name = "Soldier_" + i.ToString();
                    comp = new LineupItemComponent();
                    comp.MyStart(go);
                    defenseSoldier_Dic.Add(comp);
                }
                if (comp == null) continue;
                if (_arenaSoldier == null || _arenaSoldier.soldier == null)
                    continue;
                Soldier _soldier = Soldier.createByID(_arenaSoldier.soldier.id);
                if (_soldier == null) continue;
                _soldier.Serialize(_arenaSoldier.soldier);
                comp.UpdateInfo(_soldier);
                comp.UpdateNum(_arenaSoldier.num);
                comp.IsEnable = true;
                comp.IsMask = false;
                comp.IsSelect = false;
                comp.IsShowEnergy = true;
                comp.IsShowLeader = false;
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_EnemyGrid.Reposition();
    }

    //private void UpdateQualifyCD()
    //{
    //    if (qualifyingCD <= 0)
    //    {
    //        Scheduler.Instance.RemoveTimer(UpdateQualifyCD);
    //        EnterQualifyBattle();
    //    }
    //    else
    //    {
    //        qualifyingCD--;
    //        view.Lbl_QualifyingCD.text = string.Format(ConstString.PLAYERINOF_QUALIFYING_ENTERBATLE, qualifyingCD);
    //    }
    //}
    #endregion
    private void OnReceiveHelp(PrisonType type, int errorcode)
    {
        if (errorcode == 0)
        {
            worldHelpCD = PlayerData.Instance._Prison.GetPrisonInfo().last_time;
            corpsHelpCD = PlayerData.Instance._Prison.GetPrisonInfo().union_last_time;
            switch (type)
            {
                case PrisonType.ReceiveAskUnionHelpResp:
                    {
                        Scheduler.Instance.AddUpdator(UpdateCorpsHelpCD);
                    } break;
                case PrisonType.ReceiveAskWorldHelpResp:
                    {
                        Scheduler.Instance.AddUpdator(UpdateWorldHelpCD);
                    } break;
            }
        }
    }

    public void OnReadySlaveBattle()
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
        UISystem.Instance.PrepareBattleView.UpdateViewInfo(EFightType.eftSlave);
    }

    public override void Uninitialize()
    {
        worldHelpCD = 0;
        corpsHelpCD = 0;
        PlayerData.Instance._Prison.PrisonControlEvent -= OnReceiveHelp;
        Scheduler.Instance.RemoveUpdator(UpdateCorpsHelpCD);
        Scheduler.Instance.RemoveUpdator(UpdateWorldHelpCD);
        //Scheduler.Instance.RemoveTimer(UpdateQualifyCD);
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        defenseSoldier_Dic.Clear();
        playerInfoComp = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskSprite.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_CorpsHelp.gameObject).onClick = ButtonEvent_CorpsHelp;
        UIEventListener.Get(view.Btn_WorldHelp.gameObject).onClick = ButtonEvent_WorldHelp;
        UIEventListener.Get(view.Btn_Revolt.gameObject).onClick = ButtonEvent_Revolt;
        UIEventListener.Get(view.Btn_Visit.gameObject).onClick = ButtonEvent_Visit;
        UIEventListener.Get(view.Btn_Slave.gameObject).onClick = ButtonEvent_Slave;
        UIEventListener.Get(view.Btn_Attack.gameObject).onClick = ButtonEvent_Attack;
        UIEventListener.Get(view.Btn_QualifyingChallenge.gameObject).onClick = ButtonEvent_QualifyingChallenge;
    }

    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}
}
