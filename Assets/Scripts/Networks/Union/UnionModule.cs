using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionModule : Singleton<UnionModule>
{
    public enum UnionAuditType
    {
        Single = 2,
        Multi = 1,
    }
    public enum UnionAuditPassType
    {
        Pass = 1,
        Refuse = 2,
    }
    public enum UnionGetHpState
    {
        NotGet = 0,
        Get = 1
    }

    public enum UnionGetHpType
    {
        Multi = 1,
        Single = 2
    }

    public enum OpenViewTypeRefreash
    {
        None,
        Hall,
        Donate
    }

    private UnionNetwork _network;
    /// <summary>
    /// 记录战斗状态  避免多次发送进入战斗
    /// </summary>
    public bool isFightState = false;

    /// <summary>
    /// 查找军团时，军团列表
    /// </summary>
    public List<BaseUnion> UnionList;
    private int _maxUnionNum;
    /// <summary>
    /// 玩家当前的军团的信息
    /// </summary>
    private Union _unionInfo;
    public int UnionPrisonBuffNum = 0;
    public Union UnionInfo
    {
        get { return _unionInfo; }
        set
        {
            if (value == null)
                return;
            _unionInfo = value;
            for (int i = 0; i < _unionInfo.members.Count; i++)
            {
                if (_unionInfo.members[i].charid == PlayerData.Instance._RoleID)
                {
                    _myUnionMemberInfo = _unionInfo.members[i];
                    _unionInfo.members.RemoveAt(i);
                    break;
                }
            }
            _unionInfo.members.Sort(SortTool.SortUnionMember);
        }
    }

    /// <summary>
    /// 暂时解锁图标信息[ID, 剩余有效时间[秒]]
    /// </summary>
    public Dictionary<uint, uint> DicLimitBadge = new Dictionary<uint, uint>();

    private BaseUnion _enemyUnion;
    public BaseUnion EnemyUnion
    {
        get { return _enemyUnion; }
    }
    
    private List<UnionCityPvpResult> _UnionCityPvpArray = new List<UnionCityPvpResult>();
    public List<UnionCityPvpResult> UnionCityPvpArrayList
    {
        get { return _UnionCityPvpArray; }
        set
        {
            _UnionCityPvpArray.Clear();
            _UnionCityPvpArray.AddRange(value);
        }
    }

    public UnionCityPvpResult GetUnionCityInfo(int index)
    {
        for (int i = 0; i < _UnionCityPvpArray.Count; i++)
        {
            if (_UnionCityPvpArray[i].city == index)
                return _UnionCityPvpArray[i];
        }
        return null;
    }

    public int MyUnionID
    {
        get { return _unionInfo == null ? PlayerData.Instance.MyUnionID : _unionInfo.base_info.id; }
    }

    public bool HasUnion
    {
        get { return MyUnionID != 0; }
    }
    /// <summary>
    /// 判断玩家是否参战的唯一标准
    /// </summary>
    public int MyCity
    {
        get { return CharUnionInfo.pvp_city; }
    }
    /// <summary>
    /// 打开的城市 并不一定是玩家参战的城市
    /// </summary>
    private int _currentCity;

    /// <summary>
    /// 玩家的军团申请信息记录
    /// </summary>
    private CharUnionInfo _charUnionInfo;
    public CharUnionInfo CharUnionInfo
    {
        get
        {
            if (_charUnionInfo == null)
                _charUnionInfo = PlayerData.Instance.CharUnionInfo;
            return _charUnionInfo;
        }
        set { _charUnionInfo = value; }
    }

    /// <summary>
    /// 玩家在当前军团中的个人信息
    /// </summary>
    private UnionMember _myUnionMemberInfo;
    public UnionMember MyUnionMemberInfo
    {
        get
        {
            if (_myUnionMemberInfo == null)
            {
                _myUnionMemberInfo = GetUnionMember(PlayerData.Instance._RoleID);
            }
            return _myUnionMemberInfo;
        }
    }

    private ArenaPlayer _myBattleInfo;
    public ArenaPlayer MyBattleInfo
    {
        get { return _myBattleInfo; }
    }

    private long _fightBeginTime = 0;
    public long FightBeginTime
    {
        get { return _fightBeginTime; }
    }
    public int TotalPveTimes
    {
        get
        {
            if (ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.ExoticAdventure) < 0)
            {
                return (int)ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mMaxUnionPVETimes;
            }
            else
            {
                int buyTimes = CharUnionInfo.today_buy_times * ConfigManager.Instance.mTimesBuyConfig.GetTimesBuyData().ExoticAdvantureTimes;
                return (int)ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mMaxUnionPVETimes + buyTimes;
            }            
        }
    }
    public int UnionFightRank;
    public void Initialize()
    {
        if (_network == null)
        {
            _network = new UnionNetwork();
            _network.RegisterMsg();
        }

        //PlayerUnionReq();
    }

    private UnionPvpState _unionPvpState;
    public UnionPvpState UnionPvpState
    {
        set
        {
            _unionPvpState = value;
            if (UISystem.Instance.UIIsOpen(UnionView.UIName))
            {
                UISystem.Instance.UnionView.InitUI();
            }
        }
        get { return _unionPvpState; }
    }

    public void Uninitialize()
    {
        if (_network != null)
            _network.RemoveMsg();
        _network = null;
        _unionInfo = null;
        _charUnionInfo = null;
        _myUnionMemberInfo = null;
        _waitingUnionInvites.Clear();
        _curUnionInvite = null;
        _joinOrKickTip = string.Empty;
        _maxUnionNum = 0;
        isFightState = false;
        PlayerData.Instance.NotifyResetEvent -= UpdateResetInfo;
    }

    private void UpdateResetInfo(NotifyReset data)
    {
        if (data == null) return;

        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_EXOTICADVANTUREVIEW))
        {
            UISystem.Instance.ExoticAdvantureView.UpdateAwardsStatus();
        }
    }

    public void UpdatePVEDgnInfo(uint dgnID, int hurt)
    {

        UnionPveDgnInfo dgnInfo = UnionModule.Instance.UnionInfo.dgn_info.Find((tmp) =>
          {
              if (tmp == null) return false;
              return tmp.dgn_id == dgnID;
          });
        if (dgnInfo != null)
            dgnInfo.total_hurt = hurt;
    }

    #region 外部调用&非消息类调用

    private bool _isOpenPve = false;
    public void OpenUnionPve()
    {
        _isOpenPve = true;
        OpenUnion();
    }

    public void UpdateUnionMember(List<UnionMember> members)
    {
        UnionInfo.members.Clear();
        UnionInfo.members.AddRange(members);
        for (int i = 0; i < UnionInfo.members.Count; i++)
        {
            if (UnionInfo.members[i].charid == PlayerData.Instance._RoleID)
            {
                _myUnionMemberInfo = UnionInfo.members[i];
                UnionInfo.members.RemoveAt(i);
                break;
            }
        }
    }

    public void OpenUnion()
    {
        OnSendOpenUnion();
    }

    public UnionPosition GetUnionMemberPosition(uint charId)
    {
        return GetUnionMember(charId).position;
    }

    public UnionMember GetUnionMember(uint charId)
    {
        if (UnionInfo == null)
        {
            //请求信息
            //初始化的时候请求
            return null;
        }

        if (charId == PlayerData.Instance._RoleID && _myUnionMemberInfo != null)
            return _myUnionMemberInfo;

        for (int i = 0; i < UnionInfo.members.Count; i++)
        {
            if (UnionInfo.members[i].charid == charId)
            {
                if (charId == PlayerData.Instance._RoleID)
                {
                    _myUnionMemberInfo = UnionInfo.members[i];
                    UnionInfo.members.RemoveAt(i);
                    return _myUnionMemberInfo;
                }
                return UnionInfo.members[i];
            }
        }

        Debug.LogWarning(string.Format("Can't get union member by id ={0}, my id ={1}", charId, PlayerData.Instance._RoleID));
        return null;
    }

    public string CheckUnionNotice(string content)
    {
        if (string.IsNullOrEmpty(content))
            return ConstString.DEFAULT_UNION_NOTICE;
        else
        {
            return content;
        }
    }

    public void SetOfflineTime(UILabel lbl, long time)
    {
        if (time != 0)
        {
            //Lbl_MemberCount.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME, _member.offline_tick);
            long offlinetime = Main.mTime - time;
            if (offlinetime > 31536000) //一年365算
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_YEAR, offlinetime / 31536000);
            }
            else if (offlinetime > 2592000) //一个月30算
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_MONTH, offlinetime / 2592000);
            }
            else if (offlinetime > 86400) //一天
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_DAY, offlinetime / 86400);
            }
            else if (offlinetime > 3600) //一小时
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_HOUR, offlinetime / 3600);
            }
            else if (offlinetime > 60) //一分钟
            {
                lbl.text = string.Format(ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_MINUTE, offlinetime / 60);
            }
            else //一分钟内
            {
                lbl.text = ConstString.FORMAT_UNION_MEMBER_OFFLINE_TIME_ONE_MINUTE;
            }
        }
        else
        {
            lbl.text = ConstString.UNION_MEMBER_ONLINE;
        }

    }

    public string GetUnionLimitTypeString(UnionLimitType type)
    {
        string str = string.Empty;
        switch (type)
        {
            case UnionLimitType.ULT_LEVEL_MANUAL:
                {
                    str = ConstString.UNION_LIMIT_ULT_LEVEL_MANUAL;
                    break;
                }
            case UnionLimitType.ULT_LEVEL_NONE:
                {
                    str = ConstString.UNION_LIMIT_ULT_LEVEL_NONE;
                    break;
                }
            case UnionLimitType.ULT_MANUAL:
                {
                    str = ConstString.UNION_LIMIT_ULT_MANUAL;
                    break;
                }
            case UnionLimitType.ULT_NONE:
                {
                    str = ConstString.UNION_LIMIT_ULT_NONE;
                    break;
                }
        }
        return str;
    }

    public ArenaPlayer GetMyBattleInfo()
    {
        //UnionCityPvpArray city = GetUnionCityInfo(MyCity);
        //if (city == null)
        //{
        //    Debug.LogError("Cant Find MyCityInfo id = " + PlayerData.Instance._RoleID + "  city = " + MyCity);
        //    return null;
        //}
        //if (city.players == null || city.players.Count < 1)
        //{
        //    Debug.LogError("Player is null !!! city = " + city);
        //    return null;
        //}
        //int count = city.players.Count;
        //for (int i = 0; i < count; i++)
        //{
        //    ArenaPlayer player = city.players[i];
        //    if (player.hero.charid == PlayerData.Instance._RoleID)
        //        return player;
        //}
        //Debug.LogError("Cant find my battle info!!! id = " + PlayerData.Instance._RoleID + " city = " + MyCity);
        return null;
    }

    private void QuickGoToUnion(string name, bool gotUnionInfo)
    {
        System.Action action = null;
        if (gotUnionInfo)
        {
            action = () =>
                         {
                             UISystem.Instance.CloseGameUI(JoinUnionView.UIName);
                             //UISystem.Instance.ShowGameUI(UnionView.UIName);
                             OpenUnion();
                         };
        }
        else
        {
            UISystem.Instance.CloseGameUI(JoinUnionView.UIName);
            action = OpenUnion;
        }
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,
            string.Format(ConstString.JOIN_UNION_SUCCEED, name),
                action, null, ConstString.CREATE_UNION_SUCCEED_GO);
    }

    private void DelOneUnionMember(uint charid)
    {
        int index = -1;
        string delPlayerName = "";
        for (int i = 0; i < UnionInfo.members.Count; i++)
        {
            if (UnionInfo.members[i].charid == charid)
            {
                delPlayerName = UnionInfo.members[i].charname;
                UnionInfo.members.RemoveAt(i);
                index = i;
                break;
            }
        }
        if (index != -1)
        {
            if (UISystem.Instance.UIIsOpen(UnionHallView.UIName))
            {
                UISystem.Instance.UnionHallView.DelOne(index);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.UNION_TIP_KICKSUCCESS, delPlayerName));
            }
        }
        UpdateMemberNum();
    }

    private void UpdateMemberNum()
    {
        UnionInfo.base_info.member_num = UnionInfo.members.Count + 1;
    }

    #region 邀请 被T 加入
    /// <summary>
    /// 显示在等待中的推送消息/军团邀请/被T/加入成功
    /// </summary>
    public void ShowWaitingNotify()
    {
        //主界面打开的时候会调用
        //判断是否有被T和加入成功的提示，有则清空邀请列表(被T24小时和已加入军团不会有邀请)
        if (string.IsNullOrEmpty(_joinOrKickTip))
        {
            //被踢
            //if (_joinOrKickTip.Equals(ConstString.UNION_TIP_KICK))
            //{
            //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.UNION_TIP_KICK);
            //    _joinOrKickTip = "";
            //}
            //else
            //{
            GetNextInviteToUnion();
            //}
        }
        else
        {
            _waitingUnionInvites.Clear();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, _joinOrKickTip);
            _joinOrKickTip = "";
        }
    }

    /// <summary>
    /// 邀请军团 后来这边又修改为玩家的accid
    /// </summary>
    /// <param name="accID"></param>
    public void OnSendInvitePlayerToUnion(uint accID)
    {
        //聊天调用接口，直接给服务器发消息即可
        InvitePlayerJoinUnionReq req = new InvitePlayerJoinUnionReq();
        req.invitees = (int)accID;
        _network.OnSendInvitePlayerToUnion(req);
    }
    /// <summary>
    /// 邀请军团回复 我邀请别人
    /// </summary>
    /// <param name="resp"></param>
    public void OnReceiveInvitePlayerToUnion(InvitePlayerJoinUnionResp resp)
    {
        //玩家不在线发送失败
        //离开24小时
        //发送成功
        if (resp.result == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.UNION_INVITE_SENDINVITE, resp.charname));
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    private void AgreeInviteToUnion()
    {
        //调用原来申请军团的接口，设置参数强制加入
        JoinUnionReq req = new JoinUnionReq();
        req.union_id = _curUnionInvite.union_id;
        req.is_invite = 1;
        _network.OnSendJoinUnion(req);
    }

    private void GetNextInviteToUnion()
    {
        //取下一个邀请
        if (_waitingUnionInvites.Count > 0)
        {
            _curUnionInvite = _waitingUnionInvites[0];
            _waitingUnionInvites.RemoveAt(0);
            ShowInvitedTip();
        }
        else
        {
            _curUnionInvite = null;
        }
    }

    private void ShowInvitedTip()
    {
        if (_curUnionInvite != null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,
               string.Format(ConstString.UNION_TIP_INVITE, _curUnionInvite.charname, _curUnionInvite.union_name), AgreeInviteToUnion
               , GetNextInviteToUnion, ConstString.BTN_TIP_AGREE, ConstString.BTN_TIP_REFUSE);
        }

    }
    #endregion

    #endregion

    #region 推送


    public void UnionInfoNotify()
    {

    }

    private List<NotifyPlayerInvitedToUnion> _waitingUnionInvites = new List<NotifyPlayerInvitedToUnion>();
    private NotifyPlayerInvitedToUnion _curUnionInvite;
    private string _joinOrKickTip = ""; //加入或者被T提示



    public void NotifyBeInvitedToUnion(NotifyPlayerInvitedToUnion resp)
    {
        if (UISystem.Instance.HintView.StartFightLoading
            || !UISystem.Instance.UIIsOpen(MainCityView.UIName))
        {
            _waitingUnionInvites.Add(resp);
        }
        else
        {
            if (_curUnionInvite == null)
            {
                _curUnionInvite = resp;
                ShowInvitedTip();
            }
            else
            {
                _waitingUnionInvites.Add(resp);
            }
        }
    }

    public void NotifyPlayerJoinUnion(NotifyPlayerJoinUnion resp)
    {
        //加入军团成功，可清空军团邀请提示

        if (UISystem.Instance.UIIsOpen(JoinUnionView.UIName))
        {
            QuickGoToUnion(resp.union_name, false);
        }
        else if (UISystem.Instance.UIIsOpen(MainCityView.UIName))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.JOIN_UNION_SUCCEED, resp.union_name));
        }
        else//战斗状态 存起来，离开战斗场景在提示
        {
            _joinOrKickTip = string.Format(ConstString.JOIN_UNION_SUCCEED, resp.union_name);
        }

        OnSendPlayerUnion();
    }

    public void NotifyPlayerKickOutUnion(NotifyPlayerKickOutUnion resp)
    {
        if (UISystem.Instance.UIIsOpen(UnionView.UIName))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.UNION_TIP_KICK,
            () =>
            {
                BackToMainCity();
            });
            return;
        }

        if (UISystem.Instance.UIIsOpen(MainCityView.UIName))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_TIP_KICK);
        }
        else//战斗状态 存起来，离开战斗场景在提示
        {
            _joinOrKickTip = ConstString.UNION_TIP_KICK;
        }

        _unionInfo.base_info.id = 0;
    }

    private void BackToMainCity()
    {
        UISystem.Instance.CloseAllUI();
        UISystem.Instance.ShowGameUI(MainCityView.UIName);
    }

    #endregion

    #region 消息

    public void OnSendManageUnionMember(ManageUnionType type, uint charid)
    {
        ManageUnionMemberReq req = new ManageUnionMemberReq();
        req.type = type;
        req.charid = charid;
        _network.OnSendManageUnionMember(req);
        Debug.Log("ManageUnionType " + type.ToString());

    }

    public void OnReceiveManageUnionMember(ManageUnionMemberResp resp)
    {
        Debug.Log("ManageUnionMemberResp " + resp.result.ToString());
        UISystem.Instance.UnionHallView.ButtonEvent_CloseSecondMenu(null);
        if (resp.result == (int)ErrorCodeEnum.PlayerHaveLeaveUnion)
        {
            ErrorCode.ShowErrorTip(resp.result);
            DelOneUnionMember(resp.other_charid);
        }
        else if (resp.result == 0)
        {
            if (resp.type == ManageUnionType.MUT_KICK)
            {
                DelOneUnionMember(resp.other_charid);
            }
            else if (resp.type == ManageUnionType.MUT_DEMOTION || resp.type == ManageUnionType.MUT_PROMOTION)
            {
                GetUnionMember(resp.other_charid).position = resp.other_position;
                UISystem.Instance.UnionHallView.RefreashItem(resp.other_charid);
            }
            else if (resp.type == ManageUnionType.MUT_ABDICATE)
            {
                GetUnionMember(resp.other_charid).position = resp.other_position;
                GetUnionMember(resp.self_charid).position = resp.self_position;
                UISystem.Instance.UnionHallView.ShowPersonalInfo();
                UISystem.Instance.UnionHallView.RefreashItemAll();

            }
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendUpdateUnionSetting(UnionLimitType type, int level, string desc)
    {
        UpdateUnionSettingReq req = new UpdateUnionSettingReq();
        req.limit_type = type;
        req.limit_level = level;
        req.desc = desc;
        _network.OnSendUpdateUnionSetting(req);
    }

    public void OnReceiveUpdateUnionSetting(UpdateUnionSettingResp resp)
    {
        if (resp.result == 0)
        {
            UnionInfo.desc = resp.desc;
            UnionInfo.base_info.limit_type = resp.limit_type;
            //Debug.LogError(UnionInfo.base_info.limit_type);
            UnionInfo.base_info.limit_level = resp.limit_level;
            UISystem.Instance.CloseGameUI(UnionSettingView.UIName);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHANGENAME_SUCCESS);
            if (UISystem.Instance.UIIsOpen(UnionView.UIName))
            {
                UISystem.Instance.UnionView.InitUI();
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    private OpenViewTypeRefreash _openView = OpenViewTypeRefreash.None;
    public void OpenUnionHall()
    {
        _openView = OpenViewTypeRefreash.Hall;
        OnSendPlayerUnion(true);
    }

    public void OpenUnionDonation()
    {
        _openView = OpenViewTypeRefreash.Donate;
        OnSendPlayerUnion(true);
    }

    public void OnSendPlayerUnion(bool showLoading = false)
    {
        PlayerUnionReq req = new PlayerUnionReq();
        _network.OnSendPlayerUnion(req, showLoading);
    }

    public void OnReceivePlayerUnion(PlayerUnionResp resp)
    {
        if (resp.result == 0)
            UnionInfo = resp.union;

        if (_openView == OpenViewTypeRefreash.Hall)
        {
            UISystem.Instance.ShowGameUI(UnionHallView.UIName);
            UISystem.Instance.UnionView.InitUI();
        }

        if (_openView == OpenViewTypeRefreash.Donate)
        {
            UISystem.Instance.ShowGameUI(UnionDonationView.UIName);
            UISystem.Instance.UnionView.InitUI();
        }
        _openView = OpenViewTypeRefreash.None;
    }

    public void OnSendAuditUnionMembers(UnionAuditType type, UnionAuditPassType passType, uint charid)
    {
        if (type == UnionAuditType.Multi)
        {
            AuditMultiUnionMemberReq req = new AuditMultiUnionMemberReq();
            req.is_passed = (int)passType;
            _network.OnSendAuditMultiUnionMember(req);
        }
        else
        {
            AuditSingleUnionMemberReq req = new AuditSingleUnionMemberReq();
            req.is_passed = (int)passType;
            req.charid = charid;
            _network.OnSendAuditSingleUnionMember(req);
        }
    }

    public void OnReceiveAuditSingleUnionMember(AuditSingleUnionMemberResp resp)
    {
        if (resp.result == 0 || resp.result == (int)ErrorCodeEnum.InformationExpired)
        {
            if (resp.result == (int)ErrorCodeEnum.InformationExpired)
            {
                ErrorCode.ShowErrorTip(resp.result);
            }
            else if (resp.is_passed == (int)UnionAuditPassType.Pass)
            {
                UnionInfo.members.Add(resp.member);
                UpdateMemberNum();
                UISystem.Instance.UnionHallView.ShowMembersOriginal();
            }
            int index = -1;
            for (int i = 0; i < UnionInfo.pending_members.Count; i++)
            {
                if (UnionInfo.pending_members[i].member.charid == resp.charid)
                {
                    UnionInfo.pending_members.RemoveAt(i);
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                UISystem.Instance.UnionApplyView.DelOne(index);
            }
            if (UnionInfo.pending_members.Count <= 0)
            {
                UISystem.Instance.CloseGameUI(UnionApplyView.UIName);
            }
            UISystem.Instance.UnionHallView.SetNotice();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnReceiveAuditMultiUnionMember(AuditMultiUnionMemberResp resp)
    {
        if (resp.result == 0)
        {
            int success = resp.success_charids == null ? 0 : resp.success_charids.Count;
            int failed = resp.failed_charids == null ? 0 : resp.failed_charids.Count;
            int remain = resp.remain_charids == null ? 0 : resp.remain_charids.Count;
            for (int i = 0; i < resp.members.Count; i++)
            {
                if (resp.members[i].charid == MyUnionMemberInfo.charid)
                {
                    resp.members.RemoveAt(i);
                }
            }
            UnionInfo.members.Clear();
            UnionInfo.members.AddRange(resp.members);
            UpdateMemberNum();

            if (remain > 0)
            {
                for (int i = UnionInfo.pending_members.Count - 1; i > -1; i--)
                {
                    if (!resp.remain_charids.Contains(UnionInfo.pending_members[i].member.charid))
                    {
                        UnionInfo.pending_members.RemoveAt(i);
                    }
                }
            }
            else
            {
                UnionInfo.pending_members.Clear();
            }

            UISystem.Instance.UnionHallView.ShowMembersOriginal();
            UISystem.Instance.CloseGameUI(UnionApplyView.UIName);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,
                string.Format(ConstString.FORMAT_UNION_MULTI_AUDIT_RESULT, success + failed + remain, success, failed, remain));
            UISystem.Instance.UnionHallView.SetNotice();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }

    }

    public void OnSendDonateToUnion(UnionDonateType type)
    {
        DonateToUnionReq req = new DonateToUnionReq();
        req.type = type;
        _network.OnSendDonateToUnion(req);
    }

    public void OnReceiveDonateToUnion(DonateToUnionResp resp)
    {
        if (resp.result == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.UNION_DONATE_SUCCEED, resp.token - PlayerData.Instance.UnionToken));
            PlayerData.Instance.UpdateUnionToken(resp.token);
            UnionInfo.resource = resp.union_res;
            UnionInfo.donate_logs.Clear();
            UnionInfo.donate_logs.AddRange(resp.donate_logs);
            if (resp.money_type == (int)ECurrencyType.Gold)
            {
                PlayerData.Instance.UpdateGold(resp.money_value);
                CharUnionInfo.gold_donate.times = resp.donate_times;
                UISystem.Instance.UnionDonationView.ShowEffect(true);
            }
            else if (resp.money_type == (int)ECurrencyType.Diamond)
            {
                PlayerData.Instance.UpdateDiamond(resp.money_value);
                CharUnionInfo.diamond_donate.times = resp.donate_times;
                UISystem.Instance.UnionDonationView.ShowEffect(false);

            }
            UISystem.Instance.UnionDonationView.RefreshDonateTimes();
            UISystem.Instance.UnionView.InitUI();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendUpdateUnionName(string name)
    {
        UpdateUnionNameReq req = new UpdateUnionNameReq();
        req.name = name;
        _network.OnSendUpdateUnionName(req);
    }

    public void OnReceiveUpdateUnionName(UpdateUnionNameResp resp)
    {
        if (resp.result == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHANGENAME_SUCCESS);
            UnionInfo.base_info.name = resp.name;
            PlayerData.Instance.UpdateDiamond(resp.diamond);
            UISystem.Instance.UnionView.InitUI();
            UISystem.Instance.UnionSettingView.InitUI();
            UISystem.Instance.CloseGameUI(ChangeUnionNameView.UIName);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendUpdateUnionIcon(string icon)
    {
        UpdateUnionIconReq req = new UpdateUnionIconReq();
        req.icon = icon;
        _network.OnSendUpdateUnionIcon(req);
    }

    public void OnReceiveUpdateUnionIcon(UpdateUnionIconResp resp)
    {
        if (resp.result == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHANGENAME_SUCCESS);
            Debug.Log("resp.icon " + resp.icon + " " + resp.diamond);
            UnionInfo.base_info.icon = resp.icon;
            PlayerData.Instance.UpdateDiamond(resp.diamond);
            UISystem.Instance.UnionView.InitUI();
            UISystem.Instance.UnionSettingView.InitUI();
            UISystem.Instance.CloseGameUI(ChangeUnionIconView.UIName);
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CHANGEUNIONBADGEVIEW);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendLevelUpUnion()
    {
        _network.OnSendLevelUpUnion(new LevelUpUnionReq());
    }

    public void OnReceiveLevelUpUnion(LevelUpUnionResp resp)
    {
        if (resp.result == 0)
        {
            UISystem.Instance.UnionView.PlayLVUPEffect();
            Debug.Log(resp.level);
            UnionInfo.base_info.level = resp.level;
            UnionInfo.resource = resp.resource;
            UISystem.Instance.UnionView.InitUI();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendVisitUnionMember(uint charid)
    {
        VisitUnionMemberReq req = new VisitUnionMemberReq();
        req.charid = charid;
        _network.OnSendVisitUnionMember(req);
    }

    public void OnReceiveVisitUnionMember(VisitUnionMemberResp resp)
    {
        Debug.Log("VisitUnionMemberResp " + resp.result + " " + resp.charid);
        if (resp.result == (int)ErrorCodeEnum.PlayerHaveLeaveUnion)
        {
            ErrorCode.ShowErrorTip(resp.result);
            DelOneUnionMember(resp.charid);
        }
        else if (resp.result == 0)
        {
            _myUnionMemberInfo.interviewees.Add(resp.charid);
            UnionMember member = UISystem.Instance.UnionHallView.RefreashItem(resp.charid);
            UISystem.Instance.UnionHallView.ShowHpRecord();
            bool visitBack = false;
            for (int i = 0; i < _myUnionMemberInfo.visitors.Count; i++)
            {
                if(_myUnionMemberInfo.visitors[i].charid == resp.charid)
                {
                    visitBack = true;
                }
            }
            string tip = "";
            if (member != null && visitBack)
            {
                tip = string.Format(ConstString.UNION_TIP_VISITBACK_SUCCEED_TO_NAME, member.charname,
                    ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.UNION_SEND_HP_COUNT));
            }
            else if (member != null && !visitBack)
	        {
                tip = string.Format(ConstString.UNION_TIP_VISIT_SUCCEED_TO_NAME, member.charname,
                    ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.UNION_SEND_HP_COUNT));
            }
            else
            {
                tip = ConstString.UNION_TIP_VISIT_SUCCEED;
            }
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, tip);

        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendLeaveUnion()
    {
        LeaveUnionReq req = new LeaveUnionReq();
        _network.OnSendLeaveUnion(req);
    }

    public void OnReceiveLeaveUnion(LeaveUnionResp resp)
    {
        if (resp.result == 0)
        {
            //TODO:  清空军团数据
            _unionInfo.base_info.id = 0;
            UISystem.Instance.CloseGameUI(UnionView.UIName);
            UISystem.Instance.CloseGameUI(UnionHallView.UIName);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_QUIT_SUCCEED);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendGetUnionMemberHp(uint charid)
    {
        GetUnionHpReq req = new GetUnionHpReq();
        //1表示批量领取, 2表示领取单个， 
        req.type = (int)UnionGetHpType.Single;
        req.charid = charid;
        OnSendGetUnionHp(req);
    }

    public void OnSendOneKeyGetUnionHp()
    {
        GetUnionHpReq req = new GetUnionHpReq();
        //1表示批量领取, 2表示领取单个， 
        req.type = (int)UnionGetHpType.Multi;
        OnSendGetUnionHp(req);
    }

    private void OnSendGetUnionHp(GetUnionHpReq req)
    {
        _network.OnSendGetUnionHp(req);
    }

    public void OnReceiveGetUnionHp(GetUnionHpResp resp)
    {
        if (resp.result == 0 || resp.result == (int)ErrorCodeEnum.OverMaxPHPower || resp.result == (int)ErrorCodeEnum.PlayerHaveLeaveUnion)
        {
            if (resp.result == (int)ErrorCodeEnum.OverMaxPHPower)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                                                          (resp.charids == null || resp.charids.Count == 0)
                                                              ? ConstString.UNION_GET_HP_FAILED
                                                              : ConstString.UNION_GET_HP_PART);
            }
            else if (resp.result == (int)ErrorCodeEnum.PlayerHaveLeaveUnion)
                ErrorCode.ShowErrorTip(resp.result);
            else
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_GET_HP_SUCCEED);

            PlayerData.Instance.UpdateSP(resp.hp_power);
            CharUnionInfo.get_union_hp_times = resp.hp_times;
            UISystem.Instance.UnionHallView.ShowHpRecord();

            if (resp.charids != null && resp.charids.Count > 0)
            {
                for (int i = 0; i < _myUnionMemberInfo.visitors.Count; i++)
                {
                    if (resp.charids.Contains(_myUnionMemberInfo.visitors[i].charid))
                    {
                        _myUnionMemberInfo.visitors[i].state = (int)UnionGetHpState.Get;
                        UISystem.Instance.UnionHallView.RefreashItem(_myUnionMemberInfo.visitors[i].charid);
                    }
                }
            }
            if (resp.removed_charids != null)
            {
                for (int i = 0; i < resp.removed_charids.Count; i++)
                {
                    DelOneUnionMember(resp.removed_charids[i]);
                }
            }

        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendJoinUnion(int id)
    {
        JoinUnionReq req = new JoinUnionReq();
        req.union_id = id;
        _network.OnSendJoinUnion(req);
    }

    public void OnReceiveJoinUnion(JoinUnionResp resp)
    {

        if (resp.result == 0)
        {
            UnionInfo = resp.union;
            QuickGoToUnion(UnionInfo.base_info.name, true);
        }
        else if (resp.result == (int)ErrorCodeEnum.PendingUnionApplication)
        {
            ErrorCode.ShowErrorTip(resp.result);
            //服务端每个玩家的申请数据三天才清除，所以这里要刷新 不能直接ADD
            if (CharUnionInfo.apply_unions != null)
            {
                bool found = false;
                int count = CharUnionInfo.apply_unions.Count;
                for (int i = 0; i < count; i++)
                {
                    ApplyUnion info = CharUnionInfo.apply_unions[i];
                    if (info.union_id == resp.apply_union.union_id)
                    {
                        CharUnionInfo.apply_unions.RemoveAt(i);
                        CharUnionInfo.apply_unions.Insert(i, resp.apply_union);
                    }
                }
                if (!found)
                    CharUnionInfo.apply_unions.Add(resp.apply_union);
            }
            CharUnionInfo.apply_times++;
            UISystem.Instance.JoinUnionView.SetCharJoinInfo();
            UISystem.Instance.JoinUnionView.RefreashItem(resp.apply_union.union_id);
        }
        else if (resp.result == (int)ErrorCodeEnum.LeaveUnionInCD)
        {
            uint timer = ConfigManager.Instance.mUnionConfig.GetUnionBaseData().LeaveUnionCD;
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.ERR_EXIT_UNION_TIMER, timer / 3600));
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
            GetNextInviteToUnion();
        }
    }



    public void OnSendCreateUnion(Union union)
    {
        CreateUnionReq req = new CreateUnionReq();
        req.union = union;
        _network.OnSendCreateUnion(req);
    }

    public void OnReceiveCreateUnion(CreateUnionResp resp)
    {
        if (resp.result == 0)
        {
            UnionInfo = resp.union;
            PlayerData.Instance.UpdateDiamond(resp.diamond);
            UISystem.Instance.CloseGameUI(CreateUnionView.UIName);
            UISystem.Instance.CloseGameUI(JoinUnionView.UIName);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.CREATE_UNION_SUCCEED, OnSendOpenUnion, null, ConstString.CREATE_UNION_SUCCEED_GO);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendUnionPage(int page)
    {
        //优化：军团信息请求完毕之后不要再发送数据
        if (UnionList != null && UnionList.Count >= _maxUnionNum)
        {
            //Debug.Log("_maxUnionNum = " + _maxUnionNum + " UnionList.Count = " + UnionList.Count);
            return;
        }
        OpenUnionReq req = new OpenUnionReq();
        req.page = page;
        _network.OnSendUnionPage(req);
    }

    private void OnSendOpenUnion()
    {
        if (UnionList != null)
            UnionList.Clear();

        OpenUnionReq req = new OpenUnionReq();
        req.page = 1;
        _network.OnSendUnionPage(req);
    }

    public void OnReceiveOpenUnion(OpenUnionResp resp)
    {
        //为0 表示未加入军团，显示军团列表，为1表示军团信息， 其他未错误码 
        if (resp.result == 0)
        {
            _maxUnionNum = resp.max_num;
            if (UnionList == null) UnionList = new List<BaseUnion>();
            UnionList.AddRange(resp.union_list);
            CharUnionInfo = resp.union_info;
            if (!UISystem.Instance.UIIsOpen(JoinUnionView.UIName))
                UISystem.Instance.ShowGameUI(JoinUnionView.UIName);
            UISystem.Instance.JoinUnionView.Page = resp.page;
            UISystem.Instance.JoinUnionView.ShowUnionList();
            if (_isOpenPve)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_TIP_TO_JOIN);
                _isOpenPve = false;
            }
        }
        else if (resp.result == 1)
        {
            UnionInfo = resp.union;
            UnionFightRank = resp.union_clash_rank;
            _unionPvpState = resp.union_clash_state;
            this.UnionPrisonBuffNum = resp.all_up_probability;
            UISystem.Instance.ShowGameUI(UnionView.UIName);
            if (_isOpenPve)
            {
                OnSendOpenUnionPveDgn();
                _isOpenPve = false;
            }
            else
            {
                if(resp.up_id == 0 ||resp.down_id == 0 || string.IsNullOrEmpty(resp.up_name) || string.IsNullOrEmpty(resp.down_name))
                {
                    return;
                }
                string content = "";
                if (resp.up_id == PlayerData.Instance._AccountID)
                {
                    content = string.Format(ConstString.TIP_UNION_CHARMAN_OFF_FOR_OTHER,
                        resp.down_name, ConfigManager.Instance.mUnionConfig.GetUnionBaseData().RemoveChairmanCD / 86400,
                        ConfigManager.Instance.mUnionConfig.GetUnionBaseData().ChooseChairmanTime / 3600, ConstString.YOU);
                }
                else if (resp.down_id == PlayerData.Instance._AccountID)
                {
                    content = string.Format(ConstString.TIP_UNION_CHARMAN_OFF_SELF,
                        ConfigManager.Instance.mUnionConfig.GetUnionBaseData().RemoveChairmanCD / 86400,
                        ConfigManager.Instance.mUnionConfig.GetUnionBaseData().ChooseChairmanTime / 3600, resp.up_name);
                }
                else
                {
                    content = string.Format(ConstString.TIP_UNION_CHARMAN_OFF_FOR_OTHER,
                        resp.down_name, ConfigManager.Instance.mUnionConfig.GetUnionBaseData().RemoveChairmanCD / 86400,
                        ConfigManager.Instance.mUnionConfig.GetUnionBaseData().ChooseChairmanTime / 3600, resp.up_name);
                }
                UISystem.Instance.ShowGameUI(AdvanceTipView.UIName);
                UISystem.Instance.AdvanceTipView.SetContent(content);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendSearchUnion(int id)
    {
        QueryUnionReq req = new QueryUnionReq();
        req.union_id = id;
        _network.OnSendUnionPage(req);
    }

    public void OnReceiveSearchUnion(QueryUnionResp resp)
    {
        if (resp.result == 0)
        {
            UISystem.Instance.JoinUnionView.ShowSearchUnion(resp.base_union);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }



    /// <summary>
    /// 军团图标查询
    /// </summary>
    /// <param name="req"></param>
    public void OnSendQueryUnionIcon()
    {
        //OnReceiveQueryUnionIcon(null);
        //return;
        QueryUnionIconReq tmpData = new QueryUnionIconReq();
        _network.OnSendQueryUnionIcon(tmpData);
    }
    public void OnReceiveQueryUnionIcon(QueryUnionIconResp vData)
    {
        if ((vData != null) && (vData.result == (int)ErrorCodeEnum.SUCCESS))
        {
            if (vData.icon_time_list != null)
            {
                if (DicLimitBadge == null)
                {
                    DicLimitBadge = new Dictionary<uint, uint>();
                }
                else
                {
                    DicLimitBadge.Clear();
                }
                uint tmpKey = 0;
                foreach (iconValidTime tmpSingleData in vData.icon_time_list)
                {
                    if (tmpSingleData != null)
                    {
                        if (uint.TryParse(tmpSingleData.icon, out tmpKey))
                        {
                            if (!DicLimitBadge.ContainsKey(tmpKey))
                            {
                                DicLimitBadge.Add(tmpKey, tmpSingleData.valid_time);
                            }
                        }
                    }
                }
            }
        }
        //打开徽章选择界面//
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHANGEUNIONBADGEVIEW);
    }
    /// <summary>
    /// 刷新军团图标信息[限时徽章时间到期]
    /// </summary>
    public void RefreshBadgeInfo()
    {
        if ((UnionInfo != null) && (UnionInfo.base_info != null))
        {
            uint tmpPreID = 0;
            uint.TryParse(UnionInfo.base_info.icon, out tmpPreID);
            UnionInfo.base_info.icon = ConfigManager.Instance.mUnionConfig.GetMinUnionIconData().mID.ToString();
            if (DicLimitBadge != null)
            {
                if (DicLimitBadge.ContainsKey(tmpPreID))
                {
                    DicLimitBadge.Remove(tmpPreID);
                }
            }
        }
    }


    #region 异域探险

    public void OnSendOpenUnionPveDgn()
    {
        OpenUnionPveDgnReq data = new OpenUnionPveDgnReq();
        _network.OnSendOpenUnionPveDgn(data);
    }

    public void OnReceiveOpenUnionPveDgn(OpenUnionPveDgnResp resp)
    {
        if (resp.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UnionInfo.dgn_info.Clear();
            UnionInfo.dgn_info.AddRange(resp.dgn_info);
            CharUnionInfo.today_pve_times = resp.today_pve_times;
            CharUnionInfo.today_pve_tick = resp.today_pve_tick;
            CharUnionInfo.today_rewards_tick = resp.today_rewards_tick;
            CharUnionInfo.get_today_rewards = resp.get_today_rewards;
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXOTICADVANTUREVIEW);
            UISystem.Instance.ExoticAdvantureView.UpdateViewInfo();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendResetUnionPveDgn()
    {
        ResetUnionPveDgnReq data = new ResetUnionPveDgnReq();
        _network.OnSendResetUnionPveDgn(data);
    }

    public void OnReceiveResetUnionPveDgn(ResetUnionPveDgnResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UnionInfo.dgn_info.Clear();
            UnionInfo.resource = data.resource;
            UISystem.Instance.UnionView.InitUI();
            UISystem.Instance.ExoticAdvantureView.OnResetUnionPveDgnResp();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendStartUnionPveDgn(uint dgn_id, List<SoldierList> soldiers, List<EquipList> equips)
    {
        if (isFightState)
            return;
        isFightState = true;
        StartUnionPveDgnReq data = new StartUnionPveDgnReq();
        data.dgn_id = dgn_id;
        data.soldiers.Clear();
        data.soldiers.AddRange(soldiers);
        data.attack_equips.Clear();
        data.attack_equips.AddRange(equips);
        _network.OnSendStartUnionPveDgn(data);
    }

    public void OnReceiveStartUnionPvpDgn(StartUnionPveDgnResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            UnionModule.Instance.CharUnionInfo.today_pve_times = data.today_times;
            UISystem.Instance.PrepareBattleView.OnStartUnionPvpDgnResp(data.hero_attr);
        }
        else
        {
            isFightState = false;
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendUnionPveDgnTodayRward()
    {
        UnionPveDgnTodayRewardReq data = new UnionPveDgnTodayRewardReq();
        _network.OnSendUnionPveDgnTodayRward(data);
    }

    public void OnReceiveUnionPveDgnTodayReward(UnionPveDgnTodayRewardResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDropData(data.drop_items);
            UnionModule.Instance.CharUnionInfo.get_today_rewards = 1;
            UISystem.Instance.ExoticAdvantureView.OnUnionPveDgnTodayRewardResp(data.drop_items);
        }
        else if (data.result == (int)ErrorCodeEnum.CannotGetRewards)
        {
            UISystem.Instance.ExoticAdvantureView.OnUnionPveDgnTodayRewardResp(null, true);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendQueryUnionPveDgnState(uint dgnID)
    {
        QueryUnionPveDgnStateReq data = new QueryUnionPveDgnStateReq();
        data.dgn_id = dgnID;
        _network.OnSendQueryUnionPveDgnState(data);
    }

    public void OnReceiveQueryUnionPveDgnState(QueryUnionPveDgnStateResp data)
    {
        Debug.Log("QueryUnionPveDgnStateResp");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UpdatePVEDgnInfo(data.dgn_id, data.total_hurt);
            UISystem.Instance.ExoticAdvantureView.OnQueryUnionPveDgnState(false, data.dgn_id, data.challenger);
        }
        else if (data.result == (int)ErrorCodeEnum.UnionDgnIsChallenged)
        {
            UpdatePVEDgnInfo(data.dgn_id, data.total_hurt);
            UISystem.Instance.ExoticAdvantureView.OnQueryUnionPveDgnState(true, data.dgn_id, data.challenger);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendReadyUnionPveDgn(uint dgn_id)
    {
        ReadyUnionPveDgnReq data = new ReadyUnionPveDgnReq();
        data.dgn_id = dgn_id;
        _network.OnSendReadyUnionPveDgn(data);
    }

    public void OnReceiveReadyUnionPveDgn(ReadyUnionPveDgnResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS || data.result == (int)ErrorCodeEnum.UnionDgnIsChallenged)
        {
            UpdatePVEDgnInfo(data.dgn_id, data.total_hurt);
            UISystem.Instance.ExoticAdvantureInfoView.OnReadyUnionPveDgnResp(data);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }


    public void OnSendCancelReadyUnionPve()
    {
        CancelReadyUnionPveReq data = new CancelReadyUnionPveReq();
        _network.OnSendCancelReadyUnionPve(data);
    }
    public void OnReceiveCancelReadyUnionPve(CancelReadyUnionPveResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.PrepareBattleView.OnCancelReadyUnionPve();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
        //不用做任何处理
    }

    #endregion

    #region UnionRank

    public void OnSendUnionPveDgnRank()
    {
        UnionPveDgnRankReq data = new UnionPveDgnRankReq();
        _network.OnSendUnionPveDgnRank(data);
    }

    public void OnReceiveUnionPveDgnRank(UnionPveDgnRankResp response)
    {
        if (response.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONRANKVIEW);
            UISystem.Instance.UnionRankView.OnShowUnionRank(UnionRankType.Damage);
            UISystem.Instance.UnionRankView.OnUpdateDamageRank(response.rank_list);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    public void OnSendUnionPvpRank()
    {
        UnionPvpRankReq req = new UnionPvpRankReq();
        _network.OnSendUnionPvpRankReq(req);
    }

    public void OnReceiveUnionPvpRank(UnionPvpRankResp response)
    {
        if (response.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONRANKVIEW);
            UISystem.Instance.UnionRankView.OnShowUnionRank(UnionRankType.Hegemony);
            UISystem.Instance.UnionRankView.OnUpdateHegemonyRank(response,response.rank_list);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }

    }

    public void OnSendUnionPvpKillRank()
    {
        UnionPvpKillRankReq req = new UnionPvpKillRankReq();
        _network.OnSendUnionPvpKillRankReq(req);
    }

    public void OnReceiveUnionPvpKillRank(UnionPvpKillRankResp response)
    {
        if (response.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONRANKVIEW);
            UISystem.Instance.UnionRankView.OnShowUnionRank(UnionRankType.Kill);
            UISystem.Instance.UnionRankView.OnUpdateKillRank(response.rank_list);
        }
        else
        {
            ErrorCode.ShowErrorTip(response.result);
        }
    }

    #endregion

    #region 军团争霸

    public void OnSendApplyUnionPVP()
    {
        ApplyForUnionPvpReq req = new ApplyForUnionPvpReq();
        _network.OnSendApplyUnionPVP(req);
    }

    public void OnReceiveApplyUnionPVP(ApplyForUnionPvpResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            UnionModule.Instance.OnSendOpenUnionPVP();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendOpenUnionPVP()
    {
        OpenUnionPvpReq req = new OpenUnionPvpReq();
        _network.OnSendOpenUnionPVP(req);
    }

    public void OnReceiveOpenUnionPVP(OpenUnionPvpResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            Debug.LogWarning("OnReceiveOpenUnionPVP ...... city = " + data.city + " data.game_index = " + data.game_index);
            UnionPvpState = data.state;
            _fightBeginTime = data.start_tick;
            CharUnionInfo.pvp_city = data.city;
            _enemyUnion = CommonFunction.CopyBaseUnion(data.enemy_union);
            UISystem.Instance.GuildHegemonyView.UpdateHegemonyPn(data);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendJoinUnionPVP(int city, List<SoldierList> soldiers, List<EquipList> equips)
    {
        JoinUnionPvpReq req = new JoinUnionPvpReq();
        req.city = city;
        req.attack_soldiers.AddRange(soldiers);
        req.attack_equips.AddRange(equips);
        _network.OnSendJoinUnionPVP(req);
    }

    public void OnReceiveJoinUnionPVP(JoinUnionPvpResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            CharUnionInfo.pvp_city = data.city;
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
            UISystem.Instance.UnionReadinessView.OnUpdateView(_currentCity, data.self_array);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendCancelUnionPVP()
    {
        CancelUnionPvpReq req = new CancelUnionPvpReq();
        _network.OnSendCancelUnionPVP(req);
    }

    public void OnReceiveCancelUnionPVP(CancelUnionPvpResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            CharUnionInfo.pvp_city = 0;
            _myBattleInfo = null;
            UISystem.Instance.UnionReadinessView.OnUpdateView(_currentCity, data.self_array);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void OnSendOpenUnionCity(int city)
    {
        OpenUnionCityReq req = new OpenUnionCityReq();
        req.city = city;
        _currentCity = city;
        _network.OnSendOpenUnionCity(req);

    }

    public void OnReceiveOpenUnionCity(OpenUnionCityResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            switch (data.state)
            {
                case UnionPvpState.UPS_READY:
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONREADINESSVIEW);
                    UISystem.Instance.UnionReadinessView.OnUpdateView(_currentCity, data.self_array);
                    break;
                case UnionPvpState.UPS_FIGHTING:
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONHEGEMONYVIEW);
                    UISystem.Instance.UnionHegemonyView.OnShowHegemonyBattle(_currentCity, UnionInfo.base_info, _enemyUnion, data);
                    break;
                default:
                    break;
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    #endregion

    #endregion
}
