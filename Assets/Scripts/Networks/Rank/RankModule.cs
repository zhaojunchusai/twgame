using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class RankModule : Singleton<RankModule>
{
    private RankNetwork _netWork;
    //查看玩家信息
    private RankInfo _tempInfo;
    private PlayerInfoTypeEnum _infoType;
    public bool NeedRefreshRank = false;
    //缓存的数据
    private Dictionary<RankType, RankInfoResp> _cachedRankInfo;
    private List<string> _refreshTimeList;
    public string refreshTimeStr = "";
    public string refreshTimeStr_campaign = "";
    /// <summary>
    /// 排行榜信息请求 这个请求不一定会发送出去 有可能直接取缓存的数据
    /// </summary>
    /// <param name="type"></param>
    public void SendRankInfoRequsest(RankType type)
    {

        if (type == RankType.ARENA_RANK||type==RankType.CROSSSERVERWAR_PERSONAL)
        {
            RankInfoReq req = new RankInfoReq();
            req.rank_type = (uint)type;
            _netWork.SendRankInfoRequsest(req);
        }

        if (NeedRefreshRank)
        {
            _cachedRankInfo.Clear();
            NeedRefreshRank = false;
            RankInfoReq req = new RankInfoReq();
            req.rank_type = (uint)type;
            _netWork.SendRankInfoRequsest(req);
        }
        else
        {
            RankInfoResp cachedResponse;
            _cachedRankInfo.TryGetValue(type, out cachedResponse);
            //已经缓存的直接显示
            if (cachedResponse != null)
            {
                UISystem.Instance.RankView.OnUpdateRankInfo(cachedResponse.rank_info);
            }
            //没有缓存的请求服务端
            else
            {
                RankInfoReq req = new RankInfoReq();
                req.rank_type = (uint)type;
                _netWork.SendRankInfoRequsest(req);
            }
        }
    }
    //排行榜信息回复
    public void OnRankInfoResponse(RankInfoResp response)
    {
        if (response.result != 0)
        {
            ErrorCode.ShowErrorTip(response.result);
        }
        else
        {
            switch ((RankType)response.rank_type)
            {
                case RankType.POLE_RANK:
                case RankType.ARENA_RANK:
                case RankType.CROSSSERVERWAR_PERSONAL:
                case RankType.CROSSSERVERWAR_UNION:
                    UISystem.Instance.RankView.OnUpdateRankInfo(response.rank_info);
                    break;
                case RankType.COMBAT_RANK:
                case RankType.ENDLESS_A:
                case RankType.ENDLESS_B:
                case RankType.ENDLESS_C:
                case RankType.LEVEL_RANK:
                    _cachedRankInfo.Add((RankType)response.rank_type, response);
                    if ((RankType)response.rank_type == RankType.LEVEL_RANK)
                    {
                        _refreshTimeList.Clear();
                        _refreshTimeList.AddRange(response.update_time);
                        refreshTimeStr = "";
                        int count = _refreshTimeList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (i < count - 1)
                            {
                                refreshTimeStr += _refreshTimeList[i].Substring(0, 2) + ConstString.RANK_LABEL_TIMESPLIT;
                            }
                            else
                                refreshTimeStr += _refreshTimeList[i].Substring(0, 2);
                        }
                    }
                    UISystem.Instance.RankView.OnUpdateRankInfo(response.rank_info);
                    break;
                case RankType.EXPLORE_RANK:
                case RankType.GRID_RANK:
                    {
                        _refreshTimeList.Clear();
                        _refreshTimeList.AddRange(response.update_time);
                        refreshTimeStr = "";
                        int count = _refreshTimeList.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (i < count - 1)
                            {
                                refreshTimeStr += _refreshTimeList[i].Substring(0, 2) + ConstString.RANK_LABEL_TIMESPLIT;
                            }
                            else
                                refreshTimeStr += _refreshTimeList[i].Substring(0, 2);
                        }
                        UISystem.Instance.RankView.OnUpdateRankInfo(response.rank_info);
                    }
                    break;
                case RankType.CAMPAIGN_UNION:
                case RankType.CAMPAIGN_PLAYER:
                case RankType.CAMPAIGN_ALL_PLAYER:
                case RankType.UNIONOVERLORD:
                    //_cachedRankInfo.Add((RankType)response.rank_type, response);
                    _refreshTimeList.Clear();
                    _refreshTimeList.AddRange(response.update_time);
                    refreshTimeStr_campaign = "";
                    int count1 = _refreshTimeList.Count;
                    for (int i = 0; i < count1; i++)
                    {
                        if (i < count1 - 1)
                        {
                            refreshTimeStr_campaign += _refreshTimeList[i].Substring(0, 5) + ConstString.RANK_LABEL_TIMESPLIT;
                        }
                        else
                            refreshTimeStr_campaign += _refreshTimeList[i].Substring(0, 5);
                    }
                    UISystem.Instance.RankView.OnUpdateRankInfo(response.rank_info);
                    break;
            }
        }
    }
    /// <summary>
    /// 点击玩家的具体信息请求 玩家具体信息：基础信息（等级 昵称 战力等）+ 演舞台防御阵容
    /// </summary>
    /// <param name="type"></param>
    /// <param name="info"></param>
    public void SendRankPalyerInfoRequset(RankType type, RankInfo info)
    {
        _infoType = CommonFunction.GetInfoTypeByRankType(type);
        RankPlayerInfoReq req = new RankPlayerInfoReq();
        req.rank_type = (uint)type;
        if ((type == RankType.UNIONOVERLORD) || (type == RankType.EXPLORE_RANK) || (type == RankType.GRID_RANK))
        {
            req.charid = info.union_info.union_id;
        }
        else
        {
            req.charid = info.charid;
        }
        req.area_id = info.area_id;
        _tempInfo = new RankInfo();
        CommonFunction.CopyRankInfo(info, _tempInfo);
        _netWork.SendRankPalyerInfoRequset(req);
    }
    /// <summary>
    /// 点击玩家的具体信息回复 服务端只发送了演舞台防御阵容 其他信息客户端已经有 所以自己构造一个数据
    /// </summary>
    /// <param name="response"></param>
    public void OnRankPlayerInfoResponse(RankPlayerInfoResp response)
    {
        if (response.result != 0)
        {
            ErrorCode.ShowErrorTip(response.result);
        }
        else
        {
            if ((response.rank_type == (uint)RankType.UNIONOVERLORD) || (response.rank_type == (uint)RankType.EXPLORE_RANK) || (response.rank_type == (uint)RankType.GRID_RANK))
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONMEMBERINFO);
                UISystem.Instance.UnionMemberInfoView.ShowView(response);
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONINFOVIEW);
            }
            else
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
                //自己构造数据给玩家信息界面
                ArenaPlayer player = CommonFunction.CreateArenaPlayerInfo(_tempInfo, response.soldiers);
                UISystem.Instance.PlayerInfoView.UpdateViewInfo(_infoType, player);
            }
        }
    }

    public void Initialize()
    {
        NeedRefreshRank = false;
        _cachedRankInfo = new Dictionary<RankType, RankInfoResp>();
        _refreshTimeList = new List<string>();
        refreshTimeStr = "";
        if (_netWork == null)
        {
            _netWork = new RankNetwork();
            _netWork.RegisterMsg();
        }
    }

    public void Uninitialize()
    {
        _netWork.RemoveMsg();
        _netWork = null;
        _cachedRankInfo.Clear();
        _cachedRankInfo = null;
        _refreshTimeList.Clear();
        _refreshTimeList = null;
        refreshTimeStr = "";
    }

    private bool CheckCachedComplete()
    {
        return _cachedRankInfo.ContainsKey(RankType.COMBAT_RANK)
            && _cachedRankInfo.ContainsKey(RankType.LEVEL_RANK)
            && _cachedRankInfo.ContainsKey(RankType.ENDLESS_A)
            && _cachedRankInfo.ContainsKey(RankType.ENDLESS_B)
            && _cachedRankInfo.ContainsKey(RankType.ENDLESS_C);
    }
}
