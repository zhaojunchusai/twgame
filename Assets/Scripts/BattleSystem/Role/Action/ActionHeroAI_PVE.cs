using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 角色行为-英雄AI[PVE]
/// </summary>
public class ActionHeroAI_PVE : ActionBase
{
    /// <summary>
    /// 场景边框宽度
    /// </summary>
    private const float LIMIT_SCENE_WIDTH_LEFT = 50;
    private const float LIMIT_SCENE_WIDTH_RIGHT = 160;
    /// <summary>
    /// 敌方英雄左边界
    /// </summary>
    private const float LIMIT_ENEMY_LEFT = -1848;
    /// <summary>
    /// 敌方英雄右边界
    /// </summary>
    private const float LIMIT_ENEMY_FIRHT = 0;

    private const int HP_LIMIT_0 = 0;
    private const int HP_LIMIT_1 = 30;
    private const int HP_LIMIT_2 = 60;
    private const int HP_LIMIT_3 = 100;


    private bool isAutoMove = false;
    private bool isAutoSkill = false;
    private bool isAutoSummon = false;


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
    /// 移动上一次执行时间
    /// </summary>
    private float preMoveTime;
    /// <summary>
    /// 战斗场景摄像机比例
    /// </summary>
    private float cameraOrthographicSize;

    private List<ERoleSeat> listStanceOrder = new List<ERoleSeat>();
    private Dictionary<int, List<FightViewItem_Soldier>> dicSoldier = new Dictionary<int, List<FightViewItem_Soldier>>();
    private List<FightViewItem_Skill> listSkill = new List<FightViewItem_Skill>();
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
        ObtainSkillListInfo();
        pPetData = UISystem.Instance.FightView.pPetData;
    }

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        SetIdleInfo(true);
        SetRefresh();
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        preMoveTime = Time.time;
        Scheduler.Instance.AddUpdator(CheckAndSetStatus);
        ObtainSoldierListInfo();
        return true;
    }

    public override void SetRefresh()
    {
        isAutoMove = UISystem.Instance.FightView.Get_IsAutoMove;
        isAutoSkill = UISystem.Instance.FightView.Get_IsAutoSkill;
        isAutoSummon = UISystem.Instance.FightView.Get_IsAutoSummon;
        UpdateAutoCallOrder();
    }

    public void UpdateAutoCallOrder()
    {
        if (!UISystem.Instance.FightView.Get_IsAutoSummon)
        {
            foreach (KeyValuePair<int, List<FightViewItem_Soldier>> item in dicSoldier)
            {
                List<FightViewItem_Soldier> soldierComp = item.Value;
                if (soldierComp == null)
                    continue;
                for (int m = 0; m < soldierComp.Count; m++)
                {
                    FightViewItem_Soldier comp = soldierComp[m];
                    if (comp == null)
                        continue;
                    comp.UpdateAutoOrder(false, 0);
                }
            }
        }
        byte index = 0;
        if ((listSkill != null) && (listSkill.Count > 0))
        {
            for (int i = 0; i < listSkill.Count; i++)
            {
                FightViewItem_Skill comp = listSkill[i];
                if (comp == null || comp.Get_SkillData == null)
                {
                    comp.UpdateAutoOrder(false, 0);
                    continue;
                }
                index++;
                comp.UpdateAutoOrder(UISystem.Instance.FightView.Get_IsAutoSkill, index);
            }
        }

    }

    public override void SetDestroy()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
        {
            pRoleBase.Get_MainSpine.EndEvent -= Action_EndEvent;
            pRoleBase.Get_MainSpine.EventsEvent -= Action_EventsEvent;
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
        {
            return;
        }

        //播放待机动画//
        SetIdleInfo(false);
        if (_CurStatus == EActionStatus.easNone)
            return;
        _CurStatus = EActionStatus.easAI;
    }


    /// <summary>
    /// 获取召唤士兵信息
    /// </summary>
    private void ObtainSoldierListInfo()
    {
        if (listStanceOrder == null)
            listStanceOrder = new List<ERoleSeat>();
        listStanceOrder.Clear();
        listStanceOrder.Add(ERoleSeat.ersNear);
        listStanceOrder.Add(ERoleSeat.ersMiddle);
        listStanceOrder.Add(ERoleSeat.ersFar);
        dicSoldier.Clear();
        if (pRoleBase == null)
            return;
        if ((pRoleBase.Get_FightType == EFightType.eftPVP) || (pRoleBase.Get_FightType == EFightType.eftSlave) ||
            (pRoleBase.Get_FightType == EFightType.eftServerHegemony) || (pRoleBase.Get_FightType == EFightType.eftQualifying))
            return;
        SortSoldierList(FightViewPanel_PVE.Instance.soldierItemList);
    }
    private void SortSoldierList(List<FightViewItem_Soldier> vList)
    {
        if (vList == null)
            return;
        List<FightViewItem_Soldier> tmpList = new List<FightViewItem_Soldier>();
        foreach (FightViewItem_Soldier tmpSingleInfo in vList)
        {
            if (tmpSingleInfo == null)
                continue;
            if (tmpSingleInfo.Get_FightSoldierInfo == null)
                continue;
            if (tmpSingleInfo.Get_FightSoldierInfo.Att == null)
                continue;
            tmpList.Add(tmpSingleInfo);
        }
        if (tmpList == null)
            return;
        tmpList.Sort((left, right) =>
        {
            Soldier tmpLeft = left.Get_FightSoldierInfo;
            Soldier tmpRight = right.Get_FightSoldierInfo;
            if ((tmpLeft == null) || (tmpRight == null))
                return 0;
            if (tmpLeft.Att.Stance != tmpRight.Att.Stance)
            {
                if (tmpLeft.Att.Stance > tmpRight.Att.Stance)
                    return -1;
                return 1;
            }
            if (tmpLeft.Level != tmpRight.Level)
            {
                if (tmpLeft.Level > tmpRight.Level)
                    return -1;
                return 1;
            }
            if (tmpLeft.Att.Star != tmpRight.Att.Star)
            {
                if (tmpLeft.Att.Star > tmpRight.Att.Star)
                    return -1;
                return 1;
            }
            if (tmpLeft.Att.quality != tmpRight.Att.quality)
            {
                if (tmpLeft.Att.quality > tmpRight.Att.quality)
                    return -1;
                return 1;
            }
            if (tmpLeft.Att.id != tmpRight.Att.id)
            {
                if (tmpLeft.Att.id < tmpRight.Att.id)
                    return -1;
                return 1;
            }
            return 0;
        });
        foreach (FightViewItem_Soldier tmpSingleInfo in tmpList)
        {
            if (tmpSingleInfo == null)
                continue;
            if (tmpSingleInfo.Get_FightSoldierInfo == null)
                continue;
            if (tmpSingleInfo.Get_FightSoldierInfo.Att == null)
                continue;
            if (dicSoldier.ContainsKey(tmpSingleInfo.Get_FightSoldierInfo.Att.Stance))
            {
                dicSoldier[tmpSingleInfo.Get_FightSoldierInfo.Att.Stance].Add(tmpSingleInfo);
            }
            else
            {
                List<FightViewItem_Soldier> tmpListSoldier = new List<FightViewItem_Soldier>();
                tmpListSoldier.Add(tmpSingleInfo);
                dicSoldier.Add(tmpSingleInfo.Get_FightSoldierInfo.Att.Stance, tmpListSoldier);
            }
        }
    }

    /// <summary>
    /// 获取技能信息
    /// </summary>
    private void ObtainSkillListInfo()
    {
        listSkill.Clear();
        if (pRoleBase == null)
            return;
        if ((pRoleBase.Get_FightType == EFightType.eftPVP) || (pRoleBase.Get_FightType == EFightType.eftSlave) ||
            (pRoleBase.Get_FightType == EFightType.eftServerHegemony) || (pRoleBase.Get_FightType == EFightType.eftQualifying))
            return;
        int tmpCount = FightViewPanel_PVE.Instance.skillItemList.Count;
        for (int i = 0; i < tmpCount; i++)
        {
            if (FightViewPanel_PVE.Instance.skillItemList[i] == null)
                continue;
            listSkill.Add(FightViewPanel_PVE.Instance.skillItemList[i]);
        }
    }

    /// <summary>
    /// 设置宠物释放技能
    /// </summary>
    private bool SetPetSkillOperate()
    {
        if (!isAutoSkill)
            return false;
        if (UISystem.Instance.FightView.Get_CurValue_PetPower < UISystem.Instance.FightView.Get_MAXValue_PetPower)
            return false;
        if ((pPetData == null) || (pPetData.Skill == null) || (pPetData.Skill.Att == null))
            return false;
        if (FightViewPanel_PVE.Instance.Get_ItemStatus != EFightViewSkillStatus.eskNormal)
            return false;
        UISystem.Instance.FightView.ButtonEvent_PVEPetSkill(null);
        return true;
    }

    /// <summary>
    /// 设置英雄释放技能
    /// </summary>
    private int indexSkill = 0;
    private bool SetSkillOperate()
    {
        if (!isAutoSkill)
        {
            return false;
        }
        if ((listSkill == null) || (listSkill.Count <= 0))
            return false;
        if (indexSkill >= listSkill.Count)
            indexSkill = 0;
        for (int i = indexSkill; i < listSkill.Count; i++)
        {
            FightViewItem_Skill comp = listSkill[i];
            if (comp == null)
                continue;
            if (comp.Get_SkillData == null)
            {
                if (i == listSkill.Count - 1)
                {
                    indexSkill = 0;
                    break;
                }
                continue;
            }
            if (!comp.CheckIsCanUseSkill())
            {
                indexSkill = i;
                break;
            }
            indexSkill = i + 1;
            return true;
        }
        return false;
    }

    private int indexStanceSoldier = 0;
    private void SetSoldierOperate()
    {
        if (!isAutoSummon)
            return;
        if ((dicSoldier == null) || (dicSoldier.Count <= 0))
            return;
        if ((listStanceOrder == null) || (listStanceOrder.Count <= 0))
            return;
        if (indexStanceSoldier >= listStanceOrder.Count)
            indexStanceSoldier = 0;
        byte mCallOrder = 1;

        for (int i = 0; i < listStanceOrder.Count; i++)
        {
            ERoleSeat seat = listStanceOrder[i];
            if (!dicSoldier.ContainsKey((int)seat))
                continue;
            List<FightViewItem_Soldier> soldierComp = dicSoldier[(int)seat];
            if (soldierComp == null)
                continue;
            bool status = false;
            for (int m = 0; m < soldierComp.Count; m++)
            {
                FightViewItem_Soldier comp = soldierComp[m];
                if (comp == null)
                    continue;
                if (comp.CheckIsFull())
                {
                    comp.UpdateAutoOrder(false);
                }
                else
                {
                    if (!status)
                    {
                        status = true;
                        comp.UpdateAutoOrder(true, mCallOrder);
                    }
                    else
                    {
                        comp.UpdateAutoOrder(false, mCallOrder);
                    }
                }
            }
            if (status)
            {
                mCallOrder++;
            }
        }

        for (int i = indexStanceSoldier; i < listStanceOrder.Count; i++)
        {
            if (!dicSoldier.ContainsKey((int)listStanceOrder[i]))
                continue;
            List<FightViewItem_Soldier> soldierComp2 = dicSoldier[(int)listStanceOrder[i]];
            int tmpValue = SetSingleStanceSoldier(soldierComp2);
            if ((tmpValue == 0) || (tmpValue == 1))
            {
                indexStanceSoldier = i + 1;
                return;
            }
            else
            {
                return;
            }
        }
        indexStanceSoldier = 0;
    }
    /// <summary>
    /// 设定单一站位士兵召唤[0-无数据 1-成功召唤 2-等待]
    /// </summary>
    /// <returns></returns>
    private int SetSingleStanceSoldier(List<FightViewItem_Soldier> vSingleStanceSoldierList)
    {
        if ((vSingleStanceSoldierList == null) || (vSingleStanceSoldierList.Count <= 0))
            return 0;
        for (int i = 0; i < vSingleStanceSoldierList.Count; i++)
        {
            if (vSingleStanceSoldierList[i] == null)
                continue;
            if (vSingleStanceSoldierList[i].Get_FightSoldierInfo == null)
                continue;
            if (vSingleStanceSoldierList[i].CheckIsFull())
                continue;
            if (vSingleStanceSoldierList[i].CheckIsCanCallSoldier())
                return 1;
            return 2;
        }
        return 0;
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
        if (!isAutoMove)
            return;
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
                pRoleBase.Get_Direction = ERoleDirection.erdRight;
            else
                pRoleBase.Get_Direction = ERoleDirection.erdLeft;
        }
        else
        {
            if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
                pRoleBase.Get_Direction = ERoleDirection.erdLeft;
            else
                pRoleBase.Get_Direction = ERoleDirection.erdRight;
        }

        //设置速度//
        if (pRoleBase.Get_Direction == ERoleDirection.erdRight)
            actualMoveSpeed = pRoleBase.GetRoleActualSpeed();
        else
            actualMoveSpeed = -pRoleBase.GetRoleActualSpeed();

        if (!this.Get_CurAnimationName.Equals(GlobalConst.ANIMATION_NAME_MOVE))
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

        //检测宠物是否能够释放技能//
        if (SetPetSkillOperate())
        {
            pRoleBase._PreTime = Time.time;
            return;
        }

        //检测是否能够释放技能//
        if (SetSkillOperate())
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
        if (pRoleBase.Get_IsSkilling)
        {
            return;
        }

        SetSoldierOperate();

        RoleAttribute tmpEnemy = CheckIsHaveEnemy();
        if (tmpEnemy != null)
        {
            if ((isAutoMove) && (isAutoSkill))
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
            }
            _CurStatus = EActionStatus.easNormal;
            SetFireInfo();
            return;
        }
        else
        {
            if ((isAutoMove) && (isAutoSkill))
            {
                if (CheckIsCanForward() && !CheckIsFarSoldier())
                {
                    SetMoveInfo(true);
                    return;
                }
            }
            SetIdleInfo(false);
            preMoveTime = Time.time;
        }
    }

    /// <summary>
    /// 检测角色血量
    /// </summary>
    private int CheckRoleHPStatus()
    {
        int tmpResult = 0;
        if (pRoleBase == null)
            return tmpResult;
        tmpResult = pRoleBase.Get_FightAttribute.HP * 100 / pRoleBase.Get_MaxHPValue;
        if (tmpResult < 0)
            tmpResult = 0;
        else if (tmpResult > 100)
            tmpResult = 100;
        return tmpResult;
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
            if (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x <= LIMIT_SCENE_WIDTH_LEFT * cameraOrthographicSize)
                return false;
        }
        else
        {
            if (pRoleBase.transform.localPosition.x >= LIMIT_ENEMY_FIRHT)
                return false;
        }
        return true;
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
            ////检测场景物件是否已经移动到前边界-检测角色是否已经移动到前边界//
            if (pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x >= screenWidth - LIMIT_SCENE_WIDTH_RIGHT * cameraOrthographicSize)
                return false;
        }
        else
        {
            if (pRoleBase.transform.localPosition.x <= LIMIT_ENEMY_LEFT)
                return false;
        }
        return true;
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
        if (pRoleBase == null)
            return false;
        List<RoleAttribute> tmpRoles = CommonFunction.FindHitFightObjects(pRoleBase, pRoleBase.Get_RoleCamp, false);
        if ((tmpRoles == null) && (tmpRoles.Count <= 0))
            return false;
        float tmpScreenProportionX = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        bool tmpIsHaveFarSoldier = false;
        for (int i = 0; i < tmpRoles.Count; i++)
        {
            if (tmpRoles[i] == null)
                continue;
            if (tmpRoles[i].Get_FightAttribute == null)
                continue;
            SoldierAttributeInfo tmpInfo = ConfigManager.Instance.mSoldierData.FindById(tmpRoles[i].Get_FightAttribute.KeyData);
            if (tmpInfo == null)
                continue;
            if ((ERoleSeat)tmpInfo.Stance != ERoleSeat.ersFar)
            {
                return false;
            }
            else
            {
                if (!tmpIsHaveFarSoldier)
                {
                    if (Mathf.Abs(tmpRoles[i].transform.position.x - pRoleBase.transform.position.x) * tmpScreenProportionX <= 80)
                        tmpIsHaveFarSoldier = true;
                }
            }
        }

        return tmpIsHaveFarSoldier;
    }

    /// <summary>
    /// 检测是否有敌人在攻击范围内
    /// </summary>
    /// <returns>true-有敌人</returns>
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

        if (tmpDis < pRoleBase.Get_FightAttribute.AttDistance * 0.9F)
            return true;

        return false;
    }

}