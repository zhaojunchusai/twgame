using UnityEngine;
using System.Collections;

/// <summary>
/// 战斗对话状态
/// </summary>
public class ActionDialogue : ActionBase
{

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
        {
            pRoleBase.Get_Direction = ERoleDirection.erdLeft;
        }
        else
        {
            pRoleBase.Get_Direction = ERoleDirection.erdRight;
        }
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        _PreStatus = EActionStatus.easNormal;
        _CurStatus = EActionStatus.easNormal;

        return true;
    }

}