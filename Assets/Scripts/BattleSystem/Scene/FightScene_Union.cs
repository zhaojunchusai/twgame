using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

/// <summary>
/// 异域探险战场
/// </summary>
public class FightScene_Union : FightSceneBase
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
    private const float LIMIT_LEFT = -512;
    /// <summary>
    /// 右边界
    /// </summary>
    private const float LIMIT_RIGHT = -512;



    ///// <summary>
    ///// 战斗场景背景图
    ///// </summary>
    //private UITexture fightBackGround;
    /// <summary>
    /// 怪物配置表引用
    /// </summary>
    private MonsterAttributeConfig pMonsterAttributeConfig;
    /// <summary>
    /// 己方士兵信息
    /// </summary>
    public List<FightSoldierInfo> soldierList = new List<FightSoldierInfo>();
    /// <summary>
    /// 关卡数据
    /// </summary>
    private StageInfo stageInfo;
    /// <summary>
    /// 关卡怪物列表
    /// </summary>
    private List<RefreshMonsterInfo> stageMonsterList = new List<RefreshMonsterInfo>();
    private Dictionary<int, List<RefreshMonsterInfo>> stageMonsterDic = new Dictionary<int, List<RefreshMonsterInfo>>();
    /// <summary>
    /// 关卡怪物数据信息
    /// </summary>
    private Dictionary<uint, MonsterAttributeInfo> dicMonsterInfo = new Dictionary<uint, MonsterAttributeInfo>();
    /// <summary>
    /// 场景怪物计数器
    /// </summary>
    private CounterTool counter_Monster;
    /// <summary>
    /// 关卡怪物种类信息[根据配置表ID记录相应数据 重复创建时调用]
    /// </summary>
    private Dictionary<uint, ShowInfoBase> stageMonsterInfoDic = new Dictionary<uint, ShowInfoBase>();
    /// <summary>
    /// 上次怪物刷新时间[秒]
    /// </summary>
    private float preRefreshMonsterTime;
    /// <summary>
    /// 刷怪时间间隔[秒]
    /// </summary>
    private float disRefreshMonsterTime;
    /// <summary>
    /// 对战BOSS
    /// </summary>
    private RoleBase pRoleBoss;
    /// <summary>
    /// BOSS最大血量
    /// </summary>
    private int roleBossMaxHP;



    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
        pathIndex_Hero = 3;
        base.Initialize(vInfo);
        if (sceneInfo != null)
            stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(sceneInfo.sceneID);
        pMonsterAttributeConfig = ConfigManager.Instance.mMonsterData;
        InitSceneStatus();
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus, CommandEvent_StartShowFightStatus);
        CancelInvoke("CreateOperate_Enemy");
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
        MaxGroundWidth = 1024;

        base.SetLocalUseComponentsAndDatas();
    }

    protected override void SetFightSceneBackGround()
    {
        if (fightBackGround == null)
            return;
        if (sceneInfo == null)
            return;

        fightBackGround.mainTexture = null;
        string tmpName = string.Format("{0}.assetbundle", sceneInfo.sceneBackGround);
        ResourceLoadManager.Instance.LoadAloneImage(tmpName, (texture) =>
        {
            fightBackGround.mainTexture = texture;
            fightBackGround.gameObject.SetActive(true);
        });
    }

    protected override void PreLoadResource()
    {
        base.PreLoadResource();
        if (pMonsterAttributeConfig == null)
            return;
        List<uint> tmpListID = new List<uint>();
        if (stageMonsterList != null)
        {
            for (int i = 0; i < stageMonsterList.Count; i++)
            {
                if (stageMonsterList[i] == null)
                    continue;
                if (stageMonsterList[i].ArrMonsterID != null)
                {
                    for (int j = 0; j < stageMonsterList[i].ArrMonsterID.Length; j++)
                    {
                        uint tmpMonsterID = stageMonsterList[i].ArrMonsterID[j];
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
                                {
                                    tmpSkill.CacheEffect();
                                }
                            }
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
                for (int i = 0; i < tmpInfo.Value.Count; i++)
                {
                    if (tmpInfo.Value[i] == null)
                        continue;
                    if (tmpInfo.Value[i].ArrMonsterID != null)
                    {
                        for (int j = 0; j < tmpInfo.Value[i].ArrMonsterID.Length; j++)
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
                                    {
                                        tmpSkill.CacheEffect();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }


    protected override void AddRolePath()
    {
        for (int i = 0; i < ROLEPATH_MAX_COUNT; i++)
            AddSingleRolePath(-(i * ROLEPATH_DISTANCE));
    }

    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        _SceneStatus = ESceneStatus.essVictory;
        CancelInvoke("CreateOperate_Enemy");
        int tmpHurtValue = roleBossMaxHP - UISystem.Instance.FightView.Get_InitUnionHurt;
        Debug.LogWarning(string.Format("Victory: [{0}, {1}]", roleBossMaxHP, UISystem.Instance.FightView.Get_InitUnionHurt));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        FightRelatedModule.Instance.SendUnionPveDgnReward(stageInfo.ID, tmpHurtValue, (int)_SceneStatus);
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        _SceneStatus = ESceneStatus.essFailure;
        CancelInvoke("CreateOperate_Enemy");
        int tmpHurtValue = 0;
        if (pRoleBoss != null)
        {
            tmpHurtValue = roleBossMaxHP - UISystem.Instance.FightView.Get_InitUnionHurt - pRoleBoss.Get_FightAttribute.HP;
        }
        //Debug.LogWarning(string.Format("Failure: [{0}, {1}, {2}]", roleBossMaxHP, UISystem.Instance.FightView.Get_InitUnionHurt, pRoleBoss.Get_FightAttribute.HP));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        FightRelatedModule.Instance.SendUnionPveDgnReward(stageInfo.ID, tmpHurtValue, (int)_SceneStatus);
        return true;
    }

    public override void ChangeFightSpeedOperate(float vFightSpeed)
    {
        base.ChangeFightSpeedOperate(vFightSpeed);
        if (stageMonsterList.Count <= 0)
            return;
        //删除原有怪物刷新方法//
        CancelInvoke("CreateOperate_Enemy");
        //修改怪物刷新时间//
        disRefreshMonsterTime = disRefreshMonsterTime - (Time.time - preRefreshMonsterTime) * vFightSpeed;
        preRefreshMonsterTime = Time.time;
        //重新开始刷怪//
        Invoke("CreateOperate_Enemy", disRefreshMonsterTime / vFightSpeed);
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
                {
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsDieHero;
                }
                break;
            case EPVEFinishStatus.epvefsDieMonster:
            case EPVEFinishStatus.epvefsDisBoss:
                {
                    if (counter_Monster == null)
                        return;
                    counter_Monster.DelCount();
                    if (counter_Monster.Get_Count > 0)
                        return;
                    Operate_Victory();
                }
                break;
            case EPVEFinishStatus.epvefsOutTime:
                {
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsOutTime;
                }
                break;
            default:
                { }
                break;
        }
    }
    /// <summary>
    /// 开始显示战斗
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartShowFightStatus(object vDataObj)
    {
        stageMonsterInfoDic.Clear();
        if (stageMonsterList.Count <= 0)
            return;
        CreateOperate_Self();
        disRefreshMonsterTime = stageMonsterList[0].RefreshTime;
        preRefreshMonsterTime = Time.time;
        Invoke("CreateOperate_Enemy", stageMonsterList[0].RefreshTime);
    }



    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitSceneStatus()
    {
        if (stageInfo == null)
            return;
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;
        CancelInvoke("CreateOperate_Enemy");
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //创建英雄角色//
        //CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
        //    new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
        //        (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, sceneInfo.sceneType, 0, null, CheckResourceIsReady));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(FightRelatedModule.Instance.SelfHeroInfo, 0, pathIndex_Hero, ERoleType.ertHero,
                (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, sceneInfo.sceneType, -380, null, CheckResourceIsReady));

        ObtainSoldierInfo();
        //获取怪物刷新表//
        stageMonsterList.Clear();
        stageMonsterDic.Clear();
        dicMonsterInfo.Clear();
        SceneManager.Instance.GetStageMonsterInfo(ConfigManager.Instance.mStageMonsterData.FindByID(stageInfo.ID), out stageMonsterList, out stageMonsterDic);
        //设置计数器//
        if (counter_Monster == null)
            counter_Monster = new CounterTool();
        else
            counter_Monster.ReInitCounter();
        for (int i = 0; i < stageMonsterList.Count; i++)
            counter_Monster.AddCount(stageMonsterList[i].ArrMonsterID.Length);
        foreach (KeyValuePair<int, List<RefreshMonsterInfo>> tmpInfo in stageMonsterDic)
        {
            for (int i = 0; i < tmpInfo.Value.Count; i++)
                counter_Monster.AddCount(tmpInfo.Value[i].ArrMonsterID.Length);
        }

        _SceneStatus = ESceneStatus.essNormal;
        PreLoadResource();
    }

    /// <summary>
    /// 获取士兵数据
    /// </summary>
    private void ObtainSoldierInfo()
    {
        soldierList.Clear();
        if (UISystem.Instance.FightView.listSoldierInfo == null)
            return;
        soldierList.AddRange(UISystem.Instance.FightView.listSoldierInfo);
    }

    /// <summary>
    /// 创建自己角色
    /// </summary>
    private void CreateOperate_Self()
    {
        if (soldierList == null)
            return;
        for (int i = 0; i < soldierList.Count; i++)
        {
            if (soldierList[i] == null)
                continue;
            for (int j = 0; j < soldierList[i].mNum; j++)
            {
                if (soldierList[i].mSoldier == null)
                    continue;
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                    new CData_CreateRole(soldierList[i].mSoldier.showInfoSoldier, soldierList[i].mSoldier.uId, Get_OtherIndex,
                        ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcSelf, EFightType.eftExpedition, 0));
            }
        }
    }

    /// <summary>
    /// 创建敌人角色
    /// </summary>
    private void CreateOperate_Enemy()
    {
        if (_SceneStatus != ESceneStatus.essNormal)
            return;
        if (stageMonsterList.Count <= 0)
            return;
        if (stageMonsterList[0] == null)
            return;
        if (stageMonsterList[0].ArrMonsterID == null)
            return;
        disRefreshMonsterTime = 0;

        for (int i = 0; i < stageMonsterList[0].ArrMonsterID.Length; i++)
        {
            uint tmpMonsterID = stageMonsterList[0].ArrMonsterID[i];

            if (!stageMonsterInfoDic.ContainsKey(tmpMonsterID))
            {
                if (pMonsterAttributeConfig == null)
                    continue;
                MonsterAttributeInfo tmpSingleMonsterData = pMonsterAttributeConfig.GetMonsterAttributeByID(tmpMonsterID);
                if (tmpSingleMonsterData == null)
                    continue;
                dicMonsterInfo.Add(tmpMonsterID, tmpSingleMonsterData);
                ShowInfoBase tmpInfo = new ShowInfoBase();
                tmpInfo.ReSetFightAttribute(tmpMonsterID, tmpSingleMonsterData.HP, 0, tmpSingleMonsterData.Attack, CommonFunction.GetSecondTimeByMilliSecond(tmpSingleMonsterData.AttRate),
                    tmpSingleMonsterData.AttDistance, tmpSingleMonsterData.Accuracy, tmpSingleMonsterData.Crit, tmpSingleMonsterData.Dodge, tmpSingleMonsterData.Tenacity, tmpSingleMonsterData.Speed);
                stageMonsterInfoDic.Add(tmpMonsterID, tmpInfo);
            }

            if ((dicMonsterInfo.ContainsKey(tmpMonsterID)) && (dicMonsterInfo[tmpMonsterID] != null))
            {
                if (dicMonsterInfo[tmpMonsterID].IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
                {
                    ShowInfoBase tmpBossInfo = new ShowInfoBase();
                    tmpBossInfo.CopyTo(stageMonsterInfoDic[tmpMonsterID]);
                    roleBossMaxHP = tmpBossInfo.HP;
                    tmpBossInfo.HP -= UISystem.Instance.FightView.Get_InitUnionHurt;
                    if (tmpBossInfo.HP < 0)
                        tmpBossInfo.HP = 1;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        //new CData_CreateRole(stageMonsterInfoDic[tmpMonsterID], 0, Get_OtherIndex,
                        new CData_CreateRole(tmpBossInfo, 0, Get_OtherIndex,
                                    ERoleType.ertMonster, EHeroGender.ehgNone, EFightCamp.efcEnemy,
                                    sceneInfo.sceneType, stageMonsterList[0].initPosX, null, (roleBase) =>
                                    {
                                        if (roleBase != null)
                                        {
                                            roleBase.ReSetRoleHP(UISystem.Instance.FightView.Get_InitUnionHurt, HurtType.Crite, false);
                                            //roleBossMaxHP = roleBase.Get_MaxHPValue;
                                            roleBase.pRpleHP.SetCurAndMaxValue(roleBossMaxHP - UISystem.Instance.FightView.Get_InitUnionHurt, roleBossMaxHP);
                                            //roleBase.pRpleHP.RefreshSlider();
                                            pRoleBoss = roleBase;
                                        }
                                    }));
                    continue;
                }
            }
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                new CData_CreateRole(stageMonsterInfoDic[tmpMonsterID], 0, Get_OtherIndex,
                    ERoleType.ertMonster, EHeroGender.ehgNone, EFightCamp.efcEnemy,
                    sceneInfo.sceneType, stageMonsterList[0].initPosX));
        }
        preRefreshMonsterTime = Time.time;
        if ((stageMonsterList.Count > 1) && (stageMonsterList[1] != null))
        {
            disRefreshMonsterTime = stageMonsterList[1].RefreshTime - stageMonsterList[0].RefreshTime;
            Invoke("CreateOperate_Enemy", disRefreshMonsterTime / SceneManager.Instance.Get_FightSpeed);
        }

        stageMonsterList.RemoveAt(0);
    }
}
