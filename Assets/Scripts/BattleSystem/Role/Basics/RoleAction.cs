using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TdSpine;

/// <summary>
/// 行为管理
/// 
///           Soldier     Monster     Hero        AIHero        UIShow
/// 界面                                                        UIShow
/// 静止                              Idle
/// 移动      AIMove      -           HeroMove    
/// 战斗      AIFire      -           -           HeroAIFire[包含移动和战斗]
/// 死亡      Death       -           -               -
/// 胜利      Victory     -           -               -
/// 失败      Failure     -           -               -
/// </summary>
public class RoleAction : RoleAttribute
{
    protected RoleBase pRoleBase;

    /// <summary>
    /// 行为列表[行为类名, 行为]
    /// </summary>
    protected Dictionary<string, ActionBase> _DicAction = new Dictionary<string, ActionBase>();

    /// <summary>
    /// 当前状态名
    /// </summary>
    public string Get_CurrentActionName
    {
        get {
            return _CurrentActionName;
        }
    }
    public string _CurrentActionName = string.Empty;

    /// <summary>
    /// 当前状态
    /// </summary>
    public ActionBase Get_CurrentAction
    {
        get {
            if (string.IsNullOrEmpty(_CurrentActionName))
                return null;
            if (_DicAction.Count <= 0)
                return null;
            if (!_DicAction.ContainsKey(_CurrentActionName))
                return null;
            return _DicAction[_CurrentActionName];
        }
    }

    public void ReInitInfo_Action()
    {
        pRoleBase = null;
        if (_DicAction != null)
            _DicAction.Clear();
        _CurrentActionName = string.Empty;
    }

    /// <summary>
    /// 添加一个行为
    /// </summary>
    /// <typeparam name="T">行为</typeparam>
    protected void AddSingleAction<T>() where T : ActionBase, new()
    {
        //获取行为名//
        string tmpActionName = typeof(T).Name;
        //检测行为名是否正确//
        if (string.IsNullOrEmpty(tmpActionName))
            return;
        //检测是否已存在//
        if (_DicAction.ContainsKey(tmpActionName))
            return;
        //创建行为对象//
        T tmpT = new T();
        ////初始化行为//
        tmpT.SetInit(pRoleBase, tmpActionName);
        //添加到行为列表//
        _DicAction.Add(tmpActionName, tmpT);
    }

    /// <summary>
    /// 切换行为
    /// </summary>
    /// <param name="vActionName">行为类名</param>
    public void ExChangeAction(string vActionName)
    {
        if ((_CurrentActionName != null) && (_CurrentActionName.Equals("ActionDeath")))
            return;
        if (string.IsNullOrEmpty(vActionName))
            return;
        if (!_DicAction.ContainsKey(vActionName))
            return;
        if (_DicAction[vActionName] == null)
            return;
        if (!vActionName.Equals("ActionDeath"))
        {
            if (roleType != ERoleType.ertShooter)
            {
                if (!IsLive())
                    return;
            }
        }

        //检测是否当前行为//
        if (vActionName.Equals(_CurrentActionName))
        {
            //刷新行为演示//
            _DicAction[vActionName].SetRefresh();
            return;
        }

        //检测当前行为是否存在//
        if ((!string.IsNullOrEmpty(_CurrentActionName)) && (_DicAction.ContainsKey(_CurrentActionName)) && (_DicAction[_CurrentActionName] != null))
        {
            //停止当前行为//
            _DicAction[_CurrentActionName].SetStop();
        }

        //激活目标行为//
        _DicAction[vActionName].SetActive();
        //设置当前状态索引//
        _CurrentActionName = vActionName;
    }
    public void ExChangeAction<T>() where T : ActionBase
    {
        ExChangeAction(typeof(T).Name);
    }

    /// <summary>
    /// 移除一个行为
    /// </summary>
    /// <param name="vActionName">行为类名</param>
    public bool RemoveSingleAction(string vActionName)
    {
        if (string.IsNullOrEmpty(vActionName))
            return false;
        if (!_DicAction.ContainsKey(vActionName))
            return false;
        if (_DicAction[vActionName] == null)
            return false;
        _DicAction[vActionName].SetDestroy();
        return _DicAction.Remove(vActionName);
    }
    public bool RemoveSingleAction<T>() where T : ActionBase
    {
        return RemoveSingleAction(typeof(T).Name);
    }

    /// <summary>
    /// 清空所有行为
    /// </summary>
    public void ClearActions()
    {
        foreach (KeyValuePair<string, ActionBase> tmpInfo in _DicAction)
        {
            if (tmpInfo.Value == null)
                continue;
            tmpInfo.Value.SetDestroy();
        }
        _DicAction.Clear();
    }

    /// <summary>
    /// 查找一个行为
    /// </summary>
    /// <param name="vActionName"></param>
    /// <returns></returns>
    public ActionBase FindSingleAction(string vActionName)
    {
        if (string.IsNullOrEmpty(vActionName))
            return null;
        if (!_DicAction.ContainsKey(vActionName))
            return null;
        if (_DicAction[vActionName] == null)
            return null;
        return _DicAction[vActionName];
    }
    public ActionBase FindSingleAction<T>() where T : ActionBase
    {
        return FindSingleAction(typeof(T).Name);
    }

    /// <summary>
    /// 设置行为暂停
    /// </summary>
    public void SetActionPause()
    {
        if (Get_CurrentAction == null)
            return;
        Get_CurrentAction.SetPause();
    }

    /// <summary>
    /// 设置行为继续
    /// </summary>
    public void SetActionResume()
    {
        if (Get_CurrentAction == null)
            return;
        Get_CurrentAction.SetResume();
    }
}