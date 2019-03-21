using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 主线剧情/兵伐不臣战场
/// </summary>
public class FightScene_Activity : FightSceneBase
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
    /// 自己基地 护送起始地点 传送门[配置表修改]
    /// </summary>
    [HideInInspector]
    public Transform trans_Barracks_Self;
    /// <summary>
    /// 地方基地 护送完成地点[配置表修改]
    /// </summary>
    [HideInInspector]
    public Transform trans_Barracks_Enemy;



    /// <summary>
    /// 关卡怪物列表
    /// </summary>
    private List<RefreshMonsterInfo> _StageMonsterList = new List<RefreshMonsterInfo>();
    private Dictionary<int, List<RefreshMonsterInfo>> stageMonsterDic = new Dictionary<int, List<RefreshMonsterInfo>>();
    /// <summary>
    /// 进攻模式敌方城堡血量对应刷怪Key值
    /// </summary>
    private List<int> listSpecialEnemy = new List<int>();
    /// <summary>
    /// 关卡怪物种类信息
    /// </summary>
    private Dictionary<uint, ShowInfoBase> _StageMonsterInfoDic = new Dictionary<uint, ShowInfoBase>();
    /// <summary>
    /// 怪物配置表引用
    /// </summary>
    private MonsterAttributeConfig pMonsterAttributeConfig;
    /// <summary>
    /// 场景怪物计数器
    /// </summary>
    private CounterTool _Counter_Monster;
    /// <summary>
    /// 通过传送点怪物计数器
    /// </summary>
    private CounterTool _Counter_TransMonster;
    /// <summary>
    /// 结束数值
    /// </summary>
    private int _FightFinishValue;
    /// <summary>
    /// 上次怪物刷新时间[秒]
    /// </summary>
    private float _PreRefreshMonsterTime;
    /// <summary>
    /// 刷怪时间间隔[秒]
    /// </summary>
    private float _DisRefreshMonsterTime;
    /// <summary>
    /// 传送门
    /// </summary>
    private Transfer _TransferObj;
    /// <summary>
    /// 自己基地脚本
    /// </summary>
    private Barracks_Self _Barracks_Self;
    /// <summary>
    /// 敌方基地脚本
    /// </summary>
    private Barracks_Enemy _Barracks_Enemy;
    /// <summary>
    /// 关卡数据
    /// </summary>
    private StageInfo _StageInfo;
    /// <summary>
    /// 护送目标
    /// </summary>
    private RoleAttribute escortAttribute;
    /// <summary>
    /// 标志物件血量
    /// </summary>
    private int markObjInitHP = 0;



    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SceneParse);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SceneResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Enemy, CommandEvent_ExchangeInfo_Enemy);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartShowBossTalk, CommandEvent_StartShowBossTalk);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);

        _TransferObj = null;
        pathIndex_Hero = 3;

        base.Initialize(vInfo);
        if (sceneInfo != null)
            _StageInfo = ConfigManager.Instance.mStageData.GetInfoByID(sceneInfo.sceneID);

        if (sceneInfo.sceneType == EFightType.eftMain)
            TalkingDataManager.Instance.OnBegin(sceneInfo.sceneID.ToString());

        pMonsterAttributeConfig = ConfigManager.Instance.mMonsterData;
        InitSceneStatus();
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SceneParse);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SceneResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Enemy, CommandEvent_ExchangeInfo_Enemy);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartShowBossTalk, CommandEvent_StartShowBossTalk);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
       if (this._TransferObj != null)
            this._TransferObj = null;
        if (_Barracks_Self != null)
            _Barracks_Self.ReSet();
        if (_Barracks_Enemy != null)
            _Barracks_Enemy.ReSet();
        CancelInvoke("CreateEnemyOperate");
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
        trans_Barracks_Enemy = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Barracks_Enemy").gameObject.GetComponent<Transform>();

        fightBackGround = transOther.FindChild("Tex_BackGround").gameObject.GetComponent<UITexture>();
        fightBackGround.gameObject.SetActive(false);
        limitLeft = LIMIT_LEFT;
        limitRight = LIMIT_RIGHT;
        
        _Barracks_Self = trans_Barracks_Self.gameObject.AddComponent<Barracks_Self>();
        _Barracks_Enemy = trans_Barracks_Enemy.gameObject.AddComponent<Barracks_Enemy>();
        _Barracks_Self.SetLocalUseComponentsAndDatas();
        _Barracks_Enemy.SetLocalUseComponentsAndDatas();
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
        if (pMonsterAttributeConfig == null)
            return;
        List<uint> tmpListID = new List<uint>();
        if (_StageMonsterList != null)
        {
            int tmpListCount = _StageMonsterList.Count;
            for (int i = 0; i < tmpListCount; i++)
            {
                if (_StageMonsterList[i] == null)
                    continue;
                if (_StageMonsterList[i].ArrMonsterID == null)
                    continue;
                int tmpArrLength = _StageMonsterList[i].ArrMonsterID.Length;
                for (int j = 0; j < tmpArrLength; j++)
                {
                    uint tmpMonsterID = _StageMonsterList[i].ArrMonsterID[j];
                    if (tmpListID.Contains(tmpMonsterID))
                        continue;
                    tmpListID.Add(tmpMonsterID);
                    MonsterAttributeInfo tmpSingleMonsterData = pMonsterAttributeConfig.GetMonsterAttributeByID(tmpMonsterID);
                    if (tmpSingleMonsterData == null)
                        continue;
                    ResourceLoadManager.Instance.LoadCharacter(tmpSingleMonsterData.ResourceID, ResourceLoadType.AssetBundle, (obj) =>
                    {
                        if (obj != null)
                        {
                            AloneObjectCache.Instance.LoadGameObject(obj, (go) =>
                            {
                                AloneObjectCache.Instance.FreeObject(go);
                            }, tmpSingleMonsterData.ResourceID, 5);
                        }
                    });
                    if (!tmpSingleMonsterData.trajectory.Equals("0") && !tmpSingleMonsterData.trajectory.Equals(string.Empty))
                    {
                        EffectObjectCache.Instance.LoadGameObject(tmpSingleMonsterData.trajectory, (go) =>
                        {
                            EffectObjectCache.Instance.FreeObject(go);
                        }, tmpSingleMonsterData.trajectory, 10);
                    }
                    if (tmpSingleMonsterData.Skill != null)
                    {
                        foreach (uint id in tmpSingleMonsterData.Skill)
                        {
                            Skill tmpSkill = Skill.createByID(id);
                            if (tmpSkill != null)
                                tmpSkill.CacheEffect();
                        }
                    }
                }
            }
        }
        if (stageMonsterDic != null)
        {
            foreach (KeyValuePair<int, List<RefreshMonsterInfo>> tmpInfo in stageMonsterDic)
            {
                if (tmpInfo.Value == null)
                    continue;
                int tmpListCount = tmpInfo.Value.Count;
                for (int i = 0; i < tmpListCount; i++)
                {
                    if (tmpInfo.Value[i] == null)
                        continue;
                    if (tmpInfo.Value[i].ArrMonsterID == null)
                        continue;
                    int tmpArrLength = tmpInfo.Value[i].ArrMonsterID.Length;
                    for (int j = 0; j < tmpArrLength; j++)
                    {
                        uint tmpMonsterID = tmpInfo.Value[i].ArrMonsterID[j];
                        if (tmpListID.Contains(tmpMonsterID))
                            continue;
                        tmpListID.Add(tmpMonsterID);
                        MonsterAttributeInfo tmpSingleMonsterData = pMonsterAttributeConfig.GetMonsterAttributeByID(tmpMonsterID);
                        if (tmpSingleMonsterData == null)
                            continue;
                        ResourceLoadManager.Instance.LoadCharacter(tmpSingleMonsterData.ResourceID, ResourceLoadType.AssetBundle, (obj) =>
                        {
                            if (obj != null)
                            {
                                AloneObjectCache.Instance.LoadGameObject(obj, (go) =>
                                {
                                    AloneObjectCache.Instance.FreeObject(go);
                                }, tmpSingleMonsterData.ResourceID, 5);
                            }
                        });
                        if (tmpSingleMonsterData.Skill != null)
                        {
                            foreach (uint id in tmpSingleMonsterData.Skill)
                            {
                                Skill tmpSkill = Skill.createByID(id);
                                if (tmpSkill != null)
                                    tmpSkill.CacheEffect();
                            }
                        }
                    }
                }
            }
        }
    }
    
    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        _SceneStatus = ESceneStatus.essVictory;
        SetFightFinishValue();
        CancelInvoke("CreateEnemyOperate");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, _FightFinishValue));
        FightRelatedModule.Instance.SendDungeonReward((int)_SceneStatus, sceneInfo.sceneID, _FightFinishValue);
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        _SceneStatus = ESceneStatus.essFailure;
        SetFightFinishValue();
        CancelInvoke("CreateEnemyOperate");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, _FightFinishValue));
        FightRelatedModule.Instance.SendDungeonReward((int)_SceneStatus, sceneInfo.sceneID, _FightFinishValue);
        return true;
    }

    protected override void AddRolePath()
    {
        for (int i = 0; i < ROLEPATH_MAX_COUNT; i++)
            AddSingleRolePath(-(i * ROLEPATH_DISTANCE));
    }

    public override void ChangeFightSpeedOperate(float vFightSpeed)
    {
        base.ChangeFightSpeedOperate(vFightSpeed);
        if (_StageMonsterList.Count <= 0)
            return;
        //删除原有怪物刷新方法//
        CancelInvoke("CreateEnemyOperate");
        //修改怪物刷新时间//
        _DisRefreshMonsterTime = _DisRefreshMonsterTime - (Time.time - _PreRefreshMonsterTime) * vFightSpeed;
        _PreRefreshMonsterTime = Time.time;
        //重新开始刷怪//
        Invoke("CreateEnemyOperate", _DisRefreshMonsterTime / vFightSpeed);
    }


    /// <summary>
    /// 战斗结束判断
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FinishOperate(object vDataObj)
    {
        FightDestroyInfo tmpInfo = (FightDestroyInfo)vDataObj;
        if (tmpInfo == null)
            return;

        switch (tmpInfo._DestroyStatus)
        {
            case EPVEFinishStatus.epvefsDieHero:
                {//己方英雄死亡//
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsDieHero;
                }
                break;
            case EPVEFinishStatus.epvefsDieBarracksSelf:
                {//己方城堡死亡//
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsDieBarracksSelf;
                }
                break;
            case EPVEFinishStatus.epvefsDieBarracksEnemy:
                {//敌方城堡死亡//
                    Operate_Victory();
                }
                break;
            case EPVEFinishStatus.epvefsDieEscort:
                {//护送目标死亡//
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsDieEscort;
                }
                break;
            case EPVEFinishStatus.epvefsOutEscort:
                {//护送目标达到关卡//
                    //escortAttribute = (RoleAttribute)tmpInfo._DestroyID;
                    Operate_Victory();
                }
                break;
            case EPVEFinishStatus.epvefsDieTransfer:
                {//敌方士兵通过传送阵//
                    if (_Counter_Monster != null)
                    {
                        _Counter_Monster.DelCount();
                    }
                    if (_Counter_TransMonster == null)
                    {
                        if (_Counter_Monster.Get_Count <= 0)
                            Operate_Victory();
                        return;
                    }
                    _TransferObj.ChangeTransferCount();
                    _Counter_TransMonster.DelCount();
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Self, _Counter_TransMonster.Get_Count);
                    if (_Counter_TransMonster.Get_Count > 0)
                    {
                        if (_Counter_Monster.Get_Count <= 0)
                            Operate_Victory();
                        return;
                    }
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsDieTransfer;
                }
                break;
            case EPVEFinishStatus.epvefsDieMonster:
            case EPVEFinishStatus.epvefsDisBoss:
                {//敌方士兵死亡//
                    if (_StageInfo.FireType != (int)EPVESceneType.epvestDefen && _StageInfo.FireType != (int)EPVESceneType.epvestTransfer)
                        return;
                    if (_Counter_Monster == null)
                        return;
                    _Counter_Monster.DelCount();
                    if (_Counter_Monster.Get_Count > 0)
                        return;
                    Operate_Victory();
                }
                break;
            case EPVEFinishStatus.epvefsOutTime:
                {//结算时间到//
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsOutTime;
                }
                break;
            case EPVEFinishStatus.epvefsDieSoldier:
                { }
                break;
        }
    }

    /// <summary>
    /// 暂停战斗
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SceneParse(object vDataObj)
    {
        CancelInvoke("CreateEnemyOperate");
        if (_StageMonsterList.Count <= 0)
            return;
        _DisRefreshMonsterTime = _DisRefreshMonsterTime - (Time.time - _PreRefreshMonsterTime) * SceneManager.Instance.Get_FightSpeed;
    }

    /// <summary>
    /// 继续战斗
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SceneResume(object vDataObj)
    {
        _PreRefreshMonsterTime = Time.time;
        Invoke("CreateEnemyOperate", _DisRefreshMonsterTime / SceneManager.Instance.Get_FightSpeed);
    }

    /// <summary>
    /// 修改敌方城堡血量
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ExchangeInfo_Enemy(object vDataObj)
    {
        if (vDataObj == null)
            return;
        int tmpCurValue = (int)vDataObj;
        if (tmpCurValue <= 0)
        {
            tmpCurValue = 0;
            return;
        }
        int tmpValue = tmpCurValue * 100 / markObjInitHP;
        if (stageMonsterDic == null)
            return;
        if (stageMonsterDic.Count <= 0)
            return;
        foreach (KeyValuePair<int, List<RefreshMonsterInfo>> tmpSingleInfo in stageMonsterDic)
        {
            if (tmpSingleInfo.Key < tmpValue)
                continue;
            if (listSpecialEnemy.Contains(tmpSingleInfo.Key))
                continue;
            if (tmpSingleInfo.Value == null)
                continue;
            listSpecialEnemy.Add(tmpSingleInfo.Key);

            for (int i = 0; i < tmpSingleInfo.Value.Count; i++)
            {
                if (tmpSingleInfo.Value[i] == null)
                    continue;
                for (int j = 0; j < tmpSingleInfo.Value[i].ArrMonsterID.Length; j++)
                {
                    uint tmpMonsterID = tmpSingleInfo.Value[i].ArrMonsterID[j];

                    //检测是否存在ID对应数据//
                    if (!_StageMonsterInfoDic.ContainsKey(tmpMonsterID))
                    {
                        if (pMonsterAttributeConfig == null)
                            continue;
                        //获取对应怪物数据//
                        MonsterAttributeInfo tmpSingleMonsterData = pMonsterAttributeConfig.GetMonsterAttributeByID(tmpMonsterID);
                        if (tmpSingleMonsterData == null)
                            continue;
                        //设置怪物信息//
                        ShowInfoBase tmpInfo = new ShowInfoBase();
                        tmpInfo.ReSetFightAttribute(tmpMonsterID, tmpSingleMonsterData.HP, 0, tmpSingleMonsterData.Attack, CommonFunction.GetSecondTimeByMilliSecond(tmpSingleMonsterData.AttRate),
                            tmpSingleMonsterData.AttDistance, tmpSingleMonsterData.Accuracy, tmpSingleMonsterData.Crit, tmpSingleMonsterData.Dodge, tmpSingleMonsterData.Tenacity, tmpSingleMonsterData.Speed);
                        //添加怪物数据//
                        _StageMonsterInfoDic.Add(tmpMonsterID, tmpInfo);
                    }
                    //通知创建怪物//
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        new CData_CreateRole(_StageMonsterInfoDic[tmpMonsterID], 0, Get_OtherIndex,
                            ERoleType.ertMonster, EHeroGender.ehgNone, EFightCamp.efcEnemy,
                            sceneInfo.sceneType, tmpSingleInfo.Value[i].initPosX));
                }
            }
        }

        //删除已使用数据//
        for (int i = 0; i < listSpecialEnemy.Count; i++)
        {
            if (stageMonsterDic.ContainsKey(listSpecialEnemy[i]))
                stageMonsterDic.Remove(listSpecialEnemy[i]);
        }
    }

    /// <summary>
    /// BOSS出场
    /// </summary>
    private void CommandEvent_BossInScene(object vDataObj)
    {
        initPos = transContent.localPosition;
    }

    /// <summary>
    /// 开启摄像机移动
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartShowBossTalk(object vDataObj)
    {
        if (vDataObj == null)
            return;
        TalkManager.Instance.ReStartTalk(_StageInfo.ID, (EChatType)vDataObj);
    }
    /// <summary>
    /// 开始显示战斗
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartShowFightStatus(object vDataObj)
    {
        _StageMonsterInfoDic.Clear();
        if (_StageMonsterList.Count <= 0)
            return;
        _DisRefreshMonsterTime = _StageMonsterList[0].RefreshTime;
        _PreRefreshMonsterTime = Time.time;
        Invoke("CreateEnemyOperate", _StageMonsterList[0].RefreshTime);
    }



    /// <summary>
    /// 初始化设置
    /// </summary>
    private void InitSceneStatus()
    {
        if (_StageInfo == null)
            return;
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;

        //关闭怪物刷新//
        CancelInvoke("CreateEnemyOperate");
        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        //物件重置//
        if (_Barracks_Self != null)
            _Barracks_Self.ReSet();
        if (_Barracks_Enemy != null)
            _Barracks_Enemy.ReSet();
        if (_TransferObj != null)
        {
            GameObject.Destroy(_TransferObj.gameObject);
            _TransferObj = null;
        }
        //角色重置//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //创建英雄角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
                (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, sceneInfo.sceneType, 0, null, CheckResourceIsReady));

        //获取怪物刷新表//
        _StageMonsterList.Clear();
        stageMonsterDic.Clear();
        SceneManager.Instance.GetStageMonsterInfo(ConfigManager.Instance.mStageMonsterData.FindByID(_StageInfo.ID), out _StageMonsterList, out stageMonsterDic);

        //设置计数器//
        if (_Counter_Monster == null)
            _Counter_Monster = new CounterTool();
        else
            _Counter_Monster.ReInitCounter();
        for (int i = 0; i < _StageMonsterList.Count; i++)
            _Counter_Monster.AddCount(_StageMonsterList[i].ArrMonsterID.Length);
        foreach (KeyValuePair<int, List<RefreshMonsterInfo>> tmpInfo in stageMonsterDic)
        {
            for (int i = 0; i < tmpInfo.Value.Count; i++)
                _Counter_Monster.AddCount(tmpInfo.Value[i].ArrMonsterID.Length);
        }

        if (_Counter_TransMonster == null)
            _Counter_TransMonster = new CounterTool();
        else
            _Counter_TransMonster.ReInitCounter();
        _Counter_TransMonster.AddCount(_StageInfo.MaxEnemyCount);

        SetSceneMarkObj();

        //开启刷怪//
        _SceneStatus = ESceneStatus.essNormal;
        PreLoadResource();
    }
    
    /// <summary>
    /// 设置结算最终数值
    /// </summary>
    private void SetFightFinishValue()
    {
        _FightFinishValue = 0;
        switch ((EPVESceneType)_StageInfo.FireType)
        {
            case EPVESceneType.epvestAttack:
                {
                    _FightFinishValue = UISystem.Instance.FightView.GetFightDisTime();
                }
                break;
            case EPVESceneType.epvestDefen:
                {
                    if (_Barracks_Self == null)
                        return;
                    _FightFinishValue = _Barracks_Self.Get_FightAttribute.HP * 100 / markObjInitHP;
                }
                break;
            case EPVESceneType.epvestEscort:
                {
                    if (escortAttribute == null)
                        return;
                    _FightFinishValue = escortAttribute.Get_FightAttribute.HP * 100 / markObjInitHP;
                }
                break;
            case EPVESceneType.epvestTransfer:
                {
                    _FightFinishValue = (markObjInitHP - _Counter_TransMonster.Get_Count);
                }
                break;
            default:
                { }
                break;
        }
    }
    
    /// <summary>
    /// 设置场景标志物件
    /// </summary>
    private void SetSceneMarkObj()
    {
        if (_StageInfo == null)
            return;
        SceneManager.Instance.DicSceneMarkObj.Clear();
        _DicMarkObj.Clear();

        if (_Barracks_Enemy != null)
        {
            if ((_StageInfo.PlayType == (int)EFightType.eftActivity) && (_StageInfo.FireType != (int)EPVESceneType.epvestAttack))
            { }
            else
            {
                _Barracks_Enemy.SetBarracksTexture(_StageInfo.CastleType, _StageInfo.EnemyBarracksPic);
                ShowInfoBase tmpAttributeEnemy = new ShowInfoBase();
                tmpAttributeEnemy.HP = _StageInfo.EnemyBaseHP;
                _Barracks_Enemy.InitRoleAttribute(tmpAttributeEnemy);
                _DicMarkObj.Add(ESceneMarkType.esmtBarracks_Enemy, _Barracks_Enemy);
                SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtBarracks_Enemy, _Barracks_Enemy);
                markObjInitHP = tmpAttributeEnemy.HP;
                SceneManager.Instance.Get_EnemyBarracksPosX = _Barracks_Enemy.transform.localPosition.x;
            }
        }
        
        switch ((EPVESceneType)_StageInfo.FireType)
        {
            case EPVESceneType.epvestAttack://进攻//
                {
                    if (_Barracks_Self != null)
                    {
                        _Barracks_Self.SetBarracksShowStatus(_StageInfo.CastleType);
                        ShowInfoBase tmpAttributeSelf = new ShowInfoBase();
                        tmpAttributeSelf.CopyTo(PlayerData.Instance.mCastleInfo.mAttribute);
                        tmpAttributeSelf.HP += _StageInfo.OwnBaseHP;
                        _Barracks_Self.InitRoleAttribute(tmpAttributeSelf);
                        _DicMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
                        SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
                    }
                    //if (_Barracks_Enemy != null)
                    //{
                    //    _Barracks_Enemy.SetBarracksTexture(_StageInfo.CastleType, _StageInfo.EscortTarget);
                    //    ShowInfoBase tmpAttributeEnemy = new ShowInfoBase();
                    //    tmpAttributeEnemy.HP = _StageInfo.EnemyBaseHP;
                    //    _Barracks_Enemy.InitRoleAttribute(tmpAttributeEnemy);
                    //    _DicMarkObj.Add(ESceneMarkType.esmtBarracks_Enemy, _Barracks_Enemy);
                    //    SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtBarracks_Enemy, _Barracks_Enemy);
                    //    markObjInitHP = tmpAttributeEnemy.HP;
                    //    SceneManager.Instance.Get_EnemyBarracksPosX = _Barracks_Enemy.transform.localPosition.x;
                    //}
                }
                break;
            case EPVESceneType.epvestDefen://防守//
                {
                    if (_Barracks_Self != null)
                    {
                        _Barracks_Self.SetBarracksShowStatus(_StageInfo.CastleType);
                        ShowInfoBase tmpAttributeSelf = new ShowInfoBase();
                        tmpAttributeSelf.CopyTo(PlayerData.Instance.mCastleInfo.mAttribute);
                        tmpAttributeSelf.HP += _StageInfo.OwnBaseHP;
                        markObjInitHP = tmpAttributeSelf.HP;
                        _Barracks_Self.InitRoleAttribute(tmpAttributeSelf);
                        _DicMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
                        SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtBarracks_Self, _Barracks_Self);
                    }
                }
                break;
            case EPVESceneType.epvestEscort://护送//
                {
                    ShowInfoBase tmpAttribute = new ShowInfoBase();
                    tmpAttribute.KeyData = _StageInfo.ID;
                    tmpAttribute.HP = _StageInfo.EscortTargetHP;
                    markObjInitHP = tmpAttribute.HP;
                    tmpAttribute.MoveSpeed = _StageInfo.EscortTargetSpeed;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        new CData_CreateRole(tmpAttribute, 0, 4, ERoleType.ertEscort, EHeroGender.ehgNone,
                            EFightCamp.efcSelf, sceneInfo.sceneType, 0, trans_Barracks_Self, role => { escortAttribute = role; }));

                    ResourceLoadManager.Instance.LoadEffect(GlobalConst.Effect.ESCORT_TERMINAL, (obj) =>
                    {
                        GameObject tmpObj = CommonFunction.InstantiateObject(obj, trans_Barracks_Self);
                        if (tmpObj != null)
                        {
                            _TransferObj = tmpObj.AddComponent<Transfer>();
                            _TransferObj.name = "EscortTerminal";
                            _DicMarkObj.Add(ESceneMarkType.esmtTransfer, _TransferObj);
                            _TransferObj.transform.localPosition = new Vector3(_StageInfo.MarkPos, 40, 0);
                        }
                    });
                }
                break;
            case EPVESceneType.epvestTransfer://传送//
                {
                    ResourceLoadManager.Instance.LoadEffect(GlobalConst.Effect.PORTAL, (obj) =>
                    {
                        GameObject tmpObj = CommonFunction.InstantiateObject(obj, transOther);
                        if (tmpObj != null)
                        {
                            _TransferObj = tmpObj.AddComponent<Transfer>();
                            _TransferObj.name = "TransferObj";
                            ShowInfoBase tmpInfo = new ShowInfoBase();
                            tmpInfo.HP = _StageInfo.MaxEnemyCount;
                            markObjInitHP = tmpInfo.HP;
                            _TransferObj.InitRoleAttribute(tmpInfo);
                            _TransferObj.ReSetFireColumn(_StageInfo.EscortTarget);
                            _DicMarkObj.Add(ESceneMarkType.esmtTransfer, _TransferObj);
                            SceneManager.Instance.DicSceneMarkObj.Add(ESceneMarkType.esmtTransfer, _TransferObj);
                            _TransferObj.gameObject.AddComponent<BoxCollider>();
                            _TransferObj.gameObject.GetComponent<BoxCollider>().size = new Vector3(100, 5000, 0);
                            _TransferObj.transform.localPosition = new Vector3(_StageInfo.MarkPos, -30, 0);
                            _TransferObj.gameObject.GetComponent<UISprite>().depth = 20;
                            SceneManager.Instance.Get_TransferPosX = _TransferObj.transform.localPosition.x;
                        }

                    });
                }
                break;
            default:
                {
                    _Barracks_Enemy = null;
                    return;
                }
                break;
        }
    }

    /// <summary>
    /// 创建敌人
    /// </summary>
    private void CreateEnemyOperate()
    {
        if (_SceneStatus != ESceneStatus.essNormal)
            return;
        if (_StageMonsterList.Count <= 0)
            return;
        if (_StageMonsterList[0] == null)
            return;
        if (_StageMonsterList[0].ArrMonsterID == null)
            return;
        _DisRefreshMonsterTime = 0;

        for (int i = 0; i < _StageMonsterList[0].ArrMonsterID.Length; i++)
        {
            uint tmpMonsterID = _StageMonsterList[0].ArrMonsterID[i];

            //检测是否存在ID对应数据//
            if (!_StageMonsterInfoDic.ContainsKey(tmpMonsterID))
            {
                if (pMonsterAttributeConfig == null)
                    continue;
                //获取对应怪物数据//
                MonsterAttributeInfo tmpSingleMonsterData = pMonsterAttributeConfig.GetMonsterAttributeByID(tmpMonsterID);
                if (tmpSingleMonsterData == null)
                    continue;
                //设置怪物信息//
                ShowInfoBase tmpInfo = new ShowInfoBase();
                tmpInfo.ReSetFightAttribute(tmpMonsterID, tmpSingleMonsterData.HP, 0, tmpSingleMonsterData.Attack, CommonFunction.GetSecondTimeByMilliSecond(tmpSingleMonsterData.AttRate),
                    tmpSingleMonsterData.AttDistance, tmpSingleMonsterData.Accuracy, tmpSingleMonsterData.Crit, tmpSingleMonsterData.Dodge, tmpSingleMonsterData.Tenacity, tmpSingleMonsterData.Speed);
                //添加怪物数据//
                _StageMonsterInfoDic.Add(tmpMonsterID, tmpInfo);
            }
            //通知创建怪物//
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                new CData_CreateRole(_StageMonsterInfoDic[tmpMonsterID], 0, Get_OtherIndex,
                    ERoleType.ertMonster, EHeroGender.ehgNone, EFightCamp.efcEnemy, 
                    sceneInfo.sceneType, _StageMonsterList[0].initPosX));
        }
        _PreRefreshMonsterTime = Time.time;
        //检测是否刷新完毕//
        if ((_StageMonsterList.Count > 1) && (_StageMonsterList[1] != null))
        {
            //设置下次刷新时间//
            _DisRefreshMonsterTime = _StageMonsterList[1].RefreshTime - _StageMonsterList[0].RefreshTime;
            Invoke("CreateEnemyOperate", _DisRefreshMonsterTime / SceneManager.Instance.Get_FightSpeed);
        }
        //删除当前怪物数据//
        _StageMonsterList.RemoveAt(0);
    }

}