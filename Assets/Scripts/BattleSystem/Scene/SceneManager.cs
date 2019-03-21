using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

public class SceneManager : Singleton<SceneManager>
{
    /// <summary>
    /// 字符串处理位数
    /// </summary>
    private const int DEL_TYPE_STRING_COUNT = 3;
    /// <summary>
    /// 自动战斗状态
    /// </summary>
    private const string AUTO_FIGHT_STATUS = "Auto_Fight_Status_{0}";


    /// <summary>
    /// 场景对象
    /// </summary>
    private FightSceneBase curScene;
    /// <summary>
    /// 场景类型
    /// </summary>
    private EFightType curSceneType;
    /// <summary>
    /// 场景配置表ID
    /// </summary>
    private uint fightID;
    /// <summary>
    /// 战斗速度
    /// </summary>
    private float fightSpeed;
    /// <summary>
    /// 传送门坐标
    /// </summary>
    private float transferPosX;
    /// <summary>
    /// 怪物城堡坐标
    /// </summary>
    private float enemyBarracksPosX;
    private bool fightIsFinished;
    /// <summary>
    /// 场景特殊物件
    /// </summary>
    public Dictionary<ESceneMarkType, RoleAttribute> DicSceneMarkObj = new Dictionary<ESceneMarkType, RoleAttribute>();
    /// <summary>
    /// 自动战斗状态
    /// </summary>
    public int AutoFightStatus;
    public bool IsAutoMove;
    public bool IsAutoSkill;
    public bool IsAutoSummon;
    /// <summary>
    /// 变形器动画资源列表[资源名, 动画资源]
    /// </summary>
    public Dictionary<string, GameObject> DicChangeSkeletonDataAssetInfo = new Dictionary<string, GameObject>();

    /// <summary>
    /// 场景对象
    /// </summary>
    public FightSceneBase Get_CurScene
    {
        get {
            return curScene;
        }
    }
    /// <summary>
    /// 场景类型
    /// </summary>
    public EFightType Get_CurSceneType
    {
        get {
            return curSceneType;
        }
    }
    /// <summary>
    /// 场景配置表ID
    /// </summary>
    public uint Get_FightID
    {
        get {
            return fightID;
        }
        set {
            fightID = value;
        }
    }
    /// <summary>
    /// 获取战斗速度
    /// </summary>
    public float Get_FightSpeed
    {
        get {
            return fightSpeed;
        }
    }
    /// <summary>
    /// 设置 获取传送门坐标
    /// </summary>
    public float Get_TransferPosX
    {
        get {
            return transferPosX;
        }
        set {
            transferPosX = value;
        }
    }
    /// <summary>
    /// 设置 获取怪物城堡坐标
    /// </summary>
    public float Get_EnemyBarracksPosX
    {
        get {
            return enemyBarracksPosX;
        }
        set {
            enemyBarracksPosX = value;
        }
    }
    /// <summary>
    /// 战斗是否结束
    /// </summary>
    public bool Get_FightIsFinished
    {
        get
        {
            return fightIsFinished;
        }
    }
    /// <summary>
    /// 根据资源名获取变形动画资源
    /// </summary>
    /// <param name="vResName"></param>
    /// <returns></returns>
    public SkeletonDataAsset Get_SingleSkeletonDataAssetByName(string vResName)
    {
        if (string.IsNullOrEmpty(vResName))
        {
            return null;
        }
        else
        {
            if (DicChangeSkeletonDataAssetInfo == null)
            {
                return null;
            }
            else
            {
                if (!DicChangeSkeletonDataAssetInfo.ContainsKey(vResName))
                {
                    return null;
                }
                else
                {
                    if (DicChangeSkeletonDataAssetInfo[vResName] == null)
                    {
                        DicChangeSkeletonDataAssetInfo.Remove(vResName);
                        return null;
                    }
                    else
                    {
                        if (DicChangeSkeletonDataAssetInfo[vResName].GetComponent<SkeletonAnimation>() == null)
                        {
                            return null;
                        }
                        else
                        {
                            return DicChangeSkeletonDataAssetInfo[vResName].GetComponent<SkeletonAnimation>().skeletonDataAsset;
                        }
                    }
                }
            }
        }
    }
    public void Add_SingleSkeletonDataAssetByName(string vResName, GameObject vResObj)
    {
        if (DicChangeSkeletonDataAssetInfo == null)
        {
            DicChangeSkeletonDataAssetInfo = new Dictionary<string, GameObject>();
        }
        if ((!string.IsNullOrEmpty(vResName)) && (vResObj != null))
        {
            if (!DicChangeSkeletonDataAssetInfo.ContainsKey(vResName))
            {
                DicChangeSkeletonDataAssetInfo.Add(vResName, vResObj);
            }
        }
    }


    /// <summary>
    /// 注册对外接口方法
    /// </summary>
    public void Initialize()
    {
        curSceneType = EFightType.eftUI;
        fightSpeed = GlobalConst.MIN_FIGHT_SPEED;
        fightIsFinished = false;
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_SceneCreate, CommandEvent_SceneCreate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_SceneDelete, CommandEvent_SceneDelete);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_SceneReSet, CommandEvent_SceneRefresh);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_LoadAutoStatus, CommandEvent_LoadAutoStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
    }
    /// <summary>
    /// 销毁接口方法
    /// </summary>
    public void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_SceneCreate, CommandEvent_SceneCreate);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_SceneDelete, CommandEvent_SceneDelete);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_SceneReSet, CommandEvent_SceneRefresh);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_LoadAutoStatus, CommandEvent_LoadAutoStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
    }

    /// <summary>
    /// 创建场景
    /// </summary>
    /// <param name="vDataObj">关卡ID</param>
    private void CommandEvent_SceneCreate(object vDataObj)
    {
        fightIsFinished = false;
        CreateSceneInfo tmpInfo = (CreateSceneInfo)vDataObj;
        if (tmpInfo == null)
            return;

        Get_TransferPosX = -100000;
        Get_EnemyBarracksPosX = 0;

        //切换场景//
        if (curScene != null)
        {
            DestroyScene();
        }

        curSceneType = tmpInfo.sceneType;
        if (!CommonFunction.CheckFightType(curSceneType))
            return;
        //判断数据是否正确//
        if ((curSceneType == EFightType.eftMain) || (curSceneType == EFightType.eftActivity) ||
            (curSceneType == EFightType.eftEndless) || (curSceneType == EFightType.eftNewGuide) ||
            (curSceneType == EFightType.eftUnion) || (curSceneType == EFightType.eftCaptureTerritory) ||
            (curSceneType == EFightType.eftCrossServerWar))
        {
            StageInfo tmpStageInfo = ConfigManager.Instance.mStageData.GetInfoByID(tmpInfo.sceneID);
            if (tmpStageInfo == null)
                return;
        }
        else if (curSceneType == EFightType.eftExpedition)
        {
            ExpeditionData tmpExpeditionInfo = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(tmpInfo.sceneID);
            if (tmpExpeditionInfo == null)
                return;
        }
        Get_FightID = tmpInfo.sceneID;

        string tmpScenePath = string.Empty;
        string tmpSceneType = string.Empty;
        if ((curSceneType == EFightType.eftMain) || (curSceneType == EFightType.eftActivity))
            tmpSceneType = EFightType.eftActivity.ToString();
        else if ((curSceneType == EFightType.eftPVP) || (curSceneType == EFightType.eftSlave))
            tmpSceneType = EFightType.eftPVP.ToString();
        else
            tmpSceneType = curSceneType.ToString();
        tmpSceneType = tmpSceneType.Substring(DEL_TYPE_STRING_COUNT, tmpSceneType.Length - DEL_TYPE_STRING_COUNT);
        tmpScenePath = string.Format(GlobalConst.RESOURCE_SCENE, tmpSceneType);

        ResourceLoadManager.Instance.LoadAssetAlone(tmpScenePath,
            (obj) =>
            {
                if (obj != null)
                {
                    GameObject tmpObj = GameObject.Instantiate(obj) as GameObject;

                    if (tmpObj != null)
                    {
                        switch (curSceneType)
                        {
                            case EFightType.eftMain:
                            case EFightType.eftActivity:
                            default:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_Activity>();
                                    break;
                                }
                            case EFightType.eftNewGuide:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_NewGuide>();
                                    break;
                                }
                            case EFightType.eftEndless:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_Endless>();
                                    break;
                                }
                            case EFightType.eftExpedition:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_Expedition>();
                                    break;
                                }
                            case EFightType.eftCaptureTerritory:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_CaptureTerritory>();
                                    break;
                                }
                            case EFightType.eftPVP:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_PVP>();
                                    break;
                                }
                            case EFightType.eftSlave:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_Slave>();
                                    break;
                                }
                            case EFightType.eftUnion:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_Union>();
                                    break;
                                }
                            case EFightType.eftServerHegemony:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_ServerHegemony>();
                                    break;
                                }
                            case EFightType.eftQualifying:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_Qualifying>();
                                    break;
                                }
                            case EFightType.eftCrossServerWar:
                                {
                                    curScene = tmpObj.AddComponent<FightScene_CrossServerWar>();
                                    break;
                                }
                        }
                        if (curScene != null)
                            curScene.Initialize(tmpInfo);
                    }
                }
            },
            (vDebug) =>
            {
                Debug.LogWarning(vDebug);
            });
    }

    /// <summary>
    /// 关闭当前场景
    /// </summary>
    private void CommandEvent_SceneDelete(object vDataObj)
    {
        fightIsFinished = false;
        curSceneType = EFightType.eftUI;
        DestroyScene();
        Save_AutoFight_Status();
    }

    /// <summary>
    /// 重置当前场景[CreateSceneInfo]
    /// </summary>
    private void CommandEvent_SceneRefresh(object vDataObj)
    {
        fightIsFinished = false;
        if (curScene == null)
            return;
        Get_TransferPosX = -100000;
        Get_EnemyBarracksPosX = 0;
        curScene.ReSetStatus(vDataObj);
    }

    /// <summary>
    /// 改变战斗速度
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ChangeSpeed(object vDataObj)
    {
        fightSpeed = (float)vDataObj;
        if (curScene == null)
            return;
        curScene.ChangeFightSpeedOperate(fightSpeed);
    }

    /// <summary>
    /// 读取自动战斗状态
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_LoadAutoStatus(object vDataObj)
    {
        Load_AutoFight_Status();
    }

    /// <summary>
    /// 战斗结束
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightFinished(object vDataObj)
    {
        fightIsFinished = true;
    }



    /// <summary>
    /// 删除当前场景
    /// </summary>
    private void DestroyScene()
    {
        //清空当前场景//
        if (curScene != null)
        {
            curScene.Uninitialize();
            curScene = null;
        }
        if (this.DicSceneMarkObj != null)
            this.DicSceneMarkObj.Clear();
        //清除内存//
        if (AloneObjectCache.Instance != null)
            AloneObjectCache.Instance.Delete();
        if (EffectObjectCache.Instance != null)
            EffectObjectCache.Instance.Delete();
        SkillManage.Instance.RemoveAll();
        System.GC.Collect();
        ResourceLoadManager.Instance.ReleaseRequestBundle();
        System.GC.Collect();
    }
    /// <summary>
    /// 删除变形动画资源列表
    /// </summary>
    private void DestroyChangeSpineInfo()
    {
        if (DicChangeSkeletonDataAssetInfo != null)
        {
            foreach (KeyValuePair<string, GameObject> tmpSingle in DicChangeSkeletonDataAssetInfo)
            {
                if (tmpSingle.Value != null)
                {
                    GameObject.Destroy(tmpSingle.Value.gameObject);
                }
            }
            DicChangeSkeletonDataAssetInfo.Clear();
            DicChangeSkeletonDataAssetInfo = null;
        }
    }


    /// <summary>
    /// 获取关卡怪物信息
    /// </summary>
    /// <param name="vMonsterInfoList"></param>
    /// <returns></returns>
    public List<RefreshMonsterInfo> GetStageMonsterInfo(List<StageMonsterInfo> vMonsterInfoList)
    {
        if (vMonsterInfoList == null)
            return null;

        List<RefreshMonsterInfo> tmpResult = new List<RefreshMonsterInfo>();
        List<RefreshMonsterInfo> tmpSingleList = new List<RefreshMonsterInfo>();
        for (int i = 0; i < vMonsterInfoList.Count; i++)
        {
            if (vMonsterInfoList[i] == null)
                continue;
            if ((vMonsterInfoList[i].TimeSlot.Count == 1) && (vMonsterInfoList[i].TimeSlot[0] == 0))
                continue;
            tmpSingleList.Clear();
            GetSingleMonsterList(vMonsterInfoList[i], out tmpSingleList);
            if (tmpSingleList == null)
                continue;
            tmpResult.AddRange(tmpSingleList);
        }
        return tmpResult;
    }
    public void GetStageMonsterInfo(List<StageMonsterInfo> vMonsterInfoList, out List<RefreshMonsterInfo> vResultList)
    {
        if (vMonsterInfoList == null)
        {
            vResultList = null;
            return;
        }

        List<RefreshMonsterInfo> tmpResult = new List<RefreshMonsterInfo>();
        List<RefreshMonsterInfo> tmpSingleList = new List<RefreshMonsterInfo>();
        for (int i = 0; i < vMonsterInfoList.Count; i++)
        {
            if (vMonsterInfoList[i] == null)
                continue;
            if ((vMonsterInfoList[i].TimeSlot.Count == 1) && (vMonsterInfoList[i].TimeSlot[0] == 0))
                continue;
            tmpSingleList.Clear();
            GetSingleMonsterList(vMonsterInfoList[i], out tmpSingleList);
            if (tmpSingleList == null)
                continue;
            tmpResult.AddRange(tmpSingleList);
        }
        vResultList = tmpResult;
    }
    public void GetStageMonsterInfo(List<StageMonsterInfo> vMonsterInfoList, out List<RefreshMonsterInfo> vCommonList, out Dictionary<int, List<RefreshMonsterInfo>> vSpecialDic)
    {
        if (vMonsterInfoList == null)
        {
            vCommonList = null;
            vSpecialDic = null;
            return;
        }

        List<RefreshMonsterInfo> tmpCommonList = new List<RefreshMonsterInfo>();
        Dictionary<int, List<RefreshMonsterInfo>> tmpSpecialDic = new Dictionary<int, List<RefreshMonsterInfo>>();
        for (int i = 0; i < vMonsterInfoList.Count; i++)
        {
            if (vMonsterInfoList[i] == null)
                continue;
            if ((vMonsterInfoList[i].TimeSlot.Count == 1) && (vMonsterInfoList[i].TimeSlot[0] == 0))
            {
                int tmpKey = 0;
                List<RefreshMonsterInfo> tmpSpecialList = new List<RefreshMonsterInfo>();
                GetSingleMonsterList(vMonsterInfoList[i], out tmpKey, out tmpSpecialList);
                if (tmpSpecialList == null)
                    continue;
                if (tmpSpecialDic.ContainsKey(tmpKey))
                    tmpSpecialDic[tmpKey].AddRange(tmpSpecialList);
                else
                    tmpSpecialDic.Add(tmpKey, tmpSpecialList);
            }
            //【AtSea-关卡特殊刷怪】------------------------------------------------------------------------------------------------------------------//
            else if ((vMonsterInfoList[i].TimeSlot.Count == 1) && (vMonsterInfoList[i].TimeSlot[0] == -1))
            {
                Dictionary<int, uint> tmpRandomDic = new Dictionary<int, uint>();
                //总权重//
                int tmpTotalValue = 0;
                foreach (KeyValuePair<uint, byte> tmpInfo in vMonsterInfoList[i].MonsterInfo)
                {
                    for (int j = 0; j < tmpInfo.Value; j++)
                    {
                        tmpRandomDic.Add(tmpTotalValue + j, tmpInfo.Key);
                    }
                    tmpTotalValue += tmpInfo.Value;
                }
                RefreshMonsterInfo tmpMonsterInfo = new RefreshMonsterInfo(0, vMonsterInfoList[i].MonsterNum, vMonsterInfoList[i].InitPosX);
                for (int j = 0; j < vMonsterInfoList[i].MonsterNum; j++)
                {
                    int tmpRandomKey = UnityEngine.Random.Range(0, tmpTotalValue);
                    if (!tmpRandomDic.ContainsKey(tmpRandomKey))
                        continue;
                    tmpMonsterInfo.AddSingleID(tmpRandomDic[tmpRandomKey]);
                }
                if (tmpCommonList.Count == 0)
                    tmpCommonList.Add(tmpMonsterInfo);
                else
                    tmpCommonList.Insert(0, tmpMonsterInfo);
            }
            //【AtSea-关卡特殊刷怪】------------------------------------------------------------------------------------------------------------------//
            else
            {
                List<RefreshMonsterInfo> tmpSingleList = new List<RefreshMonsterInfo>();
                GetSingleMonsterList(vMonsterInfoList[i], out tmpSingleList);
                if (tmpSingleList == null)
                    continue;
                tmpCommonList.AddRange(tmpSingleList);
            }
        }

        tmpCommonList.Sort(SortList);
        vCommonList = tmpCommonList;
        vSpecialDic = tmpSpecialDic;
    }
    /// <summary>
    /// 获取单次刷新怪物信息
    /// </summary>
    /// <param name="vSingleInfo"></param>
    /// <returns></returns>
    private void GetSingleMonsterList(StageMonsterInfo vSingleInfo, out List<RefreshMonsterInfo> vResultList)
    {
        if (vSingleInfo == null)
        {
            vResultList = null;
            return;
        }

        //怪物刷新次数//
        int tmpRefreshCount = (vSingleInfo.TimeSlot[1] - vSingleInfo.TimeSlot[0]) / vSingleInfo.TimeInterval + 1;
        //随机怪物ID//
        Dictionary<int, uint> tmpRandomDic = new Dictionary<int, uint>();
        //总权重//
        int tmpTotalValue = 0;
        foreach (KeyValuePair<uint, byte> tmpInfo in vSingleInfo.MonsterInfo)
        {
            for (int i = 0; i < tmpInfo.Value; i++)
            {
                tmpRandomDic.Add(tmpTotalValue + i, tmpInfo.Key);
            }
            tmpTotalValue += tmpInfo.Value;
        }
        List<RefreshMonsterInfo> tmpResultList = new List<RefreshMonsterInfo>();
        for (int i = 0; i < tmpRefreshCount; i++)
        {
            RefreshMonsterInfo tmp = new RefreshMonsterInfo(CommonFunction.GetSecondTimeByMilliSecond(vSingleInfo.TimeSlot[0] + i * vSingleInfo.TimeInterval), vSingleInfo.MonsterNum, vSingleInfo.InitPosX);
            for (int j = 0; j < vSingleInfo.MonsterNum; j++)
            {
                int tmpRandomKey = UnityEngine.Random.Range(0, tmpTotalValue);
                if (!tmpRandomDic.ContainsKey(tmpRandomKey))
                    continue;
                tmp.AddSingleID(tmpRandomDic[tmpRandomKey]);
            }
            tmpResultList.Add(tmp);
        }
        vResultList = tmpResultList;
    }
    private void GetSingleMonsterList(StageMonsterInfo vSingleInfo, out int vKey, out List<RefreshMonsterInfo> vValueList)
    {
        if (vSingleInfo == null)
        {
            vKey = 0;
            vValueList = null;
            return;
        }

        //随机怪物ID//
        Dictionary<int, uint> tmpRandomDic = new Dictionary<int, uint>();
        //总权重//
        int tmpTotalValue = 0;
        foreach (KeyValuePair<uint, byte> tmpInfo in vSingleInfo.MonsterInfo)
        {
            for (int i = 0; i < tmpInfo.Value; i++)
            {
                tmpRandomDic.Add(tmpTotalValue + i, tmpInfo.Key);
            }
            tmpTotalValue += tmpInfo.Value;
        }
        List<RefreshMonsterInfo> tmpResultList = new List<RefreshMonsterInfo>();
        RefreshMonsterInfo tmp = new RefreshMonsterInfo(vSingleInfo.TimeSlot[0], vSingleInfo.MonsterNum, vSingleInfo.InitPosX);
        for (int j = 0; j < vSingleInfo.MonsterNum; j++)
        {
            int tmpRandomKey = UnityEngine.Random.Range(0, tmpTotalValue);
            if (!tmpRandomDic.ContainsKey(tmpRandomKey))
                continue;
            tmp.AddSingleID(tmpRandomDic[tmpRandomKey]);
        }
        tmpResultList.Add(tmp);
        vKey = vSingleInfo.TimeInterval;
        vValueList = tmpResultList;
    }

    private int SortList(RefreshMonsterInfo vA, RefreshMonsterInfo vB)
    {
        if (vA.RefreshTime >= vB.RefreshTime)
            return 1;
        else
            return -1;
    }

    /// <summary>
    /// 本地保存/读取自动战斗状态
    /// </summary>
    private void Save_AutoFight_Status()
    {
        PlayerPrefsTool.WriteInt(AppPrefEnum.AutoFightStatus, AutoFightStatus);
        Save_AutoFight_Move();
        Save_AutoFight_Skill();
        Save_AutoFight_Summon();
        Save_FightSpeed();
    }
    private void Load_AutoFight_Status()
    {
        AutoFightStatus = PlayerPrefsTool.ReadInt(AppPrefEnum.AutoFightStatus);
        Load_AutoFight_Move();
        Load_AutoFight_Skill();
        Load_AutoFight_Summon();
        Load_FightSpeed();
    }

    private void Save_AutoFight_Move()
    {
        PlayerPrefsTool.WriteBool(AppPrefEnum.AutoFightMove, IsAutoMove);
    }
    private void Load_AutoFight_Move()
    {
        IsAutoMove = PlayerPrefsTool.ReadBool(AppPrefEnum.AutoFightMove);
    }

    private void Save_AutoFight_Skill()
    {
        PlayerPrefsTool.WriteBool(AppPrefEnum.AutoFightSkill, IsAutoSkill);
    }
    private void Load_AutoFight_Skill()
    {
        IsAutoSkill = PlayerPrefsTool.ReadBool(AppPrefEnum.AutoFightSkill);
    }

    private void Save_AutoFight_Summon()
    {
        PlayerPrefsTool.WriteBool(AppPrefEnum.AutoFightSummon, IsAutoSummon);
    }
    private void Load_AutoFight_Summon()
    {
        IsAutoSummon = PlayerPrefsTool.ReadBool(AppPrefEnum.AutoFightSummon);
    }

    private void Save_FightSpeed()
    {
        PlayerPrefsTool.WriteFloat(AppPrefEnum.FightSpeed, fightSpeed);
    }
    private void Load_FightSpeed()
    {
        fightSpeed = PlayerPrefsTool.ReadFloat(AppPrefEnum.FightSpeed);
        if (fightSpeed < GlobalConst.MIN_FIGHT_SPEED)
            fightSpeed = GlobalConst.MIN_FIGHT_SPEED;
        if (fightSpeed > GlobalConst.MAX_FIGHT_SPEED)
            fightSpeed = GlobalConst.MAX_FIGHT_SPEED;
    }
}
