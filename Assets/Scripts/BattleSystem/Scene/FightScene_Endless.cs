using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 无尽战场
/// </summary>
public class FightScene_Endless : FightSceneBase
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



    ///// <summary>
    ///// 战斗场景背景图
    ///// </summary>
    //private UITexture fightBackGround;
    /// <summary>
    /// 关卡怪物列表
    /// </summary>
    private List<RefreshMonsterInfo> _StageMonsterList = new List<RefreshMonsterInfo>();
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
    /// 剩余怪物刷新时间
    /// </summary>
    private float _LastRefreshMonsterTime;
    /// <summary>
    /// 上次怪物刷新时间
    /// </summary>
    private float _PreRefreshMonsterTime;
    /// <summary>
    /// 刷怪时间间隔
    /// </summary>
    private float _DisRefreshMonsterTime;
    /// <summary>
    /// 关卡怪物积分表
    /// </summary>
    private ChapterGradeInfo _ChapterGradeInfo;
    /// <summary>
    /// 消灭怪物列表
    /// </summary>
    private int _DisMonsterNum;
    /// <summary>
    /// 消灭Boss列表
    /// </summary>
    private int _DisBossNum;
    /// <summary>
    /// 关卡数据
    /// </summary>
    private StageInfo _StageInfo;



    //-----------------------------------------------------------入口函数---------------------------------------------------------------------------//
    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SceneParse);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SceneResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartShowBossTalk, CommandEvent_StartShowBossTalk);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
        pathIndex_Hero = 3;
        base.Initialize(vInfo);
        if (sceneInfo != null)
            _StageInfo = ConfigManager.Instance.mStageData.GetInfoByID(sceneInfo.sceneID);
        pMonsterAttributeConfig = ConfigManager.Instance.mMonsterData;

        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;

        InitSceneStatus(false);
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_SceneParse);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_SceneResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartShowBossTalk, CommandEvent_StartShowBossTalk);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
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
        if (pMonsterAttributeConfig == null)
            return;
        List<uint> tmpListID = new List<uint>();
        if (_StageMonsterList != null)
        {
            for (int i = 0; i < _StageMonsterList.Count; i++)
            {
                if (_StageMonsterList[i] == null)
                    continue;
                for (int j = 0; j < _StageMonsterList[i].ArrMonsterID.Length; j++)
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
                        }, tmpSingleMonsterData.trajectory,10);
                    }
                    foreach (uint id in tmpSingleMonsterData.Skill)
                    {
                        Skill tmpSkill = Skill.createByID(id);
                        tmpSkill.CacheEffect();
                    }

                }
            }
        }
    }
    


    //-----------------------------------------------------------事件方法---------------------------------------------------------------------------//
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
                {
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsDieHero;
                }
                break;
            case EPVEFinishStatus.epvefsDisBoss:
            case EPVEFinishStatus.epvefsDieMonster:
                {
                    if (_Counter_Monster == null)
                        return;
                    _Counter_Monster.DelCount();
                    //添加积分//
                    AddFightScore((uint)tmpInfo._DestroyID);
                    if (_Counter_Monster.Get_Count > 0)
                        return;
                    Operate_Victory();
                }
                break;
            case EPVEFinishStatus.epvefsStop:
                {
                    Operate_Failure();
                }
                break;
            case EPVEFinishStatus.epvefsOutTime:
                {
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsOutTime;
                }
                break;
            case EPVEFinishStatus.epvefsDieSoldier:
                { }
                break;
            default:
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

    //-----------------------------------------------------------继承方法---------------------------------------------------------------------------//
    public override void ReSetStatus(object vData)
    {
        foreach (KeyValuePair<int, RoleBase> tmpInfo in pRoleManager.Get_RoleDic)
        {
            tmpInfo.Value.InitAction();
        }

        CreateSceneInfo tmpSceneInfo = (CreateSceneInfo)vData;
        if (tmpSceneInfo == null)
            return;
        sceneInfo.CopyTo(tmpSceneInfo);
        StageInfo tmpStageInfo = ConfigManager.Instance.mStageData.GetInfoByID(sceneInfo.sceneID);
        if (tmpStageInfo == null)
            return;
        if (_StageInfo == null)
            _StageInfo = new StageInfo();
        _StageInfo.CopyTo(tmpStageInfo);
        InitSceneStatus(true);
        SceneManager.Instance.Get_FightID = _StageInfo.ID;
        base.ReSetStatus(vData);
    }

    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        _SceneStatus = ESceneStatus.essVictory;
        CancelInvoke("CreateEnemyOperate");
        FightRelatedModule.Instance.SendEndlessDungeonReward((int)ESceneStatus.essVictory, sceneInfo.sceneID, _DisMonsterNum, _DisBossNum, 0, 0, 0, 0, UISystem.Instance.FightView.GetFightDisTime());
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(ESceneStatus.essVictory, 0, _DisMonsterNum, _DisBossNum));
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        _SceneStatus = ESceneStatus.essFailure;
        CancelInvoke("CreateEnemyOperate");
        FightRelatedModule.Instance.SendEndlessDungeonReward((int)ESceneStatus.essFailure, sceneInfo.sceneID, _DisMonsterNum, _DisBossNum, 0, 0, 0, 0, UISystem.Instance.FightView.GetFightDisTime());
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(ESceneStatus.essFailure, 0, _DisMonsterNum, _DisBossNum));
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
        _DisRefreshMonsterTime = (_DisRefreshMonsterTime - (Time.time - _PreRefreshMonsterTime) * vFightSpeed);
        _PreRefreshMonsterTime = Time.time;
        //重新开始刷怪//
        Invoke("CreateEnemyOperate", _DisRefreshMonsterTime / vFightSpeed);
    }


    //-----------------------------------------------------------工具函数---------------------------------------------------------------------------//
    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitSceneStatus(bool vIsReSet)
    {
        _DisMonsterNum = 0;
        _DisBossNum = 0;
        //关闭怪物刷新//
        CancelInvoke("CreateEnemyOperate");
        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);

        //获取怪物刷新表//
        _StageMonsterList.Clear();
        _StageMonsterList.AddRange(SceneManager.Instance.GetStageMonsterInfo(ConfigManager.Instance.mStageMonsterData.FindByID(_StageInfo.ID)));

        //设置计数器//
        if (_Counter_Monster == null)
            _Counter_Monster = new CounterTool();
        else
            _Counter_Monster.ReInitCounter();
        for (int i = 0; i < _StageMonsterList.Count; i++)
            _Counter_Monster.AddCount(_StageMonsterList[i].ArrMonsterID.Length);

        if (vIsReSet)
        {
            //修改角色位置//
            foreach (KeyValuePair<int, RoleBase> tmpInfo in pRoleManager.Get_RoleDic)
            {
                if (tmpInfo.Value == null)
                    continue;
                tmpInfo.Value.transform.localPosition = new Vector3(0, tmpInfo.Value.transform.localPosition.y, 0);
            }
            CheckResourceIsReady(null);
        }
        else
        {
            //角色重置//
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);
            //创建英雄角色//
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
                    (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, EFightType.eftEndless, 0, null, CheckResourceIsReady));
        }

        //开启刷怪//
        _SceneStatus = ESceneStatus.essNormal;
        PreLoadResource();
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
                MonsterAttributeInfo tmpSingleMonsterData = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(tmpMonsterID);
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

    /// <summary>
    /// 添加积分
    /// </summary>
    /// <param name="vMonsterID"></param>
    private void AddFightScore(uint vMonsterID)
    {
        if (_StageInfo == null)
            return;
        ChapterGradeInfo tmpGradeInfo = ConfigManager.Instance.mChapterGradeData.GetSingleDataByID(_StageInfo.ChapterID);
        if (tmpGradeInfo == null)
            return;
        MonsterAttributeInfo tmpMonsterInfo = pMonsterAttributeConfig.GetMonsterAttributeByID(vMonsterID);
        if (tmpMonsterInfo == null)
            return;
        float tmpScore = 0;
        if (tmpMonsterInfo.IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
        {
            tmpScore = tmpGradeInfo.BossScore;
            _DisBossNum += 1;
        }
        else
        {
            tmpScore = tmpGradeInfo.MonsterScore;
            _DisMonsterNum += 1;
        }

        if (UISystem.Instance.FightView == null)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, tmpScore);
    }
    private void DelFightScore(uint vSoldierID)
    {
        if (_StageInfo == null)
            return;
        ChapterGradeInfo tmpGradeInfo = ConfigManager.Instance.mChapterGradeData.GetSingleDataByID(_StageInfo.ChapterID);
        if (tmpGradeInfo == null)
            return;
        SoldierAttributeInfo tmpSoldierInfo = ConfigManager.Instance.mSoldierData.FindById(vSoldierID);
        if (tmpSoldierInfo == null)
            return;
        if (UISystem.Instance.FightView == null)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, (float)tmpGradeInfo.SoldierScore);
    }

}
