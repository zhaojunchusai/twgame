using UnityEngine;
using System.Collections;

/// <summary>
/// 胜利行为
/// </summary>
public class ActionVictory : ActionBase
{

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        if (!pRoleBase.Get_MainSpine.CheckIsHaveAnimation(GlobalConst.ANIMATION_NAME_VICTORY))
            RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        else
            RepleaceAnimation(GlobalConst.ANIMATION_NAME_VICTORY, true);
        return true;
    }

}
