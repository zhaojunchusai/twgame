namespace Assets.Script.Common.StateMachine
{
    public class State {
		//public  string StateName;
		//
        /// <summary>
        /// 状态被创建(加载资源)
        /// </summary>
        public virtual void Create()
        {
            
        }

        /// <summary>
        /// 状态被销毁(清理资源)
        /// </summary>
        public virtual void Destory()
        {
            
        }

        /// <summary>
        /// 进入状态
        /// </summary>
        public virtual void Enter()
        {

        }

        /// <summary>
        /// 状态更新
        /// </summary>
        public virtual void Update()
        {

        }

        /// <summary>
        /// 退出状态
        /// </summary>
        public virtual void Exit()
        {

        }

        public virtual string GetStateName()
        {
            return string.Empty;
        }
    }
}
