using UnityEngine;
using System.Collections;

/// <summary>
/// 角色行为-界面演示
/// </summary>
public class ActionUIShow : ActionBase
{

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        PushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        return true;
    }

}
