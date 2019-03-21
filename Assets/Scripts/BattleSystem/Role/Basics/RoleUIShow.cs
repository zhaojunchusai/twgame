using UnityEngine;
using System.Collections;

/// <summary>
/// 角色-界面演示
/// </summary>
public class RoleUIShow : RoleBase
{

    public override void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex,
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        base.UnInitialization(vIsDelete);
    }

    public override void InitAction()
    {
        AddSingleAction<ActionUIShow>();
        ExChangeAction<ActionUIShow>();
    }

}
