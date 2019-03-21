using UnityEngine;
using System.Collections;

/// <summary>
/// 角色状态-静止[界面 战斗开始过场]
/// </summary>
public class ActionStatic : ActionBase
{

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        if (UISystem.Instance.FightView != null)
        {
            if (UISystem.Instance.FightView.Get_FightStartIsOver)
            {
                pRoleBase.InitAction();
                return true;
            }
        }
        PushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        return true;
    }

    public override void SetDestroy()
    {
        base.SetDestroy();
    }
}
