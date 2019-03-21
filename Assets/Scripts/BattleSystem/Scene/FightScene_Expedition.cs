#region 
/*
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

/// <summary>
/// 远征战场
/// </summary>
public class FightScene_Expedition : FightSceneBase
{
    /// <summary>
    /// 路径数量
    /// </summary>
    private const byte ROLEPATH_MAX_COUNT = 7;
    /// <summary>
    /// 路径间距
    /// </summary>
    private const float ROLEPATH_DISTANCE = 20.0f;
    /// <summary>
    /// 左边界
    /// </summary>
    private const float LIMIT_LEFT = -516;
    /// <summary>
    /// 右边界
    /// </summary>
    private const float LIMIT_RIGHT = -1532;
    /// <summary>
    /// 敌方英雄死亡状态
    /// </summary>
    private const int ENEMY_HERO_STATUS_DEAD = 1;


    /// <summary>
    /// 自己基地 护送起始地点 传送门[配置表修改]
    /// </summary>
    [HideInInspector]
    public Transform trans_Barracks_Self;


    ///// <summary>
    ///// 战斗场景背景图
    ///// </summary>
    //private UITexture fightBackGround;
    /// <summary>
    /// 士兵死亡计数器
    /// </summary>
    public Dictionary<ulong, int> _SoldierCount = new Dictionary<ulong, int>();
    public Dictionary<ulong, CounterTool> _Counter_Soldier = new Dictionary<ulong, CounterTool>();
    public Dictionary<ulong, CounterTool> _Counter_Enemy = new Dictionary<ulong, CounterTool>();
    /// <summary>
    /// 己方士兵信息
    /// </summary>
    public List<FightSoldierInfo> _SoldierList = new List<FightSoldierInfo>();
    /// <summary>
    /// 敌方玩家信息
    /// </summary>
    private FightPlayerInfo _EnemyInfo;
    /// <summary>
    /// 敌方角色计数器
    /// </summary>
    private CounterTool _EnemyCounter;
    /// <summary>
    /// 配置表数据
    /// </summary>
    private ExpeditionData expeditionInfo;
    /// <summary>
    /// 敌方英雄是否死亡
    /// </summary>
    private bool enemyHeroIsDeath;
    /// <summary>
    /// 角色资源总计数
    /// </summary>
    private CounterTool _TotalRoleCounter;

    private List<FightDestroyInfo> listFinishInfo = new List<FightDestroyInfo>();
    /// <summary>
    /// 自己基地脚本
    /// </summary>
    private Barracks_Self _Barracks_Self;
    /// <summary>
    /// 结束城堡血量
    /// </summary>
    private int _FinalBarracksHP;

    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        pathIndex_Hero = 3;
        base.Initialize(vInfo);
        if (sceneInfo != null)
            expeditionInfo = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(sceneInfo.sceneID);
        InitSceneStatus();
        if (listFinishInfo == null)
        {
            listFinishInfo = new List<FightDestroyInfo>();
        }
        else
        {
            listFinishInfo.Clear();
        }
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        if (_Barracks_Self != null)
            _Barracks_Self.ReSet();
        base.Uninitialize();
    }

    protected override void SetLocalUseComponentsAndDatas()
    {
        Transform tmpLocalTrans = this.transform;
        sceneCamera = tmpLocalTrans.FindChild("Camera").gameObject.GetComponent<Camera>();
        shakeCamera = tmpLocalTrans.FindChild("CameraShake").gameObject.GetComponent<Camera>();
        transContent = tmpLocalTrans.FindChild("Camera/SceneContent");
        transOther = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other").gameObject.GetComponent<Transform>();
        transHero = tmpLocalTrans.FindChild("Camera/SceneContent/Pos_Hero").gameObject.GetComponent<Transform>();
        transSelf = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Self").gameObject.GetComponent<Transform>();
        transEnemy = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Enemy").gameObject.GetComponent<Transform>();
        trans_Barracks_Self = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Barracks_Self").gameObject.GetComponent<Transform>();
        fightBackGround = transOther.FindChild("Tex_BackGround").gameObject.GetComponent<UITexture>();
        fightBackGround.gameObject.SetActive(false);
        limitLeft = LIMIT_LEFT;
        limitRight = LIMIT_RIGHT;

        if (trans_Barracks_Self != null)
        {
            _Barracks_Self = trans_Barracks_Self.gameObject.AddComponent<Barracks_Self>();
        }
        if (_Barracks_Self != null)
        {
            _Barracks_Self.SetLocalUseComponentsAndDatas();
        }
        MaxGroundWidth = 2048;

        base.SetLocalUseComponentsAndDatas();
    }

    protected override void SetFightSceneBackGround()
    {
        if (fightBackGround == null)
            return;
        fightBackGround.mainTexture = null;
        if (sceneInfo == null)
            return;

        for (int i = 0; i < 2; i++)
        {
            if (ListTexture.Count <= i)
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightBackGround.gameObject, fightBackGround.transform.parent);
                if (tmpObj == null)
                    continue;
                ListTexture.Add(tmpObj.GetComponent<UITexture>());
            }
        }
        for (int i = 0; i < ListTexture.Count; i++)
        {
            if (ListTexture[i] == null)
                continue;
            ListTexture[i].transform.localPosition = new Vector3(i * 1024, 0, 0);
            ListTexture[i].transform.localScale = Vector3.one;

            string tmpName = string.Empty;
            if (i == 0)
                tmpName = string.Format("{0}_L.assetbundle", sceneInfo.sceneBackGround);
            else
                tmpName = string.Format("{0}_R.assetbundle", sceneInfo.sceneBackGround);

            ResourceLoadManager.Instance.LoadAloneImage(tmpName, (texture) =>
            {
                ListTexture[i].mainTexture = texture;
                ListTexture[i].MakePixelPerfect();
                ListTexture[i].gameObject.SetActive(true);
            });
        }
    }

    protected override void PreLoadResource()
    {
        base.PreLoadResource();

        //
        _TotalRoleCounter.AddCount();
        CheckResourceIsReady(null);
        if (_EnemyInfo == null || _EnemyInfo.mSoldierList == null)
            return;

        for (int i = 0; i < _EnemyInfo.mSoldierList.Count;++i )
        {
            FightSoldierInfo tmpInfo = _EnemyInfo.mSoldierList[i];
            if (tmpInfo == null)
                continue;

            foreach (Skill tmpSkill in tmpInfo.mSoldier._skillsDepot._skillsList)
            {
                if (tmpSkill == null)
                    continue;

                tmpSkill.CacheEffect();
            }

        }
    }

    protected override void AddRolePath()
    {
        for (int i = 0; i < ROLEPATH_MAX_COUNT; i++)
            AddSingleRolePath(-(i * ROLEPATH_DISTANCE));
    }


    private void FinishOperate()
    {
        if (_SceneStatus == ESceneStatus.essNormal)
        {
            if (listFinishInfo != null)
            {
                int tmpIndex = 0;
                foreach (FightDestroyInfo tmpInfo in listFinishInfo)
                {
                    if (tmpInfo == null)
                    {
                        tmpIndex += 1;
                    }
                    else
                    {
                        switch (tmpInfo._DestroyStatus)
                        {
                            case EPVEFinishStatus.epvefsDieHero:
                                {
                                    Operate_Failure();
                                    fightFailReason = EPVEFinishStatus.epvefsDieHero;
                                }
                                break;
                            case EPVEFinishStatus.epvefsDieSoldier:
                                {
                                    ulong tmpUID = (ulong)tmpInfo._DestroyID;
                                    Debug.LogError("tmpUID: ------------------------------------------------------");
                                    Debug.LogError("tmpUID: " + tmpUID);
                                    if (tmpUID == 0)
                                        return;
                                    if (!_Counter_Soldier.ContainsKey(tmpUID))
                                        return;
                                    _Counter_Soldier[tmpUID].AddCount();
                                }
                                break;
                            case EPVEFinishStatus.epvefsDisEnemyHero:
                                {
                                    enemyHeroIsDeath = true;
                                    CheckIsVictory();
                                }
                                break;
                            case EPVEFinishStatus.epvefsDisEnemySoldier:
                                {
                                    ulong tmpUID = (ulong)tmpInfo._DestroyID;
                                    if (!_Counter_Enemy.ContainsKey(tmpUID))
                                        return;
                                    _Counter_Enemy[tmpUID].DelCount();
                                    CheckIsVictory();
                                }
                                break;
                            case EPVEFinishStatus.epvefsOutTime:
                                {
                                    Operate_Failure();
                                    fightFailReason = EPVEFinishStatus.epvefsOutTime;
                                }
                                break;
                            case EPVEFinishStatus.epvefsDieBarracksSelf:
                                {//己方城堡死亡//
                                    Operate_Failure();
                                    fightFailReason = EPVEFinishStatus.epvefsDieBarracksSelf;
                                }
                                break;
                            default:
                                { }
                                break;
                        }
                        break;
                    }
                }
                for (int i = 0; i < tmpIndex + 1; i++)
                {
                    listFinishInfo.RemoveAt(0);
                }

                if (listFinishInfo.Count > 0)
                {
                    FinishOperate();
                }
            }
        }
    }
    /// <summary>
    /// 战斗结束判断
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FinishOperate(object vDataObj)
    {
        Debug.Log("【1】CommandEvent_FinishOperate");
        FightDestroyInfo tmpInfo = (FightDestroyInfo)vDataObj;
        if (tmpInfo == null)
            return;
        Debug.Log("【2】CommandEvent_FinishOperate");
        int tmpInitCount = listFinishInfo.Count;
        listFinishInfo.Add(tmpInfo);
        if (tmpInitCount == 0)
        {
            Debug.Log("【3】CommandEvent_FinishOperate");
            FinishOperate();
        }

        //FightDestroyInfo tmpInfo = (FightDestroyInfo)vDataObj;
        //if (tmpInfo == null)
        //    return;

        //switch (tmpInfo._DestroyStatus)
        //{
        //    case EPVEFinishStatus.epvefsDieHero:
        //        {
        //            Operate_Failure();
        //            fightFailReason = EPVEFinishStatus.epvefsDieHero;
        //        }
        //        break;
        //    case EPVEFinishStatus.epvefsDieSoldier:
        //        {
        //            ulong tmpUID = (ulong)tmpInfo._DestroyID;
        //            if (!_Counter_Soldier.ContainsKey(tmpUID))
        //                return;
        //            _Counter_Soldier[tmpUID].AddCount();
        //        }
        //        break;
        //    case EPVEFinishStatus.epvefsDisEnemyHero:
        //        {
        //            enemyHeroIsDeath = true;
        //            CheckIsVictory();
        //        }
        //        break;
        //    case EPVEFinishStatus.epvefsDisEnemySoldier:
        //        {
        //            ulong tmpUID = (ulong)tmpInfo._DestroyID;
        //            if (!_Counter_Enemy.ContainsKey(tmpUID))
        //                return;
        //            _Counter_Enemy[tmpUID].DelCount();
        //            CheckIsVictory();
        //        }
        //        break;
        //    case EPVEFinishStatus.epvefsOutTime:
        //        {
        //            Operate_Failure();
        //            fightFailReason = EPVEFinishStatus.epvefsOutTime;
        //        }
        //        break;
        //    default:
        //        { }
        //        break;
        //}
    }
    private void SetEnemySoldiersInfo()
    {
        _FinalBarracksHP = 0;
        if (_Barracks_Self != null)
        {
            _FinalBarracksHP = _Barracks_Self.Get_FightAttribute.HP;
        }
        if (_Counter_Enemy == null)
            return;
        if ((PlayerData.Instance._ExpeditionInfo.enemies != null) && (PlayerData.Instance._ExpeditionInfo.enemies.soldiers != null))
        {
            for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.enemies.soldiers.Count; i++)
            {
                if (!_Counter_Enemy.ContainsKey(PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i].soldier.uid))
                    continue;
                PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i].num = _Counter_Enemy[PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i].soldier.uid].Get_Count;
            }
        }
    }


    /// <summary>
    /// 检测是否胜利
    /// </summary>
    private void CheckIsVictory()
    {
        if (!enemyHeroIsDeath)
            return;
        foreach (KeyValuePair<ulong, CounterTool> tmpInfo in _Counter_Enemy)
        {
            if (tmpInfo.Value.Get_Count > 0)
            {
                return;
            }
        }
        Operate_Victory();
    }

    public override bool Operate_Victory()
    {
        Debug.LogError("Operate_Victory           1");
        if (!base.Operate_Victory())
            return false;
        Debug.LogError("Operate_Victory           2");
        List<SoldierList> tmpSoldierList = new List<SoldierList>();
        foreach (KeyValuePair<ulong, CounterTool> tmpInfo in _Counter_Soldier)
        {
            if (tmpInfo.Value.Get_Count == 0)
                continue;
            SoldierList tmpData = new SoldierList();
            tmpData.uid = tmpInfo.Key;
            tmpData.num = tmpInfo.Value.Get_Count;
            tmpSoldierList.Add(tmpData);
        }

        if (enemyHeroIsDeath)
            PlayerData.Instance._ExpeditionInfo.enemies.hero = null;
        SetEnemySoldiersInfo();
        FightRelatedModule.Instance.SendExpeditionResult(1, tmpSoldierList, PlayerData.Instance._ExpeditionInfo.enemies, _FinalBarracksHP);
        _SceneStatus = ESceneStatus.essVictory;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        List<SoldierList> tmpSoldierList = new List<SoldierList>();
        foreach (KeyValuePair<ulong, CounterTool> tmpInfo in _Counter_Soldier)
        {
            SoldierList tmpData = new SoldierList();
            tmpData.uid = tmpInfo.Key;
            if (_SoldierCount.ContainsKey(tmpInfo.Key))
                tmpData.num = _SoldierCount[tmpInfo.Key];
            else
                tmpData.num = 0;
            tmpSoldierList.Add(tmpData);
        }

        if (enemyHeroIsDeath)
            PlayerData.Instance._ExpeditionInfo.enemies.hero = null;
        SetEnemySoldiersInfo();
        FightRelatedModule.Instance.SendExpeditionResult(2, tmpSoldierList, PlayerData.Instance._ExpeditionInfo.enemies, _FinalBarracksHP);
        _SceneStatus = ESceneStatus.essFailure;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        return true;
    }

    protected override void CheckResourceIsReady(RoleBase vRole)
    {
        _TotalRoleCounter.DelCount();
        if (_TotalRoleCounter.Get_Count > 0)
            return;
        base.CheckResourceIsReady(vRole);
    }


    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitSceneStatus()
    {
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;
        SceneManager.Instance.DicSceneMarkObj.Clear();
        _DicMarkObj.Clear();

        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        //物件重置//
        if (_Barracks_Self != null)
            _Barracks_Self.ReSet();
        //清空角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //设置城堡//
        if (_Barracks_Self != null)
        {
            _Barracks_Self.SetBarracksShowStatus(expeditionInfo.castletype);
            ShowInfoBase tmpAttributeSelf = new ShowInfoBase();
            tmpAttributeSelf.CopyTo(FightRelatedModule.Instance.mCastleInfo.mAttribute);
            _Barracks_Self.InitRoleAttribute(tmpAttributeSelf);
            _DicMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
            SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
        }

        //获取怪物刷新表//
        GetFightDatas();
        PreLoadResource();

        //开启刷怪//
        _SceneStatus = ESceneStatus.essNormal;
        CreateSoldier();
        CreateEnemy();
    }

    /// <summary>
    /// 获取战斗数据
    /// </summary>
    private void GetFightDatas()
    {
        if (PlayerData.Instance._ExpeditionInfo == null)
            return;

        //设置己方士兵信息//
        if (_SoldierList == null)
            _SoldierList = new List<FightSoldierInfo>();
        _SoldierList.Clear();
        if (UISystem.Instance.FightView.listSoldierInfo != null)
            _SoldierList.AddRange(UISystem.Instance.FightView.listSoldierInfo);

        //设置敌方信息//
        if (_EnemyInfo == null)
            _EnemyInfo = new FightPlayerInfo(PlayerData.Instance._ExpeditionInfo.enemies);
        else
            _EnemyInfo.ReSetInfo(PlayerData.Instance._ExpeditionInfo.enemies);

        GetTotalRoleCounter();
    }

    /// <summary>
    /// 统计角色总数
    /// </summary>
    private void GetTotalRoleCounter()
    {
        _TotalRoleCounter = new CounterTool();
        _TotalRoleCounter.AddCount();
        if (_SoldierList != null)
        {
            for (int i = 0; i < _SoldierList.Count; i++)
            {
                _TotalRoleCounter.AddCount(_SoldierList[i].mNum);
            }
        }

        if ((PlayerData.Instance._ExpeditionInfo.enemies.hero != null) && (PlayerData.Instance._ExpeditionInfo.enemies.hero.is_dead != ENEMY_HERO_STATUS_DEAD))
            _TotalRoleCounter.AddCount();
        if (_EnemyInfo.mSoldierList != null)
        {
            for (int i = 0; i < _EnemyInfo.mSoldierList.Count; i++)
            {
                _TotalRoleCounter.AddCount(_EnemyInfo.mSoldierList[i].mNum);
            }
        }
    }

    /// <summary>
    /// 创建士兵
    /// </summary>
    private void CreateSoldier()
    {
        //创建英雄角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
                (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, EFightType.eftExpedition, 0, null, CheckResourceIsReady));

        if (_SoldierList == null)
            return;

        //清空计数器//
        _Counter_Soldier.Clear();
        _SoldierCount.Clear();

        for (int i = 0; i < _SoldierList.Count; i++)
        {
            if (_SoldierList[i] == null)
                continue;
            for (int j = 0; j < _SoldierList[i].mNum; j++)
            {
                if (_SoldierList[i].mSoldier == null)
                    continue;
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                    new CData_CreateRole(_SoldierList[i].mSoldier.showInfoSoldier, _SoldierList[i].mSoldier.uId, Get_OtherIndex,
                        ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcSelf, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
            }
            //设置计数器//
            if (!_Counter_Soldier.ContainsKey(_SoldierList[i].mSoldier.uId))
            {
                _Counter_Soldier.Add(_SoldierList[i].mSoldier.uId, new CounterTool());
            }
            if (!_SoldierCount.ContainsKey(_SoldierList[i].mSoldier.uId))
            {
                _SoldierCount.Add(_SoldierList[i].mSoldier.uId, _SoldierList[i].mNum);
            }
        }
    }
    /// <summary>
    /// 创建敌人
    /// </summary>
    private void CreateEnemy()
    {
        if (_SceneStatus != ESceneStatus.essNormal)
            return;
        if (_EnemyInfo == null)
            return;

        enemyHeroIsDeath = true;
        if ((PlayerData.Instance._ExpeditionInfo.enemies.hero != null) && (PlayerData.Instance._ExpeditionInfo.enemies.hero.is_dead != ENEMY_HERO_STATUS_DEAD))
        {
            if (_EnemyInfo.mAttribute != null)
            {
                _EnemyInfo.mAttribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(_EnemyInfo.mAttribute.AttRate);
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                    new CData_CreateRole(_EnemyInfo.mAttribute, 0, pathIndex_Hero, ERoleType.ertHero,
                        (EHeroGender)_EnemyInfo.mGender, EFightCamp.efcEnemy, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
                enemyHeroIsDeath = false;
            }
        }

        _Counter_Enemy.Clear();
        if (_EnemyInfo.mSoldierList != null)
        {
            for (int i = 0; i < _EnemyInfo.mSoldierList.Count; i++)
            {
                if (_EnemyInfo.mSoldierList[i] == null)
                    continue;
                for (int j = 0; j < _EnemyInfo.mSoldierList[i].mNum; j++)
                {
                    if (_EnemyInfo.mSoldierList[i].mSoldier == null)
                        continue;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        new CData_CreateRole(_EnemyInfo.mSoldierList[i].mSoldier.showInfoSoldier, _EnemyInfo.mSoldierList[i].mSoldier.uId,
                            Get_OtherIndex, ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcEnemy, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
                }
                CounterTool tmpCounter = new CounterTool();
                tmpCounter.AddCount(_EnemyInfo.mSoldierList[i].mNum);
                _Counter_Enemy.Add(_EnemyInfo.mSoldierList[i].mSoldier.uId, tmpCounter);
            }
        }
    }

}
*/
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

/// <summary>
/// 远征战场
/// </summary>
public class FightScene_Expedition : FightSceneBase
{
    /// <summary>
    /// 路径数量
    /// </summary>
    private const byte ROLEPATH_MAX_COUNT = 7;
    /// <summary>
    /// 路径间距
    /// </summary>
    private const float ROLEPATH_DISTANCE = 20.0f;
    /// <summary>
    /// 左边界
    /// </summary>
    private const float LIMIT_LEFT = -516;
    /// <summary>
    /// 右边界
    /// </summary>
    private const float LIMIT_RIGHT = -1532;
    /// <summary>
    /// 敌方英雄死亡状态
    /// </summary>
    private const int ENEMY_HERO_STATUS_DEAD = 1;


    /// <summary>
    /// 自己基地 护送起始地点 传送门[配置表修改]
    /// </summary>
    [HideInInspector]
    public Transform trans_Barracks_Self;


    /// <summary>
    /// 士兵死亡计数器
    /// </summary>
    public Dictionary<ulong, int> _SoldierCount = new Dictionary<ulong, int>();
    public Dictionary<ulong, CounterTool> _Counter_Soldier = new Dictionary<ulong, CounterTool>();
    public Dictionary<ulong, CounterTool> _Counter_Enemy = new Dictionary<ulong, CounterTool>();
    /// <summary>
    /// 己方士兵信息
    /// </summary>
    public List<FightSoldierInfo> _SoldierList = new List<FightSoldierInfo>();
    /// <summary>
    /// 敌方玩家信息
    /// </summary>
    private FightPlayerInfo _EnemyInfo;
    /// <summary>
    /// 敌方角色计数器
    /// </summary>
    private CounterTool _EnemyCounter;
    /// <summary>
    /// 配置表数据
    /// </summary>
    private ExpeditionData expeditionInfo;
    /// <summary>
    /// 敌方英雄是否死亡
    /// </summary>
    private bool enemyHeroIsDeath;
    /// <summary>
    /// 角色资源总计数
    /// </summary>
    private CounterTool _TotalRoleCounter;
    /// <summary>
    /// 自己基地脚本
    /// </summary>
    private Barracks_Self _Barracks_Self;
    /// <summary>
    /// 结束城堡血量
    /// </summary>
    private int _FinalBarracksHP;

    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        pathIndex_Hero = 3;
        base.Initialize(vInfo);
        if (sceneInfo != null)
            expeditionInfo = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(sceneInfo.sceneID);
        InitSceneStatus();
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        if (_Barracks_Self != null)
            _Barracks_Self.ReSet();
        base.Uninitialize();
    }

    protected override void SetLocalUseComponentsAndDatas()
    {
        Transform tmpLocalTrans = this.transform;
        sceneCamera = tmpLocalTrans.FindChild("Camera").gameObject.GetComponent<Camera>();
        shakeCamera = tmpLocalTrans.FindChild("CameraShake").gameObject.GetComponent<Camera>();
        transContent = tmpLocalTrans.FindChild("Camera/SceneContent");
        transOther = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other").gameObject.GetComponent<Transform>();
        transHero = tmpLocalTrans.FindChild("Camera/SceneContent/Pos_Hero").gameObject.GetComponent<Transform>();
        transSelf = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Self").gameObject.GetComponent<Transform>();
        transEnemy = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Enemy").gameObject.GetComponent<Transform>();
        trans_Barracks_Self = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Barracks_Self").gameObject.GetComponent<Transform>();
        fightBackGround = transOther.FindChild("Tex_BackGround").gameObject.GetComponent<UITexture>();
        fightBackGround.gameObject.SetActive(false);
        limitLeft = LIMIT_LEFT;
        limitRight = LIMIT_RIGHT;

        if (trans_Barracks_Self != null)
        {
            _Barracks_Self = trans_Barracks_Self.gameObject.AddComponent<Barracks_Self>();
        }
        if (_Barracks_Self != null)
        {
            _Barracks_Self.SetLocalUseComponentsAndDatas();
        }
        MaxGroundWidth = 2048;

        base.SetLocalUseComponentsAndDatas();
    }

    protected override void SetFightSceneBackGround()
    {
        if (fightBackGround == null)
            return;
        fightBackGround.mainTexture = null;
        if (sceneInfo == null)
            return;

        for (int i = 0; i < 2; i++)
        {
            if (ListTexture.Count <= i)
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightBackGround.gameObject, fightBackGround.transform.parent);
                if (tmpObj == null)
                    continue;
                ListTexture.Add(tmpObj.GetComponent<UITexture>());
            }
        }
        for (int i = 0; i < ListTexture.Count; i++)
        {
            if (ListTexture[i] == null)
                continue;
            ListTexture[i].transform.localPosition = new Vector3(i * 1024, 0, 0);
            ListTexture[i].transform.localScale = Vector3.one;

            string tmpName = string.Empty;
            if (i == 0)
                tmpName = string.Format("{0}_L.assetbundle", sceneInfo.sceneBackGround);
            else
                tmpName = string.Format("{0}_R.assetbundle", sceneInfo.sceneBackGround);

            ResourceLoadManager.Instance.LoadAloneImage(tmpName, (texture) =>
            {
                ListTexture[i].mainTexture = texture;
                ListTexture[i].MakePixelPerfect();
                ListTexture[i].gameObject.SetActive(true);
            });
        }
    }

    protected override void PreLoadResource()
    {
        base.PreLoadResource();
        _TotalRoleCounter.AddCount();
        CheckResourceIsReady(null);
        if (_EnemyInfo == null || _EnemyInfo.mSoldierList == null)
            return;

        for (int i = 0; i < _EnemyInfo.mSoldierList.Count; ++i)
        {
            FightSoldierInfo tmpInfo = _EnemyInfo.mSoldierList[i];
            if (tmpInfo == null)
                continue;

            foreach (Skill tmpSkill in tmpInfo.mSoldier._skillsDepot._skillsList)
            {
                if (tmpSkill == null)
                    continue;

                tmpSkill.CacheEffect();
            }
        }
    }

    protected override void AddRolePath()
    {
        for (int i = 0; i < ROLEPATH_MAX_COUNT; i++)
            AddSingleRolePath(-(i * ROLEPATH_DISTANCE));
    }

    /// <summary>
    /// 战斗结束判断
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FinishOperate(object vDataObj)
    {
        if (_SceneStatus == ESceneStatus.essNormal)
        {
            FightDestroyInfo tmpInfo = (FightDestroyInfo)vDataObj;
            if (tmpInfo != null)
            {
                switch (tmpInfo._DestroyStatus)
                {
                    case EPVEFinishStatus.epvefsDieHero:
                        {
                            Operate_Failure();
                            fightFailReason = EPVEFinishStatus.epvefsDieHero;
                        }
                        break;
                    case EPVEFinishStatus.epvefsDieSoldier:
                        {
                            ulong tmpUID = (ulong)tmpInfo._DestroyID;
                            if (!_Counter_Soldier.ContainsKey(tmpUID))
                                return;
                            _Counter_Soldier[tmpUID].AddCount();
                        }
                        break;
                    case EPVEFinishStatus.epvefsDisEnemyHero:
                        {
                            enemyHeroIsDeath = true;
                            CheckIsVictory();
                        }
                        break;
                    case EPVEFinishStatus.epvefsDisEnemySoldier:
                        {
                            ulong tmpUID = (ulong)tmpInfo._DestroyID;
                            if (!_Counter_Enemy.ContainsKey(tmpUID))
                                return;
                            _Counter_Enemy[tmpUID].DelCount();
                            CheckIsVictory();
                        }
                        break;
                    case EPVEFinishStatus.epvefsOutTime:
                        {
                            Operate_Failure();
                            fightFailReason = EPVEFinishStatus.epvefsOutTime;
                        }
                        break;
                    case EPVEFinishStatus.epvefsDieBarracksSelf:
                        {//己方城堡死亡//
                            Operate_Failure();
                            fightFailReason = EPVEFinishStatus.epvefsDieBarracksSelf;
                        }
                        break;
                    default:
                        { }
                        break;
                }
            }
        }
    }
    private void SetEnemySoldiersInfo()
    {
        _FinalBarracksHP = 0;
        if (_Barracks_Self != null)
        {
            _FinalBarracksHP = _Barracks_Self.Get_FightAttribute.HP;
        }
        if (_Counter_Enemy == null)
            return;
        if ((PlayerData.Instance._ExpeditionInfo.enemies != null) && (PlayerData.Instance._ExpeditionInfo.enemies.soldiers != null))
        {
            for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.enemies.soldiers.Count; i++)
            {
                if (!_Counter_Enemy.ContainsKey(PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i].soldier.uid))
                    continue;
                PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i].num = _Counter_Enemy[PlayerData.Instance._ExpeditionInfo.enemies.soldiers[i].soldier.uid].Get_Count;
            }
        }
    }


    /// <summary>
    /// 检测是否胜利
    /// </summary>
    private void CheckIsVictory()
    {
        if (!enemyHeroIsDeath)
            return;
        foreach (KeyValuePair<ulong, CounterTool> tmpInfo in _Counter_Enemy)
        {
            if (tmpInfo.Value.Get_Count > 0)
            {
                return;
            }
        }
        Operate_Victory();
    }

    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        List<SoldierList> tmpSoldierList = new List<SoldierList>();
        foreach (KeyValuePair<ulong, CounterTool> tmpInfo in _Counter_Soldier)
        {
            if (tmpInfo.Value.Get_Count == 0)
                continue;
            SoldierList tmpData = new SoldierList();
            tmpData.uid = tmpInfo.Key;
            tmpData.num = tmpInfo.Value.Get_Count;
            tmpSoldierList.Add(tmpData);
        }

        if (enemyHeroIsDeath)
            PlayerData.Instance._ExpeditionInfo.enemies.hero = null;
        SetEnemySoldiersInfo();
        FightRelatedModule.Instance.SendExpeditionResult(1, tmpSoldierList, PlayerData.Instance._ExpeditionInfo.enemies, _FinalBarracksHP);
        _SceneStatus = ESceneStatus.essVictory;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        List<SoldierList> tmpSoldierList = new List<SoldierList>();
        foreach (KeyValuePair<ulong, CounterTool> tmpInfo in _Counter_Soldier)
        {
            SoldierList tmpData = new SoldierList();
            tmpData.uid = tmpInfo.Key;
            if (_SoldierCount.ContainsKey(tmpInfo.Key))
                tmpData.num = _SoldierCount[tmpInfo.Key];
            else
                tmpData.num = 0;
            tmpSoldierList.Add(tmpData);
        }

        if (enemyHeroIsDeath)
            PlayerData.Instance._ExpeditionInfo.enemies.hero = null;
        SetEnemySoldiersInfo();
        FightRelatedModule.Instance.SendExpeditionResult(2, tmpSoldierList, PlayerData.Instance._ExpeditionInfo.enemies, _FinalBarracksHP);
        _SceneStatus = ESceneStatus.essFailure;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        return true;
    }

    protected override void CheckResourceIsReady(RoleBase vRole)
    {
        _TotalRoleCounter.DelCount();
        if (_TotalRoleCounter.Get_Count > 0)
            return;
        base.CheckResourceIsReady(vRole);
    }


    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitSceneStatus()
    {
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;
        SceneManager.Instance.DicSceneMarkObj.Clear();
        _DicMarkObj.Clear();

        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        //物件重置//
        if (_Barracks_Self != null)
            _Barracks_Self.ReSet();
        //清空角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //设置城堡//
        if (_Barracks_Self != null)
        {
            _Barracks_Self.SetBarracksShowStatus(expeditionInfo.castletype);
            ShowInfoBase tmpAttributeSelf = new ShowInfoBase();
            tmpAttributeSelf.CopyTo(FightRelatedModule.Instance.mCastleInfo.mAttribute);
            _Barracks_Self.InitRoleAttribute(tmpAttributeSelf);
            _DicMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
            SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
        }

        //获取怪物刷新表//
        GetFightDatas();
        PreLoadResource();

        //开启刷怪//
        _SceneStatus = ESceneStatus.essNormal;
        CreateSoldier();
        CreateEnemy();
    }

    /// <summary>
    /// 获取战斗数据
    /// </summary>
    private void GetFightDatas()
    {
        if (PlayerData.Instance._ExpeditionInfo == null)
            return;

        //设置己方士兵信息//
        if (_SoldierList == null)
            _SoldierList = new List<FightSoldierInfo>();
        _SoldierList.Clear();
        if (UISystem.Instance.FightView.listSoldierInfo != null)
            _SoldierList.AddRange(UISystem.Instance.FightView.listSoldierInfo);

        //设置敌方信息//
        if (_EnemyInfo == null)
            _EnemyInfo = new FightPlayerInfo(PlayerData.Instance._ExpeditionInfo.enemies);
        else
            _EnemyInfo.ReSetInfo(PlayerData.Instance._ExpeditionInfo.enemies);

        GetTotalRoleCounter();
    }

    /// <summary>
    /// 统计角色总数
    /// </summary>
    private void GetTotalRoleCounter()
    {
        _TotalRoleCounter = new CounterTool();
        _TotalRoleCounter.AddCount();
        if (_SoldierList != null)
        {
            for (int i = 0; i < _SoldierList.Count; i++)
            {
                _TotalRoleCounter.AddCount(_SoldierList[i].mNum);
            }
        }

        if ((PlayerData.Instance._ExpeditionInfo.enemies.hero != null) && (PlayerData.Instance._ExpeditionInfo.enemies.hero.is_dead != ENEMY_HERO_STATUS_DEAD))
            _TotalRoleCounter.AddCount();
        if (_EnemyInfo.mSoldierList != null)
        {
            for (int i = 0; i < _EnemyInfo.mSoldierList.Count; i++)
            {
                _TotalRoleCounter.AddCount(_EnemyInfo.mSoldierList[i].mNum);
            }
        }
    }

    /// <summary>
    /// 创建士兵
    /// </summary>
    private void CreateSoldier()
    {
        //创建英雄角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
                (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, EFightType.eftExpedition, 0, null, CheckResourceIsReady));

        if (_SoldierList == null)
            return;

        //清空计数器//
        _Counter_Soldier.Clear();
        _SoldierCount.Clear();

        for (int i = 0; i < _SoldierList.Count; i++)
        {
            if (_SoldierList[i] == null)
                continue;
            for (int j = 0; j < _SoldierList[i].mNum; j++)
            {
                if (_SoldierList[i].mSoldier == null)
                    continue;
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                    new CData_CreateRole(_SoldierList[i].mSoldier.showInfoSoldier, _SoldierList[i].mSoldier.uId, Get_OtherIndex,
                        ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcSelf, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
            }
            //设置计数器//
            if (!_Counter_Soldier.ContainsKey(_SoldierList[i].mSoldier.uId))
            {
                _Counter_Soldier.Add(_SoldierList[i].mSoldier.uId, new CounterTool());
            }
            if (!_SoldierCount.ContainsKey(_SoldierList[i].mSoldier.uId))
            {
                _SoldierCount.Add(_SoldierList[i].mSoldier.uId, _SoldierList[i].mNum);
            }
        }
    }
    /// <summary>
    /// 创建敌人
    /// </summary>
    private void CreateEnemy()
    {
        if (_SceneStatus != ESceneStatus.essNormal)
            return;
        if (_EnemyInfo == null)
            return;

        enemyHeroIsDeath = true;
        if ((PlayerData.Instance._ExpeditionInfo.enemies.hero != null) && (PlayerData.Instance._ExpeditionInfo.enemies.hero.is_dead != ENEMY_HERO_STATUS_DEAD))
        {
            if (_EnemyInfo.mAttribute != null)
            {
                _EnemyInfo.mAttribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(_EnemyInfo.mAttribute.AttRate);
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                    new CData_CreateRole(_EnemyInfo.mAttribute, 0, pathIndex_Hero, ERoleType.ertHero,
                        (EHeroGender)_EnemyInfo.mGender, EFightCamp.efcEnemy, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
                enemyHeroIsDeath = false;
            }
        }

        _Counter_Enemy.Clear();
        if (_EnemyInfo.mSoldierList != null)
        {
            for (int i = 0; i < _EnemyInfo.mSoldierList.Count; i++)
            {
                if (_EnemyInfo.mSoldierList[i] == null)
                    continue;
                for (int j = 0; j < _EnemyInfo.mSoldierList[i].mNum; j++)
                {
                    if (_EnemyInfo.mSoldierList[i].mSoldier == null)
                        continue;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        new CData_CreateRole(_EnemyInfo.mSoldierList[i].mSoldier.showInfoSoldier, _EnemyInfo.mSoldierList[i].mSoldier.uId,
                            Get_OtherIndex, ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcEnemy, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
                }
                CounterTool tmpCounter = new CounterTool();
                tmpCounter.AddCount(_EnemyInfo.mSoldierList[i].mNum);
                _Counter_Enemy.Add(_EnemyInfo.mSoldierList[i].mSoldier.uId, tmpCounter);
            }
        }
    }

}
