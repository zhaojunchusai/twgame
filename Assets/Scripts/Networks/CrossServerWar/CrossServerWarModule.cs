using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using System.Collections.Generic;
using System.Linq;

public class CrossServerWarModule : Singleton<CrossServerWarModule> {
    public CrossServerWarNetworK _crossServerWarNetwork;
    /// <summary>
    /// 纪录地图移动位置
    /// </summary>
    public Vector3 recordPos;

    public bool isBattleStart;
    public long time_Battle;
    public long time_CD;
    public uint unionGoods;
    public List<TerritoryPointRank> tileUnionRank;
    public List<TerritoryPointRank> tilePersonalRank;
    public uint game_times;
    public bool isCrossServer;
    public List<string> serverNameList;
    public int normal_tile_num;
    public int special_tile_num;
    public int fight_tile_num;
    public int normal_output;
    public uint buyCDTimes;

    public void Initialize()
    {
        if (_crossServerWarNetwork == null)
        {
            _crossServerWarNetwork = new CrossServerWarNetworK();
        }
        _crossServerWarNetwork.RegisterMsg();

        if (tileUnionRank == null)
            tileUnionRank = new List<TerritoryPointRank>();
        if (tilePersonalRank == null)
            tilePersonalRank = new List<TerritoryPointRank>();
        if (serverNameList == null)
            serverNameList = new List<string>();
    }
    public void Uninitialize()
    {
        if (_crossServerWarNetwork != null)
        {
            _crossServerWarNetwork.RemoveMsg();
            _crossServerWarNetwork = null;
        }
        if (tileUnionRank != null)
        {
            tileUnionRank.Clear();
            tileUnionRank = null;
        }
        if (tilePersonalRank != null)
        {
            tilePersonalRank.Clear();
            tilePersonalRank = null;
        }
        if (serverNameList != null)
        {
            serverNameList.Clear();
            serverNameList = null;
        }
    }
    //进入跨服战主界面信息请求及回复
    public void OnSendEnterCrossServerWar()
    {
        EnterCrossServerWarReq req = new EnterCrossServerWarReq();
        _crossServerWarNetwork.SendEnterCrossServerWar(req);
    }
    public void OnReceiveEnterCrossServerWar(EnterCrossServerWarResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                normal_tile_num = 0;
                special_tile_num = 0;
                fight_tile_num = 0;
                normal_output = 0;
                isBattleStart = data.game_state == 1 ? true : false;
                time_Battle = (long)data.time_battle;
                time_CD = (long)data.time_cd;
                unionGoods = data.union_goods;
                game_times = data.game_times;
                buyCDTimes = data.buy_times;
                List<uint> idList = new List<uint>();
                foreach(var tmp in data.tileinfo)
                {
                    if (UISystem.Instance.CrossServerWarView.tileDic.ContainsKey(tmp.tile_id))
                    {
                        UISystem.Instance.CrossServerWarView.tileDic[tmp.tile_id].SetTile(tmp.owner_typ, tmp.border_type, tmp.tile_union_name, tmp.turns,tmp.MaxScore,tmp.icon);
                        idList.Add(tmp.tile_id);
                    }
                    else
                        Debug.LogError("the tile id: " + tmp.tile_id + " is not exist");
                }
                foreach(TileInfo tile in UISystem.Instance.CrossServerWarView.tileDic.Values)
                {
                    if (idList.Contains(tile.Tile_ID))
                        continue;
                    tile.SetTile();
                }

            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }

        }
        else
        {
            Debug.LogError("EnterCrossServerWarResp is null");
        }
    }
    //对于地块上伤害排名的请求及回复
    public void OnSendTileRankReq(uint id)
    {
        TileRankReq req = new TileRankReq();
        req.tile_id = id;
        _crossServerWarNetwork.SendTileRankReq(req);
    }
    public void OnReceiveTileRankReq(TileRankResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                tileUnionRank.Clear();
                tileUnionRank = GetRankDic(data.tile_union_rank);
                tilePersonalRank.Clear();
                tilePersonalRank = GetRankDic(data.tile_personal_rank);
                UISystem.Instance.CrossServerWarView.ShowTileRank(tileUnionRank, true);
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
        else
        {
            Debug.LogError("TileRankResp is null");
        }
    }
    //进入跨服及奖励信息界面请求与回复
    public void OnSendEnterServerAndAwardReq()
    {
        EnterServerAndAwardReq req = new EnterServerAndAwardReq();
        _crossServerWarNetwork.SendEnterServerAndAwardReq(req);
    }
    public void OnReceiveEnterServerAndAwardResp(EnterServerAndAwardResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                isCrossServer = data.server_state == 1 ? true : false;
                serverNameList = data.server_name;
                serverNameList.Sort();
                UISystem.Instance.CrossServerWarView.GetServerToShowInfo();
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
        else
        {
            Debug.LogError("EnterServerAndAwardResp is null");
        }
    }
    /// <summary>
    /// 将服务器返回的字符串拆成需要的形式
    /// </summary>
    private List<TerritoryPointRank> GetRankDic(List<string> list)
    {
        List<TerritoryPointRank> tileRankList = new List<TerritoryPointRank>();

        if (list == null || list.Count < 0)
        {
            Debug.LogError(" input string list is null");
            return tileRankList;
        }
        foreach(string str in list)
        {
            string[] s = str.Split(':');
            TerritoryPointRank tileRank = new TerritoryPointRank(s[0], s[1]);
            tileRankList.Add(tileRank);
        }
        tileRankList = tileRankList.OrderByDescending(s => s.point).ToList();
        for(int i = 0; i < tileRankList.Count; i++)
        {
            tileRankList[i].order = i+1;
        }
        return tileRankList;
    }
    //耗费元宝清除CD
    public void OnSendBuyCDClear()
    {
        if (isBattleStart)
        {
            BuyCDClearReq req = new BuyCDClearReq();
            _crossServerWarNetwork.SendBuyCDClearReq(req);
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CROSSSERVERWAR_CANTBUYCD);          
        }
    }
    public void OnReceiveBuyCDClear(BuyCDClearResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                PlayerData.Instance.UpdateItem(data.buyItem);
                buyCDTimes = data.buy_times;
                time_CD = Main.mTime;
                UISystem.Instance.CrossServerWarView.ShowGateInfo();
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
                UISystem.Instance.CrossServerWarView.autoBuyCD = false;
            }
        }
        else
        {
            Debug.LogError("BuyCDClearResp is null");
        }
    }
    //发起挑战
    public void OnSendGotoBattle(List<SoldierList> _soldiers,List<EquipList> _equips)
    {
        GotoBattleReq req = new GotoBattleReq();
        req.tile_id = UISystem.Instance.CrossServerWarView.cur_tileID;
        req.soldiers.Clear();
        req.soldiers.AddRange(_soldiers);
        req.equips.Clear();
        req.equips.AddRange(_equips);
        _crossServerWarNetwork.SendGotoBattleReq(req);
    }
    public void OnReceiveGotoBattle(GotoBattleResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.PrepareBattleView.OnStartCrossServerWarResp(data.hero_attribute);
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
        else
        {
            Debug.LogError("GotoBattleResp is null");
        }
    }
    //推送设置
    public void CrossServerWarNotify(NotifyRefresh resp)
    {
        if (resp.status == 0)
        {
            isBattleStart = false;
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_CROSSSERVERWAR))
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CAPTURETERRITORYCAOMPLETE);
        }
        else
        {
            isBattleStart = true;
        }
        buyCDTimes = 0;
        time_Battle = resp.num;
        if (UISystem.Instance.UIIsOpen(UnionView.UIName))
        {
            UISystem.Instance.UnionView.UpdateCrossServerWarNotice();
        }
    }
    /// <summary>
    /// 战斗后退回主界面
    /// </summary>
    public void ReturnToBattleField()
    {
        if (!UnionModule.Instance.HasUnion)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                string.Format(ConstString.ERR_NO_UNION, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel));
            return;
        }
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONVIEW);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CROSSSERVERWAR);
        UISystem.Instance.CrossServerWarView.view.Gobj_StageInfo.SetActive(false);
        UISystem.Instance.CrossServerWarView.view.Gobj_TileInfo.SetActive(false);
    }
}

public class TerritoryPointRank
{
    public int order;
    public string name;
    public int point;

    public TerritoryPointRank(string _name,string _point)
    {
        order = 0;
        name = _name;
        point = int.Parse(_point);
    }
    public TerritoryPointRank()
    {
        order = 0;
        name = "";
        point = 0;
    }
}

public class SpecialProductionItem
{
    public uint tileId;
    public string production;
    public string name;

    public SpecialProductionItem(uint _id,string _productiong,string _name)
    {
        tileId = _id;
        production = _productiong;
        name = _name;
    }
}