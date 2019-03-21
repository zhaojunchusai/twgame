using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TdSpine;
using Assets.Script.Common;

/// <summary>
/// 变身状态
/// </summary>
public enum EChangeStatus
{
    ecsNone = 0,//未变形//
    ecsAtSelf,//自己变形//
    ecsAtEnemy//被敌方变形//
}

/// <summary>
/// 属性管理
/// </summary>
public class RoleAttribute : MonoBehaviour
{

    /// <summary>
    /// 击退限定时间[秒]
    /// </summary>
    private const float LIMIT_REPEL_TIME = 1;
    /// <summary>
    /// 拉人技能最近距离
    /// </summary>
    private const float MIN_PULL_DISTANCE = 100;

    public string CheckRoleHP;
    /// <summary>
    /// 战斗属性
    /// </summary>
    protected ShowInfoBase fightAttribute;
    public ShowInfoBase basefightAttribute;
    /// <summary>
    /// 阵营
    /// </summary>
    protected EFightCamp roleCamp;
    /// <summary>
    /// 角色种类
    /// </summary>
    protected ERoleType roleType;
    /// <summary>
    /// 场景类型
    /// </summary>
    public EFightType RoleFightType;
    /// <summary>
    /// 身上的Buff
    /// </summary>
    public BuffDepot BuffManage;
    /// <summary>
    /// 身上的持续性特效
    /// </summary>
    public SpecilDepot SpecilDepotManage;
    /// <summary>
    /// 效果器免疫
    /// </summary>
    public EffectImmune EffectImmuneManage;
    /// <summary>
    /// 方位
    /// </summary>
    protected ERoleDirection direction;
    /// <summary>
    /// 动画组件
    /// </summary>
    protected MainSpine _MainSpine;
    /// <summary>
    /// 护盾特效
    /// </summary>
    public List<string> shiedEffectName;
    /// <summary>
    /// 宠物
    /// </summary>
    public RoleBase rolePet;
    public Skill tmpSkill;

    //-----------------------------普攻伤害相关系数-----------------------------//
    /// <summary>
    /// 普攻伤害系数[1]
    /// </summary>
    public float _Coeff_Attack_Hurt;
    /// <summary>
    /// 克制系数[1]
    /// </summary>
    protected float _Coeff_Attack_Restraint;
    /// <summary>
    /// 暴击系数[1.5]
    /// </summary>
    protected float _Coeff_Attack_Crit;
    /// <summary>
    /// 命中率系数1
    /// </summary>
    protected float _Coeff_Attack_AccuracyRate_1;
    /// <summary>
    /// 命中率系数2
    /// </summary>
    protected float _Coeff_Attack_AccuracyRate_2;
    /// <summary>
    /// 暴击率系数1
    /// </summary>
    protected float _Coeff_Attack_CritRate_1;
    /// <summary>
    /// 暴击率系数2
    /// </summary>
    protected float _Coeff_Attack_CritRate_2;
    /// <summary>
    /// 普攻减伤数值
    /// </summary>
    public int _Num_Attack_Weaken;
    /// <summary>
    /// 普攻减伤系数
    /// </summary>
    public float _Coeff_Attack_Weaken = 1;
    /// <summary>
    /// 普攻加成系数
    /// </summary>
    public float _Add_Attack_Hurt = 1;

    //-----------------------------技能伤害相关系数-----------------------------//
    /// <summary>
    /// 技能伤害系数[1]
    /// </summary>
    public float _Coeff_Skill_Hurt;
    /// <summary>
    /// 克制系数[1]
    /// </summary>
    public float _Coeff_Skill_Restraint;
    /// <summary>
    /// 被动技能加成[0.1f]
    /// </summary>
    public float _Coeff_Skill_Passive;
    /// <summary>
    /// 技能减伤数值
    /// </summary>
    public int _Num_Skill_Weaken;
    /// <summary>
    /// 技能减伤系数
    /// </summary>
    public float _Coeff_Skill_Weaken = 1;
    /// <summary>
    /// 技能伤害加成系数
    /// </summary>
    public float _Add_Skill_Hurt;
    /// <summary>
    /// 魔法消耗增减数值
    /// </summary>
    public int _Num_MP_Cost;
    /// <summary>
    /// 魔法消耗增减系数
    /// </summary>
    public float _Coeff_Mp_Cost = 1;
    /// <summary>
    /// 技能触发几率增减系数
    /// </summary>
    public float _Coeff_Trigger_Pro = 1;
    /// <summary>
    /// 光环范围增加数值
    /// </summary>
    public float _Num_Halo_Range;
    /// <summary>
    /// 光环范围增加系数
    /// </summary>
    public float _Coeff_Halo_Range = 1;
    //-----------------------------召唤系数-----------------------------//
    /// <summary>
    /// 召唤冷却增减数值
    /// </summary>
    public int Num_Summon_Interval;
    /// <summary>
    /// 召唤冷却增减系数
    /// </summary>
    public float _Coeff_Summon_Interval = 1;
    //-----------------------------最终数值-----------------------------//
    /// <summary>
    /// 攻击伤害
    /// </summary>
    [HideInInspector]
    public float _AttackHurt;
    /// <summary>
    /// 命中率
    /// </summary>
    [HideInInspector]
    public float _AccuracyRate;
    /// <summary>
    /// 暴击率
    /// </summary>
    [HideInInspector]
    public float _CritRate;
    /// <summary>
    /// 技能伤害
    /// </summary>
    [HideInInspector]
    public float _SkillHurt;
    /// <summary>
    /// 角色血条[界面不用显示]
    /// </summary>
    public RoleHPBase pRpleHP;
    /// <summary>
    /// 上一次攻击时间
    /// </summary>
    public float _PreTime = 0;
    /// <summary>
    /// 攻击间隔时间[秒]
    /// </summary>
    public float _AttRateTime;

    /// <summary>
    /// 被击退时间
    /// </summary>
    private float repelInitTime;
    /// <summary>
    /// 被击退距离
    /// </summary>
    private float repelDistance;
    /// <summary>
    /// 被击退时初始位置
    /// </summary>
    private Vector3 repelInitPos;
    /// <summary>
    /// 护盾血量
    /// </summary>
    private int shieldValue;
    /// <summary>
    /// 最大血量值
    /// </summary>
    private int maxHPValue;
    /// <summary>
    /// 最大魔法值
    /// </summary>
    private int maxMPValue;
    /// <summary>
    /// 最大能量值
    /// </summary>
    private int maxEnergyValue;
    /// <summary>
    /// 是否开启敌人检测
    /// </summary>
    protected bool isOpenCheckEnemy;
    /// <summary>
    /// 受击音效
    /// </summary>
    protected string hitAudio;
    /// <summary>
    /// 标准移动速度[像素]
    /// </summary>
    protected float normalSpeed;
    /// <summary>
    /// 标准攻击延迟[秒]
    /// </summary>
    protected float normalInterval;
    /// <summary>
    /// Spine速度类型
    /// </summary>
    private ESpineSpeedType spineSpeedType;
    /// <summary>
    /// 角色等级
    /// </summary>
    protected int roleLevel;
    /// <summary>
    /// 暂停操作次数
    /// </summary>
    protected int pauseCount = 0;
    /// <summary>
    /// 修改对话框角度
    /// </summary>
    public System.Action chatChangeEulerAngles;
    public event System.Action ChangeEulerAngles;
    /// <summary>
    /// 是否被策反[true-已被策反 false-未被策反]
    /// </summary>
    protected bool isSubverted = false;
    protected int rolePathIndex;
    protected EFightType fightType;
    /// <summary>
    /// 是否在场景外
    /// </summary>
    public bool IsOutSide = false;

    private float preInviTime;
    private float keepInviTime;
    private float changeInviTime;
    private float preRoleAlpha;
    private float sceneRoleParentPosX = 0;
    private bool triggerBlood = false;
    private bool isShowHurtHint = false;
    private float pullInitTime;
    private float pullDistance;
    private Vector3 pullInitPos;
    private bool isPulling = false;
    protected bool isSummon = false;//是否召唤角色//
    protected int summonDuration = 0;//召唤持续时间[秒]//

    private SkeletonDataAsset initSkeletonDataAsset = null;//角色原始动画资源//
    public EChangeStatus changeStatus = EChangeStatus.ecsNone;//变形状态//
    private float initScale;

    public delegate void ReSetRoleHPDelegate(int vValue, HurtType vType,RoleAttribute vTarget);   
    public event ReSetRoleHPDelegate ReSetRoleHPEvent;
    public int Get_RolePathIndex
    {
        get
        {
            return rolePathIndex;
        }
    }
    public EFightType Get_FightType
    {
        get
        {
            return fightType;
        }
    }
    /// <summary>
    /// 获取移动倍速
    /// </summary>
    public float Get_MoveTimeScale
    {
        get {
            if (fightAttribute == null)
                return 1;
            if ((normalSpeed == 0) || (fightAttribute.MoveSpeed == 0))
                return 1;
            return 1 + (fightAttribute.MoveSpeed - normalSpeed) / (fightAttribute.MoveSpeed + normalSpeed);
        }
    }
    /// <summary>
    /// 修改移动速度
    /// </summary>
    /// <param name="vChangeValue"></param>
    /// <param name="vTimeScale"></param>
    public void ReSetMoveValue(int vChangeValue = 0)
    {
        if (fightAttribute == null)
            return;
        fightAttribute.MoveSpeed += vChangeValue;
        if (spineSpeedType == ESpineSpeedType.esstMove)
            SetSpineTimeScale(spineSpeedType);
    }
    /// <summary>
    /// 获取技能倍速
    /// </summary>
    public float Get_SkillTimeScale
    {
        get {
            if (fightAttribute == null)
                return 1;
            if ((normalInterval == 0) || (fightAttribute.AttRate == 0))
                return 1;
            return normalInterval / fightAttribute.AttRate;
        }
    }
    /// <summary>
    /// 修改攻击间隔
    /// </summary>
    /// <param name="vChangeValue"></param>
    public void ReSetSkillValue(float vChangeValue = 0)
    {
        if (fightAttribute == null)
            return;
        fightAttribute.AttRate += vChangeValue;
        if (spineSpeedType == ESpineSpeedType.esstSkill)
            SetSpineTimeScale(spineSpeedType);
    }
    /// <summary>
    /// 设置Spine动画速度
    /// </summary>
    /// <param name="vType"></param>
    public void SetSpineTimeScale(ESpineSpeedType vType)
    {
        if (_MainSpine == null)
            return;
        spineSpeedType = vType;
        if (spineSpeedType == ESpineSpeedType.esstSkill)
            _MainSpine.setTimeScare(Get_SkillTimeScale);
        else
            _MainSpine.setTimeScare(Get_MoveTimeScale);
    }

    /// <summary>
    /// 动画
    /// </summary>
    public MainSpine Get_MainSpine
    {
        get
        {
            return _MainSpine;
        }
    }
    /// <summary>
    /// 方向
    /// </summary>
    public ERoleDirection Get_Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;

            if (_MainSpine == null)
                return;
            if (_MainSpine.skeletonAnimation == null)
                return;
            if (_MainSpine.skeletonAnimation.skeleton == null)
                return;

            if (direction == ERoleDirection.erdRight)
            {
                _MainSpine.TurnRight();
            }
            else
            {
                _MainSpine.TurnLeft();
            }
            if (pRpleHP != null)
                pRpleHP.ChangeEulerAngles(direction);

            if (chatChangeEulerAngles != null)
                chatChangeEulerAngles();
            if (ChangeEulerAngles != null)
                ChangeEulerAngles();
        }
    }
    /// <summary>
    /// 战斗属性
    /// </summary>
    public ShowInfoBase Get_FightAttribute
    {
        get
        {
            return fightAttribute;
        }
    }
    
    /// <summary>
    /// 普攻伤害系数[1]
    /// </summary>
    public float Get_Coeff_Attack_Hurt
    {
        get
        {
            return _Coeff_Attack_Hurt;
        }
    }
    /// <summary>
    /// 克制系数[1]
    /// </summary>
    public float Get_Coeff_Attack_Restraint
    {
        get
        {
            return _Coeff_Attack_Restraint;
        }
    }
    /// <summary>
    /// 暴击系数[1.5]
    /// </summary>
    public float Get_Coeff_Attack_Crit
    {
        get
        {
            return _Coeff_Attack_Crit;
        }
    }
    /// <summary>
    /// 命中率系数1
    /// </summary>
    public float Get_Coeff_Attack_AccuracyRate_1
    {
        get
        {
            return _Coeff_Attack_AccuracyRate_1;
        }
    }
    /// <summary>
    /// 命中率系数2
    /// </summary>
    public float Get_Coeff_Attack_AccuracyRate_2
    {
        get
        {
            return _Coeff_Attack_AccuracyRate_2;
        }
    }
    /// <summary>
    /// 暴击率系数1
    /// </summary>
    public float Get_Coeff_Attack_CritRate_1
    {
        get
        {
            return _Coeff_Attack_CritRate_1;
        }
    }
    /// <summary>
    /// 暴击率系数2
    /// </summary>
    public float Get_Coeff_Attack_CritRate_2
    {
        get
        {
            return _Coeff_Attack_CritRate_2;
        }
    }
    /// <summary>
    /// 技能伤害系数[1]
    /// </summary>
    public float Get_Coeff_Skill_Hurt
    {
        get
        {
            return _Coeff_Skill_Hurt;
        }
    }
    /// <summary>
    /// 克制系数[1]
    /// </summary>
    public float Get_Coeff_Skill_Restraint
    {
        get
        {
            return _Coeff_Skill_Restraint;
        }
    }
    /// <summary>
    /// 被动技能加成[0.1f]
    /// </summary>
    public float Get_Coeff_Skill_Passive
    {
        get
        {
            return _Coeff_Skill_Passive;
        }
    }
    
    /// <summary>
    /// 角色血条控制
    /// </summary>
    public RoleHPBase Get_RoleHPManager
    {
        get
        {
            return pRpleHP;
        }
    }
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int Get_MaxHPValue
    {
        get {
            return maxHPValue;
        }
        set {
            maxHPValue = value;
            if (this != null)
            {
                if (this.pRpleHP != null)
                    this.pRpleHP.SetCurAndMaxValue(fightAttribute.HP, maxHPValue);
            }
        }
    }
    public int Get_MaxMPValue
    {
        get {
            return maxMPValue;
        }
        set {
            maxMPValue = value;
        }
    }
    public int Get_MaxEnergyValue
    {
        get {
            return maxEnergyValue;
        }
        set {
            maxEnergyValue = value;
        }
    }
    /// <summary>
    /// 是否开启敌人检测
    /// </summary>
    public bool Get_isOpenCheckEnemy
    {
        get {
            return isOpenCheckEnemy;
        }
    }
    /// <summary>
    /// 获取阵营
    /// </summary>
    public EFightCamp Get_RoleCamp
    {
        get {
            return roleCamp;
        }
    }
    /// <summary>
    /// 获取角色类型
    /// </summary>
    public ERoleType Get_RoleType
    {
        get {
            return roleType;
        }
    }
    /// <summary>
    /// 获取角色等级
    /// </summary>
    public int Get_RoleLevel
    {
        get {
            return roleLevel;
        }
    }
    public bool Get_IsSubverted
    {
        get {
            return isSubverted;
        }
        set {
            isSubverted = value;
        }
    }


    /// <summary>
    /// 重新初始化
    /// </summary>
    public void ReInitInfo_Attribute()
    {
        BuffManage = null;
        if (SpecilDepotManage != null)
            SpecilDepotManage.Destory();
        SpecilDepotManage = null;
        EffectImmuneManage = null;
        _MainSpine = null;
        if (shiedEffectName != null)
            shiedEffectName.Clear();
        shiedEffectName = null;
        isSubverted = false;
        IsOutSide = false;
        triggerBlood = false;
        isShowHurtHint = false;
        isSummon = false;
    }

    /// <summary>
    /// 初始化战斗公式相关系数
    /// </summary>
    protected virtual void InitRoleCoefficient()
    {
        _Coeff_Attack_Weaken = 1;
        _Add_Attack_Hurt = 1;
        _Coeff_Skill_Weaken = 1;
        _Coeff_Mp_Cost = 1;
        _Coeff_Trigger_Pro = 1;
        _Coeff_Halo_Range = 1;
        _Coeff_Summon_Interval = 1;
        _PreTime = 0;
        pauseCount = 0;
        sceneRoleParentPosX = 0;
        summonDuration = 0;
    }

    /// <summary>
    /// 初始化角色属性
    /// </summary>
    public virtual void InitRoleAttribute(ShowInfoBase vFightAttribute)
    {
        if (fightAttribute == null)
        {
            fightAttribute = new ShowInfoBase();
        }
        if (basefightAttribute == null)
            basefightAttribute = new ShowInfoBase();
        fightAttribute.CopyTo(vFightAttribute);
        basefightAttribute.CopyTo(vFightAttribute);
        _AttRateTime = fightAttribute.AttRate;
        InitRoleCoefficient();
        this.BuffManage = new BuffDepot();
        this.SpecilDepotManage = new SpecilDepot();
        this.EffectImmuneManage = new EffectImmune();
        Get_MaxHPValue = fightAttribute.HP;
        isOpenCheckEnemy = true;

        hitAudio = GlobalConst.Sound.AUDIO_Hit_Normal;
        sceneRoleParentPosX = 0;
        if (this.transform.parent != null)
            sceneRoleParentPosX = this.transform.parent.localPosition.x;

        CheckRoleHP = string.Format("Name: [{0}], HP: [{1}]", this.name, vFightAttribute.HP);
    }

    /// <summary>
    /// 结束操作
    /// </summary>
    protected virtual void FinalOperate() { }

    /// <summary>
    /// 受击操作
    /// </summary>
    protected virtual void HitOperate(HurtType vHurtType, int vCurHP) { }

    /// <summary>
    /// 设置阵营
    /// </summary>
    /// <param name="vRoleCamp"></param>
    protected virtual void SetRoleCamp(EFightCamp vRoleCamp)
    {
        roleCamp = vRoleCamp;
    }
    
    /// <summary>
    /// 设置角色类型
    /// </summary>
    /// <param name="vRoleType"></param>
    protected virtual void SetRoleType(ERoleType vRoleType)
    {
        roleType = vRoleType;
    }

    /// <summary>
    /// 检测是否出界[true-出界 false-未出界]
    /// </summary>
    /// <returns></returns>
    public bool CheckIsOutSide(float vMove = 0)
    {
        float tmpLocalPosX = this.transform.localPosition.x + vMove;
        float tmpMinPosX = 50;
        float tmpMaxPosX = 0;
        if ((fightType == EFightType.eftNewGuide) || (fightType == EFightType.eftUnion) || (fightType == EFightType.eftCrossServerWar))
            tmpMaxPosX = 974;
        else
            tmpMaxPosX = 1998;

        if (roleType == ERoleType.ertHero)
        {
            if (roleCamp == EFightCamp.efcEnemy)
            {
                //if ((tmpMinPosX < tmpLocalPosX + sceneRoleParentPosX) && (tmpLocalPosX + sceneRoleParentPosX < tmpMaxPosX))
                    return false;
            }
            else
            {//自己英雄默认在场景中//
                return false;
            }
        }
        else if ((roleType == ERoleType.ertSoldier) || (roleType == ERoleType.ertMonster) || (roleType == ERoleType.ertEscort))
        {
            if ((tmpMinPosX < tmpLocalPosX + sceneRoleParentPosX) && (tmpLocalPosX + sceneRoleParentPosX < tmpMaxPosX))
                return false;
        }
        else if ((roleType == ERoleType.ertBarracks) || (roleType == ERoleType.ertShooter) || (roleType == ERoleType.ertTransfer))
        {//默认在场景中//
            return false;
        }
        return true;
    }

    /// <summary>
    /// 计算普攻伤害[后续根据需求考虑是否需要在子类中实现]
    /// 普攻伤害：攻击力 * 普攻伤害系数[1] * (1 + 被动技能加成) * 克制系数[1] * 暴击系数[1.5]
    /// 命中率：攻方命中值 * 命中系数1[0.8] / (攻方命中值 + 守方闪避值) + 命中系数2[0.45]
    /// 暴击率：攻方暴击值 * 暴击系数1[0.8] / (攻方暴击值 + 守方韧性值) + 暴击系数2[-0.35]
    /// </summary>
    /// <param name="vEnemy">敌方角色</param>
    public HurtType CalculationCommonHurt(RoleAttribute vEnemy)
    {
        _AttackHurt = 0;
        if (vEnemy == null)
            return HurtType.none;
        if (vEnemy.Get_FightAttribute == null)
            return HurtType.none; 
        if (fightAttribute == null)
            return HurtType.none;

        //命中率//
        _AccuracyRate = 0;
        if (fightAttribute.Accuracy + vEnemy.Get_FightAttribute.Dodge != 0)
            _AccuracyRate = fightAttribute.Accuracy * _Coeff_Attack_AccuracyRate_1 / (fightAttribute.Accuracy + vEnemy.Get_FightAttribute.Dodge) + _Coeff_Attack_AccuracyRate_2;
        if (!SkillTool.ProbMachine(_AccuracyRate))
            return HurtType.UnAccuracyRate;

        //普通攻击伤害//
        _AttackHurt = fightAttribute.Attack * _Coeff_Attack_Hurt * (1 + _Coeff_Skill_Passive) * _Coeff_Attack_Restraint;
        //减伤//
        _AttackHurt = _AttackHurt * vEnemy._Coeff_Attack_Weaken + vEnemy._Num_Attack_Weaken;
        if (_AttackHurt < 1)
            _AttackHurt = 1;
        //暴击率//
        _CritRate = 0;
        if (fightAttribute.Crit + vEnemy.Get_FightAttribute.Tenacity != 0)
            _CritRate = fightAttribute.Crit * _Coeff_Attack_CritRate_1 / (fightAttribute.Crit + vEnemy.Get_FightAttribute.Tenacity) + _Coeff_Attack_CritRate_2;
        if (_CritRate <= 0)
            return HurtType.Normal;
        //暴击攻击伤害//
        if (SkillTool.ProbMachine(_CritRate))
        {
            _AttackHurt = _AttackHurt * _Coeff_Attack_Crit;
            if (_AttackHurt < 1)
                _AttackHurt = 1;
            return HurtType.Crite;
        }
        return HurtType.Normal;
    }

    /// <summary>
    /// 添加最大血量
    /// </summary>
    /// <param name="vAddHP"></param>
    public void ReSetMaxHP(int vAddHP)
    {
        maxHPValue = maxHPValue + vAddHP;
        if (maxHPValue == 0)
            maxHPValue = 1;
        if (this != null)
        {
            if (this.pRpleHP != null)
                this.pRpleHP.SetMaxHp(maxHPValue);
        }
        if (fightAttribute != null)
        {
            if (fightAttribute.HP > maxHPValue)
                fightAttribute.HP = maxHPValue;
        }
    }

    /// <summary>
    /// 重置角色生命
    /// </summary>
    /// <param name="vValue">生命修改值</param>
    /// <param name="type">受击类型</param>
    /// <param name="vIsShowChange">是否显示修改值</param>
    /// <param name="vIsInit">是否初始设置</param>
    public virtual bool ReSetRoleHP(int vValue, HurtType vType, bool vIsShowChange = true, bool vIsInit = false)
    {
        if (!IsLive())
        {
            return false;
        }
        if (SceneManager.Instance.Get_FightIsFinished)
        {
            return false;
        }
        if (SceneManager.Instance.Get_CurScene._SceneStatus != ESceneStatus.essNormal)
        {
            return false;
        }
        if (ReSetRoleHPEvent != null)
            ReSetRoleHPEvent(vValue, vType,this);
        HurtType tmpType = vType;
        int tmpValue = vValue;
        if (vValue < 0)
        {
            if (shieldValue + vValue > 0)
            {
                shieldValue += vValue;
                tmpValue = 0;
                tmpType = HurtType.Shield;
            }
            else
            {
                if ((shieldValue != 0) && (this != null) && this.shiedEffectName != null)
                {
                    foreach (string name in this.shiedEffectName)
                    {
                        if (this.SpecilDepotManage == null)
                            break;

                        this.SpecilDepotManage.RemoveSpecilBuff(name);
                    }
                    if (this.shiedEffectName != null)
                        this.shiedEffectName.Clear();
                }
                tmpValue = shieldValue + vValue;
                shieldValue = 0;
                if (!vIsInit)
                {
                    HitOperate(tmpType, fightAttribute.HP + tmpValue);
                }
            }
        }

        if ((fightType == EFightType.eftServerHegemony) && (roleCamp == EFightCamp.efcEnemy))
        {
            if (this != null)
            {
                if (this.pRpleHP != null)
                {
                    this.pRpleHP.RefreshHp(tmpValue, tmpType, false);
                }
                if ((this.GetComponent<RoleAction>() != null) && (this.GetComponent<RoleAction>().Get_CurrentAction != null))
                {
                    this.GetComponent<RoleAction>().Get_CurrentAction.SetHittedStatus(tmpValue);
                }
                if ((!vIsInit) && (tmpValue < 0))
                {
                    //Debug.LogError(tmpValue);
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RecordServerHegemonyHurt, Mathf.Abs(tmpValue));
                }
            }
            return true;
        }

        if (fightAttribute.HP + tmpValue > maxHPValue)
        {
            tmpValue = maxHPValue - fightAttribute.HP;
        }
        if (fightType == EFightType.eftCaptureTerritory)
        {
            if (roleCamp == EFightCamp.efcEnemy)
            {
                if (tmpValue < 0)
                {
                    float tmpScore = 0;
                    if (Mathf.Abs(tmpValue) > Mathf.Abs(fightAttribute.HP))
                    {
                        //tmpScore = Mathf.Abs(((float)fightAttribute.HP * ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor) * (1 + CaptureTerritoryModule.Instance.GetCurrentTokenIncrease()));
                        tmpScore = Mathf.Abs((float)fightAttribute.HP);
                        //Debug.LogError("HP: " + tmpScore);
                    }
                    else
                    {
                        //tmpScore = Mathf.Abs((tmpValue * ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor) * (1 + CaptureTerritoryModule.Instance.GetCurrentTokenIncrease()));
                        tmpScore = Mathf.Abs(tmpValue);
                        //Debug.LogError("Va: " + tmpScore);
                    }
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, tmpScore);
                }
            }
        }
		else if (fightType == EFightType.eftCrossServerWar)
        {
            if (roleCamp == EFightCamp.efcEnemy)
            {
                if (tmpValue < 0)
                {
                    float tmpScore = 0;
                    if (Mathf.Abs(tmpValue) > Mathf.Abs(fightAttribute.HP))
                    {
                        tmpScore = Mathf.Abs((float)fightAttribute.HP);
                    }
                    else
                    {
                        tmpScore = Mathf.Abs(tmpValue);
                    }
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, tmpScore);
                }
            }
        }
        fightAttribute.HP += tmpValue;
        if (fightAttribute.HP < 0)
            fightAttribute.HP = 0;

        if (this != null)
        {
            if (this.pRpleHP != null)
                this.pRpleHP.RefreshHp(tmpValue, tmpType);
        }
        
        if (roleType == ERoleType.ertBarracks)
        {
            int tmpHPValue = fightAttribute.HP;
            if (roleCamp == EFightCamp.efcSelf)
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Self, tmpHPValue);
            else
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Enemy, tmpHPValue);
        }
        else if (roleType == ERoleType.ertEscort)
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetStar, fightAttribute.HP * 100 / maxHPValue);
        }

        if ((roleType == ERoleType.ertHero) && (roleCamp == EFightCamp.efcSelf))
        {
            if (!triggerBlood && fightAttribute.HP * 100 / maxHPValue <= 60)
            {
                GuideManager.Instance.CheckTrigger(GuideTrigger.HeroBlood60);
                triggerBlood = true;
            }

            if (fightAttribute.HP * 100 / maxHPValue > 50)
            {
                if (isShowHurtHint)
                {
                    isShowHurtHint = false;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightHurtHint);
                }
            }
            else
            {
                if (!isShowHurtHint)
                {
                    isShowHurtHint = true;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightHurtHint);
                }
            }
        }
        if (fightAttribute.HP <= 0)
        {
            if (this != null)
            {
                if (this.gameObject.GetComponent<BoxCollider>() != null)
                    this.gameObject.GetComponent<BoxCollider>().enabled = false;
            }
            FinalOperate();
            return false;
        }

        if (this != null)
        {
            if ((this.GetComponent<RoleAction>() != null) && (this.GetComponent<RoleAction>().Get_CurrentAction != null))
            {
                this.GetComponent<RoleAction>().Get_CurrentAction.SetHittedStatus(tmpValue);
            }
        }

        return true;
    }

    /// <summary>
    /// 设置角色能量
    /// </summary>
    /// <param name="vValue">能量修改值</param>
    /// <returns></returns>
    public bool ReSetRoleEnergy(int vValue)
    {
        if (!IsLive())
            return false;
        if (vValue >= 0)
        {
            fightAttribute.Energy += vValue;
        }
        else
        {
            if (Mathf.Abs(vValue) > fightAttribute.Energy)
            {
                fightAttribute.Energy = 0;
                FinalOperate();
                return true;
            }
            fightAttribute.Energy -= (Mathf.Abs(vValue));
        }
        return true;
    }

    /// <summary>
    /// 战斗属性增加
    /// </summary>
    /// <param name="attribute"></param>
    public void RoleShowInfoAdd(fogs.proto.msg.Attribute attribute)
    {
        if (this == null)
            return;

        this.fightAttribute.Attack += attribute.phy_atk;
        this.fightAttribute.Crit += attribute.crt_rate;
        this.fightAttribute.Dodge += attribute.ddg_rate;
        this.fightAttribute.Accuracy += attribute.acc_rate;
        this.fightAttribute.AttDistance += attribute.atk_space;
        this.fightAttribute.AttRate += CommonFunction.GetSecondTimeByMilliSecond(attribute.atk_interval);
        this.fightAttribute.Energy += attribute.energy_max;
        this.fightAttribute.EnergyRecovery += attribute.energy_revert;
        this.fightAttribute.HP += attribute.hp_max;
        this.fightAttribute.HPRecovery += attribute.hp_revert;
        this.fightAttribute.MoveSpeed += attribute.speed;
        this.fightAttribute.MP += attribute.mp_max;
        this.fightAttribute.MPRecovery += attribute.mp_revert;
        this.fightAttribute.Tenacity += attribute.tnc_rate;
    }

    /// <summary>
    /// 检测自己是否活着
    /// </summary>
    /// <returns></returns>
    public bool IsLive()
    {
        if (fightAttribute == null)
            return false;
        if (fightAttribute.HP <= 0)
            return false;
        return true;
    }


    //技能效果=====================================================================================================================//
    /// <summary>
    /// 护盾
    /// </summary>
    /// <param name="vEffect"></param>
    /// <param name="vShieldValue"></param>
    public void OpenShield(Effects vEffect, int vShieldValue)
    {
        shieldValue = vShieldValue;
    }
    public void CloseShield()
    {
        shieldValue = 0;
        if (this != null && this.shiedEffectName != null && this.SpecilDepotManage != null)
        {
            foreach(string name in this.shiedEffectName )
            {
                this.SpecilDepotManage.RemoveSpecilBuff(name);
            }
            this.shiedEffectName.Clear();
        }
    }

    /// <summary>
    /// 被击退
    /// </summary>
    /// <param name="vDistance"></param>
    public void Repel(float vDistance)
    {
        if (this == null)
            return;
        if (!IsLive())
            return;
        if (pauseCount == 0)
            SetPause();
        repelInitTime = Time.time;
        repelDistance = vDistance;
        repelInitPos = this.transform.localPosition;
        Scheduler.Instance.AddUpdator(ShowRepel);
    }
    private void ShowRepel()
    {
        if (this == null)
            return;
        //计算击退持续时间//
        float tmpTimeDistance = Time.time - repelInitTime;
        if (tmpTimeDistance >= LIMIT_REPEL_TIME)
        {
            if (UISystem.Instance.FightView.Get_FightIsPause)
                return;
            Scheduler.Instance.RemoveUpdator(ShowRepel);
            repelInitTime = 0;
            SetResume();
        }
        else
        {
            float tmpDistance = repelDistance * (2 * tmpTimeDistance * LIMIT_REPEL_TIME - tmpTimeDistance * tmpTimeDistance) / (LIMIT_REPEL_TIME * LIMIT_REPEL_TIME);
            if (roleCamp == EFightCamp.efcSelf)
                tmpDistance = -tmpDistance;
            if (!CheckIsOutSide(tmpDistance))
            {
                this.transform.localPosition = new Vector3(repelInitPos.x + tmpDistance, repelInitPos.y, repelInitPos.z);
                //if (roleCamp == EFightCamp.efcSelf)
                //    this.transform.localPosition = new Vector3(repelInitPos.x - tmpDistance, repelInitPos.y, repelInitPos.z);
                //else
                //    this.transform.localPosition = new Vector3(repelInitPos.x + tmpDistance, repelInitPos.y, repelInitPos.z);
            }
        }
    }

    /// <summary>
    /// 拉近
    /// </summary>
    /// <param name="vTarget">施法角色</param>
    /// <param name="vDistance">初始拉动距离</param>
    /// <returns>是否成功</returns>
    public bool Pull(RoleAttribute vTarget, float vDistance)
    {
        if (this == null)
            return false;
        if (!IsLive())
            return false;
        if (vTarget == null)
            return false;
        if (!vTarget.IsLive())
            return false;
        if (isPulling)
            return false;
        isPulling = true;
        if (pauseCount == 0)
            SetPause();

        pullInitTime = Time.time;

        float tmpRoleDistance = Mathf.Abs(this.transform.position.x - vTarget.transform.position.x) * SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        if (tmpRoleDistance - vDistance > MIN_PULL_DISTANCE)
            pullDistance = vDistance;
        else
            pullDistance = tmpRoleDistance - MIN_PULL_DISTANCE;
        if (pullDistance < 0)
        {
            isPulling = false;
            SetResume();
            return false;
        }

        pullInitPos = this.transform.localPosition;
        Scheduler.Instance.AddUpdator(ShowPull);
        return true;
    }
    private void ShowPull()
    {
        if (this == null)
            return;
        //计算击退持续时间//
        float tmpTimeDistance = Time.time - pullInitTime;
        if (tmpTimeDistance >= LIMIT_REPEL_TIME)
        {
            if (UISystem.Instance.FightView.Get_FightIsPause)
                return;
            isPulling = false;
            Scheduler.Instance.RemoveUpdator(ShowPull);
            pullInitTime = 0;
            SetResume();
        }
        else
        {
            float tmpDistance = pullDistance * (2 * tmpTimeDistance * LIMIT_REPEL_TIME - tmpTimeDistance * tmpTimeDistance) / (LIMIT_REPEL_TIME * LIMIT_REPEL_TIME);
            if (roleCamp == EFightCamp.efcSelf)
                this.transform.localPosition = new Vector3(pullInitPos.x + tmpDistance, pullInitPos.y, pullInitPos.z);
            else
                this.transform.localPosition = new Vector3(pullInitPos.x - tmpDistance, pullInitPos.y, pullInitPos.z);
        }
    }

    /// <summary>
    /// 策反
    /// </summary>
    public void Subverted(int vDuration)
    {
        isSubverted = true;
        if (roleCamp == EFightCamp.efcSelf)
            Get_Direction = ERoleDirection.erdLeft;
        else
            Get_Direction = ERoleDirection.erdRight;
    }
    public void CloseSubverted()
    {
        if (!isSubverted)
            return;
        if (!IsLive())
            return;
        isSubverted = false;
        if (roleCamp == EFightCamp.efcSelf)
            Get_Direction = ERoleDirection.erdRight;
        else
            Get_Direction = ERoleDirection.erdLeft;
    }

    /// <summary>
    /// 召唤
    /// </summary>
    /// <param name="vDuration"></param>
    public bool Get_IsSummon
    {
        get {
            return isSummon;
        }
    }
    public void Summon(int vDuration)
    {
        isSummon = true;
        summonDuration = vDuration;
        _MainSpine.SetColor(new Color(0, 0, 1));
    }

    /// <summary>
    /// 隐身
    /// </summary>
    /// <param name="vTime">持续时间</param>
    public void StartInvisible(float vTime)
    {
        if (this._MainSpine == null)
            return;

        isOpenCheckEnemy = false;
        changeInviTime = vTime / 8;
        preInviTime = Time.time;
        keepInviTime = vTime - changeInviTime * 2;
        preRoleAlpha = 1;
        this.GetComponent<BoxCollider>().enabled = false;
        Scheduler.Instance.AddUpdator(ShowInvisible);
    }
    private void ShowInvisible()
    {
        preRoleAlpha -= 0.5f * (Time.time - preInviTime) * SceneManager.Instance.Get_FightSpeed / changeInviTime;
        preInviTime = Time.time;
        if (preRoleAlpha <= 0.5f)
        {
            preRoleAlpha = 0.5f;
            Scheduler.Instance.RemoveUpdator(ShowInvisible);
        }
        if (_MainSpine == null)
            return;
        _MainSpine.ResetAlph(preRoleAlpha);
    }
    public void CloseInvisible()
    {
        if (this._MainSpine == null)
            return;

        _MainSpine.ResetAlph(1);
        isOpenCheckEnemy = true;
        this.GetComponent<BoxCollider>().enabled = true;
    }

#region 变形效果器
    public void StartChangeSkeletonDataAsset(string vResName = "role_10000000")
    {
        SkeletonDataAsset tmpSkeletonDataAsset = SceneManager.Instance.Get_SingleSkeletonDataAssetByName(vResName);
        if (tmpSkeletonDataAsset != null)
        {
            StartChangeSkeletonDataAsset(tmpSkeletonDataAsset);
        }
        else
        {
            ResourceLoadManager.Instance.LoadCharacter(vResName, ResourceLoadType.AssetBundle, (obj) =>
            {
                if (obj != null)
                {
                    GameObject tmpObj = CommonFunction.InstantiateObject(obj, SceneManager.Instance.Get_CurScene.transform);
                    tmpObj.SetActive(false);
                    if (tmpObj.GetComponent<SkeletonAnimation>() != null)
                    {
                        if (tmpObj.GetComponent<SkeletonAnimation>().skeletonDataAsset != null)
                        {
                            StartChangeSkeletonDataAsset(tmpObj.GetComponent<SkeletonAnimation>().skeletonDataAsset);
                            SceneManager.Instance.Add_SingleSkeletonDataAssetByName(vResName, tmpObj);
                        }
                    }
                }
            });
        }
    }
    public void StartChangeSkeletonDataAsset(SkeletonDataAsset vSkeletonDataAsset)
    {
        if (IsLive())
        {
            if (vSkeletonDataAsset != null)
            {
                if (changeStatus == EChangeStatus.ecsNone)
                {
                    if (_MainSpine != null)
                    {
                        if (_MainSpine.skeletonAnimation != null)
                        {
                            if (initSkeletonDataAsset == null)
                            {
                                initSkeletonDataAsset = _MainSpine.skeletonAnimation.skeletonDataAsset;
                            }
                            initScale = this.transform.localScale.x;
                            this.transform.localScale = new Vector3(initScale, initScale, initScale);
                            _MainSpine.skeletonAnimation.skeletonDataAsset = vSkeletonDataAsset;
                            _MainSpine.DeleteEvent();
                            _MainSpine.skeletonAnimation.Reset();
                            changeStatus = EChangeStatus.ecsAtEnemy;
                        }
                    }
                }
            }
            Scheduler.Instance.RemoveUpdator(ShowInvisible);
            CloseInvisible();
            ChangeAction();
        }
    }
    public void CloseChangeSkeletonDataAsset()
    {
        //if (_MainSpine != null)
        //{
        //    if (_MainSpine.skeletonAnimation != null)
        //    {
        //        if (initSkeletonDataAsset != null)
        //        {
        //            _MainSpine.skeletonAnimation.skeletonDataAsset = initSkeletonDataAsset;
        //            this.transform.localScale = new Vector3(initScale, initScale, initScale);
        //            _MainSpine.skeletonAnimation.Reset();
        //        }
        //    }
        //}
        changeStatus = EChangeStatus.ecsNone;
        ChangeAction();
    }
    public void CloseChangeSkeletonDataAssetOperate()
    {
        if (_MainSpine != null)
        {
            if (_MainSpine.skeletonAnimation != null)
            {
                if (initSkeletonDataAsset != null)
                {
                    _MainSpine.skeletonAnimation.skeletonDataAsset = initSkeletonDataAsset;
                    this.transform.localScale = new Vector3(initScale, initScale, initScale);
                    _MainSpine.InitSkeletonAnimation();
                    //_MainSpine.skeletonAnimation.Reset();
                }
            }
        }
    }
#endregion
#region 召唤宠物效果器
    public virtual void StartSummonPet(RoleAttribute target,int attack)
    {
    }
    public virtual void SummonPet(int attack,int keydata)
    {
        ShowInfoBase tmpInfo = new ShowInfoBase();
        tmpInfo.CopyTo(this.basefightAttribute);
        tmpInfo.KeyData = (uint)keydata;
        tmpInfo.Attack = attack;
        RoleManager.Instance.CreateSingleRole(tmpInfo, 0, 0, ERoleType.ertPet, 0, EHeroGender.ehgMale, EFightCamp.efcNone, EFightType.eftMain, this.transform.parent,
            (role) =>
            {
                if (role != null)
                {
                    role.StartSummonPet(this, attack);
                    this.rolePet = role;
                }
            });
    }
    public virtual void RecyclePet()
    {
        if (this.rolePet == null)
            return;
        this.rolePet.UnInitialization();
        this.rolePet = null;
    }
#endregion
#region 普攻改为技能
    public void StartAttackToSkill(Skill sk)
    {
        this.tmpSkill = null;
        this.tmpSkill = sk;
    }
    public void EndAttackToSkill()
    {
        this.tmpSkill = null;
    }
#endregion
    //技能效果=====================================================================================================================//
    
    /// <summary>
    /// 暂停
    /// </summary>
    public virtual void SetPause()
    {
        if (!IsLive())
            return;
        if (pauseCount <= 0)
        {
            pauseCount = 0;
            if (this._MainSpine != null)
            {
                _MainSpine.Pause();
            }
        }
        pauseCount += 1;
    }

    /// <summary>
    /// 暂停恢复
    /// </summary>
    public virtual void SetResume()
    {
        if (!IsLive())
            return;
        pauseCount -= 1;
        if (pauseCount > 0)
            return;
        if (this._MainSpine == null)
            return;
        _MainSpine.Resume();
    }

    public virtual void ChangeAction()
    {
        if (roleCamp == EFightCamp.efcSelf)
        {
            Get_Direction = ERoleDirection.erdRight;
        }
        else
        {
            Get_Direction = ERoleDirection.erdLeft;
        }
    }

    /// <summary>
    /// 免疫效果过滤-false表示不能执行效果
    /// </summary>
    /// <param name="effect"></param>
    /// <returns></returns>
    public bool SkillFilter_Barracks(Effects vEffect)
    {
        return SkillFilter(vEffect, SpecialField.SPECIALID_IMMUNEID_BARRACKS);
    }
    public bool SkillFilter_Transfer(Effects vEffect)
    {
        return SkillFilter(vEffect, SpecialField.SPECIALID_IMMUNEID_TRANSFER);
    }
    public bool SkillFilter_Escort(Effects vEffect)
    {
        return SkillFilter(vEffect, SpecialField.SPECIALID_IMMUNEID_ESCORT);
    }
    public bool SkillFilter_Hero(Effects vEffect)
    {
        return SkillFilter(vEffect, SpecialField.SPECIALID_IMMUNEID_HERO);
    }
    public bool SkillFilter_Boss(Effects vEffect)
    {
        return SkillFilter(vEffect, SpecialField.SPECIALID_IMMUNEID_BOSS);
    }
    public bool SkillFilter_SuperMonster(Effects vEffect)
    {
        return SkillFilter(vEffect, SpecialField.SPECIALID_IMMUNEID_SUPERMONSTER);
    }
    private bool SkillFilter(Effects vEffect, uint vSpecialID)
    {
        if (vEffect == null)
            return false;
        if (vEffect.info.effectType == 3 || vEffect.info.effectType == 4)
            return false;
        string tmpArrImmuneIDValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(vSpecialID);
        if (string.IsNullOrEmpty(tmpArrImmuneIDValue))
            return true;
        string[] tmpArrImmuneID = tmpArrImmuneIDValue.Split(',');
        for (int i = 0; i < tmpArrImmuneID.Length; i++)
        {
            if (string.IsNullOrEmpty(tmpArrImmuneID[i]))
                continue;
            int tmpSingleImmuneID = 0;
            if (!int.TryParse(tmpArrImmuneID[i], out tmpSingleImmuneID))
                tmpSingleImmuneID = 0;
            if (vEffect.info.effectId == tmpSingleImmuneID)
                return false;
        }
        return true;
    }
}