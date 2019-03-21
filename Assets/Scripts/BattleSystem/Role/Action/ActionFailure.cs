using UnityEngine;
using System.Collections;

/// <summary>
/// 失败行为
/// </summary>
public class ActionFailure : ActionBase
{

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        return true;
    }
}
