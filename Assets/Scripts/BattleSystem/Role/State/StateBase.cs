using UnityEngine;
using System.Collections;

public class StateBase
{

    protected RoleStateManager _RoleStateManager;
    protected RoleBase _RoleBase;
    protected ERoleState _RoleState;
    public ERoleState Get_RoleState
    {
        get
        {
            return _RoleState;
        }
    }

    /// <summary>
    /// 状态动画回调
    /// </summary>
    protected virtual void StateAnimationComplete()
    { }

    /// <summary>
    /// 构造状态信息
    /// </summary>
    /// <param name="vRoleBase">关联角色</param>
    /// <param name="vRoleStateManager">关联状态管理器</param>
    /// <param name="vRoleState">当前状态</param>
    public StateBase(RoleBase vRoleBase, RoleStateManager vRoleStateManager, ERoleState vRoleState)
    {
        if ((vRoleBase == null) || (vRoleStateManager == null))
            return;
        _RoleBase = vRoleBase;
        _RoleStateManager = vRoleStateManager;
        _RoleState = vRoleState;
    }


    /// <summary>
    /// 状态自动执行方法
    /// </summary>
    public virtual void StateAI() { }

    /// <summary>
    /// 刷新状态
    /// </summary>
    public virtual void StateReFresh() { }

    /// <summary>
    /// 销毁状态
    /// </summary>
    public virtual void StateClose()
    {
        _RoleBase.Get_MainSpine.repleaceAnimation("idle", true);
    }
}
