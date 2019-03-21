using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 角色行为-英雄AI攻击
/// </summary>
public class ActionHeroAIFire : ActionBase
{
    /// <summary>
    /// 场景边框宽度
    /// </summary>
    private const float LIMIT_SCENE_WIDTH_LEFT = 50;
    private const float LIMIT_SCENE_WIDTH_RIGHT = 160;
    /// <summary>
    /// 敌方英雄左边界
    /// </summary>
    private const float LIMIT_ENEMY_LEFT = -1850;
    /// <summary>
    /// 敌方英雄右边界
    /// </summary>
    private const float LIMIT_ENEMY_FIRHT = 0;



    /// <summary>
    /// 当前场景
    /// </summary>
    private FightSceneBase pFightSceneBase;
    /// <summary>
    /// 场景宽度
    /// </summary>
    private float screenWidth;
    /// <summary>
    /// 场景中心点
    /// </summary>
    private float screenCenter;
    /// <summary>
    /// 场景左端限制坐标
    /// </summary>
    private float limitLeft;
    /// <summary>
    /// 场景有段限制坐标
    /// </summary>
    private float limitRight;
    /// <summary>
    /// 真实移动速度
    /// </summary>
    private float actualMoveSpeed;
    /// <summary>
    /// 英雄主动技能ID列表
    /// </summary>
    private List<Weapon> roleSkillIDList = new List<Weapon>();
    /// <summary>
    /// 移动上一次执行时间
    /// </summary>
    private float preMoveTime;
    /// <summary>
    /// 释放技能索引
    /// </summary>
    private int indexSkill = 0;
    /// <summary>
    /// 战斗场景摄像机比例
    /// </summary>
    private float cameraOrthographicSize;
    private PetData pPetData;



    public override void SetInit(RoleBase vRoleBase, string vActionName)
    {
        pFightSceneBase = SceneManager.Instance.Get_CurScene;
        if (pFightSceneBase != null)
        {
            screenWidth = pFightSceneBase.Get_CenterScreenWidth;
            limitLeft = pFightSceneBase.limitLeft;
            limitRight = pFightSceneBase.limitRight;
            cameraOrthographicSize = pFightSceneBase.SceneWidthSize;
        }
        else
        {
            screenWidth = 1024;
            limitLeft = -516;
            limitRight = -1532;
            cameraOrthographicSize = 1;
        }
        screenCenter = screenWidth / 2;
        base.SetInit(vRoleBase, vActionName);
        pPetData = UISystem.Instance.FightView.pPetData;
    }

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        SetIdleInfo(true);
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        if (roleSkillIDList != null)
            roleSkillIDList.Clear();
        GetHeroSkill();
        Scheduler.Instance.AddUpdator(CheckAndSetStatus);
        return true;
    }

    private void GetHeroSkill()
    {
        if (pRoleBase == null)
            return;
        if ((pRoleBase.RoleFightType != EFightType.eftExpedition) && (pRoleBase.RoleFightType != EFightType.eftPVP) && 
            (pRoleBase.RoleFightType != EFightType.eftSlave) && (pRoleBase.RoleFightType != EFightType.eftServerHegemony) && 
            (pRoleBase.RoleFightType != EFightType.eftQualifying))
            return;
        if (UISystem.Instance.FightView == null)
            return;

        FightPlayerInfo tmpPlayerInfo;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcNone)
        {
            return;
        }
        else if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            tmpPlayerInfo = UISystem.Instance.FightView.fightPlayer_Self;
        }
        else
        {
            tmpPlayerInfo = UISystem.Instance.FightView.fightPlayer_Enemy;
        }

        if (tmpPlayerInfo == null)
            return;
        if (tmpPlayerInfo.mEquip == null)
            return;
        if (tmpPlayerInfo.mEquip._EquiptList == null)
            return;
        for (int i = 0; i < tmpPlayerInfo.mEquip._EquiptList.Count; i++)
        {
            if (tmpPlayerInfo.mEquip._EquiptList[i] == null)
                continue;
            if (tmpPlayerInfo.mEquip._EquiptList[i].Att == null)
                continue;
            if (tmpPlayerInfo.mEquip._EquiptList[i].Att.type != 0)
                continue;
            roleSkillIDList.Add(tmpPlayerInfo.mEquip._EquiptList[i]);
        }
    }

    public override void SetDestroy()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
        {
            pRoleBase.Get_MainSpine.EndEvent -= Action_EndEvent;
            pRoleBase.Get_MainSpine.EventsEvent += Action_EventsEvent;
        }
        Scheduler.Instance.RemoveUpdator(CheckAndSetStatus);
        base.SetDestroy();
    }

    public override void SetResume()
    {
        base.SetResume();
        preMoveTime = Time.time;
        if (pRoleBase != null)
            pRoleBase._PreTime = Time.time;
    }

    protected override void ActionSingleTonSet()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
        {
            pRoleBase.Get_MainSpine.EndEvent += Action_EndEvent;
            pRoleBase.Get_MainSpine.EventsEvent += Action_EventsEvent;
        }
    }


    protected override void Action_EndEvent(string vAnimationName)
    {
        if (_CurStatus == EActionStatus.easNone)
            return;
        if (pRoleBase == null)
            return;
        //检测是否当前行为动画//
        bool tmpCheckName = false;
        if ((GlobalConst.ANIMATION_NAME_ABILITY_BASIC.Equals(vAnimationName)) || (GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE.Equals(vAnimationName)))
        {
            tmpCheckName = true;
        }
        else
        {
            for (int i = 0; i < GlobalConst.ANIMATION_NAME_SKILL_ARR.Length; i++)
            {
                if (GlobalConst.ANIMATION_NAME_SKILL_ARR[i].Equals(vAnimationName))
                {
                    tmpCheckName = true;
                    break;
                }
            }
        }

        if (!tmpCheckName)
            return;

        //播放待机动画//
        SetIdleInfo(false);

        ////检测敌人是否存在//
        //bool tmpIsChange = pRoleBase.CheckEnemyIsChange();
        //RoleAttribute tmpEnemy = pRoleBase.Get_Enemy;
        //if (tmpEnemy != null)
        //{
        //    HurtType tmpHurtType = pRoleBase.CalculationCommonHurt(tmpEnemy);
        //    tmpEnemy.ReSetRoleHP(-(int)pRoleBase._AttackHurt, tmpHurtType);
        //}

        if (_CurStatus == EActionStatus.easNone)
            return;
        _CurStatus = EActionStatus.easAI;
    }

    protected override void Action_EventsEvent(string vAnimationName, string vEventName)
    {
        if (_CurStatus == EActionStatus.easNone)
            return;
        if (pRoleBase == null)
            return;
        if (!GlobalConst.ANIMATION_NAME_ABILITY_BASIC.Equals(vAnimationName))
            return;
        if (!vEventName.Equals(GlobalConst.ANIMATION_EVENT_PROJECTILE) && !vEventName.Equals(GlobalConst.ANIMATION_EVENT_PORJECTILE))
            return;
        bool tmpIsChange = pRoleBase.CheckEnemyIsChange();
        RoleAttribute tmpEnemy = pRoleBase.Get_Enemy;
        if (tmpEnemy != null)
        {
            if (pRoleBase.tmpSkill == null)
            {
                HurtType tmpHurtType = pRoleBase.CalculationCommonHurt(tmpEnemy);
                tmpEnemy.ReSetRoleHP(-(int)pRoleBase._AttackHurt, tmpHurtType);
            }
            else
            {
                pRoleBase.tmpSkill.UseSkill(pRoleBase);
            }
        }
    }

    /// <summary>
    /// 设置宠物释放技能
    /// </summary>
    private bool SetPetSkillOperate()
    {
        if (pRoleBase == null)
            return false;
        string tmpAnimationName = string.Empty;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            if (UISystem.Instance.FightView.Get_CurValue_PetPower < UISystem.Instance.FightView.Get_MAXValue_PetPower)
                return false;
            if ((pPetData == null) || (pPetData.Skill == null) || (pPetData.Skill.Att == null) || (pPetData.PetInfo == null))
                return false;
            if (FightViewPanel_PVP.Instance.Get_ItemStatus(EFightCamp.efcSelf) != EFightViewSkillStatus.eskNormal)
                return false;
            UISystem.Instance.FightView.ButtonEvent_PVPPetSkill_Self(pRoleBase.gameObject);
            tmpAnimationName = pPetData.PetInfo.skill_anim;
        }
        else
        {
            if (UISystem.Instance.FightView.Get_CurValue_EnemyPetPower < UISystem.Instance.FightView.Get_MAXValue_EnemyPetPower)
                return false;
            if (UISystem.Instance.FightView.fightPlayer_Enemy.mPetInfo == null)
                return false;
            CombatPetInfo tmpPetInfo = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(UISystem.Instance.FightView.fightPlayer_Enemy.mPetInfo.id);
            if (tmpPetInfo == null)
                return false;
            SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(tmpPetInfo.skillID);
            if (tmpSkillInfo == null)
                return false;
            if (FightViewPanel_PVP.Instance.Get_ItemStatus(EFightCamp.efcEnemy) != EFightViewSkillStatus.eskNormal)
                return false;
            UISystem.Instance.FightView.ButtonEvent_PVPPetSkill_Enemy(pRoleBase.gameObject);
            tmpAnimationName = tmpPetInfo.skill_anim;
        }
        RepleaceAnimation(tmpAnimationName, false);
        return true;
    }

    /// <summary>
    /// 设置英雄技能
    /// </summary>
    private uint curSkillID = 0;
    private float preSkillTime = 0;
    Dictionary<uint, float> dicSkillTime = new Dictionary<uint, float>();
    private bool SetAIHeroSkill()
    {
        if (pRoleBase == null)
            return false;
        if (pRoleBase.Get_FightAttribute == null)
            return false;
        if (roleSkillIDList == null)
            return false;
        if (roleSkillIDList.Count <= 0)
            return false;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcNone)
            return false;
        if (UISystem.Instance.FightView == null)
            return false;

        //选择释放技能//
        if (indexSkill >= roleSkillIDList.Count)
            indexSkill = 0;
        for (int i = indexSkill; i < roleSkillIDList.Count; i++)
        {
            if ((roleSkillIDList[i] != null) && (roleSkillIDList[i]._skill != null) && (roleSkillIDList[i]._skill.Att != null))
            {
                if (dicSkillTime.ContainsKey(roleSkillIDList[i]._skill.Att.nId))
                {
                    if ((Time.time - dicSkillTime[roleSkillIDList[i]._skill.Att.nId]) * SceneManager.Instance.Get_FightSpeed > CommonFunction.GetSecondTimeByMilliSecond(roleSkillIDList[i]._skill.Att.waitTime))
                    {
                        indexSkill = i;
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    indexSkill = i;
                    break;
                }
            }
            if (i == roleSkillIDList.Count - 1)
                return false;
        }

        int tmpCurMagic = 0;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
            tmpCurMagic = UISystem.Instance.FightView.Get_CurValue_Magic;
        else
            tmpCurMagic = UISystem.Instance.FightView.Get_CurValue_EnemyMagic;

        float tmpMagic = roleSkillIDList[indexSkill]._skill.Att.expendMagic * pRoleBase._Coeff_Mp_Cost + pRoleBase._Num_MP_Cost;
        int tmpExpendMagic = (int)tmpMagic;
        if (tmpCurMagic < tmpExpendMagic)
            return false;
        preSkillTime = Time.time;
        //扣除魔法//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerMagic, new FightDecMP(pRoleBase.Get_RoleCamp, -tmpExpendMagic));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ChangeCurPetPower, pRoleBase.Get_RoleCamp);

        //释放技能//
        roleSkillIDList[indexSkill]._skill.UseSkill(pRoleBase);
        RepleaceAnimation(roleSkillIDList[indexSkill].Att.skillAnimation, false);
        if (pRoleBase.Get_MainSpine != null)
            pRoleBase.Get_MainSpine.RepleaceEquipment(roleSkillIDList[indexSkill].Att.id);
        curSkillID = roleSkillIDList[indexSkill]._skill.Att.nId;
        indexSkill += 1;
        if (!dicSkillTime.ContainsKey(curSkillID))
        {
            dicSkillTime.Add(curSkillID, preSkillTime);
        }
        else
        {
            dicSkillTime[curSkillID] = preSkillTime;
        }
        return true;
    }

    /// <summary>
    /// 设置停止状态
    /// </summary>
    private void SetIdleInfo(bool vIsReplace)
    {
        if (pRoleBase == null)
            return;
        if (!pRoleBase.IsLive())
            return;
        if (vIsReplace)
        {
            RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
            if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
                pRoleBase.Get_Direction = ERoleDirection.erdRight;
            else
                pRoleBase.Get_Direction = ERoleDirection.erdLeft;
        }
        else
            PushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, GlobalConst.ANIMATION_TRACK_BASE);
    }

    /// <summary>
    /// 设置角色移动信息[true-前进 false-后退]
    /// </summary>
    /// <param name="vIsForward"></param>
    private void SetMoveInfo(bool vIsForward)
    {
        if (pRoleBase == null)
            return;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstMove);
        if (pFightSceneBase == null)
            return;
        if (pFightSceneBase.transOther == null)
            return;
        if (pFightSceneBase.sceneCamera == null)
            return;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcNone)
            return;

        //设置方位//
        if (vIsForward)
        {
            if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
            {
                pRoleBase.Get_Direction = ERoleDirection.erdRight;
            }
            else
            {
                pRoleBase.Get_Direction = ERoleDirection.erdLeft;
            }
        }
        else
        {
            if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
            {
                pRoleBase.Get_Direction = ERoleDirection.erdLeft;
            }
            else
            {
                pRoleBase.Get_Direction = ERoleDirection.erdRight;
            }
        }

        //设置速度//
        if (pRoleBase.Get_Direction == ERoleDirection.erdRight)
            actualMoveSpeed = pRoleBase.GetRoleActualSpeed();
        else
            actualMoveSpeed = -pRoleBase.GetRoleActualSpeed();

        if (!Get_CurAnimationName.Equals(GlobalConst.ANIMATION_NAME_MOVE))
        {
            preMoveTime = Time.time;
        }
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);

        float tmpCurTime = Time.time;
        //float tmpSpeed = actualMoveSpeed * (tmpCurTime - preMoveTime) * SceneManager.Instance.Get_FightSpeed;
        float tmpSpeed = actualMoveSpeed * CommonFunction.GetReasonableIntervalTime(tmpCurTime, preMoveTime) * SceneManager.Instance.Get_FightSpeed;

        preMoveTime = tmpCurTime;

        //获取角色当前屏幕坐标//
        float tmpCurScreenPosX = pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x;

        //场景物件坐标移动//
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            if (pFightSceneBase.transOther.transform.localPosition.x > limitLeft)
            {
                pFightSceneBase.transOther.transform.localPosition = new Vector3(limitLeft, 0, 0);
            }
            else if (pFightSceneBase.transOther.transform.localPosition.x < limitRight)
            {
                pFightSceneBase.transOther.transform.localPosition = new Vector3(limitRight, 0, 0);
            }
            else
            {
                if (Mathf.Abs(screenCenter - tmpCurScreenPosX) <= GlobalConst.SCENE_CENTER_LIMIT_WIDTH)
                {
                    float tmpValue = pFightSceneBase.transOther.transform.localPosition.x - tmpSpeed;
                    if (tmpValue >= limitLeft)
                    {
                        pFightSceneBase.transOther.transform.localPosition = new Vector3(limitLeft, 0, 0);
                    }
                    else if (tmpValue <= limitRight)
                    {
                        pFightSceneBase.transOther.transform.localPosition = new Vector3(limitRight, 0, 0);
                    }
                    else
                    {
                        pFightSceneBase.transOther.transform.localPosition -= new Vector3(tmpSpeed, 0, 0);
                        return;
                    }
                }
            }
        }

        //角色移动//
        pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
    }

    /// <summary>
    /// 设置角色攻击信息
    /// </summary>
    private void SetFireInfo()
    {
        if (_CurStatus == EActionStatus.easNone)
            return;
        if ((pRoleBase == null) || (_CurStatus != EActionStatus.easNormal))
        {
            _CurStatus = EActionStatus.easAI;
            preMoveTime = Time.time;
            return;
        }
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);

        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
            pRoleBase.Get_Direction = ERoleDirection.erdRight;
        else
            pRoleBase.Get_Direction = ERoleDirection.erdLeft;

        //检测战斗条件是否达成//
        if (!CheckIsCanFire())
        {
            _CurStatus = EActionStatus.easAI;
            preMoveTime = Time.time;
            return;
        }

        //检测宠物是否释放技能//
        if (SetPetSkillOperate())
        {
            pRoleBase._PreTime = Time.time;
            return;
        }

        //检测是否释放技能//
        if (SetAIHeroSkill())
        {
            pRoleBase._PreTime = Time.time;
            return;
        }

        //检测时间是否达成//
        float tmpCDTime = CheckFightIsCDTime();
        if (tmpCDTime > 0)
        {
            Scheduler.Instance.AddTimer(tmpCDTime, false, SetFireInfo);
            return;
        }

        //重置攻击时间//
        pRoleBase._PreTime = Time.time;
        //重新开始攻击//
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_ABILITY_BASIC, false);
    }


    /// <summary>
    /// 检测战斗时间
    /// </summary>
    /// <returns>大于0表示在CD中</returns>
    private float CheckFightIsCDTime()
    {
        if (pRoleBase == null)
            return 1;
        float tmpRateTime = pRoleBase._AttRateTime;
        return ((pRoleBase._PreTime + tmpRateTime / SceneManager.Instance.Get_FightSpeed) - Time.time);
    }

    /// <summary>
    /// 检测战斗条件
    /// </summary>
    private bool CheckIsCanFire()
    {
        if (pRoleBase == null)
            return false;

        //检测自己是否死亡//
        if (!pRoleBase.IsLive())
        {
            SetStop();
            return false;
        }
        if (pRoleBase.CheckIsOutSide())
            return false;

        //检测敌方是否死亡//
        pRoleBase.CheckEnemyIsChange();
        if (pRoleBase.Get_Enemy == null)
            return false;

        return true;
    }



    /// <summary>
    /// 检测并设置状态
    /// </summary>
    private void CheckAndSetStatus()
    {
        if (_CurStatus != EActionStatus.easAI)
            return;
        if (pRoleBase == null)
            return;
        if (!pRoleBase.IsLive())
            return;

        RoleAttribute tmpEnemy = CheckIsHaveEnemy();
        if (tmpEnemy != null)
        {
            if (tmpEnemy.Get_RoleType != ERoleType.ertBarracks)
            {
                if (CheckIsCanBack())
                {
                    if (CheckIsHaveSoldier())
                    {
                        //判断是否在后退//
                        if (CheckIsBackOperate())
                        {
                            if (CheckIsMaxDis())
                            {
                                SetMoveInfo(false);
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (CheckIsMaxDis())
                        {
                            SetMoveInfo(false);
                            return;
                        }
                    }
                }
            }
            _CurStatus = EActionStatus.easNormal;
            SetFireInfo();
            return;
        }
        else
        {
            if (CheckIsCanForward() && !CheckIsFarSoldier())
            {
                SetMoveInfo(true);
                return;
            }
            SetIdleInfo(false);
            preMoveTime = Time.time;
        }
    }

    /// <summary>
    /// 检测是否正在后退操作
    /// </summary>
    /// <returns>true-正在后退 false-未后退</returns>
    private bool CheckIsBackOperate()
    {
        if (pRoleBase != null)
        {
            if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
            {
                if (pRoleBase.Get_Direction == ERoleDirection.erdLeft)
                {
                    return true;
                }
            }
            else if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
            {
                if (pRoleBase.Get_Direction == ERoleDirection.erdRight)
                {
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 检测是否能够后退
    /// </summary>
    /// <returns>true-可以后退</returns>
    private bool CheckIsCanBack()
    {
        if (pRoleBase == null)
            return false;
        if (pFightSceneBase == null)
            return false;
        if (pFightSceneBase.sceneCamera == null)
            return false;

        //检测是否已经到达边界//
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            //检测场景物件是否已经移动到后边界-检测角色是否已经移动到后边界//
            //if ((pFightSceneBase.transOther.transform.localPosition.x >= limitLeft) && (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x <= LIMIT_SCENE_WIDTH_LEFT))
            //    return false;

            if (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x <= LIMIT_SCENE_WIDTH_LEFT * cameraOrthographicSize)
                return false;
        }
        else
        {
            if (pRoleBase.transform.localPosition.x >= LIMIT_ENEMY_FIRHT)
                return false;
        }

        return true;



        ////检测是否已经到达边界//
        //if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        //{
        //    //检测场景物件是否已经移动到后边界//
        //    if (pFightSceneBase.transOther.transform.localPosition.x < limitLeft)
        //        return true;
        //    //检测角色是否已经移动到后边界//
        //    if (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x > LIMIT_SCENE_WIDTH_LEFT)
        //        return true;
        //}
        //else
        //{
        //    if (pRoleBase.transform.localPosition.x < LIMIT_ENEMY_FIRHT)
        //        return true;
        //}

        //return false;
    }

    /// <summary>
    /// 检测是否能够前进
    /// </summary>
    /// <returns>true-可以前进</returns>
    private bool CheckIsCanForward()
    {
        if (pRoleBase == null)
            return false;
        if (pFightSceneBase == null)
            return false;
        if (pFightSceneBase.sceneCamera == null)
            return false;

        //检测是否已经到达边界//
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            //检测场景物件是否已经移动到前边界-检测角色是否已经移动到前边界//
            //if ((pFightSceneBase.transOther.transform.localPosition.x <= limitRight) && (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x >= screenWidth - LIMIT_SCENE_WIDTH_RIGHT))
            //    return false;

            //if (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x >= (screenWidth - LIMIT_SCENE_WIDTH_RIGHT) * cameraOrthographicSize)
            //    return false;

            if (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x >= screenWidth - LIMIT_SCENE_WIDTH_RIGHT * cameraOrthographicSize)
                return false;
        }
        else
        {
            if (pRoleBase.transform.localPosition.x <= LIMIT_ENEMY_LEFT)
                return false;
        }

        return true;



        ////检测是否已经到达边界//
        //if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        //{
        //    //检测场景物件是否已经移动到前边界//
        //    if (pFightSceneBase.transOther.transform.localPosition.x > limitRight)
        //        return true;
        //    //检测角色是否已经移动到前边界//
        //    if (tmpCurScreenPosX < screenWidth - LIMIT_SCENE_WIDTH_RIGHT)
        //        return true;
        //}
        //else
        //{
        //    if (pRoleBase.transform.localPosition.x > LIMIT_ENEMY_LEFT)
        //        return true;
        //}

        //return false;
    }

    /// <summary>
    /// 检测前方是否有己方士兵
    /// </summary>
    /// <returns>true-有己方士兵</returns>
    private bool CheckIsHaveSoldier()
    {
        if (pRoleBase == null)
            return false;
        RoleAttribute tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, pRoleBase.Get_RoleCamp, false);
        if (tmpRole == null)
            return false;
        return true;
    }

    /// <summary>
    /// 检测是否远程兵种
    /// </summary>
    /// <returns></returns>
    private bool CheckIsFarSoldier() 
    {
        //if (pRoleBase == null)
        //    return false;
        //bool result = true;
        ////List<RoleAttribute> tmpRoles = CommonFunction.FindHitFightObjects(pRoleBase, pRoleBase.Get_RoleCamp, true, 80);
        //List<RoleAttribute> tmpRoles = CommonFunction.FindHitFightObjects(pRoleBase, pRoleBase.Get_RoleCamp);
        //if (tmpRoles.Count <= 0)
        //    return false;

        //for (int i = 0; i < tmpRoles.Count;i++ )
        //{
        //    SoldierAttributeInfo tmpInfo = ConfigManager.Instance.mSoldierData.FindById(tmpRoles[i].Get_FightAttribute.KeyData);
        //    if ((ERoleSeat)tmpInfo.Stance != ERoleSeat.ersFar) 
        //    {
        //        result = false;
        //        break;
        //    }
        
        //}
        //return result;

        if (pRoleBase == null)
            return false;
        List<RoleAttribute> tmpRoles = CommonFunction.FindHitFightObjects(pRoleBase, pRoleBase.Get_RoleCamp, false);
        if ((tmpRoles == null) && (tmpRoles.Count <= 0))
            return false;
        bool tmpResult = false;
        float tmpScreenProportionX = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        for (int i = 0; i < tmpRoles.Count; i++)
        {
            if (tmpRoles[i] == null)
                continue;
            if (tmpRoles[i].Get_FightAttribute == null)
                continue;
            SoldierAttributeInfo tmpInfo = ConfigManager.Instance.mSoldierData.FindById(tmpRoles[i].Get_FightAttribute.KeyData);
            if ((ERoleSeat)tmpInfo.Stance != ERoleSeat.ersFar)
            {
                return false;
            }
            else
            {
                if (tmpResult)
                    continue;
                if (Mathf.Abs(pRoleBase.transform.position.x - tmpRoles[i].transform.position.x) * tmpScreenProportionX <= 80)
                    tmpResult = true;
            }
        }
        return tmpResult;
    }

    /// <summary>
    /// 检测是否有敌人在攻击范围内
    /// </summary>
    /// <returns>true-有敌人</returns>
    //private bool CheckIsHaveEnemy()
    //{
    //    if (pRoleBase == null)
    //        return false;
    //    RoleAttribute tmpRole = null;
    //    if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
    //        tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, EFightCamp.efcSelf);
    //    else
    //        tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, EFightCamp.efcEnemy);
    //    if (tmpRole == null)
    //        return false;
    //    return true;
    //}
    private RoleAttribute CheckIsHaveEnemy()
    {
        if (pRoleBase == null)
            return null;
        RoleAttribute tmpRole = null;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
            tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, EFightCamp.efcSelf);
        else
            tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, EFightCamp.efcEnemy);
        return tmpRole;
    }

    /// <summary>
    /// 检测是否最大距离
    /// </summary>
    /// <returns></returns>
    public bool CheckIsMaxDis() 
    {
        if (pRoleBase == null)
            return false;
        RoleAttribute tmpRole = null;
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
            tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, EFightCamp.efcSelf);
        else
            tmpRole = CommonFunction.FindHitSingleFightObject(pRoleBase, EFightCamp.efcEnemy);
        if (tmpRole == null)
            return false;

        float tmpDis = Mathf.Abs(pRoleBase.transform.position.x - tmpRole.transform.position.x) * SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
 
        if (tmpDis < pRoleBase.Get_FightAttribute.AttDistance *0.9F)
            return true;

        return false;
    }

}
