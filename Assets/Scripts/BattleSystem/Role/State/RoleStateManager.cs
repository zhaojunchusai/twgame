using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色战斗状态
/// </summary>
public class RoleStateManager {

    private RoleBase _RoleBase;
    private List<StateBase> _ListState = new List<StateBase>();//状态列表[只执行最上面一个状态]//
    private StateBase _CurrentState;


    //获取状态列表中最新的状态//
    public StateBase Get_CurrentState
    {
        get {
            return _CurrentState;
        }
    }


    public RoleStateManager(RoleBase vRoleBase, ERoleState vInitRoleState)
    {
        _RoleBase = vRoleBase;
        //添加一个初始状态[初始状态永远存在不会被删除]//
        AddSingleState(vInitRoleState);
    }

    /// <summary>
    /// 检查状态是否正确
    /// </summary>
    /// <param name="vInitRoleState"></param>
    /// <returns></returns>
    private bool CheckStateIsRight(ERoleState vInitRoleState)
    {
        if (_CurrentState != null)
        {
            //判断是否当前状态//
            if (_CurrentState.Get_RoleState == vInitRoleState)
            {
                _CurrentState.StateReFresh();
                return false;
            }

            //判断当前状态是否是死亡状态//
            if (_CurrentState.Get_RoleState == ERoleState.ersDeath)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 添加一个最新的状态到列表的最前端
    /// </summary>
    public void AddSingleState(ERoleState vInitRoleState)
    {
        if (!CheckStateIsRight(vInitRoleState))
            return;

        switch (vInitRoleState)
        {
            case ERoleState.ersIdle:
                {
                    //AddSingleState(new RoleState_Idle(_RoleBase, this, vInitRoleState));
                }
                break;
            default:
                {
                    //AddSingleState(new RoleState_Death(_RoleBase, this, vInitRoleState));
                }
                break;
        }
    }
    public void AddSingleState(StateBase vStateBase)
    {
        if (vStateBase == null)
            return;
        _ListState.Insert(0, vStateBase);
        _CurrentState = _ListState[0];
    }

    /// <summary>
    /// 改变当前状态
    /// </summary>
    /// <param name="vInitRoleState"></param>
    public void ChangeSingleState(ERoleState vInitRoleState)
    {
        if (_ListState.Count <= 1)
            return;

        if (!CheckStateIsRight(vInitRoleState))
            return;

        //删除当前运行状态//
        DeleteSingleState();
        //添加最新状态//
        AddSingleState(vInitRoleState);
    }

    /// <summary>
    /// 插入一个状态到列表中的指定位置
    /// </summary>
    /// <param name="vRoleStateBase"></param>
    /// <param name="vIndex"></param>
    public void InsertSingleState(StateBase vStateBase, int vIndex)
    {
        if (vStateBase == null)
            return;
        if ((vIndex < 0) || (vIndex >= _ListState.Count))
            return;
        _ListState.Insert(vIndex, vStateBase);
        if (vIndex == 0)
            _CurrentState = _ListState[0];
    }

    /// <summary>
    /// 删除列表最前端的一个状态
    /// </summary>
    public void DeleteSingleState()
    {
        if (_ListState.Count <= 1)
            return;
        _CurrentState.StateClose();
        _ListState.RemoveAt(0);
        _CurrentState = _ListState[0];
    }

    /// <summary>
    /// 清空状态列表
    /// </summary>
    public void ClearWholeState()
    {
        _CurrentState.StateClose();
        _ListState.Clear();
    }

    /// <summary>
    /// 当前状态AI
    /// </summary>
    public void CurrentStateAI()
    {
        if (_CurrentState == null)
            return;
        _CurrentState.StateAI();
    }
}
