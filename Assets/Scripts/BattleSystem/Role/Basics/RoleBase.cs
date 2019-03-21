using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TdSpine;
using Assets.Script.Common;

/// <summary>
/// 角色基类
/// </summary>
public class RoleBase : RoleAction
{
    /// <summary>
    /// 是否已经初始化
    /// </summary>
    private bool isAlreadyInitialization = false;
    /// <summary>
    /// 场景角色唯一ID
    /// </summary>
    protected int sceneRoleID;
    /// <summary>
    /// 敌人
    /// </summary>
    protected RoleAttribute roleEnemy;
    /// <summary>
    /// 战斗场景管理器
    /// </summary>
    protected SceneManager pSceneManager;
    /// <summary>
    /// 屏幕X坐标比例
    /// </summary>
    protected float screenProportion_X;
    /// <summary>
    /// 角色UID
    /// </summary>
    public ulong mUID;
    /// <summary>
    /// 是否通过命令设置暂停[true-是 false-不是]
    /// </summary>
    protected bool isCommandPause;
    /// <summary>
    /// 角色技能效果器
    /// </summary>
    protected SkillsDepot skillsDepot;
    /// <summary>
    /// 角色性别
    /// </summary>
    protected EHeroGender roleGender;
    /// <summary>
    /// 是否使用技能
    /// </summary>
    protected bool isSkilling;

    private bool isShowHitEffect = false;
    private SpineBase hitSpineEffect = null;
    private Transform transParent = null;
    private Vector3 transScale;


    /// <summary>
    /// 场景唯一ID
    /// </summary>
    public int Get_SingleID
    {
        get {
            return sceneRoleID;
        }
    }
    /// <summary>
    /// 获取敌人数据
    /// </summary>
    public RoleAttribute Get_Enemy
    {
        get {
            if (roleEnemy == null)
                return null;
            if (!roleEnemy.IsLive())
                return null;
            return roleEnemy;
        }
    }
    /// <summary>
    /// 获取角色技能效果器
    /// </summary>
    public SkillsDepot Get_skillsDepot
    {
        get {
            return skillsDepot;
        }
    }
    /// <summary>
    /// 获取角色性别
    /// </summary>
    public EHeroGender Get_RoleGender
    {
        get {
            return roleGender;
        }
    }
    /// <summary>
    /// 是否使用技能
    /// </summary>
    public bool Get_IsSkilling
    {
        get {
            return isSkilling;
        }
    }
    public Transform Get_TransParent
    {
        get {
            return transParent;
        }
    }
    public Vector3 Get_TransScale
    {
        get {
            return transScale;
        }
    }


    public virtual void ReInitInfo_Role()
    {
        ReInitInfo_Attribute();
        ReInitInfo_Action();
        skillsDepot = null;
        hitSpineEffect = null;
        isAlreadyInitialization = false;
        isShowHitEffect = false;
        isCommandPause = false;
    }

    /// <summary>
    /// 初始化角色信息
    /// </summary>
    /// <param name="vSceneRoleID">唯一ID</param>
    /// <param name="vRoleInfo"></param>
    /// <param name="vRoleType">角色种类</param>
    /// <param name="vRolePathIndex">行走路径</param>
    public virtual void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex, 
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightCountdownOperate, CommandEvent_FightCountdownOperate);

        pRoleBase = this;
        transParent = this.transform.parent;
        transScale = this.transform.localScale;
        if (pSceneManager == null)
            pSceneManager = SceneManager.Instance;
        if (_MainSpine == null)
        {
            if (this.gameObject.GetComponent<MainSpine>() == null)
                _MainSpine = this.gameObject.AddComponent<MainSpine>();
            else
                _MainSpine = this.gameObject.GetComponent<MainSpine>();
        }
        _MainSpine.ResetAlph(1);
        isCommandPause = false;
        mUID = vUID;
        isSkilling = false;
        roleGender = vHeroGender;
        fightType = vFightType;
        rolePathIndex = vRolePathIndex;
        
        if (isAlreadyInitialization)
            return;
        isAlreadyInitialization = true;

        sceneRoleID = vSceneRoleID;

        SetRoleCamp(vFightCamp);
        RoleFightType = vFightType;
        SetRoleType(vRoleType);

        if(this.Get_RoleCamp == EFightCamp.efcEnemy)
        {
            if (vRoleType == ERoleType.ertHero)
                this.pRpleHP = new EnemyHeroHp();
            else if (vRoleType == ERoleType.ertBarracks)
                this.pRpleHP = null;
            else if (vRoleType == ERoleType.ertEscort)
                this.pRpleHP = null;
            else
                this.pRpleHP = new EnemySoldierHp();
        }
        else
        {
            if (vRoleType == ERoleType.ertHero)
                this.pRpleHP = new HeroHp();
            else if (vRoleType == ERoleType.ertEscort)
                this.pRpleHP = new EscortHp();
            else if (vRoleType == ERoleType.ertBarracks)
                this.pRpleHP = null;
            else if (vRoleType == ERoleType.ertEscort)
                this.pRpleHP = null;
            else if (vRoleType == ERoleType.ertShooter)
                this.pRpleHP = null;
            else
                this.pRpleHP = new SoldierHp();
        }
        if (this.pRpleHP != null)
            this.pRpleHP.Instance(this,vRoleType,Vector3.zero);

        InitScreenProportion();
        _MainSpine.InitSkeletonAnimation();
        if (vRoleType == ERoleType.ertHero)
            Main.Instance.StartCoroutine(SpineEquipInit((vRolePathIndex + 1) * 3));
        else
            _MainSpine.setSortingOrder((vRolePathIndex + 1) * 3);

        InitRoleAttribute((ShowInfoBase)vRoleInfo);
        RandomRoleFightAttribute();
        ReSetShowSpeed(vSpeed);
        SetNormalValue();
        InitAction();
        SetRoleLevel(vRoleType, vFightCamp, vFightType, vUID);

        if (roleType != ERoleType.ertShooter)
        {
            if (!IsLive())
            {
                ExChangeAction<ActionDeath>();
                return;
            }
        }

        if (vFightType == EFightType.eftUI)
            return;
        if (!RoleManager.Instance.Get_IsPauseStatus)
            return;
        isCommandPause = true;
        SetPause();
    }
    private IEnumerator SpineEquipInit(int index)
    {
        yield return 0;
        if (_MainSpine == null)
            yield break;
        _MainSpine.setSortingOrder(index);
    }
    /// <summary>
    /// 销毁角色
    /// </summary>
    public virtual void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightCountdownOperate, CommandEvent_FightCountdownOperate);

        //if ((roleCamp == EFightCamp.efcSelf) && (fightAttribute != null))
        if ((fightAttribute != null))
        {
            if (!isSummon)
            {
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_DeleteSingleSoldierNum, new DeleteSingleSoldierInfo(fightAttribute.KeyData, roleCamp));
            }
        }
        ClearActions();

        if (hitSpineEffect != null)
        {
            hitSpineEffect = null;
        }
        if (gameObject != null)
            gameObject.SetActive(false);
        string tmpObjName = this.name;
        this.tmpSkill = null;
        Destroy(this);
        if (vIsDelete)
        {
            if (_MainSpine != null)
                _MainSpine.UnInit();
            ReInitInfo_Role();
            Destroy(this);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            if (_MainSpine != null)
            {
                _MainSpine.ResetAlph(1);
                _MainSpine.UnInit();
            }
            ReInitInfo_Role();
            AloneObjectCache.Instance.FreeObject(this.gameObject);
        }
    }

    /// <summary>
    /// 战斗停止
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightSetPause(object vDataObj)
    {
        if (isCommandPause)
            return;
        isCommandPause = true;
        SetPause();
    }
    /// <summary>
    /// 战斗继续
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightSetResume(object vDataObj)
    {
        if (!isCommandPause)
            return;
        if (vDataObj == null)
            return;
        if (((bool)vDataObj) == false)
            return;
        isCommandPause = false;
        SetResume();
    }
    /// <summary>
    /// 战斗倒计时操作
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightCountdownOperate(object vDataObj)
    {
        ShowSummon();
        RecoverHeroHPOperate();
    }

    /// <summary>
    /// 恢复血量
    /// </summary>
    private void RecoverHeroHPOperate()
    {
        if (fightType == EFightType.eftUI)
            return;
        if (isCommandPause)
            return;
        if (fightAttribute == null)
            return;
        if (fightAttribute.HPRecovery <= 0)
            return;
        ReSetRoleHP(fightAttribute.HPRecovery, HurtType.Normal, false);
    }

    /// <summary>
    /// 随机角色战斗属性
    /// </summary>
    private void RandomRoleFightAttribute()
    {
        if ((roleType == ERoleType.ertBarracks) || (roleType == ERoleType.ertEscort) || (roleType == ERoleType.ertTransfer) || (roleType == ERoleType.ertHero))
            return;
        if (fightAttribute == null)
            return;
        fightAttribute.Attack = (int)SetSingleFightAttributeRanValue(fightAttribute.Attack, ERandomFightAttributeType.erfatAttack);
        fightAttribute.AttRate = SetSingleFightAttributeRanValue(fightAttribute.AttRate, ERandomFightAttributeType.erfatAttRate);
        fightAttribute.AttDistance = (int)SetSingleFightAttributeRanValue(fightAttribute.AttDistance, ERandomFightAttributeType.erfatAttDistance);
        fightAttribute.MoveSpeed = (int)SetSingleFightAttributeRanValue(fightAttribute.MoveSpeed, ERandomFightAttributeType.erfatMoveSpeed);
    }

    /// <summary>
    /// 设置单个战斗属性数值
    /// </summary>
    /// <param name="vInitValue"></param>
    /// <param name="vType"></param>
    /// <returns></returns>
    private float SetSingleFightAttributeRanValue(float vInitValue, ERandomFightAttributeType vType)
    {
        RandomAttributeInfo tmpRanInfo = ConfigManager.Instance.mRandomAttributeConfig.FindById((int)vType);
        if (tmpRanInfo == null)
            return vInitValue;
        return vInitValue * Random.RandomRange(tmpRanInfo.min, tmpRanInfo.max);
    }

    /// <summary>
    /// 设置角色等级
    /// </summary>
    /// <param name="vRoleType"></param>
    /// <param name="vFightCamp"></param>
    /// <param name="vFightType"></param>
    /// <param name="vUID"></param>
    private void SetRoleLevel(ERoleType vRoleType, EFightCamp vFightCamp, EFightType vFightType, ulong vUID)
    {
        roleLevel = 1;
        if (fightAttribute == null)
            return;
        uint tmpKey = fightAttribute.KeyData;
        switch (vRoleType)
        {
            case ERoleType.ertHero:
                {//英雄//
                    roleLevel = (int)tmpKey;
                }
                break;
            case ERoleType.ertSoldier:
                {//士兵//
                    Soldier tmpSoldier = null;
                    if (vFightCamp == EFightCamp.efcSelf)
                    {
                        tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(vUID);
                    }
                    else
                    {
                        if (vFightType == EFightType.eftExpedition)
                            tmpSoldier = PlayerData.Instance.expeditionSoldierDopot.FindByUid(vUID);
                        else if ((vFightType == EFightType.eftPVP) || (vFightType == EFightType.eftSlave) || (vFightType == EFightType.eftServerHegemony) || (vFightType == EFightType.eftQualifying))
                        {
                            if ((UISystem.Instance.FightView != null) || (UISystem.Instance.FightView.fightPlayer_Enemy != null) || (UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList != null))
                            {
                                for (int i = 0; i < UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList.Count; i++)
                                {
                                    if (UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList[i].mSoldier.uId == vUID)
                                        tmpSoldier = UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList[i].mSoldier;
                                }
                            }
                        }
                    }
                    if (tmpSoldier == null)
                        return;
                    roleLevel = tmpSoldier.Level;
                }
                break;
            case ERoleType.ertShooter:
                {//城堡射手//
                    if (PlayerData.Instance.mShooterList == null)
                        return;
                    for (int i = 0; i < PlayerData.Instance.mShooterList.Count; i++)
                    {
                        if (PlayerData.Instance.mShooterList[i] == null)
                            continue;
                        if (PlayerData.Instance.mShooterList[i].mID != tmpKey)
                            continue;
                        roleLevel = PlayerData.Instance.mShooterList[i].mLevel;
                        break;
                    }
                }
                break;
            case ERoleType.ertMonster:
                {//怪物//
                    MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(tmpKey);
                    if (tmpMonster != null)
                        roleLevel = tmpMonster.Level;
                }
                break;
            default:
                {
                    return;
                }
        }
    }

    /// <summary>
    /// 设置移动和攻击的标准值
    /// </summary>
    private void SetNormalValue()
    {
        if (fightAttribute == null)
            return;
        normalSpeed = 1;
        normalInterval = 1;
        if (roleType == ERoleType.ertHero)
        {
            HeroAttributeInfo tmpHero = ConfigManager.Instance.mHeroData.FindByLevel((int)fightAttribute.KeyData);
            if (tmpHero == null)
                return;
            normalSpeed = tmpHero.NormalSpeed;
            normalInterval = CommonFunction.GetSecondTimeByMilliSecond(tmpHero.NormalInterval);
        }
        else if (roleType == ERoleType.ertSoldier)
        {
            SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(fightAttribute.KeyData);
            if (tmpSoldier == null)
                return;
            normalSpeed = tmpSoldier.NormalSpeed;
            normalInterval = CommonFunction.GetSecondTimeByMilliSecond(tmpSoldier.NormalInterval);
        }
        else if (roleType == ERoleType.ertMonster)
        {
            MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(fightAttribute.KeyData);
            if (tmpMonster == null)
                return;
            normalSpeed = tmpMonster.NormalSpeed;
            normalInterval = CommonFunction.GetSecondTimeByMilliSecond(tmpMonster.NormalInterval);
        }
        else if (roleType == ERoleType.ertShooter)
        {
            normalSpeed = fightAttribute.MoveSpeed;
            normalInterval = fightAttribute.AttRate;
        }
        else if (roleType == ERoleType.ertEscort)
        {
            normalSpeed = fightAttribute.MoveSpeed;
            normalInterval = fightAttribute.AttRate;
        }
    }

    /// <summary>
    /// 初始化屏幕比例
    /// </summary>
    protected void InitScreenProportion()
    {
        screenProportion_X = 0;
        if (pSceneManager == null)
            pSceneManager = SceneManager.Instance;
        if (pSceneManager.Get_CurScene == null)
            return;
        screenProportion_X = pSceneManager.Get_CurScene.Get_ScreenProportion_X;
    }

    /// <summary>
    /// 检测敌人是否已经改变
    /// </summary>
    /// <returns>true-改变 false-没有改变</returns>
    public bool CheckEnemyIsChange()
    {
        RoleAttribute tmpEnemy = null;
        //查找对应阵营的敌人//
        if (this.roleCamp == EFightCamp.efcEnemy)
        {
            if (isSubverted)
                tmpEnemy = CommonFunction.FindHitSingleFightObject(this, EFightCamp.efcEnemy);
            else
                tmpEnemy = CommonFunction.FindHitSingleFightObject(this, EFightCamp.efcSelf);
        }
        else
        {
            if (isSubverted)
                tmpEnemy = CommonFunction.FindHitSingleFightObject(this, EFightCamp.efcSelf);
            else
                tmpEnemy = CommonFunction.FindHitSingleFightObject(this, EFightCamp.efcEnemy);
        }



        if (tmpEnemy == null)
        {
            if (roleEnemy == null)
                return false;
            else
            {
                roleEnemy = null;
                return true;
            }
        }
        else
        {
            if (tmpEnemy.CheckIsOutSide())
            {
                roleEnemy = null;
                return true;
            }
            if (roleEnemy == null)
                roleEnemy = tmpEnemy;
            else
            {
                if (roleEnemy.name.Equals(tmpEnemy.name))
                    return false;
                roleEnemy = tmpEnemy;
            }
        }
        return true;
    }


    protected override void SetRoleCamp(EFightCamp vRoleCamp)
    {
        base.SetRoleCamp(vRoleCamp);

        //设置方向//
        if (vRoleCamp == EFightCamp.efcEnemy)
        {
            Get_Direction = ERoleDirection.erdLeft;
            this.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            Get_Direction = ERoleDirection.erdRight;
            this.transform.localEulerAngles = Vector3.zero;
        }
    }

    protected override void HitOperate(HurtType vHurtType, int vCurHP)
    {
        if ((vHurtType != HurtType.Crite) && (vHurtType != HurtType.Normal))
            return;

        if (vHurtType == HurtType.Crite)
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Hit_Power, this.transform));
        }
        else
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(hitAudio, this.transform));
        }

        //创建播放受击特效//
        if (_MainSpine == null)
            return;
        _MainSpine.FadeColor(Color.red);

        //return;
        if (isShowHitEffect)
            return;
        isShowHitEffect = true;
        if (hitSpineEffect != null)
        {
            ShowHitEffect();
        }
        else
        {
            float tmpScaleValue = SkillTool.GetLocalScare(this);
            ResourceLoadManager.Instance.LoadEffect("effect_general_hit.assetbundle", (GameObject gb) =>
            {
                GameObject go = CommonFunction.InstantiateObject(gb, this.transform);
                if (go != null)
                {
                    go.transform.localScale = new Vector3(1 / this.transform.localScale.x * tmpScaleValue, 1 / this.transform.localScale.y * tmpScaleValue, 1);
                    go.transform.localPosition = SkillTool.GetBonePosition(this, 2);

                    hitSpineEffect = go.AddComponent<TdSpine.SpineBase>();
                    ShowHitEffect();
                }
            });
        }
    }
    private void ShowHitEffect()
    {
        if (hitSpineEffect == null)
            return;
        hitSpineEffect.InitSkeletonAnimation();
        if (_MainSpine != null)
            hitSpineEffect.setSortingOrder(_MainSpine.GetMaxSort() + 1);
        hitSpineEffect.pushAnimation("animation", false, 1);
        hitSpineEffect.EndEvent += (string s) =>
        { 
            isShowHitEffect = false;
        };
    }

    public override void SetPause()
    {
        base.SetPause();
        SetActionPause();
    }

    public override void SetResume()
    {
        base.SetResume();
        if (pauseCount > 0)
            return;
        SetActionResume();
    }
    

    /// <summary>
    /// 初始化角色行为
    /// </summary>
    public virtual void InitAction() { }

    /// <summary>
    /// 重置角色行为[子类实现]
    /// </summary>
    public virtual void ReSetRoleAction(string vActionName = "") { }

    /// <summary>
    /// 重置角色速度[基础移动速度-最终数值的具体计算方式在子类中实现]
    /// </summary>
    public virtual float GetRoleActualSpeed()
    {
        if (fightAttribute == null)
            return 0;
        return fightAttribute.MoveSpeed;
    }

    /// <summary>
    /// 重置显示速度
    /// </summary>
    /// <param name="vSpeed"></param>
    public virtual void ReSetShowSpeed(float vSpeed)
    {
        //设置动画播放速度//
        if (_MainSpine == null)
            return;
        _MainSpine.SetMultiple(vSpeed);
    }


    /// <summary>
    /// 显示召唤角色状态
    /// </summary>
    public void ShowSummon()
    {
        if (!isSummon)
            return;
        if (!IsLive())
            return;
        if (summonDuration <= 0)
        {
            //通知删除角色//
            isSummon = false;
            if (this.GetComponent<BoxCollider>() != null)
                this.GetComponent<BoxCollider>().enabled = false;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleDelete, sceneRoleID);
            return;
        }
        summonDuration--;
    }
}
