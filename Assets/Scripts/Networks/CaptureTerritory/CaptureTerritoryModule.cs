using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class CaptureTerritoryModule : Singleton<CaptureTerritoryModule>
{
    private CaptureTerritoryNetwork _network;
    public List<TokenLevel> TokenInfo
    {
        private set
        {
            _tokenInfo = value;
        }
        get
        {
            if (_tokenInfo == null)
                return new List<TokenLevel>();
            return _tokenInfo;
        }
    }
    private List<TokenLevel> _tokenInfo;
    public List<ChestInfo> ChestInfo
    {
        private set
        {
            _chestInfo = value;
            TotalBoxCount = 0;
            if (_chestInfo.Count > 0)
            {
                foreach (ChestInfo item in _chestInfo)
                {
                    TotalBoxCount += item.num;
                }
            }
        }
        get
        {
            if (_chestInfo == null)
                return new List<ChestInfo>();
            return _chestInfo;
        }
    }
    private List<ChestInfo> _chestInfo;

    public List<CampaignRankInfo> UnionRank
    {
        private set
        {
            _unionRank = value;
        }
        get
        {
            if (_unionRank == null)
                return new List<CampaignRankInfo>();
            return _unionRank;
        }
    }
    private List<CampaignRankInfo> _unionRank;
    public List<CampaignRankInfo> PersonalRank
    {
        private set
        {
            _personalRank = value;
        }
        get
        {
            if (_personalRank == null)
                return new List<CampaignRankInfo>();
            return _personalRank;
        }
    }
    private List<CampaignRankInfo> _personalRank;

    public List<CampaignRankInfo> UnionMemberScoreList;

    private bool _isOpenCTView = false;
    private Dictionary<uint, AccidToNum> _allocateBoxRecord = new Dictionary<uint, AccidToNum>();
    private int _boxAllocatedCount;
    private List<UnionOccupiedCity> UnionOccupiedCityInfo = null;

    public event System.Action FightCDTimerUpdate;
    public event System.Action CampaignTimerUpdate;

    public int CampaignTimer;
    public int FightCDTimer;
    public int TotalBoxCount;
    /// <summary>
    /// 立即出战不再提醒
    /// </summary>
    public bool FightCDAutoBuy = false;
    public ECaptureTerritoryFightState FightState;    
    public CampaignRankInfo MyPersonRankInfo = null;
    public CampaignRankInfo MyUnionRankInfo = null;
    public CampaignRankInfo MyDamageRankInfo = null;

    public uint CurrentCityID;
    public CaptureCityData CurrentCity;
    public NationType CurrentCountry;
    public void Initialize()
    {
        if (_network == null)
        {
            _network = new CaptureTerritoryNetwork();
            _network.RegisterMsg();
        }
    }

    public void Uninitialize()
    {
        if (_network != null)
        {
            _network.RemoveMsg();
            _network = null;
        }

        _isOpenCTView = false;
        _allocateBoxRecord.Clear();
        _boxAllocatedCount = 0;
        FightCDTimerUpdate = null;
        CampaignTimerUpdate = null;
        CampaignTimer = 0;
        FightCDTimer = 0;
        TotalBoxCount = 0;
        MyPersonRankInfo = null;
        MyUnionRankInfo = null;
        CurrentCityID = 0;
        CurrentCity = null;
        FightCDAutoBuy = false;
    }

    public void OpenCaptureTerritory()
    {
        if (!UnionModule.Instance.HasUnion)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
            return;
        }
        SendGetCampaignTokenInfo(true);
    }

    public void SelectedCity(CaptureCityData city)
    {
        CurrentCity = city;
        CurrentCityID = CurrentCity.mID;
        CurrentCountry = CurrentCity.mCountry;
        SendGetCampaignRank();
    }
    /// <summary>
    /// 获取当前城池战场的令牌提升效果（0-1）
    /// </summary>
    public float GetCurrentTokenIncrease()
    {
        return GetTokenIncrease(CurrentCountry);
    }
    /// <summary>
    /// 根据国家获取令牌效果
    /// </summary>
    /// <returns></returns>
    public float GetTokenIncrease(NationType nation)
    {
        foreach (TokenLevel item in TokenInfo)
        {
            if ((NationType)item.nation == nation)
            {
                return ConfigManager.Instance.mCaptureTerritoryConfig.GetDamageIncreaseByLevel(nation, item.level);
            }
        }
        return 0;
    }
    public TokenLevel GetTokenLevel(NationType nation)
    {
        foreach (TokenLevel item in TokenInfo)
        {
            if ((NationType)item.nation == nation)
            {
                return item;
            }
        }
        TokenLevel token = new TokenLevel();
        token.nation = (int)nation;
        token.level = 0;
        return token;
    }

    /// <summary>
    /// 添加是否成功
    /// </summary>
    /// <param name="allocation"></param>
    /// <returns></returns>
    public bool SetBoxAllocation(AccidToNum allocation)
    {
        if (_allocateBoxRecord.ContainsKey(allocation.accid))
        {
            _boxAllocatedCount = _boxAllocatedCount - _allocateBoxRecord[allocation.accid].num + allocation.num;

            if (_boxAllocatedCount > TotalBoxCount)
                return false;

            _allocateBoxRecord[allocation.accid].num = allocation.num;            
        }
        else
        {
            _boxAllocatedCount = _boxAllocatedCount + allocation.num;
            if (_boxAllocatedCount > TotalBoxCount)
                return false;
            _allocateBoxRecord.Add(allocation.accid, allocation);
        }

        if (_allocateBoxRecord[allocation.accid].num <= 0)
        {
            _allocateBoxRecord.Remove(allocation.accid);
        }
        return true; 
    }
    /// <summary>
    /// 宝箱是否已经分配光
    /// </summary>
    /// <returns></returns>
    public bool BoxNotEnoughAllocation()
    {
        return _boxAllocatedCount >= TotalBoxCount;
    }
    public void ClearBoxAllocation()
    {
        _allocateBoxRecord.Clear();
        _boxAllocatedCount = 0;
    }

    public void SetFightCDTimer(long time)
    {
        FightCDTimer = Mathf.Max(0, (int)(time - Main.mTime));
        UpdateCDTimer();
        if (FightCDTimer > 0)
        {
            Assets.Script.Common.Scheduler.Instance.AddTimer(1, true, UpdateCDTimer);
        }
    }

    private void UpdateCDTimer()
    {
        if (FightCDTimerUpdate != null)
        {
            FightCDTimerUpdate();
        }

        --FightCDTimer;
        if (FightCDTimer < 0)
        {
            UpdateCDTimerFinish();
        }
    }
    private void UpdateCDTimerFinish()
    {
        FightCDTimer = 0;
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(UpdateCDTimer);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="time">结束时间</param>
    public void SetCampaignTimer(long time)
    {
        CampaignTimer = Mathf.Max(0, (int)(time - Main.mTime));
        UpdateCampaignTimer();
        if (CampaignTimer > 0)
        {
            Assets.Script.Common.Scheduler.Instance.AddTimer(1, true, UpdateCampaignTimer);
        }
    }
    private void UpdateCampaignTimer()
    {
        if (CampaignTimerUpdate != null)
        {
            CampaignTimerUpdate();
        }

        --CampaignTimer;
        if (CampaignTimer < 0)
        {
            UpdataCampaignTimerFinish();
        }
    }

    private void UpdataCampaignTimerFinish()
    {
        CampaignTimer = 0;
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(UpdateCampaignTimer);
    }

    public UnionOccupiedCity GetUnionOccupiedCity(uint cityID)
    {
        foreach (var item in UnionOccupiedCityInfo)
        {
            if (cityID == item.city)
            {
                return item;
            }
        }

        UnionOccupiedCity info = new UnionOccupiedCity();
        info.city = cityID;
        info.union_name = "";
        return info;
    }

    #region 服务器消息通信

    public void CaptureTerritoryNotify(NotifyRefresh notify)
    {
        if (notify.type == NotifyType.CAMPAIGN)
        {
            bool needFinishEffect = FightState == ECaptureTerritoryFightState.Fighting
                && (ECaptureTerritoryFightState)notify.status == ECaptureTerritoryFightState.NotFighting; 

            FightState = (ECaptureTerritoryFightState)notify.status;
            if (FightState == ECaptureTerritoryFightState.Fighting)
            {
                SetCampaignTimer(notify.num);
            }
            else
            {
                SetCampaignTimer(notify.num);
                if (UnionModule.Instance.HasUnion)
                    SendGetCampaignTokenInfo(false);
                
                if (needFinishEffect
                    && UISystem.Instance.UIIsOpen(CaptureTerritoryView.UIName)
                    && !UISystem.Instance.UIIsOpen(CaptureTerritoryInfoView.UIName)
                    && !UISystem.Instance.UIIsOpen(PrepareBattleView.UIName))
                {
                    UISystem.Instance.ShowGameUI(CaptureTerritoryCompleteView.UIName);
                }
            }

            if (UISystem.Instance.UIIsOpen(UnionView.UIName))
            {
                UISystem.Instance.UnionView.UpdateCaptureTerritoryNotice();                
            }
            if (UISystem.Instance.UIIsOpen(CaptureTerritoryView.UIName))
            {
                UISystem.Instance.CaptureTerritoryView.RefreshCityState();
            }
        }
    }

    public void SendGetCampaignTokenInfo(bool isOpenView = false)
    {
        _isOpenCTView = isOpenView;
        GetCampaignTokenInfoReq req = new GetCampaignTokenInfoReq();
        _network.SendGetCampaignTokenInfo(req, isOpenView);
    }
    public void ReceiveGetCampaignTokenInfo(GetCampaignTokenInfoResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            TokenInfo = data.token_info;
            ChestInfo = data.chest_info;
            //UnionMemberScoreList = data.score_info;
            MyDamageRankInfo = data.self_rank;
            UnionOccupiedCityInfo = data.occupied_info;
            if (_isOpenCTView)
            {
                UnionModule.Instance.MyUnionMemberInfo.position = data.position;
                UISystem.Instance.ShowGameUI(CaptureTerritoryView.UIName);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
                UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.World);
            }
            else
            {
                if (UISystem.Instance.UIIsOpen(CaptureTerritoryView.UIName))
                {
                    UISystem.Instance.CaptureTerritoryView.InitUI();
                }
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendGetCampaignRank()
    {
        GetCampaignRankReq req = new GetCampaignRankReq();
        req.id = CurrentCityID;
        _network.SendGetCampaignRank(req);
    }
    public void ReceiveGetCampaignRank(GetCampaignRankResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            MyPersonRankInfo = data.p_self_rank;
            MyUnionRankInfo = data.u_self_rank;
            UnionRank = data.u_rank_info;
            PersonalRank = data.p_rank_info;
            UISystem.Instance.ShowGameUI(CaptureCityInfoView.UIName);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendActivateToken(NationType nation)
    {
        ActivateTokenReq req = new ActivateTokenReq();
        req.city = nation;
        _network.SendActivateToken(req);
    }
    public void ReceiveActivateToken(ActivateTokenResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            TokenInfo = data.token_info;
            PlayerData.Instance.UpdateDiamond(data.diamond);
            if (UISystem.Instance.UIIsOpen(CaptureTokenView.UIName))
            {
                UISystem.Instance.CaptureTokenView.InitUI();
                UISystem.Instance.CaptureTokenView.ShowEffect(data.city);
            }

            if (UISystem.Instance.UIIsOpen(CaptureTerritoryView.UIName))
            {
                UISystem.Instance.CaptureTerritoryView.InitToken();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendStartCampaignPvp(uint stageID, List<SoldierList> soldiers, List<EquipList> equips)
    {
        StartCampaignPvpReq req = new StartCampaignPvpReq();
        req.id = CurrentCityID;
        req.soldiers.Clear();
        req.soldiers.AddRange(soldiers);
        req.equips.Clear();
        req.equips.AddRange(equips);
        _network.SendStartCampaignPvp(req);
    }
    public void ReceiveStartCampaignPvp(StartCampaignPvpResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            TokenInfo = data.token_info;
            UISystem.Instance.PrepareBattleView.OnStartCaptureTerritoryResp(data.hero_attr);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendCampaignReward()
    {
        CampaignRewardReq req = new CampaignRewardReq();
        _network.SendCampaignReward(req);
    }
    public void ReceiveCampaignReward(CampaignRewardResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {

        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendAssignChest()
    {
        if (_allocateBoxRecord == null || _allocateBoxRecord.Count <= 0)
            return;
        List<AccidToNum> list = new List<AccidToNum>(_allocateBoxRecord.Values);        
        AssignChestReq req = new AssignChestReq();
        req.accid_num.AddRange(list);
        _network.SendAssignChest(req);
    }
    public void ReceiveAssignChest(AssignChestResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ALLOCATE_BOX_DONE);
            ChestInfo = data.left_chest_info;
            _allocateBoxRecord.Clear();
            if (UISystem.Instance.UIIsOpen(AllocateBoxView.UIName))
            {
                UISystem.Instance.AllocateBoxView.CleanRecords();
            }
            UISystem.Instance.CaptureTerritoryView.InitAllocateBoxNotice();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendClearCampaignCD()
    {
        ClearCampaignCDReq req = new ClearCampaignCDReq();
        _network.SendClearCampaignCD(req);
    }
    public void ReceiveClearCampaignCD(ClearCampaignCDResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            FightCDTimer = 0;
            UpdateCDTimer();
            PlayerData.Instance.UpdateDiamond(data.diamond);
            UISystem.Instance.CaptureCityInfoView.ShowCityFightInfoView();
            //if (FightCDAutoBuy)
            //{
            //UISystem.Instance.CaptureCityInfoView.ShowCityFightInfoView();                
            //}
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }
    /// <summary>
    /// 获取分配宝箱时的成员排行
    /// </summary>
    public void SendGetScoreInfo()
    {
        GetScoreInfoReq req = new GetScoreInfoReq();
        _network.SendGetScoreInfo(req);
    }
    public void ReceiveGetScoreInfo(GetScoreInfoResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UnionMemberScoreList = data.rank_info;
            if(UnionMemberScoreList != null)
            {
                UnionMemberScoreList.RemoveAll((tmp) => { return tmp == null; });
                UnionMemberScoreList.Sort((left, right) =>
                {
                    if (left.score != right.score)
                    {
                        if (left.score > right.score)
                            return -1;
                        else
                            return 1;
                    }
                    if (left.level != right.level)
                    {
                        if (left.level > right.level)
                            return -1;
                        else
                            return 1;
                    }
                    if (left.id != right.id)
                    {
                        if (left.id > right.id)
                            return -1;
                        else
                            return 1;
                    }
                    return 0;
                });
            }
            UISystem.Instance.ShowGameUI(AllocateBoxView.UIName);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    #endregion
}
