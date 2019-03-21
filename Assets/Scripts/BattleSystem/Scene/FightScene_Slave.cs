using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

/// <summary>
/// 奴隶战场
/// </summary>
public class FightScene_Slave : FightSceneBase
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
    /// 己方计数器
    /// </summary>
    private CounterTool counterSelf;
    /// <summary>
    /// 敌方计数器
    /// </summary>
    private CounterTool counterEnemy;
    /// <summary>
    /// 己方玩家信息
    /// </summary>
    public FightPlayerInfo fightInfoSelf;
    /// <summary>
    /// 敌方玩家信息
    /// </summary>
    public FightPlayerInfo fightInfoEnemy;
    /// <summary>
    /// 角色资源总计数
    /// </summary>
    private CounterTool _TotalRoleCounter;



    public override void Initialize(CreateSceneInfo vInfo)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        pathIndex_Hero = 3;
        base.Initialize(vInfo);
        InitSceneStatus();
    }

    public override void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
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

        //for (int i = 0; i < 2; i++)
        //{
        //    GameObject tmpObj = CommonFunction.InstantiateObject(fightBackGround.gameObject, fightBackGround.transform.parent);
        //    if (tmpObj == null)
        //        continue;
        //    tmpObj.transform.localPosition = new Vector3(i * 1024, 0, 0);
        //    tmpObj.transform.localScale = Vector3.one;

        //    string tmpName = string.Empty;
        //    if (i == 0)
        //        tmpName = string.Format("{0}_L.assetbundle", sceneInfo.sceneBackGround);
        //    else
        //        tmpName = string.Format("{0}_R.assetbundle", sceneInfo.sceneBackGround);

        //    ResourceLoadManager.Instance.LoadAloneImage(tmpName, (texture) =>
        //    {
        //        tmpObj.GetComponent<UITexture>().mainTexture = texture;
        //        tmpObj.GetComponent<UITexture>().MakePixelPerfect();
        //        tmpObj.gameObject.SetActive(true);
        //    });
        //}


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

    protected override void AddRolePath()
    {
        for (int i = 0; i < ROLEPATH_MAX_COUNT; i++)
            AddSingleRolePath(-(i * ROLEPATH_DISTANCE));
    }

    /*
     * Prison.cs
     * public fogs.proto.msg.EnslaveFightBeforeReq SaveFightBeforeReq = new EnslaveFightBeforeReq();
     * SaveFightBeforeReq.type[EnslaveFightType]
     * public void SendEnslaveFightOverReq(EnslaveFightResult result, int key, string sign)
     * public void ReceiveEnslaveFightOverResp(EnslaveFightOverResp result)
     * PlayerData.Instance._Prison
     */
    public override bool Operate_Victory()
    {
        if (!base.Operate_Victory())
            return false;
        _SceneStatus = ESceneStatus.essVictory;
        FightRelatedModule.Instance.SendEnslaveFightOverReq(EnslaveFightResult.EFR_WIN, 2, "3");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        return true;


    }

    public override bool Operate_Failure()
    {
        if (!base.Operate_Failure())
            return false;
        _SceneStatus = ESceneStatus.essFailure;
        FightRelatedModule.Instance.SendEnslaveFightOverReq(EnslaveFightResult.EFER_LOSE, 2, "3");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightFinished, new FightFinishedInfo(_SceneStatus, 0));
        return true;
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
                    AddDeathSelfCount();
                }
                break;
            case EPVEFinishStatus.epvefsDisEnemyHero:
                {
                    AddDeathEnemyCount();
                }
                break;
            case EPVEFinishStatus.epvefsDisEnemySoldier:
                {
                    AddDeathEnemyCount();
                    UISystem.Instance.FightView.ReSetEnemySoldierInfo();
                }
                break;
            case EPVEFinishStatus.epvefsOutTime:
                {
                    //提示超时//
                    Operate_Failure();
                    fightFailReason = EPVEFinishStatus.epvefsOutTime;
                }
                break;
            case EPVEFinishStatus.epvefsDieSoldier:
                {
                    AddDeathSelfCount();
                    UISystem.Instance.FightView.ReSetSelfSoldierInfo();
                }
                break;
            default:
                { }
                break;
        }
    }


    /// <summary>
    /// 初始化场景
    /// </summary>
    private void InitSceneStatus()
    {
        if (pRoleManager == null)
            pRoleManager = RoleManager.Instance;

        //场景重置//
        transOther.localPosition = new Vector3(limitLeft, 0, 0);
        //清空角色//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear);

        //获取怪物刷新表//
        GetFightRoleInfo();

        //开启刷怪//
        _SceneStatus = ESceneStatus.essNormal;
        CreateFightRole();
    }

    /// <summary>
    /// 检测是否已经超过战斗限制时间
    /// </summary>
    /// <returns></returns>
    private bool CheckIsOutFightTime()
    {
        return false;
    }

    /// <summary>
    /// 获取战斗双方角色信息
    /// </summary>
    private void GetFightRoleInfo()
    {
        fightInfoSelf = UISystem.Instance.FightView.fightPlayer_Self;
        fightInfoEnemy = UISystem.Instance.FightView.fightPlayer_Enemy;
        GetTotalRoleCounter();
    }

    /// <summary>
    /// 创建战斗双方角色
    /// </summary>
    private void CreateFightRole()
    {
        CreateRole_Self();
        CreateRole_Enemy();
    }

    /// <summary>
    /// 创建己方角色
    /// </summary>
    private void CreateRole_Self()
    {
        if (fightInfoSelf == null)
            return;

        //设置计数器//
        if (counterSelf == null)
            counterSelf = new CounterTool();
        else
            counterSelf.ReInitCounter();

        //创建英雄//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate, new CData_CreateRole(fightInfoSelf.mAttribute,
            0, pathIndex_Hero, ERoleType.ertHero, (EHeroGender)fightInfoSelf.mGender, EFightCamp.efcSelf, EFightType.eftSlave, 0, null, 
            (roleObj) =>
                {
                    //if (fightInfoSelf.mEquip != null)
                    //{
                    //    roleObj.Get_MainSpine.RepleaceEquipment(fightInfoSelf.mEquip);
                    //}

                    _TotalRoleCounter.DelCount();
                    if (_TotalRoleCounter.Get_Count > 0)
                        return;
                    base.CheckResourceIsReady(roleObj);
                }));
        counterSelf.AddCount();

        //创建武将//
        if (fightInfoSelf.mSoldierList != null)
        {
            for (int i = 0; i < fightInfoSelf.mSoldierList.Count; i++)
            {
                if (fightInfoSelf.mSoldierList[i] == null)
                    continue;
                for (int j = 0; j < fightInfoSelf.mSoldierList[i].mNum; j++)
                {
                    if (fightInfoSelf.mSoldierList[i].mSoldier == null)
                        continue;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        new CData_CreateRole(fightInfoSelf.mSoldierList[i].mSoldier.showInfoSoldier, fightInfoSelf.mSoldierList[i].mSoldier.uId,
                            Get_OtherIndex, ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcSelf, EFightType.eftSlave, 0, null, CheckResourceIsReady));
                }
                counterSelf.AddCount(fightInfoSelf.mSoldierList[i].mNum);
            }
        }
    }

    /// <summary>
    /// 创建敌方角色
    /// </summary>
    private void CreateRole_Enemy()
    {
        if (fightInfoEnemy == null)
            return;

        //设置计数器//
        if (counterEnemy == null)
            counterEnemy = new CounterTool();
        else
            counterEnemy.ReInitCounter();

        //创建英雄//
        fightInfoEnemy.mAttribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(fightInfoEnemy.mAttribute.AttRate);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate, new CData_CreateRole(fightInfoEnemy.mAttribute,
            0, pathIndex_Hero, ERoleType.ertHero, (EHeroGender)fightInfoEnemy.mGender, EFightCamp.efcEnemy, EFightType.eftSlave, 0, null, 
            (roleObj) =>
                {
                    //if (fightInfoEnemy.mEquip != null)
                    //{
                    //    roleObj.Get_MainSpine.RepleaceEquipment(fightInfoEnemy.mEquip);
                    //}

                    _TotalRoleCounter.DelCount();
                    if (_TotalRoleCounter.Get_Count > 0)
                        return;
                    base.CheckResourceIsReady(roleObj);
                }));

        counterEnemy.AddCount();

        //创建武将//
        if (fightInfoEnemy.mSoldierList != null)
        {
            for (int i = 0; i < fightInfoEnemy.mSoldierList.Count; i++)
            {
                if (fightInfoEnemy.mSoldierList[i] == null)
                    continue;
                for (int j = 0; j < fightInfoEnemy.mSoldierList[i].mNum; j++)
                {
                    if (fightInfoEnemy.mSoldierList[i].mSoldier == null)
                        continue;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate,
                        new CData_CreateRole(fightInfoEnemy.mSoldierList[i].mSoldier.showInfoSoldier, fightInfoEnemy.mSoldierList[i].mSoldier.uId,
                            Get_OtherIndex, ERoleType.ertSoldier, EHeroGender.ehgNone, EFightCamp.efcEnemy, EFightType.eftSlave, 0, null, CheckResourceIsReady));
                }
                counterEnemy.AddCount(fightInfoEnemy.mSoldierList[i].mNum);
            }
        }
    }

    /// <summary>
    /// 添加死亡敌人记录
    /// </summary>
    private void AddDeathEnemyCount()
    {
        if (counterEnemy == null)
            return;
        counterEnemy.DelCount();

        if (counterEnemy.Get_Count <= 0)
            Operate_Victory();
    }
    private void AddDeathSelfCount()
    {
        if (counterSelf == null)
            return;
        counterSelf.DelCount();
        if (counterSelf.Get_Count <= 0)
        {
            Operate_Failure();
            fightFailReason = EPVEFinishStatus.epvefsDieSoldier;
        }
    }

    /// <summary>
    /// 统计角色总数
    /// </summary>
    private void GetTotalRoleCounter()
    {
        if (_TotalRoleCounter == null)
            _TotalRoleCounter = new CounterTool();
        else
            _TotalRoleCounter.ReInitCounter();
        if (fightInfoSelf != null)
        {
            if (fightInfoSelf.mAttribute != null)
                _TotalRoleCounter.AddCount();
            if (fightInfoSelf.mSoldierList != null)
            {
                for (int i = 0; i < fightInfoSelf.mSoldierList.Count; i++)
                {
                    _TotalRoleCounter.AddCount(fightInfoSelf.mSoldierList[i].mNum);
                }
            }
        }
        if (fightInfoEnemy != null)
        {
            if (fightInfoEnemy.mAttribute != null)
                _TotalRoleCounter.AddCount();
            if (fightInfoEnemy.mSoldierList != null)
            {
                for (int i = 0; i < fightInfoEnemy.mSoldierList.Count; i++)
                {
                    _TotalRoleCounter.AddCount(fightInfoEnemy.mSoldierList[i].mNum);
                }
            }
        }
    }

    protected override void CheckResourceIsReady(RoleBase vRole)
    {
        _TotalRoleCounter.DelCount();
        if (_TotalRoleCounter.Get_Count > 0)
            return;
        base.CheckResourceIsReady(vRole);
    }

}