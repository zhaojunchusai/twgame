using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using CodeStage.AntiCheat.ObscuredTypes;

/// <summary>
/// 攻城略地战场
/// </summary>
/// 结束条件：
/// 己方英雄死亡
/// 敌方全部死亡
/// 时间到
public class FightScene_CaptureTerritory : FightSceneBase
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
    /// 关卡怪物列表
    /// </summary>
    private List<RefreshMonsterInfo> _StageMonsterList = new List<RefreshMonsterInfo>();
    /// <summary>
    /// 关卡怪物种类信息
    /// </summary>
    private Dictionary<uint, ShowInfoBase> _StageMonsterInfoDic = new Dictionary<uint, ShowInfoBase>();
    /// <summary>
    /// 场景怪物计数器
    /// </summary>
    private CounterTool _Counter_Monster;
    /// <summary>
    /// 己方士兵信息
    /// </summary>
    public List<FightSoldierInfo> _SoldierList = new List<FightSoldierInfo>();
    /// <summary>
    /// 怪物配置表引用
    /// </summary>
    private MonsterAttributeConfig pMonsterAttributeConfig;
    /// <summary>
    /// 上次怪物刷新时间[秒]
    /// </summary>
    private float _PreRefreshMonsterTime;
    /// <summary>
    /// 刷怪时间间隔[秒]
    /// </summary>
    private float _DisRefreshMonsterTime;
    /// <summary>
    /// 伤害
    /// </summary>
    private ObscuredFloat _Damage;



    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SceneParse);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SceneResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, CommandEvent_ReSetPlayerScore);
        pathIndex_Hero = 3;
        _Damage = 0;
        base.Initialize(vInfo);
        pMonsterAttributeConfig = ConfigManager.Instance.mMonsterData;
        InitSceneStatus();
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SceneParse);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SceneResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, CommandEvent_ReSetPlayerScore);
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
        fightBackGround = transOther.FindChild("Tex_BackGround").gameObject.GetComponent<UITexture>();
        fightBackGround.gameObject.SetActive(false);
        limitLeft = LIMIT_LEFT;
        limitRight = LIMIT_RIGHT;
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
    }

    protected override void AddRolePath()
    {
        for (int i = 0; i < ROLEPATH_MAX_COUNT; i++)
        {
            AddSingleRolePath(-(i * ROLEPATH_DISTANCE));
        }
    }

    public override void ChangeFightSpeedOperate(float vFightSpeed)
    {
        base.ChangeFightSpeedOperate(vFightSpeed);
        if (_StageMonsterList.Count != 0)
        {
            //删除原有怪物刷新方法//
            CancelInvoke("CreateEnemyOperate");
            //修改怪物刷新时间//
            _DisRefreshMonsterTime = _DisRefreshMonsterTime - (Time.time - _PreRefreshMonsterTime) * vFightSpeed;
            _PreRefreshMonsterTime = Time.time;
            //重新开始刷怪//
            Invoke("CreateEnemyOperate", _DisRefreshMonsterTime / vFightSpeed);
        }
    }


    /// <summary>
    /// 战斗结束判断
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FinishOperate(object vDataObj)
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
                case EPVEFinishStatus.epvefsDieMonster:
                    {
                        if (_Counter_Monster != null)
                        {
                            _Counter_Monster.DelCount();
                            if (_Counter_Monster.Get_Count <= 0)
                            {
                                Operate_Victory();
                            }
                        }
                    }
                    break;
                case EPVEFinishStatus.epvefsOutTime:
                    {
                        Operate_Failure();
                        fightFailReason = EPVEFinishStatus.epvefsOutTime;
                    }
                    break;
                case EPVEFinishStatus.epvefsStop:
                    {
                        Operate_Failure();
                        fightFailReason = EPVEFinishStatus.epvefsStop;
                    }
                    break;
                default:
                    { }
                    break;
            }
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
    /// 开始刷怪
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
    /// 设置玩家积分
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetPlayerScore(object vDataObj)
    {
        if (vDataObj != null)
        {
            _Damage += (float)vDataObj;
            if (_Damage < 0)
            {
                _Damage = 0;
            }
        }
    }


    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        _SceneStatus = ESceneStatus.essVictory;
        CancelInvoke("CreateEnemyOperate");
        ObscuredInt tmpScore = (int)((_Damage * ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor) * (1 + CaptureTerritoryModule.Instance.GetCurrentTokenIncrease()));
//        Debug.LogError(string.Format("-----------------------[{0}, {1}, {2}]", _Damage, ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor, CaptureTerritoryModule.Instance.GetCurrentTokenIncrease()));
        ObscuredInt tmpDamage = (int)_Damage;
        FightRelatedModule.Instance.SendCaptureTerritoryReq(tmpScore, tmpDamage);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(ESceneStatus.essVictory, 0));
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        _SceneStatus = ESceneStatus.essFailure;
        CancelInvoke("CreateEnemyOperate");
        ObscuredInt tmpScore = (int)((_Damage * ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor) * (1 + CaptureTerritoryModule.Instance.GetCurrentTokenIncrease()));
//        Debug.LogError(string.Format("-----------------------[{0}, {1}, {2}]", _Damage, ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor, CaptureTerritoryModule.Instance.GetCurrentTokenIncrease()));
        ObscuredInt tmpDamage = (int)_Damage;
        FightRelatedModule.Instance.SendCaptureTerritoryReq(tmpScore, tmpDamage);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(ESceneStatus.essFailure, 0));
        return true;
    }

    protected override void CheckResourceIsReady(RoleBase vRole)
    {
        base.CheckResourceIsReady(vRole);
    }


    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitSceneStatus()
    {
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;

        //关闭怪物刷新//
        CancelInvoke("CreateEnemyOperate");
        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        //清空角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //获取怪物刷新表//
        _StageMonsterList.Clear();
        _StageMonsterList.AddRange(SceneManager.Instance.GetStageMonsterInfo(ConfigManager.Instance.mStageMonsterData.FindByID(sceneInfo.sceneID)));

        //设置计数器//
        if (_Counter_Monster == null)
            _Counter_Monster = new CounterTool();
        else
            _Counter_Monster.ReInitCounter();
        for (int i = 0; i < _StageMonsterList.Count; i++)
            _Counter_Monster.AddCount(_StageMonsterList[i].ArrMonsterID.Length);
        
        GetFightDatas();
        
        //开启刷怪//
        _SceneStatus = ESceneStatus.essNormal;
        CreateSoldier();

        PreLoadResource();
    }

    /// <summary>
    /// 获取战斗数据
    /// </summary>
    private void GetFightDatas()
    {
        if (_SoldierList == null)
        {
            _SoldierList = new List<FightSoldierInfo>();
        }
        _SoldierList.Clear();
        if (UISystem.Instance.FightView.listSoldierInfo != null)
        {
            for (int i = 0; i < UISystem.Instance.FightView.listSoldierInfo.Count; i++)
            {
                _SoldierList.Add(new FightSoldierInfo(UISystem.Instance.FightView.listSoldierInfo[i].mSoldier, UISystem.Instance.FightView.listSoldierInfo[i].mNum));
            }
        }
    }

    /// <summary>
    /// 创建士兵
    /// </summary>
    private void CreateSoldier()
    {
        //创建英雄角色//
        //CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
        //    new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
        //        (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, EFightType.eftExpedition, 0, null, CheckResourceIsReady));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(FightRelatedModule.Instance.SelfHeroInfo, 0, pathIndex_Hero, ERoleType.ertHero,
                (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, EFightType.eftExpedition, 0, null, CheckResourceIsReady));

        if (_SoldierList == null)
            return;

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
