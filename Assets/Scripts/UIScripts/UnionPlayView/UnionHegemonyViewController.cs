using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;

public class UnionHegemonyViewController : UIBase
{
    const float TIME_ONE_ROUND = 10;//一轮战斗时长
    const int MAXCOUNT = 5;
    const string ITEMNAME = "RestTeam_{0}";
    int BATTLEROUND = 5;
    public static bool CancelBattle = false;
    public UnionHegemonyView view;
    private List<ArenaPlayer> _selfPlayerList;
    private List<ArenaPlayer> _enemyPlayerList;
    private List<UnionHTeamItem> _selfTeamItemList;
    private List<UnionHTeamItem> _enemyTeamItemList;
    private List<UnionPvpRecord> _fightResultList;
    private int curRound = 0;
    private HegemonyBattleResult _lastResult = HegemonyBattleResult.None;
    private HegemonyBattleResult _finalResult = HegemonyBattleResult.None;

    private BaseUnion selfUnion;
    private BaseUnion enmyUnion;
    private UnionPvpRecord LatPvpRecord;
    private bool EndFight = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new UnionHegemonyView();
            view.Initialize();
            BtnEventBinding();
            _selfPlayerList = new List<ArenaPlayer>();
            _enemyPlayerList = new List<ArenaPlayer>();
            _selfTeamItemList = new List<UnionHTeamItem>();
            _enemyTeamItemList = new List<UnionHTeamItem>();
            _fightResultList = new List<UnionPvpRecord>();
            CancelBattle = false;
            //Scheduler.Instance.AddUpdator(TestAttack);
        }
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        CancelBattle = true;
        view.UnionHPlayerInfoItem_Self.StopAllCoroutines();
        view.UnionHPlayerInfoItem_Enemy.StopAllCoroutines();
        DestroyItemList();
        Clear();
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.EndObject).onClick += ButtonEvent_EndPanel;
        UIEventListener.Get(view.Spt_BackBG.gameObject).onClick += ButtonEvent_ClosePanel;
        view.UnionHPlayerInfoItem_Self.InBattleComplete = InSelfBattleComplete;
        view.UnionHPlayerInfoItem_Enemy.InBattleComplete = InEnemyBattleComplete;
        view.UIWrapContent_UIGrid_Self.onInitializeItem = WrapEvent_UpdateRestTeam_Self;
        view.UIWrapContent_UIGrid_Enemy.onInitializeItem = WrapEvent_UpdateRestTeam_Enemy;
    }
    public void OnShowHegemonyBattle(BaseUnion selfUnion, BaseUnion enemyUnion, OpenUnionCityResp data)
    {
        if (data == null)
            return;
        this.OnShowHegemonyBattle(1, selfUnion, enemyUnion, data);
        this.BATTLEROUND = 1;
        view.Lbl_TitleLb.text = ConstString.UNIONPRISON_FIGHT;
        this.view.EndObject.gameObject.SetActive(true);
    }
    public void OnShowHegemonyBattle(int city, BaseUnion selfUnion, BaseUnion enemyUnion, OpenUnionCityResp data)
    {
        //Debug.LogWarning("OnUpdateView  1  selfUnion " + selfUnion.name);
        //Debug.LogWarning("OnUpdateView  2  enemy " + enemyUnion.name);
        //Debug.LogWarning("OnUpdateView  3  selfList.Count = " + data.self_array.Count);
        //Debug.LogWarning("OnUpdateView  4  enemyList.Count = " + data.enemy_array.Count);
        //foreach (UnionPvpRecord result in data.record)
        //{
        //    Debug.Log("Battle ......... win " + result.win_charid + " lose " + result.lose_charid + " win player ID " + result.win_player.hero.charid);
        //}
        //foreach (ArenaPlayer player in data.self_array)
        //{
        //    if (player.hero.charid == PlayerData.Instance._RoleID)
        //    {
        //        foreach (ArenaSoldier soldier in player.soldiers)
        //        {
        //            Debug.Log("Self Player ...... " + player.hero.charid + " soldier id = " + soldier.soldier.id + " num = " + soldier.num);
        //        }
        //    }
        //}

        Clear();
        view.Lbl_TitleLb.text = ConstString.GUILDCITYNAME[city - 1] + ConstString.UNIONBATTLE_LABEL_CITY;
        _finalResult = (HegemonyBattleResult)data.fight_result;
         _selfPlayerList.Clear();
        _selfPlayerList.AddRange(data.self_array);
        _enemyPlayerList.Clear();
        _enemyPlayerList.AddRange(data.enemy_array);
        _fightResultList.Clear();
        _fightResultList.AddRange(data.record);
        this.selfUnion = selfUnion;
        this.enmyUnion = enemyUnion;
        //其中一个或者军团没有人再此城市参战 直接结算
        int selfWinCount = 0, enemyWinCount = 0, selfTeamCount = _selfPlayerList.Count, enemyTeamCount = _enemyPlayerList.Count;
        long existTime = Main.mTime - (long)data.start_tick;
        int index = Mathf.FloorToInt(existTime / TIME_ONE_ROUND);
       
        if (data.self_array.Count < 1 || data.enemy_array.Count < 1)
        {
            view.UnionHUnionInfoItem_Self.UpdataItem(selfUnion, selfTeamCount, 0, 0);
            view.UnionHUnionInfoItem_Enemy.UpdataItem(enemyUnion, enemyTeamCount, 0, 0);
            if (data.self_array != null && data.self_array.Count > 0)
            {
                view.UnionHPlayerInfoItem_Self.UpdateItem(_selfPlayerList[0]);
                _selfPlayerList.RemoveAt(0);
            }
            if (data.enemy_array != null && data.enemy_array.Count > 0)
            {
                view.UnionHPlayerInfoItem_Enemy.UpdateItem(_enemyPlayerList[0]);
                _enemyPlayerList.RemoveAt(0);
            }
            UpdateRestTeam();
            HegemonySettle();
            return;
        }
      
        //Debug.Log("Battle existTime: " + existTime + "; index = " + index + "; battle.Count = " + _fightResultList.Count + " start_tick "+ data.start_tick + " Main.Time "+Main.mTime );
        //超时结算
        if (index >= _fightResultList.Count)
        {
            //先统计一下各队胜场
            TryCalculateWinCount(_fightResultList.Count - 1, out selfWinCount, out enemyWinCount);
            //最后一场的数据
            RestoreLastSoldier();
            view.UnionHUnionInfoItem_Self.UpdataItem(selfUnion, selfTeamCount, selfTeamCount - _selfPlayerList.Count, selfWinCount);
            view.UnionHUnionInfoItem_Enemy.UpdataItem(enemyUnion, enemyTeamCount, enemyTeamCount - _enemyPlayerList.Count, enemyWinCount);
            UpdateRestTeam();
            HegemonySettle();
            return;
        }
        //战斗进行一段时间  需要断点重现 从中途开始表现 //
        if (0 < index && index <= _fightResultList.Count - 1)
        {
            //先统计一下各队胜场
            TryCalculateWinCount(index, out selfWinCount, out enemyWinCount);
            //断点重现
            RestorePlayerSoldier(index);
        }
        //战斗刚刚开始 从第一场开始
        else if (index <= 0)
        {
            view.UnionHPlayerInfoItem_Self.UpdateItem(_selfPlayerList[0]);
            view.UnionHPlayerInfoItem_Enemy.UpdateItem(_enemyPlayerList[0]);
            _selfPlayerList.RemoveAt(0);
            _enemyPlayerList.RemoveAt(0);
        }
        view.UnionHUnionInfoItem_Self.UpdataItem(selfUnion, selfTeamCount, selfTeamCount -_selfPlayerList.Count, selfWinCount);
        view.UnionHUnionInfoItem_Enemy.UpdataItem(enemyUnion, enemyTeamCount ,enemyTeamCount -_enemyPlayerList.Count, enemyWinCount);
        UpdateRestTeam();
        _lastResult = HegemonyBattleResult.None;
        Scheduler.Instance.AddTimer(1f, false, NextBattle);
    }

    private void UpdateRestTeam()
    {
        //Debug.LogWarning("_selfPlayerList.Count = "+ _selfPlayerList.Count + " _enemyPlayerList.Count = " +_enemyPlayerList.Count);
        Main.Instance.StartCoroutine(UpdateTeamItemInspector(_selfPlayerList, true));
        Main.Instance.StartCoroutine(UpdateTeamItemInspector(_enemyPlayerList, false));
    }

    private void WrapEvent_UpdateRestTeam_Self(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _selfPlayerList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
            go.name = realIndex.ToString();
        }
        UnionHTeamItem item = go.GetComponent<UnionHTeamItem>();
        if (item == null)
            item = _enemyTeamItemList[wrapIndex];
        ArenaPlayer data = _selfPlayerList[realIndex];
        item.UpdateItem(data.hero.charname, realIndex + 1, data.hero.level, data.combat_power);
    }

    private void WrapEvent_UpdateRestTeam_Enemy(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _enemyPlayerList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        UnionHTeamItem item = go.GetComponent<UnionHTeamItem>();
        if (item == null)
            item = _enemyTeamItemList[wrapIndex];
        ArenaPlayer data = _enemyPlayerList[realIndex];
        item.UpdateItem(data.hero.charname, realIndex + 1, data.hero.level, data.combat_power);
    }

    private void ButtonEvent_ClosePanel(GameObject go)
    {
        CancelBattle = true;
        Scheduler.Instance.RemoveTimer(NextBattle);

        view.UnionHPlayerInfoItem_Self.StopAllCoroutines();
        view.UnionHPlayerInfoItem_Enemy.StopAllCoroutines();

        view.Lbl_TitleLb.text = ConstString.UNIONBATTLE_LABEL_CITY;
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_UNIONHEGEMONYVIEW);
    }
    private void ButtonEvent_EndPanel(GameObject go)
    {
        if (this.EndFight)
            return;

        int selfWinCount = 0, enemyWinCount = 0, selfTeamCount = _selfPlayerList.Count, enemyTeamCount = _enemyPlayerList.Count;
        
        //先统计一下各队胜场
        //TryCalculateWinCount(_fightResultList.Count - 1, out selfWinCount, out enemyWinCount);
        //最后一场的数据
        //RestoreLastSoldier();
        if(_fightResultList.Count > 0)
        {
            this.LatPvpRecord = _fightResultList[_fightResultList.Count - 1];
        }
        if(this.LatPvpRecord != null)
        {
            ArenaPlayer list = this.LatPvpRecord.win_player;
            List<ArenaPlayer> tmpList = new List<ArenaPlayer>();
            
            if (_finalResult == HegemonyBattleResult.Self)
            {
                for (int i = this._selfPlayerList.Count - 1; i >= 0;--i )
                {

                    if (_selfPlayerList[i] != null && _selfPlayerList[i].hero.accid != list.hero.accid)
                        tmpList.Add(_selfPlayerList[i]);
                    else
                    {
                        list.combat_power = _selfPlayerList[i].combat_power;
                        enemyWinCount = i + 1;
                        break;
                    }
                }
                selfWinCount = view.UnionHUnionInfoItem_Enemy.MaxCount;
                tmpList.Reverse();
                _selfPlayerList = tmpList;
                view.UnionHPlayerInfoItem_Self.UpdateItem(list);
                if (this._enemyPlayerList.Count > 0)
                {
                    view.UnionHPlayerInfoItem_Enemy.UpdateItem(this._enemyPlayerList[this._enemyPlayerList.Count - 1]);
                }
                this._enemyPlayerList.Clear();
                view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
                view.UnionHPlayerInfoItem_Enemy.DoSettle();
                view.UnionHUnionInfoItem_Self.UpdateResult(true);
                view.UnionHUnionInfoItem_Enemy.UpdateResult(false);
                view.UnionHUnionInfoItem_Self.ResetWinCount(selfWinCount);
                view.UnionHUnionInfoItem_Enemy.UpdateWinCount(enemyWinCount);
            }
            else if (_finalResult == HegemonyBattleResult.Enemy)
            {
                for (int i = this._enemyPlayerList.Count - 1; i >= 0; --i)
                {
                    if (_enemyPlayerList[i] != null && _enemyPlayerList[i].hero.accid != list.hero.accid)
                        tmpList.Add(_enemyPlayerList[i]);
                    else
                    {
                        list.combat_power = _enemyPlayerList[i].combat_power;
                        selfWinCount = i + 1;
                        break;
                    }
                }
                enemyWinCount = view.UnionHUnionInfoItem_Self.MaxCount;
                tmpList.Reverse();
                _enemyPlayerList = tmpList;
                view.UnionHPlayerInfoItem_Enemy.UpdateItem(list);

                if (this._selfPlayerList.Count > 0)
                    view.UnionHPlayerInfoItem_Self.UpdateItem(this._selfPlayerList[this._selfPlayerList.Count - 1]);
                this._selfPlayerList.Clear();
                view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
                view.UnionHPlayerInfoItem_Self.DoSettle();
                view.UnionHUnionInfoItem_Enemy.UpdateResult(true);
                view.UnionHUnionInfoItem_Self.UpdateResult(false);
                view.UnionHUnionInfoItem_Enemy.ResetWinCount(enemyWinCount);
                view.UnionHUnionInfoItem_Self.UpdateWinCount(selfWinCount);
            }
            else
            {
                if (this._selfPlayerList.Count > 0)
                    view.UnionHPlayerInfoItem_Self.UpdateItem(this._selfPlayerList[this._selfPlayerList.Count - 1]);
                if (this._enemyPlayerList.Count > 0)
                    view.UnionHPlayerInfoItem_Enemy.UpdateItem(this._enemyPlayerList[this._enemyPlayerList.Count - 1]);
                this._enemyPlayerList.Clear();
                this._selfPlayerList.Clear();
                view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
                view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
                view.UnionHPlayerInfoItem_Self.DoSettle();
                view.UnionHPlayerInfoItem_Enemy.DoSettle();
            }
        }
        else
        {
            view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
            view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
        }
        view.UnionHUnionInfoItem_Self.UpdateRestCount(selfTeamCount - _selfPlayerList.Count);
        view.UnionHUnionInfoItem_Enemy.UpdateRestCount(enemyTeamCount - _enemyPlayerList.Count);
        UpdateRestTeam();
        this.LatPvpRecord = null;
        CancelBattle = true;
        this.EndFight = true;
        Scheduler.Instance.RemoveTimer(NextBattle);
        view.UnionHPlayerInfoItem_Self.StopAllCoroutines();
        view.UnionHPlayerInfoItem_Enemy.StopAllCoroutines();
        return;
    }
    private void DestroyItemList()
    {
        for (int i = _selfTeamItemList.Count - 1; i >= 0; i--)
        {
            UnionHTeamItem item = _selfTeamItemList[i];
            _selfTeamItemList.RemoveAt(i);
            GameObject.Destroy(item.gameObject);
        }
        for (int i = _enemyTeamItemList.Count - 1; i >= 0; i--)
        {
            UnionHTeamItem item = _enemyTeamItemList[i];
            _enemyTeamItemList.RemoveAt(i);
            GameObject.Destroy(item.gameObject);
        }
        _selfTeamItemList.Clear();
        _enemyTeamItemList.Clear();
    }

    private void Clear()
    {
        this.view.EndObject.gameObject.SetActive(false);
        CancelBattle = true;
        this.BATTLEROUND = 5;
        Scheduler.Instance.RemoveTimer(NextBattle);
        view.UnionHUnionInfoItem_Self.Clear();
        view.UnionHUnionInfoItem_Enemy.Clear();
        view.UnionHPlayerInfoItem_Self.Clear();
        view.UnionHPlayerInfoItem_Enemy.Clear();
        view.UIWrapContent_UIGrid_Self.ReGetChild();
        view.UIWrapContent_UIGrid_Enemy.ReGetChild();
        view.Grd_UIGrid_Self.Reposition();
        view.Grd_UIGrid_Enemy.Reposition();
        view.ScrView_RestTeamSrollView_Self.ResetPosition();
        view.ScrView_RestTeamSrollView_Enemy.ResetPosition();
        _selfPlayerList.Clear();
        _enemyPlayerList.Clear();
        _fightResultList.Clear();
        LatPvpRecord = null;
        this.EndFight = false;
        _lastResult = HegemonyBattleResult.None;
        _finalResult = HegemonyBattleResult.None;
        selfUnion = null;
        enmyUnion = null;
        CancelBattle = false;
    }

    private IEnumerator UpdateTeamItemInspector(List<ArenaPlayer> list, bool isSelfUnion)
    {
        UIWrapContent warpContent = null;
        UIScrollView scrollView = null;
        UIGrid grid = null;
        List<UnionHTeamItem> itemList = null;

        if (isSelfUnion)
        {
            itemList = _selfTeamItemList;
            warpContent = view.UIWrapContent_UIGrid_Self;
            grid = view.Grd_UIGrid_Self;
            scrollView = view.ScrView_RestTeamSrollView_Self;
        }
        else
        {
            itemList = _enemyTeamItemList;
            warpContent = view.UIWrapContent_UIGrid_Enemy;
            grid = view.Grd_UIGrid_Enemy;
            scrollView = view.ScrView_RestTeamSrollView_Enemy;
        }
        //grid.Reposition();
       // yield return 0;
       // scrollView.ResetPosition();
       // yield return 0;
        //warpContent.CleanChild();

        int infoCount = list.Count;
        int itemCount = itemList.Count;
        int index = Mathf.CeilToInt((float)infoCount / warpContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        warpContent.minIndex = -index;
        warpContent.maxIndex = 0;
        warpContent.cullContent = true;
        if (infoCount > MAXCOUNT)
        {
            warpContent.enabled = true;
            if (infoCount % 2 != 0)
            {
                warpContent.cullContent = false;
            }
            else
            {
                warpContent.cullContent = true;
            }
            infoCount = MAXCOUNT;
        }
        else
        {
            warpContent.enabled = false;
        }
        if (itemCount > infoCount)
        {
            for (int i = infoCount; i < itemCount; i++)
            {
                itemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < infoCount; i++)
        {
            if (itemCount <= i)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Obj_UnionHTeamItemObj, grid.transform);
                UnionHTeamItem item = go.AddComponent<UnionHTeamItem>();
                item.name = string.Format(ITEMNAME, i);
                itemList.Add(item);
            }
            else
            {
                itemList[i].gameObject.SetActive(true);
            }
            ArenaPlayer data = list[i];
            itemList[i].UpdateItem(data.hero.charname, i + 1, data.hero.level, data.combat_power);
        }
        if (infoCount > MAXCOUNT - 2)
            warpContent.ReGetChild();
        yield return 0;
        grid.repositionNow = true;
        yield return 0;
        scrollView.ResetPosition();
    }

    #region 争霸战斗
    /// <summary>
    /// 一回合战斗的表现
    /// </summary>
    /// <param name="result"></param>
    private void StartBattle(UnionPvpRecord result)
    {
        //Debug.LogWarning("StartBattle Battle ......... win " + result.win_charid + " lose " + result.lose_charid + " win player ID " + result.win_player.hero.charid);
        //同归于尽
        if (result.win_charid == 0)
        {
            //Debug.LogWarning(" BattleHandle Lose-Lose ");
            _lastResult = HegemonyBattleResult.Lose_Lose;
            view.UnionHPlayerInfoItem_Self.IsWin = false;
            view.UnionHPlayerInfoItem_Enemy.IsWin = false;
            view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
            view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
        }
        else//一胜一负
        {
            if (result.win_charid == view.UnionHPlayerInfoItem_Self.ID)
            {
                //Debug.LogWarning(" BattleHandle Self Win ");
                _lastResult = HegemonyBattleResult.Self;
                view.UnionHPlayerInfoItem_Self.IsWin = true;
                view.UnionHPlayerInfoItem_Self.SetRestSoldier(result.win_player.soldiers);
                view.UnionHPlayerInfoItem_Enemy.IsWin = false;
                view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
            }
            else if (result.win_charid == view.UnionHPlayerInfoItem_Enemy.ID)
            {
                //Debug.LogWarning(" BattleHandle Enemy Win ");
                _lastResult = HegemonyBattleResult.Enemy;
                view.UnionHPlayerInfoItem_Self.IsWin = false;
                view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
                view.UnionHPlayerInfoItem_Enemy.IsWin = true;
                view.UnionHPlayerInfoItem_Enemy.SetRestSoldier(result.win_player.soldiers);
            }
            else
            {
                Debug.LogError("Union Hegemony winner id is wrong !!! id = " + result.win_charid + " self.id = " + view.UnionHPlayerInfoItem_Self.ID + " enemy.id = " + view.UnionHPlayerInfoItem_Enemy.ID);
                return;
            }
        }
        view.UnionHPlayerInfoItem_Self.StartBattle(0F);
        view.UnionHPlayerInfoItem_Enemy.TweenPosition();
    }
    /// <summary>
    /// 开始表现战斗 这里是循环的 一回合结束之后这里还会在调用一次 直到表现完成
    /// </summary>
    private void NextBattle()
    {
        if (CancelBattle)
            return;

        if (_fightResultList.Count < 1)
        {
            HegemonySettle();
            this.EndFight = true;
            return;
        }
        switch (_lastResult)
        {
            case HegemonyBattleResult.Self:
                view.UnionHPlayerInfoItem_Enemy.UpdateItem(_enemyPlayerList[0]);
                _enemyPlayerList.RemoveAt(0);
                view.UnionHUnionInfoItem_Self.UpdateWinCount(1);
                view.UnionHUnionInfoItem_Enemy.UpdateRestCount(1);
                break;
            case HegemonyBattleResult.Enemy:
                view.UnionHPlayerInfoItem_Self.UpdateItem(_selfPlayerList[0]);
                _selfPlayerList.RemoveAt(0);
                view.UnionHUnionInfoItem_Enemy.UpdateWinCount(1);
                view.UnionHUnionInfoItem_Self.UpdateRestCount(1);
                break;
            case HegemonyBattleResult.Lose_Lose:
                view.UnionHPlayerInfoItem_Self.UpdateItem(_selfPlayerList[0]);
                view.UnionHPlayerInfoItem_Enemy.UpdateItem(_enemyPlayerList[0]);
                _selfPlayerList.RemoveAt(0);
                _enemyPlayerList.RemoveAt(0);
                view.UnionHUnionInfoItem_Self.UpdateRestCount(1);
                view.UnionHUnionInfoItem_Enemy.UpdateRestCount(1);
                break;
            case HegemonyBattleResult.None:
                break;
        }
        curRound = 1;
        _lastResult = HegemonyBattleResult.None;
        LatPvpRecord = _fightResultList[0];
        StartBattle(LatPvpRecord);
        _fightResultList.RemoveAt(0);
        UpdateRestTeam();
    }

    private void InSelfBattleComplete()
    {
        if (curRound >= BATTLEROUND)
        {
            //按照设定 输家补一次伤害 
            ShowLastEffect(_lastResult);
            RoundSettle();
        }
        else
        {
            view.UnionHPlayerInfoItem_Enemy.StartBattle(0F);
            view.UnionHPlayerInfoItem_Self.TweenPosition();
        }
    }

    private void InEnemyBattleComplete()
    {
        curRound += 1;
        view.UnionHPlayerInfoItem_Self.StartBattle(0F);
        view.UnionHPlayerInfoItem_Enemy.TweenPosition();
    }
    /// <summary>
    /// 一回合的结算
    /// </summary>
    private void RoundSettle()
    {
        view.UnionHPlayerInfoItem_Self.DoSettle();
        view.UnionHPlayerInfoItem_Enemy.DoSettle();
        Scheduler.Instance.AddTimer(1f, false, NextBattle);
    }
    /// <summary>
    /// 战役的结算
    /// </summary>
    private void HegemonySettle()
    {
        Debug.Log("Battle is Finish !!! _finalResult = " + _finalResult);
        switch (_finalResult)
        {
            case HegemonyBattleResult.Self:
                view.UnionHUnionInfoItem_Self.UpdateResult(true);
                view.UnionHUnionInfoItem_Enemy.UpdateResult(false);
                view.UnionHUnionInfoItem_Self.UpdateWinCount(1);
                break;
            case HegemonyBattleResult.Enemy:
                view.UnionHUnionInfoItem_Enemy.UpdateResult(true);
                view.UnionHUnionInfoItem_Self.UpdateResult(false);
                view.UnionHUnionInfoItem_Enemy.UpdateWinCount(1);
                break;
            default:
                break; 
        }
    }
    /// <summary>
    /// 断点重现 需要前面两场数据才能重现 本场的断点
    /// </summary>
    /// <param name="index"></param>
    private void RestorePlayerSoldier(int index)
    {
        UnionPvpRecord preResult = _fightResultList[index - 1];
        UnionPvpRecord curResult = _fightResultList[index];
        LatPvpRecord = _fightResultList[index];
        _fightResultList.RemoveRange(0, index);

        //Debug.Log("RestoreBattle Pre winner " + preResult.win_charid + " pre Result.win_charid = " + preResult.win_charid);
        //foreach (ArenaSoldier soldier in preResult.win_player.soldiers)
        //{
        //    Debug.Log("RestoreBattle Pre Winner  soldier " + soldier.soldier.id + " num " + soldier.num);
        //}
        //Debug.Log("RestoreBattle Current winner " + curResult.win_charid + " currrrrrr Result.win_charid = " + curResult.win_charid);
        //foreach (ArenaSoldier soldier in curResult.win_player.soldiers)
        //{
        //    Debug.Log("RestoreBattle curretn Winner soldier " + soldier.soldier.id + " num " + soldier.num);
        //}
        //foreach (ArenaPlayer ap in _selfPlayerList)
        //{
        //    Debug.LogWarning("In Self ======== ap.id "+ ap.hero.charid);
        //}

        //foreach (ArenaPlayer ap in _enemyPlayerList)
        //{
        //    Debug.LogWarning("In Enemy ======== ap.id " + ap.hero.charid);
        //}
        //双输
        if (preResult.win_charid == 0)
        {
            if (InThisUion(curResult.win_player.hero.charid, _selfPlayerList))
            {
                view.UnionHPlayerInfoItem_Self.UpdateItem(GetPlayerDataByID(curResult.win_player.hero.charid, _selfPlayerList));
                DeleteVailedPlayerInfo(ref _selfPlayerList, curResult.win_player.hero.charid);

                view.UnionHPlayerInfoItem_Enemy.UpdateItem(GetPlayerDataByID(curResult.lose_charid, _enemyPlayerList));
                DeleteVailedPlayerInfo(ref _enemyPlayerList, curResult.lose_charid);
            }
            else
            {
                view.UnionHPlayerInfoItem_Self.UpdateItem(GetPlayerDataByID(curResult.lose_charid, _selfPlayerList));
                DeleteVailedPlayerInfo(ref _selfPlayerList, curResult.lose_charid);

                view.UnionHPlayerInfoItem_Enemy.UpdateItem(GetPlayerDataByID(curResult.win_player.hero.charid, _enemyPlayerList));
                DeleteVailedPlayerInfo(ref _enemyPlayerList, curResult.win_player.hero.charid);
            }
        }
        else//一输一赢
        {
            //判断上一场赢家属于哪边 让后用战斗结果的数据初始化赢家
            if (InThisUion(preResult.win_player.hero.charid, _selfPlayerList))
            {
                view.UnionHPlayerInfoItem_Self.UpdateItem(preResult.win_player);
                if (InThisUion(curResult.win_player.hero.charid, _selfPlayerList))
                {
                    view.UnionHPlayerInfoItem_Enemy.UpdateItem(GetPlayerDataByID(curResult.lose_charid, _enemyPlayerList));
                    DeleteVailedPlayerInfo(ref _enemyPlayerList, curResult.lose_charid);
                }
                else
                {
                    view.UnionHPlayerInfoItem_Enemy.UpdateItem(GetPlayerDataByID(curResult.win_player.hero.charid, _enemyPlayerList));
                    DeleteVailedPlayerInfo(ref _enemyPlayerList, curResult.win_player.hero.charid);
                }
                DeleteVailedPlayerInfo(ref _selfPlayerList, preResult.win_player.hero.charid);
            }
            else
            {
                view.UnionHPlayerInfoItem_Enemy.UpdateItem(preResult.win_player);
                if (InThisUion(curResult.win_player.hero.charid, _selfPlayerList))
                {
                    view.UnionHPlayerInfoItem_Self.UpdateItem(GetPlayerDataByID(curResult.win_player.hero.charid, _selfPlayerList));
                    DeleteVailedPlayerInfo(ref _selfPlayerList, curResult.win_player.hero.charid);
                }
                else
                {
                    view.UnionHPlayerInfoItem_Self.UpdateItem(GetPlayerDataByID(curResult.lose_charid, _selfPlayerList));
                    DeleteVailedPlayerInfo(ref _selfPlayerList, curResult.lose_charid);
                }
                DeleteVailedPlayerInfo(ref _enemyPlayerList, preResult.win_player.hero.charid);
            }
        }
    }
    /// <summary>
    /// 重现最后一条数据 只需要一条战斗数据就可以重现
    /// </summary>
    private void RestoreLastSoldier()
    {
        if (_fightResultList.Count > 1)
        {
            RestorePlayerSoldier(_fightResultList.Count - 1);
        }
        else if (_fightResultList.Count == 1)
        {
            if (_selfPlayerList.Count > 0)
            {
                view.UnionHPlayerInfoItem_Self.UpdateItem(_selfPlayerList[0]);
                _selfPlayerList.RemoveAt(0);
            }
            if (_enemyPlayerList.Count > 0)
            {
                view.UnionHPlayerInfoItem_Enemy.UpdateItem(_enemyPlayerList[0]);
                _enemyPlayerList.RemoveAt(0);
            }
        }
        if(_fightResultList.Count > 0)
        {
            List<ArenaSoldier> list = _fightResultList[_fightResultList.Count - 1].win_player.soldiers;

            if (_finalResult == HegemonyBattleResult.Self)
            {
                view.UnionHPlayerInfoItem_Self.SetRestSoldier(list);
                view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
            }
            else if (_finalResult == HegemonyBattleResult.Enemy)
            {
                view.UnionHPlayerInfoItem_Enemy.SetRestSoldier(list);
                view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
            }
            else
            {
                view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
                view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
            }
        }
        else
        {
            view.UnionHPlayerInfoItem_Self.KillAllSoldiers();
            view.UnionHPlayerInfoItem_Enemy.KillAllSoldiers();
        }
        view.UnionHPlayerInfoItem_Enemy.DoSettle();
        view.UnionHPlayerInfoItem_Self.DoSettle();
    }

    private void ShowLastEffect(HegemonyBattleResult result)
    {
        if (result == HegemonyBattleResult.Enemy
            || result == HegemonyBattleResult.Lose_Lose)
        {
            view.UnionHPlayerInfoItem_Enemy.ShowAttackedEffect();
            view.UnionHPlayerInfoItem_Self.TweenPosition();
        }
    }
    
    #endregion

    private void TestAttack()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //view.UnionHPlayerInfoItem_Self.ShowAttackedEffect();
           // view.UnionHPlayerInfoItem_Enemy.ShowAttackedEffect();
        }
    }

    private bool InThisUion(uint id, List<ArenaPlayer> sourceList)
    {
        int count = sourceList.Count;
        for (int i = 0; i < count; i++)
        {
            ArenaPlayer ap = sourceList[i];
            if (ap.hero.charid == id)
            {
                return true;
            }
        }
        return false;
    }

    private void TryCalculateWinCount(int index, out int selfWins, out int enemyWins)
    {
        selfWins = 0;
        enemyWins = 0;
        for (int i = 0; i < index; i++)
        {
            UnionPvpRecord result = _fightResultList[i];
            if (result.win_charid == 0)// 双输谁也不加
                continue;
            else
            {
                //一输一赢 赢家加一
                if (InThisUion(result.win_charid, _selfPlayerList))
                {
                    selfWins += 1;
                }
                else
                    enemyWins += 1;
            }
        }
    }

    private ArenaPlayer GetPlayerDataByID(uint id, List<ArenaPlayer> sourceList)
    {
        int count = sourceList.Count;
        for (int i = 0; i < count; i++)
        {
            ArenaPlayer ap = sourceList[i];
            if (ap.hero.charid == id)
            {
                return ap;
            }
        }
        return null;
    }
    /// <summary>
    /// 从玩家列表中删除无用的数据 
    /// </summary>
    /// <param name="list"></param>
    /// <param name="id"></param>
    private void DeleteVailedPlayerInfo(ref List<ArenaPlayer> list, uint id)
    {
        int count = list.Count;
        int index = 0;
        for (int i = 0; i < count; i++)
        {
            ArenaPlayer player = list[i];

            if (player.hero.charid == id)
            {
                index = i;
                break;
            }
        }
        list.RemoveRange(0, index + 1);
    }

   
}
