using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 新手引导战场
/// </summary>
public class FightScene_NewGuide : FightSceneBase
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


    /// <summary>
    /// 新手引导关卡数据
    /// </summary>
    private StageInfo stageInfo;
    ///// <summary>
    ///// 战斗场景背景图
    ///// </summary>
    //private UITexture fightBackGround;
    /// <summary>
    /// 关卡怪物列表
    /// </summary>
    private List<RefreshMonsterInfo> stageMonsterList = new List<RefreshMonsterInfo>();
    private Dictionary<int, List<RefreshMonsterInfo>> stageMonsterDic = new Dictionary<int, List<RefreshMonsterInfo>>();
    /// <summary>
    /// 关卡怪物种类信息
    /// </summary>
    private Dictionary<uint, ShowInfoBase> stageMonsterInfoDic = new Dictionary<uint, ShowInfoBase>();
    /// <summary>
    /// 刷怪时间间隔[秒]
    /// </summary>
    private float disRefreshMonsterTime;
    /// <summary>
    /// 怪物配置表引用
    /// </summary>
    private MonsterAttributeConfig pMonsterAttributeConfig;
    /// <summary>
    /// 场景怪物计数器
    /// </summary>
    private CounterTool counterMonster;
    /// <summary>
    /// 左移位置
    /// </summary>
    private Transform transPosLeft;
    private UISprite Spt_Quan_Left;
    private UISprite Spt_Arrow_Left;
    /// <summary>
    /// 右移位置
    /// </summary>
    private Transform transPosRight;
    private UISprite Spt_Quan_Right;
    private UISprite Spt_Arrow_Right;
    /// <summary>
    /// 是否到达左边指定位置
    /// </summary>
    private bool isGetPosLeft;
    /// <summary>
    /// 是否到达右边指定位置
    /// </summary>
    private bool isGetPosRight;
    /// <summary>
    /// 是否第一次刷怪
    /// </summary>
    private bool isFirstRefreshEnemy;


    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_ReSetMoveStatus, CommandEvent_ReSetMoveStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_NewGuideReCreateEnemy, CommandEvent_NewGuideReCreateEnemy);
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
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_ReSetMoveStatus, CommandEvent_ReSetMoveStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_NewGuideReCreateEnemy, CommandEvent_NewGuideReCreateEnemy);
        CancelInvoke("CreateEnemyOperate");
        base.Uninitialize();
    }

    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        _SceneStatus = ESceneStatus.essVictory;
        CancelInvoke("CreateEnemyOperate");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        FightViewPanel_Result.Instance.Refresh_NewGuide((int)_SceneStatus, sceneInfo.sceneID);
        return true;
    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        _SceneStatus = ESceneStatus.essFailure;
        CancelInvoke("CreateEnemyOperate");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        FightViewPanel_Result.Instance.Refresh_NewGuide((int)_SceneStatus, sceneInfo.sceneID);
        return true;
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
        transPosLeft = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Pos_Left");
        if (transPosLeft != null)
        {
            transPosLeft.gameObject.SetActive(false);
        }
        Spt_Quan_Left = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Pos_Left/Spt_Quan").gameObject.GetComponent<UISprite>();
        if (Spt_Quan_Left != null)
        {
            CommonFunction.SetSpriteName(Spt_Quan_Left, "CMN_quan1");
        }
        Spt_Arrow_Left = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Pos_Left/Spt_Quan/Spt_Arrow").gameObject.GetComponent<UISprite>();
        if (Spt_Arrow_Left != null)
        {
            CommonFunction.SetSpriteName(Spt_Arrow_Left, "Cmn_icon_Arrow");
        }
        transPosRight = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Pos_Right");
        if (transPosRight != null)
        {
            transPosRight.gameObject.SetActive(false);
        }
        Spt_Quan_Right = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Pos_Right/Spt_Quan").gameObject.GetComponent<UISprite>();
        if (Spt_Quan_Right != null)
        {
            CommonFunction.SetSpriteName(Spt_Quan_Right, "CMN_quan1");
        }
        Spt_Arrow_Right = tmpLocalTrans.FindChild("Camera/SceneContent/Empty_Other/Refresh_Pos_Right/Spt_Quan/Spt_Arrow").gameObject.GetComponent<UISprite>();
        if (Spt_Arrow_Right != null)
        {
            CommonFunction.SetSpriteName(Spt_Arrow_Right, "Cmn_icon_Arrow");
        }

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
        string tmpName = string.Format("{0}_L.assetbundle", sceneInfo.sceneBackGround);
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
                    foreach (uint id in tmpSingleMonsterData.Skill)
                    {
                        Skill tmpSkill = Skill.createByID(id);
                        if (tmpSkill != null)
                            tmpSkill.CacheEffect();
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
                        foreach (uint id in tmpSingleMonsterData.Skill)
                        {
                            Skill tmpSkill = Skill.createByID(id);
                            tmpSkill.CacheEffect();
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
            case EPVEFinishStatus.epvefsDieMonster:
            case EPVEFinishStatus.epvefsDisBoss:
                {//敌方士兵死亡//
                    if (counterMonster == null)
                        return;
                    counterMonster.DelCount();
                    if (counterMonster.Get_Count > 0)
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
        }
    }
    /// <summary>
    /// 开启移动位置检测
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetMoveStatus(object vDataObj)
    {
        EHeroMoveType tmpHeroMoveType = (EHeroMoveType)vDataObj;

        if (transPosLeft != null)
        {
            if (tmpHeroMoveType != EHeroMoveType.ehmtOnlyLeft)
                transPosLeft.gameObject.SetActive(false);
            else
            {
                Scheduler.Instance.AddUpdator(CheckHeroPos);
                transPosLeft.gameObject.SetActive(true);
            }
        }
        if (transPosRight != null)
        {
            if (tmpHeroMoveType != EHeroMoveType.ehmtOnlyRight)
                transPosRight.gameObject.SetActive(false);
            else
            {
                Scheduler.Instance.AddUpdator(CheckHeroPos);
                transPosRight.gameObject.SetActive(true);
            }
        }


        if (transPosLeft != null)
            transPosLeft.gameObject.SetActive(false);
        if (transPosRight != null)
            transPosRight.gameObject.SetActive(false);
        if (tmpHeroMoveType == EHeroMoveType.ehmtOnlyLeft)
        {
            if (transPosLeft != null)
            {
                Scheduler.Instance.AddUpdator(CheckHeroPos);
                transPosLeft.gameObject.SetActive(true);
            }
        }
        else if (tmpHeroMoveType == EHeroMoveType.ehmtOnlyRight)
        {
            if (transPosRight != null)
            {
                Scheduler.Instance.AddUpdator(CheckHeroPos);
                transPosRight.gameObject.SetActive(true);
            }
        }
        else
        {
            //开启刷怪//
            stageMonsterInfoDic.Clear();
            if (stageMonsterList.Count > 0)
                Invoke("CreateEnemyOperate", stageMonsterList[0].RefreshTime);
        }
    }

    /// <summary>
    /// 新手引导第二次开启刷怪
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_NewGuideReCreateEnemy(object vDataObj)
    {
        if (!isFirstRefreshEnemy)
            return;
        GuideManager.Instance.CheckTrigger(GuideTrigger.SpecialFightNorAtkSoldier);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerMagic, new FightDecMP(EFightCamp.efcSelf, 150));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_StartRecoveryMagic);
        isFirstRefreshEnemy = false;
        if (stageMonsterList.Count > 0)
            Invoke("CreateEnemyOperate", stageMonsterList[0].RefreshTime);
    }

    
    /// <summary>
    /// 初始化设置
    /// </summary>
    private void InitSceneStatus()
    {
        if (stageInfo == null)
            return;
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;
        isGetPosLeft = false;
        isGetPosRight = false;

        //关闭怪物刷新//
        isFirstRefreshEnemy = true;
        CancelInvoke("CreateEnemyOperate");
        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        //角色重置//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //创建英雄角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
            new CData_CreateRole(PlayerData.Instance._Attribute, 0, pathIndex_Hero, ERoleType.ertHero,
                (EHeroGender)PlayerData.Instance._Gender, EFightCamp.efcSelf, sceneInfo.sceneType, 0, null, CheckResourceIsReady));

        //获取怪物刷新表//
        stageMonsterList.Clear();
        stageMonsterDic.Clear();
        SceneManager.Instance.GetStageMonsterInfo(ConfigManager.Instance.mStageMonsterData.FindByID(stageInfo.ID), out stageMonsterList, out stageMonsterDic);

        //设置计数器//
        if (counterMonster == null)
            counterMonster = new CounterTool();
        else
            counterMonster.ReInitCounter();
        for (int i = 0; i < stageMonsterList.Count; i++)
            counterMonster.AddCount(stageMonsterList[i].ArrMonsterID.Length);
        foreach (KeyValuePair<int, List<RefreshMonsterInfo>> tmpInfo in stageMonsterDic)
        {
            for (int i = 0; i < tmpInfo.Value.Count; i++)
                counterMonster.AddCount(tmpInfo.Value[i].ArrMonsterID.Length);
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

            //检测是否存在ID对应数据//
            if (!stageMonsterInfoDic.ContainsKey(tmpMonsterID))
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
                stageMonsterInfoDic.Add(tmpMonsterID, tmpInfo);
            }
            //通知创建怪物//
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                new CData_CreateRole(stageMonsterInfoDic[tmpMonsterID], 0, Get_OtherIndex,
                    ERoleType.ertMonster, EHeroGender.ehgNone, EFightCamp.efcEnemy,
                    sceneInfo.sceneType, stageMonsterList[0].initPosX));
        }
        //检测是否刷新完毕//
        if (!isFirstRefreshEnemy)
        {
            if ((stageMonsterList.Count > 1) && (stageMonsterList[1] != null))
            {
                //设置下次刷新时间//
                disRefreshMonsterTime = stageMonsterList[1].RefreshTime - stageMonsterList[0].RefreshTime;
                Invoke("CreateEnemyOperate", disRefreshMonsterTime / SceneManager.Instance.Get_FightSpeed);
            }
        }
        //删除当前怪物数据//
        stageMonsterList.RemoveAt(0);
    }

    /// <summary>
    /// 检测英雄位置
    /// </summary>
    private void CheckHeroPos()
    {
        if ((isGetPosLeft == true) && (isGetPosRight == true))
            return;
        if (RoleManager.Instance.Get_Hero == null)
            return;
        if (!isGetPosLeft)
        {
            if (transPosLeft != null)
            {
                if (transPosLeft.gameObject.activeSelf)
                {
                    if (Mathf.Abs(transPosLeft.position.x - RoleManager.Instance.Get_Hero.transform.position.x) <= 0.1f)
                    {
                        //通知新手引导到达左边指定位置//
                        Scheduler.Instance.RemoveUpdator(CheckHeroPos);
                        GuideManager.Instance.CheckTrigger(GuideTrigger.MoveDirCompleted);
                        isGetPosLeft = true;
                    }
                }
            }
        }
        if (!isGetPosRight)
        {
            if (transPosRight != null)
            {
                if (transPosRight.gameObject.activeSelf)
                {
                    if (Mathf.Abs(transPosRight.position.x - RoleManager.Instance.Get_Hero.transform.position.x) <= 0.1f)
                    {
                        //通知新手引导到达右边指定位置//
                        Scheduler.Instance.RemoveUpdator(CheckHeroPos);
                        GuideManager.Instance.CheckTrigger(GuideTrigger.MoveDirCompleted);
                        isGetPosRight = true;
                    }
                }
            }
        }
    }
}
