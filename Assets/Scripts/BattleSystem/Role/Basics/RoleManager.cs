using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 清空角色类型数据
/// </summary>
public class ClearRoleInfo
{
    public EFightCamp ClearRoleCamp;
    public ERoleType ClearRoleType;

    public ClearRoleInfo(EFightCamp vClearRoleCamp, ERoleType vClearRoleType)
    {
        ClearRoleCamp = vClearRoleCamp;
        ClearRoleType = vClearRoleType;
    }
}
public class RoleManager : Singleton<RoleManager>
{
    /// <summary>
    /// 护送目标固定层级
    /// </summary>
    private const int ESCORTSORT = 4;
    /// <summary>
    /// 界面演示角色固定层级
    /// </summary>
    private const int UIROLESORT = 8;


    /// <summary>
    /// 场景角色唯一ID
    /// </summary>
    private int _SceneRoleSingleID;
    /// <summary>
    /// 英雄角色ID
    /// </summary>
    private int _HeroID;
    /// <summary>
    /// 是否暂停状态
    /// </summary>
    private bool isPauseStatus;
    /// <summary>
    /// 角色队列
    /// </summary>
    private Dictionary<int, RoleBase> _DicRole = new Dictionary<int, RoleBase>();
    /// <summary>
    /// 是否已经设定角色比例
    /// </summary>
    private bool isSetRoleScaleValue = false;
    /// <summary>
    /// 角色比例-界面
    /// </summary>
    private float ROLE_SCALE_UI_HERO = 0;//0.5f;
    /// <summary>
    /// 角色比例-城楼射手
    /// </summary>
    private float ROLE_SCALE_UI_SHOOTER = 0;//0.3f;
    /// <summary>
    /// 角色比例-英雄
    /// </summary>
    private float ROLE_SCALE_HERO = 0;//0.35f;
    /// <summary>
    /// 角色比例-士兵
    /// </summary>
    private float ROLE_SCALE_SOLDIER = 0;//0.3f;
    /// <summary>
    /// 角色比例-怪物
    /// </summary>
    private float ROLE_SCALE_MONSTER = 0;//0.3f;
    /// <summary>
    /// 角色比例-BOSS
    /// </summary>
    private float ROLE_SCALE_BOSS = 0;//0.45f;
    /// <summary>
    /// 角色比例-超级兵
    /// </summary>
    private float ROLE_SCALE_SUPER = 0;//0.45f;
    /// <summary>
    /// 角色比例-护送目标
    /// </summary>
    private float ROLE_SCALE_ESCORT = 0;//0.4f;
    private Vector3 roleBoxSize = new Vector3(100, 5000, 1000);


    /// <summary>
    /// 英雄角色ID
    /// </summary>
    public int Get_HeroID
    {
        get {
            return _HeroID;
        }
    }
    /// <summary>
    /// 英雄
    /// </summary>
    public RoleBase Get_Hero
    {
        get {
            return GetSingleRoleBySingleID(_HeroID);
        }
    }
    /// <summary>
    /// 获取角色队列
    /// </summary>
    public Dictionary<int, RoleBase> Get_RoleDic
    {
        get {
            return _DicRole;
        }
    }
    public bool Get_IsPauseStatus
    {
        get {
            return isPauseStatus;
        }
    }
    public float Get_UIHero_Scale
    {
        get {
            return ROLE_SCALE_UI_HERO;
        }
    }


    public void Initialize()
    {
        isPauseStatus = false;
        isSetRoleScaleValue = false;
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleCreate, CommandEvent_RoleCreate);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleDelete, CommandEvent_RoleDelete);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleClear, CommandEvent_RoleClear);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_SetRoleScaleValue, CommandEvent_SetRoleScaleValue);
    }

    public void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleCreate, CommandEvent_RoleCreate);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleDelete, CommandEvent_RoleDelete);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleClear, CommandEvent_RoleClear);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_SetRoleScaleValue, CommandEvent_SetRoleScaleValue);
    }


    /// <summary>
    /// 创建一个角色
    /// </summary>
    private void CommandEvent_RoleCreate(object vDataObj)
    {
        if (vDataObj == null)
            return;

        CData_CreateRole tmpInfo = (CData_CreateRole)vDataObj;
        if (tmpInfo == null)
            return;

        CreateSingleRole(tmpInfo.RoleInfo, tmpInfo.UID, tmpInfo.PathIndex, tmpInfo.Type, tmpInfo.InitPosX,
            tmpInfo.HeroGender, tmpInfo.FightCamp, tmpInfo.FightType, tmpInfo.Parent, tmpInfo.OnLoad, tmpInfo.IsActive);
    }

    /// <summary>
    /// 删除一个角色
    /// </summary>
    /// <param name="vRoleSingleID">角色唯一ID</param>
    private void CommandEvent_RoleDelete(object vDataObj)
    {
        if (vDataObj == null)
            return;
        if (_DicRole == null)
            return;
        if (_DicRole.Count <= 0)
            return;

        int tmpKeyValue = (int)vDataObj;
        if (!_DicRole.ContainsKey(tmpKeyValue))
            return;
        _DicRole[tmpKeyValue].UnInitialization(false);
        _DicRole.Remove(tmpKeyValue);
    }

    /// <summary>
    /// 清空角色信息
    /// </summary>
    private void CommandEvent_RoleClear(object vDataObj)
    {
        if (vDataObj == null)
        {
            _HeroID = 0;
            _SceneRoleSingleID = 0;

            foreach (KeyValuePair<int, RoleBase> tmpInfo in _DicRole)
            {
                if (tmpInfo.Value == null)
                    continue;
                tmpInfo.Value.UnInitialization();
            }
            _DicRole.Clear();
        }
        else
        {
            ClearRoleInfo tmpRoleInfo = (ClearRoleInfo)vDataObj;

            List<int> tmpIndex = new List<int>();
            foreach (KeyValuePair<int, RoleBase> tmpInfo in _DicRole)
            {
                if (tmpInfo.Value != null)
                {
                    if ((tmpInfo.Value.Get_RoleType == tmpRoleInfo.ClearRoleType) && (tmpInfo.Value.Get_RoleCamp == tmpRoleInfo.ClearRoleCamp))
                    {
                        tmpInfo.Value.UnInitialization();
                        tmpIndex.Add(tmpInfo.Key);
                    }
                }
            }
            for (int i = 0; i < tmpIndex.Count; i++)
            {
                _DicRole.Remove(tmpIndex[i]);
            }
        }
    }

    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightSetPause(object vDataObj)
    {
        isPauseStatus = true;
    }

    /// <summary>
    /// 继续
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightSetResume(object vDataObj)
    {
        isPauseStatus = false;
    }

    /// <summary>
    /// 设置角色比例
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SetRoleScaleValue(object vDataObj)
    {
        if (isSetRoleScaleValue)
            return;
        isSetRoleScaleValue = true;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_UIHERO), out ROLE_SCALE_UI_HERO))
            ROLE_SCALE_UI_HERO = 0.5f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_UISHOOTER), out ROLE_SCALE_UI_SHOOTER))
            ROLE_SCALE_UI_SHOOTER = 0.3f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_HERO), out ROLE_SCALE_HERO))
            ROLE_SCALE_HERO = 0.35f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_SOLDIER), out ROLE_SCALE_SOLDIER))
            ROLE_SCALE_SOLDIER = 0.3f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_MONSTER), out ROLE_SCALE_MONSTER))
            ROLE_SCALE_MONSTER = 0.3f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_BOSS), out ROLE_SCALE_BOSS))
            ROLE_SCALE_BOSS = 0.45f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_SUPER), out ROLE_SCALE_SUPER))
            ROLE_SCALE_SUPER = 0.45f;
        if (!float.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_ROLESCALE_ESCORT), out ROLE_SCALE_ESCORT))
            ROLE_SCALE_ESCORT = 0.4f;
    }



    /// <summary>
    /// 创建一个角色
    /// </summary>
    /// <param name="vRoleData">战斗数据</param>
    /// <param name="vUID">UID[0-英雄 怪物 界面显示  UID-战斗场景武将]</param>
    /// <param name="vRolePathIndex">场景路径索引[界面演示表示索引]</param>
    /// <param name="vRoleType">角色种类</param>
    /// <param name="vHeroGender">性别[英雄]</param>
    /// <param name="vFightCamp">角色阵营[默认值表示界面]</param>
    /// <param name="vFightType">战斗场景[默认值表示界面]</param>
    /// <param name="vParent">父物件</param>
    /// <param name="onLoad">回调函数[界面]</param>
    /// <param name="vIsActive">是否激活显示</param>
    public void CreateSingleRole(object vRoleData, ulong vUID, int vRolePathIndex, ERoleType vRoleType, int vInitPosX = 0, EHeroGender vHeroGender = EHeroGender.ehgNone, 
        EFightCamp vFightCamp = EFightCamp.efcNone, EFightType vFightType = EFightType.eftUI, Transform vParent = null, System.Action<RoleBase> onLoad = null, bool vIsActive = true)
    {
        //属性//
        ShowInfoBase tmpInfo = (ShowInfoBase)vRoleData;
        if (tmpInfo == null)
            return;
        //资源路径//
        string tmpResourcePath = string.Empty;
        //缩放比例//
        Vector3 tmpScals = Vector3.one;

        switch (vRoleType)
        {
            case ERoleType.ertHero:
                {
                    tmpResourcePath = CommonFunction.GetHeroResourceNameByGender(vHeroGender);
                }
                break;
            case ERoleType.ertSoldier:
                {
                    SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(tmpInfo.KeyData);
                    if (tmpSoldier == null) return;
                    tmpResourcePath = tmpSoldier.Animation;
                }
                break;
            case ERoleType.ertShooter:
                {
                    CastleAttributeInfo tmpShooter = ConfigManager.Instance.mCastleConfig.FindByID(tmpInfo.KeyData);
                    if (tmpShooter == null) return;
                    tmpResourcePath = tmpShooter.fight_source;
                }
                break;
            case ERoleType.ertMonster:
                {
                    MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(tmpInfo.KeyData);
                    if (tmpMonster == null) return;
                    tmpResourcePath = tmpMonster.ResourceID;
                }
                break;
            case ERoleType.ertEscort:
                {
                    StageInfo tmpStage = ConfigManager.Instance.mStageData.GetInfoByID(tmpInfo.KeyData);
                    if (tmpStage == null) return;
                    tmpResourcePath = tmpStage.EscortTarget;
                }
                break;
            case ERoleType.ertPet:
                {
                    SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(tmpInfo.KeyData);
                    if (tmpSoldier == null) return;
                    tmpResourcePath = tmpSoldier.Animation;
                }
                break;
            default:
                { }
                break;
        }
        if (string.IsNullOrEmpty(tmpResourcePath))
            return;
        
        ResourceLoadManager.Instance.LoadCharacter(tmpResourcePath, ResourceLoadType.AssetBundle, (obj) => 
        {
            if (obj == null)
            {
                AloneObjectCache.Instance.Delete();
                if (onLoad != null)
                    onLoad(null);
                return;
            }
            AloneObjectCache.Instance.LoadGameObject(obj, (go) => 
            {
                if (go == null)
                {
                    AloneObjectCache.Instance.Delete();
                    if (onLoad != null)
                        onLoad(null);
                    return;
                }
                RoleBase tmpRoleBase = go.GetComponent<RoleBase>();
                if (vFightType == EFightType.eftUI)
                {
                    if (tmpRoleBase == null)
                        tmpRoleBase = go.AddComponent<RoleUIShow>();
                    float tmpScaleValue = ROLE_SCALE_UI_SHOOTER;
                    if (vRoleType == ERoleType.ertHero)
                        tmpScaleValue = ROLE_SCALE_UI_HERO;
                    if (vRoleType == ERoleType.ertSoldier)
                    {
                        SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(tmpInfo.KeyData);
                        if (tmpSoldier != null)
                            tmpScaleValue = tmpSoldier.Scale;
                    }
                    tmpScals *= tmpScaleValue;

                    SetSingleRoleInfo(go, vParent, Vector3.one * tmpScaleValue, Vector3.zero);
                    go.name = string.Format("UIRole_{0}", vRolePathIndex);
                    tmpRoleBase.Initialization(vRolePathIndex, vRoleData, vRoleType, UIROLESORT, 1, vUID, vFightCamp, vFightType, vHeroGender);
                    if (onLoad != null)
                        onLoad(tmpRoleBase);
                    go.SetActive(vIsActive);
                }
                else
                {
                    if (SceneManager.Instance.Get_CurScene != null)
                    {
                        _SceneRoleSingleID += 1;
                        if (tmpRoleBase == null)
                        {
                            CreateSingleRoleOperate(go, vRoleData, vParent, _SceneRoleSingleID, tmpInfo.KeyData, vRolePathIndex, 
                                vUID, vRoleType, vFightCamp, vFightType, vHeroGender, vInitPosX, onLoad, vIsActive);
                        }
                        else
                        {
                            Main.Instance.StartCoroutine(ReCreateSingleDelayDestroyRoleOperate(go, vRoleData, vParent, _SceneRoleSingleID, 
                                tmpInfo.KeyData, vRolePathIndex, vUID, vRoleType, vFightCamp, vFightType, vHeroGender, vInitPosX, onLoad, vIsActive));
                        }
                    }
                    else
                    {
                        AloneObjectCache.Instance.Delete();
                        if (onLoad != null)
                            onLoad(null);
                    }
                }
            });
        });
    }

    private IEnumerator ReCreateSingleDelayDestroyRoleOperate(GameObject vObj, object vRoleData, Transform vParent, int vSceneRoleSingleID, uint vKeyValue, int vRolePathIndex, ulong vUID,
        ERoleType vRoleType, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender = EHeroGender.ehgNone, int vInitPosX = 0, System.Action<RoleBase> onLoad = null, bool vIsActive = true)
    {
        if (vObj == null)
            yield break;
        int tmpSceneRoleSingleID = vSceneRoleSingleID;
        yield return (vObj.GetComponent<RoleBase>() == null);
        CreateSingleRoleOperate(vObj, vRoleData, vParent, tmpSceneRoleSingleID, vKeyValue, vRolePathIndex, vUID, vRoleType, vFightCamp, vFightType, vHeroGender, vInitPosX, onLoad, vIsActive);
    }
    private void CreateSingleRoleOperate(GameObject vObj, object vRoleData, Transform vParent, int vSceneRoleSingleID, uint vKeyValue, int vRolePathIndex, ulong vUID, ERoleType vRoleType, 
        EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender = EHeroGender.ehgNone, int vInitPosX = 0, System.Action<RoleBase> onLoad = null, bool vIsActive = true)
    {
        int tmpSceneRoleSingleID = vSceneRoleSingleID;
        FightSceneBase tmpScene = SceneManager.Instance.Get_CurScene;
        if ((vObj == null) || (tmpScene == null) || (vRoleData == null))
        {
            AloneObjectCache.Instance.Delete();
            if (onLoad != null)
                onLoad(null);
            return;
        }
        if (vObj.GetComponent<BoxCollider>() == null)
        {
            vObj.AddComponent<BoxCollider>();
        }
        vObj.GetComponent<BoxCollider>().size = roleBoxSize;
        vObj.GetComponent<BoxCollider>().enabled = true;

        RoleBase tmpRoleBase = null;
        Transform tmpParent = vParent;
        Vector3 tmpScals = Vector3.one;
        float tmpShowSpeed = SceneManager.Instance.Get_FightSpeed;
        switch (vRoleType)
        {
            case ERoleType.ertHero:
                {
                    if (vFightCamp == EFightCamp.efcSelf)
                    {
                        if ((vFightType != EFightType.eftPVP) && (vFightType != EFightType.eftSlave) && 
                            (vFightType != EFightType.eftServerHegemony) && (vFightType != EFightType.eftQualifying))
                        {
                            tmpRoleBase = vObj.AddComponent<RoleAIHero_PVE>();
                        }
                        else
                        {
                            tmpRoleBase = vObj.AddComponent<RoleAIHero>();
                        }
                        tmpParent = tmpScene.transHero;
                        _HeroID = tmpSceneRoleSingleID;
                    }
                    else
                    {
                        tmpRoleBase = vObj.AddComponent<RoleAIHero>();
                        tmpParent = tmpScene.transEnemy;
                    }
                    tmpScals = Vector3.one * ROLE_SCALE_HERO;
                }
                break;
            case ERoleType.ertSoldier:
                {
                    tmpRoleBase = vObj.AddComponent<RoleSoldier>();
                    if (vFightCamp == EFightCamp.efcSelf)
                        tmpParent = tmpScene.transSelf;
                    else
                        tmpParent = tmpScene.transEnemy;
                    tmpScals = Vector3.one * ROLE_SCALE_SOLDIER;
                }
                break;
            case ERoleType.ertShooter:
                {
                    tmpRoleBase = vObj.AddComponent<RoleShooter>();
                    tmpScals = Vector3.one * ROLE_SCALE_SOLDIER;
                    vObj.GetComponent<BoxCollider>().enabled = false;
                }
                break;
            case ERoleType.ertPet:
                {
                    tmpRoleBase = vObj.AddComponent<RolePet>();
                    tmpScals = Vector3.one * ROLE_SCALE_SOLDIER;
                    vObj.GetComponent<BoxCollider>().enabled = false;
                }
                break;
            case ERoleType.ertMonster:
                {
                    tmpRoleBase = vObj.AddComponent<RoleMonster>();
                    if (tmpParent == null)
                    {
                        tmpParent = tmpScene.transEnemy;
                    }
                    MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(vKeyValue);
                    if (tmpMonster != null)
                    {
                        if (tmpMonster.IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
                        {
                            tmpScals = Vector3.one * ROLE_SCALE_BOSS;
                        }
                        else if (tmpMonster.IsBoss == GlobalConst.MONSTER_TYPE_SUPER)
                        {
                            tmpScals = Vector3.one * ROLE_SCALE_SUPER;
                        }
                        else
                        {
                            tmpScals = Vector3.one * ROLE_SCALE_MONSTER;
                        }
                    }
                }
                break;
            case ERoleType.ertEscort:
                {
                    tmpRoleBase = vObj.AddComponent<RoleEscort>();
                    tmpScals = Vector3.one * ROLE_SCALE_ESCORT;
                }
                break;
            default:
                {
                    AloneObjectCache.Instance.FreeObject(vObj);
                    if (onLoad != null)
                        onLoad(null);
                    return;
                }
        }

        _DicRole.Add(tmpSceneRoleSingleID, tmpRoleBase);
        float tmpPosY = tmpScene.GetSingleRolePath(vRolePathIndex);
        if (vRoleType == ERoleType.ertEscort)
        {
            tmpPosY -= tmpParent.transform.localPosition.y;
        }
        SetSingleRoleInfo(vObj, tmpParent, tmpScals, new Vector3(vInitPosX, tmpPosY, 0));
        vObj.name = string.Format("FightRole_{0}", tmpSceneRoleSingleID);
        tmpRoleBase.Initialization(tmpSceneRoleSingleID, vRoleData, vRoleType, vRolePathIndex, tmpShowSpeed, vUID, vFightCamp, vFightType, vHeroGender);

        if (onLoad != null)
            onLoad(tmpRoleBase);
        vObj.SetActive(vIsActive);
    }


    /// <summary>
    /// 设置单个角色信息
    /// </summary>
    /// <param name="vRoleObj">角色物件</param>
    /// <param name="vParent">父物件</param>
    /// <param name="vScale">比例</param>
    /// <param name="vPosition">位置</param>
    private void SetSingleRoleInfo(GameObject vRoleObj, Transform vParent, Vector3 vScale, Vector3 vPosition)
    {
        if (vRoleObj == null)
            return;
        if (vParent == null)
            return;
        vRoleObj.transform.parent = vParent;
        vRoleObj.layer = vParent.gameObject.layer;
        vRoleObj.transform.localScale = vScale;
        vRoleObj.transform.localPosition = vPosition;
    }
    
    
    /// <summary>
    /// 通过Key值查找一个角色
    /// </summary>
    /// <param name="vKey">战斗场景-唯一ID  界面-索引</param>
    /// <returns></returns>
    public RoleBase GetSingleRoleBySingleID(int vKey)
    {
        if (_DicRole == null)
            return null;
        if (_DicRole.Count <= 0)
            return null;
        if (!_DicRole.ContainsKey(vKey))
            return null;
        return _DicRole[vKey];
    }

}
