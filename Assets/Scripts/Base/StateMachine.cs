using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script.Common.StateMachine
{
    public class StateMachine
    {
        private State _currentState;
        private State _previousState;
        private State _globalState;


		public Dictionary<string, State> _states = new Dictionary<string, State>();
		
		/// <summary>
        /// 初始化
        /// </summary>
        public void Initialize()
        {
            _currentState = null;
            _previousState = null;
            _globalState = null;
            _states = new Dictionary<string, State>();
        }

        /// <summary>
        /// 反初始化
        /// </summary>
        public void Uninitialize()
        {
            
        }

        /// <summary>
        /// 进入全局状态
        /// </summary>
        public void GlobalStateEnter()
        {
            _globalState.Enter();
        }

        /// <summary>
        /// 设置全局状态
        /// </summary>
        /// <param name="stateName"></param>
        public void SetGlobalStateState(string stateName)
        {
            var globalState = FindState(stateName);
            if (null == globalState)
            {
                return;
            }
            _globalState = globalState;
            _globalState.Enter();
        }

        /// <summary>
        /// 设置当前状态
        /// </summary>
        /// <param name="stateName">状态名</param>
        /// <returns></returns>
        public bool SetCurrentState(string stateName)
        {
            return ChangeState(stateName);
        }

        public void Update()
        {
            //全局状态的运行
            if (_globalState != null)
                _globalState.Update();

            //一般当前状态的运行
            if (_currentState != null)
                _currentState.Update();
        }

        public bool ChangeState(string newStateName)
        {
            return ChangeState(FindState(newStateName));
        }

        /// <summary>
        /// 切换状态
        /// </summary>
        /// <param name="newState">新状态</param>
        /// <returns>是否切换成功</returns>
        public bool ChangeState(State newState)
        {
            if (newState == null)
            {
               Debug.LogError("状态不存在");
                return false;
            }
            //退出之前状态
            if (_currentState != null)
            {
                _currentState.Exit();
                //保存之前状态
                _previousState = _currentState;
            }

            //设置当前状态
            _currentState = newState;
			//CommonFunction.DebugLog (_currentState);
            //进入当前状态
            _currentState.Enter();
            return true;  
        }
        
        /// <summary>
        /// 回退到前一个状态
        /// </summary>
        public void RevertToPreviousState()
        {
            ChangeState(_previousState);
        }

        /// <summary>
        /// 返回当前状态
        /// </summary>
        /// <returns>当前状态</returns>
        public State CurrentState()
        { 
            return _currentState;
        }

        /// <summary>
        /// 返回全局状态
        /// </summary>
        /// <returns></returns>
        public State GlobalState()
        {
            return _globalState;
        }

        /// <summary>
        /// 返回前一个状态
        /// </summary>
        /// <returns></returns>
        public State PreviousState()
        {
            return _previousState;
        }

        /// <summary>
        /// 加载一个状态
        /// </summary>
        /// <typeparam name="TState">状态</typeparam>
        /// <returns>状态实例</returns>
        public TState AddState<TState>() where TState : State, new()
        {
            var stateName = typeof (TState).Name;
            if (_states.ContainsKey(stateName))
            {
                return _states[stateName] as TState;
            }
            var state = new TState();
            state.Create();
            _states[stateName] = state;
            return state;
        }
		public State AddState(string statename,State state)
		{
			if(!_states.ContainsKey(statename))
			{
				_states.Add(statename,state);
				_states[statename].Create();
			}
			return state;
		}

        /// <summary>
        /// 移除一个状态机
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        public void RemoveState<TState>() where TState : State
        {
            State state;
            var stateName = typeof (TState).Name;
            if (_states.TryGetValue(stateName, out state))
            {
                state.Destory();
                _states.Remove(stateName);
            }
        }

        /// <summary>
        /// 找到一个State实例
        /// </summary>
        /// <param name="stateName">state名</param>
        /// <returns>实例</returns>
        public State FindState(string stateName)
        {
            if (_states.ContainsKey(stateName))
            {
                return _states[stateName];
            }
            return null;
        }
    }
} 
