using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

public class CrossServerWarViewController : UIBase 
{
    #region 变量
    public CrossServerWarView view;

    /// <summary>
    /// 地图缩小比例
    /// </summary>
    private const float scale = 0.565f;
    private const float scale_single = 1.45f;

    //奖励信息界面下，战役/赛季奖励按钮切换状态
    private Color state_down_color = new Color(182/255f, 101/255f, 24/255f);
    private Color state_up_color = new Color(115/255f, 71/255f, 37/255f);
    private const string state_down_sprite = "XTSZ_Bth_Off";
    private const string state_up_sprite = "ZCJ_bg_jiaanniu";
    /// <summary>
    /// 地块列表
    /// </summary>
    public Dictionary<uint, TileInfo> tileDic;
    /// <summary>
    /// 地块特殊产出列表
    /// </summary>
    private List<CrossServerAwardItem> specialProductionList;
    /// <summary>
    /// 战役倒计时
    /// </summary>
    private long battleCountDown;
    /// <summary>
    /// 修整倒计时
    /// </summary>
    private long restCountDown;
    /// <summary>
    /// 地块排行榜列表
    /// </summary>
    private List<TilePointRankItem> tileRankList;
    //地块排行榜切换时选中遮罩的位置
    private Vector3 unionBtnPos = new Vector3(-249, -200, 0);
    private Vector3 personalBtnPos = new Vector3(-84, -200, 0);
    /// 特殊产出列表
    List<SpecialProductionItem> spItemList;
    List<SpecialProductionDesc> spDescList;
    /// <summary>
    /// 购买CD免提示
    /// </summary>
    public bool autoBuyCD;
    /// <summary>
    /// 当前选中的地块ID
    /// </summary>
    public uint cur_tileID;
    /// <summary>
    /// 地块守将信息
    /// </summary>
    private List<GateEnemyInfoComponent> enemy_dic;
    /// <summary>
    /// 地块关卡信息
    /// </summary>
    private StageInfo stageInfo;
    #endregion

    #region 初始化函数
    public override void Initialize()
    {
        if (view == null)
            view = new CrossServerWarView();
        view.Initialize();
        BtnEventBinding();

        InitData();
        InitView();
    }

    /// <summary>
    /// 界面显示初始化
    /// </summary>
    public void InitView()
    {
        SpecialSetting(CrossServerWarModule.Instance.isCrossServer);
        //战场沙盘默认开始显示一屏
        view.Trans_BattleField.localPosition = Vector3.zero;
        view.Scro_BattleField.enabled = false;
        CommonFunction.SetBtnState(view.Btn_DetailviewButton.gameObject, true);
        CommonFunction.SetBtnState(view.Btn_OverviewButton.gameObject, false);
        //打开聊天窗口，默认军团信息
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.Corps);
        //生成主界面地块
        Main.Instance.StartCoroutine(CreatAllTile());
        UpdateAllTileInfo();
        //添加主界面计时
        UpdateTimeShow();
        Scheduler.Instance.AddTimer(1, true, UpdateTimeShow);

        //若战役未开始，默认显示信息界面
        if (!CrossServerWarModule.Instance.isBattleStart)
        {
            GetServerToShowInfo();
        }
    }

    /// <summary>
    /// 数据信息初始化
    /// </summary>
    public void InitData()
    {
        if(tileDic==null)
            tileDic = new Dictionary<uint, TileInfo>();
        if(specialProductionList==null)
            specialProductionList = new List<CrossServerAwardItem>();
        if(tileRankList==null)
            tileRankList = new List<TilePointRankItem>();
        if (spItemList == null)
            spItemList = new List<SpecialProductionItem>();
        if (spDescList == null)
            spDescList = new List<SpecialProductionDesc>();
        if (enemy_dic == null)
            enemy_dic = new List<GateEnemyInfoComponent>();
        //读取配置表规则
        view.Lbl_RuleDesc.text = CommonFunction.ReplaceEscapeChar(ConfigManager.Instance.mCrossServerWarConfig.GetCrossServerWarSettingData().rule);
    }
    #endregion

    #region 按键响应函数
    /// <summary>
    /// 退出跨服战场
    /// </summary>
    public void ButtonEvent_QuitButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CROSSSERVERWAR);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.World);

    }
    /// <summary>
    /// 弹出排行榜
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_RankListButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RANKVIEW);
        UISystem.Instance.RankView.ShowRank(RankType.CROSSSERVERWAR_UNION);
    }
    /// <summary>
    /// 打开规则界面
    /// </summary>
    public void ButtonEvent_RuleButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.Gobj_Rule.SetActive(true);
        view.ScrView_RuleDesc.ResetPosition();
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
    }
    /// <summary>
    /// 查询地块信息
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_InputSearchTileSearchButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        SearchTileID(view.Lbl_InputSearchTileSearchID.text);
    }
    /// <summary>
    /// 总览地图
    /// </summary>
    public void ButtonEvent_OverviewButton(GameObject btn)
    {
        if (view.Scro_BattleField.enabled)
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
            view.Scro_BattleField.enabled = false;
            CrossServerWarModule.Instance.recordPos = view.Trans_BattleField.localPosition;
            if(CrossServerWarModule.Instance.isCrossServer)
                view.Trans_ShowField.localScale = Vector3.one * scale;
            else
            {
                view.Trans_ShowField.localScale = Vector3.one;
                SetWholeTileName(false);
            }
            view.Trans_BattleField.localPosition = Vector3.zero;
            CommonFunction.SetBtnState(view.Btn_DetailviewButton.gameObject, true);
            CommonFunction.SetBtnState(view.Btn_OverviewButton.gameObject, false);
        }
    }
    /// <summary>
    /// 展示地图细节
    /// </summary>
    public void ButtonEvent_DetailviewButton(GameObject btn)
    {
        if (!view.Scro_BattleField.enabled)
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
            view.Scro_BattleField.enabled = true;
            if(CrossServerWarModule.Instance.isCrossServer)
                view.Trans_ShowField.localScale = Vector3.one;
            else
            {
                view.Trans_ShowField.localScale = Vector3.one * scale_single;
                SetWholeTileName(true);
            }
            view.Trans_BattleField.localPosition = CrossServerWarModule.Instance.recordPos;
            CommonFunction.SetBtnState(view.Btn_DetailviewButton.gameObject, false);
            CommonFunction.SetBtnState(view.Btn_OverviewButton.gameObject, true);
            view.Btn_OverviewButton.defaultColor = Color.white;
        }
    }
    /// <summary>
    /// 打开服务器（奖励）信息界面
    /// </summary>
    public void ButtonEvent_InformationButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        GetServerToShowInfo();
    }
    /// <summary>
    /// 打开军团信息界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_UnionButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.Gobj_Union.SetActive(true);
        ShowUnionInfo();
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
    }
    /// <summary>
    /// 关闭规则界面
    /// </summary>
    public void ButtonEvent_CloseRuleButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_Rule.SetActive(false);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.Corps);
    }
    /// <summary>
    /// 选中地块军团排行
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_UnionRank(GameObject btn)
    {
        if (view.Spt_Selected.transform.localPosition == unionBtnPos)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ShowTileRank(CrossServerWarModule.Instance.tileUnionRank, true);
    }
    /// <summary>
    /// 选中地块个人排行
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_PersonRank(GameObject btn)
    {
        if (view.Spt_Selected.transform.localPosition == personalBtnPos)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ShowTileRank(CrossServerWarModule.Instance.tilePersonalRank, false);
    }
    /// <summary>
    /// 发起战斗按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Fight(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (!UnionModule.Instance.HasUnion)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONVIEW);
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CROSSSERVERWAR);
            return;
        }
        if (tileDic[cur_tileID].owner_type != 0 && tileDic[cur_tileID].border_type == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RANK_CROSSSERVERWAR_SAFETILE);
            return;
        }
        int cost = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData(CrossServerWarModule.Instance.buyCDTimes + 1).CrossServerConsume.Number;
        if (restCountDown > 0)
        {
            if (autoBuyCD && CrossServerWarModule.Instance.buyCDTimes != 0 && cost == ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData(CrossServerWarModule.Instance.buyCDTimes).CrossServerConsume.Number)
                CrossServerWarModule.Instance.OnSendBuyCDClear();
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark, string.Format(ConstString.CROSSSERVER_FIGHT, cost,
                    CommonFunction.GetMoneyNameByType(ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData(CrossServerWarModule.Instance.buyCDTimes + 1).CrossServerConsume.Type)),
                    CrossServerWarModule.Instance.OnSendBuyCDClear, null, ConstString.CAPTURETERRITORY_FIGHT_BTN, "", AutoBuyCDMarkCallback, false);
            }

        }
        else
        {
            ShowGateInfo();
        }
    }
    /// <summary>
    /// 关闭地块信息界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_CloseTileInfoButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_TileInfo.SetActive(false);
        //ClearItems(view.Grd_TileRankGrid);
        Scheduler.Instance.RemoveTimer(FightCDCountDown);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.Corps);
    }
    /// <summary>
    /// 打开军团特殊奖励信息界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_DetailInfoButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.Gobj_SpecialProduction.SetActive(true);
        ShowSpecialProduction();
    }
    /// <summary>
    /// 关闭军团特殊奖励信息界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_CloseSPOutputButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_SpecialProduction.SetActive(false);
        //ClearItems(view.Table_UnionOutputTable);
    }
    /// <summary>
    /// 关闭服务器（奖励）信息界面
    /// </summary>
    public void ButtonEvent_CloseServerInfoButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        ClearItems(view.UITable_Table);
        ClearItems(view.Grd_ServerListGrid);
        view.Gobj_ServerInfo.SetActive(false);
        Scheduler.Instance.RemoveTimer(TimeCountInServerList);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.Corps);
    }
    /// <summary>
    /// 关闭军团信息界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_CloseUnionInfoButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_Union.SetActive(false);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.Corps);
    }
    /// <summary>
    /// 跨服战场信息中，查看战役奖励按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_BattleAward(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ClearItems(view.UITable_Table);
        Scheduler.Instance.AddFrame(2, false, ReadBattleAwardItems);
    }
    /// <summary>
    /// 跨服战场信息中，查看赛季奖励按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_SeasonAward(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ClearItems(view.UITable_Table);
        Scheduler.Instance.AddFrame(2, false, ReadSeasonAwardItems);
    }
    /// <summary>
    /// 关闭关卡信息界面
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_StageClose(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_StageInfo.gameObject.SetActive(false);
        Scheduler.Instance.RemoveTimer(ShowCountdownInStage);
    }
    /// <summary>
    /// 点击进入战斗准备
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ReadyBattle(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (CrossServerWarModule.Instance.isBattleStart)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
            UISystem.Instance.PrepareBattleView.UpdateViewInfo(EFightType.eftCrossServerWar, stageInfo);
            CrossServerWarModule.Instance.recordPos = view.Trans_BattleField.localPosition;
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ACTIVITIES_OPENTIMENULL);
        }
    }
    /// <summary>
    /// 点击显示守将信息
    /// </summary>
    private void ButtonEvent_EnemyComp(GateEnemyInfoComponent baseComp, bool isPress)
    {
        if (baseComp == null) return;
            HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.MonsterInfo);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_QuitButton.gameObject).onClick = ButtonEvent_OverviewButton;
        UIEventListener.Get(view.Btn_QuitButton.gameObject).onClick += ButtonEvent_QuitButton;
        UIEventListener.Get(view.Btn_RankListButton.gameObject).onClick = ButtonEvent_RankListButton;
        UIEventListener.Get(view.Btn_RuleButton.gameObject).onClick = ButtonEvent_RuleButton;
        UIEventListener.Get(view.Btn_InputSearchTileSearchButton.gameObject).onClick = ButtonEvent_InputSearchTileSearchButton;
        UIEventListener.Get(view.Btn_OverviewButton.gameObject).onClick = ButtonEvent_OverviewButton;
        UIEventListener.Get(view.Btn_DetailviewButton.gameObject).onClick = ButtonEvent_DetailviewButton;
        UIEventListener.Get(view.Btn_InformationButton.gameObject).onClick = ButtonEvent_InformationButton;
        UIEventListener.Get(view.Btn_UnionButton.gameObject).onClick = ButtonEvent_UnionButton;
        UIEventListener.Get(view.Btn_CloseRuleButton.gameObject).onClick = ButtonEvent_CloseRuleButton;
        UIEventListener.Get(view.Spt_RuleMask.gameObject).onClick = ButtonEvent_CloseRuleButton;
        UIEventListener.Get(view.Btn_UnionRank.gameObject).onClick = ButtonEvent_UnionRank;
        UIEventListener.Get(view.Btn_PersonRank.gameObject).onClick = ButtonEvent_PersonRank;
        UIEventListener.Get(view.Btn_Fight.gameObject).onClick = ButtonEvent_Fight;
        UIEventListener.Get(view.Btn_CloseTileInfoButton.gameObject).onClick = ButtonEvent_CloseTileInfoButton;
        UIEventListener.Get(view.Spt_TileInfoMask.gameObject).onClick = ButtonEvent_CloseTileInfoButton;
        UIEventListener.Get(view.Btn_DetailInfoButton.gameObject).onClick = ButtonEvent_DetailInfoButton;
        UIEventListener.Get(view.Btn_CloseSPOutputButton.gameObject).onClick = ButtonEvent_CloseSPOutputButton;
        UIEventListener.Get(view.Spt_SPInfoMask.gameObject).onClick = ButtonEvent_CloseSPOutputButton;
        UIEventListener.Get(view.Btn_CloseServerInfoButton.gameObject).onClick = ButtonEvent_CloseServerInfoButton;
        UIEventListener.Get(view.Spt_ServerInfoMask.gameObject).onClick = ButtonEvent_CloseServerInfoButton;
        UIEventListener.Get(view.Btn_CloseUnionInfoButton.gameObject).onClick = ButtonEvent_CloseUnionInfoButton;
        UIEventListener.Get(view.Spt_UnionInfoMask.gameObject).onClick = ButtonEvent_CloseUnionInfoButton;
        UIEventListener.Get(view.Btn_BattleAward.gameObject).onClick = ButtonEvent_BattleAward;
        UIEventListener.Get(view.Btn_SeasonAward.gameObject).onClick = ButtonEvent_SeasonAward;
        UIEventListener.Get(view.Btn_StageClose.gameObject).onClick = ButtonEvent_StageClose;
        UIEventListener.Get(view.Spt_StageInfoMask.gameObject).onClick = ButtonEvent_StageClose;
        UIEventListener.Get(view.Btn_ReadyBattle.gameObject).onClick = ButtonEvent_ReadyBattle;

    }
    #endregion

    #region 逆初始化函数
    public override void Uninitialize()
    {
        //清除计时器
        Scheduler.Instance.RemoveTimer(UpdateTimeShow);
        Scheduler.Instance.RemoveTimer(FightCDCountDown);
        Scheduler.Instance.RemoveTimer(TimeCountInServerList);
        Scheduler.Instance.RemoveTimer(ShowCountdownInStage);
    }
    public override void Destroy()
    {
        CrossServerWarModule.Instance.recordPos = Vector3.zero;
        //清除地块的特殊产出数据
        if (specialProductionList != null)
        {
            specialProductionList.Clear();
            specialProductionList = null;
        }
        //清除地块数据
        if (tileDic != null)
        {
            tileDic.Clear();
            tileDic = null;
        }
        //清除地块排行榜数据
        if (tileRankList != null)
        {
            tileRankList.Clear();
            tileRankList = null;
        }
        //清除特殊产出列表数据
        if (spItemList != null)
        {
            spItemList.Clear();
            spItemList = null;
        }
        if (spDescList != null)
        {
            spDescList.Clear();
            spDescList = null;
        }
        //清除关卡界面守将数据
        if (enemy_dic != null)
        {
            enemy_dic.Clear();
            enemy_dic = null;
        }
        stageInfo = null;
        //清除预制
        if (view != null)
        {
            ClearAllItems();
            view = null;
        }
    }
    private void ClearAllItems()
    {

        ClearItems(view.Table_UnionOutputTable);
        ClearItems(view.Grd_TileRankGrid.transform);
        ClearItems(view.Grd_SpecialProduction.transform);
        ClearItems(view.Grd_EnemyGrid.transform);
    }
    #endregion
    //=============================================功能函数=========================================
    #region 主界面

    /// <summary>
    /// 根据配置表生成地块
    /// </summary>
    private IEnumerator CreatAllTile()
    {
        if (tileDic == null || tileDic.Count == 0)
        {
            List<CrossServerTerritoryData> tiles = ConfigManager.Instance.mCrossServerWarConfig.GetAllTileInfo();
            foreach (CrossServerTerritoryData tmp in tiles)
            {
                if (tmp.territory_type == -1)
                    continue;
                TileInfo go = CommonFunction.InstantiateItem<TileInfo>(view.Gobj_TileItem, view.Trans_TileGrid);
                if (tmp.row_num % 2 == 1)
                {
                    go.transform.localPosition = new Vector3(86 * (tmp.column_num - 1), -52* (tmp.row_num - 1), 0);
                }
                else
                {
                    go.transform.localPosition = new Vector3(86 * (tmp.column_num - 1) + 43, -52 * (tmp.row_num - 1), 0);
                }
                go.Initialize(tmp.territory_id, tmp.goods_num, tmp.special_production, tmp.territory_type, tmp.name);
                go.name = tmp.name;
                go.gameObject.SetActive(true);
                tileDic.Add(go.Tile_ID, go);
                if (tmp.special_production != 0)
                {
                    SpecialProductionItem item = new SpecialProductionItem(tmp.territory_id, tmp.desc, tmp.name);
                    spItemList.Add(item);
                }
            }
        }
        
        yield return 0;
    }
    /// <summary>
    /// 更新地块信息
    /// </summary>
    public void UpdateAllTileInfo()
    {
        CrossServerWarModule.Instance.OnSendEnterCrossServerWar();
    }
    /// <summary>
    /// 查询地块
    /// </summary>
    /// <param name="id"></param>
    private void SearchTileID(string name)
    {
        if (tileDic == null||tileDic.Count<1)
        {
            Debug.LogError("the tileDic is none for some reason");
            return;
        }
        uint id = 0;
        foreach(TileInfo tile in tileDic.Values)
        {
            if (tile.name == name.ToUpper())
                id = tile.Tile_ID;
        }
        if (id != 0)
        {
            tileDic[id].ShowTileInfo();
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CROSSSERVER_SEARCH_ERROR);
        }
    }
    /// <summary>
    /// 更新主界面的时间显示信息
    /// </summary>
    private void UpdateTimeShow()
    {
        UpdateTime();
        view.Spt_BattleCountdown.gameObject.SetActive(true);
        if (CrossServerWarModule.Instance.isBattleStart)
        {
            if (restCountDown > 0)
            {
                if (!view.Spt_RestCountdown.gameObject.activeSelf)
                {
                    view.Lbl_RestCountdownDesc.gameObject.SetActive(true);
                    view.Spt_RestCountdown.gameObject.SetActive(true);
                }
                view.Lbl_RestCountdownDesc.text = string.Format(ConstString.CROSSSERVER_REST_TIME, CommonFunction.GetTimeString(restCountDown));
            }
            else
            {
                if (view.Spt_RestCountdown.gameObject.activeSelf)
                {
                    view.Lbl_RestCountdownDesc.gameObject.SetActive(false);
                    view.Spt_RestCountdown.gameObject.SetActive(false);
                }
            }
            view.Lbl_BattleCountdownDesc.text = string.Format(ConstString.CROSSSERVER_BATTLE_START_TIME, CommonFunction.GetTimeString(battleCountDown));
        }
        else
        {
            if (view.Spt_RestCountdown.gameObject.activeSelf)
            {
                view.Lbl_RestCountdownDesc.gameObject.SetActive(false);
                view.Spt_RestCountdown.gameObject.SetActive(false);
            }
            view.Lbl_BattleCountdownDesc.text = string.Format(ConstString.CROSSSERVER_BATTLE_END_TIME, CommonFunction.GetTimeString(battleCountDown));
        }

    }
    /// <summary>
    /// 单服战特殊设置
    /// </summary>
    private void SpecialSetting(bool isCross)
    {
        if (isCross)
            return;
        view.Trans_ShowField.GetComponent<TweenScale>().to = Vector3.one;
        view.Trans_BattleFieldSprite.localScale = Vector3.one * scale;
        view.Trans_TileGrid.transform.localPosition = new Vector3(-379, 179, 0);
    }
    /// <summary>
    /// 变更军团名显示方式
    /// </summary>
    /// <param name="isWhole"></param>
    private void SetWholeTileName(bool isWhole)
    {
        foreach (TileInfo tile in UISystem.Instance.CrossServerWarView.tileDic.Values)
        {
            tile.SetTileName(isWhole);
        }
    }
    /// <summary>
    /// 战斗结束后，战场置总览状态
    /// </summary>
    public void ResetFiledAfterBattler()
    {
        if (CrossServerWarModule.Instance.isCrossServer)
            view.Trans_ShowField.localScale = Vector3.one * scale;
        else
        {
            view.Trans_ShowField.localScale = Vector3.one;
            SetWholeTileName(true);
        }
    }
    #endregion

    #region 地块信息界面
    /// <summary>
    /// 根据服务器返回更新地块信息
    /// </summary>
    public void ShowTileInfo(uint owner_type,uint id,uint goods_num,uint output,string union_name,uint border_type,uint turns,string _name)
    {
        view.Gobj_TileInfo.SetActive(true);
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        ShowFightState();
        switch (owner_type)
        {
            case 0:
                view.Lbl_TileTitle.text = ConstString.CROSSSERVER_TILEINFO_TITLE_UNOCCUPPIED;
                view.Lbl_OwnerContent.text = ConstString.CROSSSERVER_TILEINFO_OWNER_UNOCCUPPIED;
                view.Lbl_StateContent.text = ConstString.CROSSSERVER_TILEINFO_STATE_BATTLE;
                view.Lbl_MilitarySuppliesContent.text = goods_num.ToString();
                view.Lbl_ControlTurnstContent.text = ConstString.CROSSSERVER_TILEINFO_TURNS_UNOCCUPPIED;
                break;
            case 1:
            case 2:
                view.Lbl_TileTitle.text = ConstString.CROSSSERVER_TILEINFO_TITLE_OCCUPPIED;
                view.Lbl_OwnerContent.text = string.Format(ConstString.CROSSSERVER_TILEINFO_OWNER_OCCUPPIED, union_name);
                if (border_type == 1)
                    view.Lbl_StateContent.text = ConstString.CROSSSERVER_TILEINFO_STATE_BATTLE;
                else
                    view.Lbl_StateContent.text = ConstString.CROSSSERVER_TILEINFO_STATE_SAFE;
                view.Lbl_MilitarySuppliesContent.text = string.Format(ConstString.CROSSSERVER_TILEINFO_GOODS, goods_num,
                    (int)((Math.Max(1, turns)-1) * ConfigManager.Instance.mCrossServerWarConfig.GetCrossServerWarSettingData().retain_gain));
                view.Lbl_ControlTurnstContent.text = string.Format(ConstString.CROSSSERVER_TILEINFO_TURNS_OCCUPPIED, turns);
                break;
        }
        cur_tileID = id;
        view.Lbl_TileIDContent.text = _name;
        if (output == 0)
        {
            view.Lbl_SpecialProdutionLabel.gameObject.SetActive(false);
            view.Grd_SpecialProduction.gameObject.SetActive(false);
        }
        else
        {
            view.Lbl_SpecialProdutionLabel.gameObject.SetActive(true);
            view.Grd_SpecialProduction.gameObject.SetActive(true);
            List<CommonItemData> outputList = CommonFunction.GetCommonItemDataList(output);
            if (outputList == null || outputList.Count < 1 )
            {
                Debug.LogError("Tile's special production drop id is wrong id =" + output);
                return;
            }
            if (outputList.Count > specialProductionList.Count)
            {
                for (int i = 0; i < outputList.Count; i++)
                {
                    if (i < specialProductionList.Count)
                    {
                        specialProductionList[i].SetItemInfo(outputList[i], view.Scr_OutputPanel);
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_AwardGoods, view.Grd_SpecialProduction.transform);
                        go.name = "Output" + i.ToString();
                        CrossServerAwardItem item = go.AddComponent<CrossServerAwardItem>();
                        specialProductionList.Add(item);
                        item.Initialize();
                        item.SetItemInfo(outputList[i], view.Scr_OutputPanel);
                    }
                }
            }
            else
            {
                for(int i = 0; i < specialProductionList.Count; i++)
                {
                    if (i < outputList.Count)
                    {
                        specialProductionList[i].SetItemInfo(outputList[i], view.Scr_OutputPanel);
                    }
                    else
                    {
                        specialProductionList[i].gameObject.SetActive(false);
                    }
                }
            }

            view.Grd_SpecialProduction.Reposition();
            view.Scr_OutputPanel.ResetPosition();
        }
    }
    /// <summary>
    /// 显示排行榜信息
    /// </summary>
    public void ShowTileRank(List<TerritoryPointRank> ranklist,bool isUnionRank)
    {
        if (ranklist == null)
        {
            Debug.LogError("Tile's ranklist is none");
            return;
        }
        TerritoryPointRank myRank = new TerritoryPointRank();
        if (isUnionRank)
        {
            view.Lbl_TitleName.text = ConstString.SUB_TITLE_UNION_NAME;
            myRank = ranklist.Find(s => { return s.name == UnionModule.Instance.UnionInfo.base_info.name; });
            view.Lbl_MyName.text = UnionModule.Instance.UnionInfo.base_info.name;
            view.Spt_Selected.transform.localPosition = unionBtnPos;
        }
        else
        {
            view.Lbl_TitleName.text = ConstString.SUB_TITLE_MEMBER_NAME;
            myRank = ranklist.Find(s => { return s.name == PlayerData.Instance._NickName; });
            view.Lbl_MyName.text = PlayerData.Instance._NickName;
            view.Spt_Selected.transform.localPosition = personalBtnPos;
            if (ranklist.Count > 50)
                ranklist.RemoveRange(50, ranklist.Count - 50);
        }
        if (myRank == null)
        {
            view.Lbl_MyRank.text = ConstString.NOT_JOIN_FIGHT;
            view.Lbl_MyScore.text = "0";
        }
        else
        {
            view.Lbl_MyRank.text = myRank.order.ToString();
            view.Lbl_MyScore.text = myRank.point.ToString();
        }
        if (tileRankList.Count < ranklist.Count)
        {
            for(int i = 0; i < ranklist.Count; i++)
            {
                if (i < tileRankList.Count)
                {
                    tileRankList[i].SetLabel(ranklist[i]);
                }
                else
                {
                    TilePointRankItem go = CommonFunction.InstantiateItem<TilePointRankItem>(view.Gobj_TileRank, view.Grd_TileRankGrid.transform);
                    go.name = "RankItem" + i;
                    go.Initialize();
                    go.SetLabel(ranklist[i]);
                    tileRankList.Add(go);
                }
            }
        }
        else
        {
            for(int i = 0; i < tileRankList.Count; i++)
            {
                if (i < ranklist.Count)
                {
                    tileRankList[i].SetLabel(ranklist[i]);
                }
                else
                {
                    tileRankList[i].gameObject.SetActive(false);
                }
            }
        }
        view.Grd_TileRankGrid.Reposition();
    }
    /// <summary>
    /// 显示进攻CD及状态
    /// </summary>
    private void ShowFightState()
    {
        if (CrossServerWarModule.Instance.isBattleStart)
        {
            CommonFunction.SetBtnState(view.Btn_Fight.gameObject, true);
            view.Btn_Fight.defaultColor = Color.white;
            if (restCountDown > 0)
            {
                view.Lbl_FightCDTip.gameObject.SetActive(true);
                view.Lbl_BtnFightLabel.text = ConstString.CROSSSERVER_ATTACK_IMMEDIATELY;
                FightCDCountDown();
                Scheduler.Instance.AddTimer(1, true, FightCDCountDown);
            }
            else
            {
                view.Lbl_FightCDTip.gameObject.SetActive(false);
                view.Lbl_BtnFightLabel.text = ConstString.CROSSSERVER_ATTACK;
            }
        }
        else
        {
            view.Lbl_FightCDTip.gameObject.SetActive(false);
            CommonFunction.SetBtnState(view.Btn_Fight.gameObject, false);
        }
    }
    /// <summary>
    /// 进攻CD倒计时
    /// </summary>
    private void FightCDCountDown()
    {
        if (restCountDown > 0)
        {
            view.Lbl_FightCDTip.text = string.Format(ConstString.CROSSSERVER_RESTCOUNTDOWN, CommonFunction.GetTimeString(restCountDown));
        }
        else
        {
            Scheduler.Instance.RemoveTimer(FightCDCountDown);
            ShowFightState();
        }
    }
    /// <summary>
    /// 购买CD是否免提示选择
    /// </summary>
    /// <param name="selected"></param>
    private void AutoBuyCDMarkCallback(bool selected)
    {
        autoBuyCD = selected;
    }

    #endregion

    #region 服务器（奖励）信息界面
    
    public void GetServerToShowInfo()
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        ShowServerInfo();
        ReadBattleAwardItems();
    }
    /// <summary>
    /// 显示设置
    /// </summary>
    private void ShowServerInfo()
    {
        view.Gobj_ServerInfo.SetActive(true);

        if (CrossServerWarModule.Instance.isCrossServer)
        {
            view.Lbl_ServerTitle.text = ConstString.CROSSSERVER_SERVERINFO_CROSSSERVERTITLE;
            view.Trans_AwardListBlock.localPosition = new Vector3(112, -64, 0);
            view.Spt_AwardListBG.width = 407;
            view.Spt_AwardTitleBG.width = 393;
            view.Gobj_ServerListBlock.SetActive(true);
            ShowServersName(CrossServerWarModule.Instance.serverNameList);
        }
        else
        {
            view.Lbl_ServerTitle.text = ConstString.CROSSSERVER_SERVERINFO_SINGLESERVERTITLE;
        }
        TimeCountInServerList();
        Scheduler.Instance.AddTimer(1, true, TimeCountInServerList);
    }
    /// <summary>
    /// 服务器奖励界面倒计时
    /// </summary>
    private void TimeCountInServerList()
    {
        if (CrossServerWarModule.Instance.isBattleStart)
        {
            view.Lbl_Bulletin.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_BATTLESTART, CrossServerWarModule.Instance.game_times, CommonFunction.GetTimeString(battleCountDown));
        }
        else
        {
            view.Lbl_Bulletin.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_BATTLEEND, CrossServerWarModule.Instance.game_times, CommonFunction.GetTimeString(battleCountDown));
        }
    }
    /// <summary>
    /// 从配置表读入战役奖励内容,并生成UI
    /// </summary>
    private void ReadBattleAwardItems()
    {
        view.Lbl_TitleAward.text = ConstString.CROSSSERVER_SERVERINFO_BATTLEAWARD;
        List<CrossServerBattleData> dataList = ConfigManager.Instance.mCrossServerWarConfig.GetCrossServerWarBattleDataList();
        if (dataList == null && dataList.Count < 1)
        {
            Debug.LogError("Battle award list doesn't exist in config");
            return;
        }
        for(int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].goods_ceil < dataList[i].goods_floor && dataList[i].goods_ceil!=-1)
            {
                Debug.LogError("line " + i + " in battle congfig is error : the number of goods error");
                continue;
            }
            else 
            {
                List<CommonItemData> itemList = CommonFunction.GetCommonItemDataList(dataList[i].goods_award);
                if (itemList == null || itemList.Count < 1)
                {
                    Debug.LogError("line " + i + " in battle congfig is error : the award dropID error");
                    continue;
                }
                else
                {

                    if (dataList[i].goods_floor < dataList[i].goods_ceil)
                    {
                        view.Lbl_RankingLabel.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_GOODSLABEL_NORMAL, dataList[i].goods_floor, dataList[i].goods_ceil);
                    }
                    else if(dataList[i].goods_floor == dataList[i].goods_ceil)
                    {
                        view.Lbl_RankingLabel.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_GOODSLABEL_EQUAL, dataList[i].goods_floor);
                    }
                    else
                    {
                        view.Lbl_RankingLabel.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_GOODSLABEL_GREATER, dataList[i].goods_floor);
                    }
                    GameObject awardItem = CommonFunction.InstantiateObject(view.Gobj_RankingAwardItem, view.UITable_Table.transform);
                    awardItem.name = "AwardItem" + i.ToString();
                    awardItem.SetActive(true);
                    Transform trans = awardItem.transform.FindChild("RankingGoods");
                    for (int j = 0; j < itemList.Count; j++)
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_AwardGoods, trans);
                        go.name = "Goods" + j.ToString();
                        CrossServerAwardItem item = go.AddComponent<CrossServerAwardItem>();
                        item.Initialize();
                        item.SetItemInfo(itemList[j], view.ScrView_ShowAwards);                       
                    }
                    trans.GetComponentInChildren<UIGrid>().Reposition();
                    ClearItems(view.Grd_RankingGoods);
                }
            }
        }
        view.UITable_Table.Reposition();
        view.ScrView_ShowAwards.ResetPosition();
        //=========================================↑生成奖励，↓更改按钮============================================
        view.Btn_BattleAward.collider.enabled = false;
        CommonFunction.SetSpriteName(view.Spt_BtnBattleAwardBattleAwardBtn, state_down_sprite);
        view.Lbl_BtnBattleAwardLabel.color = state_down_color;
        view.Btn_SeasonAward.collider.enabled = true;
        view.Btn_SeasonAward.SetState(UIButtonColor.State.Normal, true);
        CommonFunction.SetSpriteName(view.Spt_BtnSeasonAwardSeasonAwardBtn, state_up_sprite);
        view.Lbl_BtnSeasonAwardLabel.color = state_up_color;
    }
    /// <summary>
    /// 从配置表读入赛季奖励内容,并生成UI
    /// </summary>
    private void ReadSeasonAwardItems()
    {
        view.Lbl_TitleAward.text = ConstString.CROSSSERVER_SERVERINFO_SEASONAWARD;
        List<CrossServerSeasonData> dataList = ConfigManager.Instance.mCrossServerWarConfig.GetCrossServerWarSeasonDataList();
        if (dataList == null && dataList.Count < 1)
        {
            Debug.LogError("Battle award list doesn't exist in config");
            return;
        }
        for (int i = 0; i < dataList.Count; i++)
        {
            if (dataList[i].rank_ceil < dataList[i].rank_floor && dataList[i].rank_ceil != -1)
            {
                Debug.LogError("line " + i + " in season congfig is error : the number of rank error");
                continue;
            }
            else
            {
                List<CommonItemData> itemList = CommonFunction.GetCommonItemDataList(dataList[i].rank_award);
                if (itemList == null || itemList.Count < 1)
                {
                    Debug.LogError("line " + i + " in season congfig is error : the award dropID error");
                    continue;
                }
                else
                {

                    if (dataList[i].rank_floor < dataList[i].rank_ceil)
                    {
                        view.Lbl_RankingLabel.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_RANKLABEL_NORMAL, dataList[i].rank_floor, dataList[i].rank_ceil);
                    }
                    else if (dataList[i].rank_floor == dataList[i].rank_ceil)
                    {
                        view.Lbl_RankingLabel.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_RANKLABEL_EQUAL, dataList[i].rank_floor);
                    }
                    else
                    {
                        view.Lbl_RankingLabel.text = string.Format(ConstString.CROSSSERVER_SERVERINFO_RANKLABEL_GREATER, dataList[i].rank_floor);
                    }
                    GameObject awardItem = CommonFunction.InstantiateObject(view.Gobj_RankingAwardItem, view.UITable_Table.transform);
                    awardItem.name = "AwardItem" + i.ToString();
                    awardItem.SetActive(true);
                    Transform trans = awardItem.transform.FindChild("RankingGoods");
                    for (int j = 0; j < itemList.Count; j++)
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_AwardGoods, trans);
                        go.name = "Goods" + j.ToString();
                        CrossServerAwardItem item = go.AddComponent<CrossServerAwardItem>();
                        item.Initialize();
                        item.SetItemInfo(itemList[j], view.ScrView_ShowAwards);
                    }
                    trans.GetComponentInChildren<UIGrid>().Reposition();
                    ClearItems(view.Grd_RankingGoods);
                }
            }
        }
        view.UITable_Table.Reposition();
        view.ScrView_ShowAwards.ResetPosition();
        //=========================================↑生成奖励，↓更改按钮============================================
        view.Btn_BattleAward.collider.enabled = true;
        view.Btn_BattleAward.SetState(UIButtonColor.State.Normal, true);
        view.Btn_BattleAward.ResetDefaultColor();
        CommonFunction.SetSpriteName(view.Spt_BtnBattleAwardBattleAwardBtn, state_up_sprite);
        view.Lbl_BtnBattleAwardLabel.color = state_up_color;
        view.Btn_SeasonAward.collider.enabled = false;
        CommonFunction.SetSpriteName(view.Spt_BtnSeasonAwardSeasonAwardBtn, state_down_sprite);
        view.Lbl_BtnSeasonAwardLabel.color = state_down_color;
    }
    /// <summary>
    /// 显示参与跨服战的服务器名
    /// </summary>
    /// <param name="list"></param>
    private void ShowServersName(List<string> list)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("There is no data about servers");
            return;
        }
        for(int i = 0; i < list.Count; i++)
        {
            GameObject go = CommonFunction.InstantiateObject(view.Lbl_ServerName.gameObject, view.Grd_ServerListGrid.transform);
            go.GetComponent<UILabel>().text = list[i];
        }
        view.Grd_ServerListGrid.Reposition();
    }
    #endregion

    #region 军团信息界面
    /// <summary>
    /// 显示军团界面信息
    /// </summary>
    public void ShowUnionInfo()
    {
        view.Lbl_UnionNameLabel.text = UnionModule.Instance.UnionInfo.base_info.name;
        view.Lbl_UnionLevelLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LEVEL, UnionModule.Instance.UnionInfo.base_info.level);
        CommonFunction.SetSpriteName(view.Spt_IconSprite, ConfigManager.Instance.mUnionConfig.GetUnionIconByID(UnionModule.Instance.UnionInfo.base_info.icon));
        view.Lbl_PopulationtnContent.text = UnionModule.Instance.UnionInfo.base_info.member_num + "/" + ConfigManager.Instance.mUnionConfig.GetUnionLvUpData(UnionModule.Instance.UnionInfo.base_info.level).mMemberCount;
        view.Lbl_MaterialContent.text = CrossServerWarModule.Instance.unionGoods.ToString();
        view.Lbl_NormalTerritoryContent.text = CrossServerWarModule.Instance.normal_tile_num.ToString();
        view.Lbl_SpecialTerritoryContent.text = CrossServerWarModule.Instance.special_tile_num.ToString();
        view.Lbl_FightTerritoryContent.text = CrossServerWarModule.Instance.fight_tile_num.ToString();
        view.Lbl_MinProdutionContent.text = CrossServerWarModule.Instance.normal_output.ToString();
    }
    /// <summary>
    /// 展示特殊产出
    /// </summary>
    private void ShowSpecialProduction()
    {
        //spItemList.Sort();
        for(int i=0;i<spItemList.Count;i++)
        {
            if (spDescList.Count < spItemList.Count)
            {
                SpecialProductionDesc go = CommonFunction.InstantiateItem<SpecialProductionDesc>(view.Gobj_OutputItem, view.Table_UnionOutputTable.transform);
                go.Initialize();
                go.name = "special production" + i;
                go.gameObject.SetActive(true);
                spDescList.Add(go);
                
            }

            spDescList[i].SetSpecialProductionDesc(spItemList[i].tileId, spItemList[i].production,spItemList[i].name);
        }
        view.Table_UnionOutputTable.Reposition();
    }
    #endregion

    #region 关卡信息界面
    /// <summary>
    /// 显示关卡信息
    /// </summary>
    public void ShowGateInfo()
    {
        view.Gobj_StageInfo.gameObject.SetActive(true);
        if (CrossServerWarModule.Instance.isCrossServer)
            view.Lbl_TypeTitle.text = ConstString.CROSSSERVER_SERVERINFO_CROSSSERVERTITLE;
        else
            view.Lbl_TypeTitle.text = ConstString.CROSSSERVER_SERVERINFO_SINGLESERVERTITLE;
        if (CrossServerWarModule.Instance.tileUnionRank.Count > 0)
        {
            view.Lbl_CorpsName.text = CrossServerWarModule.Instance.tileUnionRank[0].name;
            view.Lbl_IntegralNum.text = CrossServerWarModule.Instance.tileUnionRank[0].point.ToString();
        }
        else
        {
            view.Lbl_CorpsName.text = "--";
            view.Lbl_IntegralNum.text = "0";
        }
        TerritoryPointRank rank = CrossServerWarModule.Instance.tileUnionRank.Find(s => s.name == UnionModule.Instance.UnionInfo.base_info.name);
        if (rank == null)
        {
            view.Lbl_MyIntegralNum.text = "0";
        }
        else
        {
            view.Lbl_MyIntegralNum.text = rank.point.ToString();
        }
        ShowCountdownInStage();
        Scheduler.Instance.AddTimer(1, true, ShowCountdownInStage);
        stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(ConfigManager.Instance.mCrossServerWarConfig.GetTileStageID(cur_tileID));
        if (stageInfo == null)
        {
            Debug.LogError("stage info is null");
            return;
        }
        else
        {
            view.Lbl_StageDesc.text = stageInfo.Describe;
            UpdateEnemyCast();
        }

    }
    /// <summary>
    /// 关卡信息界面倒计时显示
    /// </summary>
    private void ShowCountdownInStage()
    {
        view.Lbl_GateCount.text = CommonFunction.GetTimeString(battleCountDown);
    }
    /// <summary>
    /// 更新地块守将信息
    /// </summary>
    /// <param name="stageInfo"></param>
    private void UpdateEnemyCast()
    {
        view.Gobj_Enemy.gameObject.SetActive(false);
        if (stageInfo.EnemySquad.Count <= enemy_dic.Count)
        {
            for (int index = 0; index < enemy_dic.Count; index++)
            {
                GateEnemyInfoComponent comp = enemy_dic[index];
                if (index < stageInfo.EnemySquad.Count)
                {
                    SingleEnemyInfo _singleinfo = stageInfo.EnemySquad[index];
                    if (_singleinfo == null) continue;
                    MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                    if (_monster == null) continue;
                    if (_singleinfo.IsBoss == 1)
                    {
                        comp.UpdateInfo(_monster, true);
                    }
                    else
                    {
                        comp.UpdateInfo(_monster, false);
                    }
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.Clear();
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int enemyObj_count = enemy_dic.Count;
            for (int index = 0; index < stageInfo.EnemySquad.Count; index++)
            {
                SingleEnemyInfo _singleinfo = stageInfo.EnemySquad[index];
                if (_singleinfo == null) continue;
                MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                if (_monster == null) continue;
                GateEnemyInfoComponent comp = null;
                if (index < enemyObj_count)
                {
                    comp = enemy_dic[index];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_Enemy, view.Grd_EnemyGrid.transform);
                    comp = new GateEnemyInfoComponent(go);
                    go.name = _monster.ID.ToString();
                    enemy_dic.Add(comp);
                }
                if (_singleinfo.IsBoss == 1)
                {
                    comp.UpdateInfo(_monster, true);
                }
                else
                {
                    comp.UpdateInfo(_monster, false);
                }
                comp.AddPressLisetener(ButtonEvent_EnemyComp);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_EnemyGrid.repositionNow = true;
    }
    #endregion

    #region 通用方法
    /// <summary>
    /// 清除生成的物品
    /// </summary>
    private void ClearItems(UITable table)
    {
        List<Transform> childs = table.GetChildList();
        for (int i = 0; i < childs.Count; i++)
            UnityEngine.GameObject.Destroy(childs[i].gameObject);
    }
    /// <summary>
    /// 清除生成的物品
    /// </summary>
    private void ClearItems(UIGrid grid)
    {
        List<Transform> childs = grid.GetChildList();
        for (int i = 0; i < childs.Count; i++)
            UnityEngine.GameObject.Destroy(childs[i].gameObject);
    }
    /// <summary>
    /// 清除生成的物品
    /// </summary>
    private void ClearItems(Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            UnityEngine.GameObject.Destroy(trans.GetChild(i).gameObject);
        }
    }
    /// <summary>
    /// 时间更新函数
    /// </summary>
    private void UpdateTime()
    {
        battleCountDown = CrossServerWarModule.Instance.time_Battle - Main.mTime;
        restCountDown = CrossServerWarModule.Instance.time_CD - Main.mTime;
        //Debug.LogError(CrossServerWarModule.Instance.time_CD);
        if (battleCountDown < 0)
        {
            battleCountDown = 0;
            UpdateAllTileInfo();
        }
    }

    #endregion

    //===========================test=============================


}
