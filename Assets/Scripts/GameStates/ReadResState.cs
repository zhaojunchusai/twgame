using UnityEngine;
using System.Collections;
namespace Assets.Script.Common.StateMachine
{
    public class ReadResState : State
    {
        public static string StateName { get { return typeof(ReadResState).Name; } }

        public override void Create()
        {
            base.Create();
        }

        public override void Enter()
        {
            //TODO：得去资源到游戏中 比如图集 读取完成 跳到登陆界面
            base.Enter();
            if (GlobalConst.OPENDLC)
                ResourceLoadManager.Instance.StartCoroutine(PreLoadLocalRes());
            else
                ResourceLoadManager.Instance.StartCoroutine(PreLoadAllRes());
        }

        public override void Update()
        {
            base.Update();
        }

        public override void Exit()
        {
            base.Exit();
            UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_CHECKVERSIONVIEW);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_SetRoleScaleValue);
        }

        private IEnumerator PreLoadAllRes() 
        {
            yield return ResourceLoadManager.Instance.StartCoroutine( ResourceLoadManager.Instance.PreLoadAllViews());
           // ConfigManager.Instance.mAppConfig = new AppConfig();
           // ConfigManager.Instance.mAppConfig.LoadLocalConfig();
           
           Main.Instance.StartCoroutine( ConfigManager.Instance.LoadConfig(() =>
            {
                NetWorkManager.Instance.Initialize();
                NetWorkManager.Instance.SetLoginNetWork();
                Main.Instance.ChangeState(LoginState.StateName);
            }));
        }

        private IEnumerator PreLoadLocalRes()
        {
            yield return ResourceLoadManager.Instance.StartCoroutine(ResourceLoadManager.Instance.PreLoadLocalViews());
           // ConfigManager.Instance.mAppConfig = new AppConfig();
           // ConfigManager.Instance.mAppConfig.LoadLocalConfig();
            Main.Instance.StartCoroutine(ConfigManager.Instance.LoadConfig(() =>
            {
                NetWorkManager.Instance.Initialize();
                NetWorkManager.Instance.SetLoginNetWork();
                Main.Instance.ChangeState(LoginState.StateName);
            }));
        }
        public override string GetStateName()
        {
            return "ReadResState";
        }
    }
}

